using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Health : MonoBehaviour {

    public float healthPoints;
    public GameObject deathEffect;
    public Room_BlockKillEnemies blockKillEnemies;
    public GameObject visualAspectBoss;
    public GameObject statsBoss;
    public GameObject doorClosed, doorBlock;

    private bool deathComplete = false;

    private void Start() {
        Manager_Vault.healthBossPoints = healthPoints;
        Manager_Vault.currentBossPoints = Manager_Vault.healthBossPoints;
    }

    private void Update() {
        GameObject e = null;
        if (Manager_Vault.deathBoss && !deathComplete) {
            e = Instantiate(deathEffect, transform.position, Quaternion.identity);
            e.GetComponent<ParticleSystem>().Play();
            blockKillEnemies.enemiesToKill.Clear();
            visualAspectBoss.SetActive(false);
            deathComplete = true;
            statsBoss.SetActive(false);
            FindObjectOfType<Audiomanager>().Play("WinSound");
            FindObjectOfType<Audiomanager>().Stop("MenuMusic");
            FindObjectOfType<Audiomanager>().Stop("BossMusic");
            FindObjectOfType<Audiomanager>().Play("GamePlay");
        }

        if (e != null) {
            if (Manager_Vault.deathBoss && e.GetComponent<ParticleSystem>().isStopped) {
                Destroy(e);
                Destroy(gameObject);
            }
        }
    }

}
