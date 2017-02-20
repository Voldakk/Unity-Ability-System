using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "New RayDamageHeal", menuName = "Abilities/RayDamageHeal")]
    public class RayDamageHeal : AutoTargetRay
    {
        protected override void OnKeyHold()
        {
            base.OnKeyHold();

            if (target != null)
            {
                Health health = target.GetComponent<Health>();
                if (health != null)
                {
                    if(currentDamage != 0)
                        health.Damage(currentDamage * Time.deltaTime);

                    if (currentHealing != 0)
                        health.Heal(currentHealing * Time.deltaTime);
                }
            }
        }
    }
}