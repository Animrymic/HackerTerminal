# HackerTerminal

HackerTerminal is a local cybersecurity challenge application built with Angular, ASP.NET Core, and Entity Framework Core. Players complete timed, randomized missions, earn score and XP, review their history, unlock achievements, and compare rankings.

## Features

- Account registration and JWT-based login
- Randomized missions in nine categories: Password, Cipher, Log Analysis, Base64, URL Analysis, Phishing, SQL Injection, Network Analysis, and Hash Analysis
- Easy, Medium, and Hard mission difficulties
- Mission search, category and difficulty filters, and random mission selection
- Timed sessions with difficulty-based scoring and a penalty for incorrect answers
- Player profile with XP, score, operator level, mission history, and achievements
- Leaderboard for all-time, weekly, and monthly rankings
- Personal rank and nearby competitors

## Project structure

- `HackerTerminal.API` — ASP.NET Core REST API, authentication, and mission endpoints
- `HackerTerminal.Core` — shared application entities
- `HackerTerminal.Infrastructure` — Entity Framework Core database context and migrations
- `hacker-terminal` — Angular frontend

## Requirements

- .NET 10 SDK
- Node.js and npm
- SQL Server LocalDB
- Visual Studio with the Package Manager Console, or the .NET Entity Framework tools

The API's current local database configuration uses the LocalDB instance `(localdb)\ProjectModels` and database `HackerTerminalDb`. If your SQL Server LocalDB setup differs, update the `HackerTerminalConnection` connection string in your local API configuration.

## Local setup

### 1. Configure the JWT signing key

The JWT signing key belongs in .NET User Secrets. Do not put the real key in `appsettings.json`, this README, or source control.

From the repository root, set your own long, random key:

```powershell
dotnet user-secrets set "Jwt:Key" "REPLACE_WITH_YOUR_OWN_LONG_RANDOM_KEY" --project .\HackerTerminal.API\HackerTerminal.API.csproj