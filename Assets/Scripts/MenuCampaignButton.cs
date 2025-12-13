using UnityEngine;

public class MenuCampaignButton : MonoBehaviour
{
    public void OnCampaignClicked()
    {
        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.StartMinigameSequence();
        }
        else
        {
            Debug.LogError("MenuCampaignButton: GlobalGameManager.Instance es null");
        }
    }
}
