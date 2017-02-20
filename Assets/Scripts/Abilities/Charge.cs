using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "New Charge", menuName = "Abilities/Charge")]
    public class Charge : Ability
    {
        public float speed;
        public float maxDistance;
        public float maxTime;
        public float pinDamage;
        public float pinDistance;
        public float bumpDamage;
        public float bombKnockback;
        public float steeringPower;
        public float stopAngle;

        private global::Movement movement;
        private Rigidbody rigidbody;
        private MouseLook mouseLook;
        private Character pinedCharacter;
        private float distanceTraveled;

        public LayerMask solidLayers;

        public override void OnStart()
        {
            base.OnStart();

            movement = transform.GetComponent<global::Movement>();
            rigidbody = transform.GetComponent<Rigidbody>();
            mouseLook = transform.GetComponent<MouseLook>();
        }

        protected override void OnKeyDown()
        {
            base.OnKeyDown();

            if (Time.time > lastUse + cooldown)
            {
                lastUse = Time.time;

                StartCharge();
            }
        }
        protected override void OnKeyHold()
        {
            base.OnKeyHold();
        }

        protected override void OnKeyUp()
        {
            base.OnKeyUp();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if(active)
            {
                distanceTraveled += (rigidbody.velocity).magnitude * Time.deltaTime;
                if (distanceTraveled >= maxDistance || Time.time > lastUse + maxTime)
                {
                    Stop();
                }
                else
                {
                    float inputX = Input.GetAxis("Horizontal");
                    rigidbody.AddRelativeForce(Vector3.forward  * speed, ForceMode.VelocityChange);
                    transform.Rotate(Vector3.up, inputX * steeringPower * Time.deltaTime, Space.Self);

                    if(pinedCharacter != null)
                    {
                        pinedCharacter.transform.position = transform.position + transform.forward * pinDistance;
                        pinedCharacter.transform.rotation = transform.rotation;
                    }
                }
            }
        }

        protected override void OnInterupt()
        {
            base.OnInterupt();
            Stop();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Stop();
        }

        public override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);
            if (!active)
                return;

            Character character = collision.transform.GetComponent<Character>();
            if (character != null)
            {
                Debug.Log("Collided with character " + character.gameObject.name);
                //float angle = Vector3.Angle(transform.forward, collision.contacts[0].point - transform.position);

                if (pinedCharacter == null)
                {
                    Debug.Log("Pinned " + character.gameObject.name);
                    Pin(character);
                }
                else
                {
                    character.GetComponent<Health>().Damage(bumpDamage * currentDamage);
                }
            }
            else if (solidLayers.Contains(collision.gameObject.layer) && Vector3.Angle(collision.contacts[0].normal, -transform.forward) < stopAngle)
            {
                if (pinedCharacter != null)
                {
                    pinedCharacter.GetComponent<Health>().Damage(pinDamage * currentDamage);
                }
                Stop();
            }
        }

        private void StartCharge()
        {
            active = true;
            movement.enabled = false;
            mouseLook.freeze = true;
            mouseLook.SetRotation(transform.rotation, mainCamera.transform.rotation);
            distanceTraveled = 0;
            weaponCamera.enabled = false;
            mainCamera.transform.localPosition = new Vector3(0, 1, -2);
        }
        private void Stop()
        {
            active = false;
            movement.enabled = true;

            rigidbody.velocity *= 0.2f;
            rigidbody.angularVelocity = Vector3.zero;

            mouseLook.SetRotation(transform.rotation, mainCamera.transform.rotation);
            mouseLook.freeze = false;

            if(pinedCharacter != null)
                Unpin();
            pinedCharacter = null;

            weaponCamera.enabled = true;
            mainCamera.transform.localPosition = new Vector3(0, 0.8f, 0);
        }
        private void Pin(Character character)
        {
            pinedCharacter = character;

            pinedCharacter.transform.position = transform.position + transform.forward * pinDistance;
            pinedCharacter.transform.rotation = transform.rotation;

            pinedCharacter.transform.SetParent(transform);
            pinedCharacter.gameObject.layer = 12;
        }
        private void Unpin()
        {
            pinedCharacter.transform.SetParent(null);
            pinedCharacter.gameObject.layer = 11;

            transform.Translate(Vector3.back * 2);
        }
    }
}