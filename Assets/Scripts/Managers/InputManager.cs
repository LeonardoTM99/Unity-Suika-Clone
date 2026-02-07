using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerControls _playerControlsScript;

    private float moveDir;
    private bool dropPressed;



    private void Awake()
    {
        //SINGLETON
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _playerControlsScript = new PlayerControls();
    }

    private void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    // --- Listen for Input ---
    private void Update()
    {
        //Move Input
        moveDir = _playerControlsScript.Gameplay.Move.ReadValue<float>();
        //Drop Input
        dropPressed = _playerControlsScript.Gameplay.Drop.WasPressedThisFrame();

    }



    #region Get Input Methods

    public float GetMoveInput()
    {
        return moveDir;
    }

    public bool GetDropInput()
    {
        return dropPressed;
    }

    #endregion

    #region Helper Methods

    public void EnableGameplayInput()
    {
        _playerControlsScript.Gameplay.Enable();
    }

    public void DisableGameplayInput()
    {
        _playerControlsScript.Gameplay.Disable();
    }

    #endregion

    #region Game State Logic

    private void HandleGameStateChanged(GameStateManager.GameState state)
    {
        if (state == GameStateManager.GameState.Aiming)
        {
            EnableGameplayInput();
        }
        else
        {
            DisableGameplayInput();
        }
    }

    #endregion
}
