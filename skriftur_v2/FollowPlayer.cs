using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;


    // Update is called once per frame
    // Uppfærsla er kölluð einu sinni í hvern ramma
    void Update ()
    {
        transform.position = player.position + offset;
    }
}
