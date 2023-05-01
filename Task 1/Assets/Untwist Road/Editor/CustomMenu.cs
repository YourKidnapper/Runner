using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Menu))]
public class CustomMenu : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Menu menu = (Menu)target;

        if (GUILayout.Button("Reset to standard")) //If the button was pressed, restore the default values
        {
            menu.TestingLevelId = 1;
            menu.TestingMode = false;

            menu.timeMax = 2;

            menu.vibrationPower = 40;
        }
        if (GUI.changed) //Saving changes
        {
            EditorUtility.SetDirty(menu);
            EditorSceneManager.MarkSceneDirty(menu.gameObject.scene);
        }
    }
}
