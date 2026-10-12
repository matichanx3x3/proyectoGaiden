---
tipo: enemigo
id: ENE-002
nombre: Acechador Ágil
categoria: Rápido / Presión
estado: Aprobado
version: "2.0"
tags:
  - gdd/enemigo
  - gdd/acechador
---

# Ficha de Enemigo: Acechador Ágil (`ZombieFast`)

> [!NOTE] Propósito del Documento
> Especificación de la criatura rápida de Proyecto Gaiden, diseñada para forzar reflejos inmediatos en el combate por retículo.

---

## 1. Parámetros del Retículo & Estadísticas
- **Tipo en Motor:** `EnemyType.ZombieFast`
- **Puntos de Vida (HP):** `35 HP` (Salud reducida).
- **Poder de Ataque:** `14 HP` (Impacto veloz).
- **Ancho Zona de Impacto (Hit Half-Width):** `12 px` (Diana estrecha).
- **Ancho Diana Crítica (Crit Half-Width):** `2 px` (Máxima precisión requerida).
- **Intervalo de Ataque:** `120 frames` (**Solo 2.0 segundos entre ataques**).
- **Estrategia Recomendada:** Utilizar la Escopeta para impactar sin necesidad de ajustar la diana crítica en el último frame.
