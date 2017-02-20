using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Abilities.ChangeAbilities))]
public class ChangeAbilitiesEditor : Editor
{
    Abilities.ChangeAbilities ability;

    public void OnEnable()
    {
        ability = (Abilities.ChangeAbilities)target;
    }
    public override void OnInspectorGUI()
    {
        ability.iconType = (Abilities.IconType)EditorGUILayout.EnumPopup("Icon type", ability.iconType);
        if (ability.iconType != Abilities.IconType.None)
            ability.icon = (Sprite)EditorGUILayout.ObjectField(ability.icon, typeof(Sprite), false, GUILayout.Width(65f), GUILayout.Height(65f));

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("interruptedBy"), true);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        ability.scrollWheel = EditorGUILayout.Toggle("Use scroll wheel ", ability.scrollWheel);
        EditorGUILayout.Space();

        serializedObject.Update();

        ability.key1 = (KeyCode)EditorGUILayout.EnumPopup("Activation key 1", ability.key1);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("set1"), true);
        EditorGUILayout.Space();

        ability.key2 = (KeyCode)EditorGUILayout.EnumPopup("Activation key 2", ability.key2);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("set2"), true);

        serializedObject.ApplyModifiedProperties();

        EditorUtility.SetDirty(ability);
    }
}