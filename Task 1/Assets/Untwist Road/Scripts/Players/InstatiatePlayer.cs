using UnityEngine;

public class InstatiatePlayer : MonoBehaviour
{
    [Header("Player prefab")]
    public GameObject prefab;
    Vector3 startPosition;

    void Awake()
    {
        GameObject player = Instantiate(prefab); //Instantiate player
        player.transform.parent = gameObject.transform; //Add parent to player

        Transform cylinder = GameObject.FindGameObjectWithTag("MainCylinder").transform; //Get cylinder transform
        startPosition = new Vector3(1,cylinder.transform.localPosition.y + cylinder.transform.localScale.y / 2 + prefab.transform.localScale.y / 2,cylinder.transform.localPosition.z); //Calculate player start position depend cylinder position
        player.transform.localPosition = startPosition; //Set position for player
    }
}
