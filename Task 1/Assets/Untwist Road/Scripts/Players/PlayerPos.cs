using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    CylinderCont cylinder;

    void Start()
    {
        cylinder = GameObject.FindGameObjectWithTag("MainCylinder").GetComponent<CylinderCont>();
    }

    void LateUpdate()
    {
        transform.position = new Vector3(cylinder.transform.position.x, transform.position.y, cylinder.transform.position.z); //Player movement behind the cylinder from above
    }

    void OnCollisionEnter(Collision collision) 
    {
        if (collision.collider.tag == "GroundOn")
        {
            if (transform.position.z >= cylinder.posStart.z + cylinder.distance) //When game over and the player jumped from the end of the road to the house
            {
                cylinder.animator.Play("dead"); //Play dead animation
                cylinder.playerrb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX; //Freeze rotaiton and position x
            }
        }
    }
}