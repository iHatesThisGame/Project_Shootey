using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class triggerSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] int numberToSpawn;

    int numberSpawned;
    bool playerInRange;
    bool isSpawning;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(numberToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRange && !isSpawning && numberSpawned < numberToSpawn)
        {
            StartCoroutine(spawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;
        Instantiate(objectToSpawn, spawnPos[Random.Range(0, spawnPos.Length)].position, transform.rotation);
        numberSpawned++;

        yield return new WaitForSeconds(timeBetweenSpawns);

        isSpawning = false;
    }
}
