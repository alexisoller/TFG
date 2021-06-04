using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ALL : MonoBehaviour {

    [Header("GENERAL:")]
    public GameObject player;
    public GameObject damageVisualAspect;
    public float lifePoints;
    public GameObject dieEffect;

    [Header("MOVEMENT:")]
    public bool quiet;
    [Space(10)]
    public bool moveToPlayer;
    [Space(10)]
    public bool moveRandom;
    public float numberOfPositionsToQuiet;
    [Space(10)]
    public float speed;

    [Header("ATTACK:")]
    public bool harmless;
    [Space(10)]
    public bool attackWhenTouch;
    [Space(10)]
    public bool attackArea;
    public float radioArea;
    [Space(10)]
    public bool attackProyectiles;
    public GameObject proyectile;
    public bool objectivePlayer;
    public GameObject[] pointsToShoot;
    /*[Space(10)]
    public bool attackGrenades;
    public GameObject grenade;
    public float radioGrenade;
    public float areaEffect;
    public bool isRandomlyInArea;
    public int amountOfThrowInArea;
    public bool isToPlayer;*/
    [Space(10)]
    public float damagePoints;
    public float timeToReload;


    [Header("SKILL:")]
    public bool skillfill;
    [Space(10)]
    public bool skillInvisible;
    [Space(10)]
    public bool skillShield;
    public float healthShield;
    [Space(10)]
    public bool skillInvoque;
    public GameObject enemyToInvoque;
    public GameObject[] pointsToInvoque;

    private void Update() {
        //MOVIMIENTO
        if (!quiet && moveToPlayer && !moveRandom) { // Se mueve hacia el jugador
            //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        } else if(!quiet && !moveToPlayer && moveRandom) { //Se mueve de forma aleatoria


        }

    }

    private void OnTriggerEnter2D(Collider2D col) {
        
    }

}
