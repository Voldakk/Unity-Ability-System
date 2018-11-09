using System.Collections;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "New AnimationTrigger", menuName = "Abilities/AnimationTrigger")]
    public class AnimationTrigger : Ability
    {
        public GameObject prefab;
        public string animationTrigger;
        public string animationName;
        public float delay;
        public Ability ability;

        private Transform item;
        private Animator anim;

        public override void OnStart()
        {
            base.OnStart();

            // Create the item
            item = FindExistingWeapon(prefab);
            if (item == null)
                item = Instantiate(prefab, weaponCamera.transform, false).transform;
            anim = item.GetComponent<Animator>();

        }
        protected override void OnKeyDown()
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(animationName))
                return;

            base.OnKeyDown();

            if (Time.time > lastUse + cooldown)
            {
                lastUse = Time.time;

                anim.SetTrigger(animationTrigger);
                active = true;

                GameManager.instance.StartCoroutine(Trigger());
            }
        }
        public override void OnUpdate()
        {
            base.OnUpdate();

            active = anim.GetCurrentAnimatorStateInfo(0).IsName(animationName);
        }
        public IEnumerator Trigger()
        {
            yield return new WaitForSeconds(delay);
            ability.KeyDown();
        }
    }
}