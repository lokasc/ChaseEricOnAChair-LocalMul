using UnityEngine;
using UnityEngine.Splines;

public class MoveBasedOnSpline : MonoBehaviour
{
    public SplineContainer spline;

    public float speed;

    public float combinedSpeed;
    float splineLength,distancePercentage;
    void Start()
    {
        combinedSpeed = speed;
        splineLength = spline.CalculateLength();
    }

    void Update()
    {
        distancePercentage += combinedSpeed * Time.deltaTime/splineLength;

        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;
        
        if (distancePercentage > 1f)
        {
            distancePercentage = 0f;
        }
        
        
        // Rotate Eric based on the Spline's Forward Direction!
        // Get the position between curernt and next position = direction
        
        Vector3 lookAheadPosition = spline.EvaluatePosition(distancePercentage + 0.005f);


        transform.forward = lookAheadPosition - currentPosition;
    }
}
