using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrialManager : MonoBehaviour
{

    public GameObject[] arrayStandAt;
    public GameObject[] arrayLookAt;
    public GameObject[] arrayPointAt;
    public GameObject SA, LA, PA, corrCompass, corrLine;
    public Text textInstructions, textSA, textLA;
    public static string section = "Practice";
    private static bool created = false;
    public static int trialnum = -1;
    public static float trialStart;
    public int trialMax = 12;
    public float timeLimit = 300;
    public float trialSceneInitTime;
    public InstructionsManager IM;
    public bool trialBegan = false;
    public DataManager DM;
    public TextMeshProUGUI tokenText;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("TRIAL SCENE LOADED: " + trialnum);
        NextTrial();
        corrCompass = GameObject.Find("CorrectCircle");
        corrLine = GameObject.Find("CorrectLine");

    }

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            Debug.Log("Awake: " + this.gameObject);
        }


    }


    // Update is called once per frame
    void Update()
    {

    }

    public void NextTrial()
    {
        // If trial number is equal to trial max plus inistruction & practice scenes (4) we load Completion screen. We subtract one because scenes initialize at 0, not 1.
        if (trialnum == trialMax + 3 || (Time.time - trialSceneInitTime >= timeLimit && trialBegan))
        {
            SceneManager.LoadScene("Complete");
        }
 
        else if (trialnum == -1 && !InstructionsManager.instructionsViewed)
        {
            section = "Instructions";
            SceneManager.LoadScene("Instructions");
            Debug.Log("TM INSTRUCTIONS LOADED " + trialnum);

        }
       
        else if (trialnum == 0 && !InstructionsManager.practiceViewed)
        {
            section = "Practice";
            SceneManager.LoadScene("Instructions");
            Debug.Log("TM PRACTICE LOADED " + trialnum);

        } else if (trialnum == 3 && !InstructionsManager.testViewed)
        {
            section = "Test";
            SceneManager.LoadScene("Instructions");
            Debug.Log("TM TEST LOADED " + trialnum);


        }
        else
        {
            Debug.Log("Current Trial loaded: " + trialnum);
            if (trialnum == 3)
            {
                trialSceneInitTime = Time.time;
                trialBegan = true;
            }

            trialnum += 1;
            Debug.Log("NT+ loaded: " + trialnum);

            SA = arrayStandAt[trialnum];
            LA = arrayLookAt[trialnum];
            PA = arrayPointAt[trialnum];

            if (section == "Instructions")
            {
                textInstructions.text = "Imagine you are standing at the <b>" + SA.name + "</b> and facing the <b>" + LA.name + "</b>. Point to the <b>" + PA.name
                + "</b>\n\nHere is a sample item that has the correct answer shown. Orient your line (Black) using the mouse to the desired response. Match your answer to the correct answer shown (Green) and click to set your answer (Purple). Can you satisfy yourself that this answer is the correct answer?" 
                + "\n\nPlease press ENTER when finished.";

                corrCompass = GameObject.Find("CorrectCircle");
                corrLine = GameObject.Find("CorrectLine");

                float correctAngle = DM.GetCorrectAngle();
                corrCompass.transform.eulerAngles = new Vector3(0, 0, 0);
                corrCompass.transform.Rotate(0, 0, correctAngle);
                corrLine.GetComponent<MeshRenderer>().enabled = true;
                
            } 
            else
            {
                textInstructions.text = "Imagine you are standing at the <b>" + SA.name + "</b> and facing the <b>" + LA.name + "</b>. Point to the <b>" + PA.name
                + "</b>\n\nPlease press ENTER when finished.";
            }


            textSA.text = SA.name;
            textLA.text = LA.name;

            trialStart = Time.time;

            Debug.Log("||Next Trial Loaded|| \tTrial: " + trialnum + "\tSA: " + SA.name + "\tLA: " + LA.name + "\tPA: " + PA.name + "\tTSITdiff: " + (Time.time - trialSceneInitTime));
        }

    }
}
