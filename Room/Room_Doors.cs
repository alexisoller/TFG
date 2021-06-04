using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room_Doors : MonoBehaviour {

    [Header("General:")]
    public bool isClosed;
    public GameObject enter, exit, closed;
    public GameObject visualAspect;
    public GameObject mainCamera;
    [HideInInspector] public float sizeCamera;
    public GameObject nextCameraPoint;
    public Room_PlayableArea playerInRoom;
    [Header("Entrance to the Next Level:")]
    public bool isEntryToNextLevel;
    public string sceneToLoad;
    [Header("Entrance to the Boss:")]
    public bool isEntryToBoss;
    public GameObject doorClosed, doorBlock;
    [Header("Entrance to the Dungeon:")]
    public bool isEntryToDungeon;
    public GameObject playerBody;
    public GameObject playerVisualAspect;
    public GameObject entranceAndExit;
    public GameObject enterDungeon, exitDungeon;
    public GameObject enterDungeonCameraPoint;
    public GameObject selfCameraPoint;
    public GameObject blockEntrance, closeEntrance;

    private bool pass = false, nextLevel = false;
    private bool enterOk = false;
    private float sizeCameraRoom = 9;
    private float sizeCameraRoomBoss = 25;


    private void Start() {
        if (isClosed) {
            closed.gameObject.SetActive(true);
            visualAspect.SetActive(false);
            enter.SetActive(false);
            exit.SetActive(false);
        }
        if (isEntryToDungeon) {
            playerBody.transform.position = enterDungeon.transform.position;
            mainCamera.transform.position = enterDungeonCameraPoint.transform.position;
            playerVisualAspect.SetActive(false);
        } else {
            entranceAndExit.SetActive(false);
        }
        if (isEntryToBoss) {
            sizeCamera = sizeCameraRoomBoss;
        } else {
            sizeCamera = sizeCameraRoom;
        }
    }

    private void Update() {
        if (pass) {
            if (playerBody.transform.position == enter.transform.position || enterOk) {
                enterOk = true;
                if (playerBody.transform.position == exit.transform.position && 
                    mainCamera.transform.position == nextCameraPoint.transform.position &&
                    mainCamera.gameObject.GetComponent<Camera>().orthographicSize >= sizeCamera) {
                    if (isEntryToBoss) {
                        doorBlock.SetActive(true);
                        doorClosed.SetActive(true);
                    }
                    mainCamera.gameObject.GetComponent<Camera>().orthographicSize = sizeCamera;
                    playerBody.GetComponent<CircleCollider2D>().enabled = true;
                    enterOk = false;
                    pass = false;
                    Manager_Vault.allowMove = true;
                    Manager_Vault.allowDash = true;
                    Manager_Vault.allowMoveEnemies = true;
                    Manager_Vault.allowAttackEnemies = true;
                } else {
                    mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, nextCameraPoint.transform.position, Manager_Vault.speedDoors * 2);
                    if (mainCamera.gameObject.GetComponent<Camera>().orthographicSize != sizeCamera) {
                        mainCamera.gameObject.GetComponent<Camera>().orthographicSize += Time.deltaTime * 2;
                    }
                    
                    playerBody.transform.position = Vector2.MoveTowards(playerBody.transform.position, exit.transform.position, Manager_Vault.speedDoors);
                }
            } else {
                playerBody.GetComponent<CircleCollider2D>().enabled = false;
                playerBody.transform.position = Vector2.MoveTowards(playerBody.transform.position, enter.transform.position, Manager_Vault.speedDoors);
            }
        }

        if (nextLevel) {
            if (playerBody.transform.position == enter.transform.position || enterOk) {
                enterOk = true;
                if (playerBody.transform.position == exit.transform.position) {
                    playerBody.GetComponent<CircleCollider2D>().enabled = true;
                    Manager_Vault.allowMove = true;
                    Manager_Vault.allowDash = true;
                    Manager_Vault.allowMoveEnemies = true;
                    Manager_Vault.allowAttackEnemies = true;
                    enterOk = false;
                    nextLevel = false;
                    if (sceneToLoad == "Level01") {
                        FindObjectOfType<Audiomanager>().Play("GamePlay");
                        FindObjectOfType<Audiomanager>().Stop("MenuMusic");
                        FindObjectOfType<Audiomanager>().Stop("BossMusic");
                    }
                    SceneManager.LoadScene(sceneToLoad);
                } else {
                    playerBody.transform.position = Vector2.MoveTowards(playerBody.transform.position, exit.transform.position, Manager_Vault.speedDoors);
                }
            } else {
                playerBody.GetComponent<CircleCollider2D>().enabled = false;
                playerBody.transform.position = Vector2.MoveTowards(playerBody.transform.position, enter.transform.position, Manager_Vault.speedDoors);
            }
        }

        if (isEntryToDungeon) {
            if (playerBody.transform.position == enterDungeon.transform.position || enterOk) {
                enterOk = true;
                playerVisualAspect.SetActive(true);
                if (playerBody.transform.position == exitDungeon.transform.position && mainCamera.transform.position == selfCameraPoint.transform.position) {
                    playerBody.GetComponent<CircleCollider2D>().enabled = true;
                    enter.gameObject.SetActive(false);
                    exit.gameObject.SetActive(false);
                    blockEntrance.SetActive(true);
                    closeEntrance.SetActive(true);
                    entranceAndExit.SetActive(false);
                    isEntryToDungeon = false;
                    Manager_Vault.allowMove = true;
                    Manager_Vault.allowDash = true;
                    Manager_Vault.allowMoveEnemies = true;
                    Manager_Vault.allowAttackEnemies = true;
                } else {
                    mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, selfCameraPoint.transform.position, Manager_Vault.speedDoors * 2);
                    playerBody.transform.position = Vector2.MoveTowards(playerBody.transform.position, exitDungeon.transform.position, Manager_Vault.speedDoors);
                }
            } else {
                playerBody.GetComponent<CircleCollider2D>().enabled = false;
                playerBody.transform.position = Vector2.MoveTowards(playerBody.transform.position, enterDungeon.transform.position, Manager_Vault.speedDoors);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(!isEntryToDungeon){
            if (!isClosed && !isEntryToNextLevel && playerInRoom.playerIsInArea) {
                if (col.gameObject.GetComponent<Player_Move>()) {
                    playerBody = col.gameObject;
                    pass = true;
                    nextLevel = false;
                    Manager_Vault.allowMove = false;
                    Manager_Vault.allowDash = false;
                    Manager_Vault.allowMoveEnemies = false;
                    Manager_Vault.allowAttackEnemies = false;
                }
            }

            if (!isClosed && isEntryToNextLevel && playerInRoom.playerIsInArea) {
                if (col.gameObject.GetComponent<Player_Move>()) {
                    playerBody = col.gameObject;
                    nextLevel = true;
                    pass = false;
                    Manager_Vault.allowMove = false;
                    Manager_Vault.allowDash = false;
                    Manager_Vault.allowMoveEnemies = false;
                    Manager_Vault.allowAttackEnemies = false;
                }
            }
        }
        
    }
}
