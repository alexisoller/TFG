using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelector_Manager : MonoBehaviour
{
    #region Variables
    GameObject currentSelected;
    public EventSystem eventSystem;
    public GameObject firstSelectedLevel;
    public bool requiresJoystick;
    #endregion


    void Start() {
        if (requiresJoystick) {
            if (SceneManager.GetActiveScene().name == "LevelSelector") {
                StartCoroutine(FirstSelectedCorrutineLevel());
            }
        }
    }

    void Update() {
        if (requiresJoystick) {
            if (eventSystem.currentSelectedGameObject != null) {
                currentSelected = eventSystem.currentSelectedGameObject;
            } else {
                eventSystem.SetSelectedGameObject(currentSelected);
            }
        }
    }

    #region FirstSelectedCorrutineLevel
    public IEnumerator FirstSelectedCorrutineLevel() {
        eventSystem.SetSelectedGameObject(null);
        yield return null;
        eventSystem.SetSelectedGameObject(firstSelectedLevel);
    }
    #endregion
}
