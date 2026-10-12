---
tipo: objeto
id: OBJ-00X
nombre: "{{title}}"
categoria: Consumible # [Arma Melee | Arma Rango | Power-Up Temporal | Consumible | Coleccionable]
rareza: Comun # [Comun | Especial | Raro | Legendario]
estado: Borrador
version: "1.0"
fecha_creacion: "{{date}}"
tags:
  - gdd/objeto
  - gdd/item
---

# Objeto / Recurso: {{title}}

> [!NOTE] Propósito del Documento
> Documenta armas, consumibles, power-ups y coleccionables: atributos, duración, impacto en el gameplay y economía.

---

## 1. Descripción & Representación Visual

- **Nombre del Objeto:** {{title}}
- **Categoría:** <!-- Power-up temporal (+poder, inmunidad), Arma alternativa, Botiquín de salud, Coleccionable secreto -->
- **Icono / Aspecto:** <!-- Descripción visual del ítem 3D en el mundo y su icono en el HUD/Inventario -->

---

## 2. Atributos & Efectos en el Juego

| Atributo | Valor | Descripción |
| :--- | :--- | :--- |
| **Efecto Primario** | <!-- +20 de daño / Inmunidad total / +50 HP --> | Modificador aplicado al jugador |
| **Duración del Efecto** | <!-- Instantáneo / 10 seg / 30 seg / Permanente --> | Tiempo de persistencia |
| **Capacidad Máxima (Stack)**| <!-- 1 / 5 / 99 / N/A --> | Cantidad máxima almacenable en inventario |
| **Valor de Compra** | <!-- 100 Monedas --> | Precio en tiendas de NPCs |
| **Valor de Venta** | <!-- 50 Monedas --> | Recompensa al venderlo |

---

## 3. Modificaciones en el Comportamiento del Jugador
<!-- ¿Cómo altera el control o la estrategia del jugador mientras está activo? (Ej: Si tiene inmunidad, puede correr a través de trampas de fuego sin esquivar) -->

---

## 4. Distribución & Obtención
- **Localizaciones:** <!-- Cofres secretos en Nivel 1 y Nivel 4; drop raro de Soldado con Escudo -->
- **Requisitos de Desbloqueo:** <!-- Si es coleccionable: Desbloquea galería de arte o modo turbo al juntar todas -->
