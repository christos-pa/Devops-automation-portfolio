# TrayHelper.py — simple tray with Exit that stops the engine task via schtasks
import sys, subprocess
from pathlib import Path
import pystray
from PIL import Image, ImageDraw

TASK_NAME = "LogWatcher"   # Scheduled Task name for the engine

def app_dir() -> Path:
    # Where this script/exe lives (works with PyInstaller onefile)
    base = getattr(sys, "_MEIPASS", None)
    return Path(base) if base else Path(__file__).resolve().parent

def icon_path() -> Path:
    # Use icon from the same folder as the EXE (do NOT bundle)
    return app_dir() / "logwatcher.ico"

def load_icon():
    p = icon_path()
    if p.exists():
        try:
            return Image.open(str(p))
        except Exception:
            pass
    # Fallback: blue dot if ico isn’t there
    img = Image.new("RGBA", (64, 64), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)
    d.ellipse((8, 8, 56, 56), fill=(60, 130, 255, 255))
    return img

def on_exit(icon, item):
    # Best-effort stop of the SYSTEM task; tray then exits
    try:
        subprocess.run(f'schtasks /End /TN "{TASK_NAME}"',
                       shell=True, capture_output=True, text=True, timeout=5)
    except Exception:
        pass
    icon.stop()

def main():
    tray = pystray.Icon(
        name="LogWatcherTray",
        icon=load_icon(),
        title="LogWatcher",
        menu=pystray.Menu(pystray.MenuItem("Exit (stop engine)", on_exit))
    )
    tray.run()

if __name__ == "__main__":
    main()
