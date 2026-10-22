# Game Brief: Amnesis

**One-sentence pitch:** Un viaje de horror introspectivo donde el claroscuro y la precisión de tiro determinan la supervivencia frente a una mente fragmentada.

## Core loop
- Explorar sectores laberínticos en perspectiva top-down 2.5D evitando o emboscando entidades hostiles y resolviendo puzles de entorno.
- Entrar en combate en primera persona al contacto: medir el tempo con la barra oscilante de puntería para disparar, defenderse o gestionar recursos.
- Obtener suministros (llaves, cartuchos, transmisiones de radio) y estabilizar la cordura para desbloquear nuevos sectores cromáticos.

## Player goal & fail state — what "working" looks like
- **Objetivo:** Descifrar la identidad de la voz de la radio y purgar las manifestaciones que custodian cada sector cromático para escapar de la instalación mental.
- **Estado de fallo:** La salud o la estabilidad mental cae a cero al ser abrumado por las entidades proyectadas; Game Over y reinicio en el último punto seguro.

## MVP — what must exist to be the game
- Control del jugador en exploración 2.5D (top-down 55º con colisiones y linterna direccional).
- Sistema de iluminación cromática y claroscuro en URP (zonas de penumbra opresiva y focos simbólicos de color).
- Puzles básicos de entorno e interactuables (puertas cerradas, llaves, terminales/notas de radio).
- Transición fluida de exploración cenital a pantalla de combate en primera persona.
- Sistema de combate con barra oscilante (hitboxes de puntería dependientes del arma equipada).
- Una entidad enemiga funcional con diseño y comportamiento reactivo según su patología asociada (influencia de *Silent Hill / The Evil Within*).
- Ciclo de inventario básico (salud, munición, selector de armas) y pantalla de game over / victoria.

## Out of scope — not building this
- Modo multijugador o cooperativo.
- Múltiples personajes jugables en el primer ciclo (solo el Investigador).
- Doblaje de voz completo (narración por subtítulos/texto con ruidos y estática de radio analógica).
- Bestiario extenso de enemigos (> 2 tipos en MVP).

## Build order
1. Combate en 1ª persona con barra oscilante (el riesgo/núcleo mecánico diferenciador de Gaiden).
2. Exploración top-down y linterna con sombras en URP.
3. Transición e integración exploración-combate con una entidad básica.
4. Puzles e interactuables de puertas/llaves y transmisiones de radio.
5. Loop de fin de partida (vida, daño, game over y resolución del sector).

---
**Who it's for / what they feel:** Fans del survival horror psicológico y RPG táctico que buscan tensión calculada, claustrofobia constante y atmósfera opresiva (*Signalis*, *Silent Hill*).

**Art & audio direction:** Claroscuro expresionista en URP con códigos cromáticos simbólicos por patología, atmósfera claustrofóbica y analógica degradada (*Signalis*, *Mouthwashing*) y criaturas grotescas de lenguaje corporal traumático (*Silent Hill*, *The Evil Within*).

**Reference games:** *Resident Evil Gaiden* (núcleo mecánico de exploración y combate con barra oscilante), *Signalis* y *Mouthwashing* (tono, aislamiento y opresión analógica), *Silent Hill* y *The Evil Within* (simbolismo psicológico de las criaturas y distorsión de la realidad).
