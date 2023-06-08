using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] Transform shooterPos;
    [SerializeField] Transform meleePos;
    [SerializeField] private GameObject enemyShooter;
    [SerializeField] private GameObject enemyMelee;
    [SerializeField] private int shooterTimer = 10;
    [SerializeField] private int meleeTimer = 10;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnShooter(shooterTimer, enemyShooter));
    }

    private IEnumerator spawnShooter(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        if (gameManager.instance.enemiesRemaining <= 20)
        {
            GameObject newEnemy = Instantiate(enemy, shooterPos.position, Quaternion.identity); 
        }
        StartCoroutine(spawnShooter(interval, enemy));
    }

    private IEnumerator spawnMelee(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        if (gameManager.instance.enemiesRemaining <= 20)
        {
            GameObject newEnemy = Instantiate(enemy, meleePos.position, Quaternion.identity); 
        }
        StartCoroutine(spawnMelee(interval, enemy));
    }
}
