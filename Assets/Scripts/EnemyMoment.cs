using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float maxAngleSpeed;
    protected float actualmaxAngleSpeed;
    [SerializeField] protected float errorMargin = 0.01f;

    [SerializeField] protected Transform playerTransform; 
    [SerializeField] protected Transform orbitTarget; 
    [SerializeField] protected float orbitSpeed = 20f;

    protected Rigidbody rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    protected virtual void Update()
    {
        OrbitAroundPlayer(); 
        MoveTowardsOrbitTarget(); 
    }

  
    protected void OrbitAroundPlayer()
    {
        orbitTarget.RotateAround(playerTransform.position, Vector3.up, orbitSpeed * Time.deltaTime);
    }


    protected void MoveTowardsOrbitTarget()
    {
        Vector3 direction = orbitTarget.position - transform.position; 
        direction.y = 0; 

        Vector3 normalizedDirection = direction.normalized;

        rb.velocity = normalizedDirection * speed;
    }
}
