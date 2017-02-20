using System.Collections;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "New GunRaycast", menuName = "Abilities/GunRaycast")]
    public class GunRaycast : Gun
    {
        public float lineTime;

        private LineRenderer lr;

        public override void OnStart()
        {
            base.OnStart();

            lr = gun.GetComponent<LineRenderer>();
        }

        protected override void Fire()
        {
            if (Time.time >= lastFire + 1 / firerate)
            {
                if (currentAmmo > 0)
                {
                    lastFire = Time.time;
                    currentAmmo--;
                    if (ammoText != null)
                        ammoText.text = string.Format("{0}/{1}", currentAmmo, ammo);
                    RaycastHit hit;
                    Ray ray = camera.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        if (lineTime > 0)
                        {
                            lr.SetPosition(0, barrelEnd.position);
                            lr.SetPosition(1, hit.point);
                            GameManager.instance.StartCoroutine(ClearLine());
                        }

                        Health health = hit.transform.GetComponent<Health>();
                        if (health != null)
                            health.Damage(currentDamage);

                        Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
                        if (rb != null)
                            rb.AddForceAtPosition(ray.direction * knockback, hit.point);
                    }
                    else if (lineTime > 0)
                    {
                        lr.SetPosition(0, barrelEnd.position);
                        lr.SetPosition(1, ray.origin + ray.direction*1000);
                        GameManager.instance.StartCoroutine(ClearLine());
                    }

                    if (currentAmmo == 0)
                        Reload();
                }
                else
                {
                    Reload();
                }
            }
        }

        IEnumerator ClearLine()
        {
            yield return new WaitForSeconds(lineTime);
            if (Time.time > lastFire + lineTime)
            {
                lr.SetPosition(0, Vector3.down);
                lr.SetPosition(1, Vector3.down);
            }
        }
    }
}