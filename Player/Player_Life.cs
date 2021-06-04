using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Life : MonoBehaviour {

    public Animator animator;
    private bool isPlayingAnim;

    public float healthPoints;
    public GameObject damageVisualAspect;
    public Image bar;
    public GameObject canvasDeath;
    public GameObject playerVisualAspect;
    public GameObject shadowVisualAspect;
    public GameObject deathEffect;
    public GameObject eyeLive1, eyeLive2;
    public GameObject eyeDeath1, eyeDeath2;
    public string sceneToPlay;

    private bool PlayAudioDeath = false;
    [HideInInspector] public float currentAmountBar;
    private bool takeDamage, effectDamage;
    private float damageRecived;
    private float count, limitCount = 0.1f;
    private float count2, limitCount2;
    private float countDeath, limitCountDeath = 2f;
    private float countDeath2, limitCountDeath2;
    private int loop = 0;
    private bool regeneratingLife = false;
    private float pointsToRegenerate;

    private void Start() {
        currentAmountBar = healthPoints;
        limitCountDeath2 = limitCountDeath * 2;
        limitCount2 = limitCount;
    }

    private void Update() {
        if (currentAmountBar <= 0) {

            animator.SetBool("IsDead", true);
            effectDamage = false;
            Die();
        }

        if (takeDamage) {
            animator.Play("Player_Hurt");
            //for character without animation
            //effectDamage = true;
            currentAmountBar = Mathf.Clamp(currentAmountBar - damageRecived, 0f, healthPoints);
            bar.transform.localScale = new Vector3(currentAmountBar / healthPoints, 1, 2);
            takeDamage = false;
        }

        if (effectDamage) {
            if (loop < 2) {
                if (count < limitCount) {
                    damageVisualAspect.SetActive(true);
                    count += Time.deltaTime;
                }
                if (count >= limitCount) {
                    damageVisualAspect.SetActive(false);
                    count2 += Time.deltaTime;
                    if (count2 >= limitCount2) {
                        loop++;
                        count = 0f;
                        count2 = 0f;
                    }
                }
            } else {
                loop = 0;
                damageVisualAspect.SetActive(false);
                effectDamage = false;
            }
        }

        if (regeneratingLife && currentAmountBar < healthPoints) {
            currentAmountBar = Mathf.Clamp(currentAmountBar + pointsToRegenerate, 0f, healthPoints);
            bar.transform.localScale = new Vector3(currentAmountBar / healthPoints, 1, 2);
        }
    }

    public void TakeDamage(float damagePoints) {
        takeDamage = true;
        damageRecived = damagePoints;
    }

    public void RegeneratingLife(float points, bool active) {
        pointsToRegenerate = points;
        regeneratingLife = active;
        FindObjectOfType<Audiomanager>().Play("LifeZone");
    }

    private void Die() {
               
        canvasDeath.SetActive(true);
        Manager_Vault.allowMoveEnemies = false;
        Manager_Vault.allowMove = false;
        Manager_Vault.allowDash = false;
        Manager_Vault.allowAttackEnemies = false;
         if (!PlayAudioDeath){
            FindObjectOfType<Audiomanager>().Play("DeathPlayer");
            PlayAudioDeath = true;
        }
        //for character without animation
        //eyeLive1.SetActive(false);
        //eyeLive2.SetActive(false);
        //eyeDeath1.SetActive(true);
        //eyeDeath2.SetActive(true);



        countDeath += Time.deltaTime;
        if (countDeath >= limitCountDeath) {
            
            GameObject effect = Instantiate(deathEffect, playerVisualAspect.transform.position, Quaternion.identity);
            effect.GetComponent<ParticleSystem>().Play();
            playerVisualAspect.SetActive(false);
            shadowVisualAspect.SetActive(false);
            countDeath = -1f;
        }

        countDeath2 += Time.deltaTime;
        if (countDeath2 >= limitCountDeath2) {
            Manager_Vault.Reset();
            SceneManager.LoadScene(sceneToPlay);
        }
    }



    
}
