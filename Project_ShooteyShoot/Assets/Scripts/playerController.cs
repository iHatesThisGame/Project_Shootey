using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour, IDamage, ICapture, IAmmo, IShield
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] ParticleSystem sprintEffect;

    [Header("----- Player Stats -----")]
    [SerializeField] public int HP;
    [SerializeField] public int shieldHP;
    [SerializeField] public int shieldMax;
    [Range(3, 8)][SerializeField] public float playerSpeed;
    [Range(15, 100)][SerializeField] float dashSpeed;
    [Range(0.1f, 1)][SerializeField] float dashTime;
    [Range(8, 25)][SerializeField] float jumpHeight;
    [Range(10, 50)][SerializeField] float gravityValue;
    [SerializeField] int jumpMax;
    [SerializeField] int meleeRange;
    [SerializeField] int meleeDamage;
    [SerializeField] float meleeCooldown;
    [SerializeField] bool sprintToggle;     // true means toggle mode, false means not toggle mode

    [Header("----- Gun Stats -----")]
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [Range(0.1f, 3)][SerializeField] float shootRate;
    [Range(1, 10)][SerializeField] public int shootDamage;
    [Range(25, 1000)][SerializeField] int shootDist;
    [SerializeField] GameObject gunModel;
    public int selectedGun;
    [SerializeField] float zoomIn;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audDamage;
    [Range(0, 1)][SerializeField] float audDamageVol;
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [SerializeField] AudioClip audGunClick;
    [Range(0, 1)][SerializeField] float audGunClickVol;
    [SerializeField] AudioClip audGunshot;
    [Range(0, 1)][SerializeField] float audGunshotVol;
    [SerializeField] AudioClip audReload;
    [Range(0, 1)][SerializeField] float audReloadVol;
    [SerializeField] AudioClip audSpawned;
    [Range(0, 1)][SerializeField] float audSpawnedVol;

    private int jumpedTimes;
    bool stepsIsPlaying;
    private bool isSprinting;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Vector3 move;
    bool isShooting;
    bool playerMelee;
    public float sprintSpeed;
    public float playerSpeedOrig;
    public Vector3 dashDir;
    bool isDashing;
    public int playerHPOrig;
    public int playerShieldOrig;
    float zoomOrig;
    public bool hasFlag;

    private void Start()
    {
        playerSpeedOrig = playerSpeed;
        sprintSpeed = playerSpeed * 2;
        playerHPOrig = HP;
        playerShieldOrig = 10;
        spawnPlayer();
        zoomOrig = Camera.main.fieldOfView;
        updatePlayerUI();
    }

    void Update()
    {
        zoomSight();

        if(gameManager.instance.activeMenu == null)
        {
            movement();

            if(gunList.Count > 0)
            {
                changeGun();

                if (Input.GetButton("Shoot") && isShooting == false)
                {
                    StartCoroutine(shoot());
                }
            }

            if (Input.GetButton("Melee") && playerMelee == false)
            {
                StartCoroutine(melee());
            }
        }
    }

    void zoomSight()
    {
        if (Input.GetButton("Zoom"))
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomIn, Time.deltaTime * 3);
        }
        else
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomOrig, Time.deltaTime * 8);
    }
    void movement()
    {
        if (!isDashing && Input.GetButtonDown("Dash"))
            StartCoroutine(dash());

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            if (!stepsIsPlaying && move.normalized.magnitude > 0.5f)
            {
                StartCoroutine(playSteps());
            }

            if (playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
                jumpedTimes = 0;
            }
        }

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && jumpedTimes < jumpMax)
        {
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
        }

        sprint();
        crouch();

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move((playerVelocity + dashDir) * Time.deltaTime);
    }

    IEnumerator dash()
    {
        isDashing = true;
        dashDir = Camera.main.transform.forward * dashSpeed;
        yield return new WaitForSeconds(dashTime);
        dashDir = Vector3.zero;
        isDashing = false;
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

        if (isSprinting && move != Vector3.zero)
        {
            if (!sprintEffect.isPlaying) 
               sprintEffect.Play(); // turns on sprint effect
        }
        else
        {
            if (sprintEffect.isPlaying) 
                sprintEffect.Stop(); //turns off sprint effect
        }
    }

    IEnumerator playSteps()
    {
        stepsIsPlaying = true;

        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);

        if (!isSprinting)
            yield return new WaitForSeconds(0.5f);
        else
            yield return new WaitForSeconds(0.2f);

        stepsIsPlaying = false;
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
        if (gunList[selectedGun].ammoCur > 0)
        {
            isShooting = true;
            gunList[selectedGun].ammoCur--;
            aud.PlayOneShot(audGunshot, audGunshotVol);
            updatePlayerUI();

            Vector2 direction = new Vector2(0.5f, 0.5f);

            RaycastHit hit;
            for (int i = 0; i < gunList[selectedGun].pellets; i++)
            {
                if (Physics.Raycast(Camera.main.ViewportPointToRay(direction), out hit, shootDist))
                {
                    IDamage damageable = hit.collider.GetComponent<IDamage>();

                    if (damageable != null)
                    {
                        damageable.takeDamage(shootDamage);
                    }
                    Instantiate(gunList[selectedGun].hitEffect, hit.point, Quaternion.identity);
                }
                direction = direction + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            }
            yield return new WaitForSeconds(shootRate);
            isShooting = false; 
        }
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
    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / playerHPOrig;
        gameManager.instance.playerOvershieldBar.fillAmount = (float)shieldHP / playerShieldOrig; 

        if (gunList.Count > 0)
        {
            gameManager.instance.ammoCurText.text = gunList[selectedGun].ammoCur.ToString("F0");
            gameManager.instance.ammoMaxText.text = gunList[selectedGun].ammoMax.ToString("F0");
        }
    }

    public void takeDamage(int dmg)
    {
        if (shieldHP > 0)
        {
            shieldHP -= dmg;
            aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol);
            updatePlayerUI();
            StartCoroutine(playerFlashDamage()); 
        }
        else
        {
            HP -= dmg;
            aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol);
            updatePlayerUI();
            StartCoroutine(playerFlashDamage());
        }

        if(HP <= 0)
        {
            gameManager.instance.loseMessageText.text = "You were killed";
            gameManager.instance.youLose();
        }
    }
    public void capture(GameObject flag)
    {
        Destroy(flag);
    }

    public void addShield(int addShieldHP)
    {
        shieldHP += addShieldHP;
        //if (shieldHP > shieldMax)
        //{
        //    shieldHP = shieldMax;
        //}
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

        aud.PlayOneShot(audSpawned, audSpawnedVol);
        updatePlayerUI();
    }

    public void gunPickup(gunStats gunStat)
    {
        gunList.Add(gunStat);

        shootDamage = gunStat.shootDamage;
        shootDist = gunStat.shootDist;
        shootRate = gunStat.shootRate;

        gunModel.GetComponent<MeshFilter>().mesh = gunStat.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().material = gunStat.model.GetComponent<MeshRenderer>().sharedMaterial;

        selectedGun = gunList.Count - 1;
        aud.PlayOneShot(audGunClick, audGunClickVol);
        updatePlayerUI();
    }

    void changeGun()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            changeGunStats();
            aud.PlayOneShot(audGunClick, audGunClickVol);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGunStats();
            aud.PlayOneShot(audGunClick, audGunClickVol);
        }
    }

    void changeGunStats()
    {
        shootDamage = gunList[selectedGun].shootDamage;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;

        gunModel.GetComponent<MeshFilter>().mesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().material = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;

        updatePlayerUI();
    }

    public void pickupAmmo(int amount, GameObject obj)
    {
        if(gunList.Count > 0)
        {
            int ammoDif = gunList[selectedGun].ammoMax - gunList[selectedGun].ammoCur;

            gunList[selectedGun].ammoCur += amount;

            if (gunList[selectedGun].ammoCur > gunList[selectedGun].ammoMax)
                gunList[selectedGun].ammoCur = gunList[selectedGun].ammoMax;

            ammoPickup ammopickup = obj.GetComponent<ammoPickup>();

            ammopickup.ammoAmount -= ammoDif;

            if (ammopickup.ammoAmount <= 0)
            {
                Destroy(obj);
            }

            updatePlayerUI();
            aud.PlayOneShot(audReload, audReloadVol);
        }
    }
}