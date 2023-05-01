using UnityEngine;

public class Finish : MonoBehaviour
{
    [Header("X score: 1,2,3..")]
    [Range(1, 100)]
    public int x;

    [Header("The number of coins that the player will receive")]
    [Range(10,1000)]
    public int addCoins;

    [Header("Vibration Power")]
    [Range(0, 200)]
    public int vibrationPower = 100;

    int Coins; //Total coins

    CylinderCont cylinder;
    bool check = false;

    void Start()
    {
        cylinder = GameObject.FindGameObjectWithTag("MainCylinder").GetComponent<CylinderCont>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player") //When the player, namely the Player, touches the finish, add more to the total number of coins, set the finish points equal to X, victory = 1. Turn on the animation of victory.
        {
            if (check == false)
            {
                Coins = PlayerPrefs.GetInt("Coins"); //Get coins number
                Coins = Coins + addCoins; //Add coins
                PlayerPrefs.SetInt("Coins", Coins); //Save

                PlayerPrefs.SetInt("xScore", x); //Set xScore number
                PlayerPrefs.SetInt("isWin", 1); //Open win panel

                cylinder.animator.SetBool("win", true); //Play win animation

                if (PlayerPrefs.GetInt("VibrationOn") == 1) //If vibration is not turned off, the phone vibrates.
                {
                    Vibration.Vibrate(vibrationPower);
                }
                check = true;
            }
        }
    }
}