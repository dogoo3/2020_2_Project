using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AllClearWindow : MonoBehaviour
{
    [SerializeField] private Text text_score;

    public void Init()
    {
        ScoreManager.instance.ShowResultScore(text_score);
        gameObject.SetActive(true);
    }

    public void TouchExitButton()
    {
        SceneManager.LoadScene("Title");
    }
}
