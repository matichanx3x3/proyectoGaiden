---
title: "05 - Scripts C# (Arquitectura Unity)"
tags: [scripts, csharp, source-code, unity, gaiden]
type: code-index
created: 2026-10-09
updated: 2026-10-09
---

# 05 - Scripts C# (Arquitectura Unity)

Esta sección contiene el índice y especificación de los **31 scripts en C#** implementados y probados en el proyecto de Unity (`Assets/Scripts/`).

---

## 📂 Organización de Scripts en `Assets/Scripts/`

```
Assets/Scripts/
├── Camera/
│   └── CameraController.cs         # Cámara top-down en perspectiva (~55° pitch) con SmoothDamp
├── Combat/
│   ├── CombatArenaDirector.cs      # Orquesta la arena 3D (03_CombatArena) y proyecta al enemigo
│   └── ReticleController.cs        # Aguja oscilante a 60 FPS, zonas de impacto y diana crítica
├── Core/
│   ├── AudioSystem.cs              # Reproductor 2D/3D con cross-fade y pistas ambientales
│   ├── GameManager.cs              # Máquina de estados global (Exploration, Combat, Inventory, etc.)
│   ├── ResourceSystem.cs           # Repositorio auto-instanciable O(1) de ScriptableObjects
│   ├── StaticInstance.cs           # Clases base genéricas: StaticInstance, Singleton, PersistentSingleton
│   └── Systems.cs                  # Contenedor raíz persistente DontDestroyOnLoad
├── Editor/
│   └── FunctionalTestingSceneBuilder.cs # Constructor automatizado de la escena de pruebas funcionales
├── Exploration/
│   ├── DoorInteractable.cs         # Puertas batientes y corredizas interactivas con soporte para llaves/keycards
│   ├── ExplorationController.cs    # Movimiento 3D con CharacterController, escaleras, empuje de cajas y New Input System
│   ├── ItemPickup.cs               # Recogida en suelo con trigger, flotación senoidal y dual pickup
│   ├── LadderZone.cs               # Zona de escaleras verticales con modo de escalado dinámico
│   └── PushableBox.cs              # Cajas físicas empujables con Rigidbody, masa y damping lineal
├── Inventory/
│   └── InventorySystem.cs          # Maletín de 8 casillas, munición (tope 99), consumibles y DropItem en 3D
├── Managers/
│   ├── CombatManager.cs            # Carga/descarga aditiva de 03_CombatArena, retículo y turnos
│   ├── PartyManager.cs             # Gestión del trío (Barry, Leon, Lucia), veneno y daño
│   └── UnitManager.cs              # Spawner y tracking de unidades en overworld (RemoveEnemy robusto)
├── Scriptables/
│   ├── ScriptableEnemy.cs          # Parámetros de monstruo: HP, velocidad aguja, hit/crit width
│   ├── ScriptableHero.cs           # Estadísticas de personaje: HP max, velocidad, retrato
│   ├── ScriptableItem.cs           # Datos de consumibles, llaves y protecciones
│   └── ScriptableWeapon.cs         # Tipo de arma, munición, daño base y multiplicador crítico
├── UI/
│   ├── CombatUI.cs                 # Canvas translúcido de combate, barra oscilante y feedback de impacto
│   ├── ExplorationHUD.cs           # Interfaz de salud del héroe activo, arma y munición
│   ├── GameOverUI.cs               # Pantalla de derrota con reintento y reinicio
│   └── InventoryUI.cs              # Rejilla 8 casillas con debounce (0.25s), atajos 1-8, soltar ítem y cierre limpio
├── Units/
│   ├── EnemyOverworldUnit.cs       # Cáscara de zombie en el overworld (Trigger de combate al contacto)
│   ├── HeroUnit.cs                 # Cáscara del jugador en exploración con CharacterController
│   └── UnitBase.cs                 # Clase base abstracta de unidad con IDamageable
└── Utilities/
    ├── Helpers.cs                  # Extensiones y funciones matemáticas de interpolación
    └── Interfaces.cs               # Contratos desacoplados: IDamageable, IInteractable, ICombatTarget
```

---

## 🚀 Verificación de Estado en Editor

- **Compilación C#**: 0 errores de compilación (`Compilation Succeeded`).
- **Sistema de Entrada**: 100% migrado a `UnityEngine.InputSystem`.
- **Escenas en Build Settings**:
  0. `Assets/Scenes/00_Bootstrap.unity` (0)
  1. `Assets/Scenes/01_Functional_Testing.unity` (1)
  2. `Assets/Scenes/02_Ship_DeckA.unity` (2)
  3. `Assets/Scenes/03_CombatArena.unity` (3)
- **Hoja de Ruta Detallada**: Consulta [[01 - Roadmap y Estado del Proyecto (TODO)]] para revisar los hitos completados y el backlog de producción.
