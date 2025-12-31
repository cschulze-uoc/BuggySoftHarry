using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance { get; private set; }

    [Header("Escenas de minijuegos en orden")]
    public string[] minigameSceneNames;

    [Header("Nombre de la escena final")]
    public string finalScoreSceneName = "99_FinalScore";

    [Header("Puntuaci�n total de la partida actual")]
    public int totalScore = 0;

    [Header("Top 5 mejores puntuaciones (globales)")]
    public List<int> highScores = new List<int>();

    int currentMinigameIndex = -1;

    // ? NUEVO: saber si estamos en campa�a o en juego suelto
    [SerializeField] private bool isCampaignActive = false;
    public bool IsCampaignActive => isCampaignActive;

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
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // --- INICIO DE CAMPA�A DESDE EL MEN� ---
    public void StartMinigameSequence()
    {
        isCampaignActive = true;
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

    // ? NUEVO: empezar un juego suelto (SIN campa�a)
    public void StartSingleMinigame(string sceneName)
    {
        isCampaignActive = false;
        currentMinigameIndex = -1;   
        totalScore = 0;              
        SceneManager.LoadScene(sceneName);
    }

    // ? NUEVO: terminar campa�a manualmente (si quieres)
    public void EndCampaign()
    {
        isCampaignActive = false;
        currentMinigameIndex = -1;
    }

    // --- CAMBIO ENTRE MINIJUEGOS ---
    public void GoToNextMinigame()
    {
        // ? Si NO es campa�a, no hacemos �game loop�
        if (!isCampaignActive)
        {
            SceneManager.LoadScene("00_MainMenu");
            return;
        }

        if (minigameSceneNames == null || minigameSceneNames.Length == 0)
        {
            SceneManager.LoadScene("00_MainMenu");
            return;
        }

        currentMinigameIndex++;

        if (currentMinigameIndex >= 0 && currentMinigameIndex < minigameSceneNames.Length)
        {
            string sceneName = minigameSceneNames[currentMinigameIndex];
            SceneManager.LoadScene(sceneName);
        }
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
        highScores.Sort((a, b) => b.CompareTo(a));

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
