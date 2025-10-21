# 🔁 NMI Restarter

**NMI Restarter** is a lightweight automated recovery utility designed to monitor and restart services or processes that become unresponsive.  
It ensures continuous uptime for critical systems such as kiosks, terminals, and backend servers by automatically relaunching applications or triggering reboots when required.

---

## 📘 Overview
NMI Restarter was built for **unattended systems** — where manual intervention isn’t always possible.  
It continuously checks defined Windows processes or services and automatically restarts them if they stop responding, crash, or close unexpectedly.  
This tool is especially useful for **remote environments**, **car park systems**, or **public kiosks** that must remain online 24/7.

---

## ✨ Features
- 🔄 **Automatic restart** — restarts unresponsive or stopped processes instantly  
- 🧠 **Configurable targets** — specify which applications or services to monitor  
- 🕓 **Timed checks** — set custom intervals for health checks  
- 🪟 **Runs silently** — minimal footprint with optional tray logo  
- ♻️ **Persistent startup** — can be installed to run on Windows boot  
- 💾 **Activity logging** — records all restarts with timestamps for auditing  
- 🧰 **Batch setup** — includes installer and uninstaller scripts for quick deployment  

---

## 🚀 Quick Start

### 1️⃣ Edit your configuration
Open `config.json` and update:
- Names or executable paths of services/processes to monitor  
- Interval time (in seconds) between each health check  
- Optional restart or reboot actions  

> 💡 The provided configuration contains example values — adjust them before use.

---

### 2️⃣ Run the tool
Run the executable:

'NMI_Restarter.exe'

The application begins monitoring the specified processes and automatically restarts any that stop or hang.

---

### 3️⃣ Optional: Enable automatic startup
To set NMI Restarter to run automatically at system boot:

'Install_NMI.bat'

To remove it later:

'Uninstall_NMI.bat'

🧩 The installer ensures the utility launches at startup and operates silently in the background.

---

## 🧩 Folder Structure
| File / Folder | Description |
|----------------|--------------|
| `config.json` | Example configuration (process names, intervals, actions) |
| `NMI_Restarter.exe` | Main monitoring and restart executable |
| `Install_NMI.bat` | Adds NMI Restarter to Windows startup |
| `Uninstall_NMI.bat` | Removes NMI Restarter from startup |
| `NMI_Restarter.ico` | Tray/logo icon (branding only) |
| `NMI_Restarter_ReadMe.pdf` | Optional documentation |
| `logs/` | Folder for storing restart logs and activity reports |

---

## 🧠 Operation Flow
1. The tool loads the configuration and begins periodic checks of defined targets.  
2. If a monitored process stops, hangs, or becomes unresponsive:  
   - The process is terminated (if needed).  
   - It is relaunched automatically.  
3. Each restart event is logged with a timestamp for diagnostics.  
4. On reboot, NMI Restarter starts automatically and resumes operation — no user login required.

---

## 📸 Example Screenshot
<p align="left">
  <img src="https://github.com/user-attachments/assets/YOUR-NMI-IMAGE-LINK-HERE" width="320" alt="NMI Restarter Screenshot"><br>
  <em>Example showing automatic restart after detected process failure</em>
</p>

---

> 🧩 Note: Example configuration only — replace with your own monitored services and settings before deployment.

---

## 🧑‍💻 Author
Developed by **Christos Paraskevopoulos**  
📧 [christos1129@gmail.com](mailto:christos1129@gmail.com)

© 2025 — Part of the [DevOps Automation Portfolio](../README.md)
