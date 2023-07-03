using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heatSeekingBullet : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float destroyTimer = 3f;
    private Rigidbody rb;
    private Vector3 direction;
    public float speed { get; set; } = 100f;

    private void Start()
    {
        Destroy(gameObject, destroyTimer);
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();

        if (damageable != null)
        {
            damageable.takeDamage(damage);
        }

        Destroy(gameObject);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
