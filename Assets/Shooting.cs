using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject DuckInHand;
    public Transform Cam;


    public float dashCd;
    public bool readyToThrow;


    Ray ray1;
    RaycastHit hited;
    
    float maxDist = 1000;
    public LayerMask layer;

    public GameObject Object;
    public GameObject KaboomObject;

    private void Start()
    {
         DuckInHand.SetActive(true);
    }
    // Update is called once per frame
    private void Update()
    {
        ray1 = new Ray(transform.position, transform.forward);
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 1000;
        Debug.DrawRay(transform.position, forward, Color.green);
        if (Physics.Raycast(ray1, out RaycastHit hited, maxDist, layer) && Input.GetMouseButton(0) && readyToThrow == true)
        {
            Cam.LookAt(hited.point);
            Shoot();
        }
        else if (Input.GetMouseButton(1) && readyToThrow == true)
        {
            Kaboom();
        }
    }
    void Shoot()
    {   
        readyToThrow = false;

        Instantiate(Object, Cam.position, Cam.rotation);
        dashCd = 0.5f;
        Debug.Log("WorkingShot");
        DuckInHand.SetActive(false);
        Invoke(nameof(ResetShoot), dashCd);
    }
    void Kaboom()
    {   
        readyToThrow = false;
        Instantiate(KaboomObject, Cam.position, Cam.rotation);
        dashCd = 2.5f;
        Debug.Log("WorkingShot");
        DuckInHand.SetActive(false);
        Invoke(nameof(ResetShoot), dashCd);
    }
    void ResetShoot()
    {
        readyToThrow = true;
        DuckInHand.SetActive(true);
    }
}
