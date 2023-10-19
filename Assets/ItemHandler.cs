using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public bool pressed;
    public bool grounded;
    public LayerMask GroundUNow;
    public Transform Item;
    public Transform Player;
    RaycastHit hit;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	grounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5f, GroundUNow);
        if (grounded == true && Input.GetMouseButtonDown(1))
	    pressed = true;
	if (grounded == true && Input.GetMouseButtonDown(2))
	    pressed = true;
	if (Input.GetKeyDown("1"))
	    pressed = false;
	if (pressed == true)    
	    Item.transform.position = Player.transform.position;
	    Item.transform.rotation = Player.transform.rotation;
	if (pressed == false) 
	    Item.transform.position = Item.transform.position;
	    Item.transform.rotation = Item.transform.rotation;
    }
}
