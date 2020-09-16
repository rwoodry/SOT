using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class LaunchManager : MonoBehaviour
{
    public InputField workerIDField;
    public Button submitID;
    public GameObject instructText;
    public static string workerID;
    private bool sotReadyToStart = false;
    public TrialManager TM;
    public static string token_SOT;
    private static System.Random random = new System.Random();
    private static bool created = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (sotReadyToStart & Input.GetKeyDown("return"))
        {
            SceneManager.LoadScene("TrialScreen");
            TrialManager.trialnum = -1;

        }
    }

    public void SubmitID()
    {
        if (workerIDField.text != "")
        {
            instructText.GetComponent<TextMeshProUGUI>().enabled = true;
            workerID = workerIDField.text;
            workerIDField.GetComponent<InputField>().interactable = false;
            sotReadyToStart = true;

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            token_SOT = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
            Debug.Log(workerID + "TOKEN: " + token_SOT);

        }
    }


}
