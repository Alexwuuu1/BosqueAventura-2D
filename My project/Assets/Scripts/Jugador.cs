using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// Controlador principal del jugador para el videojuego 2D de plataformas.
/// Desarrollado por Alexwuuu1 siguiendo la guía técnica oficial.
/// Gestiona movimiento horizontal, salto con detección de suelo, animaciones,
/// recolección de coleccionables y colisiones de riesgo / enemigos.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(Animator))]
public class Jugador : MonoBehaviour
{
    [Header("Parámetros de Movimiento")]
    [Tooltip("Velocidad de desplazamiento horizontal")]
    public float velocidad = 2f;

    [Tooltip("Impulso vertical aplicado al saltar")]
    public float alturaSalto = 4f;

    [Header("Detección de Superficie")]
    [Tooltip("Punto de comprobación ubicado en los pies del personaje")]
    public Transform comprobadorPiso;

    [Tooltip("Radio del círculo de comprobación de suelo")]
    public float radioComprobadorPiso = 0.035f;

    [Tooltip("Capa asignada a las plataformas y pisos")]
    public LayerMask layerPiso;

    [Header("Interfaz de Usuario")]
    [Tooltip("Referencia al componente de texto TextMeshPro para el contador")]
    public TMP_Text textoAbejas;

    // Componentes principales
    private Rigidbody2D _rb2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    // Estado interno del personaje
    private float _movimientoHorizontal;
    private bool _solicitarSalto;
    private int _contadorAbejas;

    /// <summary>
    /// Indica si el personaje se encuentra actualmente apoyado sobre el suelo.
    /// </summary>
    public bool EstaEnPiso { get; private set; }

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ActualizarContadorUI();
    }

    private void Update()
    {
        LeerEntradas();
        ConfigurarAnimaciones();
    }

    private void FixedUpdate()
    {
        ActualizarEstadoSuelo();
        AplicarFisicaMovimiento();
    }

    /// <summary>
    /// Captura el input del usuario tanto para el sistema Legacy como para el nuevo Input System.
    /// </summary>
    private void LeerEntradas()
    {
        float ejeX = 0f;
        bool botonSalto = false;

#if ENABLE_LEGACY_INPUT_MANAGER
        ejeX = Input.GetAxisRaw("Horizontal");
        botonSalto = Input.GetButtonDown("Jump");
#elif ENABLE_INPUT_SYSTEM
        var teclado = Keyboard.current;
        if (teclado != null)
        {
            ejeX = (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed ? 1f : 0f)
                 - (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed ? 1f : 0f);
            botonSalto = teclado.spaceKey.wasPressedThisFrame;
        }
#endif

        _movimientoHorizontal = ejeX;

        // Registrar solicitud de salto únicamente si está en piso
        if (botonSalto && EstaEnPiso)
        {
            _solicitarSalto = true;
        }

        // Voltear sprite según la dirección
        if (_movimientoHorizontal != 0f)
        {
            _spriteRenderer.flipX = _movimientoHorizontal < 0f;
        }
    }

    /// <summary>
    /// Sincroniza los parámetros del Animator Controller según el estado físico.
    /// </summary>
    private void ConfigurarAnimaciones()
    {
        _animator.SetFloat("Velocidad", Mathf.Abs(_movimientoHorizontal));
        _animator.SetFloat("VelocidadVertical", _rb2D.linearVelocity.y);
        _animator.SetBool("estaEnPiso", EstaEnPiso);
    }

    /// <summary>
    /// Valida si el personaje está en contacto con una plataforma en la capa indicada.
    /// </summary>
    private void ActualizarEstadoSuelo()
    {
        EstaEnPiso = comprobadorPiso != null && _rb2D.linearVelocity.y <= 0.1f &&
                     Physics2D.OverlapCircle(comprobadorPiso.position, radioComprobadorPiso, layerPiso);
    }

    /// <summary>
    /// Aplica velocidades físicas calculadas al Rigidbody2D.
    /// </summary>
    private void AplicarFisicaMovimiento()
    {
        float velocidadY = _rb2D.linearVelocity.y;

        if (_solicitarSalto && EstaEnPiso)
        {
            velocidadY = alturaSalto;
            EstaEnPiso = false;
        }

        _solicitarSalto = false;
        _rb2D.linearVelocity = new Vector2(_movimientoHorizontal * velocidad, velocidadY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Recolección de abejas
        if (collision.CompareTag("abejita"))
        {
            Destroy(collision.gameObject);
            _contadorAbejas++;
            ActualizarContadorUI();
            return;
        }

        // 2. Colisión con caracol (daño o pisotón)
        if (collision.CompareTag("caracol"))
        {
            bool impactoDesdeArriba = _rb2D.linearVelocity.y <= 0.1f &&
                GetComponent<CapsuleCollider2D>().bounds.min.y >= collision.bounds.max.y - 0.05f;

            var caracol = collision.GetComponent<Caracol>();
            if (impactoDesdeArriba && caracol != null)
            {
                caracol.Pisar();
                _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x, alturaSalto * 0.7f);
                EstaEnPiso = false;
                return;
            }

            ReiniciarEscena();
            return;
        }

        // 3. Colisión con peligros o jabalí
        if (collision.CompareTag("puerquito"))
        {
            ReiniciarEscena();
        }
    }

    /// <summary>
    /// Actualiza el texto en pantalla con las abejas recolectadas.
    /// </summary>
    public void ActualizarContadorUI()
    {
        if (textoAbejas != null)
        {
            textoAbejas.text = _contadorAbejas.ToString();
        }
    }

    /// <summary>
    /// Reinicia la escena actual ante caídas o contacto con enemigos.
    /// </summary>
    private static void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

