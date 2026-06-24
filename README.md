# Kaman Chat AI - Desktop Assistant

A sophisticated Windows Forms desktop application developed in C# that integrates advanced Artificial Intelligence and Cloud APIs. The project features a modern user interface and supports multi-model interactions along with speech synthesis capabilities.

## 🚀 Features
* **Multi-Model Support:** Integration with OpenAI and Azure OpenAI APIs for intelligent chat interactions.
* **Text-to-Speech (TTS):** Implements Google Cloud Text-to-Speech API to convert AI text responses into high-quality spoken audio.
* **Modern UI/UX:** Styled using Bunifu UI WinForms for a responsive, sleek, and dark-themed desktop experience.
* **Console Sandbox:** Includes a companion console application (`ConsoleAppchat`) used for prototyping API connections and testing background logic.

## 🛠️ Tech Stack & Libraries
* **Language:** C# (.NET Framework)
* **UI Framework:** Windows Forms & Bunifu UI
* **AI Integrations:** OpenAI SDK, Azure.AI.OpenAI, Betalgo.OpenAI, Forge.OpenAI
* **Google Cloud APIs:** Google.Cloud.TextToSpeech.V1, Google.Apis.Auth, Grpc.Core
* **Data Handling:** Newtonsoft.Json, System.Text.Json

## 📂 Project Structure
* `Kaman Chat AI/` - The main desktop application containing the UI forms, event handlers, and API integration logic.
* `ConsoleAppchat/` - A lightweight console environment used for testing API payloads and speech generation.