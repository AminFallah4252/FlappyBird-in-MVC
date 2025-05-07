# MVC Architecture in Unity Development

## 🧠 What is MVC?

**MVC (Model-View-Controller)** is a software architectural pattern that separates an application into three main components:

- **Model** – Handles data and business logic.
- **View** – Handles the UI and visuals.
- **Controller** – Handles user input and coordinates between the Model and View.

---

## 🛠️ Adapting MVC to Unity

Unity is not built around MVC. It uses a **component-based architecture**. But you can still apply MVC to organize your code better, especially for UI-heavy or scalable systems.

### ✅ Model

- Pure C# classes (no MonoBehaviour)
- Contains game logic, state, and rules
- Doesn’t know anything about Unity UI

```csharp
public class PlayerModel
{
    public int Health { get; private set; } = 100;

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0) Health = 0;
    }
}
