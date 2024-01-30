using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    // Transform del objetivo a seguir
    public Transform targetTransform;

    // Transform de la cámara
    public Transform cameraTransform;

    // Transform del pivote de la cámara
    public Transform cameraPivotTransform;

    // Transform de este objeto
    private Transform myTransform;

    // Posición original de la cámara
    private float defaultPosition;

    // Ángulo de rotación horizontal
    private float lookAngle;

    // Ángulo de rotación vertical del pivote
    private float pivotAngle;

    // Límite inferior del ángulo de pivote
    public float minimumPivot = -35;

    // Límite superior del ángulo de pivote
    public float maximumPivot = 35;

    // Velocidad de rotación horizontal de la cámara
    public float lookSpeed = 0.1f;

    // Velocidad de seguimiento de la cámara al objetivo
    public float followSpeed = 0.1f;

    // Velocidad de rotación vertical del pivote de la cámara
    public float pivotSpeed = 0.03f;
    private float targetPosition;
    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffSet = 0.2f;
    public float minimumCollisionOffSet = 0.2f;

    // Posición actual de la cámara
    private Vector3 cameraTransformPosition;

    // Máscara de capas a ignorar
    private LayerMask ignoreLayers;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    // Instancia única de la clase CameraHandler
    public static CameraHandler singleton;

    // Método llamado al despertar
    private void Awake()
    {
        // Establecer la instancia única
        singleton = this;

        // Obtener el transform de este objeto
        myTransform = transform;

        // Guardar la posición original de la cámara
        defaultPosition = cameraTransform.localPosition.z;

        // Crear una máscara de capas a ignorar
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    // Método para seguir al objetivo con suavizado
    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        myTransform.position = targetPosition;
        HandleCameraCollisions(delta);
    }

    // Método para manejar la rotación de la cámara
    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        // Ajustar el ángulo de rotación horizontal y vertical según la entrada del ratón
        lookAngle += (mouseXInput * lookSpeed) / delta;
        pivotAngle -= (mouseYInput * pivotSpeed) / delta;

        // Limitar el ángulo de pivote dentro de los límites establecidos
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

        // Aplicar la rotación horizontal al objeto principal
        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        myTransform.rotation = targetRotation;

        // Aplicar la rotación vertical al pivote de la cámara
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
