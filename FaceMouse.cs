using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

// This Script is attached to the compass object. This handles the rotation of the compass to face the user's mouse.
public class FaceMouse : MonoBehaviour
{
    
    public static Vector3 mousePos;
    public static Vector2 direction;
    public bool lineFollow = false;
    // Start is called before the first frame update
    void Start()
    {
        // Make the user's mouse visible
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        // At each update, face the compass toward's the mouse
        faceMouse();
    }

    void faceMouse()
    {
        // Convert mouse screen position to world position
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate direction vector by subtracting the compass position from mouse position
        direction = new Vector2(
            mousePos.x - transform.position.x,
            mousePos.y - transform.position.y
        );

        // Rotate the compass' transform up (north vector) to the new direction
        if (lineFollow) { transform.up = direction; }
        

    }
}
