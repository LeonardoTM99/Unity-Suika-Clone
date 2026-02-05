using UnityEngine;

public class SpawnerMagicCircles : MonoBehaviour
{
    public static SpawnerMagicCircles Instance { get; private set; }

    [SerializeField] private GameObject magicCirclePrefab; //"Empty" prefab, to be assigned data later



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

    //--- Spawning new magic circle when MERGE happens ---
    public void SpawnAtMerged(Vector2 spawnPoint, MagicCircle newMCData)
    {
        GameObject newMagicCircle;

        newMagicCircle = Instantiate(magicCirclePrefab, spawnPoint, Quaternion.identity); //Spawn the "empty" magic circle prefab at collision point

        newMagicCircle.GetComponent<MagicCircleBehavior>()._actualMagicCircle = newMCData; //Set the actual data to the "empty" prefab, turning it into the actual next magic circle.
    }
}
