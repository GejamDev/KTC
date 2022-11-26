using UnityEditor;
using System;
using UnityEngine;
public static class ObjectCreateUtility
{
    [MenuItem("GameObject/Rigger/Bundle")]
    public static void CreateRiggerBundle(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab("RiggerBundle");
    }
    [MenuItem("GameObject/Rigger/Rigger")]
    public static void CreateRigger(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab("Rigger");
    }



    [MenuItem("GameObject/Rigger/BoneObject")]
    public static void CreateBoneObject(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab("BoneObject");
    }


    [MenuItem("GameObject/Rigger/Joint")]
    public static void CreateJoint(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab("Joint");
    }


    [MenuItem("GameObject/Rigger/PivotBundle")]
    public static void CreatePivot(MenuCommand menuCommand)
    {
        GameObject a = CreateUtility.CreatePrefab("PivotStart", Vector2.down*0.5f, false);
        GameObject b = CreateUtility.CreatePrefab("PivotEnd", Vector2.up*0.5f, false);
        Selection.objects = new GameObject[2] { a, b };
    }
    [MenuItem("GameObject/Rigger/PivotStart")]
    public static void CreatePivotStart(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab("PivotStart");
    }

    [MenuItem("GameObject/Rigger/PivotEnd")]
    public static void CreatePivotEnd(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab("PivotEnd");
    }

}
