using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy_Medium : MonoBehaviour{

    public Animator animator;

    private Transform target;

    public GameObject damageVisualAspect;
    public GameObject enemyVisualAspect;
    public GameObject shadowVisualAspect;
    public GameObject[] pointsToSpawnWhenDie;
    public GameObject enemyToSpawn;
    public Room_PlayableArea playerInRoom;
    public GameObject blockAndResetRoom;

    public float healthPoints;
    public GameObject deathEffect;

    public float speed;
    public float radioDetectPlayer;
    public float damagePoints;
    public float timeAttackReload;
    public CircleCollider2D colliderEnemy;

    private bool PlayAudioDeath = false;
    private bool m_FacingRight = true;
    private GameObject player;
    [HideInInspector] public bool move = true, attack = false, injure = false, death = false;
    [HideInInspector] public float currentHealth;
    private float damageRecived;
    private bool feedbackInjure = false;
    private int loop;
    private float countAttack;
    private float count, limitCount = 0.1f;
    private float count2, limitCount2;
    [HideInInspector] public float countDeath, limitCountDeath = 1f;
    [HideInInspector] public CircleCollider2D colliderEnemyTrigger;
    [HideInInspector] public List<GameObject> newEnemy = new List<GameObject>();
    private bool allChildrensDeath = false;
    private List<GameObject> effect = new List<GameObject>();

    private void Start() {
        colliderEnemyTrigger = GetComponent<CircleCollider2D>();
        colliderEnemyTrigger.radius = radioDetectPlayer;
        currentHealth = healthPoints;
        limitCount2 = limitCount;
        player = FindObjectOfType<Player_Move>().gameObject;
        target = this.GetComponent<AIDestinationSetter>().target;
    }

    private void Update() {
        if (death) {
            move = false;
            attack = false;
            injure = false;
            feedbackInjure = false;
            Death();
        }

        if (playerInRoom.playerIsInArea) {
            if (move && Manager_Vault.allowMoveEnemies) {
                /*ansform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                if (transform.position.x < player.transform.position.x && !m_FacingRight){
                    Flip();
                }else if (transform.position.x > player.transform.position.x && m_FacingRight){
                    Flip();
                }*/
                animator.SetFloat("SpeedRun", speed * Time.deltaTime);

            }

            if (attack && Manager_Vault.allowAttackEnemies) {
                animator.Play("NPC_Med_Attack");
                countAttack += Time.deltaTime;
                if (countAttack >= timeAttackReload) {
                    if (transform.position.x < player.transform.position.x && !m_FacingRight){
                        Flip();
                    }else if (transform.position.x > player.transform.position.x && m_FacingRight){
                        Flip();
                    }

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
                injure = false;
                feedbackInjure = false;
            }
            feedbackInjure = true;
            injure = false;
            colliderEnemyTrigger.enabled = false;
        }

        if (feedbackInjure) {
            
            if (loop < 2) {
                if (count < limitCount) {
                    animator.Play("NPC_Med_Hurt");
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
                feedbackInjure = false;
                colliderEnemyTrigger.enabled = true;
            }

        }
    }

    private void Death() {

        animator.Play("NPC_Med_Dying");
        Debug.Log("enemigo muerto");

        if (!PlayAudioDeath) {
            FindObjectOfType<Audiomanager>().Play("DeathEnemyMed");
            PlayAudioDeath = true;
        }

        colliderEnemyTrigger.enabled = false;

        countDeath += Time.deltaTime;
        if (countDeath >= limitCountDeath) {
            GameObject e = Instantiate(deathEffect, enemyVisualAspect.transform.position, Quaternion.identity);
            effect.Add(e);
            e.GetComponent<ParticleSystem>().Play();
            Destroy(e,1.0f);
            enemyVisualAspect.SetActive(false);
            shadowVisualAspect.SetActive(false);
            colliderEnemy.enabled = false;

            
            for (int i = 0; i < pointsToSpawnWhenDie.Length; i++) {
                newEnemy.Add(Instantiate(enemyToSpawn, pointsToSpawnWhenDie[i].transform.position, Quaternion.identity));
                newEnemy[i].GetComponent<Enemy_Small>().playerInRoom = playerInRoom;
                newEnemy[i].GetComponent<Enemy_Small>().blockAndResetRoom = blockAndResetRoom;
                newEnemy[i].GetComponent<AIDestinationSetter>().target = target;
            }
            
            countDeath = -500;
        }

        for (int i = 0; i < newEnemy.Count; i++) {
            if (newEnemy[i].gameObject == null) {
                newEnemy.RemoveAt(i);
            }
            if (newEnemy.Count == 0) {
                allChildrensDeath = true;
            }
        }

        if (allChildrensDeath) {
            for (int i = 0; i < effect.Count; i++) {
                effect.RemoveAt(i);
            }
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
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.GetComponent<Player_Move>() && !Manager_Vault.isDash) {
            move = false;
            attack = true;
            player.GetComponent<Player_Life>().TakeDamage(damagePoints);
            
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.GetComponent<Player_Move>()) {
            
            move = true;
            attack = false;
            countAttack = 0;
        }
    }

    public void TakeDamage(float damagePoints) {
        
        
        injure = true;
        damageRecived = damagePoints;
    }
    private void Flip() {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
