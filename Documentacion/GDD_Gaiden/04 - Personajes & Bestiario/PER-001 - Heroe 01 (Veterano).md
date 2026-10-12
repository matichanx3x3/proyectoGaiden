---
tipo: personaje_jugable
id: PER-001
nombre: Héroe 01 (El Veterano)
rol: Líder Táctico / Armas Pesadas
estado: Aprobado
version: "2.0"
tags:
  - gdd/personaje
  - gdd/jugador
  - gdd/veterano
---

# Ficha de Personaje: Héroe 01 (El Veterano)

> [!NOTE] Propósito del Documento
> Especificación de diseño del protagonista principal de Proyecto Gaiden: perfil táctico, motivaciones, estadísticas y sensaciones de control.

---

## 1. Identidad & Perfil General

| Campo | Valor / Descripción |
| :--- | :--- |
| **Identificador / ID** | `PER-001` (Vinculado a `HeroId.Hero_01` en motor) |
| **Rol en el Equipo** | Líder de misión / Especialista en armamento pesado y contención |
| **Constitución Física**| Robusta y pesada (1.88 m / 92 kg) |
| **Puntos de Vida (HP)**| **100 HP** |
| **Armamento Inicial** | Pistola 9mm reglamentaria + Cuchillo de combate |
| **Especialidad Táctica**| Manejo de escopeta, alta resistencia al retroceso y potencia física |

### 1.1 Motivación Principal
Comanda la incursión en la instalación marítima aislada tras perderse las comunicaciones con el equipo de avanzada. Su objetivo prioritario es extraer la información del experimento biológico y rescatar a los supervivientes.

---

## 2. Sensaciones de Control & Game Feel

- **Inercia y Masa:** Presenta pasos firmes y un sonido contundente al desplazarse sobre rejillas metálicas.
- **Empuje de Objetos:** Es el personaje más eficiente moviendo cajas pesadas (`PushableBox`), manteniendo un ritmo uniforme sin perder estabilidad.
- **Retículo de Combate:** Su cadencia de tiro con armas de gran calibre (Escopeta, Lanzagranadas) es sólida, absorbiendo el culatazo con menor balanceo.

---

## 3. Métricas Numéricas en Motor
- **Puntos de Vida Base:** `100 HP`
- **Velocidad al Caminar:** `3.5 m/s`
- **Velocidad de Sprint:** `5.5 m/s`
- **Velocidad Arrastrando Cajas:** `2.0 m/s`
- **Radio de Colisión:** `0.45 m`
