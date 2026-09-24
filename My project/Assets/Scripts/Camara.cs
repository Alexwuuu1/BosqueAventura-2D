using UnityEngine;

/// <summary>
/// Control de seguimiento de cámara en 2D.
/// Desarrollado por Alexwuuu1 siguiendo las pautas de la guía oficial.
/// </summary>
public class Camara : MonoBehaviour
{
    [Header("Objetivo")]
    [Tooltip("Transform del personaje jugador al que debe seguir la cámara")]
    public Transform target;

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }
}

