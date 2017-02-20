using System.Collections;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "New Melee", menuName = "Abilities/Melee")]
    public class Melee : Ability
    {
        public GameObject prefab;
        public float radius;
        public float maxAngle;
        public float hitDelay;

        private Transform weapon;
        private Animator anim;


        public override void OnStart()
        {
            base.OnStart();

            // Create the weapon object
            weapon = FindExistingWeapon(prefab);
            if (weapon == null)
                weapon = ((GameObject)GameObject.Instantiate(prefab, weaponCamera.transform, false)).transform;
            anim = weapon.GetComponent<Animator>();

        }
        protected override void OnKeyDown()
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Melee") || anim.IsInTransition(0))
                return;
       
            base.OnKeyDown();

            anim.SetTrigger("Melee");
            active = true;

            GameManager.instance.StartCoroutine(ApplyDamage());
        }
        public override void OnUpdate()
        {
            base.OnUpdate();

            active = anim.GetCurrentAnimatorStateInfo(0).IsName("Melee") || anim.IsInTransition(0);
        }
        public IEnumerator ApplyDamage()
        {
            yield return new WaitForSeconds(hitDelay);

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            Health health;
            float angle;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].transform == transform)
                    continue;
                Vector3[] points = new Vector3[]
                {
                    colliders[i].bounds.ClosestPoint(weapon.transform.position),
                    colliders[i].bounds.center,
                    new Vector3(colliders[i].bounds.min.x, colliders[i].bounds.min.y, colliders[i].bounds.min.z),
                    new Vector3(colliders[i].bounds.min.x, colliders[i].bounds.min.y, colliders[i].bounds.max.z),
                    new Vector3(colliders[i].bounds.min.x, colliders[i].bounds.max.y, colliders[i].bounds.min.z),
                    new Vector3(colliders[i].bounds.min.x, colliders[i].bounds.max.y, colliders[i].bounds.max.z),
                    new Vector3(colliders[i].bounds.max.x, colliders[i].bounds.min.y, colliders[i].bounds.min.z),
                    new Vector3(colliders[i].bounds.max.x, colliders[i].bounds.min.y, colliders[i].bounds.max.z),
                    new Vector3(colliders[i].bounds.max.x, colliders[i].bounds.max.y, colliders[i].bounds.min.z),
                    new Vector3(colliders[i].bounds.max.x, colliders[i].bounds.max.y, colliders[i].bounds.max.z)
                };
                for (int n = 0; n < points.Length; n++)
                {
                    angle = Mathf.Abs(Vector3.Angle(weaponCamera.transform.forward, points[n] - weaponCamera.transform.position));
                    if (angle <= maxAngle)
                    {
                        health = colliders[i].GetComponent<Health>();
                        if (health != null)
                        {
                            health.Damage(currentDamage);
                            break;
                        }
                    }
                }                  
            }
        }
    }
}