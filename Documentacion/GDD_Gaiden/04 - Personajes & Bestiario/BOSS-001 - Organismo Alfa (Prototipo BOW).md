---
tipo: jefe_final
id: BOSS-001
nombre: Organismo Alfa (Prototipo B.O.W.)
fases_totales: 3
estado: Aprobado
version: "2.0"
tags:
  - gdd/boss
  - gdd/bow
---

# Ficha de Jefe Final: Organismo Alfa (Prototipo B.O.W.)

> [!NOTE] Propósito del Documento
> Especificación del gran enemigo némesis de Proyecto Gaiden: fases de combate, arena, telegrafiado y vulnerabilidades.

---

## 1. Parámetros del Retículo & Estadísticas Globales

| Parámetro | Valor | Justificación de Balance |
| :--- | :--- | :--- |
| **Salud Total (HP)** | **350 HP** | Exige racionar cartuchos de escopeta y proyectiles de lanzagranadas. |
| **Poder de Ataque** | **25 HP** por impacto | Cuatro ataques no evitados aniquilan a un héroe a plena salud. |
| **Ancho de Impacto (Hit Half-Width)** | `20 px` | Diana grande pero compensada por velocidad en fases finales. |
| **Ancho Crítico (Crit Half-Width)** | `3 px` | Núcleo biomutante expuesto. |
| **Intervalo de Ataque (Frames)** | `240 frames` (4.0 s) | Tiempo largo pero cada golpe es demoledor. |

---

## 2. Fases del Combate

### 2.1 Fase 1: Presión Frontal (HP: 100% - 60%)
- La aguja del retículo oscila a velocidad normal (`3 px/frame`). Cada disparo al núcleo ralentiza su avance.

### 2.2 Fase 2: Mutación Agresiva (HP: 60% - 25%)
- Sus extremidades se alargan; la aguja del retículo acelera a `4.5 px/frame`.
- El intervalo de ataque se reduce a `180 frames` (3.0 s).

### 2.3 Fase 3: Estado Crítico / Enrage (HP: 25% - 0%)
- El centro del objetivo se desplaza dinámicamente por la barra horizontal de 0 a 120 px, exigiendo anticipación máxima.
