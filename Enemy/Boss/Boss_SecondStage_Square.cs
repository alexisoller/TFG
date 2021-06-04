using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SecondStage_Square : MonoBehaviour {

    public float speedMove;
    public GameObject original;
    public GameObject exterior;
    public float damagePoints;
    public float timeAttackReload;
    public GameObject muzzle; //
    public GameObject bullet; //
    public GameObject bar; //
    public GameObject damageVisualAspect; //

    public float offset;
    public GameObject wave;
    public GameObject wavePointFinal;

    [HideInInspector] public bool inPosition;
    [HideInInspector] public bool inExternalPosition;
    private bool playerTouch = false;
    public GameObject player;
    private float countAttack;
    private float countShoot, limitCountShoot = 0.2f;

    private bool takeDamage = false;
    private bool effectDamage = false;
    private float damageRecived;
    private int loop;
    private float countDamage, limitCountDamage = 0.1f;
    private float countDamage2, limitCountDamage2;
    private float countWave, limitCountWave = 2f;

    private void Start() {
        limitCountDamage2 = limitCountDamage;
    }

    private void Update() {
        if (Manager_Vault.secondStage) {
            if (gameObject.name == "Head") {
                Vector3 dir = player.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);


                if (Manager_Vault.attackShootSquare) {
                    ShootBullets();
                }
            } else {

                if (Manager_Vault.moveToInitialPosition_Final) {
                    MoveToInitialPosition();
                }

                if (Manager_Vault.moveToExternalPosition) {
                    MoveToExternalPosition();
                }

                if (Manager_Vault.attackWaveSquare) {
                    inPosition = false;
                    inExternalPosition = false;
                    Waves();
                }
            }

            if (playerTouch) {
                Attack();
            }

            if (Manager_Vault.currentBossPoints > 0f) {
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
                            if (countDamage2 >= limitCountDamage2) {
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
                Manager_Vault.secondStage = false;
                Destroy(gameObject);
            }
        }
    }

    private void Attack() {
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
        if (countShoot >= limitCountShoot) {
            Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);
            countShoot = 0;
        }
    }

    private void Waves() {
        countWave += Time.deltaTime;
        if (countWave >= limitCountWave) {
            GameObject w = Instantiate(wave, transform.position, transform.rotation);
            w.GetComponentInChildren<Boss_Waves>().wavePointFinal = wavePointFinal;
            countWave = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.GetComponent<Player_Move>() && !Manager_Vault.isDash) {
            playerTouch = true;
            player.GetComponent<Player_Life>().TakeDamage(damagePoints);
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.GetComponent<Player_Move>()) {
            playerTouch = false;
            countAttack = 0f;
        }
    }

    private void MoveToInitialPosition() {
        transform.position = Vector2.MoveTowards(transform.position, original.transform.position, speedMove * Time.deltaTime);
        if (transform.position == original.transform.position) {
            inPosition = true;
        } else {
            inPosition = false;
        }
    }

    private void MoveToExternalPosition() {
        transform.position = Vector2.MoveTowards(transform.position, exterior.transform.position, speedMove * Time.deltaTime);
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
