using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyMovement : EnemyMovement
{

    // almacenar A: el up actual
    // calcular B: el up necesario pa que el enemigo mire perfectamente al pj
    // calcular C: la dirección que va desde A hacia B pero solo maxTurnSpeed grados
    // pasarle C al up

    // aplicar C al velocity, con la velocidad pertinente

    private void Start()
    {
       // pritn(max)
    }
}
