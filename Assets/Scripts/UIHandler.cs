using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public void ResumeGame()
    {
        SceneManager.LoadScene(0);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(0);
    }

}
