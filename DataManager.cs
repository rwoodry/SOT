using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
        X Store trial data: trialnum, standingAt, lookingAt, pointingAt, trialStartTime, trialEndTime, 
            trialReactionTime, mousePos, worldPosition, lineLength, correctAngle, obtainedAngle, errorAngleDegrees, errorAnglePercent
X Add Correct Angle Display
X Add Launcher Menu
X Add Completion Scene
- Add Feedback in between trials with 
    X on first click record data and show intermediary text
    X data as a string with error in degrees and percent
    X showing input angle vs. correct angle
    - on second click do next trial
X Add Mturk Worker ID Query
X Add Time Limit functionality
- Add Generate random token ID
- Add PHP integration

*/
public class DataManager : MonoBehaviour
{
    private GameObject compass;
    private GameObject corrCompass, inputCompass;
    private GameObject corrLine, inputLine;
    public float correctAngle;
    public TrialManager tm;
    public static bool SOTStart = false;
    private Vector3 alignMousePos;
    private float inputAngle;
    public string FILE_NAME;
    public string trialData = "";
    public Text textError;
    private float angularError, pctAngular180Error;
    // Start is called before the first frame update
    void Start()
    {
        FILE_NAME = LaunchManager.workerID + ".csv";
        compass = GameObject.Find("Compass");
        corrCompass = GameObject.Find("CorrectCircle");
        inputCompass = GameObject.Find("InputCircle");
        corrLine = GameObject.Find("CorrectLine");
        inputLine = GameObject.Find("InputLine");

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
            recordData();

    }

    public void recordData()
    {
        trialData = GetDataString();

        WriteToFile();

        DisplayCorrectAngle();

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

        float inAngle = Vector2.SignedAngle(A - B, C - B);
        return inAngle;
    }

    void DisplayCorrectAngle()
    {
        corrCompass.transform.eulerAngles = new Vector3(0, 0, 0);
        inputCompass.transform.eulerAngles = new Vector3(0, 0, 0);
        corrCompass.transform.Rotate(0, 0, correctAngle);
        inputCompass.transform.Rotate(0, 0, inputAngle);
        corrLine.GetComponent<MeshRenderer>().enabled = true;
        inputLine.GetComponent<MeshRenderer>().enabled = true;
        inputCompass.transform.up = FaceMouse.direction;
        textError.text = "Input Angle: " + Mathf.Abs(inputAngle).ToString("F2") + "\tCorrect Angle: " + Mathf.Abs(correctAngle).ToString("F2") 
            + "\nAngular Error: " + Mathf.Abs(angularError).ToString("F2") + " (" + (pctAngular180Error*100).ToString("F2") + "% error)";
    }

    public string GetDataString()
    {
        alignMousePos = new Vector3(FaceMouse.mousePos.x, FaceMouse.mousePos.y, compass.transform.position.z);

        inputAngle = GetInputAngle();
        correctAngle = GetCorrectAngle();

        float distance = Vector3.Distance(alignMousePos, compass.transform.position);
        angularError = Mathf.DeltaAngle((inputAngle), (correctAngle));
        pctAngular180Error = Mathf.Abs(angularError / 180);
        float inputAngleEulerClockwise = (360 - compass.transform.eulerAngles.z);
        float trialEnd = Time.time;
        float trialReactionTime = trialEnd - TrialManager.trialStart;

        Debug.Log(" IA: " + inputAngle + " CA: " + correctAngle + " AE: " + angularError + " TS: " + TrialManager.trialStart + " TE: " + trialEnd + " TR: " + trialReactionTime);

        string tdata = TrialManager.trialnum.ToString() + ", "
            + tm.SA.name + ", "
            + tm.LA.name + ", "
            + tm.PA.name + ", "
            + alignMousePos.ToString() + ", "
            + distance.ToString() + ", "
            + FaceMouse.direction.ToString() + ", "
            + inputAngle.ToString() + ", "
            + correctAngle.ToString() + ", "
            + angularError.ToString() + ", "
            + pctAngular180Error.ToString() + ", "
            + inputAngleEulerClockwise.ToString() + ", "
            + TrialManager.trialStart.ToString() + ", "
            + trialEnd.ToString() + ", "
            + trialReactionTime.ToString()
            ;

        return tdata;
    }

    void WriteToFile()
    {
        StreamWriter sw = File.AppendText(FILE_NAME);
        if (new FileInfo(FILE_NAME).Length == 0)
        {
            sw.WriteLine("trialNum, standAt, lookAt, pointAt, mousePos, mouseDist, mouseDir, responseAngle, correctAngle, angularError, pctAngularError, responseAngleEuler, startTime, endTime, reactionTime");
        }
        sw.WriteLine(trialData);
        sw.Close();
    }
}
