using System.Collections;
using UnityEngine;

public class BioticField : MonoBehaviour
{
    public Data data;

    Collider[] colliders;
    Health health;

    void Start()
    {
        StartCoroutine(Destroy());
    }
    void Update ()
    {
        colliders = Physics.OverlapSphere(transform.position, data.radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            health = colliders[i].GetComponent<Health>();
            if (health != null)
                health.Heal(data.healing * Time.deltaTime);
        }
	}
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(data.duration);
        GameObject.Destroy(this.gameObject);
    }
}
