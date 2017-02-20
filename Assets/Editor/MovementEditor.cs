using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Abilities.Movement))]
public class MovementEditor: Editor
{
    Abilities.Movement ability;

    public void OnEnable()
    {
        ability = (Abilities.Movement)target;
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

        ability.accelModifier = EditorGUILayout.FloatField("Acceleration modifier", ability.accelModifier);
        ability.decelModifier = EditorGUILayout.FloatField("Decceleration modifier", ability.decelModifier);

        EditorGUILayout.Space();

        ability.airborneAccelModifier = EditorGUILayout.FloatField("Airborne acceleration modifier", ability.airborneAccelModifier);
        ability.jumpModifier = EditorGUILayout.FloatField("Jump modifier", ability.jumpModifier);

        EditorGUILayout.Space();

        ability.fudgeMovifier = EditorGUILayout.FloatField("Fudge modifier", ability.fudgeMovifier);
        ability.maximumSlopeModifier = EditorGUILayout.FloatField("Maximum slope modifier", ability.maximumSlopeModifier);

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