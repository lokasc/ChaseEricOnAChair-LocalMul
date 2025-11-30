using UnityEngine;
using UnityEngine.Splines;

public class MoveBasedOnSpline : MonoBehaviour
{
    public SplineContainer spline;

    public float speed;

    float splineLength,distancePercentage;
    void Start()
    {
        splineLength = spline.CalculateLength();
    }

    void Update()
    {
        distancePercentage += speed * Time.deltaTime/splineLength;

        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        if (distancePercentage > 1f)
        {
            distancePercentage = 0f;
        }
    }
}
