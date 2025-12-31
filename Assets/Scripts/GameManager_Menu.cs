using UnityEngine;

public class GameManager_Menu : MonoBehaviour
{
    public void StartCampaign()
    {
        GlobalGameManager.Instance.StartMinigameSequence();
    }

    public void StartSingleGame(string sceneName)
    {
        GlobalGameManager.Instance.StartSingleMinigame(sceneName);
    }
}
