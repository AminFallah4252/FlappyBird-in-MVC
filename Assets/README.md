# Flappy Bird Zenject Architecture (MVC + Interfaces + Signals)

## 🧩 Architecture Overview

- **Model**: `GameModel` (ScriptableObject with state and score)
- **View**: `UIUpdater` (MonoBehaviour, updates TMP text from signals)
- **Controller**: Interface-based (`IGameController`, etc.) logic powered by Zenject
- **Communication**: `SignalBus` from Zenject for clean event flow
- **Tests**: Using `ZenjectUnitTestFixture` for clean DI and signal injection

## ✅ Pros

- Easy to test (SignalBus + DI + Interfaces)
- Swappable components (you can mock or replace controllers)
- Zenject handles lifecycle (`IInitializable`, `ITickable`)
- Great for medium and large projects

## ❌ Cons

- A bit overkill for tiny prototypes
- Requires some boilerplate (interfaces + DI)

---