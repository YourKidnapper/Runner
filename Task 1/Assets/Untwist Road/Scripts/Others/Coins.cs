using UnityEngine;

public class Coins : MonoBehaviour
{
    [Header("Vibration power when player get coin")]
    [Range(0, 80)]
    public int vibrationPower = 60;

    [Header("How much player get coins")]
    [Range(1,15)]
    public int addNumber = 3;

    int coins; //Total number of coins
    bool check = false;

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "AllOff") //The same as in Adders Script, when you touch the OffAll object, we delete the coin for optimization
        {
            Destroy(gameObject); //Destroy coin gameobject
        }

        if (target.tag == "Cylinder" || target.tag == "MainCylinder") //Add coins, you can put any value, 5, 10, by default I have 3.
        {
            if (check == false) //If its first collision
            {
                coins = PlayerPrefs.GetInt("Coins"); //Get total coins number
                coins = coins + addNumber; //Add coins
                PlayerPrefs.SetInt("Coins", coins); //Save

                if (PlayerPrefs.GetInt("VibrationOn") == 1) //If vibration is not turned off, the phone vibrates.
                {
                    Vibration.Vibrate(vibrationPower);  //Vibrate
                }

                check = true; 
                Destroy(this.gameObject); //Destroy coin
            }
        }
    }
}
