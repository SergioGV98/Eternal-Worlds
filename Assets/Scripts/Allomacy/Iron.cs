using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public class Iron : MonoBehaviour
{
    public static Iron Instance { get; private set; }

    public float moveSpeed = 3f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Burn(Transform playerTransform)
    {
        RaycastHit hit;
        if (Physics.Raycast(playerTransform.position, playerTransform.forward, out hit, 3f, LayerMask.GetMask("Iron")))
        {
            Debug.DrawLine(playerTransform.position, hit.point, Color.blue);
            MovePlayerTowardsIron(playerTransform, hit.collider.gameObject);
        }
    }

    void MovePlayerTowardsIron(Transform playerTransform, GameObject ironObject)
    {
        Vector3 directionToIron = (ironObject.transform.position - playerTransform.position).normalized;
        Vector3 newPosition = playerTransform.position + directionToIron * 2f;

        playerTransform.position = Vector3.MoveTowards(playerTransform.position, newPosition, Time.deltaTime * moveSpeed);
    }
}
