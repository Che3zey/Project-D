using UnityEngine;

public enum Season
{
    Spring,
    Summer,
    Fall,
    Winter
}

public class SeasonManager : MonoBehaviour
{
    [Header("Current Season")]
    public Season currentSeason = Season.Spring;

    [Header("Seasonal Tilemap Roots")]
    public GameObject springGrid;
    public GameObject summerGrid;
    public GameObject fallGrid;
    public GameObject winterGrid;

    [Header("Objects Affected By Seasons")]
    public SeasonalBox[] boxes;        // Boxes always exist
    public Pushable[] pushables;       // Include all Ice and Box objects here

    private void Start()
    {
        ApplySeasonEffects();
    }

    public void NextSeason()
    {
        currentSeason = (Season)(((int)currentSeason + 1) % 4);
        ApplySeasonEffects();
    }

    public void PreviousSeason()
    {
        currentSeason = (Season)(((int)currentSeason - 1 + 4) % 4);
        ApplySeasonEffects();
    }

    private void ApplySeasonEffects()
    {
        // Activate only the correct tilemap
        if (springGrid) springGrid.SetActive(currentSeason == Season.Spring);
        if (summerGrid) summerGrid.SetActive(currentSeason == Season.Summer);
        if (fallGrid)   fallGrid.SetActive(currentSeason == Season.Fall);
        if (winterGrid) winterGrid.SetActive(currentSeason == Season.Winter);

        // Update boxes sprites
        if (boxes != null)
        {
            foreach (var box in boxes)
            {
                if (box != null)
                    box.SetSeason(currentSeason);
            }
        }

        // Update pushables (boxes and ice)
        if (pushables != null)
        {
            foreach (var obj in pushables)
            {
                if (obj != null)
                    obj.ApplySeason(currentSeason); // This will setActive correctly
            }
        }
    }

    public bool CanPush(Pushable pushObj)
    {
        if (pushObj == null) return false;

        switch (pushObj.type)
        {
            case PushableType.Box: return currentSeason == Season.Fall;
            case PushableType.Ice: return currentSeason == Season.Winter;
            default: return false;
        }
    }
}
