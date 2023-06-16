using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Transform throwPos;
    [SerializeField] Animator anim;

    [Header("----- Player Stats -----")]
    [Range(1, 100)][SerializeField] int HP;
    [Range(3, 8)][SerializeField] float playerSpeed;
    [Range(8, 25)][SerializeField] float jumpHeight;
    [Range(10, 50)][SerializeField] float gravityValue;
    [SerializeField] int jumpMax;
    [SerializeField] int meleeRange;
    [SerializeField] int meleeDamage;
    [SerializeField] float meleeCooldown;
    [SerializeField] bool sprintToggle;     // true means toggle mode, false means not toggle mode

    [Header("----- Gun Stats -----")]
    [Range(0.1f, 3)][SerializeField] float shootRate;
    [Range(1, 10)][SerializeField] int shootDamage;
    [Range(25, 1000)][SerializeField] int shootDist;
    [SerializeField] GameObject hitEffect;

    [Header("----- Grenade Stats -----")]
    [SerializeField] float throwForce;
    //[SerializeField] float grenadeCooldown;
    [SerializeField] GameObject grenade;

    private int jumpedTimes;
    private bool isSprinting;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Vector3 move;
    bool isShooting;
    bool playerMelee;
    bool isThrowing;
    float sprintSpeed;
    float playerSpeedOrig;
    int playerHPOrig;

    private void Start()
    {
        playerSpeedOrig = playerSpeed;
        sprintSpeed = playerSpeed * 2;
        playerHPOrig = HP;
        spawnPlayer();
    }

    void Update()
    {
        movement();

        if (Input.GetButton("Shoot") && isShooting == false)
        {
            StartCoroutine(shoot());
        }
        if (Input.GetButton("Melee") && playerMelee == false)
        {
            StartCoroutine(melee());
        }
        if (Input.GetButtonDown("Throw") && isThrowing == false)
        {
            //throwGrenade();
            StartCoroutine(throwGrenade());
        }
    }
    void movement()
    {
        anim.SetFloat("Speed", move.normalized.magnitude);

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpedTimes = 0;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && jumpedTimes < jumpMax)
        {
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
        }

        sprint();
        crouch();

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void sprint()
    {
        if (isSprinting)
        {
            playerSpeed = sprintSpeed;
        }
        if (!isSprinting)
        {
            playerSpeed = playerSpeedOrig;
        }

        if (!sprintToggle)
        {
            if (Input.GetButton("Sprint"))
            {
                isSprinting = true;
            }
            else
            {
                isSprinting = false;
            }
        }

        if (sprintToggle)
        {
            // Sprint functionality
            if (Input.GetButtonDown("Sprint"))
            {
                isSprinting = !isSprinting;
            }
        }
    }

    void crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            controller.height = .5f;
        }
        if (Input.GetButtonUp("Crouch"))
        {
            controller.height = 2f;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            IDamage damageable = hit.collider.GetComponent<IDamage>();

            if (damageable != null)
            {
                damageable.takeDamage(shootDamage);
            }
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator melee()
    {
        playerMelee = true;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, meleeRange))
        {
            IDamage damageable = hit.collider.GetComponent<IDamage>();

            if (damageable != null)
            {
                damageable.takeDamage(meleeDamage);
            }
        }
        yield return new WaitForSeconds(meleeCooldown);
        playerMelee = false;
    }

    #region throwGrenade attempt

    IEnumerator throwGrenade()
    {
        isThrowing = true;
        Instantiate(grenade, throwPos.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.Acceleration);
        yield return new WaitForSeconds(shootRate);
        isThrowing = false;
    }

    #endregion

    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / playerHPOrig;
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        updatePlayerUI();
        StartCoroutine(playerFlashDamage());

        if(HP <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    IEnumerator playerFlashDamage()
    {
        gameManager.instance.playerFlashUI.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerFlashUI.SetActive(false);
    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
        HP = playerHPOrig;
        updatePlayerUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Level Exit")
        {
            SceneManager.LoadScene(1);

        }
    }
}