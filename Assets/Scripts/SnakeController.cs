using UnityEngine;

public class SnakeController : MonoBehaviour
{

    private Renderer _renderer;

    public float gazeNeeded = 0.1f;

    private bool isGazed = false;
    private float gazeTimer = 0f;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (isGazed)
        {
            gazeTimer += Time.deltaTime;

            float t = gazeTimer / gazeNeeded;


            if (gazeTimer >= gazeNeeded)
            {

                // Avisamos al GameManager
                HorrocruxGameManager.Instance.OnSnakeViewed();

                // Reseteamos estados locales
                gazeTimer = 0f;
                isGazed = false;
            }
        }
        else
        {
            if (gazeTimer > 0f)
            {
                gazeTimer = 0f;

            }
        }
    }

    public void OnPointerEnter()
    {
        isGazed = true;
        AudioManager.Instance.PlayLose();
    }

    public void OnPointerExit()
    {
        isGazed = false;
        gazeTimer = 0f;

    }


}
