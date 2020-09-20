using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text text_score;

    private string _scoreString = "Score : ";
    private int _score;

    private void Awake()
    {
        instance = this; 
    }

    public void UpdateScore(int _score)
    {
        this._score += _score;
        text_score.text = _scoreString + this._score.ToString();
    }

    public void ShowResultScore(Text _resultText)
    {
        _resultText.text = _score.ToString();
    }

    public void ResetScore()
    {
        _score = 0;
        text_score.text = _scoreString + this._score.ToString();
    }
}
