using NUnit.Framework;
using UnityEngine;

public class SpawnerMagicCircles : MonoBehaviour
{
    public static SpawnerMagicCircles Instance { get; private set; }

    public DifficultySettings selectedSettings;

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

    #region Spawn Methods
    public GameObject SpawnAtStartLine()
    {
        int randomIndex = Random.Range(0, selectedSettings.magicCircleData.Length);
        GameObject newMagicCircle;

        newMagicCircle = Instantiate(magicCirclePrefab, startLinePosition.position, Quaternion.identity);

        newMagicCircle.GetComponent<MagicCircleBehavior>().AssignDataToNewCircle(selectedSettings.magicCircleData[randomIndex]);

        return newMagicCircle;
    }

    //--- Spawning new magic circle when MERGE happens ---
    public void SpawnAtMerged(Vector2 spawnPoint, MagicCircle newMCData)
    {
        GameObject newMagicCircle;

        newMagicCircle = Instantiate(magicCirclePrefab, spawnPoint, Quaternion.identity); //Spawn the "empty" magic circle prefab at collision point

        newMagicCircle.GetComponent<MagicCircleBehavior>().AssignDataToNewCircle(newMCData); //Set the actual data to the "empty" prefab, turning it into the actual next magic circle.
    }

    #endregion

}
