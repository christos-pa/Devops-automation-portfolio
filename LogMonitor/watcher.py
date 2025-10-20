# watcher.py — ENGINE (runs headless as SYSTEM)
# - Reads config.json from the same folder as the .py/.exe
# - Expands LogPaths (supports * and **; replaces YYYY-MM-DD with today's date)
# - Tails only newly appended lines; survives truncation/rotation
# - Matches multiple regex Patterns (each with its own CooldownMinutes)
# - Sends email via SMTP (STARTTLS/SSL supported)
# - Clean shutdown via quit.flag (written by TrayHelper)

import os
import sys
import re
import glob
import time
import json
import smtplib
import socket
from datetime import datetime, timedelta
from email.mime.text import MIMEText

# ---------- helpers ----------

def app_dir() -> str:
    """Folder where this script/exe lives."""
    if getattr(sys, "frozen", False):  # PyInstaller exe
        return os.path.dirname(sys.executable)
    return os.path.dirname(os.path.abspath(__file__))

def load_config(path: str) -> dict:
    with open(path, "r", encoding="utf-8") as f:
        cfg = json.load(f)

    # Defaults & light validation
    cfg.setdefault("LogPaths", [])
    cfg.setdefault("Patterns", [])
    cfg.setdefault("PollIntervalSeconds", 2)

    email = cfg.setdefault("Email", {})
    email.setdefault("Server", "smtp.gmail.com")
    email.setdefault("Port", 587)
    email.setdefault("UseStartTLS", True)
    email.setdefault("UseSSL", False)
    email.setdefault("SubjectPrefix", "[LogMonitor]")
    if "From" not in email or "Password" not in email or "To" not in email:
        raise ValueError("Email.From, Email.Password, and Email.To are required in config.json")

    return cfg

def today_str() -> str:
    return datetime.now().strftime("%Y-%m-%d")

def expand_log_paths(path_patterns):
    """
    Replace YYYY-MM-DD with today, expand globs (recursive if ** present),
    and return a sorted list of existing files.
    """
    today = today_str()
    out = set()
    for pat in path_patterns:
        pat = pat.replace("YYYY-MM-DD", today)
        # Normalize to avoid \N escape issues; forward slashes work on Windows
        pat = pat.replace("\\", "/")
        for f in glob.glob(pat, recursive=True):
            if os.path.isfile(f):
                out.add(os.path.normpath(f))
    return sorted(out)

def compile_patterns(pattern_defs):
    compiled = []
    for p in pattern_defs:
        name = p.get("Name") or p.get("Regex", "unnamed")
        regex = p.get("Regex", "")
        cooldown_min = int(p.get("CooldownMinutes", 5))
        try:
            rx = re.compile(regex, re.IGNORECASE)
        except re.error as e:
            raise ValueError(f"Invalid regex for pattern '{name}': {e}")
        compiled.append({
            "Name": name,
            "Regex": rx,
            "Cooldown": timedelta(minutes=cooldown_min)
        })
    return compiled

def tail_new_lines(path: str, offsets: dict):
    """
    Yield newly appended lines since last read; handle file shrink (rotation/truncation).
    We track byte offsets per file.
    """
    prev = offsets.get(path, 0)
    try:
        size_now = os.path.getsize(path)
        with open(path, "rb") as f:
            if size_now < prev:  # rotated/truncated
                prev = 0
            f.seek(prev)
            chunk = f.read()
            offsets[path] = f.tell()
    except OSError:
        return []

    if not chunk:
        return []

    text = chunk.decode("utf-8", errors="replace")
    return text.splitlines()

def send_email(smtp_cfg: dict, subject: str, body: str) -> None:
    msg = MIMEText(body, "plain", "utf-8")
    msg["From"] = smtp_cfg["From"]
    msg["To"] = ", ".join(smtp_cfg["To"])
    msg["Subject"] = subject

    try:
        if smtp_cfg.get("UseSSL", False):
            with smtplib.SMTP_SSL(smtp_cfg["Server"], int(smtp_cfg["Port"])) as s:
                s.login(smtp_cfg["From"], smtp_cfg["Password"])
                s.send_message(msg)
        else:
            with smtplib.SMTP(smtp_cfg["Server"], int(smtp_cfg["Port"])) as s:
                if smtp_cfg.get("UseStartTLS", True):
                    s.starttls()
                s.login(smtp_cfg["From"], smtp_cfg["Password"])
                s.send_message(msg)
        print(f"[EMAIL] Sent: {subject}")
    except Exception as e:
        print(f"[ERROR] Email failed: {e}")

def quit_requested(flag_path: str) -> bool:
    return os.path.exists(flag_path)

# ---------- main loop ----------

def main():
    cfg_path = os.path.join(app_dir(), "config.json")
    cfg = load_config(cfg_path)

    email_cfg = cfg["Email"]
    subject_prefix = email_cfg.get("SubjectPrefix", "[LogMonitor]")
    poll = float(cfg.get("PollIntervalSeconds", 2))
    patterns = compile_patterns(cfg["Patterns"])

    offsets = {}          # per-file read offset
    last_alerts = {}      # per-pattern last alert timestamp
    host = socket.gethostname()
    quit_flag = os.path.join(app_dir(), "quit.flag")

    print(f"[START] Host: {host}")
    print(f"[CONF ] Config: {cfg_path}")
    print(f"[INFO ] Poll interval: {poll}s")
    print(f"[INFO ] Watching {len(cfg['LogPaths'])} path pattern(s):")
    for p in cfg["LogPaths"]:
        print(f"        - {p}")
    print(f"[INFO ] {len(patterns)} pattern(s): {[p['Name'] for p in patterns]}")
    print("")

    while True:
        try:
            # Graceful quit (set by TrayHelper)
            if quit_requested(quit_flag):
                try:
                    os.remove(quit_flag)
                except OSError:
                    pass
                print("[EXIT ] Quit flag detected. Shutting down engine.")
                break

            files = expand_log_paths(cfg["LogPaths"])
            if not files:
                print("[INFO ] No matching files yet; waiting...")
                time.sleep(poll)
                continue

            for file_path in files:
                for line in tail_new_lines(file_path, offsets):
                    for patt in patterns:
                        if patt["Regex"].search(line):
                            now = datetime.now()
                            last = last_alerts.get(patt["Name"], datetime.min)
                            # Send with cooldown per pattern
                            if now >= last + patt["Cooldown"]:
                                subject = f"{subject_prefix} {patt['Name']} on {host}"
                                body = (
                                    f"Time: {now.strftime('%Y-%m-%d %H:%M:%S')}\n"
                                    f"Host: {host}\n"
                                    f"File: {file_path}\n"
                                    f"Pattern: {patt['Name']}\n"
                                    f"Regex: {patt['Regex'].pattern}\n\n"
                                    f"Line:\n{line}\n"
                                )
                                send_email(email_cfg, subject, body)
                                last_alerts[patt["Name"]] = now
                            # Always print for visibility
                            print(f"[ALERT] {patt['Name']} | {file_path}\n  {line}\n")

            time.sleep(poll)

        except KeyboardInterrupt:
            print("\n[EXIT ] Stopped by user.")
            break
        except Exception as e:
            print(f"[ERROR] Loop exception: {e}")
            time.sleep(poll)

if __name__ == "__main__":
    main()
