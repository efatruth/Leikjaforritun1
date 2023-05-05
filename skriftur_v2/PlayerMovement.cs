using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // This is a reference to the Rigidbody component called "rb"
    // �etta er tilv�sun � Rigidbody hluti sem kallast "rb"
    public Rigidbody rb;

    public float forwardForce = 2000f;
    public float sidewaysForce = 500f;

    // We marked this as "Fixed"Update because we
    // Vi� merktum �etta sem "Fast"Uppf�rslu vegna �ess a� vi�
    // are it to mess with physics.
    // er �a� a� skipta s�r af e�lisfr��i.
    void FixedUpdate ()
    {
        // Add a forward force
        // B�ttu vi� framstyrk
        rb.AddForce(0, 0, forwardForce = 2000 * Time.deltaTime);

        if ( Input.GetKey("d")) // If the player is pressing the "d" key//Ef spilarinn er a� �ta � "d" takkann
        {
            // Add a force to the right
            // B�ttu vi� krafti til h�gri
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (Input.GetKey("a")) // If the player is pressing the "a" key// Ef spilarinn er a� �ta � "a" takkann
        {
            // Add a force to the left 
            // B�ttu vi� krafti til vinstri
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        
        //if (rb.position.y < -1f)
        //{
            //FindObjectOfType<GameManager>().EndGame();
        //}
    }
}
