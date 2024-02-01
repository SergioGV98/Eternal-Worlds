using UnityEngine;

using UnityEngine;

public class Iron : MonoBehaviour
{
    [Header("Jump Objects")]
    [SerializeField] private float minDist = 20f;
    [SerializeField] private Vector3 chestOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Camera mainCamera; // A�ade una referencia a la c�mara aqu�

    private bool isActive = false;
    private GameObject[] jumpObjects;
    private LineRenderer[] lineRenderers;

    // Initialization of LineRenderers
    private void Start()
    {
        lineRenderers = new LineRenderer[0];
    }

    // Iron behavior update
    private void Update()
    {
        if (!isActive) return;

        FindJumpObjects();
        UpdateLineRenderers();
    }

    // Find objects with the "Iron" tag
    private void FindJumpObjects()
    {
        jumpObjects = GameObject.FindGameObjectsWithTag("Iron");
    }

    // Update LineRenderers based on the found objects
    private void UpdateLineRenderers()
    {
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
                HandleActiveLineRenderer(i, chestPoint, jumpObjects[i].transform.position);
            }
            else
            {
                HandleInactiveLineRenderer(i);
            }
        }
    }

    // Handle active LineRenderer
    private void HandleActiveLineRenderer(int index, Vector3 start, Vector3 end)
    {
        if (lineRenderers[index] == null)
        {
            lineRenderers[index] = CreateLineRenderer();
        }

        lineRenderers[index].SetPosition(0, start);
        lineRenderers[index].SetPosition(1, end);
    }

    // Handle inactive LineRenderer
    private void HandleInactiveLineRenderer(int index)
    {
        if (lineRenderers[index] != null)
        {
            Destroy(lineRenderers[index].gameObject);
            lineRenderers[index] = null;
        }
    }

    // Create a new LineRenderer
    private LineRenderer CreateLineRenderer()
    {
        GameObject lineObject = new GameObject("LineRenderer");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.1f;
        return lineRenderer;
    }

    // Burn method
    public void Burn()
    {
        // Calcula la direcci�n desde la c�mara hacia el objeto apuntado
        Vector3 direction = mainCamera.transform.forward;

        // Aplica un impulso en la direcci�n calculada (ajusta la fuerza seg�n sea necesario)
        GetComponent<Rigidbody>().AddForce(direction * 10f, ForceMode.Impulse);

        Debug.Log("Estoy quemando hierro");
    }

    // Activate or deactivate Iron behavior
    public void SetActive(bool active)
    {
        isActive = active;

        if (!isActive)
        {
            DisableAllLines();
        }
    }

    // Deactivate all LineRenderers
    private void DisableAllLines()
    {
        if (lineRenderers == null) return;

        for (int i = 0; i < lineRenderers.Length; i++)
        {
            HandleInactiveLineRenderer(i);
        }
    }
}