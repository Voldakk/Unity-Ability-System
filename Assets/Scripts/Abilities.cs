using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Abilities
{
    public enum IconType { None, Normal, Weapon, WeaponNoAmmo }

    [RequireComponent(typeof(Character))]
    public class Abilities : MonoBehaviour
    {
        public GameObject _uiIconPrefab;
        public static GameObject uiIconPrefab;
        public GameObject _uiWeaponIconPrefab;
        public static GameObject uiWeaponIconPrefab;

        private CharacterStats stats;

        public List<Ability> abilities;


        void Awake()
        {
            uiIconPrefab = _uiIconPrefab;
            uiWeaponIconPrefab = _uiWeaponIconPrefab;

            stats = GetComponent<Character>().stats;
            abilities = stats.abilities.ToList();
        }
        void Start()
        {
            for (int i = abilities.Count - 1; i >= 0; i--)
            {
                abilities[i].Setup(
                    this.transform,
                    transform.Find("MainCamera").GetComponent<Camera>(),
                    transform.Find("MainCamera/WeaponCamera").GetComponent<Camera>(),
                    this);
                abilities[i].OnStart();
            }
        }

        public void Update()
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                if (Input.GetKeyDown(abilities[i].key))
                    abilities[i].KeyDown();
                if (Input.GetKey(abilities[i].key))
                    abilities[i].KeyHold();
                if (Input.GetKeyUp(abilities[i].key))
                    abilities[i].KeyUp();

                abilities[i].OnUpdate();
            }
        }
        public void Add(Ability ability)
        {
            abilities.Add(ability);
            ability.Setup(
                    this.transform,
                    transform.Find("MainCamera").GetComponent<Camera>(),
                    transform.Find("MainCamera/WeaponCamera").GetComponent<Camera>(),
                    this);
            ability.OnStart();
        }
        public bool Remove(Ability ability)
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i].GetInstanceID() == ability.GetInstanceID())
                {
                    abilities[i].OnDestroy();
                    abilities.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public bool Contains(Ability comparer, out Ability ability)
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i].GetInstanceID() == comparer.GetInstanceID())
                {
                    ability = abilities[i];
                    return true;
                }
            }
            ability = null;
            return false;
        }

        public static GameObject GetIconPrefab(IconType iconType)
        {
            switch (iconType)
            {
                case IconType.Normal:
                    return uiIconPrefab;
                case IconType.Weapon:
                case IconType.WeaponNoAmmo:
                    return uiWeaponIconPrefab;
            }

            return null;
        }
        void OnCollisionEnter(Collision collision)
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                abilities[i].OnCollisionEnter(collision);
            }
        }
    }
}