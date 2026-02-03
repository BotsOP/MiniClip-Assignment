# Design Choices

## Dependency Injection & System Communication

To connect different systems in the game, I implemented a lightweight custom dependency injection system. I didn't want to use singletons and/or FindObjectOfType, but keeping connection simple.
Systems register themselves as providers, and dependencies are injected via attributes at startup. This keeps systems loosely coupled and makes it very clear which classes depend on which other systems. It also allows most of the game logic to live outside of MonoBehaviours, which improves testability and keeps Unity-specific code seperate.
The injector acts as the composition root or the "glue" for this project.

## State Machine Design

Enemies use a generic state machine that is intentionally designed to keep logic out of individual states. This is a common pitfall I’ve seen in other projects, where states are slowly managed by themselves.
In this implementation:

- States are mostly responsible for animation and local behavior
- Transitions are defined externally using predicates
- Decision-making logic lives outside the states themselves
- This keeps states themselves small and generic


## UI Architecture (MVP)

For UI, I used the MVP (Model–View–Presenter) pattern.
Views are interfaces and only handle visuals and user input.
Presenters are pure C# classes that contain UI logic.
Models store data and business logic.
This enables to easily use create new screens through new presenters that reference already instanced models and views. Communication happens through interfaces.
This pattern is used for systems like the upgrade selection screen, game over screen, and leaderboard. And is easily testable because it does not require monobehaviours.

## Interfaces & SOLID Principles

Interfaces are used extensively across the project, even in cases where there is currently only one implementation. This is a deliberate choice to, keep systems decoupled, allow easy, extension later, enforce clear responsibilities. Some classes like the SpawnManager implement multiple interfaces to further adhere to the SOLID principles and keep responsibility seperated.

## Object Pooling

A generic Object Pool Manager was implemented to manage multiple object pools at once. It uses Unity’s ObjectPool<T>, but adds the type of the instance to act as the key for which ObjectPool<T> to use.
To avoid hardcoding behavior, the pool manager uses the Strategy Pattern to define lifecycle behavior (OnGet, OnRelease). This allows different pooled objects (enemies, effects, UI elements) to define custom behavior without modifying the pool manager itself.

## Upgrade System

The upgrade system is implemented using a combination of the Decorator Pattern and the Factory Pattern. Each upgrade wraps an IHitResolver and incrementally modifies its behavior, allowing upgrades to be stacked and extended without requiring changes to existing logic. Upgrades have a preferred order set in the factory to maintain correct upgrade order. Creating new upgrades is handled through ScriptableObjects acting as factories, which makes upgrades easy to configure, reuse, and add to the game. 

## Enemy System

Enemies are created using a mix of interfaces, inheritance, and factory-based ScriptableObjects. Each enemy type defines its data. The factory is also responsible for creating the enemy’s runtime logic. The base Mole class can be extended to implement custom behavior while still reusing shared systems like the state machine, spawning logic, and scoring. In practice, adding a new enemy usually involves creating a new ScriptableObject and only extending the base class when additional behavior is required.

## Trade-offs & Future Improvements

Object pooling keys use object types for ease of use and simplicity, which required a few empty MonoBehaviour marker classes for unique mole models. This was a trade-off to keep pool usage simple and safe for lower-level systems, but I am still not sure whether it is the best approach.

Gameplay kind of sucks and is unclear what is happening. I prioritized code and architecture for the assignment but I do want to "finish" this project by completing my vision of mixing a whack a mole game with vampire survivors.

The custom DI system works well, but if this project is going to increase in size I would start considering using existing solutions like Zenject, Reflex, or InitArgs for reliability and more functionality.

I added my singleton event manager, but in the end I used it sparingly to avoid hidden/unpredictable execution flows. But would like to use it more when finishing the project and implementing systems like VFX and sounds.

I used the new Input System, when I remembered at the end the team primarily uses the old system. Input is abstracted behind an interface so switching would be straightforward.


## Final Notes

The architecture favors clarity and separation of concerns, and most systems are designed so they can be reused or extended without modification to existing code. Overall I'm happy and confident in what I made. I know it is not perfect (otherwise I wouldn't be applying for a junior position ;) ) but would love to hear your feedback and what could be improved.
