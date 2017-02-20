using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Abilities.FlyToTarget))]
public class FlyToTargetEditor: Editor
{
    Abilities.FlyToTarget ability;

    public void OnEnable()
    {
        ability = (Abilities.FlyToTarget)target;
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

        ability.flySpeed = EditorGUILayout.FloatField("Fly speed", ability.flySpeed);
        ability.maxDistance = EditorGUILayout.FloatField("Max distance", ability.maxDistance);
        ability.maxAngle = EditorGUILayout.FloatField("Max angle", ability.maxAngle);
        ability.margin = EditorGUILayout.FloatField("Margin", ability.margin);

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