using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    // Start er kallað fyrir fyrstu rammauppfærslu
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Uppfærsla er kölluð einu sinni í hvern ramma
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
