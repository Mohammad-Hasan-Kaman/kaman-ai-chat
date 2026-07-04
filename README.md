# 💬 Kaman Chat AI - Desktop Assistant

> A sophisticated C# desktop application integrating advanced AI models (DeepSeek via OpenModel) with Google Cloud Text-to-Speech for real-time conversational interactions. Built with Bunifu UI for a modern, dark-themed user experience.

[![C#](https://img.shields.io/badge/C%23-.NET%20Framework-blue.svg?logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Release](https://img.shields.io/github/v/release/Mohammad-Hasan-Kaman/kaman-ai-chat?color=blue)](https://github.com/Mohammad-Hasan-Kaman/kaman-ai-chat/releases)

---

## 🚀 Purpose & Target Audience

This project is designed for **"AI-powered conversational assistance with speech synthesis"**:

| Audience | Usage Method |
|----------|--------------|
| **End Users** | Run the compiled `.exe` file (requires API key configuration). |
| **Developers** | Clone, restore NuGet packages, and run in Visual Studio for customization. |

---

## ✨ Key Features

- 🤖 **Advanced AI Integration:**
  - Connects to **OpenModel.ai** API supporting **DeepSeek** models (e.g., `deepseek-v4-flash`).
  - Configurable system prompts for **Persian (Farsi)** or multi-language responses.
- 🗣️ **High-Quality Text-to-Speech (TTS):**
  - **Google Cloud Text-to-Speech API:** Natural-sounding voice synthesis for AI responses.
  - **Fallback:** Native `System.Speech.Synthesis` (Windows Built-in voices) if Google API is unavailable.
- 🎨 **Modern UI/UX:**
  - **Bunifu UI Framework:** Sleek, dark-themed interface with smooth animations and custom controls.
  - Responsive chat bubbles and real-time response rendering.
- 🛠️ **Developer Friendly:**
  - Modular design for easy addition of new AI providers or voice engines.
  - Async/await pattern for non-blocking API calls.

---

## 📥 Installation & Setup

### 1. For End Users (Compiled Executable)

> **Prerequisite:** You must obtain an API key from [OpenModel.ai](https://openmodel.ai) and configure your Google Cloud credentials for TTS.

1.  Download the latest release (`ChatAI.exe`) from the [Releases](https://github.com/Mohammad-Hasan-Kaman/kaman-ai-chat/releases) page.
2.  Run the executable.
3.  **Configuration:**
    - Open `Form1.cs` in the source code (if you have it).
    - Locate the `apiKey` variable in the `GetDeepSeekResponse` method.
    - Replace `"YOUR_API_KEY_HERE"` with your actual OpenModel/DeepSeek API key.
    - *(Optional)* Set up Google Cloud Application Default Credentials for TTS.
4.  Start chatting!

### 2. For Developers (Source Code)

```bash
# Clone the repository
git clone https://github.com/Mohammad-Hasan-Kaman/kaman-ai-chat.git
cd chatai

# Open in Visual Studio
# Open "chatai.sln"
# Right-click Solution -> "Restore NuGet Packages"
# Update API keys in Form1.cs
# Press F5 to run
```

---

## 🛠 Tech Stack & Libraries

| Technology | Role |
|------------|------|
| **C# (.NET Framework)** | Core programming language |
| **Windows Forms** | Desktop UI Framework |
| **Bunifu UI WinForms** | Modern UI Components (Dark Mode, Animations) |
| **HttpClient** | REST API Communication |
| **Newtonsoft.Json** | JSON Parsing |
| **Google.Cloud.TextToSpeech.V1** | Google Cloud TTS Integration |
| **System.Speech.Synthesis** | Native Windows TTS Fallback |
| **OpenModel.ai API** | AI Language Model (DeepSeek) |

---

## 📝 How to Use

1.  **Launch the App:** Run the executable.
2.  **Type a Message:** Enter your query in the text box (e.g., "Hello", " Tell me a joke").
3.  **Get AI Response:** The app sends the request to the AI model and displays the text.
4.  **Voice Output:** The app automatically converts the text response to speech using Google Cloud TTS (or native voices).

---

## 📂 Project Structure

```
chatai/
├── Kaman Chat AI/           # Main Desktop Application Source
│   ├── Form1.cs             # UI Logic & API Integration
│   ├── Program.cs           # Entry Point
│   └── ...
├── Bunifu.UI.Form/          # Bunifu Library Source (Included)
├── LICENSE                  # MIT License
├── README.md                # This file
└── chatai.sln               # Visual Studio Solution
```

---

## 🤝 Contributing

Found a bug or have a suggestion? Please open an [Issue](https://github.com/Mohammad-Hasan-Kaman/kaman-ai-chat/issues).
Contributions are welcome!

---

## ⭐ Support

If you find this tool useful, please give it a **star**! ⭐  
Your support motivates development.

[![Star History](https://api.star-history.com/svg?repos=Mohammad-Hasan-Kaman/kaman-ai-chat&type=Date)](https://star-history.com/#Mohammad-Hasan-Kaman/kaman-ai-chat&Date)

---
*Maintained by Mohammad Hasan Kaman | Last updated: July 2026*

> **Disclaimer:** This tool is for educational and personal use. API costs (OpenModel/Google Cloud) are the responsibility of the user.
