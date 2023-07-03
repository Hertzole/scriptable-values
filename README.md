# Scriptable Values

[![openupm](https://img.shields.io/npm/v/se.hertzole.scriptable-values?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/se.hertzole.scriptable-values/)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=scriptable-values&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=scriptable-values)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=scriptable-values&metric=coverage)](https://sonarcloud.io/summary/new_code?id=scriptable-values)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=scriptable-values&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=scriptable-values)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=scriptable-values&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=scriptable-values)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=scriptable-values&metric=bugs)](https://sonarcloud.io/summary/new_code?id=scriptable-values)

## ❓ What is this?

Scriptable Values allow you to use scriptable objects for reactive values, events, and collections instead of normal C# events and singletons.

You also don't need to care about values being saved between sessions as they are cleared before you enter play mode, but this is also customizable!

## ✨ Features

- Supports fast enter play mode
- Automatically resets values to default values
- Scriptable values and events for most standard C# and Unity types
- Scriptable objects for values, events, lists, dictionaries, and pools
- Value reference type for easily picking between a constant value and a scriptable object reference
- Automatically collect stack traces to see where your values are set from
- Value and event listeners for easily hooking up events in the editor for when value changes/events are invoked
- Networking support for [FishNet](https://assetstore.unity.com/packages/tools/network/fish-net-networking-evolved-207815) and [Netcode for GameObjects](https://docs-multiplayer.unity3d.com/) (developer branch only currently!)

## 📦 Installation

Scriptable Values supports all Unity versions from **Unity 2021.3** and onward. It may support older versions but they are currently untested.

### OpenUPM (recommended)

If you have the OpenUPM CLI tool installed, you can add the package with this command:  
`openupm add se.hertzole.scriptable-values`

Otherwise follow these instructions:

1. Open Edit/Project Settings/Package Manager
2. Add a new Scoped Registry (or edit the existing OpenUPM entry)   
     Name: `package.openupm.com`  
     URL: `https://package.openupm.com`  
     Scope: `se.hertzole.scriptable-values`
3. Click `Save` (or `Apply`)
4. Open Window/Package Manager
5. Click `+`
6. Select `Add package by name...` or `Add package from git URL...`
7. Paste `se.hertzole.scriptable-values` into name 
8. Click `Add`

### Unity package manager through git
1. Open up the Unity package manager
2. Click on the plus icon in the top left and "Add package from git url"
3. Paste in `https://github.com/Hertzole/scriptable-values.git#package`  
   You can also paste in `https://github.com/Hertzole/scriptable-values.git#dev-package` if you want the latest (but unstable!) changes.

## 🛠 Usage

### Scriptable Values

![Scriptable value](https://github.com/Hertzole/scriptable-values/assets/5569364/241cdba9-222f-46dd-b09a-70dd63891014)


Scriptable Values allow you to have a single value across multiple objects and listen to its changing events.  
There are scriptable values for all primitive C# values and most standard Unity types that you can use out of the box, but it's not difficult to create your own.

#### Scriptable Values Usage

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

![Scriptable event](https://github.com/Hertzole/scriptable-values/assets/5569364/1c2a93c2-3df7-43e8-9ab1-d3ebf87af267)

Scriptable Events aim to replace normal C# events and the requirement to know the object they come from. With scriptable events, an event can come from anywhere but you are still able to know where if needed.  
There are scriptable events for all primitive C# values and most standard Unity types that you can use out of the box, but it's not difficult to create your own.

#### Scriptable Events Usage
 
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

![Scriptable game object list](https://github.com/Hertzole/scriptable-values/assets/5569364/6da80bd8-7a9d-4b86-ae31-65653ff703a6)

There are two scriptable collections that you can inherit from, list and dictionary, to create collections that you can use across your objects.  
There's also a premade `ScriptableGameObjectList` that you can use to store game objects.

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

### Scriptable Pool

![Scriptable game object pool](https://github.com/Hertzole/scriptable-values/assets/5569364/7d733396-4f74-4afa-8301-1d297641d932)

Scriptable Pool is a type of scriptable object that you can use for pooling objects. It can be a normal C# class, a component, game object, or even scriptable objects.  
There's a premade `ScriptableGameObjectPool` that you can use to pool game objects.

#### Scriptable Pool Usage

```cs
public class EnemySpawner : MonoBehaviour
{
    public ScriptableGameObjectPool enemyPool;
    
    public void SpawnEnemy()
    {
        GameObject enemy = enemyPool.Get();
        enemy.transform.position = Vector3.zero;
    }
}

public class EnemyHealth : MonoBehaviour
{
    public ScriptableGameObjectPool enemyPool;

    public void Kill()
    {
        enemyPool.Return(this.gameObject);
    }
}
```

**Creating your own class pool:**    
```cs
[CreateAssetMenu(fileName = "New Scriptable String Builder Pool", menuName = "My Pools/String Builder Pool")]
public class ScriptableStringBuilderPool : ScriptablePool<StringBuilder> 
{
    // Called when you need to create a new object.
    protected override StringBuilder CreateObject()
    {
        return new StringBuilder();
    }
    
    // Called when a object may need to be destroyed, like when clearing the pool.
    protected override void DestroyObject(StringBuilder item)
    {
        // Can't destroy a string builder...
    }
}
```

**Creating your own component pool:**    
```cs
[CreateAssetMenu(fileName = "New Scriptable Enemy Pool", menuName = "My Pools/Enemy Pool")]
public class ScriptableEnemyPool : ScriptableComponentPool<Enemy> 
{
    // Nothing more, it's as simple as that!
}
```

**Creating your own scriptable object pool:**    
```cs
[CreateAssetMenu(fileName = "New Scriptable Int Value Pool", menuName = "My Pools/Int Value Pool")]
public class ScriptableIntValuePool : ScriptableObjectPool<ScriptableInt> 
{
    // Nothing more, it's as simple as that!
}
```

### Value Reference

![Value reference](https://github.com/Hertzole/scriptable-values/assets/5569364/7fcc6a1e-3108-4df3-87c7-5ed5383abdc9)

Value reference is a special type that allows you you to pick in the inspector if you want a constant value in the inspector or a scriptable value reference (and even a addressable reference if you have addressables installed!). Meanwhile, in your code, you only interact with a single `Value` property and you don't need to care what type it is. 

#### Value Reference Usage

```cs
public class PauseManager : MonoBehaviour
{
    public BoolReference isPaused; // You can use a built-in type.
    public ValueReference<float> timescale; // You can also just use the generic type.
    
    public void TogglePause()
    {
        isPaused.Value = !isPaused.Value;
        timescale.Value = isPaused.Value ? 0f : 1f;
    }
}
```

## 🌐 Networking Support

You can automatically sync scriptable values, lists, and dictionaries over the network. All of the networking solutions follow the same setup.

### FishNet
```cs
public ScriptableInt intValue;
public ScriptableIntList list;
public ScriptableIntDictionary dictionary;

[SyncObject]
private readonly SyncedScriptableValue<int> syncedInt = new SyncedScriptableValue<int>();
[SyncObject]
private readonly SyncedScriptableList<int> syncedList = new SyncedScriptableList<int>();
[SyncObject]
private readonly SyncedScriptableDictionary<int, int> syncedDictionary = new SyncedScriptableDictionary<int, int>();

void Awake()
{
    // Always initialize the synced objects as early as possible!
    syncedInt.Initialize(intValue);
    syncedList.Initialize(list);
    syncedDictionary.Initialize(dictionary);
}

void OnDestroy()
{
    // It's important to dispose the synced objects when you're done with them!
    syncedInt.Dispose();
    syncedList.Dispose();
    syncedDictionary.Dispose();
}

void OnEnable()
{
    // To listen to events, you always go to the source scriptable object.
    intValue.OnValueChanged += OnIntValueChanged;
}

void OnDisable()
{
    intValue.OnValueChanged -= OnIntValueChanged;
}

void SetValue(int value)
{
    // To set a value, you always go to the source object.
    // Only the server can set values.
    intValue.Value = value;
}
```

### Netcode for GameObjects
```cs
public ScriptableInt intValue;
public ScriptableIntList list;
// Dictionary is currently not supported.

private readonly SyncedScriptableValue<int> syncedInt = new SyncedScriptableValue<int>();
private readonly SyncedScriptableList<int> syncedList = new SyncedScriptableList<int>();

void Awake()
{
    // Always initialize the synced objects as early as possible!
    syncedInt.Initialize(intValue);
    syncedList.Initialize(list);
}

void OnDestroy()
{
    // It's important to dispose the synced objects when you're done with them!
    syncedInt.Dispose();
    syncedList.Dispose();
}

void OnEnable()
{
    // To listen to events, you always go to the source scriptable object.
    intValue.OnValueChanged += OnIntValueChanged;
}

void OnDisable()
{
    intValue.OnValueChanged -= OnIntValueChanged;
}

void SetValue(int value)
{
    // To set a value, you always go to the source object.
    // Only the server can set values.
    intValue.Value = value;
}
```
   
## ♥ Support

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/I2I4IHAK)
