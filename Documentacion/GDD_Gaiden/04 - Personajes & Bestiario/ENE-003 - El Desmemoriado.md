---
tipo: enemigo
id: ENE-003
nombre: El Desmemoriado (Amnesia Disociativa)
categoria: Pesado / Carga
estado: Aprobado
version: "1.0"
tags:
  - gdd/enemigo
  - gdd/tid
  - gdd/amnesia
  - gdd/sintoma
---

# Ficha de Enemigo: El Desmemoriado (`ENE-003`)

> [!NOTE] Rol de Diseño & Simbolismo Psicológico
> Encarnación del síntoma de la **Amnesia Lacunar / Disociativa del Trastorno de Identidad Disociativo (TID)**.
> Habita en el **Sector 05 (Contención Profunda & Núcleo)** bajo iluminación de refracción en prisma y aberraciones cromáticas.
> Es una bestia corpulenta y letal en línea recta, pero cuya mente fragmentada pierde el hilo del combate ante maniobras evasivas precisas.

---

## 1. Parámetros del Retículo & Estadísticas

- **Tipo en Motor:** `EnemyType.ZombieArmored`
- **Puntos de Vida (HP):** `70 HP` (Cuerpo acorazado y macizo).
- **Poder de Ataque:** `18 HP` (Golpe demoledor de embestida).
- **Ancho Zona de Impacto (Hit Half-Width):** `14 px`.
- **Ancho Diana Crítica (Crit Half-Width):** `3 px` (Situada en la nuca / sien expuesta).
- **Intervalo de Ataque:** `200 frames` (~3.3 segundos).

---

## 2. Comportamiento en Exploración & Combate (Mecánica de Olvido)

1. **Detección & Carga:**
   - Al avistar al Investigador en pasillos o salas abiertas, emite un rugido gutural e inicia una carrera ciega en estampida hacia su última posición conocida.
2. **La Evasión y Pérdida de Memoria:**
   - Si el jugador ejecuta una **Esquiva Táctica** lateral en el último segundo (`45 STA`), la criatura sigue de largo con su inercia.
   - Al no encontrar al objetivo de frente, **se detiene en seco, baja los brazos, mira a la izquierda y a la derecha con ademán desorientado y confuso**, como si hubiera olvidado por completo qué estaba haciendo o por qué corría.
3. **Reseteo de Rutina:**
   - Permanece inmóvil durante `3.0 segundos` (ventana dorada para atacarle por la espalda en impacto crítico garantizado). Si no es atacado, reanuda su patrulla pasiva desde cero.
