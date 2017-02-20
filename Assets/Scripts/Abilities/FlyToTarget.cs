using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "New FlyToTarget", menuName = "Abilities/FlyToTarget")]
    public class FlyToTarget : AutoTarget
    {
        public float flySpeed;
        public float margin;

        private Transform currentTarget;
        private Rigidbody rigidbody;

        public override void OnStart()
        {
            base.OnStart();

            rigidbody = transform.GetComponent<Rigidbody>();
        }

        protected override void OnKeyDown()
        {
            base.OnKeyDown();

            if(currentTarget != null)
            {
                Interrupt();
            }
            else if(target != null)
            {
                if (Time.time > lastUse + cooldown)
                {
                    currentTarget = target.transform;
                    rigidbody.isKinematic = true;
                    lastUse = Time.time;
                    active = true;
                }
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if(currentTarget != null)
            {
                transform.Translate((currentTarget.position - transform.position).normalized * flySpeed * Time.deltaTime, Space.World);

                if(Vector3.Distance (transform.position, currentTarget.position) <= margin)
                {
                    Interrupt(); 
                }
            }
        }
        
        private void Interrupt()
        {
            currentTarget = null;
            rigidbody.isKinematic = false;
            active = false;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();

            Interrupt();
        }
    }
}