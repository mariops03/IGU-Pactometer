# IGU-Pactometer

Desktop application that simulates a political coalition pactometer for Spanish elections. Built with C# and WPF as a university project for the GUI (Interfaces Gráficas de Usuario) course.

## What it does

- **Electoral data management**: Create and manage electoral processes (general elections, regional elections) with party names, seat counts, and custom colors.
- **Bar chart visualization**: Displays election results as proportional bar charts, rendered entirely with custom drawing code (no charting libraries).
- **Comparative charts**: Compare results from the same parliament across different election years, showing seat gains/losses per party.
- **Pactometer**: Interactive coalition builder — drag parties into a coalition column to see if they reach absolute majority. A horizontal line marks the threshold.
- **Data export**: Export charts as images.
- **Pre-loaded data**: Includes real results from Spanish General Elections (2023, 2019) and Castilla y León regional elections (2022, 2019, 2015).

## Architecture

The project follows the **MVVM pattern** (Model-View-ViewModel):

```
Pactometro/
├── Model/              # Data classes (ProcesoElectoral, Partido, DatosElectorales)
├── ViewModels/         # Business logic and data binding
├── Views/              # XAML windows (MainWindow, VentanaSecundaria, VentanaExportar...)
├── Helpers/            # Input validation utilities
└── src/                # Icons and image assets
```

**Key design decisions:**
- All three chart types (results, comparative, pactometer) are drawn on a WPF `Canvas` using custom rendering — no external chart libraries.
- Non-modal secondary window for data management, synced with the main chart view.
- Material Design theme via [MaterialDesignInXAML](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit).

## Tech stack

- **C#** / **.NET Framework 4.7.2**
- **WPF** (Windows Presentation Foundation)
- **MVVM** architecture
- **Material Design** for styling
- **Newtonsoft.Json** for data serialization
- **WriteableBitmapEx** for image export

## Running it

1. Open `Pactometro.sln` in Visual Studio
2. Build and run (F5)

> Requires .NET Framework 4.7.2 and Windows.

## Context

Built for the **Interfaces Gráficas de Usuario** course (3rd year, Computer Engineering degree, Universidad de Salamanca, 2023-24).