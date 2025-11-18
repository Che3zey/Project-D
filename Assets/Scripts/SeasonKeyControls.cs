using UnityEngine;

public class SeasonKeyControls : MonoBehaviour
{
    public KeyCode changeSeasonKey = KeyCode.E;
    private SeasonManager seasonManager;

    void Start()
    {
        // New Unity API — no warnings
        seasonManager = Object.FindFirstObjectByType<SeasonManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(changeSeasonKey))
        {
            if (seasonManager != null)
            {
                seasonManager.NextSeason();
            }
            else
            {
                Debug.LogError("SeasonManager not found in scene!");
            }
        }
    }
}
