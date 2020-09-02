using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


/* 
TODO:
X On click record & display position of mousePos and worldPosition
X On click record & display distance from center
X On click record & display angle difference from north
X Organize Canvas objects to be in correct positions
X Add a white background to Canvas
X Add Game Objects overlayed on Stimulus to each object's center displayed in Stimulus, name them appropriately
X Write function that takes three objects (standingAt, lookingAt, pointingAt):
        X calculates degrees of difference b/w standingAt::PointingAt and standingAt::lookingAt
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
public class DataManager : MonoBehaviour
{
    private GameObject compass;
    public Text text_startAt;
    public Text text_lookAt;
    public Text text_pointAt;

    public string standingAt;
    public string lookingAt;
    public string pointingAt;
    public float correctAngle;

    // Start is called before the first frame update
    void Start()
    {
        compass = GameObject.Find("Compass");

        standingAt = "Barrel";
        lookingAt = "Wheel";
        pointingAt = "TrashCan";

        text_startAt.text = "Stand at: " + standingAt;
        text_lookAt.text = "Look at: " + lookingAt;
        text_pointAt.text = "Point at: " + pointingAt;


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
            recordData();

    }

    void recordData()
    {


        correctAngle = GetCorrectAngle();

        Vector3 alignMousePos = new Vector3(FaceMouse.mousePos.x, FaceMouse.mousePos.y, compass.transform.position.z);
        float distance = Vector3.Distance(alignMousePos, compass.transform.position);
        
        Debug.Log("StandAt: " + standingAt + "\tLookAt: " + lookingAt + "\tPointAt: " + pointingAt
            + "\tRotation: " + (360 - compass.transform.eulerAngles.z)
            + "\tCorrect Angle: " + (360 - correctAngle)
            + "\tAnglular Error: " + Mathf.DeltaAngle((360 - correctAngle), (360 - compass.transform.eulerAngles.z))
            /*            + "\tMouse Pos: " + FaceMouse.mousePos
                        + "\tWorld Pos: " + Input.mousePosition
                        + "\tAligned Mouse Pos: " + alignMousePos*/
            + "\tDir: " + FaceMouse.direction
            + "\tDist: " + distance
            );

    }

    public float GetCorrectAngle()
    {
        Vector3 A = GameObject.Find(lookingAt).transform.position;
        Vector3 B = GameObject.Find(standingAt).transform.position;
        Vector3 C = GameObject.Find(pointingAt).transform.position;

        float corrAngle = Vector2.Angle(A - B, C - B);
        return corrAngle;
        
    }


}
