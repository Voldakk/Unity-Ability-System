using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "New SlowFall", menuName = "Abilities/SlowFall")]
    public class SlowFall : Ability
    {
        public float gravity = 9.81f;

        private global::Movement movement;
        private Rigidbody rigidbody;

        public override void OnStart()
        {
            base.OnStart();

            active = false;

            movement = transform.GetComponent <global::Movement>();
            rigidbody = transform.GetComponentInChildren<Rigidbody>();
        }
        protected override void OnKeyHold()
        {
            base.OnKeyHold();

            if(movement.FallingDown && !active)
            {
                active = true;
                rigidbody.useGravity = false;
            }
            else if(!movement.FallingDown && active)
            {
                active = false;
                rigidbody.useGravity = true;
            }
        }
        public override void OnUpdate()
        {
            base.OnUpdate();

            if(active)
            {
                rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
            }
        }
        protected override void OnKeyUp()
        {
            base.OnKeyUp();

            active = false;
            rigidbody.useGravity = true;
        }
    }
}