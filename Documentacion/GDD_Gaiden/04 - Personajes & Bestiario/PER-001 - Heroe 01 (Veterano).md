---
tipo: personaje_jugable
id: PER-001
nombre: El Investigador
rol: Protagonista Solitario / Superviviente
estado: Aprobado
version: "3.0"
tags:
  - gdd/personaje
  - gdd/jugador
  - gdd/investigador
---

# Ficha de Personaje: El Investigador (`PER-001`)

> [!NOTE] Propósito del Documento
> Especificación del protagonista único de **Amnesis**: perfil psicológico, sensaciones de control, atributos base y curva de progresión.

---

## 1. Identidad & Perfil General

| Campo | Valor / Descripción |
| :--- | :--- |
| **Identificador / ID** | `PER-001` (`HeroId.Investigator`) |
| **Rol en el Juego** | Sujeto desorientado / Investigador en busca de verdad y catarsis |
| **Constitución Física** | Promedio (1.78 m / 74 kg), sujeto a fatiga física y estrés psíquico |
| **Puntos de Vida Base (HP)** | **100 HP** (Mejorable hasta 250 HP) |
| **Defensa Base (DEF)** | **0 DEF** (Mejorable hasta 40% de mitigación pasiva) |
| **Estamina / Foco (STA)** | **100 STA** (Mejorable hasta 200 STA) |
| **Armamento Inicial** | Pistola de percusión desgastada (9mm) + Linterna analógica + Radio portátil |

### 1.1 Motivación Principal
Despierta en una instalación en ruinas con amnesia traumática inducida. Guiado por la voz distante de una radio analógica, debe abrirse paso a través de sectores dominados por las aberraciones de su propia psique, desbloqueando armas y mejorando sus capacidades para confrontar su trauma y escapar.

---

## 2. Sensaciones de Control & Game Feel

- **Inercia y Respiración:** Movimiento deliberado y tenso. Cuando la salud o la estamina son bajas, el paso se vuelve errático y la respiración diegética aumenta de volumen.
- **Linterna & Cono de Visión:** El cono de luz en URP genera sombras en tiempo real; los monstruos reaccionan a la luz directa o a las pisadas apresuradas.
- **Mecánica Defensiva:**
  - **Bloqueo (Guardia):** Coloca el arma en posición defensiva absorbiendo el impacto a costa de estamina.
  - **Esquiva (Dodge):** Paso lateral rápido con *invulnerability frames* ajustados.

---

## 3. Métricas Numéricas en Motor

- **Puntos de Vida Base:** `100 HP`
- **Defensa Base:** `0 DEF`
- **Estamina Base:** `100 STA`
- **Tasa de Regeneración de Estamina:** `20 STA / segundo` (tras 1.0 s de retraso sin gastar)
- **Velocidad al Caminar:** `3.2 m/s`
- **Velocidad de Sprint:** `5.2 m/s` (consume `10 STA / s`)
- **Velocidad Arrastrando Cajas/Interactuables:** `1.8 m/s`
- **Radio de Colisión (Character Controller):** `0.40 m`
- **Altura de Cápsula:** `1.80 m`

---

## 4. Enlaces Relacionados
- [[04.1 - Índice de Personajes Jugables (Héroes)]]
- [[03.2 - Sistema de Combate, Armas & Habilidades]]
