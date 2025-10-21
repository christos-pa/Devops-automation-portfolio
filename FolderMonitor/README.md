# 📂 FolderMonitor

**FolderMonitor** is a lightweight automation tool that continuously watches selected folders and triggers instant alerts or logs whenever files are added, removed, or modified.  
Designed for reliability and simplicity, it runs quietly in the background and can be configured to email notifications when changes occur.

---

## 📘 Overview
FolderMonitor is ideal for IT engineers, DevOps professionals, and administrators who need to:
- Track updates in system directories or shared folders.
- Monitor incoming/outgoing data feeds.
- Receive immediate alerts when files change.
- Keep audit logs for compliance or troubleshooting.

It can run both as a **background service** or interactively for live testing.

---

## ✨ Features
- 📁 **Real-time monitoring** — detects new, edited, or deleted files instantly.  
- 📨 **Email alerts** — optional notifications with full event details.  
- 📄 **Detailed logging** — automatic timestamped log creation.  
- ⚙️ **Customizable JSON config** — choose which folders to watch and how to react.  
- 🪟 **Windows compatible** — runs silently or with tray icon support.  

---

## 🚀 Quick Start

### 1️⃣ Edit your configuration
Open `config.json` and update your folder paths, email recipients, and monitoring options.  
> 💡 The included configuration uses *example* values — replace them before running.

---

### 2️⃣ Run the tool
Run the `.exe` file:  
`folder_monitor.exe`  

The tool will begin watching all defined folders and log or alert whenever changes occur.

---

### 3️⃣ Optional: Run automatically on startup
Use the included batch file:  
`install_folder_monitor_usertray.bat`  

To uninstall:  
`uninstall_folder_monitor.bat`  

---

## 🧩 Folder Structure
| File / Folder | Description |
|----------------|--------------|
| `folder_monitor.exe` | Compiled executable for Windows |
| `folder_monitor.py` | Main application script |
| `config.json` | Example configuration file |
| `icon.ico` | Tray icon asset |
| `install_folder_monitor_usertray.bat` | Adds FolderMonitor to Windows startup |
| `uninstall_folder_monitor.bat` | Removes FolderMonitor from startup |
| `FolderMonitor_Guide.pdf` | Optional user guide (if included) |

---

## 📸 Example Screenshots

<p align="left">
  <img src="https://github.com/user-attachments/assets/88d2dfe4-64e8-4d4e-8306-e4cceb2f1672" width="320" alt="Email Alert Example">
</p>

---

> 🧩 Note: Example configuration only — replace with your own credentials before deployment.

---

## 🧑‍💻 Author
Developed by **Christos Paraskevopoulos**  
📧 [christos1129@gmail.com](mailto:christos1129@gmail.com)

© 2025 — Part of the [DevOps Automation Portfolio](../README.md)
