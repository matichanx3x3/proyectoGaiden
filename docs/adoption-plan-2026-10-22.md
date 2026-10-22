# Plan de Adopción Brownfield (Gaiden Unity)

**Fecha**: 2026-10-22  
**Proyecto**: Resident Evil Gaiden Remake / Proyecto Gaiden (Unity 2D/3D)  
**Modo de Rigor**: Standard | **Automatización**: Guided

---

## 1. Resumen de la Auditoría

El proyecto cuenta con un volumen alto de documentación de alta calidad y código implementado que actualmente reside fuera de las rutas convencionales del plugin:

| Artefacto Existente | Ubicación Actual | Destino / Estrategia de Enlace | Estado de Cumplimiento |
| :--- | :--- | :--- | :--- |
| **GDD / Diseño de Juego** | `Documentacion/GDD_Gaiden/` | Registrar índice de sistemas y conservar bóveda Obsidian. | **Alto** (90+ docs estructurados) |
| **Arquitectura Técnica** | `Doc Tecnica/` | Conservar como fuente de verdad técnica; mapear decisiones clave a `docs/architecture/`. | **Alto** (Patrones y clases detalladas) |
| **Código C# Unity** | `Assets/Scripts/` | Configurado en `project.yaml` (`code_root: Assets/Scripts`). | **Activo** (33 scripts funcionales) |
| **Gestión de Sprints/Tareas** | *No formalizado* | `production/sprints/` y `production/epics/`. | **Pendiente** (A crear en Paso 3) |
| **Suite de Tests** | Escena funcional | Configurar ensamblado de pruebas de Unity (`Assets/Tests`). | **Recomendado** |

---

## 2. Plan de Migración y Convivencia

1. **Sin reescrituras innecesarias**: Mantener intactas las carpetas `Doc Tecnica/` y `Documentacion/` para el uso continuo en Obsidian.
2. **Puente de Integración CCGS**:
   - Registrar la referencia al Game Concept y al Índice de Sistemas apuntando a los MOCs ya creados.
   - Crear las carpetas de gestión ágil en `production/epics` y `production/sprints`.
3. **Paso Siguiente**: Desglosar los módulos prioritarios en historias y generar el plan del Sprint 1.
