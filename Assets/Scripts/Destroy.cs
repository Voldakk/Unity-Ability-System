using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float delay;

	void Start ()
    {
        GameObject.Destroy(gameObject, delay);
	}
}
