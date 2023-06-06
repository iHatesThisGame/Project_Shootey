using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(3, 8)][SerializeField] float playerSpeed;
    [Range(8, 25)][SerializeField] float jumpHeight;
    [Range(10, 50)][SerializeField] float gravityValue;
    [SerializeField] int jumpMax;
    [SerializeField] bool sprintToggle;     // true means toggle mode, false means not toggle mode

    [Header("----- Gun Stats -----")]
    [Range(0.1f, 3)][SerializeField] float shootRate;
    [Range(1, 10)][SerializeField] int shootDamage;
    [Range(25, 1000)][SerializeField] int shootDist;
    [SerializeField] GameObject hitEffect;

    private int jumpedTimes;
    private bool isSprinting;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Vector3 move;
    bool isShooting;
    float sprintSpeed;
    float playerSpeedOrig;

    private void Start()
    {
        playerSpeedOrig = playerSpeed;
        sprintSpeed = playerSpeed * 2;
    }

    void Update()
    {
        movement();

        if (Input.GetButton("Shoot") && isShooting == false)
        {
            StartCoroutine(shoot());
        }
    }

    private void movement()
    {
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

    IEnumerator shoot()
    {
        isShooting = true;
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
}
