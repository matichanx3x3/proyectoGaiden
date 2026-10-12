---
tipo: dashboard_moc
proyecto: Proyecto Gaiden
version: "2.1"
tags:
  - gdd/moc
  - gdd/indice
---

# 🗺️ Índice General del GDD (Dashboard MOC)

> [!NOTE] Bóveda Oficial de Diseño de Videojuegos (GDD)
> Esta bóveda de Obsidian documenta la visión completa de **Game Design, mecánicas de juego, personajes, narrativa, niveles y producción** para **Proyecto Gaiden**.
> 
> 💡 **Inspiración del Proyecto:** Proyecto Gaiden toma como **inspiración mecánica y de sistemas** las ideas clásicas de *Resident Evil Gaiden* (el combate aditivo de retículo oscilante, la gestión de un grupo de 3 supervivientes y la tensión de supervivencia y puzles espaciales), desarrolladas sobre una identidad y arquitectura original propia en Unity.
> 
> ⚠️ **Separación de Responsabilidades:** Toda la arquitectura de software, C#, componentes de motor y diagramas técnicos residen en `Doc Tecnica/`.

---

## 🧭 Módulos de Diseño del GDD

### 📂 00 - Visión & Concepto General
- [[00.1 - Resumen Ejecutivo & Game Concept]] — High concept, survival horror táctico y propuesta de valor.
- [[00.2 - Pilares de Diseño, Aesthetics & USP]] — Tensión del retículo oscilante, puzles de entorno y marco MDA.
- [[00.3 - Público Objetivo & Taxonomía de Bartle]] — Demografía y clasificación de perfiles de jugador.
- [[00.4 - Plataformas & Requisitos Técnicos]] — Requisitos mínimos, recomendados y target de 60 FPS.
- [[00.5 - Referencias & Benchmarks]] — Inspiración en títulos de timing y clásicos de supervivencia.

### 📂 01 - Narrativa & Lore
- [[01.1 - Sinopsis & Estructura Argumental]] — Actos I, II, III: infiltración, brote biológico y escape marítimo.
- [[01.2 - Lore, Reglas del Universo & Setting]] — Reglas del virus mutagénico y escasez de recursos.
- [[01.3 - Regiones & Localizaciones]] — Complejo de investigación en altamar y módulos de contención.
- [[01.4 - Facciones & Grupos del Mundo]] — Equipo táctico de rescate vs. aberraciones mutadas.

### 📂 02 - Jugabilidad & Game Loop
- [[02.1 - Core Game Loop & Dinámicas Clave]] — Bucle de exploración 3D, tensión de puertas, retículo y puzles.
- [[02.2 - Modos de Juego & Estructura]] — Campaña principal y gestión del Party System.
- [[02.3 - Motivación del Jugador (Problema-Resolución-Recompensa)]] — Matriz sistemática de Toulouse Lautrec aplicada al juego.
- [[02.4 - Mapeo de Controles & Esquema de Entrada]] — Esquema unificado para Gamepad (PS/Xbox) y Teclado+Ratón.

### 📂 03 - Mecánicas & Sistemas
- [[03.1 - Métricas del Personaje & Locomoción]] — Perspectiva cenital 55°, velocidades, CharacterController y casos límite.
- [[03.2 - Sistema de Combate, Armas & Habilidades]] — Retículo oscilante (0..120 px, 3 px/frame), dianas críticas, turn timers y arsenal.
- [[03.3 - Salud, Estados Alterados, Muerte & Checkpoints]] — HP del grupo, envenenamiento y conmutación de héroes.
- [[03.4 - Interactuables de Entorno, Puzles & Obstáculos]] — Puertas de dos fases, arrastre de cajas, sockets azul/rojo y escaleras.
- [[03.5 - Economía, Tiendas & Recompensas]] — Racionamiento estricto de munición y suministros de supervivencia.
- [[03.6 - Inventario, Consumibles & Coleccionables]] — Maletín de 8 casillas, hierbas verde/roja/mixta, tarjetas de acceso y DropItem.

### 📂 04 - Personajes & Bestiario
- [[04.1 - Índice de Personajes Jugables (Héroes)]] — Sistema de grupo de supervivientes:
  - 👤 [[PER-001 - Heroe 01 (Veterano)]] (100 HP, Armas Pesadas, Líder)
  - 👤 [[PER-002 - Heroe 02 (Explorador Tactico)]] (100 HP, Infiltrador Ágil)
  - 👤 [[PER-003 - Heroe 03 (Especialista Biologica)]] (120 HP, Resistencia Celular)
