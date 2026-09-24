# Bitácora Técnica de Desarrollo y Seguimiento del PDF

**Estudiante:** Alexwuuu1  
**Referencia:** *Creando un videojuego 2D con Unity*, Ing. Rubén Gonzalo Soria Soria, 2026, 144 páginas.  

El proyecto implementa fielmente la estética, recursos y mecánicas detalladas en la guía oficial. Se utiliza Unity 6000.3.16f1 y los paquetes originales `Legacy-Fantasy - High Forest 2.3` y `Legacy Fantasy - Debug Map` de Anokolisa.


## Etapa 1: proyecto base

- Páginas 1-3: proyecto existente Universal 2D.
- Git en la raíz del repositorio. Proyecto Unity dentro de `My project`.
- Assets, sus `.meta`, Packages y ProjectSettings versionados. Library, Temp, Logs y builds excluidos.

## Etapa 2: escenario y jugador (páginas 4-107)

- Paquetes originales en `Assets/Sprites`.
- Personaje Idle y Background procedentes de los prefabs importados de Aseprite.
- Cámara ortográfica Size 1.5. Background detrás; jugador con Order in Layer 2.
- Entrada Both; teclado A/D o flechas y espacio para saltar.
- Tiles.png recortado a 16×16, Point, 100 píxeles por unidad; Grid de 0.16×0.16.
- Piso con TilemapCollider2D y capa `Pisito`. Paleta editable en `Assets/Paletas`.
- Rigidbody2D continuo, interpolado, rotación Z bloqueada. CapsuleCollider2D y material `solido` con fricción y rebote cero.
- Script `Jugador`, comprobador de suelo y prefab reutilizable.
- Animaciones originales exportadas a clips independientes; `PjController` con Idle, Run, Jump y Jump-down.
- Parámetros exactos: `Velocidad`, `VelocidadVertical`, `estaEnPiso`.
- Script `Camara` con seguimiento en LateUpdate.

### Correcciones respecto de los fragmentos del PDF

1. Leer las teclas en Update y aplicar la velocidad en FixedUpdate. El salto pendiente se consume una sola vez.
2. Comprobar suelo al descender o estar detenido para impedir dobles saltos durante el despegue.
3. Usar SpriteRenderer.flipX para orientar el dibujo sin invertir el collider ni el comprobador de suelo.
4. Ajustar el collider y el comprobador al pivote inferior real del prefab Aseprite.
5. Mantener un radio pequeño de comprobación para el tamaño del personaje.
6. Unificar `jump-end` / `jump-down`: el estado Jump-down utiliza el clip original Jump-End.
7. Activar caída también al abandonar una plataforma; desactivar Can Transition To Self en Any State para no reiniciar cada fotograma.
8. Incluir respaldo para el Input System de la plantilla mientras se aplica Both.

## Etapa 3: interacciones (páginas 108-143)

- Tres abejas del asset original funcionan como coleccionables y actualizan un contador TextMeshPro anclado a la esquina superior izquierda.
- Un jabalí del asset original y un trigger ancho bajo el nivel reinician `NivelBosque` al tocarlos.
- El caracol usa el asset original y un trigger. El jugador lo derrota si llega desde arriba mientras desciende; rebota y el caracol se elimina al terminar su animación. Un contacto lateral reinicia el nivel.
- Las plataformas, abejas y caracol están ubicados a alturas y distancias alcanzables con el salto configurado.

El PDF termina en la preparación del caracol (pág. 143) y no define su interacción. El pisotón y el rebote son una extensión acotada para completar el evento anunciado al final de la guía.

## Alcance

Solo escritorio, como solicitó el usuario. Android no forma parte del proyecto. No se implementan controles táctiles, configuración, menús, vidas, combate con espada ni otros sistemas ajenos a la guía.

## Verificación

Las herramientas de `Assets/Editor` permiten montar la escena y comprobar física y animaciones dentro de Unity. Solo ejecutan órdenes locales explícitas en `Library/PdfCommand.txt`; no forman parte del ejecutable del juego. Los informes temporales y capturas se guardan en `artifacts`, fuera del control de versiones.
