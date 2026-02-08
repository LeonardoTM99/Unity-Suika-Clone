using System.Drawing;
using UnityEngine;

public class MagicCircleBehavior : MonoBehaviour
{
    [Header("Game Over Logic")]
    [SerializeField] private float gameOverGraceTime = 0.75f;

    //Local Variables
    private SpriteRenderer _sprite;
    private Vector3 _size;
    private int _points;
    private MagicCircle _nextMagicCircleData;
    private MagicCircle currentMagicCircleData;

    private bool hasMerged; //Flag to prevent merge logic from happening twice
    private bool hasDropped = false;
    private bool canTriggerGameOver = false;
    private float dropTime;



    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void Update()
    {
        if (hasDropped && !canTriggerGameOver)
        {
            if (Time.time - dropTime >= gameOverGraceTime)
            {
                canTriggerGameOver = true;
            }
        }
    }


    #region Helper Methods
    public void AssignDataToNewCircle(MagicCircle mcData)
    {
        currentMagicCircleData = mcData;

        //--- Get Data from Scriptable Object & assign them to variables of this script ---

        _sprite.sprite = mcData.sprite;

        _size = mcData.size;
        transform.localScale = _size;

        _points = mcData.pointsEarned;

        _nextMagicCircleData = mcData.nextCircle;
    }

    #endregion

    #region Collision Logic

    // --- Collision Merge Logic ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasDropped)
        {
            hasDropped = true;
            dropTime = Time.time;
        }

        if (collision.gameObject.CompareTag("MagicCircle"))
        {
            MagicCircleBehavior collisionCircleScript = collision.gameObject.GetComponent<MagicCircleBehavior>();

            ContactPoint2D contact = collision.contacts[0];
            Vector2 contactPoint = contact.point;

            if (collisionCircleScript.currentMagicCircleData == currentMagicCircleData && _nextMagicCircleData != null)
            {
                if (!hasMerged && !collisionCircleScript.hasMerged)
                {
                    hasMerged = true;
                    collisionCircleScript.hasMerged = true;

                    SpawnerMagicCircles.Instance.SpawnAtMerged(contactPoint, _nextMagicCircleData);
                    ScoreManager.Instance.AddPoints(_points);

                    Destroy(gameObject);
                    Destroy(collisionCircleScript.gameObject);
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LoseArea") && canTriggerGameOver)
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.GameOver);
        }
    }


    #endregion

    #region Game State Logic
    private void HandleGameStateChanged(GameStateManager.GameState state)
    {
        if (state == GameStateManager.GameState.Menu)
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }

    #endregion

}
