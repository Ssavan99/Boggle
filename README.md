# Boggle

A web implementation of the word game **Boggle**, with single-player and
multiplayer modes. Built on ASP.NET Core with a dependency-free game engine
covered by unit tests.

<!-- LIVE_URL -->
> **Live demo:** _not yet redeployed — link goes here_

<!-- Add a screenshot or short GIF once captured: ![Boggle board](docs/screenshot.png) -->

## Features

- **Single player** — 4×4 board, 180-second round, find as many words as you can.
- **Multiplayer** — several players join one game by ID and race on the same board.
- **Duplicate cancellation** — if more than one player finds a word, it scores for nobody.
- **Real dictionary validation** — every guess is checked against a 194,433-word list.
- **Server-side adjacency validation** — the client submits board *coordinates*, not
  words, and the server verifies the path traces a legal chain of touching dice.
- **Game log** — completed rounds record what each player found.

## Tech stack

| Layer | Choice |
|---|---|
| Runtime | .NET 8 (LTS) |
| Web | ASP.NET Core MVC, Razor views |
| Frontend | Vanilla JavaScript + jQuery |
| Tests | MSTest — 31 tests |
| Persistence | None — games are held in memory |
| External services | None |

The web project has **no NuGet dependencies** and no database, so it runs
anywhere a .NET 8 runtime does.

## Running it

### With the .NET SDK

```bash
dotnet run --project Boggle/Boggle.csproj --urls http://localhost:5080
```

Then open <http://localhost:5080>.

### With Docker

```bash
docker build -t boggle .
```

```bash
docker run --rm -p 8080:8080 boggle
```

Then open <http://localhost:8080>. The container honours a `PORT` environment
variable, which is what most free hosts inject.

### Tests

```bash
dotnet test UnitTests/UnitTests.csproj
```

## Architecture

```
Browser ──HTTP/JSON──▶ ServerController ──▶ Server (singleton)
                              │                  │
                              │                  └──▶ Game ──▶ Board ──▶ Die
                              │
                              ├──▶ WordValidationEngine   (adjacency rules)
                              └──▶ WordDictionary         (194k word list)
```

The client sends **board coordinates**, not words. `WordValidationEngine`
reconstructs the word from the dice at those coordinates and confirms the path
is legal, so a player cannot submit a word that is not actually on their board.

Scoring is deferred until the round ends, because duplicate cancellation cannot
be resolved until every player has finished.

### Layout

| Path | Contents |
|---|---|
| `Boggle/Models/` | Game engine — `Game`, `Board`, `Die`, `User`, `Server`, `WordDictionary` |
| `Boggle/Controllers/` | `ServerController` (JSON API), `HomeController`, `WordValidationEngine` |
| `Boggle/wwwroot/democlient.html` | The game client — served at `/` |
| `Boggle/wwwroot/js/` | `boggle.js` (screens and input), `apis.js` (server calls), `utils.js` (board rendering) |
| `Boggle/Views/` | Razor views for the supporting pages |
| `UnitTests/` | MSTest suite covering the engine and controllers |

## HTTP API

All endpoints live under `/Server` and return JSON shaped as
`{ ok: true, ... }` or `{ ok: false, msg: "..." }`.

| Endpoint | Purpose |
|---|---|
| `GET /Server/newGame` | Create a game, returns a `gameId` |
| `GET /Server/login?gameId&username` | Join a game |
| `GET /Server/startGame?gameId` | Begin the round |
| `GET /Server/getGameState?gameId&username` | Board, players, scores, time remaining |
| `GET /Server/guess?gameId&username&strcoords` | Submit a word as board coordinates |
| `GET /Server/endGame?gameId` | End the round early |
| `GET /Server/resetGame?gameId` | Reset for another round |
| `GET /Server/removePlayer?gameId&username` | Remove a player |
| `GET /Server/getGameLog?gameId` | Results of completed rounds |

Example:

```bash
curl "http://localhost:5080/Server/newGame"
```

## Rules

- Find words on a grid of random letters.
- Letters must touch **vertically**, **horizontally**, or **diagonally**, forming a chain.
- Chains may run up, down, forward, backward, and diagonally.
- You may not skip or jump over letters.
- Words must be at least **three** letters long.
- Each die may be used only once per word.
- Only words in the English dictionary count.
- If another player guessed the same word, everyone who guessed it scores zero.

Full rules: <https://www.hasbro.com/common/instruct/boggle.pdf>

### Scoring

| Word length | Points |
|---|---|
| 3–4 | 1 |
| 5 | 2 |
| 6 | 3 |
| 7 | 5 |
| 8+ | 11 |

## Project history

This started in 2021 as a **team project for a university software-engineering
course**, built by ten students, and was deployed to Azure App Service at
`totallynotboggle.azurewebsites.net`. That subscription was decommissioned with
the course, taking the site with it.

The original repository is preserved unmodified and archived at
[Ssavan99/Boggle-class-2021](https://github.com/Ssavan99/Boggle-class-2021).
The full commit history of everyone who contributed is retained in this
repository too.

This repo is a solo modernization of that work:

- Migrated .NET Core 3.1 (end-of-life since December 2022) to .NET 8 LTS
- Adopted the minimal hosting model — `Startup.cs` folded into `Program.cs`
- Fixed dictionary loading that depended on the process working directory and
  crashed outside a development layout
- Removed Azure publish profiles containing credential fields from what is a
  public repository
- Containerized the app, replacing the retired Azure deployment
