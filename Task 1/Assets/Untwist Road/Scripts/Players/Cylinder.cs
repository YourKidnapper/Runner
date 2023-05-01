using UnityEngine;

public class Cylinder : MonoBehaviour
{
    [Header("Cylinder/Animation")]
    public Transform anim;

    [Header("Rotation direction")]
    public Vector3 rotationAngle;

    [Header("Rotation speed")]
    [Range(10,60)]
    public float rotationSpeed;

    [HideInInspector]public bool Ground = true;


    void FixedUpdate()
    {
        if (PlayerPrefs.GetInt("isStarted") == 1)
        {
            //anim.transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime); //Cylinder rotating animation
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "GroundOn") //The last cylinder passes the values ​​to the first - the first master
        {
            Ground = true; //Set bool ground true, if cylinder collision with house
        }
    }
    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "GroundOff")
        {
            Ground = false; //Set bool ground off, if cylinder collision with Ground Off trigger
        }
        if (target.tag == "Dead")
        {
            GameObject.FindGameObjectWithTag("MainCylinder").GetComponent<CylinderCont>().DestroyLastCylinder(); //Destroy last cylinder, when it collision with dead trigger
        }
    }
}