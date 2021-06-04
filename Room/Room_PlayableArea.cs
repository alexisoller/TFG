using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_PlayableArea : MonoBehaviour{

    [HideInInspector] public bool playerIsInArea;

    private GameObject dashArea;

    private void Start() {
        dashArea = GetComponentInChildren<Room_DashableArea>().gameObject;
        dashArea.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.GetComponent<Player_Move>() && !Manager_Vault.isDash) {
            dashArea.SetActive(true);
            playerIsInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.GetComponent<Player_Move>() && !Manager_Vault.isDash) {
            playerIsInArea = false;
            dashArea.SetActive(false);
        }
    }
}
