using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EyeFollowing : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        Vector2 direccion = player.position - transform.position;
        transform.right = direccion; // Esto hace que el eje X del objeto apunte al jugador
    }
}
