using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    [Range(1, 100)]
    private int pointerSize = 24;
    [SerializeField]
    private Color pointerColor = Color.red;
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private GameObject pr;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) ||Input.GetMouseButton(0))
        {
            ShootRay();
        } 

    }

    private void OnGUI()
    {
        float posX = cam.pixelWidth / 2 - pointerSize / 4;
        float posY = cam.pixelHeight / 2 - pointerSize / 2;

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.normal.textColor = pointerColor;
        style.fontSize = pointerSize;
        GUI.Label(new Rect(posX, posY, pointerSize, pointerSize), "*", style);
    }

    void ShootRay()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("El rayo ha golpeado en el punto: " + hit.point);

            Debug.Log("Objeto golpeado: " + hit.collider.gameObject.name);
            Debug.DrawLine(ray.origin, hit.point, Color.red);

            /**
            if (!hasFired)
            {
                GameObject bulletShell = Instantiate(pr, weapon.transform.position, Quaternion.identity);

                Rigidbody bulletRigidbody = bulletShell.GetComponent<Rigidbody>();
                if (bulletRigidbody != null)
                {
                    bulletRigidbody.AddForce(Vector3.up * 2, ForceMode.Impulse);
                }
            }*/
        }
        else
        {
            Debug.Log("El rayo no ha golpeado en ningun lado.");
            Debug.DrawLine(ray.origin, ray.direction * 10, Color.red);
        }
    }
}

