using System;
using System.Collections;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

/* 
TODO:
X On click record & display position of mousePos and worldPosition
X On click record & display distance from center
X On click record & display angle difference from north
X Set max length of linerenderer to radius of Button_White
- Fix position y bug of objects/canvas (Canvas y = 1, causing true origin of circle to not be 0)
- Organize Canvas objects to be in correct positions
- Add a white background to Canvas
- Add Game Objects overlayed on Stimulus to each object's center displayed in Stimulus, name them appropriately
- Write function that takes three objects (standingAt, lookingAt, pointingAt):
        - sets lookingAt to north
        - calculates degrees of difference b/w standingAt::PointingAt and standingAt::lookingAt
- Create Trial handler that initializes a list of trials (whether random or pre-written)
        - Cycles through each one, resetting each time
        - Store trial data: trialnum, standingAt, lookingAt, pointingAt, trialStartTime, trialEndTime, 
            trialReactionTime, mousePos, worldPosition, lineLength, correctAngle, obtainedAngle, errorAngleDegrees, errorAnglePercent
- Add Launcher Menu
- Add Mturk Worker ID Query
- Add Time Limit functionality
- Add Generate random token ID
- Add PHP integration

*/
public class CursorHandler : MonoBehaviour
{
    private Vector3 worldPosition;
    private LineRenderer _lineRenderer;
    private Vector3 _initialPosition;
    private Vector3 _currentPosition;
    private Vector3 circleCenter;

    // Start is called before the first frame update
    void Start()
    {
        // Find Circle button game objects
        
        // Enable visibility for the user's mouse
        Cursor.visible = true;

        // Initialize a line renderer object to draw the line from center of circle to mouse pointer
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.SetWidth(1f, 1f);
        _lineRenderer.enabled = true;

        // Find the center of the Circle object
        circleCenter = GameObject.Find("Button_White").transform.position;

        // Set the first position (0) of the line renderer object to the center of the circle. Set the number of vertices on the line to 2 (straight line)
        _lineRenderer.SetPosition(0, circleCenter);
        _lineRenderer.SetVertexCount(2);


    }

    // Update is called once per frame
    void Update()
    {
        // Store the mouse position and change the Z value to match the circle object's Z, this way the two endpoints of the line lie on the same plane (Z).
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = circleCenter.z;

        // Take the edited mouse position and convert it's coordinates to world coordinates
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 northpoint = new Vector3(circleCenter.x, 10f, circleCenter.z);

        float angle = -Vector2.SignedAngle(northpoint - circleCenter, worldPosition - circleCenter);
        angle = CalculateAngle(angle);
        float radians = angle * Mathf.Deg2Rad;
        float y = 6 * Mathf.Cos(radians) + 1;
        float x = 6 * Mathf.Sin(radians);
        Vector3 line = new Vector3(x, y, worldPosition.z);

        // Set the second position (1) to the new World Coordinates of the mouse pointer.
        _lineRenderer.SetPosition(1, line);
    }

    // Function that handles all operations when Circles are clicked
    public void ClickCircle()
    {
        // Calculate the reported angle by getting the difference between the north vector and the mouse click vector
        Vector3 northpoint = new Vector3(circleCenter.x, 10f, circleCenter.z);
        float angle = -Vector2.SignedAngle(northpoint - circleCenter, worldPosition - circleCenter);
        angle = CalculateAngle(angle);

        // Calculate length of line, distance from circle center to clicked area
        float dist = Vector2.Distance(worldPosition, circleCenter);

        //Calculate point on circle border
        float radians = angle * Mathf.Deg2Rad;
        float y = 0 * Mathf.Cos(radians);
        float x = 0 * Mathf.Sin(radians);
        

        // Print values for debugging
        print(
            "Mouse Pos: " + worldPosition +  
            "\tLine Length:  " + dist +
            "\tDegree Reported: " + angle 
            );
        
    }

    // Function for converting the signed angle to 360 degrees clockwise
    public static float CalculateAngle(float angle)
    {

        if (angle < 0)              // If angle is negative (i.e on 'left' hemisphere of circle)
            angle = 360 + angle;
        else
        {
            angle = angle;  
        }
        return angle;

    }


}
