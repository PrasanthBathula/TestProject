using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI player2Score;
    // Start is called before the first frame update
    void Start()
    {
        int initScore = 0;
        UpdateScore(1, initScore.ToString());
        UpdateScore(2, initScore.ToString());
    }

    public void UpdateScore(int player, string score)
    {
        if (player == 1)
        {
            player1Score.text = "Score: " + score;
        }

        else if (player == 2)
        {
            player2Score.text = "Score: " + score;
        }
    }
}
