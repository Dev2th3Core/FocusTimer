# Contributing to Focus Timer

First off, thank you for considering contributing to Focus Timer! It's people like you that make open source such a great community. We welcome any form of contribution, from reporting bugs and suggesting features to writing code.

## Code of Conduct

This project and everyone participating in it is governed by the Focus Timer Code of Conduct. By participating, you are expected to uphold this code. Please report unacceptable behavior to dev2th3core@gmail.com.

## How Can I Contribute?

### Reporting Bugs

Bugs are tracked as GitHub Issues. Before creating a bug report, please check the existing issues to see if someone has already reported it.

When creating a bug report, please include as many details as possible:

*   A clear and descriptive title.
*   A step-by-step description of how to reproduce the issue.
*   What you expected to happen and what actually happened.
*   Your operating system and version.
*   Screenshots or GIFs if they help illustrate the problem.

### Suggesting Enhancements

Enhancement suggestions are also tracked as GitHub Issues.

*   Use a clear and descriptive title.
*   Provide a detailed explanation of the enhancement you're suggesting.
*   Explain why this enhancement would be useful to other Focus Timer users.

### Your First Code Contribution

Unsure where to begin? You can start by looking through `good-first-issue` and `help-wanted` issues.

1.  **Fork** the repository.
2.  **Clone** your forked repository: `git clone https://github.com/<your-username>/FocusTimer.git`
3.  **Create a new branch** for your changes: `git checkout -b feature/your-feature-name` or `fix/bug-name`.
4.  **Make your changes** and commit them with a descriptive message.
5.  **Push** your branch to your fork: `git push origin feature/your-feature-name`.
6.  **Open a Pull Request** to the `main` branch of the original repository.

## Development Setup

The repository contains two main projects: the Avalonia desktop application and the Next.js landing page.

### Prerequisites

*   .NET 9 SDK or later.
*   Node.js (LTS version recommended).
*   An IDE like Visual Studio 2022 or JetBrains Rider.

### Running the Desktop App (FocusTimer)

1.  Navigate to the `FocusTimer` directory.
2.  Open `focustimer.sln` in Visual Studio or Rider.
3.  Set `FocusTimer` as the startup project.
4.  Press `F5` or the "Run" button to build and start the application.

Alternatively, you can run it from the command line:

```bash
cd FocusTimer/FocusTimer
dotnet run
```

### Running the Website (Web)

1.  Navigate to the `Web` directory in your terminal.
2.  Install the dependencies:
    ```bash
    cd Web
    npm install
    ```
3.  Start the development server:
    ```bash
    npm run dev
    ```
4.  Open http://localhost:3000 in your browser.

## Pull Request Process

1.  Ensure any install or build dependencies are removed before the end of the layer when doing a build.
2.  Update the README.md with details of changes to the interface, this includes new environment variables, exposed ports, useful file locations, and container parameters.
3.  Provide a clear title and description for your pull request, explaining the "what" and "why" of your changes.
4.  Link your pull request to any relevant issues.

Thank you for your contribution!