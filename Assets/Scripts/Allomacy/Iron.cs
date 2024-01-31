using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Iron : MonoBehaviour
{
    public GameObject[] jumpObjects;
    public float moveSpeed = 3f;

    // Ajusta este offset para representar la posición del pecho
    public Vector3 chestOffset = new Vector3(0f, 1f, 0f);

    private void Update()
    {
        jumpObjects = GameObject.FindGameObjectsWithTag("Iron");

        // Obtener el punto del pecho
        Vector3 chestPoint = transform.position + chestOffset;

        foreach (GameObject go in jumpObjects)
        {
            // Dibujar línea azul desde el pecho hasta el GameObject
            Debug.DrawLine(chestPoint, go.transform.position, Color.blue);
        }
    }

    private void Awake()
    {

    }

    public void Burn()
    {

    }

    void MovePlayerTowardsIron()
    {

    }
}
