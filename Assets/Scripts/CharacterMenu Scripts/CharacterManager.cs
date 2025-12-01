using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public Camera mainCamera;
    public string[] characterNames;

    public string player1Character;
    public string player2Character;

    public float legPower1;
    public float cooldown1;
    public float steerAngle1;

    public float legPower2;
    public float cooldown2;
    public float steerAngle2;

    
    List<Character> availableCharacters = new List<Character>();

    //actually these two might not be needed
    bool isAnimating; //if camera is moving
    float transitionTime = 0.5f;//how fast we want the camera movements to be


    void Start()
    {
        for (int i = 0; i < characterNames.Length; i++)
        {
            GameObject guy = GameObject.Find(characterNames[i]);
            if (guy != null)
            {
                Character theGuy = guy.GetComponent<Character>();
                if (theGuy != null)
                    availableCharacters.Add(theGuy);
            }
        }

        //StartCoroutine(Run());
    }



    //helper coroutine for moving camera stuff
    IEnumerator MoveCamera(Transform cam, Transform target, float targetFOV, bool activating)
    {

        isAnimating = true;
       


        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;
        float startFOV = mainCamera.fieldOfView;

        float elapsed = 0f;

        while (elapsed < transitionTime)
        {
            float t = elapsed/transitionTime;
            t = t*t*(3f-2f*t);//fast at the start, slows down, we can try different curves 

            cam.position = Vector3.Lerp(startPos, target.position, t);
            cam.rotation = Quaternion.Slerp(startRot, target.rotation, t);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);

            

            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.position = target.position;
        cam.rotation = target.rotation;
        mainCamera.fieldOfView = targetFOV;

        isAnimating = false;
    }





}
