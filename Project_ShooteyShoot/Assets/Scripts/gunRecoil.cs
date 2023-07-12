using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunRecoil : MonoBehaviour
{
    [SerializeField] public float upwardForce = 0.5f;// The upward force applied during recoil
    [SerializeField] public float backwardForce = 0.1f;// The backward force applied during recoil
    [SerializeField] public float recoilDuration = 0.1f;// The duration of the recoil effect

    [SerializeField] public float shakeDuration = 0.1f;// The duration of the screen shake effect
    [SerializeField] public float shakeIntensity = 0.1f; // The intensity of the screen shake effect

    private Vector3 originalPosition;// The original position of the gun
    private Vector3 originalCameraPosition;// The original position of the camera
    private Quaternion originalCameraRotation; // The original rotation of the camera
    private bool isShooting = false;// Flag indicating if the gun is currently shooting
    private bool isEquipped = false;// Flag indicating if the gun is currently equipped
    private int bulletCount = 25;// The number of bullets remaining

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition; // Store the initial local position of the gun
        originalCameraPosition = Camera.main.transform.localPosition;// Store the initial local position of the camera
        originalCameraRotation = Camera.main.transform.localRotation;// Store the initial local rotation of the camera
    }

    // Update is called once per frame
    void Update()
    {
        if (isEquipped && Input.GetKeyDown(KeyCode.Mouse0))// Check if the gun is equipped and the left mouse button is pressed
        {
            {
                if (bulletCount > 0)// Check if there are bullets remaining
                {
                    isShooting = true;// Start shooting
                    StartCoroutine(ApplyRecoilCoroutine());// Start the recoil coroutine
                    bulletCount--;// Reduce the bullet count
                    StartCoroutine(ShakeScreenCoroutine());// Start the screen shake coroutine
                }
                else
                {
                    Debug.Log("Out of Bullets!");
                    isShooting = false; // Stop shooting
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)) // Check if the left mouse button is released
        {
            isShooting = false; // Stop shooting
        }

        // Check if the bullet count reaches zero and disable shooting
        if (bulletCount <= 0)
        {
            isShooting = false; // Stop shooting
        }
    }

    // Reload the gun with a specific ammo count
    public void ReloadGun(int ammoCount)
    {
        bulletCount += ammoCount; // Increase the bullet count
    }

    // Set the bullet count to a specific value
    public void SetBulletCount(int count)
    {
        bulletCount = count; // Set the bullet count
    }

    // Equip the gun
    public void EquipGun()
    {
        isEquipped = true; // Set the equipped flag to true
    }

    // Unequip the gun
    public void UnequipGun()
    {
        isEquipped = false; // Set the equipped flag to false
    }

    // Coroutine for applying recoil
    public IEnumerator ApplyRecoilCoroutine()
    {
        if (!isEquipped)
            yield break; // If the gun is not equipped, exit the coroutine

        if (bulletCount <= 0)
            yield break; // If there are no bullets remaining, exit the coroutine

        float elapsedTime = 0f; // Elapsed time since the start of the coroutine

        while (elapsedTime < recoilDuration)
        {
            float upwardRecoilAmount = Mathf.Lerp(0f, upwardForce, elapsedTime / recoilDuration); // Calculate the upward recoil amount based on the elapsed time
            float backwardRecoilAmount = Mathf.Lerp(0f, backwardForce, elapsedTime / recoilDuration); // Calculate the backward recoil amount based on the elapsed time

            Vector3 recoilPosition = originalPosition + Vector3.up * upwardRecoilAmount - transform.forward * backwardRecoilAmount; // Calculate the new recoil position
            transform.localPosition = recoilPosition; // Apply the recoil position to the gun

            elapsedTime += Time.deltaTime; // Increase the elapsed time
            yield return null; // Wait for the next frame
        }

        transform.localPosition = originalPosition; // Reset the gun position to the original position
    }

    // Coroutine for shaking the screen
    private IEnumerator ShakeScreenCoroutine()
    {
        while (isShooting) // Continue shaking the screen while shooting
        {
            float elapsedTime = 0f; // Elapsed time since the start of the coroutine

            while (elapsedTime < shakeDuration)
            {
                float shakeOffsetX = Random.Range(-shakeIntensity, shakeIntensity); // Random shake offset along the X-axis
                float shakeOffsetY = Random.Range(-shakeIntensity, shakeIntensity); // Random shake offset along the Y-axis
                float shakeOffsetZ = Random.Range(-shakeIntensity, shakeIntensity); // Random shake offset along the Z-axis

                Vector3 shakeOffset = new Vector3(shakeOffsetX, shakeOffsetY, shakeOffsetZ); // Combine the shake offsets into a vector
                Camera.main.transform.localPosition = originalCameraPosition + shakeOffset; // Apply the shake offset to the camera position

                elapsedTime += Time.deltaTime; // Increase the elapsed time
                yield return null; // Wait for the next frame
            }

            Camera.main.transform.localPosition = originalCameraPosition; // Reset the camera position to the original position

            yield return null; // Wait for the next frame
        }
    }
}
