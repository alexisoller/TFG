using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player_Dash : MonoBehaviour {

    

    public GameObject dashOrigin, dashPoint;
    public float speed;
    public Image bar;
    public float numOfConsecutiveDashes;
    public GameObject dashEffect;

    private float currentAmountDash;
    private float maxAmountBar = 100f;
    private float costDash;
    private GameObject currentDasEffect;
    private List<GameObject> restDasEffects;
    private float count, limitCount = 0.3f;
    private Collider2D colliderPlayer;

    private void Start() {
        //"numOfConsecutiveDashes" no puede ser mayor a "maxAmountBar."
        costDash = maxAmountBar / numOfConsecutiveDashes;
        currentAmountDash = maxAmountBar;
        restDasEffects = new List<GameObject>();
        colliderPlayer = GetComponent<CircleCollider2D>();
    }

    private void Update() {
        if (Manager_Vault.allowDash) {
            count += Time.deltaTime;
            if (Input.GetButtonDown("Fire1") && !Manager_Vault.isDash && currentAmountDash >= costDash ) { //Joystick Button 0 = A/X
                FindObjectOfType<Audiomanager>().Play("DashSound");
                Manager_Vault.isDash = true;
                Manager_Vault.allowMove = false;
                colliderPlayer.enabled = false;
                //Init Effect
                currentDasEffect = Instantiate(dashEffect, transform.position, Quaternion.identity);
                restDasEffects.Add(currentDasEffect);
                currentDasEffect.GetComponent<ParticleSystem>().Play();
                //End Effect
                UseDash();
            }

            if (Manager_Vault.isDash) {
                transform.position = Vector2.MoveTowards(transform.position, dashPoint.transform.position, speed * Time.deltaTime);
            }

            if (transform.position == dashPoint.transform.position) {
                colliderPlayer.enabled = true;
                count = 0f;
                Manager_Vault.isDash = false;
                Manager_Vault.allowMove = true;
            }
            Debug.Log("currentDasEffect" + currentDasEffect);
            /*if (currentDasEffect != null && currentDasEffect.GetComponent<ParticleSystem>().isStopped) {
                for (int i = 0; i < restDasEffects.Count; i++) {
                    Destroy(restDasEffects[i].gameObject);
                }
            }*/
            for (int i = 0; i < restDasEffects.Count; i++)
            {
                Destroy(restDasEffects[i].gameObject,1.0f);
            }

        }
    }

    public void UseDash() {
        currentAmountDash = Mathf.Clamp(currentAmountDash - costDash, 0f, maxAmountBar);
        bar.transform.localScale = new Vector3(currentAmountDash/maxAmountBar, 1, 2);
    }

    private void FixedUpdate() {
        if (!Manager_Vault.isDash) {
            dashPoint.transform.position = dashOrigin.transform.position;
            if (currentAmountDash < maxAmountBar && maxAmountBar > 0) {
                currentAmountDash = Mathf.Clamp(currentAmountDash + Manager_Vault.amountReloadDash, 0f, maxAmountBar);
                bar.transform.localScale = new Vector3(currentAmountDash / maxAmountBar, 1, 2);
            }
        }
    }
}
