using UnityEngine;
using UnityEngine.InputSystem;


public class AllomancyHandler : MonoBehaviour
{
    private PlayerControls inputActions;
    public Transform playerTransform;
    //public Iron iron;

    void Start()
    {
        inputActions = new PlayerControls();
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerActions.Iron.started += ctx => ActivateMetal(); // Usar el evento 'started' en lugar de 'performed'
            inputActions.Enable();
        }
    }

    void Update()
    {
       
    }

    public void ActivateMetal()
    {
        // L�gica de activaci�n del metal
        Debug.Log("Metal activado");
    }

    public void DeactivateMetal()
    {
        // L�gica de desactivaci�n del metal
        Debug.Log("Metal desactivado");
    }
}
