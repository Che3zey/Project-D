using UnityEngine;

public enum PushableType
{
    Box,
    Ice
}

[RequireComponent(typeof(SpriteRenderer))]
public class Pushable : MonoBehaviour
{
    public PushableType type;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            Debug.LogError("Pushable requires a SpriteRenderer.");

        // Make sure it starts active; SeasonManager controls visibility
        gameObject.SetActive(true);
    }

    public bool IsPushable()
    {
        SeasonManager seasonManager = FindObjectOfType<SeasonManager>();
        if (seasonManager == null) return false;

        return seasonManager.CanPush(this);
    }

    public void ApplySeason(Season currentSeason)
    {
        switch (type)
        {
            case PushableType.Box:
                gameObject.SetActive(true); // Boxes always visible
                break;
            case PushableType.Ice:
                gameObject.SetActive(currentSeason == Season.Winter); // Only active in Winter
                break;
        }
    }
}
