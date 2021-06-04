using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_RegeneratingLife : MonoBehaviour {

    public float pointsToRegenerate;
    public ParticleSystem effect;

    private GameObject player;
    private bool inside = false;
    private bool desactive = false;

    private void Update() {
        if (!desactive) {
            if (inside) {
                if (player.GetComponent<Player_Life>().currentAmountBar < player.GetComponent<Player_Life>().healthPoints) {
                    player.gameObject.GetComponent<Player_Life>().RegeneratingLife(pointsToRegenerate, inside);
                } else {
                    player.gameObject.GetComponent<Player_Life>().RegeneratingLife(pointsToRegenerate, false);
                    effect.Stop();
                    desactive = true;
                }
            } else {
                player.gameObject.GetComponent<Player_Life>().RegeneratingLife(pointsToRegenerate, inside);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.GetComponent<Player_Life>()) {
            player = col.gameObject;
            inside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.GetComponent<Player_Life>()) {
            player = col.gameObject;
            inside = false;
        }
    }
}
