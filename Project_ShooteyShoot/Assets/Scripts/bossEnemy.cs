using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bossEnemy : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Transform firePoint;

    [Header("----- Enemy Stats -----")]
    [Range(1, 60)][SerializeField] int HP;
    [Range(1, 100)][SerializeField] float movementSpeed;
    [Range(1, 100)][SerializeField] float rotationSpeed;
    [Range(1, 100)][SerializeField] float minDistanceToPlayer;
    [Range(1, 100)][SerializeField] float roamRadius;
    [Range(1, 100)][SerializeField] float roamChangeInterval;
    [Range(1, 100)][SerializeField] float hoverHeight;
    [Range(1, 100)][SerializeField] float hoverSpeed;
    [Range(1, 100)][SerializeField] float chaseSpeed;

    [Header("----- Weapon Stats -----")]
    [SerializeField] GameObject heatSeekingBulletPrefab;
    [Range(1, 100)][SerializeField] float burstDelay;
    [Range(1, 100)][SerializeField] int burstCount;
    [Range(1, 360)][SerializeField] float gunRotationSpeed;
    [Range(1, 100)][SerializeField] float bulletSpeed;

    private Transform playerTransform;
    private float originalY;
    private float nextFireTime;
    private bool playerSpotted = false;
    private bool isFollowingPlayer = false;
    private Vector3 roamPosition;
    private float nextRoamChangeTime;
    private Rigidbody rb;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        originalY = transform.position.y;
        nextFireTime = Time.time + 1f;
        nextRoamChangeTime = Time.time + roamChangeInterval;
        rb = GetComponent<Rigidbody>();
        SetNewRoamPosition();
    }

    private void Update()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.Normalize();

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer < minDistanceToPlayer)
        {
            playerSpotted = true;
        }

        if (playerSpotted || isFollowingPlayer)
        {
            agent.SetDestination(playerTransform.position);

            if (distanceToPlayer > minDistanceToPlayer)
            {
                isFollowingPlayer = true;
                agent.speed = chaseSpeed; 
            }
            else
            {
                isFollowingPlayer = false;
                agent.speed = movementSpeed; 
            }
        }

        if (isFollowingPlayer)
        {
            Quaternion targetBodyRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));
            headPos.rotation = Quaternion.RotateTowards(headPos.rotation, targetBodyRotation, rotationSpeed * Time.deltaTime);

            Vector3 newPosition = transform.position + directionToPlayer * movementSpeed * Time.deltaTime;
            newPosition.y = originalY; 
            rb.MovePosition(newPosition);
        }
        else
        {
            if (Time.time >= nextRoamChangeTime || Vector3.Distance(transform.position, roamPosition) <= 1f)
            {
                SetNewRoamPosition();
                nextRoamChangeTime = Time.time + roamChangeInterval;
            }

            Vector3 directionToRoamPosition = roamPosition - transform.position;
            directionToRoamPosition.Normalize();
            Quaternion targetBodyRotation = Quaternion.LookRotation(new Vector3(directionToRoamPosition.x, 0f, directionToRoamPosition.z));
            headPos.rotation = Quaternion.RotateTowards(headPos.rotation, targetBodyRotation, rotationSpeed * Time.deltaTime);

            transform.Translate(directionToRoamPosition * movementSpeed * Time.deltaTime);
        }

        float hoverOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(transform.position.x, originalY + hoverOffset, transform.position.z);

        if (Time.time >= nextFireTime && (playerSpotted || isFollowingPlayer))
        {
            StartCoroutine(ShootFireBurst(directionToPlayer));
            nextFireTime = Time.time + burstDelay;

            SetNewRoamPosition();
        }
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            isFollowingPlayer = true;
            agent.SetDestination(playerTransform.position);
            StartCoroutine(flashColor());

            StartCoroutine(KeepChasingPlayer());
        }
    }

    IEnumerator KeepChasingPlayer()
    {
        while (isFollowingPlayer)
        {
            agent.SetDestination(playerTransform.position);
            yield return null;
        }
    }

    IEnumerator flashColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    private IEnumerator ShootFireBurst(Vector3 direction)
    {
        for (int i = 0; i < burstCount; i++)
        {
            ShootFire(direction);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void ShootFire(Vector3 direction)
    {
        Vector3 directionToPlayer = playerTransform.position - firePoint.position;

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, directionToPlayer, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                headPos.rotation = Quaternion.RotateTowards(headPos.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                RaycastHit obstacleHit;
                if (!Physics.Raycast(firePoint.position, directionToPlayer, out obstacleHit, Mathf.Infinity, LayerMask.GetMask("Obstacle")))
                {
                    GameObject bulletObj = Instantiate(heatSeekingBulletPrefab, firePoint.position, targetRotation);
                    heatSeekingBullet bulletComponent = bulletObj.GetComponent<heatSeekingBullet>();
                    if (bulletComponent != null)
                    {
                        bulletComponent.speed = bulletSpeed;
                        bulletComponent.SetDirection(directionToPlayer);
                    }
                }
            }
            else
            {
                SetNewRoamPosition();
            }
        }

        SetNewRoamPosition();
    }


    private void SetNewRoamPosition()
    {
        Vector2 randomPoint = Random.insideUnitCircle * roamRadius;
        roamPosition = transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
    }
}
