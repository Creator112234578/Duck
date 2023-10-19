using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float XRotation;
    public float YRotation;
    public LayerMask GroundUNow;
    public bool grounded;
    public Transform Item;
    public Transform Player;
    RaycastHit hit;
    /*public float curentInventoryRot;
    public Transform Pocket1;
    public Transform Pocket2;
    public float Smooth;*/

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        /*if (Input.GetKey(KeyCode.E))
        {
            YRotation = Mathf.Lerp(transform.rotation.y, curentInventoryRot, 0.125f);
        }*/

    }
    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        YRotation += mouseX;
        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90f, 90f);
        transform.rotation = Quaternion.Euler(XRotation, YRotation, 0);
        orientation.rotation = Quaternion.Euler(0, YRotation, 0);

	
	
	
	
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            curentInventoryRot = YRotation;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.rotation = Quaternion.Euler(70, YRotation, 0);
            if (YRotation > curentInventoryRot + 60)
            {
                transform.rotation = Quaternion.Euler(70, curentInventoryRot + 60, 0);
            }
            if (YRotation < curentInventoryRot - 60)
            {
                transform.rotation = Quaternion.Euler(70, curentInventoryRot - 60, 0);
            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            transform.rotation = Quaternion.Euler(XRotation, YRotation, 0);
        }
*/


    }
}
