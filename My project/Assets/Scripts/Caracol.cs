using System.Collections;
using UnityEngine;

/// <summary>
/// Comportamiento del enemigo Caracol.
/// Desarrollado por Alexwuuu1.
/// Permite ser derrotado cuando el jugador lo pisa desde arriba.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Caracol : MonoBehaviour
{
    [Header("Animación y Tiempos")]
    [SerializeField] private Animator animator;
    [SerializeField] private float esperaDesaparicion = 0.55f;

    private bool _derrotado;

    public void Pisar()
    {
        if (_derrotado) return;
        _derrotado = true;

        GetComponent<Collider2D>().enabled = false;
        if (animator != null)
        {
            animator.Play("Aplastado", 0, 0);
        }

        StartCoroutine(RutinaDesaparicion());
    }

    private IEnumerator RutinaDesaparicion()
    {
        yield return new WaitForSeconds(esperaDesaparicion);
        Destroy(gameObject);
    }
}

