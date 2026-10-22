---
tipo: dashboard_moc
proyecto: Amnesis
version: "3.0"
tags:
  - gdd/moc
  - gdd/indice
---

# 🗺️ Índice General del GDD: Amnesis (Dashboard MOC)

> [!NOTE] Bóveda Oficial de Diseño de Videojuegos (GDD)
> Esta bóveda de Obsidian documenta la visión completa de **Game Design, mecánicas de juego, progresión del personaje, narrativa, bestiario, niveles y producción** para **Amnesis**.
> 
> 💡 **Identidad e Influencias:** Amnesis toma como **inspiración mecánica nuclear** el combate rítmico en primera persona con barra oscilante y la tensión de supervivencia de *Resident Evil Gaiden*, combinados con el aislamiento analógico y opresión psicológica de *Signalis* y *Mouthwashing*, y un diseño simbólico de criaturas inspirado en *Silent Hill* y *The Evil Within*.
> 
> 🧠 **Salud Mental Subtextual & Claroscuro:** La temática de trastornos mentales se canaliza mediante iluminación de claroscuro (códigos cromáticos por sector) y el lenguaje corporal de las aberraciones, sin diagnósticos explícitos en diálogos.
> 
> ⚠️ **Separación de Responsabilidades:** Toda la arquitectura técnica de software, C#, componentes de motor y diagramas residen en `Doc Tecnica/`.

---

## 🧭 Módulos de Diseño del GDD

### 📂 00 - Visión & Concepto General
- [[00.1 - Resumen Ejecutivo & Game Concept]] — High concept, survival horror psicológico, pitch y logline.
- [[00.2 - Pilares de Diseño, Aesthetics & USP]] — Precisión rítmica, claroscuro cromático, progresión solitaria y marco MDA.
- [[00.3 - Público Objetivo & Taxonomía de Bartle]] — Demografía y clasificación de perfiles de jugador.
- [[00.4 - Plataformas & Requisitos Técnicos]] — Requisitos mínimos, recomendados y target de 60 FPS en PC.
- [[00.5 - Referencias & Benchmarks]] — Inspiración en *Resident Evil Gaiden*, *Signalis*, *Mouthwashing*, *Silent Hill*.

### 📂 01 - Narrativa & Lore
- [[01.1 - Sinopsis & Estructura Argumental]] — Actos I, II, III: despertar en penumbra, fractura cromática e integración disociativa.
- [[01.2 - Lore, Reglas del Universo & Setting]] — Instalación cognitiva analógica, reglas del claroscuro y radio diegética.
- [[01.3 - Regiones & Localizaciones]] — Complejo de investigación y sectores cromáticos.
- [[01.4 - Facciones & Grupos del Mundo]] — El Investigador, la voz de la radio y los constructos psíquicos.

### 📂 02 - Jugabilidad & Game Loop
- [[02.1 - Core Game Loop & Dinámicas Clave]] — Bucle de exploración cenital 2.5D, tensión de puertas, retículo rítmico y puzles.
- [[02.2 - Modos de Juego & Estructura]] — Campaña principal individual del Investigador.
- [[02.3 - Motivación del Jugador (Problema-Resolución-Recompensa)]] — Dinámica de avance, recursos y catarsis.
- [[02.4 - Mapeo de Controles & Esquema de Entrada]] — Esquema unificado para Gamepad y Teclado+Ratón.

### 📂 03 - Mecánicas & Sistemas
- [[03.1 - Métricas del Personaje & Locomoción]] — Perspectiva cenital 55°, velocidades, estamina y CharacterController.
- [[03.2 - Sistema de Combate, Armas & Habilidades]] — Retículo oscilante (0..120 px, 3 px/frame), bloqueo/esquiva con estamina, catálogo y 6 vías de mejora por arma.
- [[03.3 - Salud, Estados Alterados, Muerte & Checkpoints]] — HP, Defensa, Estamina, intoxicación y puntos seguros de descanso.
- [[03.4 - Interactuables de Entorno, Puzles & Obstáculos]] — Puertas de dos fases, arrastre de cajas, terminales y cables.
- [[03.5 - Economía, Tiendas & Recompensas]] — Racionamiento estricto de munición, chatarra y componentes de memoria.
- [[03.6 - Inventario, Consumibles & Coleccionables]] — Maletín de casillas, estimulantes, llaves y frecuencias de radio.

