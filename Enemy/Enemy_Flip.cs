using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy_Flip : MonoBehaviour
{
    // Start is called before the first frame update
    public AIPath aiPath;

    private bool m_FacingRight = true;

    // Update is called once per frame
    void Update(){
        if (aiPath.desiredVelocity.x >= 0.01f) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        } else if (aiPath.desiredVelocity.x <= -0.01f) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    
    
        


    }
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
