using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_firstStage : MonoBehaviour {

    public float speedMoveIdle;
    public List<GameObject> balls = new List<GameObject>(); 
    public Room_PlayableArea playerInRoom;
    private CircleCollider2D circleCollision;

    private bool move = true;
    private bool waitAttackMove = true;
    private bool attackMove = false;
    private bool waitAttackShoot = false;
    private bool attackShoot = false;
    private bool vulnerableMoment = false;
    private List<bool> ballsInPosition = new List<bool>();
    private List<bool> ballsInPosition_Final = new List<bool>();
    private bool inPosition, inPosition_Final;


    //Contadores
    private float countInitMove;
    public float limitCountInitMove;
    private float countInitAttack;
    public float limitCountInitAttack;
    private float countAttackMove;
    public float limitCountAttackMove;
    private float countChangeAttack;
    public float limitCountChangeAttack;
    private float countAttackShoot;
    public float limitCountAttackShoot;
    private float countVulnerable;
    public float limitCountVulnerable;

    private void Start() {
        circleCollision = GetComponent<CircleCollider2D>();
    }

    private void Update() {
        if (playerInRoom.playerIsInArea && Manager_Vault.currentBossPoints >= 50f) {
            Move();
            if (Manager_Vault.firstStage) {
                WaitCount_MoveToQuiet();
                WaitCount_QuietToAttackMove();
                AttackMoveBall();
                CheckBallInOriginalPosition();
                WaitCount_AttackMoveToMove();
                WaitCount_QuietToPrepareAttackShoot();
                WaitCount_ExternalPositionToAttackShoot(CheckBallInExternalPosition());
                AttackMoveShoot();
                VulnerableMoment();
                CheckBallInOriginalPosition_Final();
                WaitCount_VulnerableToOriginalPositionAndMove();
            }
        }

        if (!Manager_Vault.firstStage && Manager_Vault.secondStage) {
            balls.Clear();
            circleCollision.enabled = false;
            Manager_Vault.moveToInitialPosition = false;
            Manager_Vault.moveToInitialPosition_Final = false;
            Manager_Vault.moveToExternalPosition = false;
            Destroy(gameObject);
        }
        
    }

    private void Move() {
        if (move) {
            transform.Rotate(Vector3.forward * speedMoveIdle * Time.deltaTime);
        }
    }

    private void WaitCount_MoveToQuiet() {
        if (move) {
            countInitMove += Time.deltaTime;
            if (countInitMove >= limitCountInitMove) {
                move = false;
                countInitMove = 0;
            }
        }
    }

    private void WaitCount_QuietToAttackMove() {
        if (!move && waitAttackMove) {
            countInitAttack += Time.deltaTime;
            if (countInitAttack >= limitCountInitAttack) {
                attackMove = true;
                Manager_Vault.attackMoveBall = true;
                countInitAttack = 0;
            }
        }
    }

    private void AttackMoveBall() {
        if (attackMove) {
            waitAttackMove = false;
            countAttackMove += Time.deltaTime;
            if (countAttackMove >= limitCountAttackMove) {
                attackMove = false;
                Manager_Vault.attackMoveBall = false;
                Manager_Vault.moveToInitialPosition = true;
                countAttackMove = 0;
            }
        }
    }

    private void CheckBallInOriginalPosition() {
        for (int i = 0; i < balls.Count; i++){
            if (balls[i].GetComponent<Boss_FirstStage_Ball>().inPosition) {
                ballsInPosition.Add(true);
            } else {
                ballsInPosition.Add(false);
            }
        }
    }

    private void WaitCount_AttackMoveToMove() {

        for (int i = 0; i < ballsInPosition.Count; i++) {
            if (ballsInPosition[i]) {
                inPosition = true;
            } else {
                inPosition = false;
                ballsInPosition.Clear();
                return;
            }
        }

        if (inPosition && Manager_Vault.moveToInitialPosition) {
            move = true;
            waitAttackShoot = true;
            Manager_Vault.moveToInitialPosition = false;
            inPosition = false;
        }
    }

    private void WaitCount_QuietToPrepareAttackShoot() {
        if (!move && waitAttackShoot) {
            countInitAttack += Time.deltaTime;
            if (countInitAttack >= limitCountInitAttack) {
                Manager_Vault.moveToExternalPosition = true;
                countInitAttack = 0;
            }
        }
    }

    private bool CheckBallInExternalPosition() {
        bool check = false;
        for (int i = 0; i < balls.Count; i++) {
            if (balls[i].GetComponent<Boss_FirstStage_Ball>().inExternalPosition) {
                check = true;
            } else  {
                check = false;
            }
        }
        return check;
    }

    private void WaitCount_ExternalPositionToAttackShoot(bool ballsInExternalPosition) {
        if (ballsInExternalPosition && Manager_Vault.moveToExternalPosition) {
            attackShoot = true;
            Manager_Vault.attackShootBall = true;
            Manager_Vault.moveToExternalPosition = false;
        }
    }

    private void AttackMoveShoot() {
        if (attackShoot) {
            waitAttackShoot = false;
            countAttackShoot += Time.deltaTime;
            if (countAttackShoot >= limitCountAttackShoot) {
                attackShoot = false;
                Manager_Vault.attackShootBall = false;
                vulnerableMoment = true;
                countAttackShoot = 0;
            }
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

    private void CheckBallInOriginalPosition_Final() {
        for (int i = 0; i < balls.Count; i++) {
            if (balls[i].GetComponent<Boss_FirstStage_Ball>().inPosition) {
                ballsInPosition_Final.Add(true);
            } else {
                ballsInPosition_Final.Add(false);
            }
        }
    }

    private void WaitCount_VulnerableToOriginalPositionAndMove() {
        for (int i = 0; i < ballsInPosition_Final.Count; i++) {
            if (ballsInPosition_Final[i]) {
                inPosition_Final = true;
            } else {
                inPosition_Final = false;
                ballsInPosition_Final.Clear();
                return;
            }
        }

        if (inPosition_Final && Manager_Vault.moveToInitialPosition_Final) {
            move = true;
            waitAttackMove = true;
            Manager_Vault.moveToInitialPosition_Final = false;
            inPosition_Final = false;
        }

    }
}
