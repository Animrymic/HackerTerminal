# HackerTerminal

A local cybersecurity challenge app built with Angular and ASP.NET Core. Players can complete randomized missions, earn score and XP, review mission history, and compare leaderboard rankings.

## Project structure

- `HackerTerminal.API` — ASP.NET Core API
- `HackerTerminal.Core` — shared entities
- `HackerTerminal.Infrastructure` — Entity Framework Core database context and migrations
- `hacker-terminal` — Angular frontend

## Local setup

### 1. Configure the API signing key

The JWT signing key is stored locally with .NET User Secrets. It is not included in this repository.

In Visual Studio, right-click **HackerTerminal.API** and select **Manage User Secrets**. Add a new random value:

```json
{
  "Jwt": {
    "Key": "YOUR_NEW_RANDOM_SECRET_KEY"
  }
}
