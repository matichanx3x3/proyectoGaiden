---
tipo: enemigo
id: ENE-002
nombre: El Latente (Hiperreactividad Emocional)
categoria: Rápido / Presión
estado: Aprobado
version: "4.0"
tags:
  - gdd/enemigo
  - gdd/tlp
  - gdd/sintoma
---

# Ficha de Enemigo: El Latente (`ENE-002`)

> [!NOTE] Rol de Diseño & Simbolismo Psicológico
> Encarnación del síntoma de **Hiperreactividad y Estallido de Ira del Trastorno Límite de la Personalidad (TLP)**.
> Habita en el **Sector 02 (Módulo Médico y Cuarentena)** bajo iluminación cortante en turquesa y naranja.
> Permanece en un estado de reposo engañoso hasta que se traspasa su umbral de tolerancia sensorial.

---

## 1. Parámetros del Retículo & Estadísticas

- **Tipo en Motor:** `EnemyType.ZombieFast`
- **Puntos de Vida (HP):** `35 HP` (Salud baja para compensar su agresividad letal).
- **Poder de Ataque:** `14 HP` (Impacto de desgarro violento).
- **Ancho Zona de Impacto (Hit Half-Width):** `12 px` (Diana estrecha y móvil).
- **Ancho Diana Crítica (Crit Half-Width):** `2 px` (Ventana de alta exigencia de puntería).
- **Intervalo de Ataque:** `120 frames` (**Solo 2.0 segundos entre embestidas sucesivas**).

---

## 2. Comportamiento en Exploración (IA Táctica)

1. **Estado Latente (Falso Reposo):**
   - Se mantiene inmóvil en esquinas o bajo pasarelas metálicas, bañado por una tenue luz turquesa/teal. Si el Investigador camina en sigilo y apaga la linterna, puede eludirlo por completo.
2. **Disparador de Furia (Trigger):**
   - Si el jugador corre a menos de 4 metros, enfoca su linterna sobre él por más de 1 segundo o falla un tiro con arma de fuego en la misma sala, sus ojos y vetas corporales se encienden en **rojo alarma** y arremete a velocidad de sprint extremo.
3. **Respuesta en Combate:**
   - La mejor opción táctica es ejecutar la **Esquiva Táctica** justo antes del impacto (`45 STA`) o interceptarlo con un perdigonazo de Escopeta de Trinchera a quemarropa.

---

## 3. Enlaces Relacionados
- [[04.2 - Índice del Bestiario (Enemigos Comunes)]]
- [[03.2 - Sistema de Combate, Armas & Habilidades]]
