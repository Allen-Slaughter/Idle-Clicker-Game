using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private Transform spawnPosition;

    void Start()
    {
        SpawnCloud();
    }

    private void SpawnCloud()
    {
        GameObject newCloud = Instantiate(cloudPrefab, spawnPosition.position, Quaternion.identity);
        Cloud cloud = newCloud.GetComponent<Cloud>();
        cloud.SpawnPosition = spawnPosition.position;
    }
}
