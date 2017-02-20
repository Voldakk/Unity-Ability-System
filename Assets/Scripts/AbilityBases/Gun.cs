using UnityEngine;
using UnityEngine.UI;

namespace Abilities
{
    public abstract class Gun : Ability
    {
        public float firerate;
        public bool autofire;
        public int ammo;
        public float knockback;

        public GameObject gunPrefab;

        protected Transform gun;
        protected Transform barrelEnd;
        protected Camera camera;
        protected Text ammoText;
        protected float lastFire;
        protected int currentAmmo;

        public override void OnStart()
        {
            base.OnStart();

            // Setup
            currentAmmo = ammo;
            lastFire = Time.time;

            // Create the gun object
            gun = FindExistingWeapon(gunPrefab);
            if (gun == null)
                gun = ((GameObject)GameObject.Instantiate(gunPrefab, weaponCamera.transform, false)).transform;

            // Find the barrel end
            barrelEnd = gun.Find("BarrelEnd");
            if (barrelEnd == null)
                Debug.LogError("Gun: No barrel end");

            // Create HUD element
            if (iconType == IconType.Weapon)
            {
                ammoText = uiIcon.transform.Find("Ammo/Text").GetComponent<Text>();
                ammoText.text = string.Format("{0}/{1}", currentAmmo, ammo);
            }

            lastFire = -1 / firerate;
            camera = transform.GetComponentInChildren<Camera>();
        }
        protected override void OnKeyDown()
        {
            Debug.Log("OnKeyDown");
            base.OnKeyDown();

            if (autofire)
                return;

            Debug.Log("OnKeyDown2");

            if (Time.time > lastUse + cooldown)
            {
                lastUse = Time.time;
                Debug.Log("OnKeyDown if, fire");
                Fire();
            }
        }
        protected override void OnKeyHold()
        {
            base.OnKeyHold();

            if (!autofire)
                return;

            if (Time.time > lastUse + cooldown)
            {
                lastUse = Time.time;
                Fire();
            }
        }
        protected void Reload()
        {
            currentAmmo = ammo;
            if (ammoText != null)
                ammoText.text = string.Format("Ammo: {0}/{1}", currentAmmo, ammo);
        }

        protected abstract void Fire();

        public override void OnDestroy()
        {
            base.OnDestroy();

            GameObject.Destroy(gun.gameObject);
            GameObject.Destroy(ammoText.gameObject);
        }
    }
}