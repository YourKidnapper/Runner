using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(CameraFollow))]
public class CustomCameraFollow :  Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CameraFollow cam = (CameraFollow)target;

        if (GUILayout.Button("Reset to standard")) //If the button was pressed, restore the default values
        {
            cam.offset = new Vector3(0,4,-8);
            cam.cameraSmooth = 0.2f;
        }
        if (GUI.changed) //Saving changes
        {
            EditorUtility.SetDirty(cam);
            EditorSceneManager.MarkSceneDirty(cam.gameObject.scene);
        }
    }
}
