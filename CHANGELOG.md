# 2.0.0 - Unreleased

### Added

- Support for Unity.Properties and the new runtime UI binding system. All types now implement `IDataSourceViewHashProvider` and `INotifyBindablePropertyChanged`
- Source generator for generating event boilerplate code
  - Includes new attributes for marking fields and properties; `[GenerateScriptableCallbacks]`, `[GenerateValueCallback]`, `[GenerateEventCallback]`, `[GeneratePoolCallback]`, and `[GenerateCollectionCallback]`
- RuntimeScriptableObject implements `INotifyPropertyChanging` and `INotifyPropertyChanged` interfaces that can be used to track most property changes, including regular field properties
- `ScriptableValue<T>` now has `OnBeforeSetValue(T oldValue, T newValue)` and `OnAfterSetValue(T oldValue, T newValue)` methods that can be overridden to track value changes
- `ScriptableEvent` and `ScriptableEvent<T>` now have `OnBeforeInvoke` and `OnAfterInvoke` methods that can be overridden to track event invocations
- `ICanBeReadOnly` interface for scriptable objects that can be marked as read-only
- `INotifyScriptableCollectionChanged` interface for scriptable collections that can be used to track collection changes
- Full parity with `List<T>` to `ScriptableList<T>`

### Changed

- Tooltips are no longer included in builds
- **BREAKING**: Removed `ScriptableValue<T>.GetValue()`
- **BREAKING**: You can no longer override `ScriptableValue<T>.SetValue(T, bool)` in derived classes
- **BREAKING**: `ScriptableValue<T>.OldNewValue<T>` is now obsolete, use `ValueEventHandler<T>` instead
- **BREAKING**: `ScriptableEvent<T>` no longer inherits from `ScriptableEvent` and thus does not share the same `Invoke` methods anymore
- Global and per-object collect stack traces setting is now saved in a separate user settings file instead of editor prefs and scriptable object file
- **BREAKING**: Read-only errors are now thrown as exceptions instead of logged using `Debug.LogError`
- **BREAKING**: `ScriptableList<T>` no longer has individual event callbacks, instead it has a single `OnCollectionChanged` event
  - `Resverse()` and `Sort()` now only triggers a single `Replace` event
  - `TrimExcess()` no longer triggers a change event at all
  - The enum `ListChangeType` is now marked as obsolete as it's no longer used
- `ScriptableList<T>.TrimExcess()` can now be called even when the object is marked as read-only
- `ScriptableList<T>` now implements `INotifyCollectionChanged` and `InotifyScriptableCollectionChanged<T>`
- **BREAKING**: `ScriptableDictionary<TKey, TValue>` no longer has individual event callbacks, instead it has a single `OnCollectionChanged` event
  - `TrimExcess()` no longer triggers a change event at all
  - The enum `DictionaryChangeType` is now marked as obsolete as it's no longer used
- `ScriptableDictionary<TKey, TValue>.TrimExcess()` can now be called even when the object is marked as read-only
- `ScriptableDictionary<TKey, TValue>` now implements `INotifyCollectionChanged` and `InotifyScriptableCollectionChanged<KeyValurPair<TKey, TValue>>`
- **BREAKING**: `ScriptablePool<T>.Return(T item)` is now obsolete, use `Release(T item)` instead
- **BREAKING**: `ScriptableValueListener.ToggleListening(bool listen)` is now obsolete, use `SetListening(bool listen)` instead

### Fixed

- NullReferenceException being thrown when exiting play mode when a scriptable object is selected

## 1.3.1 - 2024-01-31

### Fixed

- Fixed Odin inspector compatibility

# 1.3.0 - 2024-01-17

### Added

- Added `SetValueWithoutNotify` to `ValueReference<T>` to allow you to set the value without invoking the change event
- Added `EnsureCapacity` to `ScriptableList<T>`

### Fixed

- Fixed scriptable value editor breaking if the value is null
- Fixed scriptable value editor having the wrong height in newer Unity versions
- Fixed the package not having an author
- Fixed allocation when using `foreach` on scriptable lists and dictionaries

### Removed

- Removed obsolete `ResetValues` from `RuntimeScriptableObject`

# 1.2.0 - 2023-05-20

### Added

- Added `ValueReference<T>` type for allowing you to pick between a constant value, a scriptable value, or a addressable reference to a scriptable value
- Added icons for all built-in scriptable values and scriptable events

### Changed

- Tweaked scriptable value editor to more prominently show the current value

### Fixed

- Fixed scriptable value drawers sometimes not showing the correct display label in 2022.2
- Fixed scriptable value drawers sometimes not showing at all in IMGUI editors
- Fixed scriptable value editors having a scrollbar if addressables is installed
- Fixed event listeners having the same name as the value listeners
- Fixed editor compatibility with Odin inspector

# 1.1.0 - 2023-04-06

### Added

- Added support for disabling stack trace collection, both globally and per object
- Added scriptable pool, along with scriptable game object pool
- Added `Invoke()` to ScriptableEvent to easily invoke from Unity events
- Added `SetEqualityCheck` and `ClearOnStart` properties to scriptable list
- Added custom property drawers in 2022.2+ for scriptable types
- Added `AddRange`, `Exists`, `Find`, `InsertRange`, and `RemoveRange` to scriptable list
- Added general `OnChange` event to scriptable list and scriptable dictionary
- Added scriptable values and events for all common Unity types
- Added asset references for all available types

### Changed

- Obsoleted `ResetValues`, it's now separated into two methods; `OnStart` for when the game starts and `ClearSubscribers` for clearing event subscribers

### Fixed

- Fixed scriptable value listener editors not working as intended
- Fixed several issues with checking previous values
- Fixed values not being removed properly on dictionaries in editors
- Fixed non-serializable types breaking editors
- Fixed warnings on Unity 2022.2

# [1.0.0] - 2022-10-27

First release