using UnityEngine;

public class SnakeController : MonoBehaviour
{
    // public Material InactiveMaterial;
    // public Material GazedAtMaterial;

    private Renderer _renderer;

    public float gazeNeeded = 0.1f;

    private bool isGazed = false;
    private float gazeTimer = 0f;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        // SetMaterial(false);
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
                // SetMaterial(false);
                
                // AudioManager.Instance.StopHover();
                // AudioManager.Instance.PlayCollect();
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
        // SetMaterial(true);
        AudioManager.Instance.PlayLose();
    }

    public void OnPointerExit()
    {
        isGazed = false;
        gazeTimer = 0f;
        // SetMaterial(false);
        
        // AudioManager.Instance.StopHover();
    }

    // private void SetMaterial(bool gazedAt)
    // {
    //     if (InactiveMaterial != null && GazedAtMaterial != null)
    //     {
    //         _renderer.material = gazedAt ? GazedAtMaterial : InactiveMaterial;
    //     }
    // }
}
