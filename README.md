# Unity Fundamentals: team training

Code for an eight-lecture course that takes a game team from "new to C# and/or Unity" to writing Unity code
the way we want it written in production. Unity 6 (6000.5), URP, C# 9.

The course material lives in the course's Google Drive folder, not in this repo:
the slide decks (0, 1.1 to 4.2), **Lesson Notes** (run sheets), **Facilitator Guide** (setup, demos, likely questions),
**Handbook** (reference, exercises, misconceptions; two docs: Days 1-2 and Days 3-4) and **Cheat Sheet** (all the rules on one sheet).

## What's here

```
unity-fundamentals/
├── csharp-basics/                 .NET console solution for lecture 1.2 (examples, exercises, solutions)
└── UnityProject/                  the Unity 6 project
    ├── Assets/CourseDemos/        demo scenes, one per lecture (named day.lecture), and their small scripts
    ├── Assets/_Project/           prefabs, configs, Localization and other assets the demos use
    └── Packages/com.course.fundamentals/   the course code, as an embedded package
```

## Getting started

1. Install Unity Hub with **Unity 6000.5** (plus Windows Build Support (IL2CPP)), the **.NET 8 SDK**, and Rider or Visual Studio.
2. `cd csharp-basics` and `dotnet build CSharpBasics.sln`. Run an example with `dotnet run --project Examples -- 12`.
3. Open `UnityProject/` in Unity Hub (*Add > Add project from disk*). The first import takes a few minutes.
4. Open any scene in `Assets/CourseDemos/`. `ClickToMoveDemo` is scene 0 of the build.

Before committing: Asset Serialization is Force Text, every `.meta` file is committed, and each developer registers
UnityYAMLMerge once (see `.gitattributes`).
