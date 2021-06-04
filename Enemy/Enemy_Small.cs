using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Small : MonoBehaviour {

    Vector3 newPosition;


    public Animator animator;
    
    public bool Audio=false;
    public GameObject damageVisualAspect;
    public GameObject enemyVisualAspect;
    public GameObject shadowVisualAspect;
    public Room_PlayableArea playerInRoom;
    public GameObject blockAndResetRoom;

    public float healthPoints;
    public GameObject deathEffect;

    public float speed;
    public float radioDetectPlayer;
    public float damagePoints;
    public float timeAttackReload;
    public CircleCollider2D colliderEnemy;

    
    private bool m_FacingRight = true;
    private bool PlayAudioDeath = false;
    private GameObject player;
    private bool move = true, attack = false, injure = false, death = false;
    [HideInInspector] public float currentHealth;
    private float damageRecived;
    private bool feedbackInjure = false;
    private int loop;
    private float countAttack;
    private float count, limitCount = 0.1f;
    private float count2, limitCount2;
    private float countDeath, limitCountDeath = 1f;
    private float countDeath2, limitCountDeath2;
    private CircleCollider2D colliderEnemyTrigger;
    private GameObject effect;

    private void Start() {
        colliderEnemyTrigger = GetComponent<CircleCollider2D>();
        colliderEnemyTrigger.radius = radioDetectPlayer;
        currentHealth = healthPoints;
        limitCount2 = limitCount;
        limitCountDeath2 = limitCountDeath * 2;
        player = FindObjectOfType<Player_Move>().gameObject;
    }

    private void Update() {
        if (death) {
            transform.position = newPosition;
            move = false;
            attack = false;
            injure = false;
            feedbackInjure = false;
            Death();
            
        }
        
        if (playerInRoom.playerIsInArea) {
            if (move && Manager_Vault.allowMoveEnemies) {

                if (SceneManager.GetActiveScene().name == "Tutorial") 
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    if (transform.position.x < player.transform.position.x && !m_FacingRight)
                    {
                        Flip();
                    }
                    else if (transform.position.x > player.transform.position.x && m_FacingRight)
                    {
                        Flip();
                    }
                }

                animator.SetFloat("SpeedRun", speed * Time.deltaTime);
            }

            if (attack && Manager_Vault.allowAttackEnemies) {
                countAttack += Time.deltaTime;
                if (countAttack >= timeAttackReload) {
                    if (!Manager_Vault.isDash) {
                        player.GetComponent<Player_Life>().TakeDamage(damagePoints);
                    }
                    countAttack = 0;
                }
            }
        }

        if (injure) {
            currentHealth -= damageRecived;
            if (currentHealth <= 0) {
                death = true;
                newPosition = new Vector3(enemyVisualAspect.transform.position.x, enemyVisualAspect.transform.position.y, enemyVisualAspect.transform.position.z);
                injure = false;
                feedbackInjure = false;
            }
            feedbackInjure = true;
            colliderEnemyTrigger.enabled = false;
            injure = false;
        }

        if (feedbackInjure) {
            if (loop < 2) {
                if (count < limitCount) {
                    //damageVisualAspect.SetActive(true);
                    count += Time.deltaTime;
                }
                if (count >= limitCount) {
                    //damageVisualAspect.SetActive(false);
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
                feedbackInjure = false;
                colliderEnemyTrigger.enabled = true;
            }
        }
    }

    private void Death() {
       
        colliderEnemyTrigger.enabled = false;
        if (!PlayAudioDeath){
            FindObjectOfType<Audiomanager>().Play("DeathEnemySmall");
            animator.Play("NPC_Small_Dying");
            PlayAudioDeath = true;
        }
        countDeath += Time.deltaTime;
        if (countDeath >= limitCountDeath) {
            effect = Instantiate(deathEffect, enemyVisualAspect.transform.position, Quaternion.identity);
            effect.GetComponent<ParticleSystem>().Play();
            enemyVisualAspect.SetActive(false);
            shadowVisualAspect.SetActive(false);
            colliderEnemy.enabled = false;
            for (int i = 0; i < blockAndResetRoom.GetComponent<Room_BlockKillEnemies>().enemiesToKill.Count; i++) {
                if (blockAndResetRoom.GetComponent<Room_BlockKillEnemies>().enemiesToKill[i].gameObject == gameObject) {
                    blockAndResetRoom.GetComponent<Room_BlockKillEnemies>().enemiesToKill.RemoveAt(i);
                }
            }
            for (int i = 0; i < blockAndResetRoom.GetComponent<Room_ResetRoom>().reset.Count; i++) {
                if (blockAndResetRoom.GetComponent<Room_ResetRoom>().reset[i].enemy.gameObject == gameObject) {
                    blockAndResetRoom.GetComponent<Room_ResetRoom>().reset.RemoveAt(i);
                }
            }
            countDeath = -1;
        }

        countDeath2 += Time.deltaTime;
        if (countDeath2 >= limitCountDeath2) {
            if (effect != null) {
                Destroy(effect);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.GetComponent<Player_Move>() && !Manager_Vault.isDash && !death) {
            move = false;
            attack = true;
            player.GetComponent<Player_Life>().TakeDamage(damagePoints);
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.GetComponent<Player_Move>() && !death) {
            move = true;
            attack = false;
            countAttack = 0;
        }
    }

    public void TakeDamage(float damagePoints) {
        if (!death) {
            injure = true;
            damageRecived = damagePoints;
        }
       
    }

   
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
