using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // Referência ao Player
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10); // Por padrão, joga a câmera para trás no eixo Z

    void LateUpdate()
    {
        if (target == null) return; // Segurança extra para evitar erros se o Player não estiver atribuído

        Vector3 desiredPosition = target.position + offset;

        // Suaviza o movimento
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Atualiza a posição da câmera
        transform.position = smoothedPosition;        
    }
}