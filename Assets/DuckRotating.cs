using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckRotating : MonoBehaviour
{
    public Transform Duck;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Duck.Rotate(0, 0, 10, Space.World);
    }
}
