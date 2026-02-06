using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Panels")]
    [SerializeField] private GameObject startMenuPanel;

    [Header("Other References")]
    [SerializeField] private DifficultySettings easyDifficultySO;
    [SerializeField] private DifficultySettings normalDifficultySO;
    [SerializeField] private DifficultySettings hardDifficultySO;



    private void Start()
    {
        startMenuPanel.SetActive(true);
    }

    public void StartGame()
    {
        startMenuPanel.SetActive(false);
        GameStateManager.Instance.SetState(GameStateManager.GameState.Aiming);
    }

    public void SetEasyDifficulty()
    {
        SpawnerMagicCircles.Instance.selectedSettings = easyDifficultySO;
    }
    public void SetNormalDifficulty()
    {
        SpawnerMagicCircles.Instance.selectedSettings = normalDifficultySO;
    }
    public void SetHardDifficulty()
    {
        SpawnerMagicCircles.Instance.selectedSettings = hardDifficultySO;
    }

}
