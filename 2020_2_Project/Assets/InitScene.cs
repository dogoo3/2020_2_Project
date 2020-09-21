using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("Title");
    }
}
