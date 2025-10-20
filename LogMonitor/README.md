# 🧾 LogMonitor

**LogMonitor** is an automated log-watching tool that continuously scans specified log files for predefined keywords, errors, or warnings.  
When a match is detected, it instantly records the event and can optionally send an email alert — ensuring no critical issues go unnoticed.

---

## 📘 Overview
LogMonitor is ideal for system administrators, DevOps engineers, and IT support teams who need to:
- Detect service errors or failure patterns in real time.  
- Track critical events across multiple log files.  
- Maintain uptime and reliability by catching issues early.  
- Generate audit trails or operational incident reports.

It can run interactively or as a silent background process on Windows.

---

## ✨ Features
- 🧠 **Keyword detection** — scans logs for configurable patterns.  
- ⚙️ **Custom rules** — define keywords, log sources, and severity levels in JSON.  
- 📧 **Email notifications** — send alerts instantly when errors are found.  
- 🪵 **Timestamped logging** — each event includes date, time, and file reference.  
- 🪟 **Windows startup ready** — includes batch installer for automatic startup.  

---

## 🚀 Quick Start

### 1️⃣ Edit your configuration
Open `config.json` and update your monitored log paths, alert keywords, and notification settings.  
> 💡 The included configuration uses *example* values — replace them before running.

---

### 2️⃣ Run the tool
Run the `.exe` file:  
log_monitor.exe  

The tool will immediately start monitoring the defined log files and alert you when it detects your specified keywords.

---

### 3️⃣ Optional: Run automatically on startup
Use the included batch file:  
install_log_monitor_usertray.bat  

To uninstall:  
uninstall_log_monitor.bat  

---

## 🧩 Folder Structure
| File / Folder | Description |
|----------------|--------------|
| `log_monitor.exe` | Compiled executable for Windows |
| `log_monitor.py` | Main application script |
| `config.json` | Example configuration file |
| `icon.ico` | Tray icon asset |
| `install_log_monitor_usertray.bat` | Adds LogMonitor to Windows startup |
| `uninstall_log_monitor.bat` | Removes LogMonitor from startup |
| `LogMonitor_Guide.pdf` | Optional user guide (if included) |

---

## 📸 Example Screenshots
<p align="left">
  <img src="https://github.com/user-attachments/assets/YOUR-LOGMONITOR-IMAGE-LINK-HERE" width="320" alt="LogMonitor Example"><br>
  <em>Error Detection Example</em>
</p>

---

> 🧩 Note: Example configuration only — replace with your own credentials before deployment.

---

## 🧑‍💻 Author
Developed by **Christos Paraskevopoulos**  
📧 [christos1129@gmail.com](mailto:christos1129@gmail.com)

© 2025 — Part of the [DevOps Automation Portfolio](../README.md)
