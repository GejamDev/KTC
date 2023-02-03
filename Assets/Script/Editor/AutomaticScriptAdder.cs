#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class AutomaticScriptAdder
{
    static AutomaticScriptAdder()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    private static void OnHierarchyChanged()
    {
        //return at some condition (doesn't have anything yet)



        var objects = Object.FindObjectsOfType<GameObject>();
        foreach (var obj in objects)
        {
            if (obj.TryGetComponent(out MeshRenderer mr) || obj.TryGetComponent(out SkinnedMeshRenderer smr))
            {
                if (!obj.GetComponent<Object3D>())
                {
                    obj.AddComponent<Object3D>();
                }
            }
            if(obj.TryGetComponent(out SpriteRenderer sr))
            {
                if (!obj.GetComponent<PixelateSprite>())
                {
                    obj.AddComponent<PixelateSprite>();
                }
            }
        }
    }
}
#endif