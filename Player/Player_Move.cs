using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour {

    public Animator animator;

    private bool m_FacingRight = true;

    public float speed;
    public GameObject model;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveSpeed;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        //if (!Manager_Vault.isDash || !Manager_Vault.changingRoom) { 
        if (Manager_Vault.allowMove) {
            float input=Input.GetAxis("Horizontal");
            //transform.parent = this.transform;
            moveInput = new Vector2(input, Input.GetAxis("Vertical"));
            
            
            if (input > 0 && !m_FacingRight){
                Flip();
            }else if (Input.GetAxis("Horizontal") < 0 && m_FacingRight){
                Flip();
            }


            moveSpeed = moveInput.normalized * speed;
            animator.SetFloat("SpeedRun", Mathf.Abs(input * speed));
            transform.Translate(moveSpeed * Time.fixedDeltaTime);

            if (moveSpeed != Vector2.zero) {
                //Como el padre tienen un movimiento asignado, si se le añade una rotacion, el movimiento no funciona bien.
                //Como el hijo no se mueve (se mueve el padre), se puede rotar sin problemas de que el movimiento no funciona,
                //ya que no tiene movimiento. El padre se mueve y el hijo rota.
                model.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(moveSpeed.x, moveSpeed.y, 0f));
            }
        }
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
