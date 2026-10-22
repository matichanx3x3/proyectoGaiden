# Sprint 1: Consolidación de Combate, IA y Core Systems

**Periodo**: Sprint Actual  
**Etapa**: Pre-Production → Production  
**Objetivo del Sprint**: Robustecer las mecánicas clave del combate táctico (huida, feedback crítico), integrar la IA de patrulla y persecución en pasillos, y sentar la base para la combinación de ítems en el inventario.

---

## 🎯 Historias Seleccionadas para el Sprint

| ID | Historia / Tarea | Estimación | Módulo | Estado |
| :--- | :--- | :--- | :--- | :--- |
| **GAID-01** | **Mecánica de Huida en Combate**: Permitir abortar el combate activo con cálculo de probabilidad o penalización de turnos en `CombatManager.cs` y `CombatUI.cs`. | 3 pts | `Combat` | Ready |
| **GAID-02** | **IA de Patrulla y Detección Overworld**: Implementar estados *Patrol*, *Alert* y *Chase* en `EnemyOverworldUnit.cs` con rangos de visión/oído. | 5 pts | `Exploration / Units` | Ready |
| **GAID-03** | **Combinación de Ítems en Inventario**: Permitir combinar Hierba Verde + Hierba Verde en `InventoryUI.cs` e `InventorySystem.cs`. | 3 pts | `Inventory` | Ready |
| **GAID-04** | **Configuración de Tests de Regresión Básicos**: Crear estructura en `Assets/Tests/EditMode` para validar fórmulas de daño y consumibles sin abrir el editor en playmode. | 2 pts | `Core / QA` | Ready |

---

## 📊 Capacidad y Métricas

- **Puntos Totales Planificados**: 13 pts
- **Riesgo Técnico Principal**: Integración del NavMesh o raycasts en pasillos para la IA de los zombies en perspectiva cenital 3D.
- **Criterio de Aceptación del Sprint**: Todas las mecánicas verificadas en la escena `01_Functional_Testing` y en runtime.
