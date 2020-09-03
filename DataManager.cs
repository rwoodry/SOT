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
X Create Trial handler that initializes a list of trials (whether random or pre-written)
        X Cycles through each one, resetting each time
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
    public float correctAngle;
    public TrialManager tm;
    public static bool SOTStart = false;
    private Vector3 alignMousePos;
    private float inputAngle;

    // Start is called before the first frame update
    void Start()
    {
        compass = GameObject.Find("Compass");

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
            recordData();

    }

    public void recordData()
    {

        alignMousePos = new Vector3(FaceMouse.mousePos.x, FaceMouse.mousePos.y, compass.transform.position.z);
        float distance = Vector3.Distance(alignMousePos, compass.transform.position);

        correctAngle = GetCorrectAngle();
        inputAngle = GetInputAngle();

        Debug.Log("StandAt: " + tm.SA.name + "\tLookAt: " + tm.LA.name + "\tPointAt: " + tm.PA.name
            + "\tRotation: " + (360 - compass.transform.eulerAngles.z)
            + "\tCorrect Angle: " + (correctAngle)
            + "\tAnglular Error: " + Mathf.DeltaAngle((inputAngle), (correctAngle))
            /*            + "\tMouse Pos: " + FaceMouse.mousePos
                        + "\tWorld Pos: " + Input.mousePosition
                        + "\tAligned Mouse Pos: " + alignMousePos*/
/*            + "\tDir: " + FaceMouse.direction
            + "\tDist: " + distance*/
            + "\tAlternate Angle: " + inputAngle
            );

        tm.NextTrial();

    }

    public float GetCorrectAngle()
    {
        Vector3 A = tm.LA.transform.position;
        Vector3 B = tm.SA.transform.position;
        Vector3 C = tm.PA.transform.position;

        float corrAngle = Vector2.SignedAngle(A - B, C - B);
        return corrAngle;
        
    }

    public float GetInputAngle()
    {
        Vector3 A = transform.position;
        Vector3 B = compass.transform.position;
        Vector3 C = alignMousePos;
        Debug.Log("Compass " + B + "Norht" + A + "MOUSE " + C);
        float inAngle = Vector2.SignedAngle(A - B, C - B);
        return inAngle;
    }

}
