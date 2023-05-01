using System.Collections.Generic;
using UnityEngine;

using Input = InputWrapper.Input; //This is needed to simulate touches on a computer. Remove the comment if you are testing the game on a pc, not on a smartphone.
public class CylinderCont : MonoBehaviour
{

    [Header("Cylinders Number at this level")]
    [Range(1,6)]
    public int cylindersNum;

    [Header("Cylinders prefab. Path:Prefabs/Players/Cylinder")]
    public Cylinder prefab;

    [HideInInspector]public float maxScale = 0.6f;

    [Header("Player Speed")]
    public float speed;

    [Header("Touch speed")]
    public float defaultSpeed = 0.025f;

    [Header("SpawnBlock Prefab")]
    public GameObject SpawnBlock;

    CameraFollow cam;
    Transform parent;
    Vector3 offset;
    Rigidbody rb;
    Touch touch;

    float WidthNow, Scale, factor, smooth = 8.0f, nowSpeed = 0.025f;
    int isStarted;

    [HideInInspector] List<GameObject> spawnedBlocks = new List<GameObject>();
    [HideInInspector] public bool Stretch, Finish, Ground;
    [HideInInspector] public float distance, zDistance, posWill, xPosOfSpawn, xPosRight, xPosLeft, xPosChange, yPos, zScale, minusScale;
    [HideInInspector] public List<Cylinder> cylinders = new List<Cylinder>();
    [HideInInspector] public Vector3 posStart;
    [HideInInspector] public  Animator animator;
    [HideInInspector] public PlayerPos playerpos;
    [HideInInspector] public Rigidbody playerrb;

    void Start()
    {
        StartUp(); //Get objects and set parametrs to default
        ChangeScale(); //Update cylinders scale
        CalculateTouchMovementSpeed(); //Calculate speed depending of screen size
    }

