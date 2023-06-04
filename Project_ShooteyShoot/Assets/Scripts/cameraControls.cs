using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;
    [SerializeField] bool inverY;
    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //input reader
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        xRotation -= mouseY;

        //clamp xaxis
        xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax);

        //rotate x and y axis'
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
