 using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class LevelEditor : EditorWindow
{
    public GameObject light, players, level, finish, eventSystem, particle;
    public GameObject platform, bridge, bigAdder, smallAdder, coin, cones, cubes, window, spawnOn, deadTrigger, jumpTrigger;

    Scene scene;

    int windowId = 0;
    bool finishSpawned, editorClosed;

    CylinderCont cylinderCont;
    Texture2D image_1, image_2, image_3, image_4, image_5;

    void OnGUI()
    {
        switch(windowId) //Turn on the window we need
        {
            case 0:
                GUILayout.Label("First stage | Creating a scene with basic objects", EditorStyles.boldLabel); //Title Text

                image_1 = Resources.Load("image_1", typeof(Texture2D)) as Texture2D; //Add image
                GUILayout.Label(image_1, GUILayout.Width(300), GUILayout.Height(300));

                editorClosed = false;  //Editor opened, save changes

                if (GUI.Button(new Rect(60, 310 + EditorGUIUtility.singleLineHeight, 180, 60),"Create new level"))
                {
                    NewScene(); //Creating new scene
                    windowId = 1; //Setting next window id
                }
                if (GUI.Button(new Rect(10, 430 + EditorGUIUtility.singleLineHeight, 100, 40), "Next"))
                {
                    windowId = 1; //Setting next window id
                }
                break;
            case 1:       
                GUILayout.Label("Second stage | Cylinders customization", EditorStyles.boldLabel); //Title Text
                cylinderCont = GameObject.FindGameObjectWithTag("MainCylinder").GetComponent<CylinderCont>();

                image_2 = Resources.Load("image_2", typeof(Texture2D)) as Texture2D; //Add image
                GUILayout.Label(image_2, GUILayout.Width(300), GUILayout.Height(300));

                if (GUILayout.Button("Add cylinder", GUILayout.Width(250), GUILayout.Height(40)))
                {
                    CylinderCustomization(true);
                }
                if (GUILayout.Button("Remove cylinder", GUILayout.Width(250), GUILayout.Height(40)))
                {
                    CylinderCustomization(false);
                }
                if (GUI.Button(new Rect(10, 430 + EditorGUIUtility.singleLineHeight, 100, 40), "Next"))
                {
                    windowId = 2; //Setting next window id
                }
           break;
           case 2:
                GUILayout.Label("Stage Three | Adding objects", EditorStyles.boldLabel); //Title Text

                GUI.Label(new Rect(40, -12 + EditorGUIUtility.singleLineHeight, 160, 40), "House", EditorStyles.label);
                if (GUI.Button(new Rect(10, 16 + EditorGUIUtility.singleLineHeight, 100, 100), Resources.Load("image_3") as Texture))
                {
                    RemoveDeletedObjects();
                    PlatformsArray platformsArray = GameObject.Find("Level").GetComponentInChildren<PlatformsArray>(); //Array of all spawned houses/bridges

                    GameObject newPlatform = PrefabUtility.InstantiatePrefab(platform) as GameObject; //Spawning new house
                    newPlatform.transform.parent = platformsArray.transform; //Adding parent

                    Vector3 position = new Vector3(0,platformsArray.platforms[platformsArray.platforms.Count - 1].transform.position.y, platformsArray.platforms[platformsArray.platforms.Count - 1].transform.position.z + platformsArray.platforms[platformsArray.platforms.Count - 1].transform.localScale.z / 2 + platform.transform.localScale.z / 2 + 2);
                    newPlatform.transform.position = position; //Calculating and setting new position

                    platformsArray.platforms.Add(newPlatform); //Adding new house to array

                    UnityEditor.Selection.activeObject = newPlatform; //Selecting new object

                    GameObject newWindow = PrefabUtility.InstantiatePrefab(window) as GameObject; //Spawning window for a house and setting window position and parent
                    Vector3 windowPos = new Vector3(0, platformsArray.platforms[platformsArray.platforms.Count - 1].transform.position.y + window.transform.localScale.y / 2, platformsArray.platforms[platformsArray.platforms.Count - 1].transform.position.z - platformsArray.platforms[platformsArray.platforms.Count - 1].transform.localScale.z / 2);
                    newWindow.transform.position = windowPos;
                    newWindow.transform.parent = newPlatform.transform.parent; //Setting window parent

                }

                GUI.Label(new Rect(150, -12 + EditorGUIUtility.singleLineHeight, 160, 40), "Bridge", EditorStyles.label);
                if (GUI.Button(new Rect(120, 16 + EditorGUIUtility.singleLineHeight, 100, 100), Resources.Load("image_4") as Texture))
                {
                    RemoveDeletedObjects();
                    PlatformsArray platformsArray = GameObject.Find("Level").GetComponentInChildren<PlatformsArray>(); //Array of all spawned houses/bridges

                    GameObject newBridge = PrefabUtility.InstantiatePrefab(bridge) as GameObject; //Spawning new bridge
                    newBridge.transform.parent = platformsArray.transform; //Adding parent

                    UnityEditor.Selection.activeObject = newBridge; //Selecting new object

                    Vector3 position = new Vector3((platformsArray.platforms[platformsArray.platforms.Count - 1].transform.localScale.x - bridge.transform.localScale.x) / 2, platformsArray.platforms[platformsArray.platforms.Count - 1].transform.position.y, platformsArray.platforms[platformsArray.platforms.Count - 1].transform.position.z + platformsArray.platforms[platformsArray.platforms.Count - 1].transform.localScale.z / 2 + platform.transform.localScale.z / 2 + 2);
                    newBridge.transform.position = position; //Calculating and setting new position

                    platformsArray.platforms.Add(newBridge); //Adding new house to array
                }

                GUI.Label(new Rect(30, 118 + EditorGUIUtility.singleLineHeight, 160, 40), "Big Adder", EditorStyles.label);
                if (GUI.Button(new Rect(10, 146 + EditorGUIUtility.singleLineHeight, 100, 100), Resources.Load("image_5") as Texture))
                {
                    RemoveDeletedObjects();
                    OthersArray othersArray = GameObject.Find("Level").transform.GetChild(1).GetComponent<OthersArray>();  //Array of all spawned adders/obstacles

                    GameObject newBigAdder = PrefabUtility.InstantiatePrefab(bigAdder) as GameObject; //Spawning new adder
                    newBigAdder.transform.parent = othersArray.transform; //Adding parent

                    UnityEditor.Selection.activeObject = newBigAdder; //Selecting new object

                    if (othersArray.others.Count > 0) //Setting start position or position depending last object
                    {
                        Vector3 position = new Vector3(othersArray.others[othersArray.others.Count - 1].transform.position.x, othersArray.others[othersArray.others.Count - 1].transform.position.y, othersArray.others[othersArray.others.Count - 1].transform.position.z + othersArray.others[othersArray.others.Count - 1].transform.localScale.z / 2 + 2.1f + 2);
                        newBigAdder.transform.position = position; //Calculating and setting new position
                    }
                    else
                    {
                        newBigAdder.transform.position = new Vector3(0,bigAdder.transform.localScale.y / 2,0); 
                    }

                    othersArray.others.Add(newBigAdder); //Adding new adder to array
                }
                GUI.Label(new Rect(140, 118 + EditorGUIUtility.singleLineHeight, 160, 40), "Small Adder", EditorStyles.label);
                if (GUI.Button(new Rect(120, 146 + EditorGUIUtility.singleLineHeight, 100, 100), Resources.Load("image_6") as Texture))
                {
                    RemoveDeletedObjects();
                    OthersArray othersArray = GameObject.Find("Level").transform.GetChild(1).GetComponent<OthersArray>();  //Array of all spawned adders/obstacles

                    GameObject newSmallAdder = PrefabUtility.InstantiatePrefab(smallAdder) as GameObject; //Spawning new adder
                    newSmallAdder.transform.parent = othersArray.transform; //Adding parent

                    UnityEditor.Selection.activeObject = newSmallAdder; //Selecting new object

                    if (othersArray.others.Count > 0) //Setting start position or position depending last object
                    {
                        Vector3 position = new Vector3(othersArray.others[othersArray.others.Count - 1].transform.position.x, othersArray.others[othersArray.others.Count - 1].transform.position.y, othersArray.others[othersArray.others.Count - 1].transform.position.z + othersArray.others[othersArray.others.Count - 1].transform.localScale.z / 2 + smallAdder.transform.localScale.z / 2 + 2);
                        newSmallAdder.transform.position = position; //Calculating and setting new position
                    }
                    else
                    {
                        newSmallAdder.transform.position = new Vector3(0, smallAdder.transform.localScale.y / 2, 0);
                    }

                    othersArray.others.Add(newSmallAdder); //Adding new adder to array
                }
                GUI.Label(new Rect(40, 249 + EditorGUIUtility.singleLineHeight, 160, 40), "Cones", EditorStyles.label);
                if (GUI.Button(new Rect(10, 276 + EditorGUIUtility.singleLineHeight, 100, 100), Resources.Load("image_8") as Texture))
                {
                    RemoveDeletedObjects();
                    OthersArray othersArray = GameObject.Find("Level").transform.GetChild(1).GetComponent<OthersArray>();  //Array of all spawned adders/obstacles

                    GameObject newCones = PrefabUtility.InstantiatePrefab(cones) as GameObject; //Spawning new cones
                    newCones.transform.parent = othersArray.transform; //Adding parent

                    UnityEditor.Selection.activeObject = newCones; //Selecting new object

                    if (othersArray.others.Count > 0) //Setting start position or position depending last object
                    {
                        Vector3 position = new Vector3(othersArray.others[othersArray.others.Count - 1].transform.position.x, othersArray.others[othersArray.others.Count - 1].transform.position.y, othersArray.others[othersArray.others.Count - 1].transform.position.z + othersArray.others[othersArray.others.Count - 1].transform.localScale.z / 2 + cones.transform.localScale.z / 2 + 2);
                        newCones.transform.position = position; //Calculating and setting new position
                    }
                    else
                    {
                        newCones.transform.position = new Vector3(0, cones.transform.localScale.y / 2, 0);
                    }

                    othersArray.others.Add(newCones); //Adding new cones to array
                }
                GUI.Label(new Rect(150, 249 + EditorGUIUtility.singleLineHeight, 160, 40), "Cubes", EditorStyles.label);
                if (GUI.Button(new Rect(120, 276 + EditorGUIUtility.singleLineHeight, 100, 100), Resources.Load("image_9") as Texture))
                {
                    RemoveDeletedObjects();
                    OthersArray othersArray = GameObject.Find("Level").transform.GetChild(1).GetComponent<OthersArray>();  //Array of all spawned adders/obstacles

                    GameObject newCubes = PrefabUtility.InstantiatePrefab(cubes) as GameObject; //Spawning new cubes
                    newCubes.transform.parent = othersArray.transform; //Adding parent

                    UnityEditor.Selection.activeObject = newCubes; //Selecting new object

                    if (othersArray.others.Count > 0) //Setting start position or position depending last object
                    {
                        Vector3 position = new Vector3(othersArray.others[othersArray.others.Count - 1].transform.position.x, othersArray.others[othersArray.others.Count - 1].transform.position.y, othersArray.others[othersArray.others.Count - 1].transform.position.z + othersArray.others[othersArray.others.Count - 1].transform.localScale.z / 2 + cubes.transform.localScale.z / 2 + 2);
                        newCubes.transform.position = position; //Calculating and setting new position
                    }
                    else
                    {
                        newCubes.transform.position = new Vector3(0, cubes.transform.localScale.y / 2, 0);
                    }

                    othersArray.others.Add(newCubes); //Adding new cubes to array
                }
                GUI.Label(new Rect(265, 249 + EditorGUIUtility.singleLineHeight, 160, 40), "Coin", EditorStyles.label);
                if (GUI.Button(new Rect(230, 276 + EditorGUIUtility.singleLineHeight, 100, 100), Resources.Load("image_7") as Texture))
                {
                    RemoveDeletedObjects();
                    CoinsArray coinsArray = GameObject.Find("Level").transform.GetChild(2).GetComponent<CoinsArray>(); //Array of all spawned coins

                    GameObject newCoin = PrefabUtility.InstantiatePrefab(coin) as GameObject; //Spawning new coin
                    newCoin.transform.parent = coinsArray.transform; //Adding parent

                    UnityEditor.Selection.activeObject = newCoin; //Selecting new object

                    if (coinsArray.coins.Count > 0) //Setting start position or position depending last object
                    {
                        Vector3 position = new Vector3(coinsArray.coins[coinsArray.coins.Count - 1].transform.position.x, coinsArray.coins[coinsArray.coins.Count - 1].transform.position.y, coinsArray.coins[coinsArray.coins.Count - 1].transform.position.z + coinsArray.coins[coinsArray.coins.Count - 1].transform.localScale.z / 2 + coin.transform.localScale.z / 2 + 2);
                        newCoin.transform.position = position; //Calculating and setting new position
                    }
                    else
                    {
                        newCoin.transform.position = new Vector3(0, coin.transform.localScale.y * 2, 0);
                    }

                    coinsArray.coins.Add(newCoin); //Adding new coin to array
                }
 

                if (GUI.Button(new Rect(10, 430 + EditorGUIUtility.singleLineHeight, 100, 40) ,"Back"))
                {
                    windowId = 1; //Setting previous window id
                }
                if (GUI.Button(new Rect(115, 430 + EditorGUIUtility.singleLineHeight, 100, 40), "Next"))
                {
                    windowId = 3; //Setting new window id
                }

                break;
            case 3:
                GUILayout.Label("Stage Four | Add Triggers", EditorStyles.boldLabel); //Title Text
                GUILayout.Label("Add the triggers you want and place in the right places", EditorStyles.label);

                image_3 = Resources.Load("image_10", typeof(Texture2D)) as Texture2D; //Add image
                GUILayout.Label(image_3, GUILayout.Width(300), GUILayout.Height(300));

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Add SpawnRoad", GUILayout.Width(120), GUILayout.Height(80)))
                {
                    RemoveDeletedObjects();
                    OthersArray othersArray = GameObject.Find("Level").transform.GetChild(1).GetComponent<OthersArray>();  //Array of all spawned adders/obstacles

                    GameObject newSpawnOn = PrefabUtility.InstantiatePrefab(spawnOn) as GameObject; //Spawning new spawn trigger
                    newSpawnOn.transform.parent = othersArray.transform; //Adding parent

                    UnityEditor.Selection.activeObject = newSpawnOn; //Selecting new object

                    if (othersArray.others.Count > 0) //Setting start position or position depending last object
                    {
                        Vector3 position = new Vector3(0, othersArray.others[othersArray.others.Count - 1].transform.position.y, othersArray.others[othersArray.others.Count - 1].transform.position.z + othersArray.others[othersArray.others.Count - 1].transform.localScale.z / 2 + spawnOn.transform.localScale.z / 2 + 2);
                        newSpawnOn.transform.position = position; //Calculating and setting new position
                    }
                    else
                    {
                        newSpawnOn.transform.position = new Vector3(0, spawnOn.transform.localScale.y * 2, 0);
                    }

                    othersArray.others.Add(newSpawnOn);  //Adding new spawn trigger to array
                }
                if (GUILayout.Button("Add Dead Trigger", GUILayout.Width(120), GUILayout.Height(80)))
                {
                    RemoveDeletedObjects();
                    OthersArray othersArray = GameObject.Find("Level").transform.GetChild(1).GetComponent<OthersArray>();  //Array of all spawned adders/obstacles

                    GameObject newDeadTrigger = PrefabUtility.InstantiatePrefab(deadTrigger) as GameObject; //Spawning new deaad trigger
                    newDeadTrigger.transform.parent = othersArray.transform; //Adding parent

                    UnityEditor.Selection.activeObject = newDeadTrigger; //Selecting new object

                    if (othersArray.others.Count > 0) //Setting start position or position depending last object
                    {
                        Vector3 position = new Vector3(0, othersArray.others[othersArray.others.Count - 1].transform.position.y, othersArray.others[othersArray.others.Count - 1].transform.position.z + othersArray.others[othersArray.others.Count - 1].transform.localScale.z / 2 + spawnOn.transform.localScale.z / 2 + 2);
                        newDeadTrigger.transform.position = position; //Calculating and setting new position
                    }
                    else
                    {
                        newDeadTrigger.transform.position = new Vector3(0, spawnOn.transform.localScale.y * 2, 0);
                    }

                    othersArray.others.Add(newDeadTrigger); //Adding new dead trigger to array
                }
                if (GUILayout.Button("Add Jump Trigger", GUILayout.Width(120), GUILayout.Height(80)))
                {
                    RemoveDeletedObjects();
                    OthersArray othersArray = GameObject.Find("Level").transform.GetChild(1).GetComponent<OthersArray>();  //Array of all spawned adders/obstacles

                    GameObject newJumpTrigger = PrefabUtility.InstantiatePrefab(jumpTrigger) as GameObject; //Spawning new jump trigger
                    newJumpTrigger.transform.parent = othersArray.transform; //Adding parent

                    UnityEditor.Selection.activeObject = newJumpTrigger; //Selecting new object
                     
                    if (othersArray.others.Count > 0) //Setting start position or position depending last object
                    {
                        Vector3 position = new Vector3(0, othersArray.others[othersArray.others.Count - 1].transform.position.y, othersArray.others[othersArray.others.Count - 1].transform.position.z + othersArray.others[othersArray.others.Count - 1].transform.localScale.z / 2 + spawnOn.transform.localScale.z / 2 + 2);
                        newJumpTrigger.transform.position = position; //Calculating and setting new position
                    }
                    else
                    {
                        newJumpTrigger.transform.position = new Vector3(0, spawnOn.transform.localScale.y * 2, 0);
                    }

                    othersArray.others.Add(newJumpTrigger); //Adding new jump trigger to array
                }
                GUILayout.EndHorizontal(); 
                if (GUI.Button(new Rect(10, 430 + EditorGUIUtility.singleLineHeight, 100, 40), "Back"))
                {
                    windowId = 2; //Setting previous window id
                }
                if (GUI.Button(new Rect(115, 430 + EditorGUIUtility.singleLineHeight, 100, 40), "Next"))
                {
                    windowId = 4; //Setting new window id
                    if (!finishSpawned) //Spawning finish
                    {
                        RemoveDeletedObjects();

                        PlatformsArray platformsArray = GameObject.Find("Level").GetComponentInChildren<PlatformsArray>(); //Array of all spawned houses/bridges
                        Vector3 pos = new Vector3(0, platformsArray.platforms[platformsArray.platforms.Count - 1].transform.position.y + platformsArray.platforms[platformsArray.platforms.Count - 1].transform.localScale.y / 2 - finish.transform.localScale.y / 2, platformsArray.platforms[platformsArray.platforms.Count - 1].transform.position.z + platformsArray.platforms[platformsArray.platforms.Count - 1].transform.localScale.z / 2);
                        SpawnObject(finish, pos);

                        finishSpawned = true; //Turn on so that when passing to the previous windows the finish is not compared again
                    }
                }
                break;
            case 4:
                GUILayout.Label("Stage Five | Add Finish", EditorStyles.boldLabel); //Title Text
                GUILayout.Label("The finish has been automatically added, check if it is located correctly", EditorStyles.label);

                image_4 = Resources.Load("image_11", typeof(Texture2D)) as Texture2D; //Add image
                GUILayout.Label(image_4, GUILayout.Width(300), GUILayout.Height(300));

                GUILayout.BeginHorizontal();
                if (GUI.Button(new Rect(10, 430 + EditorGUIUtility.singleLineHeight, 100, 40), "Back"))
                {
                    windowId = 3; //Setting previous window id
                }
                if (GUI.Button(new Rect(115, 430 + EditorGUIUtility.singleLineHeight, 100, 40), "Next"))
                {
                    windowId = 5; //Setting new window id
                }
                GUILayout.EndHorizontal();
                break;
          case 5:
                GUILayout.Label("Stage Six | Adding a scene to the game", EditorStyles.boldLabel);
                GUILayout.Label("Open the settings and add the scene to Scene in Build, then close the settings", EditorStyles.label);

                image_5 = Resources.Load("image_12", typeof(Texture2D)) as Texture2D; 
                GUILayout.Label(image_5, GUILayout.Width(460), GUILayout.Height(181));

                if (GUILayout.Button("Open Settings", GUILayout.Width(100), GUILayout.Height(50)))
                {
                    EditorWindow.GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
                }
                if (GUI.Button(new Rect(10, 430 + EditorGUIUtility.singleLineHeight, 100, 40), "Back"))
                {
                    windowId = 4; //Setting previous window id//Setting previous window id
                }
                if (GUI.Button(new Rect(115, 430 + EditorGUIUtility.singleLineHeight, 100, 40), "Next"))
                {
                    windowId = 6; //Setting new window id
                    RemoveDeletedObjects();
                }
                break;
          case 6:
                GUILayout.Label("End!", EditorStyles.boldLabel); //Title Text 
                GUILayout.Label("Level creation is over! If you have any questions, you can go to the documentation", EditorStyles.label);
                GUILayout.Label("or write to us by mail zaampo.g @gmail.com", EditorStyles.label);
                GUI.Label(new Rect(175, 180 + EditorGUIUtility.singleLineHeight, 160, 40), "!Save the scene first", EditorStyles.boldLabel);

                if (GUI.Button(new Rect(10, 430 + EditorGUIUtility.singleLineHeight, 100, 40), "Back"))
                {
                    windowId = 5; //Setting previous window id
                }
                if (GUI.Button(new Rect(190, 210 + EditorGUIUtility.singleLineHeight, 100, 40), "Let's test it!")) 
                {
                    editorClosed = true; //Setting editor window closed
                    this.Close(); //Closing editor window
                    EditorSceneManager.OpenScene("Assets/Untwist Road/Scenes/" + "Menu.unity"); //Opening main scene
                    GameObject.Find("Canvas").GetComponent<Menu>().TestingMode = true; //Turn on test mode
                    GameObject.Find("Canvas").GetComponent<Menu>().TestingLevelId = EditorBuildSettings.scenes.Length - 1; //Setting test id = new level id
                    UnityEditor.EditorApplication.isPlaying = true; //Starting game
                }
                break; 


        }
        if (GUI.changed)  //If there have been changes
        {
            if (!editorClosed) //And if the editor is open
            {
                if (cylinderCont == null) //If there is no script, get it
                {
                    cylinderCont = GameObject.FindGameObjectWithTag("MainCylinder").GetComponent<CylinderCont>();
                }

                //Get arrays
                PlatformsArray platformsArray = GameObject.Find("Level").GetComponentInChildren<PlatformsArray>(); //Array of all spawned houses/bridges
                OthersArray othersArray = GameObject.Find("Level").transform.GetChild(1).GetComponent<OthersArray>();  //Array of all spawned adders/obstacles
                CoinsArray coinsArray = GameObject.Find("Level").transform.GetChild(2).GetComponent<CoinsArray>(); //Array of all spawned coins

                //Save changes
                EditorUtility.SetDirty(cylinderCont);
                EditorUtility.SetDirty(platformsArray);
                EditorUtility.SetDirty(othersArray);
                EditorUtility.SetDirty(coinsArray);
                EditorSceneManager.MarkSceneDirty(cylinderCont.gameObject.scene); //changes can be saved
            }
        }
    }

    [MenuItem("Level Editor/Open Editor")]
    public static void ObjectsPanel() //Open the level editor by clicking on the button in the menu
    {
        EditorWindow window = (LevelEditor)EditorWindow.GetWindowWithRect(typeof(LevelEditor), new Rect(0, 0, 500, 500));
    }

    [MenuItem("Level Editor/Documentation")]
    private static void Documentation()//Opening the documentation in a browser
    {
        Application.OpenURL("https://drive.google.com/file/d/1RZovTr6P0KWBwVR2GO5n7F8ewgBsI1EO/view?usp=sharing");
    } 

    void NewScene() //Create a new scene and add standard objects
    {
        int levelId = 0;
        levelId = EditorBuildSettings.scenes.Length;
        
        scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
        EditorSceneManager.SaveScene(scene, "Assets/Untwist Road/Scenes/" + "Level_" + levelId + ".unity", false);
        EditorSceneManager.OpenScene("Assets/Untwist Road/Scenes/" + "Level_" + levelId + ".unity");

        SpawnObject(light, Vector3.zero);
        SpawnObject(players, new Vector3(-9, -2, 13));
        SpawnObject(level, Vector3.zero);
        SpawnObject(particle, new Vector3(0, 0, -9));
        SpawnObject(eventSystem, Vector3.zero);

        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.4235294f, 1, 0.6784314f, 1);
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 15;
        RenderSettings.fogEndDistance = 45;

        windowId = 1;

        PlatformsArray platformsArray = GameObject.Find("Level").GetComponentInChildren<PlatformsArray>(); //Array of all spawned houses/bridges
        platformsArray.platforms.Add(level.transform.GetChild(0).GetChild(0).gameObject);
    }
    void SpawnObject(GameObject newObject, Vector3 newPosition)
    {
        GameObject newGameObject = PrefabUtility.InstantiatePrefab(newObject) as GameObject;
        newGameObject.name = newObject.name;
        newGameObject.transform.position = newPosition;
        UnityEditor.Selection.activeObject = newGameObject; //Selecting new object
    }
    void CylinderCustomization(bool add) //Remove or add cylinders by clicking on the button
    {
        if (add == true)
        {
            if (cylinderCont.cylindersNum < 7) //There are a maximum of 8 cylinders, so there is a maximum of 7
            {
                cylinderCont.transform.parent.position = new Vector3(cylinderCont.transform.parent.position.x, cylinderCont.transform.parent.position.y + cylinderCont.maxScale, cylinderCont.transform.parent.position.z);

                Cylinder newCylinder = Instantiate(cylinderCont.prefab);
                Vector3 position = new Vector3(cylinderCont.transform.position.x, cylinderCont.cylinders[cylinderCont.cylinders.Count - 1].transform.position.y - cylinderCont.cylinders[cylinderCont.cylinders.Count - 1].transform.localScale.z, cylinderCont.transform.position.z);
                newCylinder.transform.localPosition = position;

                newCylinder.transform.localScale = new Vector3(cylinderCont.maxScale, cylinderCont.maxScale, cylinderCont.maxScale);
                newCylinder.transform.SetParent(cylinderCont.transform.parent);

                cylinderCont.cylinders.Add(newCylinder);
                cylinderCont.cylindersNum = cylinderCont.cylindersNum + 1;
            }

        }
        else
        {
            if (cylinderCont.cylindersNum > 1) //There are a minimum one cylinder
            {
                DestroyImmediate(cylinderCont.cylinders[cylinderCont.cylinders.Count - 1].gameObject, false);
                cylinderCont.cylinders.RemoveAt(cylinderCont.cylindersNum);
                cylinderCont.cylindersNum = cylinderCont.cylindersNum - 1;

                cylinderCont.transform.parent.position = new Vector3(cylinderCont.transform.parent.position.x, cylinderCont.transform.parent.position.y - cylinderCont.maxScale, cylinderCont.transform.parent.position.z);
            }
        }
    }
    void RemoveDeletedObjects() //Remove deleted objects from the lists so that later there are no errors
    {
        PlatformsArray platformsArray = GameObject.Find("Level").GetComponentInChildren<PlatformsArray>(); //Array of all spawned houses/bridges
        if (platformsArray.platforms.Count != 0)
        {
            for (int i = platformsArray.platforms.Count - 1; i != 0; i--)
            {
                if (platformsArray.platforms[i] == null)
                {
                    platformsArray.platforms.Remove(platformsArray.platforms[i]);
                }
            }
        }
        OthersArray othersArray = GameObject.Find("Level").transform.GetChild(1).GetComponent<OthersArray>();  //Array of all spawned adders/obstacles
        if (othersArray.others.Count != 0)
        {
            for (int i = othersArray.others.Count - 1; i != 0; i--)
        {
            if (othersArray.others[i] == null)
            {
                othersArray.others.Remove(othersArray.others[i]);
            }
        }
    }
        CoinsArray coinsArray = GameObject.Find("Level").transform.GetChild(2).GetComponent<CoinsArray>(); //Array of all spawned coins
        if (coinsArray.coins.Count != 0)
        {
            for (int i = coinsArray.coins.Count - 1; i != 0; i--)
            {
                if (coinsArray.coins[i] == null)
                {
                    coinsArray.coins.Remove(coinsArray.coins[i]);
                }
            }
        }
    }
}
