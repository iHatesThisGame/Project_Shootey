using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTimer;

    [SerializeField] Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {

        Destroy(gameObject, destroyTimer);
        rb.velocity = (gameManager.instance.player.transform.position - transform.position) * speed;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        IDamage damageble = other.GetComponent<IDamage>();

        if (damageble != null )
        {
            damageble.takeDamage(damage);
        }

        Destroy(gameObject);
    }
}
