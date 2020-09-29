using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    
    private int _score;

    private void Awake()
    {
        instance = this; 
    }

    public void UpdateScore(int _score)
    {
        this._score += _score;
    }

    public void ShowResultScore(Text _resultText)
    {
        _resultText.text = _score.ToString();
    }

    public void ResetScore()
    {
        _score = 0;
    }
}
