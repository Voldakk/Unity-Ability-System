using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Abilities.AnimationTrigger))]
public class AnimationTriggerEditor : Editor
{
    Abilities.AnimationTrigger ability;

    public void OnEnable()
    {
        ability = (Abilities.AnimationTrigger)target;
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

        ability.cooldown = EditorGUILayout.FloatField("Cooldown", ability.cooldown);

        ability.ability = (Abilities.Ability)EditorGUILayout.ObjectField("Ability", ability.ability, typeof(Abilities.Ability), false);
        ability.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", ability.prefab, typeof(GameObject), false);

        ability.delay = EditorGUILayout.FloatField("Delay", ability.delay);

        ability.animationName = EditorGUILayout.TextField("Animation name", ability.animationName);
        ability.animationTrigger = EditorGUILayout.TextField("Animation trigger", ability.animationTrigger);

        EditorUtility.SetDirty(ability);
    }
}

/*
// Int
ability.varInt = EditorGUILayout.IntField("VarInt", ability.varInt);

// Float
ability.varFloat = EditorGUILayout.FloatField("VarFloat", ability.varFloat);

// Bool
ability.varBool = EditorGUILayout.Toggle("VarBool", ability.varBool);

// Prefab
ability.varPrefab = (GameObject)EditorGUILayout.ObjectField("Var prefab", ability.varPrefab, typeof(GameObject), false);

// LayerMask
ability.varLayerMask = EditorGUICustomLayout.LayerMaskField("VarLayerMask", ability.varLayerMask);

// List<T>
serializedObject.Update();
EditorGUILayout.PropertyField(serializedObject.FindProperty("varList"), true);
serializedObject.ApplyModifiedProperties();
*/