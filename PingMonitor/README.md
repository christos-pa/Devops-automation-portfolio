# 🖥️ PingMonitor

**PingMonitor** is a lightweight **network availability and uptime checker** designed to continuously monitor multiple hosts, log their responses, and send email alerts when repeated failures occur.  
Built for **Windows environments**, it runs quietly in the background or as a startup service.

---

## 📘 Overview
PingMonitor is ideal for IT admins or DevOps engineers who need to:
- Detect network outages across multiple endpoints.
- Receive instant alerts when critical systems stop responding.
- Maintain uptime logs for troubleshooting and reporting.

It can run both **interactively** or as a **startup tray service**, making it perfect for remote monitoring stations or kiosk setups.

---

## ✨ Features
- 🔁 **Multi-endpoint pinging** — monitor multiple IPs or hostnames concurrently  
- 🕓 **Configurable intervals** — set custom ping frequency and timeout  
- 📊 **Auto-logging** — creates timestamped logs for response results  
- 📧 **Email notifications** — get alerts after a defined number of failed pings  
- ⚙️ **Customizable JSON config** — define targets, SMTP details, and retry behavior  
- 🪟 **Simple Windows deployment** — one-click batch installer for startup integration

---

## 🚀 Quick Start

1️⃣ **Edit your configuration**

Open `config.json` and update:
```json
{
  "PingFrequencyMs": 2000,
  "MaxFailuresBeforeAlert": 30,
  "ToAddresses": ["you@example.com"],
  "SmtpServer": "smtp.example.com",
  "SmtpPort": 587
}
