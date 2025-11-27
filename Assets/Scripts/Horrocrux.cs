using UnityEngine; 

public class Horrocrux : MonoBehaviour
{
    public float gazeNeeded = 1f;     // segundos mirando para recogerlo
    private float gazeTimer = 0f;
    private bool isGazed = false;

    void Update()
    {
        if (isGazed)
        {
            gazeTimer += Time.deltaTime;
            if (gazeTimer >= gazeNeeded)
            {
                // HorrocruxGameManager.Instance.CollectHorrocrux(this);
                Destroy(gameObject);
            }
        }
        else
        {
            gazeTimer = 0f;
        }
    }

    public void OnPointerEnter()
    {
        isGazed = true;
    }

    public void OnPointerExit()
    {
        isGazed = false;
    }

    public void ForcePointerExit()
    {
        isGazed = false;
        gazeTimer = 0f;
    }
}
