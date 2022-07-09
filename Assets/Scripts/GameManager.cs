using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _deathText;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _toMenuButton;

    public bool IsGameActive { get; set; }

    private void GameOver()
    {

    }

    public void ToMenu()
    {

    }

    public void NewGame()
    {

    }


}
