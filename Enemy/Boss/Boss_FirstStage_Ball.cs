using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FirstStage_Ball : MonoBehaviour {

    public float speedMoveRandom;
    public GameObject original;
    public GameObject exterior;
    public float damagePoints;
    public BoxCollider2D colliderBox;
    public float timeAttackReload;
    public List<GameObject> muzzle = new List<GameObject>();
    public GameObject bullet;
    public bool canShoot;
    public float speedMoveIdle;
    public GameObject bar;
    public GameObject damageVisualAspect;

    private bool newPosition = true;
    private bool moveNewPosition = false;
    private float randomPosX = 0;
    private float randomPosY = 0;
    [HideInInspector] public bool inPosition;
    [HideInInspector] public bool inExternalPosition;
    private bool playerTouch = false;
    private GameObject player;
    private float countAttack;
    private float countShoot, limitCountShoot = 0.2f;

    private bool takeDamage = false;
    private bool effectDamage = false;
    private float damageRecived;
    private int loop;
    private float countDamage, limitCountDamage = 0.1f;
    private float countDamage2, limitCountDamage2;

    private void Start(){
        limitCountDamage2 = limitCountDamage;
    }

    private void Update() {

        transform.Rotate(Vector3.forward * speedMoveIdle * Time.deltaTime);

        if (Manager_Vault.attackMoveBall) {
            NewPositionRandom();
            MoveToNewPosition();
        }

        if (Manager_Vault.moveToInitialPosition) {
            MoveToInitialPosition();
        }

        if (Manager_Vault.moveToInitialPosition_Final) {
            MoveToInitialPosition();
        }

        if (Manager_Vault.moveToExternalPosition) {
            MoveToExternalPosition();
        }

        if (Manager_Vault.attackShootBall) {
            inPosition = false;
            if (canShoot) {
                ShootBullets();
            }
        }

        if (playerTouch) {
            Attack();
        }

        if (Manager_Vault.currentBossPoints >= 50f) {
            if (takeDamage) {
                effectDamage = true;
                Manager_Vault.currentBossPoints = Mathf.Clamp(Manager_Vault.currentBossPoints - damageRecived, 0f, Manager_Vault.healthBossPoints);
                bar.transform.localScale = new Vector3(Manager_Vault.currentBossPoints / Manager_Vault.healthBossPoints, 1, 2);
                takeDamage = false;
            }

            if (effectDamage) {
                if (loop < 2) {
                    if (countDamage < limitCountDamage) {
                        damageVisualAspect.SetActive(true);
                        countDamage += Time.deltaTime;
                    }

                    if (countDamage >= limitCountDamage) {
                        damageVisualAspect.SetActive(false);
                        countDamage2 += Time.deltaTime;
                        if (countDamage2 >= limitCountDamage2)
                        {
                            loop++;
                            countDamage = 0f;
                            countDamage2 = 0f;
                        }
                    }
                } else {
                    loop = 0;
                    damageVisualAspect.SetActive(false);
                    effectDamage = false;
                }
            }
        } else {
            Manager_Vault.firstStage = false;
            Manager_Vault.secondStage = true;
            Destroy(gameObject);
        }

    }

    private void Attack(){
        countAttack += Time.deltaTime;
        if (countAttack >= timeAttackReload) {
            if (!Manager_Vault.isDash) {
                player.GetComponent<Player_Life>().TakeDamage(damagePoints);
            }
            countAttack = 0;
        }
    }

    private void ShootBullets() {
        countShoot += Time.deltaTime;
        if (countShoot >= limitCountShoot){
            for (int i = 0; i < muzzle.Count; i++) {
                Instantiate(bullet, muzzle[i].transform.position, muzzle[i].transform.rotation);
            }
            countShoot = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.GetComponent<Player_Move>() && !Manager_Vault.isDash) {
            playerTouch = true;
            player = col.gameObject;
            player.GetComponent<Player_Life>().TakeDamage(damagePoints);
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.GetComponent<Player_Move>()){
            playerTouch = false;
            countAttack = 0f;
        }
    }

    private void NewPositionRandom() {
        if (newPosition) {
            Vector2 colliderPos = new Vector2(colliderBox.transform.position.x, colliderBox.transform.position.y) + colliderBox.offset;
            randomPosX = Random.Range(colliderPos.x - colliderBox.size.x / 2, colliderPos.x + colliderBox.size.x / 2);
            randomPosY = Random.Range(colliderPos.y - colliderBox.size.y / 2, colliderPos.y + colliderBox.size.y / 2);
            newPosition = false;
            moveNewPosition = true;
        }
    }

    public void MoveToNewPosition() {
        if (moveNewPosition) { 
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(randomPosX, randomPosY), speedMoveRandom * Time.deltaTime);
            inPosition = false;
            inExternalPosition = false;
            if (transform.position == new Vector3(randomPosX, randomPosY, 0f)) {
                moveNewPosition = false;
                newPosition = true;
            }
        }
    }

    private void MoveToInitialPosition() {
        transform.position = Vector2.MoveTowards(transform.position, original.transform.position, speedMoveRandom * Time.deltaTime);
        if (transform.position == original.transform.position) {
            inPosition = true;
        } else {
            inPosition = false;
        }
    }

    private void MoveToExternalPosition() {
        transform.position = Vector2.MoveTowards(transform.position, exterior.transform.position, speedMoveRandom * Time.deltaTime);
        if (transform.position == exterior.transform.position) {
            inExternalPosition = true;
        } else {
            inExternalPosition = false;
        }
    }

    public void TakeDamage(float damagePoints) {
        takeDamage = true;
        damageRecived = damagePoints;
    }
}
