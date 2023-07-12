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
        // offset makes the bullets less accurate by random amount between -0.3 and 0.3
        Vector3 offset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
        Destroy(gameObject, destroyTimer);
        rb.velocity = (gameManager.instance.player.transform.position - transform.position + offset) * speed;
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
