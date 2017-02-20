using UnityEngine;

public static class Extensions
{
    public static bool Contains(this LayerMask mask, int layer)
    {
        return ((mask.value & (1 << layer)) > 0);
    }

    public static bool Contains(this LayerMask mask, GameObject obj)
    {
        return ((mask.value & (1 << obj.layer)) > 0);
    }
}