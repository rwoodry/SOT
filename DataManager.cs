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
X Add Feedback in between trials with 
    X on first click record data and show intermediary texts
    X showing input angle vs. correct angle
    X on ENTER do next trial
    X add secondary trialTime related to Enter press
X Add Mturk Worker ID Query
X Add Time Limit functionality
X Add intructions trial text
X Add subject column
X Add North compass line
X Add stand at | look at text on compass
X Fix object names
X Adjust stim and circle size
X Add vertical line separator
X Add instructions slide
X Add instructions section
X Add practice slide
X Add practice section (3, show error)
X Edit feedback to disapear when next trial is loaded
X Add test slide
X Add test section (no feedback)
X Add proper time limit to TEST trials
X Add trial type column to data
X Add the proper twelve item orders to arrays
X Edit instructions trial to be displaying correct angle
X Add Generate random token ID
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
    private float angularError, pctAngular180Error, trialReactionTime;
    public bool responseAcquired = false;
    public static string token_SOT;
    public static string ipAddress = "";
    public string inputDomain = "spatialneuroscience.bio.uci.edu/SOT";
    // Start is called before the first frame update
    void Start()
    {
        FILE_NAME = LaunchManager.workerID + "_" + LaunchManager.token_SOT + ".csv";
        token_SOT = LaunchManager.token_SOT;
        compass = GameObject.Find("Compass");
        corrCompass = GameObject.Find("CorrectCircle");
        inputCompass = GameObject.Find("InputCircle");
        corrLine = GameObject.Find("CorrectLine");
        inputLine = GameObject.Find("InputLine");
        ipAddress = "http://" + inputDomain + "/OFTreceipt.php";

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            recordData();
        }
            
        if (Input.GetKeyDown("return") && responseAcquired)
        {
            trialData = GetDataString();
            WriteToFile();

            tm.NextTrial();

            responseAcquired = false;


            corrLine.GetComponent<MeshRenderer>().enabled = false;
            inputLine.GetComponent<MeshRenderer>().enabled = false;

        } else if (Input.GetKeyDown("return"))
        {
            tm.textInstructions.text = "Imagine you are standing at the <b>" + tm.SA.name + "</b> and facing the <b>" + tm.LA.name + "</b>. Point to the <b>" + tm.PA.name
            + "</b>\n\nPlease indicate your estimate with a mouse click, then please press ENTER when finished.";
        }

    }

    public void recordData()
    {
        responseAcquired = true;
        trialReactionTime = Time.time - TrialManager.trialStart;
        alignMousePos = new Vector3(FaceMouse.mousePos.x, FaceMouse.mousePos.y, compass.transform.position.z);

        inputAngle = GetInputAngle();
        correctAngle = GetCorrectAngle();

        DisplayCorrectAngle();
        

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

    public void DisplayCorrectAngle()
    {
        corrCompass.transform.eulerAngles = new Vector3(0, 0, 0);
        inputCompass.transform.eulerAngles = new Vector3(0, 0, 0);

        corrCompass.transform.Rotate(0, 0, correctAngle);
        inputCompass.transform.Rotate(0, 0, inputAngle);

        if (TrialManager.section != "Test")
        {
            corrLine.GetComponent<MeshRenderer>().enabled = true;
        }

        inputLine.GetComponent<MeshRenderer>().enabled = true;
        inputCompass.transform.up = FaceMouse.direction;
    }

    public string GetDataString()
    {
        float distance = Vector3.Distance(alignMousePos, compass.transform.position);
        angularError = Mathf.DeltaAngle((inputAngle), (correctAngle));
        pctAngular180Error = Mathf.Abs(angularError / 180);
        float inputAngleEulerClockwise = (360 - compass.transform.eulerAngles.z);
        float trialEnd = Time.time;
        float trialResponseTime = trialEnd - TrialManager.trialStart;

        Debug.Log(" IA: " + inputAngle + " CA: " + correctAngle + " AE: " + angularError + " TS: " + TrialManager.trialStart + " TE: " + trialEnd + " TR: " + trialReactionTime);

        string tdata = LaunchManager.workerID + ", " 
            + TrialManager.trialnum.ToString() + ", "
            + TrialManager.section + ", "
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
            + trialReactionTime.ToString() + ", "
            + trialResponseTime.ToString()
            ;

        return tdata;
    }

    void WriteToFile()
    {
        // Hard write to folder
        StreamWriter sw = File.AppendText(FILE_NAME);
        if (new FileInfo(FILE_NAME).Length == 0)
        {
            sw.WriteLine("workerID, trialNum, trialType, standAt, lookAt, pointAt, mousePos, mouseDist, mouseDir, responseAngle, correctAngle, angularError, pctAngularError, responseAngleEuler, startTime, endTime, reactionTime, responseTime");
        }
        sw.WriteLine(trialData);
        sw.Close();

        // PHP write to folder
        if (TrialManager.trialnum == 0)
        {
            WWWForm formHeader = new WWWForm();
            formHeader.AddField("input", "workerID, trialNum, trialType, standAt, lookAt, pointAt, mousePos, mouseDist, mouseDir, responseAngle, correctAngle, angularError, pctAngularError, responseAngleEuler, startTime, endTime, reactionTime, responseTime");
            formHeader.AddField("filename", FILE_NAME);

            WWW wwwH = new WWW(ipAddress, formHeader);
        }

        WWWForm form = new WWWForm();
        form.AddField("input", trialData);
        form.AddField("filename", FILE_NAME);

        WWW www = new WWW(ipAddress, form);
    }
}
