using UnityEngine;

public class Iron : MonoBehaviour
{
    private GameObject[] jumpObjects;
    private LineRenderer[] lineRenderers;
    private float minDist = 20f;
    [SerializeField] Material lineMaterial;

    public Vector3 chestOffset = new Vector3(0f, 1f, 0f);

    private bool isActive = false; // Variable para controlar la activación

    void Start()
    {
        lineRenderers = new LineRenderer[0];
    }

    void Update()
    {
        if (!isActive) return; 

        jumpObjects = GameObject.FindGameObjectsWithTag("Iron");
        Vector3 chestPoint = transform.position + chestOffset;

        if (lineRenderers.Length != jumpObjects.Length)
        {
            System.Array.Resize(ref lineRenderers, jumpObjects.Length);
        }

        for (int i = 0; i < jumpObjects.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, jumpObjects[i].transform.position);

            if (dist < minDist)
            {
                if (lineRenderers[i] == null)
                {
                    lineRenderers[i] = CreateLineRenderer();
                }

                lineRenderers[i].SetPosition(0, chestPoint);
                lineRenderers[i].SetPosition(1, jumpObjects[i].transform.position);
            }
            else
            {
                if (lineRenderers[i] != null)
                {
                    Destroy(lineRenderers[i].gameObject);
                    lineRenderers[i] = null;
                }
            }
        }
    }

    private LineRenderer CreateLineRenderer()
    {
        GameObject lineObject = new GameObject("LineRenderer");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial; 
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.1f;
        return lineRenderer;
    }

    public void SetActive(bool active)
    {
        isActive = active;
        // Si isActive es falso, desactiva o destruye todos los LineRenderers activos.
        if (!isActive)
        {
            DisableAllLines();
        }
    }

    private void DisableAllLines()
    {
        if (lineRenderers == null) return;

        for (int i = 0; i < lineRenderers.Length; i++)
        {
            if (lineRenderers[i] != null)
            {
                // Aquí tienes dos opciones: desactivar o destruir.
                // Para desactivar el GameObject que contiene el LineRenderer:
                lineRenderers[i].gameObject.SetActive(false);

                // O, si prefieres destruir el objeto por completo:
                // Destroy(lineRenderers[i].gameObject);

                lineRenderers[i] = null;
            }
        }
    }
}
