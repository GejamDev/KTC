using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public static class CreateUtility
{
    public static GameObject CreatePrefab(string path)
    {
        return CreatePrefab(path, Vector2.zero, true, false);
    }
    public static GameObject CreatePrefab(string path, Vector2 offset)
    {
        return CreatePrefab(path, offset, true, false);
    }
    public static GameObject CreatePrefab(string path, bool select)
    {
        return CreatePrefab(path, Vector2.zero, select, false);
    }
    public static GameObject CreatePrefab(string path, Vector2 offset, bool select)
    {
        return CreatePrefab(path, offset, select, false);
    }
    public static GameObject CreatePrefab(string path, Vector2 offset, bool select, bool unpack)
    {
        GameObject newObject = PrefabUtility.InstantiatePrefab(Resources.Load("Prefab/" + path)) as GameObject;
        Place(newObject, offset, select, unpack);
        return newObject;
    }

    public static void Place(GameObject gameobject, Vector3 offset, bool select, bool unpack)
    {
        //Find location
        if (Selection.activeTransform == null)
        {
            SceneView lastView = SceneView.lastActiveSceneView;
            gameobject.transform.position = lastView ? lastView.pivot : Vector3.zero;
        }
        else
        {
            gameobject.transform.SetParent(Selection.activeTransform);
            gameobject.transform.localPosition = Vector3.zero;
        }
        gameobject.transform.localPosition += offset;

        //Make sure we place the object in the proper scene, with a relevant name
        StageUtility.PlaceGameObjectInCurrentStage(gameobject);
        GameObjectUtility.EnsureUniqueNameForSibling(gameobject);

        //record undo and select
        Undo.RegisterCreatedObjectUndo(gameobject, $"Create Object : (gameobject.name)");
        if(select)
            Selection.activeObject = gameobject;

        //for prefabs, let's mark the scene as dirty for saving
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

        if (unpack)
        {
            PrefabUtility.UnpackPrefabInstance(gameobject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }


    }
}
