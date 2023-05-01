using UnityEngine;

public class LevelDistance : MonoBehaviour
{
    Transform finish, cylinder;
    float levelDistance, oneProcent, Procent;

    void Start()
    { 
        finish = GameObject.Find("Finish").transform; //Get finish transform
        cylinder = GameObject.FindGameObjectWithTag("MainCylinder").transform; //Get cylinder tranform

        levelDistance = finish.localPosition.z; //Get distance to finish

        oneProcent = levelDistance / 100; //Determine the distance to the finish line as a percentage
    }

    
    void Update()
    {
        Procent = cylinder.transform.position.z / oneProcent; //Calculate procents
        PlayerPrefs.SetFloat("LevelLine", Procent); //Updating level line procent
    }
}
