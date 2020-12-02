using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MissileMover : MonoBehaviour
{
    public GameObject target;
    bool randomTurns;
    bool directTarget;
    bool stopRotation;

    private Camera camera;

    public float moveSpeed;
    public float turnRangeMin;
    public float turnRangeMax;
    public float turnIntervalMin;
    public float turnIntervalMax;
    private float nextTurnTime;

    void Start()
    {
        target = GameObject.Find("Target");
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        
        transform.LookAt(target.transform);
        randomTurns = true;
        directTarget = false;
        stopRotation = false;
    }

    void FixedUpdate()
    {

        if (Vector3.Distance(transform.position, target.transform.position) < 2.5f)
        {
            randomTurns = false;
            directTarget = true;
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            directTarget = false;
            stopRotation = true;
        }

        if (!stopRotation)
        {
            if (directTarget)
            {
                transform.LookAt(target.transform);
            }

            if (randomTurns)
            {
                
                if(Vector3.Distance(transform.position, target.transform.position) > 8f)
                {
                    transform.LookAt(target.transform);
                } else if (Time.time > nextTurnTime)
                {
                    // Get next turn time
                    System.Random randomSeed = new System.Random();
                    double timeRange = (double)turnIntervalMax - (double)(turnIntervalMin);
                    double sampleTime = randomSeed.NextDouble();
                    double scaledTime = (sampleTime * timeRange) + (turnIntervalMin);
                    float randomTime = (float)scaledTime;
                    nextTurnTime = Time.time + randomTime;

                    // Turn random direction
                    double turnRange = (double)turnRangeMax - (double)(turnRangeMin);
                    double sampleTurn = randomSeed.NextDouble();
                    double scaledTurn = (sampleTurn * turnRange) + (turnRangeMin);
                    float turnDegree = (float)scaledTurn;
                    transform.LookAt(target.transform);
                    transform.Rotate(0.0f, turnDegree, 0.0f);
                }
            }
        }

        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        string enemyAlert = "";

        // Determine where the target is in reference to the player
        Vector3 enemyPos = Quaternion.Inverse(target.transform.rotation) * (transform.position - target.transform.position);
        bool easilyViewable = false;

        Vector3 screenPoint = camera.WorldToViewportPoint(transform.position);
        if (screenPoint.z > 0 && screenPoint.x > 0.1 && screenPoint.x < 0.9 && screenPoint.y > 0.1 && screenPoint.y < 0.9)
        {
            easilyViewable = true;
        }
        else
        {
            easilyViewable = false;
        }

        if (Vector3.Distance(target.transform.position, transform.position) < 3f)
        {
            enemyAlert += "ENEMY IN RANGE";
        }

        if (!easilyViewable)
        {
            if (enemyPos.z < 0)
            {
                enemyAlert += "\n\nENEMY TO REAR";
            }
            else if (enemyPos.x > 0)
            {
                enemyAlert += "\n\nENEMY TO RIGHT";
            }
            else if (enemyPos.x < 0)
            {
                enemyAlert += "\n\nENEMY TO LEFT";
            }
        }

        AlertLog.write(enemyAlert);
    }
}
