# Scriptable Values

## ❓ What is this?

Scriptable Values allow you to use scriptable objects for reactive values, events, and collections instead of normal C# events and singletons.

You also don't need to care about values being saved between sessions as they are cleared before you enter play mode, but this is also customizable!

## ✨ Features

### Scriptable Values

Scriptable Values allow you to have a single value across multiple objects and listen to its changing events.  
There are scriptable values for all primitive C# values that you can use out of the box, but it's not difficult to create your own.

#### Scriptable Values Usage

**Basic usage:**    
```cs
public class PlayerHealth : MonoBehaviour
{
    public ScriptableInt health;

    public void TakeDamage(int damage)
    {
        health.Value -= damage;
    }
}

public class HealthUI : MonoBehaviour
{
    public ScriptableInt health;
    public Text healthText;

    private void OnEnable()
    {
        health.OnValueChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        health.OnValueChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int oldValue, int newValue)
    {
        healthText.text = "Health: " + newValue.ToString();
    }
}
```

**Creating your own scriptable value:**  
```cs
[CreateAssetMenu(fileName = "New Scriptable Vector3", menuName = "My Values/Scriptable Vector3")]
public class ScriptableVector3 : ScriptableValue<Vector3> 
{
    // Nothing more, it's as simple as that!
}
```

### Scriptable Events

Scriptable Events aim to replace normal C# events and the requirement to know the object they come from. With scriptable events, an event can come from anywhere but you are still able to know where if needed.

#### Scriptable Events Usage

**Basic usage:**   
```cs 
public class PlayerHealth : MonoBehaviour
{
    public ScriptableIntEvent onHealthChanged;

    private int currentHealth = 100;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        onHealthChanged.Invoke(this, currentHealth);
    }
}

public class HealthUI : MonoBehaviour
{
    public ScriptableIntEvent onHealthChanged;
    public Text healthText;

    private void OnEnable()
    {
        onHealthChanged.OnInvoked += OnHealthChanged;
    }

    private void OnDisable()
    {
        onHealthChanged.OnInvoked -= OnHealthChanged;
    }

    private void OnHealthChanged(object sender, int args)
    {
        healthText.text = "Health: " + args.ToString();
    }
}
```

**Creating your own scriptable event:**    
```cs
[CreateAssetMenu(fileName = "New Scriptable Vector3 Event", menuName = "My Events/Scriptable Vector3")]
public class ScriptableVector3Event : ScriptableEvent<Vector3> 
{
    // Nothing more, it's as simple as that!
}
```

### Scriptable Collections

There are two scriptable collections that you can inherit from, list and dictionary, to create collections that you can use across your objects.  
There's also a premade ScriptableGameObjectList that you can use to store game objects.

#### Scriptable collections usage

**Basic list usage:**    
```cs
[CreateAssetMenu(fileName = "New Scriptable String List", menuName = "My Lists/Scriptable String List")]
public class ScriptableStringList : ScriptableList<string> {}

public class MessageHandler : MonoBehaviour
{
    public ScriptableStringList messageList;

    public void AddMessage(string message)
    {
        messageList.Add(message);
    }
}

public class MessageUI : MonoBehaviour
{
    public ScriptableStringList messageList;
    public Text textPrefab;

    private void OnEnable()
    {
        messageList.OnAdded += OnMessageAdded;
    }

    private void OnDisable()
    {
        messageList.OnAdded -= OnMessageAdded;
    }

    private void OnMessageAdded(string newMessage)
    {
        Text text = Instantiate(textPrefab);
        text.text = newMessage;
    }
}
```

**Basic dictionary usage:**    
```cs
[CreateAssetMenu(fileName = "New Scriptable Player Dictionary", menuName = "My Dictionaries/Scriptable Player Dictionary")]
public class ScriptablePlayerDictionary : ScriptableDictionary<int, GameObject> {}

public class PlayerID : MonoBehaviour
{
    public int myId;
    public ScriptablePlayerDictionary playersDictionary;

    private void Awake()
    {
        playersDictionary.Add(myId, gameObject);
    }
}

public class PlayersUI : MonoBehaviour
{
    public ScriptablePlayerDictionary playersDictionary;
    public Text textPrefab;

    private void OnEnable()
    {
        playersDictionary.OnAdded += OnPlayerAdded;
    }

    private void OnDisable()
    {
        playersDictionary.OnAdded -= OnPlayerAdded;
    }

    private void OnPlayerAdded(int key, GameObject value)
    {
        Text text = Instantiate(textPrefab);
        text.text = $"Player {key}: {value}";
    }
}
```

## 📦 Installation

Scriptable Values supports all Unity versions from **Unity 2021.3** and onward. It may support older versions but they are currently untested.

### Unity package manager through git
1. Open up the Unity package manager
2. Click on the plus icon in the top left and "Add package from git url"
3. Paste in `https://github.com/Hertzole/scriptable-values.git#package`  
   You can also paste in `https://github.com/Hertzole/scriptable-values.git#dev-package` if you want the latest (but unstable!) changes.
