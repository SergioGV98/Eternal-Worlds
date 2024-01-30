using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // Entradas de movimiento y rotación de la cámara
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    // Acciones del jugador
    PlayerControls inputActions;

    // Manejador de la cámara
    CameraHandler cameraHandler;

    // Entradas de movimiento y rotación de la cámara en formato Vector2
    Vector2 movementInput;
    Vector2 cameraInput;

    // Método llamado al despertar
    private void Awake()
    {
        // Obtener la instancia única del manejador de la cámara
        cameraHandler = CameraHandler.singleton;
        // Bloqueo del raton en el centro del monitor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Método llamado en cada fixed update
    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        // Verificar si el manejador de la cámara está presente
        if (cameraHandler != null)
        {
            // Seguir al objetivo y manejar la rotación de la cámara
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
        }
    }

    // Método llamado al habilitar este componente
    public void OnEnable()
    {
        // Configurar las acciones del jugador y asignar funciones a los eventos
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        }

        // Habilitar las acciones del jugador
        inputActions.Enable();
    }

    // Método llamado al deshabilitar este componente
    private void OnDisable()
    {
        // Deshabilitar las acciones del jugador
        inputActions.Disable();
    }

    // Método para procesar la entrada en cada frame
    public void TickInput(float delta)
    {
        // Procesar la entrada de movimiento
        MoveInput(delta);
    }

    // Método para procesar la entrada de movimiento
    private void MoveInput(float delta)
    {
        // Asignar valores de entrada a las variables correspondientes
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }
}
