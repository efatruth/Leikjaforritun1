using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;     // A reference to our PlayerMovement script// Tilvísun í PlayerMovement handritið okkar

    // This function runs when we hit another object.
    // Þessi aðgerð keyrir þegar við hittum annan hlut.
    // We get information about the collision and call it "collisionInfo".
    // Við fáum upplýsingar um áreksturinn og köllum hann „collisionInfo“.
    void OnCollisionEnter (Collision collisionInfo) 
    {
        // We check if the object we collided with has a tag called "Obstacle"
        // Við athugum hvort hluturinn sem við lentum í sé með merki sem heitir "Hindrun"
        if (collisionInfo.collider.tag == "Obstacle") 
        {
            movement.enabled = false; // Disable the players movement// Slökktu á hreyfingu leikmannsins
            //FindObjectOfType<GameManager>().EndGame();
        }
    }

}