- [[04.2 - Índice del Bestiario (Enemigos Comunes)]] — Arquetipos de criaturas:
  - 🧟 [[ENE-001 - Zombie Masculino]] (Infectado Estándar, 40-45 HP)
  - 🐕 [[ENE-002 - Acechador Agil]] (Acechador Rápido, 35 HP, 120 frames)
  - 🧪 [[ENE-004 - Zombie Venenoso]] (Espécimen Tóxico, 55 HP, veneno continuo)
- [[04.3 - Índice de Jefes Finales (Bosses)]] — Aberraciones mayores:
  - 👹 [[BOSS-001 - Organismo Alfa (Prototipo BOW)]] (350 HP, 3 fases progresivas)
- [[04.4 - Guía de IA de Enemigos & Árbol de Comportamiento]] — Diagrama sensorial Guardia -> Alerta -> Ataque.
- [[04.5 - Índice de NPCs & Aliados]] — Supervivientes y personal científico.

### 📂 05 - Diseño de Niveles (Level Design)
- [[05.1 - Filosofía de Level Design & Directrices]] — Regla Teach/Practice/Master, claustrofobia, claroscuro y guiding.
- [[05.2 - Estructura de Progresión & Hub Central]] — Navegación de la instalación y camarotes seguros.
- [[05.3 - Curvas de Interés, Ritmo & Tensión]] — Pacing entre valles de puzles y picos de emboscada.
- [[05.4 - Simbología & Leyenda Oficial de Layouts]] — Símbolos estándar para planos (Entrada, Cajas, Puertas, Sockets).
- [[05.5 - Índice de Niveles del Proyecto]] — Catálogo oficial de sectores y asignación cromática subtextual (00 a 05).
  - ⭐ [[05.5 - Nivel 00 - Pruebas Funcionales & Mecánicas]] — Nivel de validación con puzle de cajas, socket rojo y llave.

### 📂 06 - UI, UX & Cámara
- [[06.1 - Sistema de Cámara & Modos Dinámicos]] — Cámara cenital 3D suave (`SmoothDamp`) y look-ahead.
- [[06.2 - Especificación de HUD (In-Game)]] — Retículo oscilante en combate y HUD de exploración mínimo.
- [[06.3 - Diagrama de Flujo de Pantallas (Game Flow)]] — Navegación completa desde splash hasta victoria.
- [[06.4 - Menús de Front-End, Opciones & Accesibilidad]] — Opciones de remapeo, subtítulos y contraste.
- [[06.5 - Pantallas de Inventario, Carga & Game Over]] — Diseño del maletín de 8 casillas y Game Over.

### 📂 07 - Arte, Audio & Cinemáticas
- [[07.1 - Dirección de Arte, Paleta & Atributos Estéticos]] — Iluminación simbólica de claroscuros y sistema cromático de salud mental (5 trastornos).
- [[07.2 - Diseño de Audio, Paleta Sonora & Banda Sonora (OST)]] — Audio diegético ambiental y OST adaptativa.
- [[07.3 - Guión de Cinemáticas, Storyboards & Triggers]] — Encuentros con aliados y aparición del Organismo Alfa.


### 📂 08 - Producción, Tech & Legal
- [[08.1 - Stack Técnico & Arquitectura del Motor (Unity)]] — Unity 6, URP, Input System y Cinemachine.
- [[08.2 - Cronograma de Producción & Fases (Milestones)]] — Hitos de Preproducción a Gold Master.
- [[08.3 - Plan de Localización & Mercados]] — Plan de idiomas para Europa, América y Asia.
- [[08.4 - Licencias, Copyright & Aspectos Legales]] — Propiedad intelectual y atribución.
- [[08.5 - Apéndices & Listados Maestros de Assets]] — Listado de modelos, animaciones y SFX.

---

## 🛠️ Catálogo de Plantillas Oficiales (`_Templates/`)
- [[_Templates/Template - Ficha de Personaje (Jugable)]]
- [[_Templates/Template - Ficha de Enemigo & Comportamiento IA]]
- [[_Templates/Template - Ficha de Jefe Final (Boss)]]
- [[_Templates/Template - Ficha de NPC]]
- [[_Templates/Template - Diseño de Nivel (Level Design & Layout)]]
- [[_Templates/Template - Mecánica o Sistema]]
- [[_Templates/Template - Puzle o Interactuable de Entorno]]
- [[_Templates/Template - Objeto, Arma o Power-Up]]
- [[_Templates/Template - Pantalla o Menú UI]]
- [[_Templates/Template - Documento de Revisión & Registro de Versión]]
