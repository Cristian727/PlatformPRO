
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companions : EnemyMovement
{
    // almacenar A: el up actual
    // calcular B: el up necesario pa que el enemigo mire perfectamente al pj
    // calcular C: la dirección que va desde A hacia B pero solo maxTurnSpeed grados
    // pasarle C al up

    // aplicar C al velocity, con la velocidad pertinente
    private Transform playerTransform;

    private void Start()
    {
        actualmaxAngleSpeed = Random.Range(
            maxAngleSpeed * (1 - errorMargin),
            maxAngleSpeed * (1 + errorMargin)
        );



        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

    }

    private void Update()
    {
        if (playerTransform != null)
        {
            AlignAndMoveTowardsPlayer();
        }
    }

    private void AlignAndMoveTowardsPlayer()
    {

        Vector3 A = transform.up;


        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Vector3 B = directionToPlayer;


        float maxRadiansDelta = actualmaxAngleSpeed * Mathf.Deg2Rad * Time.deltaTime;
        Vector3 C = Vector3.RotateTowards(A, B, maxRadiansDelta, 0.0f);


        transform.up = C;


        Vector3 velocity = C.normalized * speed * Time.deltaTime;
        transform.position += velocity;
    }
    // dos niveles en dos escenas independientes
    // escena de seleccion de nivel
    // solo se puede entrar en un nivel desde la escena de seleccion de nivel
}
