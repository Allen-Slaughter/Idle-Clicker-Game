using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cloud : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    public Vector3 SpawnPosition { get; set; }

    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        transform.position = new Vector3(SpawnPosition.x, transform.position.y + Random.Range(-0.5f, 0.5f));
    }
}
