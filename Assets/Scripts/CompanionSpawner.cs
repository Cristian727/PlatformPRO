using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject companionPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxCompanions = 3;

    private List<GameObject> companions = new List<GameObject>();
    [SerializeField] private ThrowCompanion throwCompanionScript; 

    private void Start()
    {
        StartCoroutine(GenerateCompanions());
    }

    private IEnumerator GenerateCompanions()
    {
        while (true)
        {
            if (companions.Count < maxCompanions)
            {
                SpawnCompanion();
            }

            yield return new WaitForSeconds(spawnInterval);

        }
    }

    private void SpawnCompanion()
    {
        GameObject newCompanion = Instantiate(companionPrefab, spawnPoint.position, Quaternion.identity);
        companions.Add(newCompanion);

        // Actualiza la lista de compañeros en ThrowCompanion
        if (throwCompanionScript != null)
        {
            throwCompanionScript.SetCompanions(companions);
        }
    }
}
//crear un cuadrado en unity que empieza en xBound que se mueva hacia la derecha usando speed, y que cambie de color segun se mueve a la derecha y cuando llegue al limite puesto en xBound vuelva a repeter el proceso indefinidamente
