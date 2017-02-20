using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "New ChangeAbilities", menuName = "Abilities/ChangeAbilities")]
    public class ChangeAbilities : Ability
    {
        public KeyCode key1;
        public KeyCode key2;
        public bool scrollWheel = true;
        public List<Ability> set1;
        public List<Ability> set2;

        private int currentSet;

        public override void OnStart()
        {
            base.OnStart();
            currentSet = 1;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Input.GetKeyDown(key1))
                abilities.StartCoroutine(Change1());
            if (Input.GetKeyDown(key2))
                abilities.StartCoroutine(Change2());

            if (scrollWheel)
            {
                float dScroll = Input.GetAxis("Mouse ScrollWheel");
                if (dScroll > 0f)
                {
                    abilities.StartCoroutine(Change2());
                }
                else if (dScroll < 0f)
                {
                    abilities.StartCoroutine(Change1());
                }
            }
        }

        public IEnumerator Change1()
        {
            yield return new WaitForEndOfFrame();

            if (currentSet == 1)
            {
                for (int i = 0; i < set1.Count; i++)
                {
                    abilities.Remove(set1[i]);
                }
                for (int i = 0; i < set2.Count; i++)
                {
                    abilities.Add(set2[i]);
                }
                currentSet = 2;
            }
        }
        public IEnumerator Change2()
        {
            yield return new WaitForEndOfFrame();

            if (currentSet == 2)
            {
                for (int i = 0; i < set2.Count; i++)
                {
                    abilities.Remove(set2[i]);
                }
                for (int i = 0; i < set1.Count; i++)
                {
                    abilities.Add(set1[i]);
                }
                currentSet = 1;
            }
        }
    }
}