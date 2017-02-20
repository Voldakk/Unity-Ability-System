using System;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "New RayEffect", menuName = "Abilities/RayEffect")]
    public class RayEffect : AutoTargetRay
    {
        public Effect effect;
        
        protected override void OnKeyHold()
        {
            base.OnKeyHold();

            if(lastTarget != null && lastTarget != target)
            {
                Remove(lastTarget);
            }
            if (target != null)
            {
                Apply(target); 
            }
        }
        private void Apply(Character character)
        {
            Abilities targetAbilities = target.GetComponent<Abilities>();
            if (targetAbilities != null)
            {
                for (int i = 0; i < targetAbilities.abilities.Count; i++)
                {
                    targetAbilities.abilities[i].effects.Add(effect);
                }
            }
        }
        private void Remove(Character character)
        {
            Abilities targetAbilities = target.GetComponent<Abilities>();
            if (targetAbilities != null)
            {
                for (int i = 0; i < targetAbilities.abilities.Count; i++)
                {
                    targetAbilities.abilities[i].effects.Remove(effect);
                }
            }
        }
    }

    [Serializable]
    public class Effect
    {
        public float damageModifier = 1;
        public float healingModifier = 1;
    }
}