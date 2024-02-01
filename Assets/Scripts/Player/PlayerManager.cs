using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    Animator anim;
    CameraHandler cameraHandler;
    PlayerLocomotion playerLocomotion;
    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;

    // M�todo llamado al despertar
    private void Awake()
    {
        // Obtener la instancia �nica del manejador de la c�mara
        cameraHandler = CameraHandler.singleton;
        // Bloqueo del raton en el centro del monitor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponent<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();    
    }


    void Update()
    {
        float delta = Time.deltaTime;

        isInteracting = anim.GetBool("isInteracting");
       
        inputHandler.TickInput(delta);
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.CheckGround(delta);
        
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        // Verificar si el manejador de la c�mara est� presente
        if (cameraHandler != null)
        {
            // Seguir al objetivo y manejar la rotaci�n de la c�mara
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }
    }

    private void LateUpdate()
    {
        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        isSprinting = inputHandler.b_Input;
    }
}
