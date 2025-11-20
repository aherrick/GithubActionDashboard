# GitHub Action Dashboard

A simple, efficient Blazor application for monitoring and managing your GitHub Actions workflows across multiple repositories.

## Features

- **Dashboard View**: Monitor the latest status of workflows for your favorite repositories in one place.
- **Manual Triggers**: Trigger workflow runs directly from the dashboard.
- **Run History**: View a consolidated list of recent workflow runs across all your tracked repositories.
- **Efficient API Usage**: Designed to minimize GitHub API calls with manual refresh controls and smart caching to avoid rate limits.
- **Local Storage**: Remembers your favorite repositories and settings.

## Getting Started

### Prerequisites

- .NET 10.0 SDK or later
- A GitHub Personal Access Token (PAT) with `repo` and `workflow` scopes.

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/GithubActionDashboard.git
   ```
2. Navigate to the project directory:
   ```bash
   cd GithubActionDashboard
   ```
3. Run the application:
   ```bash
   dotnet run --project GithubActionDashboard
   ```

### Usage

1. Launch the app in your browser.
2. Enter your GitHub Personal Access Token (PAT) to connect.
3. Click "Select Repositories" to choose which repositories you want to monitor.
4. Use the **Dashboard** tab to see current status and trigger new runs.
5. Use the **All Runs** tab to see a history of recent activity.
6. Click the **Refresh All** button to update the data manually.

## Technologies

- Blazor WebAssembly / Server (depending on configuration)
- [Octokit.net](https://github.com/octokit/octokit.net) for GitHub API integration
- Bootstrap for UI styling
