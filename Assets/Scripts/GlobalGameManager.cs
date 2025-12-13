using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance { get; private set; }

    [Header("Escenas de minijuegos en orden")]
    public string[] minigameSceneNames;   // ej: 0 = 04_PatronusDementors_AR, 1 = 06_ChamberOfSecrets_VR

    [Header("Nombre de la escena final")]
    public string finalScoreSceneName = "99_FinalScore";

    [Header("Puntuación total de la partida actual")]
    public int totalScore = 0;

    [Header("Top 5 mejores puntuaciones (globales)")]
    public List<int> highScores = new List<int>();

    int currentMinigameIndex = -1;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScores();
    }

    // --- INICIO DE CAMPAÑA DESDE EL MENÚ ---

    public void StartMinigameSequence()
    {
        totalScore = 0;
        currentMinigameIndex = 0;

        if (minigameSceneNames != null && minigameSceneNames.Length > 0)
        {
            SceneManager.LoadScene(minigameSceneNames[0]);
        }
        else
        {
            Debug.LogError("GlobalGameManager: no hay escenas configuradas en minigameSceneNames.");
        }
    }

    // --- CAMBIO ENTRE MINIJUEGOS ---

    public void GoToNextMinigame()
    {
        if (minigameSceneNames == null || minigameSceneNames.Length == 0)
        {
            SceneManager.LoadScene("00_MainMenu");
            return;
        }

        currentMinigameIndex++;

        // Aún quedan minijuegos
        if (currentMinigameIndex >= 0 && currentMinigameIndex < minigameSceneNames.Length)
        {
            string sceneName = minigameSceneNames[currentMinigameIndex];
            SceneManager.LoadScene(sceneName);
        }
        // Ya hemos jugado todos -> ir a escena final
        else
        {
            SceneManager.LoadScene(finalScoreSceneName);
        }
    }

    // --- TOP 5 PUNTUACIONES ---

    public void RegisterFinalScore(int finalScore)
    {
        if (finalScore <= 0) return;

        highScores.Add(finalScore);
        highScores.Sort((a, b) => b.CompareTo(a));   // de mayor a menor

        if (highScores.Count > 5)
            highScores.RemoveRange(5, highScores.Count - 5);

        SaveHighScores();
    }

    void LoadHighScores()
    {
        highScores.Clear();

        for (int i = 0; i < 5; i++)
        {
            int score = PlayerPrefs.GetInt("HighScore" + i, 0);
            if (score > 0)
                highScores.Add(score);
        }

        highScores.Sort((a, b) => b.CompareTo(a));
    }

    void SaveHighScores()
    {
        for (int i = 0; i < 5; i++)
        {
            int value = (i < highScores.Count) ? highScores[i] : 0;
            PlayerPrefs.SetInt("HighScore" + i, value);
        }

        PlayerPrefs.Save();
    }
}
