using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI2 : MonoBehaviour, IDamage
{

    [Header("----- Components -----")]
    [SerializeField] Renderer bodyModel;
    [SerializeField] Renderer legsModel;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Transform meleePos;
    [SerializeField] Animator anim;

    [Header("----- Enemy Stats -----")]
    [Range(1, 10)][SerializeField] int HP;
    //[Range(1, 10)][SerializeField] float speed;
    [Range(1, 10)][SerializeField] int playerFaceSpeed;
    [Range(1, 360)][SerializeField] int viewConeAngle;
    [Range(1, 100)][SerializeField] int roamDist;
    [Range(0, 10)][SerializeField] int roamTimer;
    

    [Header("----- Weapon Stats -----")]
    [SerializeField] float meleePunchRate;
    [SerializeField] float meleePunchRange;
    [SerializeField] int meleePunchDamage;
    [SerializeField] GameObject punch;



    Vector3 playerDir;
    public bool playerInRange;
    float angleToPlayer;
    bool enemyMelee;
    Vector3 startingPos;
    bool destinationChosen;
    float stoppingDistanceOrig;
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
            if (playerInRange && !canSeePlayer())
            {
                StartCoroutine(roam());
            }
            else if (agent.destination != gameManager.instance.player.transform.position)
            {
                StartCoroutine(roam());
            } 
        }
    }

    IEnumerator roam()
    {
        if (!destinationChosen && agent.remainingDistance < 0.05f)
        {
            destinationChosen = true;

            agent.stoppingDistance = 0;

            yield return new WaitForSeconds(roamTimer);

            destinationChosen = false;

            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);

            agent.SetDestination(hit.position);
        }
    }

    bool canSeePlayer()
    {
        agent.stoppingDistance = stoppingDistanceOrig;
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewConeAngle)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    facePlayer();
                }
                if (!enemyMelee)
                {
                    StartCoroutine(melee());
                }
                return true;
            }
        }
        return false;
    }

    IEnumerator melee()
    {
        enemyMelee = true;
        anim.SetTrigger("Melee");
        
        yield return new WaitForSeconds(meleePunchRate);
        enemyMelee = false;
    }

    private void meleeAttack()
    {
        GameObject punchObject = Instantiate(punch, meleePos.position, transform.rotation);

        Punch punchComponent = punchObject.GetComponent<Punch>();

        if (punchComponent != null)
        {
            punchComponent.ApplyPunch(gameManager.instance.player);
        }
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
