using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] Transform shooterPos;
    [SerializeField] private GameObject enemyShooter;
    [SerializeField] private int shooterTimer = 3;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(shooterTimer, enemyShooter));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, shooterPos.position, Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
