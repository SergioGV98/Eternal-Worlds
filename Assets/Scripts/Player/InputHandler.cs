using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // Entradas de movimiento y rotaci�n de la c�mara
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool b_Input;
    public bool rollFlag;
    public bool sprintFlag;
    public float rollInputTimer;

    // Acciones del jugador
    PlayerControls inputActions;

    // Entradas de movimiento y rotaci�n de la c�mara en formato Vector2
    Vector2 movementInput;
    Vector2 cameraInput;


    // M�todo llamado al habilitar este componente
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

    // M�todo llamado al deshabilitar este componente
    private void OnDisable()
    {
        // Deshabilitar las acciones del jugador
        inputActions.Disable();
    }

    // M�todo para procesar la entrada en cada frame
    public void TickInput(float delta)
    {
        // Procesar la entrada de movimiento
        MoveInput(delta);
        HandleRollInput(delta);
    }

    // M�todo para procesar la entrada de movimiento
    private void MoveInput(float delta)
    {
        // Asignar valores de entrada a las variables correspondientes
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRollInput(float delta)
    {
        b_Input = inputActions.PlayerActions.Roll.IsPressed();

        if (b_Input)
        {
            rollInputTimer += delta;
            sprintFlag = true;
        } else
        {
            if(rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }
            rollInputTimer = 0;
        }
    }
}
