using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "New Placeable", menuName = "Abilities/Placeable")]
    public class Placeable : Ability
    {
        public GameObject prefab;

        protected override void OnKeyDown()
        {
            base.OnKeyDown();

            if(Time.time > lastUse + cooldown)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, -transform.up, out hit, 100f))
                {
                    lastUse = Time.time;
                    GameObject.Instantiate(prefab, hit.point, Quaternion.identity);
                }
            }
        }
    }
}