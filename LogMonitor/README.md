# 🧾 LogMonitor

**LogMonitor** is an advanced automated log-watching and alerting tool designed to run continuously, even after system reboots or user logouts.  
It monitors specific log files for error patterns or keywords and triggers notifications, file backups, or email alerts whenever an issue is detected.

---

## 📘 Overview
LogMonitor is built for IT administrators, system engineers, and DevOps operators who need a **reliable 24/7 log monitoring service** that remains active regardless of user login state.

The system operates with **two integrated modules**:
- 🧠 **LogWatcher** — the core monitoring engine that scans defined log files, tracks patterns, timestamps events, and sends notifications or emails when alerts are triggered.
- 🪟 **TrayHelper** — a lightweight companion app that runs in the Windows tray, provides user visibility, and ensures the monitoring service launches automatically at startup — even before login.

This combination makes LogMonitor ideal for **servers, kiosks, or production systems** that require non-stop event tracking.

---

## ✨ Features
- 🪵 **Real-time log monitoring** — watches multiple log files simultaneously.  
- 🔍 **Keyword detection** — define alert triggers in `config.json` for errors, warnings, or custom strings.  
- ⚙️ **Dual-component architecture** — ensures LogWatcher runs silently while TrayHelper handles user notifications.  
- ♻️ **Persistent startup** — automatically resumes operation after reboot, before login.  
- 📧 **Email alerts** — configurable SMTP options to notify teams instantly.  
- 💾 **Event logging** — creates detailed timestamped logs for each detection.  
- 🖥️ **Tray icon support** — lets the user pause, resume, or check status via the Windows tray.  
- 🧰 **Batch setup** — one-click installation or removal with preconfigured `.bat` scripts.

---

## 🚀 Quick Start

### 1️⃣ Edit your configuration
Open `config.json` and update:
- Paths of log files you want to monitor  
- Keywords or patterns to detect  
- SMTP/email alert settings (optional)

> 💡 The included configuration uses *example* values — replace them before deployment.

---

### 2️⃣ Run the tool
Run the main executable:

LogWatcher.exe

The tool will begin monitoring all log files defined in your config.json and log the results in real time.

---

### 3️⃣ Optional: Run automatically on startup
To install LogMonitor as a background service with tray access:

Install_Both.bat

To uninstall cleanly:

Uninstall_Both.bat

🧩 The installer sets up both components (LogWatcher + TrayHelper) so they:
- Run automatically at Windows startup  
- Start *before* login if configured  
- Keep the tray icon active for manual control

---

## 🧩 Folder Structure
| File / Folder | Description |
|----------------|--------------|
| `config.json` | Example configuration file (keywords, log paths, SMTP, etc.) |
| `Install_Both.bat` | Installs both LogWatcher and TrayHelper for startup persistence |
| `Install_Both.log` | Installation log output |
| `Uninstall_Both.bat` | Removes startup entries and related services |
| `LogWatcher.exe` | Main log monitoring engine |
| `TrayHelper.exe` | Tray icon companion for visibility and control |
| `watcher.py` | Source code for the monitoring engine |
| `TrayHelper.py` | Source code for the tray interface |
| `logwatcher.ico` | Tray icon resource |
| `LogWatcher_ReadMe.pdf` | PDF documentation and usage instructions |
| `Terminals/` | Folder for storing monitored logs, backups, or output reports |

---

## 🧠 Operation Flow
1. **LogWatcher** continuously scans all target files listed in the config.  
2. When a keyword or error is detected:
   - It logs the event to a text file.  
   - Sends an email notification (if enabled).  
3. **TrayHelper** keeps the user informed through a Windows tray icon:
   - Running → 🟢 active  
   - Paused → 🟡 monitoring disabled  
   - Error → 🔴 issue detected  
4. After reboot, the startup batch reinitializes both components automatically — no manual launch needed.

---

## 📸 Example Screenshot
<p align="left">
  <img src="https://github.com/user-attachments/assets/88d2dfe4-64e8-4d4e-8306-e4cceb2f1672" width="320" alt="LogMonitor Screenshot"><br>
  <em>TrayHelper alert showing detected error in monitored log file</em>
</p>

---

> 🧩 Note: Example configuration only — replace with your own paths and credentials before deployment.

---

## 🧑‍💻 Author
Developed by **Christos Paraskevopoulos**  
📧 [christos1129@gmail.com](mailto:christos1129@gmail.com)

© 2025 — Part of the [DevOps Automation Portfolio](../README.md)
