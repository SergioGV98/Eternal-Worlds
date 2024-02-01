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

    // Método llamado al despertar
    private void Awake()
    {
        // Obtener la instancia única del manejador de la cámara
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

        // Verificar si el manejador de la cámara está presente
        if (cameraHandler != null)
        {
            // Seguir al objetivo y manejar la rotación de la cámara
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
