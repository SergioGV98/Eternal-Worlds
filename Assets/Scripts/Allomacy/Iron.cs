using UnityEngine;

public class Iron : MonoBehaviour
{
    private GameObject[] jumpObjects;
    private LineRenderer[] lineRenderers;
    private float minDist = 20f;
    [SerializeField] Material lineMaterial;

    // Desplazamiento del punto de conexión al pecho del personaje
    public Vector3 chestOffset = new Vector3(0f, 1f, 0f);

    private bool isActive = false; // Variable para controlar la activación

    // Inicialización de los LineRenderers
    void Start()
    {
        lineRenderers = new LineRenderer[0];
    }

    // Actualización del comportamiento de Iron
    void Update()
    {
        // Si no está activo, salir del método
        if (!isActive) return;

        // Buscar objetos con la etiqueta "Iron"
        jumpObjects = GameObject.FindGameObjectsWithTag("Iron");
        Vector3 chestPoint = transform.position + chestOffset;

        // Ajustar el tamaño del array de LineRenderers según la cantidad de objetos encontrados
        if (lineRenderers.Length != jumpObjects.Length)
        {
            System.Array.Resize(ref lineRenderers, jumpObjects.Length);
        }

        // Iterar sobre los objetos encontrados y actualizar los LineRenderers
        for (int i = 0; i < jumpObjects.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, jumpObjects[i].transform.position);

            if (dist < minDist)
            {
                // Crear un nuevo LineRenderer si no existe
                if (lineRenderers[i] == null)
                {
                    lineRenderers[i] = CreateLineRenderer();
                }

                // Establecer los puntos de inicio y fin del LineRenderer
                lineRenderers[i].SetPosition(0, chestPoint);
                lineRenderers[i].SetPosition(1, jumpObjects[i].transform.position);
            }
            else
            {
                // Destruir el LineRenderer si la distancia es mayor que minDist
                if (lineRenderers[i] != null)
                {
                    Destroy(lineRenderers[i].gameObject);
                    lineRenderers[i] = null;
                }
            }
        }
    }

    // Método para crear un nuevo LineRenderer
    private LineRenderer CreateLineRenderer()
    {
        GameObject lineObject = new GameObject("LineRenderer");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.1f;
        return lineRenderer;
    }

    // Método para activar o desactivar el comportamiento de Iron
    public void SetActive(bool active)
    {
        isActive = active;

        // Si isActive es falso, desactiva o destruye todos los LineRenderers activos.
        if (!isActive)
        {
            DisableAllLines();
        }
    }

    // Método para desactivar todos los LineRenderers
    private void DisableAllLines()
    {
        if (lineRenderers == null) return;

        for (int i = 0; i < lineRenderers.Length; i++)
        {
            if (lineRenderers[i] != null)
            {
                Destroy(lineRenderers[i].gameObject);
                lineRenderers[i] = null;
            }
        }
    }
}
