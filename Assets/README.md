# Project Architecture and Implementation Walkthrough

## Overview

This project is structured around a typical MVCS (Model-View-Controller-Signal) architecture pattern, enhanced by the use of dependency injection provided by Zenject, facilitating a modular and maintainable structure.

---

## Architecture

### Controllers

* **GameController**

  * Manages game states, initializes game logic, handles game over events, and manages scoring.

* **BirdController**

  * Handles bird behavior, physics interactions (via Rigidbody2D), and listens for player input signals to control bird movements.

* **PipesController**

  * Controls pipes generation, positioning, and recycling, using procedural randomness to enhance gameplay.

### Models

* **GameModel**

  * ScriptableObject that tracks game state and best scores, providing persistent state management across game sessions.

### Signals

* Custom signals (`ScoreUpdatedSignal`, `StartGameRequestSignal`, `GameOverSignal`, `GameStateChangedSignal`, `PlayerFlapSignal`, `AddScoreSignal`) facilitate decoupled communication between components, enabling event-driven logic.

### Views

* View scripts (`HUDView`, `MenuView`, `PipesView`, `PatternLooper`) handle visual representation and UI interactions, subscribing to signals to reflect game state changes visually.

### Interfaces

* Clearly defined interfaces for each controller (`IGameController`, `IBirdController`, `IPipesController`, etc.) enforce modularity and decoupling.

---

## Class Relations and Binding

### Dependency Injection (Zenject)

* Zenject is extensively used to bind implementations to their respective interfaces, providing injected dependencies throughout the system.

#### Binding Example (`GameInstaller.cs`):

* Controllers, models, and signals are bound using Zenject's container to maintain a centralized setup, enhancing testability and ease of modification.

```csharp
public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameModel gameModel;

    public override void InstallBindings()
    {
        Container.BindInstance(gameModel).AsSingle();
        Container.BindInterfacesTo<GameController>().AsSingle();
        Container.BindInterfacesTo<BirdController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<SignalBus>().AsSingle();
    }
}
```

### Signals

* Controllers and views subscribe to signals to trigger logic and UI updates independently, maintaining clear separation of concerns.

---

## Implementation Walkthrough

### Game Initialization Flow:

1. **Installer (`GameInstaller`)** sets up dependencies.
2. **GameController** initializes game state through injected `GameModel`.
3. **Signals** broadcast events (`StartGameRequestSignal`, `GameStateChangedSignal`) to initiate gameplay and state updates.

### Gameplay Loop:

* Player input triggers `PlayerFlapSignal`, processed by `BirdController`.
* Pipes management handled continuously by `PipesController`.
* GameController updates game state, manages scores, and broadcasts score updates.

### Game Over:

* Triggered by collision detection managed by `BirdController`.
* `GameOverSignal` dispatched, handled by `GameController`, updating the `GameModel` and broadcasting the final score through `ScoreUpdatedSignal`.

---

## Conclusion

This implementation employs clear separation of responsibilities, leveraging dependency injection and signals to achieve a modular, maintainable, and scalable architecture suitable for Unity projects.
