using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamInPlayer : MonoBehaviour
{
    public Transform player;
    public Transform cam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cam.position = player.position;   
    }
}
