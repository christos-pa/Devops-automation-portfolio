# 🧠 NMI-Restarter

**NMI-Restarter** is a lightweight Windows utility designed to **restart, stop, or monitor NMI payment terminals** with a simple and intuitive GUI.  
It allows engineers to quickly refresh one or all connected terminals, log restart activity, and view live system status in a single interface.

---

## 🖥️ Overview

Built for technical and field engineers who need fast recovery or maintenance actions for NMI terminal connectors.

- Detects all terminals under the configured directory path  
- Supports **Restart**, **Start**, **Kill**, and **Refresh** operations  
- Logs all activity to a local file for auditing and diagnostics  
- GUI interface eliminates the need for PowerShell or manual restarts  

The tool is ideal for support environments running **multiple payment connectors** or kiosk systems.

---

## 🧠 Development Notes

NMI-Restarter was developed in **C# (.NET Framework)** using **Windows Forms (WinForms)** for its lightweight GUI.  
The goal was to simplify recovery operations for **NMI payment terminals** by providing engineers with quick visual control over process restarts.  

Key technical highlights:
- Built around `System.Diagnostics.Process` to manage terminal sessions.  
- Implements directory scanning to dynamically list active terminal connectors.  
- Uses asynchronous execution to prevent UI freezing during restart sequences.  
- Writes structured logs (`restart.log`) for every event, improving traceability.  
- Portable `.exe` build for instant use on support laptops without installation.

This design ensures stability in live support environments where quick recovery and minimal downtime are critical.

---

## ✨ Features

- 🧩 **Auto-detect terminals** — scans directories and lists connected terminals  
- ⚡ **One-click Restart / Restart All** — instantly relaunch terminals  
- 🧰 **Live Controls** — buttons for `Start`, `Kill`, and `Refresh`  
- 🪶 **Lightweight** — portable executable, no installation required  
- 📄 **Logging** — generates a detailed `restart.log` file under `/Logs`  
- ⚙️ **Configurable JSON** — `settings.json` defines connector paths and parameters  

---

## 🚀 Quick Start

### 1️⃣ Configure the tool  
Edit your `settings.json` file to match your terminal folder path, for example:  
C:\NMI-CC-Connectors\Terminals

---

### 2️⃣ Run the tool  
Simply run the `.exe` file:  
`NmiRestarterTray.exe`

The interface will display all detected terminals and provide full control over restart actions.

---

## 🧩 GUI Overview

The application provides the following controls:

| Control | Description |
|----------|-------------|
| **Dropdown Menu** | Select which terminal to control |
| **Refresh** | Re-scans for active terminals |
| **Start** | Launches the selected terminal |
| **Kill** | Terminates the selected terminal process |
| **Restart** | Restarts the selected terminal |
| **Restart All** | Restarts all detected terminals simultaneously |
| **Log Window** | Displays timestamped messages and status updates |

All actions are recorded in the `restart.log` file within the `/Logs` directory.

---

## 🪄 Folder Structure

| File / Folder | Description |
|----------------|-------------|
| `NmiRestarterTray.exe` | Compiled Windows executable |
| `settings.json` | Configuration file containing terminal path |
| `Logs/` | Directory where restart.log is automatically created |
| `README.md` | Documentation for the tool |

---

## 📸 Example Screenshot

<p align="left">
  <img src="https://github.com/user-attachments/assets/3a8d38fa-8f4d-46f1-b851-d17cfc1ad077" alt="NMI Restarter Example Screenshot">
</p>

---

## 📐 Design & Implementation

NMI-Restarter was developed in **C# (.NET Framework)** using **Windows Forms (WinForms)** for its lightweight GUI.  
The goal was to simplify recovery operations for **NMI payment terminals** by providing engineers with quick visual control over process restarts.  

**Technical highlights:**
- Built around `System.Diagnostics.Process` for process management  
- Implements directory scanning to dynamically populate terminal lists  
- Uses asynchronous operations to prevent UI blocking during restarts  
- Logs all restart actions to `restart.log` for traceability  
- Packaged as a **standalone executable** (no installation required)

This architecture ensures reliability in live support environments where **uptime and responsiveness** are critical.

---

## 📦 Download
You can download the executable directly from:  
👉 [NMI-Restarter.exe (Google Drive)](https://drive.google.com/uc?export=download&id=11cuuVgRQx6un_SLX6yZDOeA6sWdl9yLu)

---

## 🧑‍💻 Author
Developed by **Christos Paraskevopoulos**  
📧 [christos1129@gmail.com](mailto:christos1129@gmail.com)

© 2025 — Part of the **DevOps Automation Portfolio**
