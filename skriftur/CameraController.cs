using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    // Start er kalla� fyrir fyrstu rammauppf�rslu
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Uppf�rsla er k�llu� einu sinni � hvern ramma
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
