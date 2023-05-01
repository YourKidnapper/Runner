using UnityEngine;

public class Cones : MonoBehaviour
{
    float Scale; //Total scale in this moment
    [Header("How much player lose scale after collision")]
    [Range(0.05f, 10f)]
    public float minusScale; //How much will be taken away after touching

    [Header("Vibration power after collision")]
    [Range(0,80)]
    public int vibrationPower = 40;

    bool check = false;

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "AllOff") //The same as in Adders Script, when you touch the OffAll object, we delete the coin for optimization
        {
            Destroy(gameObject); //Destroy gameobject
        }

        if (target.tag == "Cylinder" || target.tag == "MainCylinder")
        {
            if (check == false) //This is necessary so that the script does not work several times.
            {
                Scale = PlayerPrefs.GetFloat("Scale");  //Get cylinder scale
                if (Scale > 0) //If the size is greater than zero, we subtract the minusScale from it.
                {
                    Scale = Scale - minusScale;  //Reduce scale
                    if (Scale > 0) //If everything is still less than zero, not less than zero, then we store the value
                    {
                        PlayerPrefs.SetFloat("Scale", Scale); //Save scare
                    }
                    else
                    {
                        PlayerPrefs.SetFloat("Scale", 0); //If less than zero, then store the value = 0
                    }
                }

                    if (PlayerPrefs.GetInt("VibrationOn") == 1) //If vibration enebled
                    {
                        Vibration.Vibrate(vibrationPower); //Vibrate
                    }
                    check = true;                
            }
        }
    }
}
