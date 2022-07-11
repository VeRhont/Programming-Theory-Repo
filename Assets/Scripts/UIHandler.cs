using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class UIHandler : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ResumeGame()
    {
        _audioSource.Play();
        SceneManager.LoadScene(0);
    }

    public void StartNewGame()
    {
        _audioSource.Play();
        var path = Application.persistentDataPath + "/player.data";
        Wallet.Count = 0;
        File.Delete(path);
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        _audioSource.Play();
        Application.Quit();
    }
}
