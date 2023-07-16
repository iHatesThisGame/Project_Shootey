using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heatSeekingBullet : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float destroyTimer = 3f;

    [SerializeField] private AudioClip laserSound;
    private Rigidbody rb;
    private Vector3 direction;
    public float speed { get; set; } = 5f;
    private AudioSource audioSource;

    private void Start()
    {
        Destroy(gameObject, destroyTimer);
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        Destroy(gameObject, destroyTimer);
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = laserSound;
        audioSource.playOnAwake = false;
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

        audioSource.Play();

        Destroy(gameObject);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
