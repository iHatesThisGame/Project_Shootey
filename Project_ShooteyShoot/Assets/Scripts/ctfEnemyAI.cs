using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ctfEnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer bodyModel;
    [SerializeField] Renderer legsModel;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Transform shootPos;
    [SerializeField] Animator anim;
    [SerializeField] Transform holdFlagPos;
    [SerializeField] Transform flagPos;

    [Header("----- Enemy Stats -----")]
    [Range(1, 10)][SerializeField] int HP;
    [Range(1, 10)][SerializeField] int playerFaceSpeed;
    [Range(1, 360)][SerializeField] int viewConeAngle;

    [Header("----- Weapon Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;

    Vector3 playerDir;
    public bool playerInRange;
    float angleToPlayer;
    bool isShooting;
    Vector3 startingPos;
    bool hasFlag;
    float stoppingDistanceOrig;
    Transform focusPos;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        flagPos = GameObject.FindGameObjectWithTag("Blue Flag").transform;
        startingPos = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
            if (!canSeePlayer() && !hasFlag)
            {
                seekFlag();
            }
            else if (hasFlag)
            {
                returnFlag();
            }
        }
    }

    void returnFlag()
    {
        if (hasFlag)
        {
            focusPos = GameObject.FindGameObjectWithTag("Red Base").transform;
        }
    }

    void seekFlag()
    {
        if (!hasFlag)
        {
            focusPos = flagPos;
            agent.stoppingDistance = 0;
            agent.SetDestination(GameObject.FindGameObjectWithTag("Blue Flag").transform.position);
        }
    }

    void pickupFlag()
    {
        flagPos = holdFlagPos;
    }

    //IEnumerator roam()
    //{
    //    if (!destinationChosen && agent.remainingDistance < 0.05f && HP > 0)
    //    {
    //        destinationChosen = true;

    //        agent.stoppingDistance = 0;

    //        yield return new WaitForSeconds(roamTimer);

    //        destinationChosen = false;

    //        Vector3 blueFlagPos = Random.insideUnitSphere * roamDist;
    //        blueFlagPos += startingPos;

    //        NavMeshHit hit;
    //        NavMesh.SamplePosition(blueFlagPos, out hit, roamDist, 1);

    //        agent.SetDestination(hit.position);
    //    }
    //}

    bool canSeePlayer()
    {
        agent.stoppingDistance = stoppingDistanceOrig;

        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        Debug.DrawRay(headPos.position, playerDir);
        Debug.Log(angleToPlayer);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit) && !hasFlag)
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
        }
        return false;
    }

    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
        if (other.CompareTag("Blue Flag") && !hasFlag)
        {
            hasFlag = true;
            pickupFlag();
        }
        if (other.CompareTag("Red Base") && hasFlag)
        {
            hasFlag = false;
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