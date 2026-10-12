---
tipo: nivel
id: LVL-00X
nombre: "{{title}}"
orden_campana: 1
dificultad: Media # [Baja | Media | Alta | Pesadilla]
tiempo_estimado: "15-20 min"
estado: Borrador # [Borrador | En Revisión | Aprobado]
version: "1.0"
fecha_creacion: "{{date}}"
autor: ""
tags:
  - gdd/nivel
  - gdd/level_design
---

# Diseño de Nivel: {{title}}

> [!NOTE] Propósito del Documento
> Especificación completa del nivel: objetivos, ambientación visual y sonora, curva de ritmo/interés, layout en planta/alzado con leyenda, puzles, retos y distribución de enemigos.

---

## 1. Concepto General & Objetivos

### 1.1 Premisa del Nivel
<!-- ¿Dónde transcurre y qué ocurre narrativamente aquí? (Ej: Almacén abandonado en los bajos fondos, asediado por patrullas enemigas) -->

### 1.2 Objetivos
- **Objetivo Principal:** <!-- Ej: Infiltrarse en la sala de calderas y rescatar al informante Miliki -->
- **Objetivos Secundarios / Opcionales:**
  - <!-- [ ] Encontrar las 3 medallas ocultas -->
  - <!-- [ ] Completar el nivel sin activar ninguna alarma -->

---

## 2. Ambientación, Arte & Audio

- **Entorno:** <!-- Interior industrial, tejados nocturnos, cloacas húmedas -->
- **Iluminación & Paleta de Color:** <!-- Tonos azulados y sombras densas, focos móviles de vigilancia -->
- **Clima / Efectos Ambientales:** <!-- Lluvia sobre cristaleras, vapor de tuberías rotas -->
- **Pista de Audio / OST:** <!-- Pista de sigilo en exploración, transición dinámica a percusión en combate -->
- **Efectos Sonoros Clave:** <!-- Chirrido de metales, gotas de agua, ecos de pisadas -->

---

## 3. Curva de Interés & Ritmo (Pacing)

> [!TIP] Gestión de la Tensión
> Equilibrar picos de adrenalina (combates duros, persecuciones) con valles de descanso (resolución de puzles, exploración, recolección de tesoros).

```
Interés / Tensión
  ▲
  │            [Combate Duro]                   [Clímax / Boss / Fin]
  │                 ▲                                    ▲
  │   [Escaramuza]  │                                    │
  │        ▲        │          [Puzle / Valle]           │
  │        │        │                 ▲                  │
  │───[Inicio]──────┴───────[Descanso]┴────[Tesoro]──────┴────► Tiempo
```

### Momentos Clave del Recorrido:
1. **Min 0-3 (Introducción):** Familiarización con el entorno y patrullas simples aisladas.
2. **Min 3-7 (Escalada):** Encuentro con soldados con escudo; introducción de trampas de fuego.
3. **Min 7-11 (Valle de Puzle):** Sala de interruptores y cajas empujables para abrir la compuerta.
4. **Min 11-16 (Pico de Tensión):** Emboscada en dos alturas con tiradores y enemigos pesados.
5. **Min 16-20 (Resolución):** Entrada a la sala final / semijefe y acceso al siguiente nivel.

---

## 4. Level Layout (Mapa & Mapeado Explicado)

### 4.1 Representación Gráfica
<!-- Adjuntar plano o mockup: ![[layout_nivel_01.png]] -->
<!-- Indicar si la perspectiva es Planta Cenital (3D/Top-down) o Alzado Lateral (2D / 2.5D) -->

### 4.2 Leyenda Oficial del Mapa
| Símbolo / Color | Elemento | Descripción en este Nivel |
| :---: | :--- | :--- |
| 🟢 `[S]` | **Inicio del Nivel** | Punto de aparición inicial del jugador |
| 🔴 `[1]` | **Soldado Básico** | Enemigo simple patrullando |
| 🛡️ `[2]` | **Soldado con Escudo**| Enemigo defensivo protegiendo pasillo estrecho |
| 🗝️ `[K]` | **Llave de Celda** | Objeto clave necesario para abrir Puerta A |
| 📦 `[B]` | **Caja Empujable** | Bloque móvil para alcanzar plataforma superior |
| ⚡ `[T]` | **Trampa / Reto** | Pinchos retráctiles o chorros de fuego temporizados |
| 💾 `[CP]`| **Checkpoint** | Punto de guardado y reaparición |
| 🏆 `[M]` | **Coleccionable** | Medalla oculta o pergamino |
| 🏁 `[E]` | **Final de Nivel** | Puerta de salida hacia Nivel Siguiente |

### 4.3 Ruta Principal (Ruta Crítica)
1. **Zona 1 (Almacén Exterior):** El jugador avanza desde `[S]`, elimina a 2 patrullas simples y llega a la compuerta bloqueada.
2. **Zona 2 (Pasillo de Trampas):** Esquiva chorros de fuego `[T]` sincronizando la patada deslizante.
3. **Zona 3 (Sala de Puzle):** Mueve la caja `[B]` para trepar a la 2ª planta y recoger la llave `[K]`.
4. **Zona 4 (Cámaras de Combate):** Enfrenta al semijefe y alcanza la salida `[E]`.

### 4.4 Rutas Opcionales & Zonas Secretas
- **Conducto de Ventilación Superior:** Requiere doble salto o empujar una caja extra; conduce a una Medalla Oculta `[M]` y un Power-up de daño.

---

## 5. Puzles del Nivel

### Puzle A: <!-- Ej: Restauración del Puente Eléctrico -->
- **Ubicación:** Sala de Máquinas (Planta Baja).
- **Mecánica:** Mover 2 cajas de alta conductividad hacia los bornes eléctricos abiertos.
- **Resultado al Resolver:** Se activa el puente levadizo que conecta con la 2ª planta.

---

## 6. Retos & Obstáculos Específicos

- **Obstáculo 1 (Pinchos en Suelo):** Salen cada 2 segundos; castigo de 20 HP por impacto.
- **Obstáculo 2 (Chorros de Fuego):** Bloquean el paso alternativamente; obligan a cronometrar el dash o deslizarse por abajo.

---

## 7. Enemigos Presentes en el Nivel
- [[04.2 - Soldado Simple]]: `6 unidades`
- [[04.2 - Soldado con Escudo]]: `3 unidades`
- [[04.2 - Semi-Jefe]]: `1 unidad en la salida`

---

## 8. Recursos, Items & Economía
- **Botiquines de Salud:** `2 botiquines medios`
- **Power-Ups:** `1x Inmunidad (10 segundos) antes de la emboscada`
- **Coleccionables:** `3 Medallas Ocultas`
