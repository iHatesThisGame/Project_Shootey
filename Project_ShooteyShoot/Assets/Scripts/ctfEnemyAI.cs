using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ctfEnemyAI : MonoBehaviour, IDamage, ICapture
{
    [Header("----- Components -----")]
    [SerializeField] Renderer bodyModel;
    [SerializeField] Renderer legsModel;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Transform shootPos;
    [SerializeField] Animator anim;

    [Header("----- Enemy Stats -----")]
    [Range(1, 10)][SerializeField] int HP;
    [Range(1, 10)][SerializeField] int playerFaceSpeed;
    [Range(1, 360)][SerializeField] int viewConeAngle;

    [Header("----- Weapon Stats -----")]
    [SerializeField] float burstRounds;
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;

    Vector3 playerDir;
    public bool playerInRange;
    float angleToPlayer;
    bool isShooting;
    Vector3 startingPos;
    float stoppingDistanceOrig;
    public bool hasFlag;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;
    }
    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);

            if (!playerInRange)
            {
                if (GameObject.FindGameObjectWithTag("Blue Flag") != null)
                {
                    seekFlag();
                }
            }
            if (playerInRange)
            {
                agent.stoppingDistance = stoppingDistanceOrig;
                canSeePlayer();
            }
        }
    }
    void seekFlag()
    {
        if (!hasFlag)
        {
            agent.SetDestination(GameObject.FindGameObjectWithTag("Blue Flag").transform.position);
            agent.stoppingDistance = 0;
            if (agent.transform.position.z == GameObject.FindGameObjectWithTag("Blue Flag").transform.position.z && agent.transform.position.x == GameObject.FindGameObjectWithTag("Blue Flag").transform.position.x)
            {
                Destroy(GameObject.FindGameObjectWithTag("Blue Flag"));
                hasFlag = true;
            }
        }
        if (hasFlag)
        {
            agent.SetDestination(GameObject.FindGameObjectWithTag("Red Flag").transform.position);
            agent.stoppingDistance = 0;
            if (agent.transform.position.z == GameObject.FindGameObjectWithTag("Red Flag").transform.position.z && agent.transform.position.x == GameObject.FindGameObjectWithTag("Red Flag").transform.position.x)
            {
                hasFlag = false;
            }
        }
    }
    bool canSeePlayer()
    {
        agent.stoppingDistance = stoppingDistanceOrig;

        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit) && playerInRange)
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewConeAngle)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    facePlayer();
                }
                if (!isShooting)
                {

                    StartCoroutine(shoot());
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    IEnumerator shoot()
    {
        isShooting = true;
        for (int i = 0; i < burstRounds; i++)
        {
            anim.SetTrigger("Shoot");
            Instantiate(bullet, shootPos.position, shootPos.transform.rotation);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    public void capture(GameObject flag)
    {
        hasFlag = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }
    public void takeDamage(int dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
            StopAllCoroutines();
            gameManager.instance.updateGameGoal(-1);
            anim.SetBool("Death", true);
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;

            gameManager.instance.killCount += 1;
            gameManager.instance.killCountText.text = gameManager.instance.killCount.ToString("F0");
        }
        else
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
            StartCoroutine(flashColor());
        }
    }
    IEnumerator flashColor()
    {
        bodyModel.material.color = Color.red;
        legsModel.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        bodyModel.material.color = Color.white;
        legsModel.material.color = Color.white;
    }
}
