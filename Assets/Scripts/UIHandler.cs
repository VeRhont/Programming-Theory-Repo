using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class UIHandler : MonoBehaviour
{
    public void ResumeGame()
    {
        SceneManager.LoadScene(0);
    }

    public void StartNewGame()
    {
        var path = Application.persistentDataPath + "/player.data";
        Wallet.Count = 0;
        File.Delete(path);
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
