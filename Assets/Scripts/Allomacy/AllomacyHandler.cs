using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum AllomancyType
{
    Iron,
    Steel,
    Pewter
}

public class AllomancyHandler : MonoBehaviour
{
    private PlayerControls inputActions;
    public Transform playerTransform;
    private AllomancyType currentAllomancy = AllomancyType.Iron;
    public RawImage allomancyImage;
    public Texture2D ironTexture;
    public Texture2D steelTexture;
    public Texture2D pewterTexture;

    private Iron iron;

    private Dictionary<AllomancyType, Texture2D> allomancyTextures;

    void Start()
    {
        inputActions = new PlayerControls();
        allomancyTextures = new Dictionary<AllomancyType, Texture2D>
        {
            { AllomancyType.Iron, ironTexture },
            { AllomancyType.Steel, steelTexture },
            { AllomancyType.Pewter, pewterTexture },
        };

        iron = GetComponent<Iron>();

        UpdateAllomancyImage();
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerActions.Iron.started += ctx => BurnMetal();
            // Boton de cambio de alomacia
            inputActions.PlayerActions.ChangeMetal.started += ctx => ChangeAllomancy();
            inputActions.Enable();
        }
    }

    public void BurnMetal()
    {
        int currentIndex = (int)currentAllomancy;

        if (currentIndex == (int)AllomancyType.Iron)
        {
            iron.SetActive(true);
            Debug.Log("Metal activado: Iron");
        }
        else if (currentIndex == (int)AllomancyType.Steel)
        {
            iron.SetActive(false);
            Debug.Log("Metal activado: Steel");
        }
        else if (currentIndex == (int)AllomancyType.Pewter)
        {
            iron.SetActive(false);
            Debug.Log("Metal activado: Pewter");
        }
    }

    private void UpdateAllomancyImage()
    {
        if (allomancyTextures.ContainsKey(currentAllomancy))
        {
            allomancyImage.texture = allomancyTextures[currentAllomancy];
        }
    }

    private void ChangeAllomancy()
    {
        // Obtiene el índice actual del enum
        int currentIndex = (int)currentAllomancy;

        // Incrementa el índice circularmente
        currentIndex = (currentIndex + 1) % Enum.GetValues(typeof(AllomancyType)).Length;

        // Asigna la nueva alomacia actual
        currentAllomancy = (AllomancyType)currentIndex;

        UpdateAllomancyImage();

        // Muestra la alomacia actual por consola
        Debug.Log("Alomacia actual: " + currentAllomancy);
    }
}