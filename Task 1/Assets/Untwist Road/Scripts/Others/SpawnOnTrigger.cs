using UnityEngine;

public class SpawnOnTrigger : MonoBehaviour
{
    float distance;
    Vector3 posStart;

    [Header("The platform on which to place the spawner.")]
    public Transform thisHouse;
    [Header("The platform to which the road will be built.")]
    public Transform nextHouse;

    [Header("If this is a spawn road at the finish line. Finish = true.")]
    public bool Finish = false;

    CylinderCont cont;
    bool check = false;

    void Awake()
    {
        posStart.z = transform.position.z; //Get spawn block start position

        cont = GameObject.FindGameObjectWithTag("MainCylinder").GetComponent<CylinderCont>(); //Get cylinder controller script

        if (Finish != true) //If its not finish
        {
            float pos_1;
            float pos_2;

            posStart.y = thisHouse.transform.position.y + thisHouse.transform.localScale.y / 2 - 0.05f; //Calculate start position Y

            pos_1 = thisHouse.localPosition.z + thisHouse.localScale.z / 2; //Start position point
            pos_2 = nextHouse.localPosition.z - nextHouse.localScale.z / 2; //Finish position point

            distance = pos_2 - pos_1; //Calculate distance
        }
        else
        {
            distance = 250; //On the finish maximum distance
            posStart.y = gameObject.transform.parent.position.y + 0.5f - 0.05f; //Calculate start position Y
        }
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Cylinder" || target.tag == "MainCylinder")
        {
            if (check == false) //When touching the player, pass him the coordinates of the start of the spawn, the distance, turn on the spawn, turn off the Ground, if this is a spawn at the finish, then Finish = true;
            {
                cont.posStart = posStart; //Set start position to cylinder contoller
                cont.distance = distance; //Set distance
                cont.SpawnNewRoadBlock();
                cont.Ground = false; //Set bool ground
                cont.Finish = Finish; //Set bool finish

                check = true;
            }
        }
    }
}
