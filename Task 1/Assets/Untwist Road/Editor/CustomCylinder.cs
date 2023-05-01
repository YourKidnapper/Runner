using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Cylinder))]
public class CustomCylinder : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Cylinder cylinder = (Cylinder)target;

        if (GUILayout.Button("Reset to standard")) //If the button was pressed, restore the default values
        {
            cylinder.rotationAngle = new Vector3(0, -10, 0);
            cylinder.rotationSpeed = 25;
        }
        if (GUI.changed) //Saving changes
        {
            EditorUtility.SetDirty(cylinder);
            EditorSceneManager.MarkSceneDirty(cylinder.gameObject.scene);
        }
    }
}
