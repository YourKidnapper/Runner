using UnityEngine;

public class AllOnTrigger : MonoBehaviour
{
    [Header("Insert these objects into the script in the prefab")]
    public GameObject Adds;
    [Header("AllOn Mesh Renderer")]
    public MeshRenderer mesh;

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "AllOn") //When the player touches this object, turn on all Adders, this optimizes the game, since Adders are not turned on all at once, but in turn, while one turns on, the other is already removed from the scene.
        {
            mesh.enabled = false; //Turn off fake adder mesh renderer
            Adds.SetActive(true); //Turn on really adders
        }
    }
}