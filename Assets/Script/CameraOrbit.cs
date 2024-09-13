using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform player;  // R�f�rence � la capsule (Player)
    public float distance = 5.0f;  // Distance initiale de la cam�ra
    public float minDistance = 2.0f;  // Distance minimale du zoom
    public float maxDistance = 10.0f; // Distance maximale du zoom
    public float zoomSpeed = 2.0f;  // Vitesse du zoom
    public float rotationSpeed = 5.0f;  // Vitesse de rotation de la cam�ra
    public float minYAngle = -20f;  // Limite inf�rieure de la rotation verticale
    public float maxYAngle = 80f;   // Limite sup�rieure de la rotation verticale

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    void Start()
    {
        // Initialiser la position de la cam�ra
        if (player != null)
        {
            transform.position = player.position - transform.forward * distance;
        }

        if (Input.GetMouseButton(0))
        {
            Debug.Log("Clic gauche d�tect�");
        }
        if (Input.GetMouseButton(1))
        {
            Debug.Log("Clic droit d�tect�");
        }
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Debug.Log("Mouse X: " + mouseX + ", Mouse Y: " + mouseY);
    }

    void Update()
    {
        if (player == null) return;  // �vite de faire des calculs si le joueur est d�truit

        // Zoom avec la molette de la souris
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);  // Limiter le zoom

        // Rotation avec la souris
        if (Input.GetMouseButton(1))  // Bouton droit de la souris
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;

            // Limiter la rotation verticale
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

            // Faire tourner le joueur dans la direction de la cam�ra
            Quaternion rotation = Quaternion.Euler(0, currentX, 0);
            player.rotation = rotation;
        }
        else if (Input.GetMouseButton(0))  // Bouton gauche de la souris
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;

            // Limiter la rotation verticale
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        }
    }

    void LateUpdate()
    {
        if (player == null) return;  // �vite de faire des calculs si le joueur est d�truit

        // Calculer la direction de la cam�ra par rapport au joueur
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // Positionner la cam�ra autour du joueur
        transform.position = player.position + rotation * direction;

        // Toujours regarder vers le joueur
        transform.LookAt(player.position + Vector3.up * 1.0f); // Ajuste la hauteur du regard
    }
}
