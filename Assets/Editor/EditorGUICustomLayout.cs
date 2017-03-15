using UnityEngine;
using UnityEditorInternal;
using System.Collections.Generic;

public class EditorGUICustomLayout
{
    static List<int> layerNumbers = new List<int>();

    public static LayerMask LayerMaskField(string label, LayerMask layerMask)
    {
        var layers = InternalEditorUtility.layers;

        layerNumbers.Clear();

        for (int i = 0; i < layers.Length; i++)
            layerNumbers.Add(LayerMask.NameToLayer(layers[i]));

        int maskWithoutEmpty = 0;
        for (int i = 0; i < layerNumbers.Count; i++)
        {
            if (((1 << layerNumbers[i]) & layerMask.value) > 0)
                maskWithoutEmpty |= (1 << i);
        }

        maskWithoutEmpty = UnityEditor.EditorGUILayout.MaskField(label, maskWithoutEmpty, layers);

        int mask = 0;
        for (int i = 0; i < layerNumbers.Count; i++)
        {
            if ((maskWithoutEmpty & (1 << i)) > 0)
                mask |= (1 << layerNumbers[i]);
        }
        layerMask.value = mask;

        return layerMask;
    }
}