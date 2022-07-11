using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _mainAudioSource;
    [SerializeField] private Slider _slider;

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

    public void ChangeVolume()
    {
        _mainAudioSource.volume = _slider.value;
    }

    public void Quit()
    {
        _audioSource.Play();
        Application.Quit();
    }
}
