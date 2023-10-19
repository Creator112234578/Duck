using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaboom : MonoBehaviour
{
    public Rigidbody rigidbody;
    public SphereCollider SC;
    public float Bullet;
    public float Mass;
    void Start()
    {   
        SC = GetComponent<SphereCollider>();
        rigidbody.mass = Mass;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.AddForce(transform.forward * Bullet, ForceMode.Impulse);
    }
    void DestroyingObject()
    {
        Destroy(this.gameObject);
    }


    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 6)
        {   
            SC.radius = 7.5f;
            Invoke(nameof(DestroyingObject), 0.0125f);
            Debug.Log("Kaboom");
        }
    }


}
