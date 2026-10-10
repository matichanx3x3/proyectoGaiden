---
title: "Resident Evil Gaiden - Proyecto de Arquitectura Unity en Obsidian"
tags: [readme, gaiden, unity]
created: 2026-10-09
updated: 2026-10-09
---

# Resident Evil Gaiden - Arquitectura Unity

Este repositorio y bóveda de Obsidian documenta la especificación técnica, patrones de diseño y arquitectura de software para recrear y modernizar el juego **Resident Evil Gaiden** (GBC, 2001) dentro del motor **Unity**.

Para navegar por la documentación interactiva:
👉 **Abre la nota principal:** [[00 - MOC (Map of Content) - Arquitectura Gaiden Unity]]  
📋 **Consulta el estado y TODO:** [[01 - Roadmap y Estado del Proyecto (TODO)]]  
🎨 **O abre el lienzo visual:** [[Gaiden_Architecture.canvas]]

## 💡 Estado Técnico Actual (PoC y Testing Funcional Verificado)
- **Exploración**: Top-Down en Perspectiva 3D (~55° pitch) con `CharacterController` y `CameraController`.
- **Interacciones del Jugador**: Puertas interactivas (`DoorInteractable`: estándar y bloqueadas por tarjeta de acceso), verticalidad y escaleras (peldaños con `stepOffset = 0.45f`, rampas y escaleras de mano verticales con `LadderZone`), empuje físico de cajas (`PushableBox` con Rigidbody) y soltado de ítems al escenario (`InventorySystem.DropItem`).
- **Escena de Pruebas**: Escena funcional dedicada `01_Functional_Testing` para validar todas las mecánicas en un entorno de pruebas controlado y multi-nivel.
- **Entrada**: Soporte completo del paquete moderno `UnityEngine.InputSystem` para teclado, ratón y gamepad.
- **Instancia de Combate**: Carga aditiva de escena 3D independiente (`03_CombatArena`, estilo *Persona* / JRPG) con retículo oscilante e iluminación de tensión.
- **Inventario**: Maletín de 8 casillas con debounce, consumo inmediato, atajos de teclado y recogida diegética de objetos (`ItemPickup`).

## Estructura de la Bóveda
- `00 - Indice/`: Mapa de contenidos, rutas de navegación recomendadas y [[01 - Roadmap y Estado del Proyecto (TODO)|Roadmap / TODO]].
- `01 - Arquitectura General/`: Setup de Unity, escena bootstrap, jerarquía de GameObjects y ciclo de juego.
- `02 - Core Systems/`: Singletons, GameManager, Systems persistentes, Audio, Recursos, Unidades y Utilidades.
- `03 - Mecanicas Gaiden/`: Combate en escena aditiva 3D, exploración top-down en perspectiva, Party, armas e inventario.
- `04 - Fundamentos Teoricos y Diseno/`: Fundamentación teórica de sistemas RPG, POO en C#, Herencia e Interfaces, Genéricos y Eventos.
- `05 - Scripts C# (Arquitectura Unity)/`: Códigos fuente C# completos y sincronizados con `Assets/Scripts/`.
- `06 - Diagramas y Canvas/`: Canvas nativo de Obsidian y diagramas Mermaid de clases y secuencias.