### 📂 04 - Personajes & Bestiario
- [[04.1 - Índice de Personajes Jugables (Héroes)]] — El Investigador (personaje solitario) y árbol de atributos (Vida, Defensa, Estamina).
  - 👤 [[PER-001 - Heroe 01 (Veterano)]] ➔ [[PER-001 - El Investigador]] (Protagonista único).
- [[04.2 - Índice del Bestiario (Enemigos Comunes)]] — Arquetipos transversales y síntomas por patología:
  - 🌐 [[ENE-001 - Zombie Masculino]] ➔ **El Intruso** (Enemigo transversal: Pensamiento Intrusivo / Ansiedad, con variaciones visuales en los 5 sectores).
  - ⚡ [[ENE-002 - Acechador Agil]] ➔ **El Latente** (Síntoma TLP: Hiperreactividad e ira, Sector 02).
  - 🧠 [[ENE-003 - El Desmemoriado]] ➔ **El Desmemoriado** (Síntoma TID: Amnesia disociativa y reseteo por esquiva, Sector 05).
  - 🌫️ [[ENE-004 - Zombie Venenoso]] ➔ **El Miasmático** (Síntoma TAC: Culpa, asfixia y drenaje de estamina, Sector 04).
- [[04.3 - Índice de Jefes Finales (Bosses)]] — Jefes mayores por patología:
  - 🌑 `BOSS-002` **El Vacío Inerte** (Sector 01: Distimia / Depresión Crónica).
  - ⚡ `BOSS-003` **La Fractura Límite** (Sector 02: Trastorno Límite / TLP).
  - ⚖️ `BOSS-004` **El Péndulo Polar** (Sector 03: Trastorno Bipolar).
  - 👄 `BOSS-005` **La Fauce Voraz** (Sector 04: Trastorno Alimentario / TAC).
  - 👁️ [[BOSS-001 - Organismo Alfa (Prototipo BOW)]] ➔ **Constructo Traumático / El Prisma** (Sector 05: Clímax TID).
- [[04.4 - Guía de IA de Enemigos & Árbol de Comportamiento]] — Sensores y patrones de agresión reactiva.
- [[04.5 - Índice de NPCs & Aliados]] — Registros de la voz de la radio y notas dejadas por sujetos anteriores.

### 📂 05 - Diseño de Niveles (Level Design)
- [[05.1 - Filosofía de Level Design & Directrices]] — Regla Teach/Practice/Master, claustrofobia, claroscuro y guiding.
- [[05.2 - Estructura de Progresión & Hub Central]] — La Estación de Monitoreo Analógico (terminales, radio, banco de armas).
- [[05.3 - Curvas de Interés, Ritmo & Tensión]] — Pacing entre valles de puzles y picos de emboscada.
- [[05.4 - Simbología & Leyenda Oficial de Layouts]] — Símbolos estándar para planos (Entrada, Cajas, Puertas, Terminales).
- [[05.5 - Índice de Niveles del Proyecto]] — Mapeo oficial de los 5 sectores cromáticos (Distimia, TLP, Bipolar, TAC, TID).
  - ⭐ [[05.5 - Nivel 00 - Pruebas Funcionales & Mecánicas]] — Validación con puzle de cajas, socket rojo y llave.

### 📂 06 - UI, UX & Cámara
- [[06.1 - Sistema de Cámara & Modos Dinámicos]] — Cámara cenital 3D suave (`SmoothDamp`) y cambio a 1ª persona en combate.
- [[06.2 - Especificación de HUD (In-Game)]] — Barra oscilante en combate y HUD de exploración mínimo.
- [[06.3 - Diagrama de Flujo de Pantallas (Game Flow)]] — Navegación completa desde splash hasta victoria.
- [[06.4 - Menús de Front-End, Opciones & Accesibilidad]] — Opciones de remapeo, subtítulos y contraste.
- [[06.5 - Pantallas de Inventario, Carga & Game Over]] — Diseño del maletín de casillas y pantalla de colapso mental.

### 📂 07 - Arte, Audio & Cinemáticas
- [[07.1 - Dirección de Arte, Paleta & Atributos Estéticos]] — Iluminación simbólica de claroscuros y sistema cromático de salud mental (Distimia, TLP, Bipolar, TAC, TID).
- [[07.2 - Diseño de Audio, Paleta Sonora & Banda Sonora (OST)]] — Estática de radio analógica, ruidos secos de maquinaria y frecuencias graves.
- [[07.3 - Guión de Cinemáticas, Storyboards & Triggers]] — Transiciones y eventos de sintonía por radio.
