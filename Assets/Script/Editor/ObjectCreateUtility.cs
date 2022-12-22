using UnityEditor;
using System;
using UnityEngine;
public static class ObjectCreateUtility
{
    const int riggerPriority = 2;
    const int pixelatePriority = 2;
    const int outlinePriority = 3;
    #region pixelation

    [MenuItem("GameObject/Pixelate", priority = pixelatePriority)]
    public static void Pixelate(MenuCommand menuCommand)
    {
        if (Selection.activeTransform == null)
        {
            return;
        }
        GameObject target = Selection.activeTransform.gameObject;
        if (!target.TryGetComponent(out SpriteRenderer sr))
            return;
        if (target.TryGetComponent(out PixelateSprite ps))
            return;
        target.AddComponent<PixelateSprite>();
    }
    #endregion

    #region rigging

    [MenuItem("GameObject/Rigger/", priority = riggerPriority)]
    [MenuItem("GameObject/Rigger/Bundle", priority =riggerPriority)]
    public static void CreateRiggerBundle(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab("RiggerBundle");
    }
    [MenuItem("GameObject/Rigger/Rigger", priority = riggerPriority)]
    public static void CreateRigger(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab("Rigger");
    }



    [MenuItem("GameObject/Rigger/BoneObject", priority = riggerPriority)]
    public static void CreateBoneObject(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab("BoneObject");
    }


    [MenuItem("GameObject/Rigger/Joint", priority = riggerPriority)]
    public static void CreateJoint(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab("Joint");
    }


    [MenuItem("GameObject/Rigger/PivotBundle", priority = riggerPriority)]
    public static void CreatePivot(MenuCommand menuCommand)
    {
        GameObject a = CreateUtility.CreatePrefab("PivotStart", Vector2.down*0.5f, false);
        GameObject b = CreateUtility.CreatePrefab("PivotEnd", Vector2.up*0.5f, false);
        Selection.objects = new GameObject[2] { a, b };
    }
    [MenuItem("GameObject/Rigger/PivotStart", priority = riggerPriority)]
    public static void CreatePivotStart(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab("PivotStart");
    }

    [MenuItem("GameObject/Rigger/PivotEnd", priority = riggerPriority)]
    public static void CreatePivotEnd(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab("PivotEnd");
    }
    #endregion

    #region outline
    [MenuItem("GameObject/MakeOutline", priority = outlinePriority)]
    public static void MakeOutline(MenuCommand menuCommand)
    {
        if (Selection.activeTransform == null)
        {
            return;
        }
        GameObject parent = Selection.activeTransform.gameObject;




        GameObject outline = CreateUtility.CreatePrefab("Outline");
        SpriteRenderer sr = outline.GetComponent<SpriteRenderer>();
        PixelOutlineRenderer por = outline.GetComponent<PixelOutlineRenderer>();


        sr.sprite = parent.GetComponent<SpriteRenderer>().sprite;
        sr.sortingOrder = parent.GetComponent<SpriteRenderer>().sortingOrder - 1;
        por.targetSprite = parent.GetComponent<PixelateSprite>();
    }
    #endregion

}
