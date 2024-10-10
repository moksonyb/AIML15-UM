# Installation
Getting Started Guide for Unity and ML-Agents Project
This guide will walk you through setting up a Unity environment with ML-Agents to enable you to work on the "Controlling Agents in the Unity Game Engine" project. The setup includes installing Unity, Python, ML-Agents, and version control with Git.

## Prerequisites

Software Requirements
- Git: Version control tool for managing your repository.
- Unity Game Engine: Version compatible with ML-Agents.
- Visual Studio (optional): Primarily for Windows users if you need a C# IDE.
- Python (3.10.x): For ML-Agents and its dependencies.
## Setting up Git and GitHub

### Install Git
Download Git and follow the installation instructions.
### Clone the Repository
`git clone --branch fix-numpy-release-21-branch https://github.com/moksonyb/AIML15-UM`
### Installing Unity
- Download Unity Hub
- Install Unity Editor
Open Unity Hub, navigate to the "Installs" tab, and install a version compatible with ML-Agents.
- Create a New Project
  
  Open Unity Hub, click on "Projects" and then "New Project."
Select the "3D Core" template, name your project, and set the location.
Click Create Project.
Adding ML-Agents Package
Open your project in Unity.
Go to Window > Package Manager.
Click on the + icon, select Add package from git URL, and enter:
`https://github.com/DennisSoemers/ml-agents/tree/fix-numpy-release-21-branch`
Unity will add the ML-Agents package to your project.
- Installing Python and Virtual Environment

  Python Installation
Download Python (3.10.x)

- Installing ML-Agents and Dependencies

  execeute the followingcommand in terminal:
`pip install mlagents`
- Running ML-Agents in Unity

  Open your Unity project.
In the Unity Console, select Play Mode to ensure everything is working correctly.
To begin training, open a terminal, navigate to your ML-Agents folder, and run:

  `mlagents-learn config/trainer_config.yaml --run-id=<your_run_id>`
Replace <your_run_id> with an identifier for the training session.
- Testing and Modifying Code

  Modify the Unity scenes and ML-Agents scripts as needed to create custom behaviors.
Document any changes in your GitHub repository for easy collaboration.
-  Final Notes

   Commit Regularly: Use git commit to keep track of changes.
   
   Push to GitHub: Keep your repository updated on GitHub with git push.
