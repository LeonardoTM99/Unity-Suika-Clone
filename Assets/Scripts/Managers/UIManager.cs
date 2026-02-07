using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    [SerializeField] private GameObject startMenuUIPanel;
    [SerializeField] private GameObject gameplayUIPanel;

    [Header("Gameplay Panel UI Elements")]
    [SerializeField] private Image firstUpcomingCircle;
    [SerializeField] private Image secondUpcomingCircle;
    [SerializeField] private Image thirdUpcomingCircle;

    [Header("Difficulty Settings")]
    [SerializeField] private DifficultySettings easyDifficultySO;
    [SerializeField] private DifficultySettings normalDifficultySO;
    [SerializeField] private DifficultySettings hardDifficultySO;



    private void Awake()
    {
        //Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;

        startMenuUIPanel.SetActive(true);
        gameplayUIPanel.SetActive(false);

        SpawnerMagicCircles.Instance.selectedSettings = normalDifficultySO;
    }


    public void StartGame()
    {
        SpawnerMagicCircles.Instance.InitializeUpcomingCircles();

        startMenuUIPanel.SetActive(false);
        gameplayUIPanel.SetActive(true);

        GameStateManager.Instance.SetState(GameStateManager.GameState.Aiming);

        RefreshUpcomingCirclesUI();

    }

    #region Difficulty

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

    #endregion

    #region UI Updates

    public void RefreshUpcomingCirclesUI()
    {
        SpawnerMagicCircles.Instance.DisplayUpcomingCirclesInUI(firstUpcomingCircle, secondUpcomingCircle, thirdUpcomingCircle);
    }

    #endregion

    #region Game State Logic

    private void HandleGameStateChanged(GameStateManager.GameState state)
    {
        bool isMenu = state == GameStateManager.GameState.Menu;

        startMenuUIPanel.SetActive(isMenu);
        gameplayUIPanel.SetActive(!isMenu);
    }

    #endregion
}
