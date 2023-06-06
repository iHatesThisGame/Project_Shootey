using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{
    [SerializeField] int radius;
    [SerializeField] int speed;
    [SerializeField] float delay;
    [SerializeField] float force;
    public GameObject explosionEffect;

    [SerializeField] Rigidbody rb;

    float countdown;
    bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
        //Destroy(gameObject, delay);
        //rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0 && !hasExploded)
        {
            explode();
            hasExploded = true;
        }
    }

    void explode()
    {
        // show explosion
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // get nearby objects to deal damage and add force
        Collider [] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            // add force
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }

            // add damage
        }

        // remove grenade
    }
}
