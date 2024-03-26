using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static int playerScore = 0;
    public static int AIScore = 0;
    public Text playerScoreText;
    public Text AIScoreText;
  
    
    private void Update()
    {
        
        playerScoreText.text = "Player Score: " + playerScore;
        AIScoreText.text = "AI Score: " + AIScore;
    }
}

