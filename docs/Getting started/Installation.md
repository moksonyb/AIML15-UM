
# Getting Started Guide for Unity and ML-Agents Project

This guide will walk you through setting up a Unity environment with ML-Agents to enable you to work on the "Controlling Agents in the Unity Game Engine" project. This setup includes installing Unity, Python, ML-Agents, and Git.

---

## Prerequisites

### Software Requirements
- **Git**: For version control.
- **Unity Game Engine**: Compatible with ML-Agents.
- **Visual Studio** (optional): Recommended for C# development.
- **Python (3.10.x)**: Required for ML-Agents.

---

## 1. Setting up Git and GitHub

1. **Install Git**: [Download Git](https://git-scm.com/downloads) and follow the installation instructions.
2. **Create a GitHub Account**: Sign up at [GitHub](https://github.com/) if you don’t already have one.
3. **Clone the Repository**:
   ```bash
   git clone https://github.com/moksonyb/AIML15-UM
   ```

   If you’re using the recommended branch for ML-Agents, clone it as follows:
   ```bash
   git clone --branch fix-numpy-release-21-branch https://github.com/DennisSoemers/ml-agents.git
   ```

---

## 2. Importing the Unity Project

1. **Open Unity Hub**.
2. Click on **Projects** and then **Open Project**.
3. Navigate to the **ML-Agents** folder where your project files are located, select the project, and click **Open**. Unity will import the existing project.
  
### Adding ML-Agents Package

1. With the project open in Unity, go to **Window > Package Manager**.
2. Click the **+** icon, select **Add package from git URL**, and enter:
   ```plaintext
   https://github.com/Unity-Technologies/ml-agents.git
   ```
3. Unity will add the ML-Agents package to your project automatically.

---

## 3. Installing Python and Setting Up a Virtual Environment

### Python Installation

1. **Download Python (3.10.x)**: [Python Download](https://www.python.org/downloads/).
2. Follow the installation instructions, ensuring **Add Python to PATH** is checked.

### Setting up a Virtual Environment

1. Open a terminal (or command prompt).
2. **Create a Virtual Environment**:
   ```bash
   python -m venv ml-agents-env
   ```
3. **Activate the Virtual Environment**:
   - **Windows**:
     ```bash
     .\ml-agents-env\Scripts\activate
     ```
   - **Mac/Linux**:
     ```bash
     source ml-agents-env/bin/activate
     ```

### Installing ML-Agents and Dependencies

With the virtual environment activated, install ML-Agents:
```bash
pip install mlagents
```

For the specific branch:
```bash
cd ml-agents
pip install -e .
```

---

## 4. Running ML-Agents in Unity

1. Open your Unity project.
2. In the **Unity Console**, enter **Play Mode** to confirm everything is working.
3. To start training, open a terminal, navigate to your ML-Agents folder, and run:
   ```bash
   mlagents-learn config/trainer_config.yaml --run-id=<your_run_id>
   ```
   Replace `<your_run_id>` with an identifier for the training session.

---

## 5. Testing and Modifying Code

- Modify Unity scenes and ML-Agents scripts to create custom behaviors.
- Document changes in your GitHub repository for easy collaboration.

---

## 6. Final Notes

- **Commit Regularly**: Use `git commit` to keep track of changes.
- **Push to GitHub**: Use `git push` to keep your repository updated.

---

This guide should now display correctly when you paste it into GitHub. Each code block is distinctly separated, and instructions should render as intended without blending code and text.

