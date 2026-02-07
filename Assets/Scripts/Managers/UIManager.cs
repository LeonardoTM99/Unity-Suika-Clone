using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Panels")]
    [SerializeField] private GameObject startMenuUIPanel;
    [SerializeField] private GameObject gameplayUIPanel;

    [Header("Other References")]
    [SerializeField] private DifficultySettings easyDifficultySO;
    [SerializeField] private DifficultySettings normalDifficultySO;
    [SerializeField] private DifficultySettings hardDifficultySO;



    private void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;

        startMenuUIPanel.SetActive(true);

        SpawnerMagicCircles.Instance.selectedSettings = normalDifficultySO;
    }

    public void StartGame()
    {
        startMenuUIPanel.SetActive(false);
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

    #region Game State Logic

    private void HandleGameStateChanged(GameStateManager.GameState state)
    {
        if (state != GameStateManager.GameState.Menu)
        {
            startMenuUIPanel.SetActive(false);
            gameplayUIPanel.SetActive(true);
        }
        else
        {
            gameplayUIPanel.SetActive(false);
            startMenuUIPanel.SetActive(true);
        }
    }

    #endregion

}
