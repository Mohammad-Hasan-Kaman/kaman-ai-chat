# 💬 Kaman Chat AI — Desktop Assistant

> A lightweight C# desktop chat client for AI assistants (DeepSeek via the OpenModel API)
> with native Windows text-to-speech. Built on **standard Windows Forms** — no third-party
> UI suite, no side-by-side DLLs, one `.exe`.

[![C#](https://img.shields.io/badge/C%23-.NET%20Framework%204.8-blue.svg?logo=c-sharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Windows](https://img.shields.io/badge/Windows-10%20%7C%2011-0078D4.svg?logo=windows&logoColor=white)](https://learn.microsoft.com/en-us/windows/win32/winprog/windows-programming-basics)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Release](https://img.shields.io/github/v/release/Mohammad-Hasan-Kaman/kaman-ai-chat?color=blue)](https://github.com/Mohammad-Hasan-Kaman/kaman-ai-chat/releases)

---

## 🚀 Purpose & Target Audience

A small, dependency-free desktop client for talking to an LLM and hearing the answer.

| Audience | Usage method |
|----------|---------------|
| **End users** | Download `kaman-ai-chat.exe` from [Releases](https://github.com/Mohammad-Hasan-Kaman/kaman-ai-chat/releases), paste your API key into the first-run dialog, start chatting. |
| **Developers** | Clone the repo, open `chatai.sln` in Visual Studio, set `KAMAN_AI_CHAT_API_KEY` (or let the app prompt you), press F5. |

---

## ✨ Key Features

- 🤖 **AI chat over the OpenModel API**
  - Calls `https://api.openmodel.ai/v1/messages` with the `deepseek-v4-flash` model.
  - Authenticated with the `x-api-key` header.
  - Asynchronous request/response handling — the window never freezes.
- 🗣️ **Native text-to-speech**
  - Uses `System.Speech.Synthesis.SpeechSynthesizer`, so replies are spoken with the voices
    already installed on Windows. No extra API key, no per-request cost.
  - Speech is optional: if no voice is installed the app still works, silently.
- 🔑 **The API key is never baked into the app**
  - Resolved at runtime from an environment variable, an `app.config` entry, or a per-user
    settings file — and entered through a first-run dialog.
  - Nothing is stored in the source tree or in the shipped executable.
- 💬 **Chat bubbles with a streaming typewriter effect**
  - Replies are rendered one bubble at a time, typed out progressively.
- 🔔 **Send / receive sounds**
  - Two short WAV cues are embedded in the executable and played on send and receive.
- 🖼️ **Standard WinForms UI**
  - Built entirely from in-box Windows Forms controls, so the release is a single
    dependency-free executable.

---

## 📥 Installation & Setup

### 1. For end users (compiled executable)

> **Prerequisite:** an API key from [OpenModel.ai](https://openmodel.ai).

1. Download the latest `kaman-ai-chat.exe` from the [Releases](https://github.com/Mohammad-Hasan-Kaman/kaman-ai-chat/releases) page.
2. Run it. On first launch a dialog asks for your API key.
3. Tick **Remember my key** to store it in `%APPDATA%\kaman-ai-chat\settings.txt`, or leave
   it unticked to supply the key through the environment variable on every launch.
4. Type a message and press **Enter** (or click the send button).

**Prefer not to use the dialog?** Set the key before launching:

```bash
set KAMAN_AI_CHAT_API_KEY=your-key-here
kaman-ai-chat.exe
```

```powershell
$env:KAMAN_AI_CHAT_API_KEY = "your-key-here"
.\kaman-ai-chat.exe
```

### 2. For developers (source code)

```bash
git clone https://github.com/Mohammad-Hasan-Kaman/kaman-ai-chat.git
cd kaman-ai-chat
```

Open `chatai.sln` in Visual Studio (2022 or newer) with the **.NET desktop development**
workload. There are **no NuGet packages to restore** — every referenced assembly ships with
the .NET Framework.

Set the key either through the environment variable or by letting the first-run dialog
prompt for it, then press **F5**.

### 3. Where the key is looked up

The first source that yields a usable value wins:

| Order | Source | Example |
|-------|--------|---------|
| 1 | Environment variable | `KAMAN_AI_CHAT_API_KEY` |
| 2 | `app.config` → `appSettings` | `<add key="OpenModelApiKey" value="..." />` |
| 3 | Per-user settings file | `%APPDATA%\kaman-ai-chat\settings.txt` |

Placeholder values such as `YOUR_API_KEY_HERE` are rejected, so a stale placeholder can
never be sent to the API by accident.

---

## 🛠 Tech Stack

| Technology | Role |
|------------|------|
| **C# (.NET Framework 4.8)** | Core language and runtime |
| **Windows Forms** | Desktop UI — standard in-box controls only |
| **HttpClient** | REST calls to the OpenModel API |
| **System.Web.Extensions / JavaScriptSerializer** | JSON parsing (in-box, no extra DLL) |
| **System.Speech.Synthesis** | Native Windows text-to-speech |
| **System.Media.SoundPlayer** | Embedded WAV send/receive cues |
| **OpenModel.ai API** | AI model (`deepseek-v4-flash`) |

---

## 📝 How to use

1. **Launch** the app and complete the first-run key prompt.
2. **Type a message** in the input box, e.g. `Hello` or `Tell me a short joke`.
3. **Send** it with **Enter** or the send button — a send cue plays.
4. **Read and hear** the reply: the answer is typed into a bubble on the right and spoken
   aloud by the Windows voice engine; a receive cue plays as it arrives.

---

## 📂 Project Structure

```
kaman-ai-chat/
├── chatai/
│   ├── Form1.cs              # UI logic, API access, reply parsing, speech
│   ├── Form1.Designer.cs     # Standard WinForms layout
│   ├── Form1.resx             # Button artwork and embedded resources
│   ├── ApiKeyForm.cs          # First-run API key dialog
│   ├── Settings.cs            # Runtime API key resolution and storage
│   ├── Program.cs             # Entry point and first-run prompt
│   ├── App.config             # Optional appSettings key entry
│   ├── app.ico                # Application icon
│   ├── SOUND1.wav             # Send cue (embedded)
│   ├── SOUND2.wav             # Receive cue (embedded)
│   ├── Properties/            # AssemblyInfo
│   └── chatai.csproj          # .NET Framework 4.8 project
├── LICENSE                   # MIT License
├── README.md                 # This file
└── chatai.sln                # Visual Studio solution
```

---

## 🧪 Building from source

```bash
msbuild chatai/chatai.csproj -p:Configuration=Release
```

The output is `chatai/bin/Release/kaman-ai-chat.exe`.

---

## 🤝 Contributing

Found a bug or have a suggestion? Open an [Issue](https://github.com/Mohammad-Hasan-Kaman/kaman-ai-chat/issues).
Contributions are welcome.

---

## ⭐ Support

If you find this tool useful, please give it a **star**! ⭐
Your support motivates development.

[![Stars](https://img.shields.io/github/stars/Mohammad-Hasan-Kaman/kaman-ai-chat?style=for-the-badge&logo=github&color=blue)](https://github.com/Mohammad-Hasan-Kaman/kaman-ai-chat/stargazers)

---

*Maintained by Mohammad Hasan Kaman · Last updated: October 2026*

> **Disclaimer:** for educational and personal use. API costs are the responsibility of the user.
