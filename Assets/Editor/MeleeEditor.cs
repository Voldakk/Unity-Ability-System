using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Abilities.Melee))]
public class MeleeEditor : Editor
{
    Abilities.Melee ability;

    public void OnEnable()
    {
        ability = (Abilities.Melee)target;
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

        ability.prefab = (GameObject)EditorGUILayout.ObjectField("Weapon prefab", ability.prefab, typeof(GameObject), false);

        ability.damage = EditorGUILayout.FloatField("Damage", ability.damage);
        ability.radius = EditorGUILayout.FloatField("Radius", ability.radius);
        ability.maxAngle = EditorGUILayout.FloatField("Max angle", ability.maxAngle);
        ability.hitDelay = EditorGUILayout.FloatField("Hit delay", ability.hitDelay);

        EditorUtility.SetDirty(ability);
    }
}