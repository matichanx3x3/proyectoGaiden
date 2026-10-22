---
tipo: enemigo
id: ENE-001
nombre: El Intruso (Pensamiento Intrusivo)
categoria: Enemigo Común Transversal
estado: Aprobado
version: "3.0"
tags:
  - gdd/enemigo
  - gdd/intruso
  - gdd/transversal
---

# Ficha de Enemigo: El Intruso (`ENE-001`)

> [!NOTE] Rol de Diseño & Simbolismo Psicológico
> **Enemigo básico universal presente en todos los sectores de Amnesis.** 
> Encarna la **ansiedad basal y el pensamiento intrusivo**. Sirve como el adversario de patrulla fundamental y la piedra angular para calibrar el timing de la aguja oscilante en la barra de sincronización.
> 
> 💡 **Adaptabilidad Estética:** No existe un único modelo estático; su textura, paleta de iluminación y sonido mutan orgánicamente según la atmósfera del sector que ocupe.

---

## 1. Parámetros del Retículo & Estadísticas Globales

- **Tipo en Motor:** `EnemyType.ZombieStandard`
- **Puntos de Vida (HP):** `45 HP` (Muere con 1 impacto crítico de escopeta o 2 impactos normales de pistola).
- **Poder de Ataque:** `8 HP` por impacto contuso.
- **Ancho Zona de Impacto (Hit Half-Width):** `18 px` (Zona amplia, ideal para calibración rítmica).
- **Ancho Diana Crítica (Crit Half-Width):** `4 px`.
- **Intervalo de Ataque:** `180 frames` (~3.0 segundos).
- **Consumo de Estamina al Bloquear:** `20 STA` (Fácilmente mitigable con guardia).
- **Reacción a la Esquiva Táctica:** Queda desbalanceado durante `1.5 segundos`, permitiendo un tiro crítico garantizado.

---

## 2. Variaciones Estéticas y Sensoriales por Sector

| Sector | Denominación Variante | Paleta Cromática | Textura & Deformidad | Efecto Sonoro Característico |
| :---: | :--- | :--- | :--- | :--- |
| **01 (Distimia)** | *Intruso Apático* | Azul noche (`#162238`), gris plúmbeo y negro | Ropa empapada, hombros vencidos, piel cenicienta | Goteo sordo y respiración asmática lenta |
| **02 (TLP)** | *Intruso Volátil* | Naranja fuego (`#F77F00`) y turquesa/teal | Piel agrietada con vetas fluorescentes y espasmos | Crujido eléctrico y chirrido de metal forzado |
| **03 (Bipolar)** | *Intruso Polar* | Rojo rubí (`#E63946`) vs Azul profundo | Mitad del cuerpo incandescente, mitad congelada | Chasquido de chispas y vaho gélido constante |
| **04 (TAC)** | *Intruso Voraz* | Azul pervinca (`#7D8CC4`) y gris ceniza | Torso hipertrofiado, mandíbula desencajada | Deglución húmeda y estertor estomacal pesado |
| **05 (TID)** | *Intruso Fracturado* | Prisma multicolor (rosa, amarillo, turquesa) | Silueta translúcida con aberración cromática | Ruido blanco de sintonizador de radio analógico |

---

## 3. Enlaces Relacionados
- [[04.2 - Índice del Bestiario (Enemigos Comunes)]]
- [[03.2 - Sistema de Combate, Armas & Habilidades]]
