using UnityEngine;

public class CamInPlayer : MonoBehaviour
{
    public Transform player;
    public Transform cam;

    // Update is called once per frame
    void Update()
    {
        cam.position = player.position;   
    }
}
