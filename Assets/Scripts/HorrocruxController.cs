using UnityEngine;

public class HorrocruxController : MonoBehaviour
{
    public Material InactiveMaterial;
    public Material GazedAtMaterial;

    private Renderer _renderer;

    public float gazeNeeded = 2f;

    private bool isGazed = false;
    private float gazeTimer = 0f;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        SetMaterial(false);
    }

    void Update()
    {
        if (isGazed)
        {
            gazeTimer += Time.deltaTime;

            float t = gazeTimer / gazeNeeded;
            HUDController.Instance.UpdateGazeProgress(t);

            if (gazeTimer >= gazeNeeded)
            {
                // Reseteamos círculo
                HUDController.Instance.UpdateGazeProgress(0f);

                // Avisamos al GameManager
                HorrocruxGameManager.Instance.OnHorrocruxCollected();

                // Reseteamos estados locales
                gazeTimer = 0f;
                isGazed = false;
                SetMaterial(false);
                
                AudioManager.Instance.StopHover();
                AudioManager.Instance.PlayCollect();
            }
        }
        else
        {
            if (gazeTimer > 0f)
            {
                gazeTimer = 0f;
                HUDController.Instance.UpdateGazeProgress(0f);
            }
        }
    }

    public void OnPointerEnter()
    {
        isGazed = true;
        SetMaterial(true);
        AudioManager.Instance.PlayHover();
    }

    public void OnPointerExit()
    {
        isGazed = false;
        gazeTimer = 0f;
        SetMaterial(false);
        HUDController.Instance.UpdateGazeProgress(0f);
        AudioManager.Instance.StopHover();
    }

    private void SetMaterial(bool gazedAt)
    {
        if (InactiveMaterial != null && GazedAtMaterial != null)
        {
            _renderer.material = gazedAt ? GazedAtMaterial : InactiveMaterial;
        }
    }
}
