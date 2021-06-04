using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SecondStage : MonoBehaviour {

    public float speedMoveIdle;
    public List<GameObject> squares = new List<GameObject>();
    public Room_PlayableArea playerInRoom;

    private bool move = true;
    private bool waitAttackShoot = true; 
    private bool attackShoot = false; 
    private bool waitAttackWave = false; 
    private bool attackWave = false; 
    private bool vulnerableMoment = false;
    private List<bool> squareInPosition_Final = new List<bool>(); 
    private bool inPosition_Final;


    //Contadores
    private float countInitMove;
    public float limitCountInitMove;
    private float countInitAttack;
    public float limitCountInitAttack;
    private float countAttackShoot; 
    public float limitCountAttackShoot; 
    private float countAttackWave; 
    public float limitCountAttackWave; 
    private float countVulnerable;
    public float limitCountVulnerable;


    private void Update() {
        print("SecondStage: " + Manager_Vault.secondStage);
        Debug.Log("Current Boss Point: " + Manager_Vault.currentBossPoints);
        if (playerInRoom.playerIsInArea && Manager_Vault.currentBossPoints > 0f) {
            Move();
            if (Manager_Vault.secondStage) {
                WaitCount_MoveToQuiet();
                WaitCount_QuietToAttackShoot();
                AttackShootSquare();
                WaitCount_QuietToAttackWave();
                AttackWaveSquare();
                WaitCount_AttackWaveToExternalPosition(CheckSquareInExternalPosition());
                VulnerableMoment();
                CheckSquareInOriginalPosition_Final();
                WaitCount_VulnerableToOriginalPositionAndMove();
            }
        }
        
        if (!Manager_Vault.secondStage && Manager_Vault.currentBossPoints <= 0f) {
            print("Entra");
            squares.Clear();
            Manager_Vault.deathBoss = true;
            Destroy(gameObject);
        }

    }

    private void Move() {
        if (move) {
            transform.Rotate(-Vector3.forward * speedMoveIdle * Time.deltaTime);
        }
    }

    private void WaitCount_MoveToQuiet() {
        if (move && !waitAttackWave) {
            countInitMove += Time.deltaTime;
            if (countInitMove >= limitCountInitMove) {
                move = false;
                Manager_Vault.allowMoveBossSquare = false;
                countInitMove = 0;
            }
        }
    }

    private void WaitCount_QuietToAttackShoot() {
        if (!move && waitAttackShoot) {
            countInitAttack += Time.deltaTime;
            if (countInitAttack >= limitCountInitAttack) {
                attackShoot = true;
                Manager_Vault.attackShootSquare = true;
                countInitAttack = 0;
            }
        }
    }

    private void AttackShootSquare() {
        if (attackShoot) {
            waitAttackShoot = false;
            countAttackShoot += Time.deltaTime;
            if (countAttackShoot >= limitCountAttackShoot) {
                attackShoot = false;
                Manager_Vault.attackShootSquare = false;
                move = true;
                Manager_Vault.allowMoveBossSquare = true;
                waitAttackWave = true;
                countAttackShoot = 0;
            }
        }
    }

    private void WaitCount_QuietToAttackWave() {
        if (move && waitAttackWave) {
            countInitAttack += Time.deltaTime;
            if (countInitAttack >= limitCountInitAttack) {
                attackWave = true;
                Manager_Vault.attackWaveSquare = true;
                countInitAttack = 0;
            }
        }
    }

    private void AttackWaveSquare() {
        if (attackWave) { 
            countAttackWave += Time.deltaTime;
            if (countAttackWave >= limitCountAttackWave) {
                attackWave = false;
                waitAttackWave = false; //
                move = false; //
                Manager_Vault.attackWaveSquare = false;
                Manager_Vault.moveToExternalPosition = true;
                countAttackWave = 0;
            }
        }
    }

    private bool CheckSquareInExternalPosition() {
        bool check = false;
        for (int i = 0; i < squares.Count; i++) {
            if (squares[i].GetComponent<Boss_SecondStage_Square>().inExternalPosition) {
                check = true;
            } else {
                check = false;
            }
        }
        return check;
    }

    private void WaitCount_AttackWaveToExternalPosition(bool squaresInExternalPosition) {
        if (squaresInExternalPosition && Manager_Vault.moveToExternalPosition) {
            vulnerableMoment = true;
            Manager_Vault.moveToExternalPosition = false;
        }
    }

    private void VulnerableMoment() {
        if (vulnerableMoment) {
            countVulnerable += Time.deltaTime;
            if (countVulnerable >= limitCountVulnerable) {
                Manager_Vault.moveToInitialPosition_Final = true;
                vulnerableMoment = false;
                countVulnerable = 0;
            }
        }
    }

    private void CheckSquareInOriginalPosition_Final() {
        for (int i = 0; i < squares.Count; i++) {
            if (squares[i].GetComponent<Boss_SecondStage_Square>().inPosition) {
                squareInPosition_Final.Add(true);
            } else {
                squareInPosition_Final.Add(false);
            }
        }
    }

    private void WaitCount_VulnerableToOriginalPositionAndMove() {
        for (int i = 0; i < squareInPosition_Final.Count; i++) {
            if (squareInPosition_Final[i]) {
                inPosition_Final = true;
            } else {
                inPosition_Final = false;
                squareInPosition_Final.Clear();
                return;
            }
        }

        if (inPosition_Final && Manager_Vault.moveToInitialPosition_Final) {
            move = true;
            Manager_Vault.allowMoveBossSquare = true;
            waitAttackShoot = true;
            Manager_Vault.moveToInitialPosition_Final = false;
            inPosition_Final = false;
        }

    }
}
