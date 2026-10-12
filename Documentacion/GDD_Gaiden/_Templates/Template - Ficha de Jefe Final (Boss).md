---
tipo: jefe_final
id: BOSS-00X
nombre: "{{title}}"
nivel_aparicion: ""
fases_totales: 3
estado: Borrador # [Borrador | En Revisión | Aprobado]
version: "1.0"
fecha_creacion: "{{date}}"
tags:
  - gdd/boss
  - gdd/jefe
---

# Ficha de Jefe Final: {{title}}

> [!NOTE] Propósito del Documento
> Documenta el enfrentamiento culminante de zona o final del juego: trasfondo narrativo, arena de combate, fases progresivas, telegrafiado de ataques y recompensas.

---

## 1. Identidad, Trasfondo & Trascendencia Narrativa

- **Nombre del Boss:** {{title}}
- **Título / Epíteto:** <!-- Ej: Maestro Traidor, El Dragón de Hierro, General de las Sombras -->
- **Motivación en la Historia:** <!-- ¿Por qué se enfrenta al protagonista? ¿Qué protege o busca imponer? -->
- **Ubicación en el Juego:** <!-- [[05.5 - Nivel XX]], Sala del Trono, Tejado del Castillo -->

---

## 2. Diseño de la Arena de Combate

- **Dimensiones & Forma:** <!-- Circular de 30m, pasarela estrecha, arena con dos alturas -->
- **Peligros Ambientales:** <!-- Caídas al vacío, charcos de fuego, pinchos perimetrales -->
- **Elementos Interactuables / Coberturas:** <!-- Columnas destructibles, palancas, ballestas de apoyo -->
- **Recursos Disponibles en la Arena:** <!-- Cofres con vida, generadores de munición, puntos de escalada -->

---

## 3. Estadísticas Globales

| Parámetro | Valor |
| :--- | :--- |
| **Salud Total (HP)** | `1000` (Distribuido entre fases) |
| **Fases de Combate** | `3 Fases` |
| **Resistencia a Daño** | `Mediana (20% reducción pasiva)` |
| **Inmunidad a Control (CC)**| Inmune a derribo estándar; susceptible a stagger tras esquivar combo |

---

## 4. Fases del Combate

### 4.1 Fase 1: Introducción & Pruebas Básicas (HP: 100% - 70%)
- **Comportamiento General:** <!-- Ataques medidos, distancia controlada, prueba los reflejos básicos del jugador -->
- **Ataques Principales:**
  - *Ataque 1:* <!-- Nombre, animación previa, daño, cómo evitarlo -->
  - *Ataque 2:* <!-- Nombre, animación previa, daño, cómo evitarlo -->
- **Punto Débil / Ventana de Castigo:** <!-- Tras el combo de 3 golpes, queda clavado 2 segundos -->

### 4.2 Fase 2: Escalada de Tensión (HP: 70% - 30%)
- **Transición:** <!-- Breve cinemática in-game o animación de furia / rompe columnas / prende fuego al arma -->
- **Modificaciones de Mecánica:** <!-- Gana velocidad (+25%), invoca esbirros o añade proyectiles -->
- **Nuevos Ataques:**
  - *Ataque Especial 1:* <!-- Ataque de área (AoE) con telegrafiado en el suelo -->
  - *Ataque Especial 2:* <!-- Embestida imparable que debe ser guiada contra columnas -->

### 4.3 Fase 3: Estado Crítico / Enrage (HP: 30% - 0%)
- **Transición:** <!-- La arena se deteriora, música sube de tempo y volumen -->
- **Comportamiento Desesperado:** <!-- Encadena ataques sin apenas descanso; ventanas de vulnerabilidad muy estrechas -->
- **Ataque Definitivo (Ultimate):** <!-- Ataque devastador con telegrafiado largo que requiere interactuar con el entorno -->

---

## 5. Tabla de Telegrafiado & Respuestas Requeridas

| Ataque del Boss | Señal de Advertencia (Telegrafiado) | Daño | Respuesta Correcta del Jugador |
| :--- | :--- | :--- | :--- |
| <!-- Barrido Espada --> | Alza espada con destello amarillo | 30 | Saltar sobre el barrido |
| <!-- Embestida Terrestre --> | Ruge y rasca el suelo con el pie | 60 | Rodar hacia un lado en el último segundo |
| <!-- Golpe Descendente AoE --> | Salta al aire con destello rojo | 80 | Alejarse del círculo marcado en el suelo |

---

## 6. Experiencia de Usuario & Puesta en Escena
- **Cinemática de Entrada:** <!-- Diálogo previo, demostración de fuerza -->
- **Música del Encuentro (OST):** <!-- [[07.2 - Diseño de Audio]], Tema orquestal con percusión intensa -->
- **Cinemática / Remate de Derrota:** <!-- QTE final, escena dramática de caída del boss -->

---

## 7. Recompensas & Desbloqueos
- **Habilidad o Arma Otorgada:** <!-- Ej: Espada de Fuego, Llave de las Catacumbas -->
- **Impacto en el Mundo:** <!-- Acceso a nuevas regiones, cambio de estado de la historia -->
- **Logro Asociado:** <!-- Nombre del trofeo o logro -->
