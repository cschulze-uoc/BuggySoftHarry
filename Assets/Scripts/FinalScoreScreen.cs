using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text;

public class FinalScoreScreen : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoresText;

    void Start()
    {
        if (GlobalGameManager.Instance == null)
        {
            if (finalScoreText != null)
                finalScoreText.text = "Error: no hay GlobalGameManager.";
            return;
        }

        int finalScore = GlobalGameManager.Instance.totalScore;

        // Registrar esta run en el top 5
        GlobalGameManager.Instance.RegisterFinalScore(finalScore);

        // Mostrar puntuación total
        if (finalScoreText != null)
            finalScoreText.text = "Puntuación total: " + finalScore;

        // Mostrar ranking
        if (highScoresText != null)
        {
            var hs = GlobalGameManager.Instance.highScores;
            StringBuilder sb = new StringBuilder();

            if (hs.Count == 0)
            {
                sb.AppendLine("Aún no hay puntuaciones.");
            }
            else
            {
                for (int i = 0; i < hs.Count; i++)
                {
                    sb.AppendLine($"{i + 1}. {hs[i]} puntos");
                }
            }

            highScoresText.text = sb.ToString();
        }
    }

    public void OnBackToMenuButton()
    {
        if (GlobalGameManager.Instance != null)
            GlobalGameManager.Instance.totalScore = 0;

        SceneManager.LoadScene("00_MainMenu");
    }
}
