# PingMonitor

Lightweight **network availability checker** that pings multiple endpoints on an interval, logs results, and optionally sends email alerts after repeated failures.

## ✨ Features
- Ping multiple hosts on a schedule  
- Track success/failure and (optionally) response times  
- Email alert after N consecutive failures  
- Simple Windows startup via batch script

## 🚀 Quick Start
1) Copy and edit the config:
```bat
copy config.json config.local.json
