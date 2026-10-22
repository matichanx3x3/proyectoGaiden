---
tipo: enemigo
id: ENE-004
nombre: El Miasmático (Culpa & Asfixia)
categoria: Especializado / Tóxico
estado: Aprobado
version: "3.0"
tags:
  - gdd/enemigo
  - gdd/tac
  - gdd/sintoma
---

# Ficha de Enemigo: El Miasmático (`ENE-004`)

> [!NOTE] Rol de Diseño & Simbolismo Psicológico
> Encarnación del síntoma de **Culpa y Entumecimiento Post-Episodio del Trastorno Alimentario Compulsivo (TAC)**.
> Habita en el **Sector 04 (Comunicaciones & Almacén de Carga)** bajo una densa niebla desaturada y luces cenitales azul pervinca.

---

## 1. Parámetros del Retículo & Estadísticas

- **Tipo en Motor:** `EnemyType.ZombiePoison`
- **Puntos de Vida (HP):** `55 HP` (Masa corporal densa y resistente).
- **Poder de Ataque:** `10 HP` por desgarro.
- **Ancho Zona de Impacto (Hit Half-Width):** `15 px`.
- **Ancho Diana Crítica (Crit Half-Width):** `3 px`.
- **Intervalo de Ataque:** `190 frames` (~3.1 segundos).
- **Efecto de Estado Especial:** **50% de probabilidad de infligir Asfixia por Miasma (`IsPoisoned = true`)**.
  - Libera un vapor denso que reduce en un **40% la tasa de regeneración de estamina** y drena salud gradualmente hasta consumir un Inhalador de Estabilización.

---

## 2. Comportamiento en Combate
- Al recibir un impacto crítico, su torso expulsa una bocanada de miasma gris que ciega parcialmente la barra oscilante durante 2 segundos.
- Requiere mantener la distancia y utilizar el bloqueo defensivo (`25 STA`) para evitar que el vapor contamine al Investigador.
