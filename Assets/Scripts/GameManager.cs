using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _deathText;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _toMenuButton;
    [SerializeField] private Button _saveAndExitButton;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public bool IsGameActive { get; set; }

    public void WinGame()
    {
        Time.timeScale = 0;
        _winText.gameObject.SetActive(true);
        ShowButtons();
    }

    public void LoseGame()
    {
        Time.timeScale = 0;
        _deathText.gameObject.SetActive(true);
        ShowButtons();
    }

    private void ShowButtons()
    {
        _newGameButton.gameObject.SetActive(true);
        _toMenuButton.gameObject.SetActive(true);
        _saveAndExitButton.gameObject.SetActive(false);
    }

    public void ReturnToMenu()
    {
        _audioSource.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void StartNewGame()
    {
        _audioSource.Play();
        Time.timeScale = 1;
        var path = Application.persistentDataPath + "/player.data";
        Wallet.Count = 0;
        File.Delete(path);
        SceneManager.LoadScene(0);
    }
}