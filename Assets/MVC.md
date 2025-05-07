# MVC Architecture in Unity Development

## 🧠 What is MVC?

**MVC (Model-View-Controller)** is a software architectural pattern that separates an application into three main components:

- Model = data + rules
- View = UI, visuals
- Controller = brain connecting the model & view

The Controller listens to user input, tells the Model to update, and then updates the View accordingly.

---

## 🛠️ Adapting MVC to Unity

Unity is not built around MVC. It uses a **component-based architecture**. 

**Is MVC Always Ideal in Unity?**

Not really. Unity’s structure often blends View and Controller together in MonoBehaviours. That’s fine for small projects, but if your game is scaling or you're working in a team, separating things out helps.

Instead of strict MVC, many devs prefer MVVM or just Separation of Concerns using ScriptableObjects, Events (like UnityEvent or C# events), or architecture frameworks like Zenject, UniRx, Entitas, or Game Architecture with ScriptableObjects.

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
```
### ✅ View

- Handles Unity UI elements
- Updates visuals only
- Doesn’t contain game logic

```csharp
public class PlayerView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    public void UpdateHealth(int currentHealth)
    {
        healthText.text = $"HP: {currentHealth}";
    }
}
```

### ✅ Controller

- Bridges Model and View
- Handles input and updates
- Can be MonoBehaviour

```csharp
public class PlayerController : MonoBehaviour
{
    private PlayerModel model;
    [SerializeField] private PlayerView view;

    private void Start()
    {
        model = new PlayerModel();
        view.UpdateHealth(model.Health);
    }

    public void OnDamageButtonPressed()
    {
        model.TakeDamage(10);
        view.UpdateHealth(model.Health);
    }
}
```
