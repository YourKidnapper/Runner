using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    [Header("Camera offset by Y and Z")]
    public Vector3 offset;
    [Header("Camera offset on finish")]
    public Vector3 finishOffset = new Vector3(0, 3, -6);
    [Header("Camera smooth")]
    [Range(0.1f,0.8f)]
    public float cameraSmooth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainCylinder").transform; //Get player transform
    }

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(transform.position.x, player.position.y + offset.y, player.position.z + offset.z); //Calculate new position
        transform.position = Vector3.Lerp(transform.position, newPos, cameraSmooth); //The camera follows the player, y and x is the offset
    } 

    public void Finished()
    {
        offset = new Vector3(offset.x, finishOffset.y, finishOffset.z); //Set offset when cylinder at finish
    }
}
