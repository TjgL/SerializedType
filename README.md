# Serialized Type
This Unity package contains a singular type `SerializedType` that allows 
the serialization of `System.Type` variables inside the Unity Inspector.

## Installation
This package can be installed using the Unity Package Manager (UPM).
* Press the `+` button in the top left corner, and then select "Install package from git URL...".
* Copy the following URL: `https://github.com/TjgL/SerializedType.git`

## Usage
To use this class, add it as a serialized variable to a script exposed to the Unity Inspector.

### Declaration
```csharp
// Without initialization
public SerializedType<Foo> MySerializedClass;

// With initialization
public SerializedType<Component> Valid = SerializedType<Component>.FromType<Transform>();
```

If the `ST_ALLOW_IMPLICIT_CASTS` scripting define is set, an implicit cast from `System.Type` to `SerializedType` can be used.
> This implicit cast only causes a runtime error.

```csharp
// This cast is valid.
public SerializedType<Component> ValidCast = typeof(Transform); 

// This cast will only cause an error when the code is run.
public SerializedType<MonoBehaviour> InvalidCast = typeof(Transform);
```

### Getting the serialized type
The type can be retrieved in two different ways. Either via the `Type` getter, or with an implicit cast.

```csharp
public SerializedType<BaseState> InitialState = SerializedType<BaseState>.FromType<IdleState>();

private void Start()
{
    if (InitialState.Type != null)
    {
        // Implicit cast from SerializedType to System.Type
        var myState = InstantiateState(InitialState);
        ...
    }
}

private static BaseState InstantiateState(System.Type state)
{
    if (state != null && state.IsSubclassOf(typeof(BaseState)))
        return Activator.CreateInstance(state) as BaseState;
    
    Debug.LogError("Incorrect state.");
    return null;
}
```
