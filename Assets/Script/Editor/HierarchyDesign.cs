using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
//[InitializeOnLoad]
//public class HierarchyColors
//{
//    [System.NonSerialized]
//    public static Color backgroundColor;
//    static HierarchyColors()
//    {
//        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
//    }

//    static void OnHierarchyGUI(int instanceID, Rect selectionRect)
//    {
//        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

//        if (gameObject == null)
//            return;

//        if (gameObject.name.Contains("Cube"))
//        {
//            Rect r = new Rect(selectionRect);
//            r.x = 0;
//            r.width = selectionRect.width;
//            r.height = selectionRect.height;
//            EditorGUI.DrawRect(r, backgroundColor);

//            // Draw the object's name in white to make it visible
//            var style = new GUIStyle();
//            style.fontStyle = FontStyle.Bold;
//            style.normal.textColor = Color.white;
//            GUI.color = Color.white;
//            GUI.Label(selectionRect, gameObject.name,style);

//        }
//    }
//}



[InitializeOnLoad]
public static class HierarchyColors
{
    // The global static color value that you want to use as the background color for object names
    public static Color backgroundColor3D = Color.blue;


    static HierarchyColors()
    {

        // Set up a delegate to handle drawing the hierarchy
        EditorApplication.hierarchyWindowItemOnGUI += (int instanceID, Rect rect) =>
        {
            // Get the object for the instance ID
            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject != null)
            {
                // Check if the object's name contains "Cube"
                if (gameObject.TryGetComponent<Object3D>(out Object3D i))
                {

                    #region background
                    //// Calculate the position for the background rect
                    //Rect backgroundRect = new Rect(rect.x, rect.y, rect.width, rect.height);

                    //// Draw the background rect with the background color
                    //EditorGUI.DrawRect(backgroundRect, backgroundColor3D);
                    //// Draw the object's name in white to make it visible
                    //var style = new GUIStyle();
                    //style.fontStyle = FontStyle.Bold;
                    //style.normal.textColor = Color.white;
                    //GUI.color = Color.white;
                    //GUI.Label(backgroundRect, gameObject.name, style);

                    #endregion


                    // Get the 3DIcon texture from the Icons folder in the AssetDatabase
                    Texture2D icon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Icons/3DIcon.png", typeof(Texture2D));


                    // Calculate the position for the icon
                    Rect iconRect = new Rect(rect.x - 30, rect.y - 2, 20, 20);

                    // Draw the icon in front of the object's name
                    GUI.Label(iconRect, icon);


                }
                else if (gameObject.TryGetComponent<Object2D>(out Object2D j))
                {
                    #region background
                    //// Calculate the position for the background rect
                    //Rect backgroundRect = new Rect(rect.x, rect.y, rect.width, rect.height);

                    //// Draw the background rect with the background color
                    //EditorGUI.DrawRect(backgroundRect, backgroundColor3D);
                    //// Draw the object's name in white to make it visible
                    //var style = new GUIStyle();
                    //style.fontStyle = FontStyle.Bold;
                    //style.normal.textColor = Color.white;
                    //GUI.color = Color.white;
                    //GUI.Label(backgroundRect, gameObject.name, style);

                    #endregion


                    // Get the 2DIcon texture from the Icons folder in the AssetDatabase
                    Texture2D icon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Icons/2DIcon.png", typeof(Texture2D));

                    // Calculate the position for the icon
                    Rect iconRect = new Rect(rect.x - 30, rect.y - 2, 20, 20);

                    // Draw the icon in front of the object's name
                    GUI.Label(iconRect, icon);


                }

            }
        };
    }
}



public class ColorChangerWindow : EditorWindow
{
    // The new color that you want to use to update the background color
    public Color newColor = Color.white;

    [MenuItem("Window/Color Changer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ColorChangerWindow));
    }


    void OnGUI()
    {
        // Display a color picker to allow the user to select the new color
        newColor = EditorGUILayout.ColorField("New Color", newColor);

        // Add a button to apply the new color
        if (GUILayout.Button("Apply"))
        {
            // Update the background color with the new color
            HierarchyColors.backgroundColor3D = newColor;
        }
    }
}