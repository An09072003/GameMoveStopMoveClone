using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;

    [Header("Alive Counter")]
    [SerializeField] private TextMeshProUGUI aliveText;

    private bool gameEnded = false;

    void Start()
    {
        winUI.SetActive(false);
        loseUI.SetActive(false);

    }

    void Update()
    {
        if (gameEnded) return;

        UpdateAliveCountAndCheckEnd();
    }

    private void UpdateAliveCountAndCheckEnd()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] bots = GameObject.FindGameObjectsWithTag("Bot");

        int aliveCount = 0;

        foreach (GameObject obj in players)
        {
            if (obj != null)
                aliveCount++;
        }

        foreach (GameObject obj in bots)
        {
            if (obj != null)
                aliveCount++;
        }

        // ✅ Cập nhật text
        aliveText.text = "ALIVE: " + aliveCount;

        // Kiểm tra thắng/thua
        if (players.Length == 0)
        {
            ShowLose();
        }
        else if (aliveCount == 1 && players.Length == 1)
        {
            ShowWin();
        }
    }

    private void ShowWin()
    {
        gameEnded = true;
        winUI.SetActive(true);
    }

    private void ShowLose()
    {
        gameEnded = true;
        loseUI.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
