using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_DashableArea : MonoBehaviour {

    public Vector2 size;

    private Room_PlayableArea playableArea;
    private Collider2D[] overlapBox;

    private void Start() {
        playableArea = GetComponentInParent<Room_PlayableArea>();
    }

    private void Update() {
        //OverlapBoxAll sirve para comprobar todos los Colliders que entren dentro de los parametros especificados.
        //OverlapBoxAll se compone por:
        //      - Vector2 Point: es el centro
        //      - Vector2 Size: es el tamaño de la caja
        //      - floar Angle: el angulo de rotacion que quieres que tenga la caja
        //      - layerMask layer: es por si quieres comporbar solo en una layer en concreto.
        overlapBox = Physics2D.OverlapBoxAll(transform.position, new Vector2(size.x, size.y), 0f);
        for (int i = 0; i < overlapBox.Length; i++) {
            if (overlapBox[i].gameObject.name == "DashPoint") {
                if (playableArea.playerIsInArea) {
                    Manager_Vault.allowDash = true;
                    break;
                }
            } else {
                Manager_Vault.allowDash = false;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0f));
    }

}
