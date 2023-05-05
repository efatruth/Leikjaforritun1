using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;     // A reference to our PlayerMovement script// Tilv�sun � PlayerMovement handriti� okkar

    // This function runs when we hit another object.
    // �essi a�ger� keyrir �egar vi� hittum annan hlut.
    // We get information about the collision and call it "collisionInfo".
    // Vi� f�um uppl�singar um �reksturinn og k�llum hann �collisionInfo�.
    void OnCollisionEnter (Collision collisionInfo) 
    {
        // We check if the object we collided with has a tag called "Obstacle"
        // Vi� athugum hvort hluturinn sem vi� lentum � s� me� merki sem heitir "Hindrun"
        if (collisionInfo.collider.tag == "Obstacle") 
        {
            movement.enabled = false; // Disable the players movement// Sl�kktu � hreyfingu leikmannsins
            //FindObjectOfType<GameManager>().EndGame();
        }
    }

}
