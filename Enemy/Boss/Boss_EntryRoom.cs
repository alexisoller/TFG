using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_EntryRoom : MonoBehaviour {

    public Room_PlayableArea playerInArea;
    public Room_Doors doorEntryToBoss;
    public Camera mainCamera;
    public GameObject cameraBossPoint;
    public float timeToIntroBoss;
    public GameObject statsBoss;

    private float count, limitCount;
    private bool start = true;

    private void Start() {

        

        limitCount = timeToIntroBoss;
    }

    private void Update() {
        if (start) {
            if (playerInArea.playerIsInArea) {
                Manager_Vault.allowMove = false;
                Manager_Vault.allowDash = false;
                count += Time.deltaTime;
            }

            if (playerInArea.playerIsInArea & mainCamera.transform.position == cameraBossPoint.transform.position &&
                mainCamera.orthographicSize == doorEntryToBoss.sizeCamera && count >= limitCount) {
                Manager_Vault.allowMove = true;
                Manager_Vault.allowDash = true;
                Manager_Vault.firstStage = true;
                statsBoss.SetActive(true);
                start = false;
                FindObjectOfType<Audiomanager>().Stop("GamePlay");
                FindObjectOfType<Audiomanager>().Stop("MenuMusic");
                FindObjectOfType<Audiomanager>().Play("BossMusic");
            }
        }
    }
}
