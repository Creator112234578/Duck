using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public Rigidbody rb;
    public SphereCollider SC;
    public float Speed;
    public float Mass;
    void Start()
    {
        rb.mass = Mass;
    }
    void Update()
    {
        rb.AddForce(transform.forward * Speed, ForceMode.Impulse);
    }
    void DestroyingObject()
    {
        Destroy(this.gameObject);
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 6)
        {
            DestroyingObject();
            Debug.Log("Hited");
        }
    }
}
