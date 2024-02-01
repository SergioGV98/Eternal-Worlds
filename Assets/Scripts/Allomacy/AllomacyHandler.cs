using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AllomancyType
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
    private bool isBurnKeyDown;

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

        ChangeAllomancy();
        UpdateAllomancyImage();
    }

    private void Update()
    {
        IsBurnPressed(isBurnKeyDown);
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerActions.ChangeMetal.started += ctx => ChangeAllomancy();
            inputActions.PlayerActions.Burn.started += ctx => IsBurnPressed(true);
            inputActions.PlayerActions.Burn.canceled += ctx => IsBurnPressed(false);
            inputActions.Enable();
        }
    }

    public void CurrentAllomancy()
    {
        int currentIndex = (int)currentAllomancy;
        iron.SetActive(currentIndex == (int)AllomancyType.Iron);
    }


    private void UpdateAllomancyImage()
    {
        if (allomancyTextures.TryGetValue(currentAllomancy, out Texture2D texture))
        {
            allomancyImage.texture = texture;
        }
    }

    private void ChangeAllomancy()
    {
        int currentIndex = (int)currentAllomancy;
        currentIndex = (currentIndex + 1) % Enum.GetValues(typeof(AllomancyType)).Length;
        currentAllomancy = (AllomancyType)currentIndex;

        CurrentAllomancy();
        UpdateAllomancyImage();

        Debug.Log("Allomancy actual: " + currentAllomancy);
    }

    private void IsBurnPressed(bool isBurnKeyDown)
    {
        this.isBurnKeyDown = isBurnKeyDown;
        if (isBurnKeyDown)
        {
            switch (currentAllomancy)
            {
                case AllomancyType.Iron:
                    iron.Burn();
                    break;
                case AllomancyType.Steel:
                    // Lógica para Steel (si es necesario)
                    break;
                case AllomancyType.Pewter:
                    // Lógica para Pewter (si es necesario)
                    break;
                default:
                    Debug.LogError("Tipo de Allomancy no reconocido");
                    break;
            }
        }
    }
}
