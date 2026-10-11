---
title: "05 - Scripts C# (Arquitectura Unity)"
tags: [scripts, csharp, source-code, unity, gaiden]
type: code-index
created: 2026-10-09
updated: 2026-10-09
---

# 05 - Scripts C# (Arquitectura Unity)

Esta sección contiene el índice y especificación de los **33 scripts en C#** implementados y probados en el proyecto de Unity (`Assets/Scripts/`).

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
│   ├── BoxDestinationPoint.cs      # Sockets de puzle en blocking (Azul: intermedio reubicable / Rojo: meta definitiva con fijación permanente)
│   ├── DoorInteractable.cs         # Puertas batientes con giro en contra del jugador, apertura lenta / de golpe (burst open), llaves y transición a nueva zona oscura
│   ├── ExplorationController.cs    # Movimiento 3D con CharacterController, escaleras, agarre de cajas con las manos (push/drag) y New Input System
│   ├── ItemPickup.cs               # Recogida en suelo con trigger, flotación/rotación continua (DOMoveY, DORotate) y secuencia punch de recolección
│   ├── LadderZone.cs               # Zona de escaleras verticales con modo de escalado dinámico
│   └── PushableBox.cs              # Cajas arrastrables solo bajo interacción activa, detección de sockets azul/rojo y peldaño escalón desplegable solo en destino final
├── Inventory/
│   └── InventorySystem.cs          # Maletín de 8 casillas, munición (tope 99), consumibles y DropItem en 3D
├── Managers/
│   ├── CombatManager.cs            # Carga/descarga aditiva de 03_CombatArena, retículo y turnos
│   ├── PartyManager.cs             # Gestión del trío (Barry, Leon, Lucia), veneno y daño
│   └── UnitManager.cs              # Spawner y tracking de unidades en overworld (RemoveEnemy robusto)
├── Scriptables/
│   ├── ScriptableEnemy.cs          # Parámetros de monstruo: HP, velocidad aguja, hit/crit width
│   ├── ScriptableHero.cs           # Estadísticas de personaje: HP max, velocidad, retrato
│   ├── ScriptableItem.cs           # Datos de consumibles, llaves (ItemId con RedKeyCard) y protecciones
│   └── ScriptableWeapon.cs         # Tipo de arma, munición, daño base y multiplicador crítico
├── UI/
│   ├── CombatUI.cs                 # Canvas translúcido de combate, barra oscilante, sacudidas de cámara (DOShakePosition) y feedback elástico
│   ├── ExplorationHUD.cs           # Interfaz de salud del héroe activo, arma, munición, sacudida de barra de vida y punch en munición
│   ├── GameOverUI.cs               # Pantalla de derrota con reintento y reinicio
│   ├── InventoryUI.cs              # Rejilla 8 casillas con debounce (0.25s), apertura pop y punch en selección/uso/soltado con DOTween (unscaledTime)
│   └── ScreenFader.cs              # Fundido a negro (Fade In/Out) en pantalla completa para transiciones de nivel y cinemáticas estilo Resident Evil
├── Units/
│   ├── EnemyOverworldUnit.cs       # Cáscara de zombie en el overworld (Trigger de combate al contacto)
│   ├── HeroUnit.cs                 # Cáscara del jugador en exploración con CharacterController
│   └── UnitBase.cs                 # Clase base abstracta de unidad con IDamageable
└── Utilities/
    ├── Helpers.cs                  # Extensiones y funciones matemáticas de interpolación
    └── Interfaces.cs               # Contratos desacoplados: IDamageable, IInteractable, ICombatTarget
```

---

## 📦 Biblioteca de Prefabs Reutilizables (`Assets/Prefabs/Exploration/`)

Para asegurar la modularidad del diseño de niveles y permitir la creación de habitaciones en el barco mediante *drag-and-drop*, se han consolidado los siguientes Prefabs estandarizados:

1. **`Door_Standard.prefab`**: Marco con bisagra desacoplada (`Hinge`), batiente con colisionador sólido (`DoorSlab`), colisionador trigger de interacción frontal/trasero, giro dinámico en contra del jugador y apertura lenta / de golpe (`DoorInteractable`).
2. **`Door_Locked_Red.prefab`**: Variante de seguridad con material rojo (`Mat_Test_Door_Red`), cerrojo bloqueado que exige `ItemId.RedKeyCard` para desbloquearse y sonido de pestillo con sacudida.
3. **`Door_LevelTransition.prefab`**: Puerta de cambio de nivel con fondo negro interior (`DarkVoid_Backdrop`), apertura hacia la oscuridad y fundido a negro (`ScreenFader`) reubicando al jugador.
4. **`PushableBox_Puzzle.prefab`**: Caja física interactiva (`PushableBox`) con agarre activo de manos, cinemática por defecto (inmóvil frente a colisiones libres), detección de sockets azul/rojo y peldaño escalón (`Step_Ledge`) desplegable exclusivamente al bloquearse en el socket rojo.
5. **`Ladder_Vertical.prefab`**: Escalera vertical de 3.5m con postes, peldaños equidistantes y trigger `LadderZone` para activación automática del modo de escalada.
6. **`Pickup_Key_Red.prefab`**: Llave de seguridad roja (`ItemId.RedKeyCard`) con animación de flotación y rotación.
7. **`Pickup_GreenHerb.prefab`**: Hierba curativa (`ItemId.GreenHerb`).
8. **`Pickup_FirstAidMed.prefab`**: Botiquín de primeros auxilios (`ItemId.FirstAidMed`).

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
