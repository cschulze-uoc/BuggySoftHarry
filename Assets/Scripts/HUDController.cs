using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
     public static HUDController Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public Image gazeProgressImage;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Horrocruxes: " + score;
    }

    public void UpdateTimer(float time)
    {
        timerText.text = "Tiempo: " + time.ToString("0");
    }

    public void UpdateGazeProgress(float t)
    {
        if (gazeProgressImage == null) return;
        gazeProgressImage.fillAmount = Mathf.Clamp01(t);
    }
}
