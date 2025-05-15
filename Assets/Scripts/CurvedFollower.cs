using UnityEngine;

public class CurvedFollower : MonoBehaviour
{
    public Transform player;              // El jugador a seguir
    public float followDistance = 3f;     // Distancia deseada al jugador
    public float followSpeed = 2f;        // Velocidad al seguir al jugador
    public float orbitSpeed = 50f;        // Velocidad de rotación/orbita alrededor del jugador

    void Update()
    {
        if (player == null) return;

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > followDistance + 0.1f)
        {
            // Acercarse suavemente al jugador
            Vector3 targetPos = player.position - directionToPlayer.normalized * followDistance;
            transform.position = Vector3.Slerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
        else
        {
            OrbitAroundPlayerXY();
        }
    }

    void OrbitAroundPlayerXY()
    {
        // Orbitando en el plano XY → eje de rotación es Z
        transform.RotateAround(player.position, Vector3.forward, orbitSpeed * Time.deltaTime);

        // Ajustar la distancia al radio correcto
        Vector3 offset = transform.position - player.position;
        transform.position = player.position + offset.normalized * followDistance;
    }
}
