using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterStats : ScriptableObject
{
    [Header("Health")]
    public float maxHealth;
    public float maxArmor;
    public float maxShield;

    [Header("Movement")]
    public float MovementSpeed = 7.0f;
    [Tooltip("Units per second acceleration")]
    public float AccelRate = 20.0f;
    [Tooltip("Units per second deceleration")]
    public float DecelRate = 20.0f;
    [Tooltip("Acceleration the player has in mid-air")]
    public float AirborneAccel = 5.0f;
    [Tooltip("The velocity applied to the player when the jump button is pressed")]
    public float JumpSpeed = 7.0f;
    [Tooltip("Extra units added to the player's fudge height... if you're rocketting off ramps or feeling too loosely attached to the ground, increase this. If you're being yanked down to stuff too far beneath you, lower this.")]
    public float FudgeExtra = 0.5f;
    [Tooltip("Maximum slope the player can walk up")]
    public float MaximumSlope = 45.0f;

    [Header("Abilities")]
    public Abilities.Ability[] abilities;
}
