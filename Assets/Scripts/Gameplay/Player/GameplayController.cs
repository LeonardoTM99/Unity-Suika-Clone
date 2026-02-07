using UnityEngine;

public class GameplayController : MonoBehaviour
{
    private GameObject currentMagicCircle;
    private Rigidbody2D currentRb;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float timeBetweenRounds = 1.5f;

    private float dropTimer;
    private bool isWaitingForNextRound;
    private bool canPlay;



    private void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void Update()
    {
        if (canPlay && currentMagicCircle != null)
        {
            float moveDir = InputManager.Instance.GetMoveInput();

            if (Mathf.Abs(moveDir) > 0.01f)
            {
                currentMagicCircle.transform.position +=
                    Vector3.right * moveDir * moveSpeed * Time.deltaTime;
            }

            if (InputManager.Instance.GetDropInput())
            {
                DropCurrentCircle();
            }
        }

        if (isWaitingForNextRound)
        {
            dropTimer += Time.deltaTime;

            if (dropTimer >= timeBetweenRounds)
            {
                isWaitingForNextRound = false;
                currentMagicCircle = null;
                currentRb = null;

                GameStateManager.Instance.SetState(GameStateManager.GameState.Aiming);
            }
        }
    }


    #region Helper Methods

    private void AskToSpawnNewCircle()
    {
        currentMagicCircle = SpawnerMagicCircles.Instance.SpawnAtStartLine();
        currentRb = currentMagicCircle.GetComponent<Rigidbody2D>();

        currentRb.bodyType = RigidbodyType2D.Kinematic;
        currentRb.gravityScale = 0f;
        canPlay = true;
    }

    private void DropCurrentCircle()
    {
        canPlay = false;

        dropTimer = 0f;
        isWaitingForNextRound = true;

        currentRb.bodyType = RigidbodyType2D.Dynamic;
        currentRb.gravityScale = 1f;

        InputManager.Instance.DisableGameplayInput();
        GameStateManager.Instance.SetState(GameStateManager.GameState.Dropped);
    }

    #endregion

    #region Game State Logic

    private void HandleGameStateChanged(GameStateManager.GameState state)
    {
        if (state == GameStateManager.GameState.Aiming)
        {
            AskToSpawnNewCircle();
        }
    }

    #endregion

}

