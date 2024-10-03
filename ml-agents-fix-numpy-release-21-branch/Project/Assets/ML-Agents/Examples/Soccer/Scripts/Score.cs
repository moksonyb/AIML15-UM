using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_Text scoreDisplay;
    public bool resetScore = false;

    private static int blueScoreValue;
    private static int purpleScoreValue;



    void Start()
    {
        scoreDisplay = GetComponent<TextMeshProUGUI>();
        scoreDisplay.text = "0:dfer0";

        if (resetScore)
        {
            // Reset the score to 0! (action)
            blueScoreValue = 0;
            purpleScoreValue = 0;
        }

        scoreDisplay.text = blueScoreValue.ToString() + ":" + purpleScoreValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBlueScore(int toAdd)
    {
        blueScoreValue = blueScoreValue + toAdd;
        scoreDisplay.text = blueScoreValue.ToString() + ":" + purpleScoreValue.ToString();
    }

    public void AddPurpleScore(int toAdd)
    {
        purpleScoreValue = purpleScoreValue + toAdd;
        scoreDisplay.text = blueScoreValue.ToString() + ":" + purpleScoreValue.ToString();
    }
}
