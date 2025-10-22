# 🖥️ Kiosk7

A secure, standalone **kiosk-mode browser** built with WebView2.  
Designed for fixed-purpose public displays, terminals, or information points that restrict user access to specific websites.

---

## 🚀 Features
- Full-screen locked browsing experience  
- Configurable **start URL**, **exit PIN**, and **domain allowlist**  
- On-screen keyboard for touch-enabled kiosks  
- Optional visible EXIT button (for testing)  
- **User cannot escape** the kiosk via keyboard shortcuts, Windows key, or Ctrl + Alt + Del  
- Runs without needing to install extra runtimes — all required `.dll` files are included

---

## ⚙️ Configuration

Edit your `settings.cfg` file to define your startup settings:  
https://www.bbc.com  

This example loads the BBC homepage when the kiosk launches.

On first startup, a **setup window** allows you to enter:
- **Start URL**
- **Exit PIN**
- **Allowed domains**
- Optional: show EXIT button or remember settings for next time  

---

## 🧩 How It Works

When launched, **Kiosk7.exe** starts a Chromium-based WebView2 instance in fullscreen mode, blocking all system shortcuts.  
It uses a local configuration file (`settings.cfg`) to remember user preferences between sessions.

If the `Kiosk7.exe.WebView2/` folder is missing, the app will automatically recreate it at runtime — no installation needed.  

---

## 📦 Distribution Notes

To run Kiosk7 on another system, copy the entire folder structure:  

Kiosk7/  
├─ Kiosk7.exe  
├─ settings.cfg  
├─ runtimes/  
│  └─ win-x64/native/WebView2Loader.dll  
├─ D3DCompiler_47_cor3.dll  
├─ vcruntime140_cor3.dll  
├─ PresentationNative_cor3.dll  
├─ PenImc_cor3.dll  
└─ wpfgfx_cor3.dll  

🧠 **Note:**  
- The app automatically generates a `Kiosk7.exe.WebView2` folder during first launch.  
- This folder only stores runtime cache (no sensitive data) and can be ignored or deleted safely.  

---

## 🛠️ Requirements
- Windows 10 or 11 (x64)
- WebView2 Runtime (Evergreen) — usually preinstalled on Windows 10/11

No manual dependencies are required; all `.dll` files are provided for compatibility.

---

## 💡 Development Notes
Built in **C# (.NET 6)** using **WinForms** and **WebView2**.  
Implements fullscreen kiosk lockdown logic, restricted navigation control, and safe local caching.  

---

## 🧑‍💻 Author
Developed by **Christos Paraskevopoulos**  
📧 christos1129@gmail.com  
© 2025 — Part of the *DevOps Automation Portfolio*
