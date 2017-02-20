using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "New GunProjectile", menuName = "Abilities/GunProjectile")]
    public class GunProjectile : Gun
    {
        public GameObject projectilePrefab;
        public float projectileSpeed;
        public float radius;
        public bool explosion;
        protected override void Fire()
        {
            Debug.Log("Fire");
            if (Time.time >= lastFire + 1 / firerate)
            {
                if (currentAmmo > 0)
                {
                    lastFire = Time.time;
                    currentAmmo--;
                    if (ammoText != null)
                        ammoText.text = string.Format("{0}/{1}", currentAmmo, ammo);

                    Ray ray = camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                    RaycastHit hit;
                    GameObject projectile = (GameObject)GameObject.Instantiate(projectilePrefab);

                    Vector3 force = Vector3.zero;
                    if (Physics.Raycast(ray, out hit, 10000))
                    {
                        force = (hit.point-barrelEnd.position).normalized * projectileSpeed;
                    }
                    else
                    {
                        force = ((ray.origin + ray.direction * 100) - barrelEnd.position).normalized * projectileSpeed;
                    }

                    projectile.transform.position = barrelEnd.position;
                    projectile.transform.rotation = Quaternion.LookRotation(force);
                    projectile.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);


                    Rocket rocket = projectile.GetComponent<Rocket>();
                    if(rocket != null)
                    {
                        rocket.damage = currentDamage;
                        rocket.radius = radius;
                        rocket.force = knockback;
                        rocket.explode = explosion;
                    }

                    if(currentAmmo == 0)
                        Reload();
                }
                else
                {
                    Reload();
                }
            }
        }
    }
}