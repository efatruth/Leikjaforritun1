using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // This is a reference to the Rigidbody component called "rb"
    // Þetta er tilvísun í Rigidbody hluti sem kallast "rb"
    public Rigidbody rb;

    public float forwardForce = 2000f;
    public float sidewaysForce = 500f;

    // We marked this as "Fixed"Update because we
    // Við merktum þetta sem "Fast"Uppfærslu vegna þess að við
    // are it to mess with physics.
    // er það að skipta sér af eðlisfræði.
    void FixedUpdate ()
    {
        // Add a forward force
        // Bættu við framstyrk
        rb.AddForce(0, 0, forwardForce = 2000 * Time.deltaTime);

        if ( Input.GetKey("d")) // If the player is pressing the "d" key//Ef spilarinn er að ýta á "d" takkann
        {
            // Add a force to the right
            // Bættu við krafti til hægri
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (Input.GetKey("a")) // If the player is pressing the "a" key// Ef spilarinn er að ýta á "a" takkann
        {
            // Add a force to the left 
            // Bættu við krafti til vinstri
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        
        //if (rb.position.y < -1f)
        //{
            //FindObjectOfType<GameManager>().EndGame();
        //}
    }
}
