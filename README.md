# 🧠 DevOps Automation Portfolio

Collection of automation and monitoring tools developed by **Christos Paraskevopoulos**,  
Software Developer at **Scheidt & Bachmann UK**.  

Developed as part of my role at Scheidt & Bachmann UK to support operational monitoring and automation across remote parking and payment systems.

---

## 🚀 Projects Overview

### [PingMonitor](PingMonitor/)
A lightweight **network availability monitoring service** that continuously pings multiple endpoints and logs uptime results.  
Features include:
- Continuous background monitoring (runs even before login)
- Tray icon indicator for connection status
- Automatic logging and optional email notifications  
🧩 *Used to monitor remote parking terminals and ensure system uptime.*

---

### [FolderMonitor](FolderMonitor/)
An **automated file and folder watcher** that tracks changes, backups, and sends email alerts.  
Ideal for monitoring transaction file drops or system exports.  
Features include:
- Real-time directory monitoring
- Configurable backup paths
- Email alerts on new or modified files  
🧩 *Deployed to track backend system logs and data folder updates.*

---

### [LogMonitor](LogMonitor/)
A **24/7 log monitoring service** for tracking key events, errors, or keywords in system log files.  
Includes a **tray helper app** for visibility and startup control.  
Features include:
- Configurable file paths and alert patterns
- Persistent Windows background service
- Email or on-screen notifications  
🧩 *Built to detect recurring errors and service disruptions automatically.*

---

### [Kiosk](Kiosk/)
A secure, standalone **Windows kiosk browser** built with C# (.NET) and WebView2.  
Locks the system into a full-screen view for single-site access.  
Features include:
- Domain allowlist and exit PIN protection
- On-screen keyboard for touch kiosks
- Portable and installation-free execution  
🧩 *Used to deploy safe public terminals for system access.*

---

### [NMI-Restarter](NMI-Restarter/)
An **automated service restarter** that monitors the NMI component in Entervo systems and restarts it on failure.  
Features include:
- Scheduled heartbeat checks
- Logging and restart tracking
- Minimal resource usage  
🧩 *Developed to ensure continuous operation of key parking software modules.*

---

## 🧰 Technologies Used
- Python 3 (Automation & Monitoring Tools)
- C# / .NET (Windows Kiosk)
- PowerShell & Batch scripting
- Windows Services & Task Scheduler
- Email Automation (SMTP)
- JSON Configurations

---

## 📬 Contact
📧 **christos1129@gmail.com**  
🔗 [LinkedIn](https://www.linkedin.com/in/christosparaskevopoulos)

---

> _All tools were independently created for internal use at **Scheidt & Bachmann UK** as part of ongoing system reliability and DevOps support efforts._
