using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Abilities.Charge))]
public class ChargeEditor: Editor
{
    Abilities.Charge ability;

    public void OnEnable()
    {
        ability = (Abilities.Charge)target;
        ability.damage = 1;
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
        ability.cooldown = EditorGUILayout.FloatField("Cooldown", ability.cooldown);
        EditorGUILayout.Space();
        ability.speed = EditorGUILayout.FloatField("Speed", ability.speed);
        ability.steeringPower = EditorGUILayout.FloatField("Steering power", ability.steeringPower);
        EditorGUILayout.Space();
        ability.maxDistance = EditorGUILayout.FloatField("Max distance", ability.maxDistance);
        ability.maxTime = EditorGUILayout.FloatField("Max time", ability.maxTime);
        EditorGUILayout.Space();
        ability.pinDamage = EditorGUILayout.FloatField("Pin damage", ability.pinDamage);
        ability.pinDistance = EditorGUILayout.FloatField("Pin distance", ability.pinDistance);
        EditorGUILayout.Space();
        ability.bumpDamage = EditorGUILayout.FloatField("Bump damage", ability.bumpDamage);
        ability.bombKnockback = EditorGUILayout.FloatField("Bump knockback", ability.bombKnockback);
        EditorGUILayout.Space();
        ability.solidLayers = EditorGUICustomLayout.LayerMaskField("Solid layers", ability.solidLayers);
        ability.stopAngle = EditorGUILayout.FloatField("Stop angle", ability.stopAngle);

        EditorUtility.SetDirty(ability);
    }
}