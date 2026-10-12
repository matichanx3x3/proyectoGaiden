---
tipo: personaje_jugable
id: PER-00X
nombre: "{{title}}"
rol: Protagonista
estado: Borrador # [Borrador | En Revisión | Aprobado]
version: "1.0"
fecha_creacion: "{{date}}"
autor: ""
tags:
  - gdd/personaje
  - gdd/jugador
---

# Ficha de Personaje: {{title}}

> [!NOTE] Propósito del Documento
> Define en detalle la identidad, trasfondo narrativo, características físicas, personalidad, esquema de controles y métricas jugables del personaje jugable según las directrices de diseño del proyecto.

---

## 1. Identidad & Perfil General

| Campo | Valor / Descripción |
| :--- | :--- |
| **Nombre Completo** | <!-- Nombre real del personaje --> |
| **Apodo / Alias** | <!-- Nombre en clave o apodo popular --> |
| **Edad** | <!-- Edad cronológica y aparente --> |
| **Género / Sexo** | <!-- Género con el que se identifica --> |
| **Lugar de Nacimiento / Origen** | <!-- Ciudad, región o reino de procedencia --> |
| **Ocupación / Rol** | <!-- Ninja, mercenario, monje, caballero, etc. --> |
| **Familia & Aliados Clave** | <!-- Relaciones directas o vínculos significativos --> |
| **Facción / Clan** | <!-- Clan Nikitonipongo, Resistencia, etc. --> |

### 1.1 Motivación Principal
<!-- ¿Qué empuja al personaje a actuar? ¿Qué pierde si fracasa? (Ej: Rescatar al presidente, venganza por su clan, redención personal) -->

### 1.2 Arco del Personaje
<!-- ¿Cómo evoluciona a lo largo del juego? Estado inicial vs. Revelación o transformación final. -->

---

## 2. Atributos Físicos & Referencias Visuales

### 2.1 Descripción Física
- **Constitución:** <!-- Atlética, ágil, robusta, esbelta -->
- **Altura & Peso:** <!-- Ej: 1.82 m / 78 kg -->
- **Color de Piel & Ojos:** <!-- Descripción visual -->
- **Cabello & Peinado:** <!-- Color, corte, detalles distintivos -->
- **Cicatrices / Marcas Distintivas:** <!-- Tatuajes, heridas de combate, marcas de clan -->
- **Indumentaria & Traje:** <!-- Descripción del ropaje, armadura, telas, accesorios ninja -->

### 2.2 Concept Art & Referencias
<!-- Insertar imagen en Obsidian con ![[nombre_imagen.png]] o enlaces a tablero de referencias -->
- **Actor / Personaje de Referencia Físico:** <!-- Ej: Tom Hardy en Warrior, Wolverine, Hayabusa -->
- **Paleta de Colores Primaria:** <!-- Colores dominantes del diseño -->

---

## 3. Personalidad, Comportamiento & Voz

### 3.1 Rasgos de Personalidad
- **Forma de Hablar / Acento:** <!-- Serio, sarcástico, lacónico, dialecto particular -->
- **Habilidades Sociales:** <!-- Solitario, líder carismático, distante -->
- **Ambiciones & Deseos:** <!-- Meta personal más allá de la misión -->
- **Debilidades & Defectos:** <!-- Impulsividad, orgullo, trauma del pasado -->
- **Talentos Especiales:** <!-- Memoria fotográfica, sigilo absoluto, maestría marcial -->

### 3.2 Diseño de Voz (Casting)
- **Tono & Registro:** <!-- Grave, áspero, enérgico, susurrado -->
- **Referentes de Voz / Actor de Doblaje:** <!-- Actor o personaje cuya voz sirve de referencia -->

---

## 4. Acciones del Personaje & Esquema de Controles

> [!TIP] Coherencia Multiplataforma
> Mapear cada acción física a su botón correspondiente en Gamepad PlayStation, Xbox y Teclado/Ratón en PC.

| Acción / Movimiento | PS4 / PS5 | Xbox One / Series | PC (Teclado + Ratón) | Descripción Mecánica & Comportamiento |
| :--- | :--- | :--- | :--- | :--- |
| **Moverse** | Stick Izquierdo | Stick Izquierdo | `W, A, S, D` | Movimiento analógico omnidireccional. |
| **Mover Cámara** | Stick Derecho | Stick Derecho | Movimiento Ratón | Rotación orbital 360° alrededor del personaje. |
| **Puñetazo Alto (Rápido)** | Triángulo (`△`) | Botón `Y` | `Clic Izquierdo` / `Ctrl` | Golpe directo rápido a la cabeza; inicia combo. |
| **Puñetazo Bajo / Remate** | Círculo (`○`) | Botón `B` | `Alt` / Tecla `E` | Golpe al plexo/barrido; rompe guardias ligeras. |
| **Patada / Ataque Fuerte** | Cuadrado (`□`) | Botón `X` | `Shift` / Tecla `Q` | Ataque pesado con retroceso sobre el enemigo. |
| **Salto** | Equis (`⨉`) | Botón `A` | `Espacio` | Salto vertical con control aéreo limitado. |
| **Patada Voladora** | `⨉` + `□` | `A` + `X` | `Espacio` + `Shift` | Ataque aéreo descendente con derribo. |
| **Patada Deslizante (Slide)**| `⨉` + `△` | `A` + `Y` | `Alt` + `Ctrl` | Deslizamiento bajo que sortea trampas y derriba. |
| **Agacharse / Sigilo** | `L1` | `LB` | Tecla `C` / `Ctrl` | Reduce radio de detección acústica y visual. |
| **Correr / Sprint** | `R1` | `RB` | `Shift` sostenido | Aumenta velocidad un 60%, gasta energía/stamina. |
| **Interacción con Entorno** | `R2` | `RT` | Tecla `F` | Empujar cajas, subir escaleras, abrir puertas. |

---

## 5. Métricas de Movimiento & Parámetros Numéricos

> [!CHECKLIST] Valores Base para Ajuste en Unity
> - [ ] Velocidad al Andar: `X m/s` (Ej: 3.5 m/s)
> - [ ] Velocidad al Correr: `X m/s` (Ej: 6.5 m/s)
> - [ ] Altura de Salto: `X m` (Ej: 1.8 m)
> - [ ] Radio de Detección al Sigilo: `X m` (Ej: 2.0 m)
> - [ ] Puntos de Vida (HP) Base: `100`
> - [ ] Estamina / Energía Base: `100`
> - [ ] Resistencia / Armadura Base: `0`

---

## 6. Habilidades Especiales & Progresión

### 6.1 Habilidades Iniciales
<!-- Listado de técnicas desbloqueadas desde el prólogo -->

### 6.2 Habilidades Desbloqueables / Mejoras
| Habilidad | Tipo (Activa/Pasiva) | Requisito / Coste | Efecto en Combate o Exploración |
| :--- | :--- | :--- | :--- |
| <!-- Ej: Golpe Ígneo --> | Activa | Nivel 2 + 500 XP | Añade daño de fuego y quema defensas. |
| <!-- Ej: Paso Sombrío --> | Pasiva | Derrotar Boss 1 | Reduce tiempo de alerta en sigilo un 30%. |

---

## 7. Relación con Otros Elementos del Juego
- **Aliados:** [[04.5 - Índice de NPCs & Aliados]]
- **Enemigos Clave / Némesis:** [[04.2 - Índice del Bestiario (Enemigos Comunes)]], [[04.3 - Índice de Jefes Finales (Bosses)]]
- **Armas y Equipamiento:** [[03.2 - Sistema de Combate, Armas & Habilidades]]
