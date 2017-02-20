using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Abilities.GunProjectile))]
public class GunProjectileEditor : Editor
{
    Abilities.GunProjectile ability;

    public void OnEnable()
    {
        ability = (Abilities.GunProjectile)target;
    }
    public override void OnInspectorGUI()
    {
        ability.iconType = (Abilities.IconType)EditorGUILayout.EnumPopup("Icon type", ability.iconType);
        if(ability.iconType != Abilities.IconType.None)
            ability.icon = (Sprite)EditorGUILayout.ObjectField(ability.icon, typeof(Sprite), false, GUILayout.Width(65f), GUILayout.Height(65f));

        ability.key = (KeyCode)EditorGUILayout.EnumPopup("Activation key", ability.key);

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("interruptedBy"), true);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        ability.cooldown = EditorGUILayout.FloatField("Cooldown", ability.cooldown);

        EditorGUILayout.Space();

        ability.gunPrefab = (GameObject)EditorGUILayout.ObjectField("Weapon prefab", ability.gunPrefab, typeof(GameObject), false);
        ability.projectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile prefab", ability.projectilePrefab, typeof(GameObject), false);

        ability.damage = EditorGUILayout.FloatField("Damage", ability.damage);
        ability.firerate = EditorGUILayout.FloatField("Firerate", ability.firerate);
        ability.ammo = EditorGUILayout.IntField("Ammo", ability.ammo);
        ability.autofire = EditorGUILayout.Toggle("Autofire (not semi)", ability.autofire);
        ability.knockback = EditorGUILayout.FloatField("Knockback", ability.knockback);

        ability.explosion = EditorGUILayout.Toggle("Explosion", ability.explosion);
        if (ability.explosion)
        {
            ability.radius = EditorGUILayout.FloatField("Radius", ability.radius);
        }

        EditorUtility.SetDirty(ability);
    }
}