using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform dropLine;
    [SerializeField] private Transform startLine;
    private GameObject currentMagicCircle;
    private Rigidbody2D currentRb;

    [Header("Parameters")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float timeBetweenRounds = 1.5f;

    //Define the move circle limit bounds
    float halfWidth;
    float leftLimitX;
    float rightLimitX;
    private float dropTimer;
    private bool isWaitingForNextRound;
    private bool canPlay;



    private void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;

        halfWidth = startLine.localScale.x * 0.5f;
        leftLimitX = startLine.position.x - halfWidth;
        rightLimitX = startLine.position.x + halfWidth;
    }

    private void Update()
    {

        if (canPlay && currentMagicCircle != null)
        {
            //Visual Drop Line Logic
            AppearDropLine();

            //Moving magic circle logic
            float moveDir = InputManager.Instance.GetMoveInput();

            if (Mathf.Abs(moveDir) > 0.01f)
            {
                Vector3 pos = currentMagicCircle.transform.position;

                pos.x += moveDir * moveSpeed * Time.deltaTime;
                pos.x = Mathf.Clamp(pos.x, leftLimitX, rightLimitX);

                currentMagicCircle.transform.position = pos;

            }

            //Dropping magic circle logic
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

    private void AppearDropLine()
    {

        //Make drop line game object active if was inactive
        if (dropLine.gameObject.activeSelf == false)
        {
            dropLine.gameObject.SetActive(true);
        }

        //Move the drop line along with the current circle
        dropLine.position = new Vector3(currentMagicCircle.transform.position.x, dropLine.position.y, dropLine.position.z);

    }

    private void DisappearDropLine()
    {
        //Deactivate drop line if was active
        if (dropLine.gameObject.activeSelf == true)
        {
            dropLine.gameObject.SetActive(false);
        }

        //Reset X position
        dropLine.position = new Vector3(0, dropLine.position.y, dropLine.position.z);

    }

    #endregion

    #region Game State Logic

    private void HandleGameStateChanged(GameStateManager.GameState state)
    {
        if (state == GameStateManager.GameState.Aiming)
        {
            AskToSpawnNewCircle();
        }
        
        if (state == GameStateManager.GameState.Dropped)
        {
            DisappearDropLine();
        }
    }

    #endregion

}

