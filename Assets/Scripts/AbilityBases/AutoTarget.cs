using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "New AutoTarget", menuName = "Abilities/AutoTarget")]
    public class AutoTarget : Ability
    {
        public TargetTeam team;

        public float maxDistance;
        public float maxAngle;

        protected Character character;
        protected Character target;
        protected Character lastTarget;
        protected float minAngle;

        public override void OnStart()
        {
            base.OnStart();

            character = transform.GetComponent<Character>();
        }

        protected override void OnKeyDown()
        {
            base.OnKeyDown();

            Findtarget();
        }

        protected override void OnKeyHold()
        {
            base.OnKeyHold();

            Findtarget();
        }

        protected override void OnKeyUp()
        {
            base.OnKeyUp();

            target = null;
        }

        private void Findtarget()
        {
            lastTarget = target;
            target = null;
            minAngle = float.MaxValue;

            for (int i = 0; i < Character.characters.Count; i++)
            {
                if (Character.characters[i].transform == transform                                                      // Self
                    || (team == TargetTeam.Friendly && character.team != Character.characters[i].team)                  // Enemy when targeting friendlies
                    || (team == TargetTeam.Enemy && character.team == Character.characters[i].team)                     // Friendly when targeting enemies 
                    || Character.characters[i].team == Team.None                                                        // Neutral
                    || Vector3.Distance(transform.position, Character.characters[i].transform.position) > maxDistance)  // Too far away
                    continue;

                float angle = Mathf.Abs(Vector3.Angle(weaponCamera.transform.forward, Character.characters[i].transform.position - weaponCamera.transform.position));
                if (angle <= maxAngle && angle < minAngle)
                {
                    if (Physics.Raycast(weaponCamera.transform.position, (Character.characters[i].transform.position - weaponCamera.transform.position), maxDistance))
                    {
                        target = Character.characters[i];
                        minAngle = angle;
                    }
                }
            }
        }
    }
}