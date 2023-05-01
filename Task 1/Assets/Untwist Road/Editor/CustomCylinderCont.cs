using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(CylinderCont)), CanEditMultipleObjects]
public class CustomCylinderCont : Editor
{
    Transform parent;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CylinderCont cylinderCont = (CylinderCont)target;

        if (GUILayout.Button("Add cylinder"))
        {
            if (cylinderCont.cylindersNum < 7) //Maximum cylinder number is 8
            {
                cylinderCont.transform.parent.position = new Vector3(cylinderCont.transform.parent.position.x, cylinderCont.transform.parent.position.y + cylinderCont.maxScale, cylinderCont.transform.parent.position.z);

                Cylinder newCylinder = Instantiate(cylinderCont.prefab);//Spawning new cylinder
                Vector3 position = new Vector3(cylinderCont.transform.position.x, cylinderCont.cylinders[cylinderCont.cylinders.Count - 1].transform.position.y - cylinderCont.cylinders[cylinderCont.cylinders.Count - 1].transform.localScale.z, cylinderCont.transform.position.z);
                newCylinder.transform.localPosition = position;//Calculating and setting new position depending last cylider

                newCylinder.transform.localScale = new Vector3(cylinderCont.maxScale, cylinderCont.maxScale, cylinderCont.maxScale); //Setting new cylinder scale
                newCylinder.transform.SetParent(cylinderCont.transform.parent); //Adding parent

                cylinderCont.cylinders.Add(newCylinder);//Adding cylinder to array
                cylinderCont.cylindersNum = cylinderCont.cylindersNum + 1; //Increasing the number of cylinders
            }

        }
        if (GUILayout.Button("Remove cylinder"))
        {
            if (cylinderCont.cylindersNum > 1) //Minimum cylinders number is one
            {
                DestroyImmediate(cylinderCont.cylinders[cylinderCont.cylinders.Count - 1].gameObject, false); //Destoying last cylinder
                cylinderCont.cylinders.RemoveAt(cylinderCont.cylindersNum); //Remove cylinder from list
                cylinderCont.cylindersNum = cylinderCont.cylindersNum - 1; //Reducing the number of cylinders

                cylinderCont.transform.parent.position = new Vector3(cylinderCont.transform.parent.position.x, cylinderCont.transform.parent.position.y - cylinderCont.maxScale, cylinderCont.transform.parent.position.z); //Put all the cylinders down
            }
            }

        if (GUILayout.Button("Reset to standard")) //If the button was pressed, restore the default values
        {
            cylinderCont.speed = 5.8f;
            cylinderCont.defaultSpeed = 0.025f;

            if (cylinderCont.cylindersNum < 4)
            {
                for (int i = 0; cylinderCont.cylindersNum != 4; i++) //Restore the standard number of cylinders
                {
                    cylinderCont.transform.parent.position = new Vector3(cylinderCont.transform.parent.position.x, cylinderCont.transform.parent.position.y + cylinderCont.maxScale, cylinderCont.transform.parent.position.z); //Raise all cylinders to spawn a new one from below

                    Cylinder newCylinder = Instantiate(cylinderCont.prefab); //Spawning new cylinder
                    Vector3 position = new Vector3(cylinderCont.transform.position.x, cylinderCont.cylinders[cylinderCont.cylinders.Count - 1].transform.position.y - cylinderCont.cylinders[cylinderCont.cylinders.Count - 1].transform.localScale.z, cylinderCont.transform.position.z);
                    newCylinder.transform.localPosition = position; //Calculating and setting new position depending last cylider

                    newCylinder.transform.localScale = new Vector3(cylinderCont.maxScale, cylinderCont.maxScale, cylinderCont.maxScale); //Setting new cylinder scale
                    newCylinder.transform.SetParent(cylinderCont.transform.parent); //Adding parent

                    cylinderCont.cylinders.Add(newCylinder); //Adding cylinder to array
                    cylinderCont.cylindersNum = cylinderCont.cylindersNum + 1; //Increasing the number of cylinders
                }
            }
            else
            {
                for (int i = 0; cylinderCont.cylindersNum != 4; i++) //Restore the standard number of cylinders
                {
                    DestroyImmediate(cylinderCont.cylinders[cylinderCont.cylinders.Count - 1].gameObject, false); //Destoying last cylinder
                    cylinderCont.cylinders.RemoveAt(cylinderCont.cylindersNum); //Remove cylinder from list
                    cylinderCont.cylindersNum = cylinderCont.cylindersNum - 1; //Reducing the number of cylinders

                    cylinderCont.transform.parent.position = new Vector3(cylinderCont.transform.parent.position.x, cylinderCont.transform.parent.position.y - cylinderCont.maxScale, cylinderCont.transform.parent.position.z); //Put all the cylinders down
                }
            }
            

        }
        if (GUI.changed) //Saving changes
        {
            EditorUtility.SetDirty(cylinderCont);
            EditorSceneManager.MarkSceneDirty(cylinderCont.gameObject.scene);
        }
    }
}
