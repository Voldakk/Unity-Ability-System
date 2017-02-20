using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "New PassiveHeal", menuName = "Abilities/PassiveHeal")]
    public class PassiveHeal : Ability
    {
        public float delay;

        private Health health;

        public override void OnStart()
        {
            base.OnStart();

            health = transform.GetComponent<Health>();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();

            active = (Time.time > health.lastDamageTime + delay);
            if (active)
                health.Heal(currentHealing * Time.deltaTime);
        }
    }
}