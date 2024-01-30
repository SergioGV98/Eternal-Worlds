using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    // Transform del objetivo a seguir
    public Transform targetTransform;

    // Transform de la c�mara
    public Transform cameraTransform;

    // Transform del pivote de la c�mara
    public Transform cameraPivotTransform;

    // Transform de este objeto
    private Transform myTransform;

    // Posici�n original de la c�mara
    private float defaultPosition;

    // �ngulo de rotaci�n horizontal
    private float lookAngle;

    // �ngulo de rotaci�n vertical del pivote
    private float pivotAngle;

    // L�mite inferior del �ngulo de pivote
    public float minimumPivot = -35;

    // L�mite superior del �ngulo de pivote
    public float maximumPivot = 35;

    // Velocidad de rotaci�n horizontal de la c�mara
    public float lookSpeed = 0.1f;

    // Velocidad de seguimiento de la c�mara al objetivo
    public float followSpeed = 0.1f;

    // Velocidad de rotaci�n vertical del pivote de la c�mara
    public float pivotSpeed = 0.03f;
    private float targetPosition;
    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffSet = 0.2f;
    public float minimumCollisionOffSet = 0.2f;

    // Posici�n actual de la c�mara
    private Vector3 cameraTransformPosition;

    // M�scara de capas a ignorar
    private LayerMask ignoreLayers;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    // Instancia �nica de la clase CameraHandler
    public static CameraHandler singleton;

    // M�todo llamado al despertar
    private void Awake()
    {
        // Establecer la instancia �nica
        singleton = this;

        // Obtener el transform de este objeto
        myTransform = transform;

        // Guardar la posici�n original de la c�mara
        defaultPosition = cameraTransform.localPosition.z;

        // Crear una m�scara de capas a ignorar
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    // M�todo para seguir al objetivo con suavizado
    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        myTransform.position = targetPosition;
        HandleCameraCollisions(delta);
    }

    // M�todo para manejar la rotaci�n de la c�mara
    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        // Ajustar el �ngulo de rotaci�n horizontal y vertical seg�n la entrada del rat�n
        lookAngle += (mouseXInput * lookSpeed) / delta;
        pivotAngle -= (mouseYInput * pivotSpeed) / delta;

        // Limitar el �ngulo de pivote dentro de los l�mites establecidos
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

        // Aplicar la rotaci�n horizontal al objeto principal
        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        myTransform.rotation = targetRotation;

        // Aplicar la rotaci�n vertical al pivote de la c�mara
        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCameraCollisions(float delta)
    {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
        {
            float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(dis - cameraCollisionOffSet);
        }

        if(Mathf.Abs(targetPosition) < minimumCollisionOffSet)
        {
            targetPosition = -minimumCollisionOffSet;
        }

        cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
        cameraTransform.localPosition = cameraTransformPosition;
    }
}
