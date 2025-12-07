using UnityEngine;

/// <summary>
/// Handles the sprite change of a box depending on the current season.
/// Works with SeasonManager to automatically update when seasons change.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SeasonalBox : MonoBehaviour
{
    [Header("Seasonal Sprites")]
    public Sprite springSprite;
    public Sprite summerSprite;
    public Sprite fallSprite;
    public Sprite winterSprite;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr == null)
            Debug.LogError("SeasonalBox requires a SpriteRenderer on the same GameObject.");
    }

    /// <summary>
    /// Updates the box's sprite based on the current season.
    /// </summary>
    /// <param name="season">Season to apply.</param>
    public void SetSeason(Season season)
    {
        if (sr == null) return;

        Sprite newSprite = season switch
        {
            Season.Spring => springSprite != null ? springSprite : sr.sprite,
            Season.Summer => summerSprite != null ? summerSprite : sr.sprite,
            Season.Fall   => fallSprite   != null ? fallSprite   : sr.sprite,
            Season.Winter => winterSprite != null ? winterSprite : sr.sprite,
            _ => sr.sprite
        };

        sr.sprite = newSprite;
    }
}
