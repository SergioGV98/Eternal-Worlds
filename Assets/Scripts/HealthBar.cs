using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;

    [SerializeField] Color highHealthColor = Color.green;
    [SerializeField] Color mediumHealthColor = Color.yellow;
    [SerializeField] Color lowHealthColor = Color.red;

    void Start()
    {
        // Puedes configurar el color inicial en Start si es necesario
        UpdateFillColor();
    }

    void Update()
    {
        // Actualiza el color cuando cambia la salud
        UpdateFillColor();
    }

    void UpdateFillColor()
    {
        RawImage fillRawImage = slider.fillRect.GetComponentInChildren<RawImage>();

        if (fillRawImage != null)
        {
            // Asigna el color según la salud actual
            if (slider.value >= 70)
            {
                fillRawImage.color = highHealthColor;
            }
            else if (slider.value >= 35)
            {
                fillRawImage.color = mediumHealthColor;
            }
            else
            {
                fillRawImage.color = lowHealthColor;
            }
        }
    }
}
