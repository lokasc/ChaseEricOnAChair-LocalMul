using UnityEngine;
using UnityEngine.Splines;

public class MoveBasedOnSpline : MonoBehaviour
{
    public SplineContainer spline;

    public float speed;

    public float currentSpeed; // current speed..
    float splineLength,distancePercentage;
    
    void Start()
    {
        currentSpeed = speed;
        splineLength = spline.CalculateLength();
        distancePercentage += currentSpeed * Time.deltaTime/splineLength;

        
    }

    void Update()
    {
        distancePercentage += currentSpeed * Time.deltaTime/splineLength;

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
