using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    // Referencia al componente Animator
    public Animator anim;

    // Identificadores hash para las variables en el Animator
    int vertical;
    int horizontal;

    // Indica si se puede rotar o no
    public bool canRotate;

    // Método llamado al iniciar
    public void Start()
    {
        // Obtener el componente Animator asociado al objeto
        anim = GetComponent<Animator>();

        // Asignar identificadores hash para las variables del Animator
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    // Método para actualizar los valores del Animator basados en el movimiento vertical y horizontal
    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
    {
        #region Vertical
        float v = 0;

        // Ajustar el valor vertical según el movimiento recibido
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            v = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }
        #endregion

        #region Horizontal
        float h = 0;

        // Ajustar el valor horizontal según el movimiento recibido
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            h = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }
        #endregion

        // Actualizar los valores en el Animator con suavizado
        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    // Método para permitir la rotación
    public void CanRotate()
    {
        canRotate = true;
    }

    // Método para detener la rotación
    public void StopRotation()
    {
        canRotate = false;
    }
}