    void FixedUpdate()
    {
        if (isStarted != PlayerPrefs.GetInt("isStarted")) //If game status changed
        {
            isStarted = PlayerPrefs.GetInt("isStarted"); //Get new status
        }

        if (isStarted == 1) // If game is started
        {
            if (Input.touchCount > 0) //Controlling the character left-right
            {
                touch = Input.GetTouch(0); //Get touch

                if (touch.phase == TouchPhase.Moved) //If touch moved
                {
                    if (!Finish) //If finish == false
                    {
                        if (Ground) //Ground true
                        {
                            if (!Stretch) //Stretch false
                            {
                                Vector3 newPos = new Vector3(Mathf.Clamp(transform.position.x + touch.deltaPosition.x * nowSpeed, -1.9f, 1.9f), transform.position.y, transform.position.z); //The player's position change is limited, maximum -1.9f and 1.9f by x
                                transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * smooth); //Change the coordinates of the player, to those that we just calculated
                            }
                            else
                            {
                                Vector3 newPos = new Vector3(Mathf.Clamp(transform.position.x + touch.deltaPosition.x * nowSpeed, -4.2f, 4.2f), transform.position.y, transform.position.z); //The player's position change is limited, maximum -4.2f and 4.2f by x
                                transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * smooth); //Change the coordinates of the player, to those that we just calculated
                            }
                        }
                        else
                        {
                            Vector3 newPos = new Vector3(Mathf.Clamp(transform.position.x + touch.deltaPosition.x * nowSpeed, -4.2f, 4.2f), transform.position.y, transform.position.z); //The player's position change is limited, maximum -4.2f and 4.2f by x
                            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * smooth); //Change the coordinates of the player, to those that we just calculated
                        }
                    }
                }
            }

            SetPosition(cylindersNum); //Set position for cylinders

            if (Scale != PlayerPrefs.GetFloat("Scale")) //If the cylinder size changed
            {
                Scale = PlayerPrefs.GetFloat("Scale"); //Get new scale

                if (Scale <= 0) //If scale < 0
                {
                    if (cylindersNum > 1) //If there are more than 1 cylinders
                    {
                        DestroyLastCylinder(); //Remove last cylinder
                    }
                    else //if it is alone
                    {
                        if (Stretch == false) //If the road isn't spawning
                        {
                            DeadInGround(); //Player dead in ground

                        }
                        else
                        {
                            DeadInRoad(); //Player dead in road
                        }
                    }
                }
                ChangeScale(); //Update scale for cylinders
            }
            if (Stretch == true) //If the road is every 0.0625f decrease the cylinder by minusScale
            {
                PlayerPrefs.SetFloat("Scale", Scale - minusScale); //Save scale value

                if (transform.position.x < xPosRight || transform.position.x > xPosLeft || transform.position.z >= posWill) //If the player leaves the road to the right or left, the road is no longer built, and if player has spawned all road
                {
                    Stretch = false; //Stop spawning road
                }

                if (transform.position.z > posStart.z) //Calculate the coordinates of where to spawn the road
                {
                    zDistance = transform.position.z - posStart.z; //Distance traveled by the player
                    if (zDistance > distance) 
                    {
                        zDistance = distance;
                    }
                    spawnedBlocks[spawnedBlocks.Count - 1].transform.position = new Vector3(spawnedBlocks[spawnedBlocks.Count - 1].transform.position.x, spawnedBlocks[spawnedBlocks.Count - 1].transform.position.y, posStart.z + zDistance / 2); //Calculate spawned road position depending spawned road size
                    spawnedBlocks[spawnedBlocks.Count - 1].transform.localScale = new Vector3(1.2f, 0.1f, zDistance); //Update road scale
                }
            }

            yPos = transform.position.y + transform.localScale.z / 2; //Player position y
            if (playerrb.transform.position.y <= yPos) //If the player is below the upper cylinder, we put him to the upper cylinder
            {
                playerrb.transform.position = new Vector3(playerrb.transform.position.x, yPos, playerrb.transform.position.z); //Update player position
            }
            if (!Ground) //If there is more than one cylinder and the value Ground! = The value of the last cylinder,                                //we get this value and if Ground = true, turn on the running animation.
            {
                if (cylindersNum > 1) //If cylinder more than one
                {
                    if (Ground != cylinders[cylinders.Count - 1].Ground && animator.GetCurrentAnimatorStateInfo(0).IsName("run")) //If cylinders are flying,  not on ground
                    {
                        Ground = cylinders[cylinders.Count - 1].Ground; //Main cylidner bool ground == last cylinder bool ground
                        if (Ground) //If ground == true
                        {
                            animator.Play("run"); //Play run animation
                            playerrb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation; //Freeze position z and rotation
                        }
                    }
                }
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) //If animator play dead animation
            {
                PlayerPrefs.SetInt("isDeaded", 1); //Game over
                PlayerPrefs.SetInt("isStarted", 0); //Stop game
            }

            if (Ground) //If on the ground, it means running
            {
                animator.Play("run"); //Play run animation
            }
            if (Finish) //Increased speed at the finish
            {
                speed = 16;
            }
        }
        else if(PlayerPrefs.GetInt("isDeaded") != 1 && !Finish) //If the player has not lost and is not at the finish line, then he simply stands
        {
            animator.Play("idle"); //Play idle animation
        }
}

    void StartUp()
    {    
        isStarted = 0; //The game is not started by default

        PlayerPrefs.SetInt("CylinderNum", cylindersNum); //Set cylinder number
        PlayerPrefs.SetFloat("Scale", maxScale); //Setting the default cylinder dimensions
        Scale = maxScale; //Set scale = max scale, when game started last cylinder have max scale

        parent = gameObject.transform.parent; //Get parent for new cyliders

        playerpos = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPos>(); //Get player position script
        playerrb = playerpos.gameObject.GetComponent<Rigidbody>(); //Get player rigidbody
        animator = playerpos.gameObject.GetComponent<Animator>(); //Get player animator

        rb = gameObject.GetComponent<Rigidbody>(); //Get this cylidner rigidbody

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>(); //Get camera follow script
    }
    void CalculateTouchMovementSpeed()
    {
        WidthNow = Screen.width; //The default is 720
        factor = 1440 / WidthNow; //Calculating the screen resolution ratio
        nowSpeed = defaultSpeed * factor; //The speed of movement of the character on different phones will be different, due to different screen resolutions. With this function, it will be the same
    }
    void SetPosition(int set) //"set" - number of cylinders
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World); //Forward movement
        for (int i = 2; i <  set + 1; i++) //Set position for all cylinders
        {
            cylinders[i].transform.position = new Vector3(transform.position.x, cylinders[i].transform.position.y, transform.position.z); //Set the same position for all cylinders
        }
    }
    void ChangeScale() 
    {
        if (Scale > maxScale + 0.05f) //If the size is larger than the maximum, set the maximum size
        {
            cylinders[cylinders.Count - 1].transform.localScale = new Vector3(maxScale, 0.6f, maxScale); //Set max scale to last cylinder
            if (cylindersNum < 8) //If there are less than eight cylinders, spawn a new one, if eight, simply set the maximum size
            {
                SpawnNewCylinder(); //Spawn new cylinder
            }
            else
            {
                Scale = maxScale; //If cylinders number == 8, set max scale to last cylinder
                PlayerPrefs.SetFloat("Scale", Scale); //Save scale
            }
        }
        else
        {
            cylinders[cylinders.Count - 1].transform.localScale = new Vector3(Scale, 0.6f, Scale); //If it isn't eighth cylinder, set scale to him
        }
    }
    public void SpawnNewRoadBlock()
    {
        if (Scale >= 0.01f || cylindersNum > 1) //If the size is more than 0.01 or more than one cylinder, you can spawn
        {
            GameObject newRoadBlock = Instantiate(SpawnBlock); //Spawn the road block

            newRoadBlock.transform.localPosition = new Vector3(transform.position.x, posStart.y, posStart.z); //Set coordinates
            spawnedBlocks.Add(newRoadBlock); //Add new road block to list

            zScale = spawnedBlocks[spawnedBlocks.Count - 1].transform.localScale.z; //zScale - road length

            Stretch = true; //true, becouse road spawning right now

            xPosOfSpawn = transform.position.x; //start position x 
            xPosRight = xPosOfSpawn - xPosChange; //Maximum point to move to the right
            xPosLeft = xPosOfSpawn + xPosChange; //Maximum point to move to the left

            posWill = posStart.z + distance; //Finish point

            zDistance = 0; //All distance
        }
    }
    void SpawnNewCylinder() //Save the cylinder to the past and add it to the cylinder sheet
    {
        Cylinder newCylinder = Instantiate(prefab); //Spawn new cylinder
        Vector3 position = new Vector3(transform.position.x, cylinders[cylinders.Count - 1].transform.position.y - cylinders[cylinders.Count - 1].transform.localScale.z, transform.position.z); //Calculate new position depending last cylinder
        newCylinder.transform.localPosition = position; //Set new position

        newCylinder.transform.localScale = new Vector3(0.01f, 0.6f, 0.01f); //Set default scale

        Scale = 0.01f; //Update scale
        PlayerPrefs.SetFloat("Scale", Scale); //Save

        newCylinder.transform.SetParent(parent); //Set parent for new cylinder

        cylinders.Add(newCylinder); //Add new cylinder to list

        cylindersNum = cylindersNum + 1; //Increase cylinder number
        PlayerPrefs.SetInt("CylinderNum", cylindersNum); //Save
    }
    public void DestroyLastCylinder() //Remove the last cylinder from the end
    {
        if (cylindersNum > 1)
        {
            Destroy(this.cylinders[cylinders.Count - 1].gameObject); //Destroy last cylinder
            cylinders.RemoveAt(cylindersNum); //Remove it from list

            cylindersNum = cylindersNum - 1; //Decreasing the cylinders number
            PlayerPrefs.SetInt("CylinderNum", cylindersNum); //Save

            Scale = maxScale; //Now last cylinder have maximum scale
            PlayerPrefs.SetFloat("Scale", Scale); //Save
        }
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Dead") //If main cylinder collision with dead trigger
        {
            IsFall(); //Player dead becouse cylinders fallen
        }
        if (target.tag == "GroundOff")
        {
            Ground = false; //Set bool ground false

            animator.Play("jump"); //Player jump animation
            playerrb.AddForce(0, 3.5f, 0, ForceMode.Impulse); //Add force to player
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "GroundOn") //When the player is back on the ground
        {
            if (isStarted == 1) //If game started
            {
                Ground = true; //Set bool ground true
                animator.Play("run"); //Play running animation
                playerrb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation; //Freeze rotation and position z
            }
        }
    }
    void DeadInGround() //Dead on collision with a conventional obstacle
    {
        PlayerPrefs.SetInt("isDeaded", 1); //Set game over = 1
        PlayerPrefs.SetInt("isStarted", 0); //Stop game

        playerpos.enabled = false; //Player position dont depening of cylinders
        playerrb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; //Freezee rotation y and z
        playerrb.AddForce(0, 3f, 2f, ForceMode.Impulse); //Add force

        animator.Play("dead"); //Play dead animation

        Scale = 0; //Scale to null
        PlayerPrefs.SetFloat("Scale", Scale); //Save

        cylinders[cylindersNum].transform.localScale = new Vector3(0, 0.6f, 0); //Set scale to null
    }
    void DeadInRoad() //Dead when the cylinders run out when spawning
    {
        Stretch = false; //Stop road spawning

        if (!Finish) //If finish == false
        {
            PlayerPrefs.SetInt("isDeaded", 1); //Player dead
        }
        else
        {
            cam.Finished(); //Not, change cam offset
        }
        PlayerPrefs.SetInt("isStarted", 0); //Stop game

        playerpos.enabled = false; //Player position dont depening of cylinders
        animator.Play("jump-up"); //Play jump animation
        playerrb.constraints = RigidbodyConstraints.FreezeRotation; //Freeze rotation
        playerrb.AddForce(0, 8f, 3f, ForceMode.Impulse); //Add force

        Scale = 0; //Scale to null
        PlayerPrefs.SetFloat("Scale", Scale); //Save

        cylinders[cylindersNum].transform.localScale = new Vector3(0, 0.6f, 0); //Set cylinder scale to null

        Rigidbody rb = cylinders[cylindersNum].GetComponent<Rigidbody>(); //Get cylider rigidbody
        rb.constraints = RigidbodyConstraints.FreezeAll; //Freeze all 
    }
   void IsFall() //Dead by falling into the abyss
    {
        PlayerPrefs.SetInt("isDeaded", 1); //Set game over
        PlayerPrefs.SetInt("isStarted", 0); //Stop game

        animator.Play("jump-up"); //Play jump up animation

        playerpos.enabled = false; //Player dont depending of cylinder
        cam.enabled = false; //Camera dont depending of player
    }

}
