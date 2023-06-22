using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] Transform enemySpawnPos;
    [SerializeField] private GameObject enemyType;
    [SerializeField] private float timeBetweenSpawns = 10;
    [SerializeField] private int spawnedLimit = 10;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(timeBetweenSpawns, enemyType));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        if (gameManager.instance.enemiesRemaining < spawnedLimit)
        {
            GameObject newEnemy = Instantiate(enemy, enemySpawnPos.position, Quaternion.identity);
            gameManager.instance.updateGameGoal(1);
        }
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
