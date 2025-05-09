<div align="center">
     <img src="https://github.com/Hertzole/scriptable-values/assets/5569364/1d5ed9ee-4025-4136-b111-73d94affd22b">
     <br>
     <img src="https://img.shields.io/github/actions/workflow/status/Hertzole/scriptable-values/main.yml?style=flat&logo=Unity&label=Unity%20tests" alt="Unity tests workflow status">
     <a href="https://openupm.com/packages/se.hertzole.scriptable-values/"><img src="https://img.shields.io/npm/v/se.hertzole.scriptable-values?label=openupm&registry_uri=https://package.openupm.com" alt="OpenUPM"></a>
     <a href="https://hertzole.github.io/scriptable-values"><img src="https://img.shields.io/badge/Documentation-darkgreen" alt="Documentation"></a>
     <br>
     <a href="https://github.com/sponsors/Hertzole"><img src="https://img.shields.io/badge/Sponsor_me-GitHub-%23EA4AAA?style=flat&logo=githubsponsors" alt="Sponsor me on github badge"></a>
     <a href="https://ko-fi.com/Hertzole"><img src="https://img.shields.io/badge/Support_me-Ko--fi-%23FF5E5B?style=flat&logo=ko-fi" alt="Support me on ko-fi badge"></a>
     <br>
     <a href="https://sonarcloud.io/summary/overall?id=scriptable-values"><img src="https://sonarcloud.io/api/project_badges/measure?project=scriptable-values&metric=alert_status" alt="Quality gate status"></a>
     <a href="https://sonarcloud.io/summary/overall?id=scriptable-values"><img src="https://sonarcloud.io/api/project_badges/measure?project=scriptable-values&metric=code_smells" alt="Code smells"></a>
     <a href="https://sonarcloud.io/summary/overall?id=scriptable-values"><img src="https://sonarcloud.io/api/project_badges/measure?project=scriptable-values&metric=sqale_rating" alt="Maintainability rating"></a>
     <a href="https://sonarcloud.io/summary/overall?id=scriptable-values"><img src="https://sonarcloud.io/api/project_badges/measure?project=scriptable-values&metric=bugs" alt="Bugs"></a>
     <a href="https://hertzole.github.io/scriptable-values/coverage"><img src="https://hertzole.github.io/scriptable-values/coverage/badge_linecoverage.svg" alt="Code coverage"></a>
</div>

## ❓ What is this?

Scriptable Values allow you to use scriptable objects for reactive values, events, and collections instead of normal C# events and singletons.

You also don't need to care about values being saved between sessions as they are cleared before you enter play mode, but this is also customizable!

## ✨ Features

- Supports fast enter-play mode
- Automatically resets values to default values
- Scriptable values and events for most standard C# and Unity types
- Scriptable objects for values, events, lists, dictionaries, and pools
- Value reference type for easily picking between a constant value and a scriptable object reference
- Automatically collect stack traces to see where your values are set from
- Value and event listeners for easily hooking up events in the editor for when value changes/events are invoked
- Supports addressables for scriptable values and events
- [Source generator for generating event callback boilerplate](https://hertzole.github.io/scriptable-values/guides/callback-generator)
- Supports Unity's new runtime binding system and Unity.Properties

## 📦 Installation

Scriptable Values supports all Unity versions from **Unity 2021.3** and onward. It may support older versions but they are currently untested.

### OpenUPM (recommended)

If you have the OpenUPM CLI tool installed, you can add the package with this command:

```bash
openupm add se.hertzole.scriptable-values
```

Otherwise, follow these instructions:

1. Open Edit/Project Settings/Package Manager
2. Add a new Scoped Registry (or edit the existing OpenUPM entry)   
     Name: `package.openupm.com`  
     URL: `https://package.openupm.com`  
     Scope: `se.hertzole.scriptable-values`
3. Click `Save` (or `Apply`)
4. Open Window/Package Manager
5. Click `+`
6. Select `Add package by name...` or `Add package from git URL...`
7. Paste `se.hertzole.scriptable-values` into the name field 
8. Click `Add`

### Unity package manager through git
1. Open up the Unity package manager
2. Click on the plus icon in the top left and "Add package from git URL"
3. Paste in `https://github.com/Hertzole/scriptable-values.git#package`  
   You can also use `https://github.com/Hertzole/scriptable-values.git#dev-package` if you want the latest (but unstable!) changes.

## 🛠 Usage

Check out the [Getting Started guide](https://hertzole.github.io/scriptable-values/guides/getting-started) for more detailed instructions!

### [Scriptable Values](https://hertzole.github.io/scriptable-values/types/scriptable-value)

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

### [Scriptable Events](https://hertzole.github.io/scriptable-values/types/scriptable-event)

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

### [Scriptable Collections](https://hertzole.github.io/scriptable-values/types/scriptable-list)

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
        messageList.OnCollectionChanged += OnMessagesChanged;
    }

    private void OnDisable()
    {
        messageList.OnCollectionChanged -= OnMessagesChanged;
    }

    private void OnMessagesChanged(CollectionChangedArgs<string> args)
    {
        if (args.Action != NotifyCollectionChangedAction.Add) return;

        Text text = Instantiate(textPrefab);
        text.text = args.NewItems[0];
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
        playersDictionary.OnCollectionChanged += OnPlayersChanged;
    }

    private void OnDisable()
    {
        playersDictionary.OnCollectionChanged -= OnPlayersChanged;
    }

    private void OnPlayersChanged(CollectionChangedArgs<KeyValuePair<int, GameObject> args)
    {
        if (args.Action != NotifyCollectionChangedAction.Add) return;

        Text text = Instantiate(textPrefab);
        KeyValuePair<int, GameObject> value = args.NewItems[0];
        text.text = $"Player {value.Key}: {key.Value}";
    }
}
```

### [Scriptable Pool](https://hertzole.github.io/scriptable-values/types/scriptable-pool)

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

### [Value Reference](https://hertzole.github.io/scriptable-values/types/value-reference)

Value reference is a special type that allows you to pick in the inspector if you want a constant value in the inspector or a scriptable value reference (and even a addressable reference if you have addressables installed!). Meanwhile, in your code, you only interact with a single `Value` property and you don't need to care what type it is. 

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
   
## 📃 License

[MIT](https://github.com/Hertzole/scriptable-values/blob/master/LICENSE.md)
