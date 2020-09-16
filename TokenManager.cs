using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TokenManager : MonoBehaviour
{
    public TextMeshProUGUI token;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(DataManager.token_SOT);
        
        token.SetText("UCISOT-" + DataManager.token_SOT);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
