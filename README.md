# BosqueAventura 2D - Videojuego de Plataformas en Unity

Proyecto individual desarrollado por **Alexwuuu1** siguiendo la guía técnica oficial *"Creando un videojuego 2D con Unity"* del Ing. Rubén Gonzalo Soria Soria (2026).

---

## 📋 Información General

- **Desarrollador:** Alexwuuu1
- **Motor:** Unity 6000.3.16f1 (Plantilla Universal 2D)
- **Plataforma Objetivo:** Escritorio (PC / Windows)
- **Recursos Gráficos:** *Legacy Fantasy - High Forest 2.0 / 2.3* y *Legacy Fantasy - Debug Map* por Anokolisa

---

## 🎮 Controles del Jugador

| Acción | Teclas Primarias | Teclas Secundarias |
|---|---|---|
| Moverse a la Izquierda | `A` | `Flecha Izquierda` |
| Moverse a la Derecha | `D` | `Flecha Derecha` |
| Saltar | `Espacio` | Barra Espaciadora |

---

## 🌟 Mecánicas Implementadas (Rúbrica de Evaluación)

### 1. Sistema de Animaciones (40%)
- **Reposo (Idle):** Animación en bucle al permanecer inmóvil.
- **Correr (Run):** Transición automática al detectar movimiento horizontal (`Velocidad > 0.05`).
- **Salto y Caída (Jump / Jump-End):** Transiciones condicionadas por velocidad vertical y detección de contacto con la superficie (`estaEnPiso`).

### 2. Coleccionables con Contador UI (20%)
- Abejas distribuidas estratégicamente a lo largo de las plataformas.
- Detección precisa de recolección (`OnTriggerEnter2D`) con destrucción instantánea del objeto coleccionado.
- Contador en pantalla vinculado en tiempo real con TextMeshPro en el Canvas.

### 3. Cámara de Seguimiento (20%)
- Script `Camara.cs` que sigue en tiempo real la posición del jugador mediante interpolación en `LateUpdate`.
- Sistema de fondo dinámico (`FondoCamara.cs`) para mantener la inmersión visual.

### 4. Enemigos, Peligros y Reinicio
- **Jabalí:** Obstáculo dañino en el suelo; el contacto reinicia la escena.
- **Caracol:** Enemigo interactivo que puede ser derrotado mediante pisotón superior (aplicando impulso de rebote al personaje); el contacto lateral reinicia el nivel.
- **Límite de Caída:** Trigger inferior para reiniciar la partida si el jugador cae al vacío.

---

## 📂 Estructura del Proyecto

- `My project/Assets/Scripts`: Scripts en C# (`Jugador.cs`, `Camara.cs`, `Caracol.cs`, `FondoCamara.cs`).
- `My project/Assets/Animaciones`: Controladores y clips de animación exportados.
- `My project/Assets/Scenes`: Escena principal `NivelBosque.unity`.
- `My project/Assets/Tiles`: Paleta de tiles y reglas de Tilemap.
- `docs/SEGUIMIENTO_PDF.md`: Bitácora técnica del seguimiento página a página del PDF.

---

## 🚀 Instrucciones de Ejecución

1. Abrir **Unity Hub**.
2. Hacer clic en **Add > Add project from disk**.
3. Seleccionar la carpeta `My project` contenida en este repositorio.
4. Abrir la escena `Assets/Scenes/NivelBosque.unity`.
5. Presionar el botón **Play** en la barra superior de Unity.
