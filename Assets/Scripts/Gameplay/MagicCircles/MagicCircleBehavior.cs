using System.Drawing;
using UnityEngine;

public class MagicCircleBehavior : MonoBehaviour
{
    [Header ("References")]
    public MagicCircle actualMagicCircle; //reference to Magic circle Scriptable Object

    //Local Variables
    private SpriteRenderer _sprite;
    private Vector3 _size;
    private string _type;
    private int _points;
    private MagicCircle _nextMagicCircleData;

    private bool hasMerged; //Flag to prevent merge logic from happening twice


    void Start()
    {
        //--- Get Data from Scriptable Object & assign them to variables of this script ---

        _sprite = GetComponent<SpriteRenderer>();
        _sprite.sprite = actualMagicCircle.sprite;

        _size = actualMagicCircle.size;
        transform.localScale = _size;

        _type = actualMagicCircle.type;

        _points = actualMagicCircle.pointsEarned;

        _nextMagicCircleData = actualMagicCircle.nextCircle;

    }


    // --- Collision Merge Logic ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Box"))
        {
            MagicCircleBehavior collisionCircleScript = collision.gameObject.GetComponent<MagicCircleBehavior>(); //Get the MagicCircleB script from collision game object

            //Get the first point where collision happened
            ContactPoint2D contact = collision.contacts[0];
            Vector2 contactPoint = contact.point;

            //--- Merge logic ---
            if (collisionCircleScript._type == _type && _nextMagicCircleData != null) //If collides with same type magic circle and there is a next evolution
            {
                if (!hasMerged && !collisionCircleScript.hasMerged) //Check hasMerged flag to prevent this merge logic from happening twice (this object and the collision object)
                {

                    //SPAWN next circle at collision point
                    SpawnerMagicCircles.Instance.SpawnAtMerged(contactPoint, _nextMagicCircleData);

                    hasMerged = true;
                    collisionCircleScript.hasMerged = true;

                    //Destroy original circle
                    Destroy(gameObject);
                    Destroy(collisionCircleScript.gameObject);
                }
            }
        }
    }
}
