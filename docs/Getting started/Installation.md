
# Getting Started Guide for Unity and ML-Agents Project

This guide will walk you through setting up a Unity environment with ML-Agents to enable you to work on the "Controlling Agents in the Unity Game Engine" project. The setup includes installing Unity, Python, ML-Agents, and version control with Git.

## Prerequisites

### Software Requirements
- **Git**: Version control tool for managing your repository.
- **Unity Game Engine**: Version compatible with ML-Agents.
- **Visual Studio (optional)**: Primarily for Windows users if you need a C# IDE.
- **Python (3.10.x)**: For ML-Agents and its dependencies.

## Setting up Git and GitHub

### Install Git
Download Git and follow the installation instructions.

### Clone the Repository
```bash
git clone --branch fix-numpy-release-21-branch https://github.com/moksonyb/AIML15-UM
```

## Importing Existing Unity Project

Instead of creating a new project, you will import an existing project from the ML-Agents folder.

1. Open **Unity Hub**.
2. Click on **Add** > **Add project from disk**.
3. Navigate to the **ML-Agents folder**, select the **Project** folder within it, and add it to Unity Hub.
4. Once added, you can open and modify this project.

## Installing Python and Virtual Environment

### Python Installation
Download Python (3.10.x) and install it, making sure to add Python to your PATH.

### Setting Up Virtual Environment
In your terminal, create and activate a virtual environment:
```bash
python -m venv ml-agents-env
```

Activate the virtual environment:
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

## Testing and Modifying Code

Modify the Unity scenes and ML-Agents scripts as needed to create custom behaviors. Document any changes in your GitHub repository for easy collaboration.

## Final Notes

- **Commit Regularly**: Use `git commit` to keep track of changes.
- **Push to GitHub**: Keep your repository updated on GitHub with `git push`.
