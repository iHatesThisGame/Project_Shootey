using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunController : MonoBehaviour
{
    private gunRecoil recoilScript;
    // Start is called before the first frame update
    void Start()
    {
        recoilScript = GetComponentInChildren<gunRecoil>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            EquipGun();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            UnequipGun();
        }
    }
    private void EquipGun()
    {
        recoilScript.EquipGun();
    }

    private void UnequipGun()
    {
        recoilScript.UnequipGun();
    }
}
