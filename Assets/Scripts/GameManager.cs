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

    public bool IsGameActive { get; set; }

    public void WinGame()
    {
        Time.timeScale = 0;
        _winText.gameObject.SetActive(true);
        _newGameButton.gameObject.SetActive(true);
        _toMenuButton.gameObject.SetActive(true);
        _saveAndExitButton.gameObject.SetActive(false);
    }

    public void LoseGame()
    {
        Time.timeScale = 0;
        _deathText.gameObject.SetActive(true);
        _newGameButton.gameObject.SetActive(true);
        _toMenuButton.gameObject.SetActive(true);
        _saveAndExitButton.gameObject.SetActive(false);
    }

    public void ToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void NewGame()
    {
        Time.timeScale = 1;
        var path = Application.persistentDataPath + "/player.data";
        Wallet.Count = 0;
        File.Delete(path);
        SceneManager.LoadScene(0);
    }
}
