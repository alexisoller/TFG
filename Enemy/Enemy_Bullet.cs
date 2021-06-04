using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour {

    public float damagePoints;
    public float speed;

    private Rigidbody2D rb;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.GetComponent<Player_Move>()) {
            if (!Manager_Vault.isDash) {
                col.GetComponent<Player_Life>().TakeDamage(damagePoints);
                Destroy(gameObject);
            }
        }
        if (col.gameObject.name == "Limits") {
            print("Bala destruida");
            Destroy(gameObject);
        }
    }

}
