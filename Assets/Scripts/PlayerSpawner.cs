using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Assign your Player Prefab")]
    public GameObject playerPrefab;

    private GameObject spawnedPlayer;

    void Start()
    {
        SpawnPlayer();
    }

    /// <summary>
    /// Instantiates the player at the spawner's position if not already spawned.
    /// </summary>
    public void SpawnPlayer()
    {
        if (spawnedPlayer != null) return;

        if (playerPrefab == null)
        {
            Debug.LogError("PlayerSpawner: No playerPrefab assigned!");
            return;
        }

        spawnedPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);

        // Optional: Keep player between scenes
        // DontDestroyOnLoad(spawnedPlayer);
    }
}
