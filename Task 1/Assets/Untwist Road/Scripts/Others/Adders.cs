using UnityEngine;

public class Adders : MonoBehaviour
{
    float Scale, Score; //Player's last cylinder size and total score

    [Header("How much the last cylinder increases when one Add'r is touched")]
    [Range(0.005f, 2f)]
    public float addScale; //How much the last cylinder increases when one Add'r is touched

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "AllOff") //When you touch the OffAll object, turn off Adder for optimization
        {
            Destroy(gameObject); //Destroy this gameobject for game optimization
        }

        if (target.tag == "Cylinder" || target.tag == "MainCylinder") //When the player touches Adder, we increase the cylinder, add points and turn on vibration if the player has not turned it off
        {
            Scale = PlayerPrefs.GetFloat("Scale"); //Get cylinder scale
            Scale = Scale + addScale; //Inscrease cylinder scale
            PlayerPrefs.SetFloat("Scale", Scale); //Save

            Score = PlayerPrefs.GetFloat("Score"); //Get score number
            Score = Score + 0.10714f; //Inscrease score
            PlayerPrefs.SetFloat("Score", Score); //Save data

            Destroy(this.gameObject); //Removing an object from the scene
        }
    }
}
