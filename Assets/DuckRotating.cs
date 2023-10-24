using UnityEngine;

public class DuckRotating : MonoBehaviour
{
    public Transform Duck;
    void Update()
    {
        Duck.Rotate(0, 0, 10, Space.World);
    }
}
