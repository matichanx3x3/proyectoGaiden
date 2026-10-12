---
tipo: ui_pantalla
id: UI-00X
nombre: "{{title}}"
contexto: Menú # [HUD In-Game | Menú Principal | Inventario | Pausa/Opciones | Game Over | Pantalla de Carga]
estado: Borrador
version: "1.0"
fecha_creacion: "{{date}}"
tags:
  - gdd/ui
  - gdd/ux
---

# Pantalla / Menú: {{title}}

> [!NOTE] Propósito del Documento
> Detalla el flujo, componentes visuales, controles y experiencia de usuario (UX) para esta pantalla o elemento de interfaz.

---

## 1. Propósito & Contexto de Uso

- **Nombre:** {{title}}
- **Contexto:** <!-- ¿En qué momento ve el jugador esta pantalla? (Ej: Durante el gameplay activo, al presionar pausa, tras perder toda la vida) -->
- **Objetivo de UX:** <!-- Claridad máxima, no saturar la vista, acceso inmediato en menos de 2 clics -->

---

## 2. Distribución Visual (Mockup / Wireframe)

```
┌────────────────────────────────────────────────────────┐
│ [Logo del Juego]                       [Perfil Jugador]│
│                                                        │
│     ► Continuar Partida                                │
│     ► Nueva Partida                                    │
│     ► Cargar Partida                                   │
│     ► Opciones (Vídeo / Audio / Controles)             │
│     ► Créditos                                         │
│     ► Salir al Escritorio                              │
│                                                        │
│ [Versión v1.0.0]                       [Prompt: Aceptar]│
└────────────────────────────────────────────────────────┘
```

---

## 3. Componentes de Información

### 3.1 Elementos Fijos
- <!-- Título del juego, fondo ambiental animado, número de versión -->

### 3.2 Elementos Dinámicos / Temporales
- <!-- Indicador de carga de guardados, aviso de autoguardado en segundo plano -->

---

## 4. Navegación & Mapeo de Controles

| Input | Acción |
| :--- | :--- |
| **D-Pad Arriba / Abajo o Stick** | Mover cursor entre opciones de la lista |
| **Botón Confirmar (`⨉` / `A` / Enter)** | Seleccionar y entrar en la opción |
| **Botón Cancelar (`○` / `B` / Esc)** | Volver a la pantalla anterior |
| **R1 / L1 (o `Q` / `E`)** | Cambiar entre pestañas en caso de submenús |

---

## 5. Feedback Audiovisual & Transiciones

- **SFX al navegar:** <!-- Clic metálico suave -->
- **SFX al confirmar:** <!-- Sonido enérgico de desenvaine -->
- **SFX al cancelar / error:** <!-- Sonido sordo o zumbido -->
- **Transición:** <!-- Fundido a negro (Screen Fader) de 0.3 segundos -->

---

## 6. Directrices de Accesibilidad
- [ ] Soporte para modo de alto contraste.
- [ ] Tamaño de fuente escalable o legible a 3 metros en pantallas de televisión.
- [ ] Posibilidad de reasignar todos los controles.
