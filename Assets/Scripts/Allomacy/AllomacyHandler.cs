using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Enumeraci�n para los tipos de Allomancy
enum AllomancyType
{
    Iron,
    Steel,
    Pewter
}

public class AllomancyHandler : MonoBehaviour
{
    // Referencias a los controles de entrada del jugador
    private PlayerControls inputActions;

    // Referencia al transform del jugador
    public Transform playerTransform;

    // Tipo actual de Allomancy
    private AllomancyType currentAllomancy = AllomancyType.Iron;

    // Referencias a las im�genes de Allomancy y sus texturas asociadas
    public RawImage allomancyImage;
    public Texture2D ironTexture;
    public Texture2D steelTexture;
    public Texture2D pewterTexture;

    // Componente Iron para el uso espec�fico de Iron Allomancy
    private Iron iron;

    // Diccionario para mapear tipos de Allomancy a sus texturas
    private Dictionary<AllomancyType, Texture2D> allomancyTextures;

    // M�todo de inicio del script
    void Start()
    {
        // Inicializa los controles de entrada y el diccionario de texturas
        inputActions = new PlayerControls();
        allomancyTextures = new Dictionary<AllomancyType, Texture2D>
        {
            { AllomancyType.Iron, ironTexture },
            { AllomancyType.Steel, steelTexture },
            { AllomancyType.Pewter, pewterTexture },
        };

        // Obtiene la referencia al componente Iron
        iron = GetComponent<Iron>();

        // Cambia la Allomancy al inicio y actualiza la imagen
        ChangeAllomancy();
        UpdateAllomancyImage();
    }

    // M�todo que se activa cuando el objeto se habilita
    public void OnEnable()
    {
        // Verifica si los controles de entrada no son nulos
        if (inputActions == null)
        {
            // Inicializa los controles de entrada y asigna funciones a eventos
            inputActions = new PlayerControls();
            inputActions.PlayerActions.Iron.started += ctx => BurnMetal();
            inputActions.PlayerActions.ChangeMetal.started += ctx => ChangeAllomancy();
            inputActions.Enable();
        }
    }

    /** M�todo para activar o desactivar el metal seg�n la Allomancy actual */
    public void BurnMetal()
    {
        int currentIndex = (int)currentAllomancy;

        // Activa o desactiva el componente Iron seg�n la Allomancy actual
        if (currentIndex == (int)AllomancyType.Iron)
        {
            iron.SetActive(true);
            Debug.Log("Metal activado: Iron");
        }
        else
        {
            iron.SetActive(false);
            Debug.Log("Metal desactivado");
        }
    }

    /** M�todo para actualizar la imagen de Allomancy en la interfaz de usuario */
    private void UpdateAllomancyImage()
    {
        // Verifica si la textura de la Allomancy actual existe en el diccionario
        if (allomancyTextures.ContainsKey(currentAllomancy))
        {
            // Asigna la textura correspondiente a la imagen de Allomancy
            allomancyImage.texture = allomancyTextures[currentAllomancy];
        }
    }

    /** M�todo para cambiar la Allomancy actual al siguiente tipo en orden circular */
    private void ChangeAllomancy()
    {
        // Obtiene el �ndice actual del enum
        int currentIndex = (int)currentAllomancy;

        // Incrementa el �ndice circularmente
        currentIndex = (currentIndex + 1) % Enum.GetValues(typeof(AllomancyType)).Length;

        // Asigna la nueva Allomancy actual
        currentAllomancy = (AllomancyType)currentIndex;

        // Llama a BurnMetal() para activar el nuevo metal autom�ticamente
        BurnMetal();

        // Actualiza la imagen de Allomancy en la interfaz de usuario
        UpdateAllomancyImage();

        // Muestra la Allomancy actual por consola
        Debug.Log("Allomancy actual: " + currentAllomancy);
    }
}
