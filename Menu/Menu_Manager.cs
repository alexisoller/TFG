using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour {

    #region Variables
    GameObject currentSelected;
    public EventSystem eventSystem;
    public GameObject firstSelectedMenu, firstSelectedCredits, firstSelectedLevels, firstSelectedGames;
    public bool requiresJoystick;
    public string sceneToPlay;
    #endregion


    private void Awake(){
        if (SceneManager.GetActiveScene().name == "Menu") {
            

            FindObjectOfType<Audiomanager>().Stop("GamePlay");
            FindObjectOfType<Audiomanager>().Play("MenuMusic");
            FindObjectOfType<Audiomanager>().Stop("BossMusic");
        }
    }

    #region Start
    void Start() {
        
        if (requiresJoystick) {
            if (SceneManager.GetActiveScene().name == "Menu") {
                StartCoroutine(FirstSelectedCorrutinMenu());
            }
            if (SceneManager.GetActiveScene().name == "Credits") {
                StartCoroutine(FirstSelectedCorrutinCredits());
            }
            if (SceneManager.GetActiveScene().name == "LevelSelector")
            {
                StartCoroutine(FirstSelectedCorrutinLevels());
            }
            if (SceneManager.GetActiveScene().name == "Level01" || SceneManager.GetActiveScene().name == "Tutorial") {
                StartCoroutine(FirstSelectedCorrutinGames());
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    #endregion

    #region Update
    void Update() {
        if (eventSystem.currentSelectedGameObject != null) {
            currentSelected = eventSystem.currentSelectedGameObject;
        } else {
            eventSystem.SetSelectedGameObject(currentSelected);
        }
    }
    #endregion

    #region FirstSelectedCorrutinMenu
    public IEnumerator FirstSelectedCorrutinMenu() {
        eventSystem.SetSelectedGameObject(null);
        yield return null;
        eventSystem.SetSelectedGameObject(firstSelectedMenu);
    }
    #endregion

    #region FirstSelectedCorrutinCredits
    public IEnumerator FirstSelectedCorrutinCredits() {
        eventSystem.SetSelectedGameObject(null);
        yield return null;
        eventSystem.SetSelectedGameObject(firstSelectedCredits);
    }
    #endregion

    #region FirstSelectedCorrutinLevels
    public IEnumerator FirstSelectedCorrutinLevels()
    {
        eventSystem.SetSelectedGameObject(null);
        yield return null;
        eventSystem.SetSelectedGameObject(firstSelectedLevels);
    }
    #endregion

    #region FirstSelectedCorrutinGames
    public IEnumerator FirstSelectedCorrutinGames() {
        eventSystem.SetSelectedGameObject(null);
        yield return null;
        eventSystem.SetSelectedGameObject(firstSelectedGames);
    }
    #endregion

    #region PlayButton
    public void Play() {
        
        FindObjectOfType<Audiomanager>().Play("BtnMenuSound");
        SceneManager.LoadScene(sceneToPlay);
    }
    #endregion

    #region CreditsButton
    public void Credits() {
        FindObjectOfType<Audiomanager>().Play("BtnMenuSound");
        SceneManager.LoadScene("Credits");
    }
    #endregion

    #region ExitButton
    public void Exit() {
        FindObjectOfType<Audiomanager>().Play("BtnMenuSound");
        Application.Quit();
    }
    #endregion

    #region BackButton
    public void Back() {
        FindObjectOfType<Audiomanager>().Play("BtnMenuSound");
        SceneManager.LoadScene("Menu");
    }
    #endregion

    #region LevelButton
    public void Level01() {SceneManager.LoadScene("Level01");}
    public void Level02() {SceneManager.LoadScene("Level02");}
    public void Level03() {SceneManager.LoadScene("Level03");}
    #endregion


    
}
