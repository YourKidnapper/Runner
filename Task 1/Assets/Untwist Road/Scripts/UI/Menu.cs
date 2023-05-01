using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
    [Header("Test Mode")]
    public bool TestingMode = false;
    [Range(1, 30)] //Set your maximum level in the game instead of 30
    public int TestingLevelId;

    [Header("Settings Files path:Sprites/Buttons")]
    public GameObject SettingsPanel;

    public Image sound;
    public Image vibration;

    public Sprite soundOn;
    public Sprite soundOff;
    public Sprite vibrationOn;
    public Sprite vibrationOff;

    [Header("Setting button animator")]
    public Animator anim;

    [Header("StartMenu")]
    public GameObject StartMenu;
    public Text CoinsNumber_One;

    [Header("Canvas/StartMenu/Menu/Levels")]
    public Image[] level;

    [Header("Path:Sprites/Levels")]
    public Sprite levelOpen;
    public Sprite levelClose;

    [Header("Canvas/StartMenu/Menu/Levels/level..")]
    public Text level1;
    public Text level2;
    public Text level3;
    public Text level4;

    [Header("InGame")]
    public GameObject InGame;
    public Text CoinsNumber_Two;
    public Text Score_One;

    public Text levelNumber_One;
    public Slider levelLine_One;

    int GamesPlayed;

    public Animator score_;

    [Header("GameOver")]
    public Text Score_Two;
    public GameObject GameOver;

    public Text levelNumber_Two;
    public Slider levelLine_Two;

    [Header("Win")]
    public GameObject Win;

    public Text xScore;
    public Text Score_Three;
    public Text CoinsNumber_Three;

    bool Pause;
    [Header("Pause")]
    public GameObject panelPause;

    bool ComplimentComplete = false;
    bool TimerOn;
    [Header("Сompliment Texts path:/InGame/Texts")]
    public Text[] compliments;
    int num, numMax = 6;

    float time;

    [Header("The time that the compliment is shown")]
    [Range(0.5f,8)]
    public float timeMax;

    int score;
    int levelId;

    [Header("Vibration length when assembling Adders. Default = 40")]
    [Range(10, 80)]
    public int vibrationPower = 40;

    [Header("Ads")]
    AdsManager adScript;

    Dictionary<int, string[]> level_map = new Dictionary<int, string[]>();
    string[] levels;

    [Header("Turn on the sounds you want and add them to the audio source")]
    public bool winSound;
    public bool loseSound;
    public bool updateScoreSound;

    AudioSource audioWin, audioLose, audioScore;

    int isDeaded, isWinned;

    void Awake()
    {
        StartUp(); //Loading data
        Levels(); //Loading level icons
    }

    void Update()
    {
        if (score != (int) PlayerPrefs.GetFloat("Score")) //If the score has changed, change it on the screen
        {
            score = (int)PlayerPrefs.GetFloat("Score"); //Convert the count from float to int and display it on the screen.
            score_.Play("Score"); //Play animation

            if (PlayerPrefs.GetInt("VibrationOn") == 1) //If vibration enabled
            {
                Vibration.Vibrate(vibrationPower); //Vibration
            }
            if (updateScoreSound) //If you have turned on
            {
                audioScore.Play(); //Play sound
            }
        }
        if (isDeaded != PlayerPrefs.GetInt("isDeaded")) //When the player has lost, turn on the losing scene.
        {
           InGame.SetActive(false); //Disable gameplay panel
           GameOver.SetActive(true); //Enable this scene

            if (loseSound) //If you have turned on
            {
                audioLose.Play(); //Play sound
            }
        }
        isDeaded = PlayerPrefs.GetInt("isDeaded");//Get isDeaded value

        if (isWinned != PlayerPrefs.GetInt("isWin"))//When the player has won, turn on the victory scene.
        {
            InGame.SetActive(false); //Disable gameplay panel
            Win.SetActive(true); //Enable win panel

            adScript.HideBanner(); //Close banner ad

            GameObject.Find("Particle").transform.GetChild(0).GetComponent<ParticleSystem>().Play(); //Play particle

            levelId = PlayerPrefs.GetInt("LevelId");
            if (!TestingMode) //If test mode disabled
            {
                levelId = levelId + 1; //Increase level id
                PlayerPrefs.SetInt("LevelId", levelId); //Set new level id
            }

            if (winSound) //If you have turned on
            {
                audioWin.Play(); //Play sound
            }
        }
        isWinned = PlayerPrefs.GetInt("isWin");//Get isWinned value

        if (StartMenu.active)//If this is the start screen, set the level icons and the number of coins.
        {
            CoinsNumber_One.text = "" + PlayerPrefs.GetInt("Coins"); //Set coins number
          
            Levels(); //Set level icons
        }
        if (InGame.active)//If the game is on
        {
            CoinsNumber_Two.text = "" + PlayerPrefs.GetInt("Coins"); //Set coins number
            Score_One.text = "" + score; //Update the score.

            LevelNow(); //Updating level progress

            if (PlayerPrefs.GetInt("GamesPlayed") != 1) //If this is not the first game
            {
                if (PlayerPrefs.GetInt("CylinderNum") == 7) //If cylinder number == 7
                {
                    if (ComplimentComplete == false) //If compliment complete == false
                    {
                        DoCompliment(); //If the player scored seven cylinders, include a compliment
                    }
                }
                else
                {
                    if (PlayerPrefs.GetInt("CylinderNum") <= 4)//If there are fewer cylinders or = 4, the complement can be turned on again if there are 7 cylinders
                    {
                        ComplimentComplete = false; //Player can get new compliment
                    }
                }
                if (TimerOn == true) //Compliment Off Timer
                {
                    time = time + Time.deltaTime; //Timer time
                    if (time > timeMax) //If time > timeMax
                    {
                        compliments[num].enabled = false; //Close complitemt
                        TimerOn = false; //Timer off
                        time = 0; //Time to default
                    }
                }
            }
        }
        if (GameOver.active) // If the game fails and a losing scene is open
        {
            Score_Two.text = "" + score; //Set total score number

            LevelLast(); //Set level icons
        }
        if (Win.active) //When you win 
        {
            CoinsNumber_Three.text = "" + PlayerPrefs.GetInt("Coins"); //Set coins number
            Score_Three.text = "" + score; //Set score number
            xScore.text = "x" + PlayerPrefs.GetInt("xScore"); //Set xScore

        }
    }

    void OnApplicationPause(bool pauseStatus) //Pause while playing
    {
        if (InGame.active == true) //If pause in started game
        {
            Pause = true; //Enable pause
            panelPause.SetActive(true); //Enable pause panel

            PlayerPrefs.SetInt("isStarted", 0); //Stop game
        }
    }

    void DoCompliment() //Turn off all compliment texts, choose one randomly and turn on the text and timer
    {
        numMax = compliments.Length; //Get maximum number compliments
        for (int i = 0; i != numMax; i++) //Disable all compliments
        {
            compliments[i].enabled = false;
        }

        num = Random.Range(0, numMax); //Get random number of compliment
        compliments[num].enabled = true; //Enable this compliment

        ComplimentComplete = true; //Set true
        TimerOn = true; //Enable timer
        time = 0; //Set time to default
    }
    void Levels() //Set the level icons in the menu, depending on the level
    {
        foreach (var item in level_map)
        {
            levels = item.Value; //Get value of levels array
            if (PlayerPrefs.GetInt("LevelId") > item.Key)  //Get array == level id
            {

                level1.text = levels[0]; //Set number of level id
                level2.text = levels[1]; //Set number of level id
                level3.text = levels[2]; //Set number of level id
                level4.text = levels[3]; //Set number of level id

                for (int i = 0; i != 4; i++) //Set icon for level icons
                {
                    int id = 0;
                    int.TryParse(levels[i], out id); //Covert string to int

                    if (PlayerPrefs.GetInt("LevelId") == id)
                    {
                        level[i].sprite = levelOpen; //This level icon = opened level icon
                    }
                    else
                    {
                        level[i].sprite = levelClose; //Last and future levels = closed icon
                    }
                }
            }            
        }
}

    void LevelNow() //Set the value of the current level
    {
        levelNumber_One.text = "" + PlayerPrefs.GetInt("LevelId"); //Set level number
        levelLine_One.value = PlayerPrefs.GetFloat("LevelLine"); //Set level line value
    }

    void LevelLast() //Setting the value of the previous level
    {
        levelNumber_Two.text = "" + PlayerPrefs.GetInt("LevelId"); //Set level number
        levelLine_Two.value = PlayerPrefs.GetFloat("LevelLine"); //Set level line value
    }

    public void OnClick_Settings()
    {
        if (SettingsPanel.active == false) //Turn settings on or off
        {
            anim.Play("SettingsOpen"); //Play animation for setting button

            SettingsPanel.SetActive(true); //Open settings panel
        }
        else
        {
            anim.Play("SettingsClose"); //Play animation for setting button

            SettingsPanel.SetActive(false); //Close settings panel     
        }
    }
    public void OnClick_Sound() //Turn the sound on or off
    {
        if (PlayerPrefs.GetInt("SoundOn") == 1) //If sounds on
        {
            AudioListener.volume = 0; //Set total volume value = 0
            PlayerPrefs.SetInt("SoundOn", 0); //Set sounds off

            sound.sprite = soundOff; //Set sound button off
        }
        else
        {
            AudioListener.volume = 1; //Set total volume value = 1
            PlayerPrefs.SetInt("SoundOn", 1); //Set sounds on

            sound.sprite = soundOn; //Set sound button on
        }
    }
    public void OnClick_Vibration() //Turn vibration on or off
    {
        if (PlayerPrefs.GetInt("VibrationOn") == 1) //If vibration on
        {
            PlayerPrefs.SetInt("VibrationOn", 0); //Set vibration off
            vibration.sprite = vibrationOff; //Set vibration button off
        }
        else
        {
            Vibration.Vibrate(100); //Vibrate
            PlayerPrefs.SetInt("VibrationOn", 1); //Set vibration on
            vibration.sprite = vibrationOn; //Set vibration button on   
        }
    }

    public void OnClick_Play() //When you press the play button, set everything to default and start the game
    {
        StartMenu.SetActive(false); //Disable start menu panel
        InGame.SetActive(true); //Enable game play panel

        PlayerPrefs.SetFloat("LevelLine", 0); //Set to null
        PlayerPrefs.SetFloat("Score", 0); // Set to null
        PlayerPrefs.SetInt("xScore", 0); //Set to null

        GamesPlayed = PlayerPrefs.GetInt("GamesPlayed"); //Get games played numbers
        GamesPlayed = GamesPlayed + 1; //Increase games played number
        PlayerPrefs.SetInt("GamesPlayed", GamesPlayed); //Set games played number

        PlayerPrefs.SetInt("isStarted", 1); //Set game started
    }

    public void OnClick_Gameover() //When you press the game over button, set everything to default and start the game
    {
        GameOver.SetActive(false); //Disable game over panel
        StartMenu.SetActive(true); //Enable start menu panel

        isDeaded = 0; //Set to null
        PlayerPrefs.SetInt("isDeaded", 0); //Set to null
        PlayerPrefs.SetFloat("LevelLine", 0); //Set to null
        PlayerPrefs.SetFloat("Score", 0); // Set to null
        PlayerPrefs.SetInt("xScore", 0); //Set to null

        if (levelId > adScript.levelId && adScript.everyLose) //if ads manager can show ad in this level, and ads manager have turned on show ad every lose
        {
            adScript.ShowInterstitialAd(); //Show ad
        }

        SceneManager.LoadScene(PlayerPrefs.GetInt("LevelId")); //Load level id scene
    }
    public void OnClick_Win() //When you press the win button on finish, set everything to default and start the game
    {
        Win.SetActive(false);
        StartMenu.SetActive(true);

        isWinned = 0; //Set to null
        PlayerPrefs.SetInt("isWin", 0); //Set to null
        PlayerPrefs.SetFloat("LevelLine", 0); //Set to null
        PlayerPrefs.SetFloat("Score", 0); // Set to null
        PlayerPrefs.SetInt("xScore", 0); //Set to null

            if (levelId > adScript.levelId && adScript.everyVictory) //If level id > level id after ads manager can show
            {
                adScript.ShowInterstitialAd(); //Show ad
            }
        SceneManager.LoadScene(PlayerPrefs.GetInt("LevelId")); //Load level scene
    }

    public void OnClick_ContinueGame() //When you press the continue button in pause, set everything to default and start the game
    {
        Pause = false; //Disable pause bool
        panelPause.SetActive(false); //Disable pause panel
         
        PlayerPrefs.SetInt("isStarted", 1); //Set game continue started
    }

    void StartUp() //Default saves
    {
        if (PlayerPrefs.GetInt("DataSetted") == 1) //If game starting not first time
        {
            PlayerPrefs.SetInt("isDeaded", 0); //Set to null
            PlayerPrefs.SetInt("isWin", 0); //Set to null
            PlayerPrefs.SetInt("isStarted", 0); //Set to null

            PlayerPrefs.SetFloat("LevelLine", 0); //Set to null
            PlayerPrefs.SetFloat("Score", 0); // Set to null
            PlayerPrefs.SetInt("xScore", 0); //Set to null

            if (TestingMode)
            {
                PlayerPrefs.SetInt("LevelId", TestingLevelId); //Set to 1
                PlayerPrefs.SetInt("GamesPlayed", 2);
            }

        }
        else //If game starting first time
        {
            PlayerPrefs.SetInt("DataSetted", 1); //Set to 1

            PlayerPrefs.SetInt("SoundOn", 1); //Set to 1
            PlayerPrefs.SetInt("VibrationOn", 1); //Set to 1

            PlayerPrefs.SetInt("NoAds", 0); //Set to null

            if (TestingMode)
            {
                PlayerPrefs.SetInt("LevelId", TestingLevelId); //Set to 1
                PlayerPrefs.SetInt("GamesPlayed", 2); 
            }
            else
            {
                PlayerPrefs.SetInt("LevelId", 1); //Set to 1
                PlayerPrefs.SetInt("GamesPlayed", 0); //Set to null
            }

            PlayerPrefs.SetInt("Coins", 0); // Set to null

            PlayerPrefs.SetInt("isStarted", 0); //Set to null
            PlayerPrefs.SetInt("isDeaded", 0); //Set to null
            PlayerPrefs.SetInt("isWin", 0); //Set to null

            PlayerPrefs.SetFloat("LevelLine", 0); //Set to null
            PlayerPrefs.SetFloat("Score", 0); // Set to null
            PlayerPrefs.SetInt("xScore", 0); //Set to null
            
        }

        level_map.Add(0, new string[] { "1", "2", "3", "4" }); //Dictionary of levels and numbers in level texts
        level_map.Add(5, new string[] { "6", "7", "8", "9" });
        level_map.Add(10, new string[] { "11", "12", "13", "14" });
        level_map.Add(15, new string[] { "16", "17", "18", "19" });
        level_map.Add(20, new string[] { "21", "22", "23", "24" });
        level_map.Add(25, new string[] { "26", "27", "28", "29" }); //If you have more than 30 levels, add the code using it as a template

        levelId = PlayerPrefs.GetInt("LevelId"); //Get level id
        adScript = gameObject.GetComponent<AdsManager>(); //Get ads manager script

        if (winSound) //If you turn on this
        {
            audioWin = GameObject.Find("AudioSource_Win").GetComponent<AudioSource>(); //Get audio source
        }
        if (loseSound)//If you turn on this
        {
            audioLose = GameObject.Find("AudioSource_Lose").GetComponent<AudioSource>();//Get audio source
        }
        if (updateScoreSound)//If you turn on this
        {
            audioScore = GameObject.Find("AudioSource_Score").GetComponent<AudioSource>();//Get audio source
        }
    }
  
}
