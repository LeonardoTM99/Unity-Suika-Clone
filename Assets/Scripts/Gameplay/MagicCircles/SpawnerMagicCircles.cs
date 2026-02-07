using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerMagicCircles : MonoBehaviour
{
    public static SpawnerMagicCircles Instance;

    [Header("References")]
    [SerializeField] private GameObject magicCirclePrefab;
    [SerializeField] private Transform startLinePosition;

    [Header("Settings")]
    public DifficultySettings selectedSettings;

    private List<MagicCircle> upcomingCircles = new List<MagicCircle>();



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    #region Upcoming Circles Logic

    public void InitializeUpcomingCircles()
    {
        upcomingCircles.Clear();

        for (int i = 0; i < 3; i++)
        {
            RefillUpcomingCirclesList();
        }
    }

    private void RefillUpcomingCirclesList()
    {
        int randomIndex = Random.Range(0, selectedSettings.magicCircleData.Length);
        upcomingCircles.Add(selectedSettings.magicCircleData[randomIndex]);
    }

    #endregion

    #region Spawn Methods

    public GameObject SpawnAtStartLine()
    {
        GameObject newMagicCircle = Instantiate(magicCirclePrefab, startLinePosition.position, Quaternion.identity);

        // Assign FIRST upcoming circle
        newMagicCircle.GetComponent<MagicCircleBehavior>().AssignDataToNewCircle(upcomingCircles[0]);

        // Consume it
        upcomingCircles.RemoveAt(0);

        // Refill only ONE at the back
        RefillUpcomingCirclesList();

        //UI refresh happens AFTER spawn
        UIManager.Instance.RefreshUpcomingCirclesUI();

        return newMagicCircle;
    }

    public void SpawnAtMerged(Vector2 spawnPoint, MagicCircle newMCData)
    {
        GameObject newMagicCircle;

        newMagicCircle = Instantiate(magicCirclePrefab, spawnPoint, Quaternion.identity);

        newMagicCircle.GetComponent<MagicCircleBehavior>().AssignDataToNewCircle(newMCData);
    }

    #endregion

    #region UI

    public void DisplayUpcomingCirclesInUI(Image first, Image second, Image third)
    {
        first.sprite = upcomingCircles[0].sprite;
        second.sprite = upcomingCircles[1].sprite;
        third.sprite = upcomingCircles[2].sprite;
    }

    #endregion
}
