---
title: "05 - Scripts C# (Arquitectura Unity)"
tags: [scripts, csharp, source-code, unity, gaiden]
type: code-index
created: 2026-10-09
updated: 2026-10-09
---

# 05 - Scripts C# (Arquitectura Unity)

Esta sección contiene las implementaciones completas en **C#**, optimizadas y listas para ser importadas en la carpeta `Assets/_Scripts/` de Unity.

---

## 📂 Organización de Archivos de Código

```
05 - Scripts C# (Arquitectura Unity)/
├── Core/
│   ├── StaticInstance.cs       # Clases base genéricas de Singleton
│   ├── Systems.cs              # Contenedor persistente DontDestroyOnLoad
│   ├── GaidenGameManager.cs    # Gestor de máquina de estados global
│   ├── ResourceSystem.cs       # Repositorio O(1) de ScriptableObjects
│   └── AudioSystem.cs          # Reproductor 2D/3D con cross-fade
├── Managers/
│   ├── UnitManager.cs          # Spawner de personajes y zombies
│   ├── PartyManager.cs         # Gestión de Barry, Leon, Lucia y salud
│   └── CombatManager.cs        # Motor de sincronización y turnos
├── Exploration/
│   └── ExplorationController.cs# Control top-down, sprint y cuadrícula
├── Combat/
│   └── ReticleController.cs    # Aguja oscilante y feedback visual
├── Inventory/
│   └── InventorySystem.cs      # 8 casillas, munición y uso de hierbas
├── Units/
│   ├── UnitBase.cs             # Clase base abstracta de unidad
│   ├── HeroUnit.cs             # Componente de jugador en el mapa
│   └── EnemyOverworldUnit.cs   # Zombie en el mapa cenital
├── Scriptables/
│   ├── ScriptableHero.cs       # Definición de héroe
│   ├── ScriptableEnemy.cs      # Parámetros de monstruo y retículo
│   ├── ScriptableWeapon.cs     # Daño base y multiplicador de arma
│   └── ScriptableItem.cs       # Configuración de consumibles y llaves
└── Utilities/
    ├── Interfaces.cs           # IDamageable, IInteractable, ICombatTarget
    └── Helpers.cs              # Extensiones y funciones matemáticas
```

---

## 🚀 Guía de Integración Rápida en Unity

1. Copiar los archivos a la carpeta `Assets/_Scripts/` del proyecto de Unity.
2. Crear un Prefab en `Assets/Resources/Systems.prefab` y asignarle el componente `Systems` con `ResourceSystem` y `AudioSystem` como componentes o hijos.
3. Crear los assets de ScriptableObjects en `Assets/Resources/Units/`, `Weapons/` e `Items/` haciendo clic derecho en la ventana del Proyecto:
   - `Create -> Gaiden -> Units -> Hero` (Barry, Leon, Lucia)
   - `Create -> Gaiden -> Units -> Enemy` (ZombieMale, Cerberus, etc.)
   - `Create -> Gaiden -> Items -> Weapon` (Handgun, Shotgun, etc.)
4. Asignar las referencias en el Inspector y pulsar **Play**.
