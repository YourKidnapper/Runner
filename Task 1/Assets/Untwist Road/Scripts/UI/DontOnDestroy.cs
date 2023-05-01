using UnityEngine.SceneManagement;
using UnityEngine;

public class DontOnDestroy : MonoBehaviour
{
    int levelId; 

    void Start()
    {
        levelId = PlayerPrefs.GetInt("LevelId"); //Get level id

        DontDestroyOnLoad(gameObject); //DontDestroyOnLoad Canvas
        SceneManager.LoadScene(levelId); //Load level scene
    }

}
