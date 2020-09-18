using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private int _score;

    public void PlusScore(int _score)
    {
        this._score += _score;
    }
}
