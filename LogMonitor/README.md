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
