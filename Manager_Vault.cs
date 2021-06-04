using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Vault : MonoBehaviour {

    #region Player Variables
    public static bool allowMove = true;
    public static bool allowDash = true;
    public static bool isDash = false;
    public static float amountReloadDash = 0.7f;
    #endregion

    #region Room Variables
    public static float speedDoors = 0.2f;
    #endregion

    #region Enemies Variables
    public static bool allowMoveEnemies = true;
    public static bool allowAttackEnemies = true;
    #endregion

    #region Boss Variables
    public static bool deathBoss = false;
    public static float healthBossPoints;
    public static float currentBossPoints;

    public static bool firstStage = false;
    public static bool attackMoveBall = false;
    public static bool moveToInitialPosition = false;
    public static bool moveToInitialPosition_Final = false;
    public static bool moveToExternalPosition = false;
    public static bool attackShootBall = false;

    public static bool secondStage = false;
    public static bool allowMoveBossSquare = true;
    public static bool attackShootSquare = false;
    public static bool attackWaveSquare = false;
    #endregion
    private void Start() {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void Reset() {
        allowMove = true;
        allowDash = true;
        isDash = false;
        amountReloadDash = 0.7f;

        speedDoors = 0.2f;

        allowMoveEnemies = true;
        allowAttackEnemies = true;

        deathBoss = false;

        firstStage = false;
        attackMoveBall = false;
        moveToInitialPosition = false;
        moveToInitialPosition_Final = false;
        moveToExternalPosition = false;
        attackShootBall = false;

        secondStage = false;
        allowMoveBossSquare = true;
        attackShootSquare = false;
        attackWaveSquare = false;
    }
}
