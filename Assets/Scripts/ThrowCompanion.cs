using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCompanion : Ability
{
    [Header("Throw Settings")]
    [SerializeField] private float throwSpeed = 5f; 
    [SerializeField] private float damageAmount = 50f;  
    [SerializeField] private float detectionRadius = 10f; 
    [SerializeField] private LayerMask enemyLayer; 

    private List<GameObject> companions; 
    private Transform playerTransform; 

    private void Start()
    {
        playerTransform = transform; 
    }

    public override void Trigger()
    {
        GameObject companionToThrow = GetRandomCompanion();
        if (companionToThrow != null)
        {
         
            GameObject closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {

                StartCoroutine(MoveCompanionToEnemy(companionToThrow, closestEnemy));
            }
        }
    }

    private GameObject GetRandomCompanion()
    {
  
        if (companions != null && companions.Count > 0)
        {
            int randomIndex = Random.Range(0, companions.Count);
            return companions[randomIndex];
        }
        return null;
    }

    private GameObject FindClosestEnemy()
    {

        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(playerTransform.position, detectionRadius, enemyLayer);

        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        
        foreach (Collider2D enemyCollider in enemiesInRange)
        {
            if (enemyCollider.CompareTag("Enemy")) 
            {
                float distance = Vector2.Distance(playerTransform.position, enemyCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemyCollider.gameObject;
                }
            }
        }

        return closestEnemy;
    }

    private IEnumerator MoveCompanionToEnemy(GameObject companion, GameObject enemy)
    {
        Vector3 enemyPosition = enemy.transform.position;
        Vector3 directionToEnemy = (enemyPosition - companion.transform.position).normalized;

        // Mientras el compañero no haya alcanzado al enemigo
        while (Vector3.Distance(companion.transform.position, enemyPosition) > 0.1f)
        {
            companion.transform.position += directionToEnemy * throwSpeed * Time.deltaTime;
            yield return null;
        }

    }



    public void SetCompanions(List<GameObject> companionsList)
    {
        companions = companionsList; 
    }
}
