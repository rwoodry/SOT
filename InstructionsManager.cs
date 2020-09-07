using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class InstructionsManager : MonoBehaviour
{
    public TextMeshProUGUI instructionsText;
    [SerializeField] RawImage instructionImage;
    public TrialManager TM;
    public static bool instructionsViewed = false;
    public static bool practiceViewed = false;
    public static bool testViewed = false;
    private static bool created = false;
    private bool loaded = false;

    // Start is called before the first frame update
    void Start()
    {
        DisplayInstructions();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && SceneManager.GetActiveScene().name == "Instructions")
        {
            ContinueToTrial();
        }
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

    public void DisplayInstructions()
    {

        if (TrialManager.section == "Instructions")
        {
            instructionImage.GetComponent<RawImage>().enabled = true;
            instructionsText.transform.localPosition = new Vector3(0, 90, 0);
            instructionsText.fontSize = 8;
            instructionsText.text = 
                "This is a test of your ability to imagine different perspectives or orientations in space. In this task, you will see a picture of an array of objects with a statement below it, together with an “arrow circle”. You will be asked to imagine that you are standing at one object in the array and facing another object. Your task is to draw a line showing the direction to a third object from this perspective. On each trial, you will be asked to imagine standing at a different first object, facing a different second object, and then to draw a line to a different third object. \n\nYou respond by “drawing” a line on the arrow circle using the computer mouse. The center of the arrow circle represents your imagined location (at the first object) and the vertical arrow represents your imagined perspective (facing the second object). You need to draw the direction to a third object from this facing direction. " +
                "\n\nLook at the sample item below. In this example you are asked to imagine that you are standing at the bell facing the tree. Your task is to draw a line indicating the direction to the drum. In the sample item this line has been drawn for you. In the test items, your task is to draw this line on the arrow circle using the computer mouse. Can you see that if you were at the bell facing the tree the drum would be in the direction shown by the dotted line? " +
                "\n\nNow you will begin practicing on the computer. " +
                "\n\nPress the SPACE BAR to continue.";

            instructionsViewed = true;
            

        } else if (TrialManager.section == "Practice")
        {
            instructionsText.fontSize = 12;
            instructionsText.alignment = TextAlignmentOptions.Center;
            instructionsText.text =
                "Now you will do three practice trials. When each trial appears, move the line to indicate your answer." +
                "\n\nOnce you have entered your answer the correct answer will be shown in green. " +
                "\n\nPress SPACE BAR to see the first practice trial.";
            practiceViewed = true;


        } else if (TrialManager.section == "Test")
        {
            instructionsText.fontSize = 12;
            instructionsText.alignment = TextAlignmentOptions.Center;
            instructionsText.text =
                "Now you will do the test. There are 12 items on this test. You will have 5 minutes to complete these items. " +
                "\n\nPlease try to respond accurately, but do not spend too much time on any one item. " +
                "\n\nWhen you are ready to start, press SPACE BAR.";
            testViewed = true;
        }

        
    }

    public void ContinueToTrial()
    {
        Debug.Log(TrialManager.trialnum);
        SceneManager.LoadScene("TrialScreen");
        

    }
}
