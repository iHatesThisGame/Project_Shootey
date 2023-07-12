using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunRecoil : MonoBehaviour
{
    [SerializeField] public float upwardForce = 0.5f;
    [SerializeField] public float backwardForce = 0.1f;
    [SerializeField] public float recoilDuration = 0.1f;

    [SerializeField] public float shakeDuration = 0.1f;
    [SerializeField] public float shakeIntensity = 0.1f;

    private Vector3 originalPosition;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isShooting = false;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition;
        originalCameraPosition = Camera.main.transform.localPosition;
        originalCameraRotation = Camera.main.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            isShooting = true;
            StartCoroutine(ApplyRecoilCoroutine());
            StartCoroutine(ShakeScreenCoroutine());
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)) 
        {
            isShooting = false;
        }
    }

    public IEnumerator ApplyRecoilCoroutine()
    {
        while (isShooting)
        {
            float elapsedTime = 0f;
            while (elapsedTime < recoilDuration)
            {
                float upwardRecoilAmount = Mathf.Lerp(0f, upwardForce, elapsedTime / recoilDuration);
                float backwardRecoilAmount = Mathf.Lerp(0f, backwardForce, elapsedTime / recoilDuration);

                Vector3 recoilPosition = originalPosition + Vector3.up * upwardRecoilAmount - transform.forward * backwardRecoilAmount; // Modified line
                transform.localPosition = recoilPosition;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            float finalBackwardRecoilAmount = Mathf.Lerp(0f, backwardForce, 1f);
            Vector3 finalRecoilPosition = originalPosition - transform.forward * finalBackwardRecoilAmount; 
            transform.localPosition = finalRecoilPosition;

            yield return new WaitForSeconds(0.1f); 

            transform.localPosition = originalPosition;

            yield return null;
        }
    }

    private IEnumerator ShakeScreenCoroutine()
    {
        while (isShooting)
        {
            float elapsedTime = 0f;
            while (elapsedTime < shakeDuration)
            {
                float shakeOffsetX = Random.Range(-shakeIntensity, shakeIntensity);
                float shakeOffsetY = Random.Range(-shakeIntensity, shakeIntensity);
                float shakeOffsetZ = Random.Range(-shakeIntensity, shakeIntensity);

                Vector3 shakeOffset = new Vector3(shakeOffsetX, shakeOffsetY, shakeOffsetZ);
                Camera.main.transform.localPosition = originalCameraPosition + shakeOffset;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Camera.main.transform.localPosition = originalCameraPosition;

            yield return null;
        }
    }
}
