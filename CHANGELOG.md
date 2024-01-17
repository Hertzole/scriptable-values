## [1.3.0] - 2024-01-17

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

## [1.2.0] - 2023-05-20

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

## [1.1.0] - 2023-04-06

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

## [1.0.0] - 2022-10-27

First release