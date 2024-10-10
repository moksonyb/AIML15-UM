# Requirements

This document outlines the required software, libraries, and dependencies to set up and run the Unity and ML-Agents project successfully.

## System Requirements

System requirements to run Unity and ML-Agents smoothly:
- **Operating System**: Windows 10 or later, macOS 10.13 or later, or a compatible Linux distribution
- **RAM**: 8 GB minimum (16 GB recommended for larger training sessions)
- **Storage**: 10 GB free space for Unity, ML-Agents, and project files
- **Graphics**: Dedicated GPU recommended for faster training

## Software Requirements

Software needed to work on this project includes:

- **Unity Game Engine**
  - Download [Unity Hub](https://unity.com/download) and install a Unity version compatible with ML-Agents.
  
- **Git** for version control
  - [Download Git](https://git-scm.com/downloads) and follow the installation instructions.
  
- **Python (3.10.x)**
  - [Python Download](https://www.python.org/downloads/) and ensure "Add Python to PATH" is selected during installation.

## Python Libraries and Dependencies

Install the following dependencies in a virtual environment to avoid conflicts.

### Setting Up Python Virtual Environment

1. **Create and activate a virtual environment**:
   ```bash
   python -m venv ml-agents-env
   ```
   
   Activate the environment:
   - **Windows**: 
     ```bash
     .\ml-agents-env\Scripts\activate
     ```
   - **Mac/Linux**:
     ```bash
     source ml-agents-env/bin/activate
     ```

2. **Install required libraries**:
   ```bash
   pip install mlagents numpy torch matplotlib
   ```

### Installing Dependencies from requirements.txt

Alternatively, use a `requirements.txt` file to install dependencies in one step.

#### Create `requirements.txt` file

Add the following content to `requirements.txt`:
```plaintext
mlagents
numpy
torch
matplotlib
```

Then, install all dependencies at once:
```bash
pip install -r requirements.txt
```
