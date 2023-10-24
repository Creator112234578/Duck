using UnityEngine;

public class Kaboom : MonoBehaviour
{
    public Rigidbody RigidBody;
    public SphereCollider SC;
    public float Bullet;
    public float Mass;
    void Start()
    {   
        SC = GetComponent<SphereCollider>();
        RigidBody.mass = Mass;
    }
    void Update()
    {
        RigidBody.AddForce(transform.forward * Bullet, ForceMode.Impulse);
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
            Debug.Log("Kaboom");
            Invoke(nameof(DestroyingObject), 0.0125f);
        }
    }


}
