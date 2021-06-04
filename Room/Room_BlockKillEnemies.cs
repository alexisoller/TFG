using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_BlockKillEnemies : MonoBehaviour {

    public bool blockedToKillEnemies;
    public Room_PlayableArea playableArea;
    public GameObject[] visualBlock;
    public GameObject[] gatesAction;
    public List<GameObject> enemiesToKill = new List<GameObject>();

    private bool start = true;
    private bool play = false;
    private bool end = false;
    private bool active;

    private void Start() {
        if (enemiesToKill.Count == 0) {
            active = false;
        } else {
            active = true;
        }
    }

    private void Update() {
        if (active) { 
            if (playableArea.playerIsInArea) {
                if (blockedToKillEnemies) {
                    if (start) {
                        for (int i = 0; i < gatesAction.Length; i++) {
                            if (!gatesAction[i].gameObject.GetComponent<Room_Doors>().isClosed) {
                                visualBlock[i].gameObject.SetActive(true);
                                gatesAction[i].gameObject.GetComponent<Room_Doors>().closed.SetActive(true);
                                
                            }
                        }
                        start = false;
                        play = true;
                    }
                    if (play) {
                        if (enemiesToKill.Count == 0) {
                            play = false;
                            end = true;
                        }
                    }
                    if (end) {
                        for (int i = 0; i < gatesAction.Length; i++) {
                            if (!gatesAction[i].gameObject.GetComponent<Room_Doors>().isClosed) {
                                visualBlock[i].gameObject.SetActive(false);
                                gatesAction[i].gameObject.GetComponent<Room_Doors>().closed.SetActive(false);
                            }
                        }
                        end = false;
                        blockedToKillEnemies = false;
                    }
                }
            }
        }
    }

    
}
