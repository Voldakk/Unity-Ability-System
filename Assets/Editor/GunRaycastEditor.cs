using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Abilities.GunRaycast))]
public class GunRaycastEditor : Editor
{
    Abilities.GunRaycast ability;
    static bool showMisc;

    public void OnEnable()
    {
        ability = (Abilities.GunRaycast)target;
    }
    public override void OnInspectorGUI()
    {
        ability.iconType = (Abilities.IconType)EditorGUILayout.EnumPopup("Icon type", ability.iconType);
        if (ability.iconType != Abilities.IconType.None)
            ability.icon = (Sprite)EditorGUILayout.ObjectField(ability.icon, typeof(Sprite), false, GUILayout.Width(65f), GUILayout.Height(65f));

        ability.key = (KeyCode)EditorGUILayout.EnumPopup("Activation key", ability.key);

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("interruptedBy"), true);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        ability.gunPrefab = (GameObject)EditorGUILayout.ObjectField("Weapon prefab", ability.gunPrefab, typeof(GameObject), false);

        ability.damage = EditorGUILayout.FloatField("Damage", ability.damage);
        ability.firerate = EditorGUILayout.FloatField("Firerate", ability.firerate);
        ability.ammo = EditorGUILayout.IntField("Ammo", ability.ammo);
        ability.autofire = EditorGUILayout.Toggle("Autofire (not semi)", ability.autofire);

        showMisc = EditorGUILayout.Foldout(showMisc, "Misc");
        if (showMisc)
        {
            ability.knockback = EditorGUILayout.FloatField("Map Sprite Width", ability.knockback);
            ability.lineTime = EditorGUILayout.FloatField("Map Sprite Width", ability.lineTime);
        }

        EditorUtility.SetDirty(ability);
    }
}