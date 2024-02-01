using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    // Objeto de la c�mara
    Transform cameraObject;

    // Manejador de entrada
    InputHandler inputHandler;

    // Direcci�n de movimiento del jugador
    Vector3 moveDirection;

    // Transform del jugador
    [HideInInspector]
    public Transform myTransform;

    // Manejador del Animator
    [HideInInspector]
    public AnimatorHandler animatorHandler;

    // Rigidbody del jugador
    public new Rigidbody rigidbody;

    // C�mara normal del jugador
    public GameObject normalCamera;

    [Header("Movement Stats")]
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float sprintSpeed = 7;
    [SerializeField] float rotationSpeed = 10;

    // M�todo llamado al inicio
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();  
        // Obtener el Rigidbody y los componentes de InputHandler y AnimatorHandler
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();

        // Obtener la transform de la c�mara principal
        cameraObject = Camera.main.transform;

        // Obtener la transform del jugador
        myTransform = transform;

        // Iniciar el AnimatorHandler
        animatorHandler.Start();
    }


    #region Movement
    // Vector normal al plano horizontal
    Vector3 normalVector;

    // M�todo para manejar la rotaci�n del jugador
    private void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        // Calcular la direcci�n de rotaci�n basada en la entrada del jugador y la c�mara
        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right * inputHandler.horizontal;

        targetDir.Normalize();
        targetDir.y = 0;

        // Si la direcci�n de rotaci�n es cero, mantener la direcci�n actual del jugador
        if (targetDir == Vector3.zero)
        {
            targetDir = myTransform.forward;
        }

        // Obtener la velocidad de rotaci�n
        float rs = rotationSpeed;

        // Calcular la rotaci�n objetivo y aplicarla suavemente
        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        // Aplicar la rotaci�n al jugador
        myTransform.rotation = targetRotation;
    }

    public void HandleMovement(float delta)
    {
        if (inputHandler.rollFlag)
            return;

        // Calcular la direcci�n de movimiento del jugador en funci�n de la c�mara
        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        // Calcular la velocidad de movimiento del jugador
        float speed = movementSpeed;

        if(inputHandler.sprintFlag)
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            moveDirection *= speed;
        }

        // Proyectar la velocidad en el plano horizontal
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        // Actualizar los valores del Animator
        animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);

        // Manejar la rotaci�n del jugador si es permitida
        if (animatorHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    public void HandleRollingAndSprinting(float delta)
    {
        if (animatorHandler.anim.GetBool("isInteracting"))
            return;

        if (inputHandler.rollFlag)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if(inputHandler.moveAmount > 0)
            {
                animatorHandler.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            } 
        }
    }

    public bool CheckGround(float delta)
    {
        RaycastHit hit;
        Debug.DrawRay(myTransform.position, Vector3.down * 0.2f, Color.red);
        if (Physics.Raycast(myTransform.position, Vector3.down, out hit, 0.2f))
        {
            return true;
        }
        return false;
    }

    #endregion
}
