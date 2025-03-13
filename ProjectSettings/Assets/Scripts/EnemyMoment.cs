using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{

    [SerializeField] protected float speed;
    Transform playerTransform;
    //[SerializeField] float minSpeed;   Manu dice que seria una habilidad muy rota, no money
    //[SerializeField] float maxSpeed;
    [SerializeField] protected float maxAngleSpeed;


}
