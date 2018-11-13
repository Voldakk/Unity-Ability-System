using UnityEngine;

public class Rocket : MonoBehaviour
{
    [HideInInspector]
    public bool explode;
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public float radius;
    [HideInInspector]
    public float force;

    public GameObject explosionPrefab;

    void OnCollisionEnter(Collision collision)
    {
        if(explode)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            Health health;

            for (int i = 0; i < colliders.Length; i++)
            {
                health = colliders[i].GetComponent<Health>();
                if (health != null)
                    health.Damage(damage);
            }

            colliders = Physics.OverlapSphere(transform.position, radius * 5);
            Rigidbody rb;
            for (int i = 0; i < colliders.Length; i++)
            {
                rb = colliders[i].GetComponent<Rigidbody>();
                if (rb != null)
                    rb.AddExplosionForce(force, transform.position, radius * 5, 3.0f);
            }
        }
        else
        {
            Health health = collision.transform.GetComponent<Health>();
            if (health != null)
                health.Damage(damage);
        }
        
        GameObject.Destroy(this.gameObject);
    }
}
