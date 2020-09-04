using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrialManager : MonoBehaviour
{

    public GameObject[] arrayStandAt;
    public GameObject[] arrayLookAt;
    public GameObject[] arrayPointAt;
    public GameObject SA;
    public GameObject LA;
    public GameObject PA;
    public Text text_startAt;
    public Text text_lookAt;
    public Text text_pointAt;
    private static bool created = false;
    public static int trialnum = 0;
    public static float trialStart;
    public int trialMax = 7;

    // Start is called before the first frame update
    void Start()
    {
        SA = arrayStandAt[trialnum];
        LA = arrayLookAt[trialnum];
        PA = arrayPointAt[trialnum];

        text_startAt.text = "Stand at: " + SA.name;
        text_lookAt.text = "Look at: " + LA.name;
        text_pointAt.text = "Point at: " + PA.name;

        trialStart = Time.time;

        Debug.Log("||SOT Started|| " + trialStart);
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
        if (trialnum == trialMax - 1)
        {
            SceneManager.LoadScene("Complete");
        }
        else
        {
            trialnum += 1;

            SA = arrayStandAt[trialnum];
            LA = arrayLookAt[trialnum];
            PA = arrayPointAt[trialnum];

            text_startAt.text = "Stand at: " + SA.name;
            text_lookAt.text = "Look at: " + LA.name;
            text_pointAt.text = "Point at: " + PA.name;

            trialStart = Time.time;

            Debug.Log("||Next Trial Loaded|| \tTrial: " + trialnum + "\tSA: " + SA.name + "\tLA: " + LA.name + "\tPA: " + PA.name);
        }

    }
}
