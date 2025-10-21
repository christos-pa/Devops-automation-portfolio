# 🧾 LogMonitor

**LogMonitor** is an automated log-watching and alerting tool designed to run continuously, even after system reboots or user logouts.  
It monitors specified log files for error patterns or keywords and can write events to a log and optionally send email alerts when matches are detected.

---

## 📘 Overview
LogMonitor is built for IT administrators, system engineers, and DevOps operators who need a **reliable 24/7 log monitoring service** that stays active through reboots.  
It runs silently in the background; a small **tray icon (branding/logo only)** is shown when a user session is active.

---

## ✨ Features
- 🪵 **Real-time log monitoring** — watches multiple log files simultaneously  
- 🔍 **Keyword detection** — configure patterns in `config.json` (errors, warnings, custom strings)  
- ♻️ **Persistent startup** — optional installer sets it to start on boot, before user login  
- 📧 **Email alerts (optional)** — SMTP settings in `config.json`  
- 💾 **Event logging** — timestamped entries with file references  
- 🖼️ **Tray icon (branding only)** — shows your logo when a user session is present  
- 🧰 **Batch setup** — one-click install/uninstall with included `.bat` scripts

---

## 🚀 Quick Start

### 1️⃣ Edit your configuration
Open `config.json` and update:
- Paths of log files to monitor  
- Keywords/patterns to detect  
- SMTP/email settings (optional)

> 💡 The included configuration uses *example* values — replace them before deployment.

---

### 2️⃣ Run the tool
Run the main executable:

LogWatcher.exe

The tool begins monitoring all log files defined in `config.json` and logs detections in real time.

---

### 3️⃣ Optional: Run automatically on startup
To install LogMonitor as a background service:

Install_Both.bat

To uninstall cleanly:

Uninstall_Both.bat

🧩 The installer configures the service to start at Windows boot so monitoring continues through reboots and before any user logs in.

---

## 🧩 Folder Structure
| File / Folder | Description |
|----------------|--------------|
| `config.json` | Example configuration (log paths, patterns, SMTP, etc.) |
| `Install_Both.bat` | Installs LogMonitor for startup persistence |
| `Install_Both.log` | Installation log output |
| `Uninstall_Both.bat` | Removes startup entries/services |
| `LogWatcher.exe` | Main log monitoring engine |
| `watcher.py` | Source code for the monitoring engine |
| `logwatcher.ico` | Tray/logo icon resource (branding only) |
| `LogWatcher_ReadMe.pdf` | PDF documentation (optional) |
| `Terminals/` | Optional folder for outputs, backups, or reports |

---

## 🧠 Operation Flow
1. LogWatcher starts (manually or on boot) and loads `config.json`.  
2. It tails each configured log file and matches lines against your patterns.  
3. On a match, it writes a timestamped event and (optionally) sends an email.  
4. When a user session is active, a simple tray **logo** may be visible; it’s branding only (no status colors, no controls).

---

## 📸 Example Screenshot
<p align="left">
  <img src="https://github.com/user-attachments/assets/88d2dfe4-64e8-4d4e-8306-e4cceb2f1672" width="320" alt="LogMonitor Screenshot"><br>
  <em>Example alert captured from monitored logs</em>
</p>

---

> 🧩 Note: Example configuration only — replace with your own paths and credentials before deployment.

---

## 🧑‍💻 Author
Developed by **Christos Paraskevopoulos**  
📧 [christos1129@gmail.com](mailto:christos1129@gmail.com)

© 2025 — Part of the [DevOps Automation Portfolio](../README.md)
