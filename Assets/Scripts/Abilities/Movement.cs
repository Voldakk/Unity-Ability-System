using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "New Movement", menuName = "Abilities/Movement")]
    public class Movement : Ability
    {
        public float speedModifier = 1;
        public float accelModifier = 1;
        public float decelModifier = 1;
        public float airborneAccelModifier = 1;
        public float jumpModifier = 1;
        public float fudgeMovifier = 1;
        public float maximumSlopeModifier = 1;
        
        private global::Movement movement;

        public override void OnStart()
        {
            base.OnStart();

            movement = transform.GetComponent<global::Movement>();
        }

        protected override void OnKeyDown()
        { 
            if (active)
                return;

            base.OnKeyDown();
            
            active = true;

            movement.MovementSpeed *= speedModifier;
            movement.AccelRate *= speedModifier;
            movement.DecelRate *= decelModifier;
            movement.AirborneAccel *= airborneAccelModifier;
            movement.JumpSpeed *= jumpModifier;
            movement.FudgeExtra *= fudgeMovifier;
            movement.MaximumSlope *= maximumSlopeModifier;
        }

        protected override void OnKeyUp()
        {
            base.OnKeyUp();
            Deactivate();
        }
        protected override void OnInterupt()
        {
            base.OnInterupt();
            Deactivate();
        }
        private void Deactivate()
        {
            if (active)
            {
                active = false;

                movement.MovementSpeed /= speedModifier;
                movement.AccelRate /= speedModifier;
                movement.DecelRate /= decelModifier;
                movement.AirborneAccel /= airborneAccelModifier;
                movement.JumpSpeed /= jumpModifier;
                movement.FudgeExtra /= fudgeMovifier;
                movement.MaximumSlope /= maximumSlopeModifier;
            }
        }
    }
}