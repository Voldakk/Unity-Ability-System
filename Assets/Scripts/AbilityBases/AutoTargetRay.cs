using UnityEngine;

namespace Abilities
{
    public class AutoTargetRay : AutoTarget
    {
        public Color rayColor;
        public GameObject weaponPrefab;

        protected Transform weapon;
        protected Transform barrelEnd;
        protected LineRenderer lr;

        public override void OnStart()
        {
            base.OnStart();

            // Create the gun object
            weapon = FindExistingWeapon(weaponPrefab);
            if(weapon == null)
                weapon = ((GameObject)GameObject.Instantiate(weaponPrefab, weaponCamera.transform, false)).transform;

            lr = weapon.GetComponent<LineRenderer>();
            lr.startColor = rayColor;
            lr.endColor = rayColor;

            // Find the barrel end
            barrelEnd = weapon.Find("BarrelEnd");
            if (barrelEnd == null)
                Debug.LogError("TargetRay: No barrel end");
        }

        protected override void OnKeyHold()
        {
            base.OnKeyHold();

            if (target != null)
            {
                active = true;
                lr.startColor = rayColor;
                lr.endColor = rayColor;
                lr.numPositions = 2;
                lr.SetPosition(0, barrelEnd.position);
                lr.SetPosition(1, target.transform.position);
            }
            else
            {
                active = false;
                lr.numPositions = 0;
            }
        }
        protected override void OnKeyUp()
        {
            base.OnKeyUp();
            active = false;

            lr.numPositions = 0;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            active = false;
            GameObject.Destroy(weapon.gameObject);
        }
        protected override void OnInterupt()
        {
            base.OnInterupt();

            lr.numPositions = 0;
        }
    }
}