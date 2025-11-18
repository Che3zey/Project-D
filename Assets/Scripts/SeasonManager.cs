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
    public Season currentSeason = Season.Spring;

    public void NextSeason()
    {
        currentSeason = (Season)(((int)currentSeason + 1) % 4);
        Debug.Log("Season changed to: " + currentSeason);
        ApplySeasonEffects();
    }

    public void PreviousSeason()
    {
        currentSeason = (Season)(((int)currentSeason - 1 + 4) % 4);
        Debug.Log("Season changed to: " + currentSeason);
        ApplySeasonEffects();
    }

    void ApplySeasonEffects()
    {
        // TODO: change tilesets, weather, music, etc.
    }
}
