using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    // Referencia al componente Animator
    public Animator anim;
    public InputHandler inputHandler;
    public PlayerLocomotion playerLocomotion;

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
        inputHandler = GetComponent<InputHandler>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        // Asignar identificadores hash para las variables del Animator
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    // Método para actualizar los valores del Animator basados en el movimiento vertical y horizontal
    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
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

        if(isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }

        // Actualizar los valores en el Animator con suavizado
        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
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

    private void OnAnimatorMove()
    {
        if(inputHandler.isInteracting == false)
        {
            return;
        }

        float delta = Time.deltaTime;
        playerLocomotion.rigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerLocomotion.rigidbody.velocity = velocity;
    }
}
