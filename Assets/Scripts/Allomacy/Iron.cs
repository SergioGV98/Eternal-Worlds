using UnityEngine;

public class Iron : MonoBehaviour
{
    [Header("Jump Objects")]
    [SerializeField] private float minDist = 20f;
    [SerializeField] private Vector3 chestOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Camera mainCamera;

    private bool isActive = false;
    private GameObject[] jumpObjects;
    private LineRenderer[] lineRenderers;

    private void Start()
    {
        lineRenderers = new LineRenderer[0];
    }

    private void Update()
    {
        if (!isActive) return;

        FindJumpObjects();
        UpdateLineRenderers();
    }

    private void FindJumpObjects()
    {
        jumpObjects = GameObject.FindGameObjectsWithTag("Iron");
    }

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

    private void HandleActiveLineRenderer(int index, Vector3 start, Vector3 end)
    {
        if (lineRenderers[index] == null)
        {
            lineRenderers[index] = CreateLineRenderer();
        }

        lineRenderers[index].SetPosition(0, start);
        lineRenderers[index].SetPosition(1, end);
    }

    private void HandleInactiveLineRenderer(int index)
    {
        if (lineRenderers[index] != null)
        {
            Destroy(lineRenderers[index].gameObject);
            lineRenderers[index] = null;
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

    public void Burn()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 cameraDirection = ray.direction;

        LineRenderer closestLine = FindClosestLineInDirection(cameraDirection);

        if (closestLine != null)
        {
            Vector3 lineCenter = (closestLine.GetPosition(0) + closestLine.GetPosition(1)) / 2f;
            Vector3 direction = (lineCenter - (transform.position + chestOffset)).normalized;

            transform.position += direction * Time.deltaTime * 5f;
            GetComponent<Rigidbody>().AddForce(direction * 10f, ForceMode.Impulse);
        }
    }

    private LineRenderer FindClosestLineInDirection(Vector3 direction)
    {
        Vector3 chestPoint = transform.position + chestOffset;
        LineRenderer closestLine = null;
        float closestDist = float.MaxValue;

        for (int i = 0; i < lineRenderers.Length; i++)
        {
            if (lineRenderers[i] != null)
            {
                Vector3 lineCenter = (lineRenderers[i].GetPosition(0) + lineRenderers[i].GetPosition(1)) / 2f;
                Vector3 lineDirection = (lineCenter - chestPoint).normalized;

                float dotProduct = Vector3.Dot(lineDirection, direction);

                if (dotProduct > 0.9f)
                {
                    float dist = Vector3.Distance(chestPoint, lineCenter);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closestLine = lineRenderers[i];
                    }
                }
            }
        }

        return closestLine;
    }

    public void SetActive(bool active)
    {
        isActive = active;

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
            HandleInactiveLineRenderer(i);
        }
    }
}
