using System.Drawing;
using UnityEngine;

public class MagicCircleBehavior : MonoBehaviour
{

    //Local Variables
    private SpriteRenderer _sprite;
    private Vector3 _size;
    private int _points;
    private MagicCircle _nextMagicCircleData;
    private MagicCircle currentMagicCircleData;

    private bool hasMerged; //Flag to prevent merge logic from happening twice

    

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
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

        if (collision.gameObject.CompareTag("MagicCircle"))
        {
            MagicCircleBehavior collisionCircleScript = collision.gameObject.GetComponent<MagicCircleBehavior>(); //Get the MagicCircleB script from collision game object

            //Get the first point where collision happened
            ContactPoint2D contact = collision.contacts[0];
            Vector2 contactPoint = contact.point;

            //--- Merge logic ---
            if (collisionCircleScript.currentMagicCircleData == currentMagicCircleData && _nextMagicCircleData != null) //If collides with same type magic circle and there is a next evolution
            {
                if (!hasMerged && !collisionCircleScript.hasMerged) //Check hasMerged flag to prevent this merge logic from happening twice (this object and the collision object)
                {

                    hasMerged = true;
                    collisionCircleScript.hasMerged = true;

                    //SPAWN next circle at collision point
                    SpawnerMagicCircles.Instance.SpawnAtMerged(contactPoint, _nextMagicCircleData);

                    //Add Points to score
                    ScoreManager.Instance.AddPoints(_points);

                    //Destroy original circle
                    Destroy(gameObject);
                    Destroy(collisionCircleScript.gameObject);
                }
            }
        }
    }

    #endregion
}
