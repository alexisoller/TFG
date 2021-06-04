using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Waves : MonoBehaviour {

    [HideInInspector] public GameObject wavePointFinal;
    public float speed;
    public GameObject padre;
    public float damagePoints;

    private float count, limitCount = 6f;

    private void Update() {
        if (wavePointFinal) {
            transform.position = Vector3.MoveTowards(transform.position, wavePointFinal.transform.position, speed * Time.deltaTime);
            count += Time.deltaTime;
            if (count >= limitCount){
                Destroy(padre);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.GetComponent<Player_Move>()) {
            col.gameObject.GetComponent<Player_Life>().TakeDamage(damagePoints);
        }
    }

}
