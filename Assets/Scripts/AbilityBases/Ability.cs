using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Abilities
{
    public abstract class Ability : ScriptableObject
    {
        [HideInInspector]
        public Transform transform;
        [HideInInspector]
        public Camera mainCamera;
        [HideInInspector]
        public Camera weaponCamera;
        [HideInInspector]
        public Abilities abilities;

        // Activation key
        public KeyCode key;

        // List of abilities this ability is interrupted by
        public Ability[] interruptedBy;
        protected bool interrupted;

        // UI icon
        public IconType iconType;
        public Sprite icon;

        protected GameObject uiIcon;
        private GameObject uiActive;
        protected GameObject cooldownIcon;
        protected Text cooldownText;

        // Common fields
        public float cooldown;
        protected float lastUse;
        protected bool active;


        public float damage = 0;
        protected float currentDamage;
        public float healing = 0;
        protected float currentHealing;

        // Effects
        [HideInInspector]
        public List<Effect> effects;

        public void Setup(Transform playerTransform, Camera main, Camera weapon, Abilities playerAbilities)
        {
            transform = playerTransform;
            mainCamera = main;
            weaponCamera = weapon;
            abilities = playerAbilities;
        }

        /// <summary>
        /// Use this for initialization
        /// </summary>
        public virtual void OnStart()
        {
            effects = new List<Effect>();
            interrupted = false;
            active = false;

            lastUse = -cooldown;

            if (iconType != IconType.None)
            {
                uiIcon = (GameObject)GameObject.Instantiate(Abilities.GetIconPrefab(iconType), GameObject.Find("HUD/Abilities").transform, false);
                uiIcon.transform.Find("Icon").GetComponent<Image>().sprite = icon;

                if (iconType == IconType.Normal)
                {
                    uiActive = uiIcon.transform.Find("Active").gameObject;
                    cooldownIcon = uiIcon.transform.Find("Cooldown").gameObject;
                    cooldownIcon.SetActive(false);
                    cooldownText = cooldownIcon.transform.Find("Text").GetComponent<Text>();

                    uiIcon.transform.Find("Key/Text").GetComponent<Text>().text = key.ToString();
                }
                if (iconType == IconType.WeaponNoAmmo)
                {
                    uiIcon.transform.Find("Ammo").gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Triggers every frame
        /// </summary>
        public virtual void OnUpdate()
        {
            // Effects
            currentDamage = damage;
            currentHealing = healing;

            for (int i = 0; i < effects.Count; i++)
            {
                damage *= effects[i].damageModifier;
                healing *= effects[i].healingModifier;
            }

            // Interruptions
            bool interruptedLastFrame = interrupted;
            interrupted = false;

            Ability ability;
            for (int i = 0; i < interruptedBy.Length; i++)
            {
                if(abilities.Contains(interruptedBy[i], out ability))
                {
                    if (ability.active)
                    {
                        interrupted = true;
                    }
                }
            }

            if (interrupted && !interruptedLastFrame)
                OnInterupt();

            // Cooldown icon
            if (cooldown > 0 && cooldownIcon != null)
            {
                float cooldownRemaining = cooldown - (Time.time - lastUse);
                if(cooldownRemaining > 0)
                {
                    if(!cooldownIcon.activeSelf)
                        cooldownIcon.SetActive(true);
                    cooldownText.text = Mathf.CeilToInt(cooldownRemaining).ToString();
                }
                else if(cooldownIcon.activeSelf)
                {
                    cooldownIcon.SetActive(false);
                }
            }

            // Active icon
            if (iconType == IconType.Normal)
            {
                if (uiActive.activeSelf != active)
                    uiActive.SetActive(active);
            }
        }

        /// <summary>
        /// When the key in pressed
        /// </summary>
        public void KeyDown()
        {
            if (!interrupted)
                OnKeyDown();
        }
        protected virtual void OnKeyDown() { }

        /// <summary>
        /// When the key is relesed
        /// </summary>
        public void KeyUp()
        {
            if (!interrupted)
                OnKeyUp();
        }
        protected virtual void OnKeyUp() { }

        /// <summary>
        /// Triggers each frame the key is held down
        /// </summary>
        public void KeyHold()
        {
            if (!interrupted)
                OnKeyHold();
        }
        protected virtual void OnKeyHold() { }

        /// <summary>
        /// Triggers when interruped by another ability
        /// </summary>
        protected virtual void OnInterupt() { }

        /// <summary>
        /// When the gameobject is destroyed or the ability is otherwise removed
        /// </summary>
        public virtual void OnDestroy()
        {
            GameObject.Destroy(uiIcon);
        }

        /// <summary>
        /// Returns a reference to the weapon if it's already instantiated by another ability 
        /// </summary>
        /// <param name="prefab">The prefab to compare with</param>
        protected Transform FindExistingWeapon(GameObject prefab)
        {
            for (int i = 0; i < weaponCamera.transform.childCount; i++)
            {
                if (weaponCamera.transform.GetChild(i).name.Contains(prefab.name))
                    return weaponCamera.transform.GetChild(i);
            }
            return null;
        }
        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
        /// </summary>
        /// <param name="collision"></param>
        public virtual void OnCollisionEnter(Collision collision) { }
    }
}