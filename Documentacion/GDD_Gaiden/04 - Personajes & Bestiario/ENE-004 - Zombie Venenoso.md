---
tipo: enemigo
id: ENE-004
nombre: Zombie Venenoso
categoria: Especializado
estado: Aprobado
version: "1.0"
tags:
  - gdd/enemigo
  - gdd/veneno
---

# Ficha de Enemigo: Zombie Venenoso

> [!NOTE] Rol de Diseño
> Enemigo de amenaza táctica que castiga severamente los fallos de timing aplicando el estado alterado de envenenamiento continuo.

---

## 1. Parámetros del Retículo & Estadísticas
- **Puntos de Vida (HP):** `55`.
- **Poder de Ataque:** `10 HP`.
- **Ancho Zona de Impacto (Hit Half-Width):** `15 px`.
- **Ancho Diana Crítica (Crit Half-Width):** `3 px`.
- **Intervalo de Ataque:** `190 frames` (~3.1 segundos).
- **Efecto de Estado Especial:** **50% de probabilidad de infligir Envenenamiento (`IsPoisoned = true`)**.
  - El veneno drena salud de forma sostenida en el mapa de exploración hasta consumir una Hierba Mixta o Spray de Primeros Auxilios.
