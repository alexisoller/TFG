using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Pause : MonoBehaviour {

    private bool activePause = false;
    public GameObject canvasPause;

    private void Update() {
        if (Input.GetButtonDown("Pause")) {
            activePause = !activePause;
            canvasPause.SetActive(activePause);
            Time.timeScale = (activePause) ? 0 : 1f;
        }
    }

    public void Restart() {
        FindObjectOfType<Audiomanager>().Play("BtnMenuSound");
        activePause = !activePause;
        canvasPause.SetActive(activePause);
        Time.timeScale = (activePause) ? 0 : 1f;
    }

}
