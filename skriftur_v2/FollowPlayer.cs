using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;


    // Update is called once per frame
    // Uppf�rsla er k�llu� einu sinni � hvern ramma
    void Update ()
    {
        transform.position = player.position + offset;
    }
}
