using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Settings")] 
    public float positionLerpSpeed = 1f;
    public float rotationLerpSpeed = 1f;
    public Transform target;
    
    
    [Header("References")]
    public Camera myCamera;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, target.position,
            positionLerpSpeed * Time.deltaTime);
        
        myCamera.transform.rotation = Quaternion.Lerp(myCamera.transform.rotation, target.transform.rotation, rotationLerpSpeed * Time.deltaTime);
    }
}
