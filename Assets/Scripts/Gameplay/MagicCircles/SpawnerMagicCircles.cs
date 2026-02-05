using NUnit.Framework;
using UnityEngine;

public class SpawnerMagicCircles : MonoBehaviour
{
    public static SpawnerMagicCircles Instance { get; private set; }

    [SerializeField] private GameObject magicCirclePrefab; //"Empty" prefab, to be assigned data later
    [SerializeField] private Transform startLinePosition;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    #region Spawn Methods
    public void SpawnAtStartLine(GameObject magicCircle)
    {
        Instantiate(magicCircle, startLinePosition.position, Quaternion.identity);
    }

    //--- Spawning new magic circle when MERGE happens ---
    public void SpawnAtMerged(Vector2 spawnPoint, MagicCircle newMCData)
    {
        GameObject newMagicCircle;

        newMagicCircle = Instantiate(magicCirclePrefab, spawnPoint, Quaternion.identity); //Spawn the "empty" magic circle prefab at collision point

        newMagicCircle.GetComponent<MagicCircleBehavior>().actualMagicCircle = newMCData; //Set the actual data to the "empty" prefab, turning it into the actual next magic circle.
    }

    #endregion

    #region Game State Logic

    private void HandleGameStateChanged(GameStateManager.GameState state)
    {
        if (state == GameStateManager.GameState.Preparing)
        {
            SpawnAtStartLine(magicCirclePrefab);
        }
    }

    #endregion
}
