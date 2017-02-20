using UnityEngine;


[CreateAssetMenu(fileName = "New Generic", menuName = "Item/Generic")]
public class Data : ScriptableObject
{
    public float damage;
    public float healing;
    public float radius;
    public float duration;
}