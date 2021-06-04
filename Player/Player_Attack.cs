using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour {

    public float damagePoints;

    private void OnTriggerEnter2D(Collider2D col){
        if (Manager_Vault.isDash) {
            if (col.gameObject.tag.Equals("Enemy")) {
                if (col.gameObject.GetComponent<Enemy_Small>()) {
                    col.gameObject.GetComponent<Enemy_Small>().TakeDamage(damagePoints);
                }
                if (col.gameObject.GetComponent<Enemy_Medium>()) {
                    col.gameObject.GetComponent<Enemy_Medium>().TakeDamage(damagePoints);
                }
                if (col.gameObject.GetComponent<Boss_FirstStage_Ball>()) {
                    col.gameObject.GetComponent<Boss_FirstStage_Ball>().TakeDamage(damagePoints);
                }
                if (col.gameObject.GetComponent<Boss_SecondStage_Square>()) {
                    col.gameObject.GetComponent<Boss_SecondStage_Square>().TakeDamage(damagePoints);
                }
            }
        }
    }


}
