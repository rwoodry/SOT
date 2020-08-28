using System.Collections;
using UnityEngine;

/* 
TODO:
- On click record & display position of mousePos and worldPosition
- On click record & display distance from center
- On click record & display angle difference from north
- Set max length of linerenderer to radius of Button_White
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
  

        // Set the second position (1) to the new World Coordinates of the mouse pointer.
        _lineRenderer.SetPosition(1, worldPosition);
    }


}
