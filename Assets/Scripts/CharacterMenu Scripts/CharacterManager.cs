using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public Camera mainCamera;
    public string[] characterNames;

    public string player1Character;
    public string player2Character;
    
    List<Character> availableCharacters = new List<Character>();

    bool isAnimating; //if camera is moving


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

        if (!activating) ActivatedBoolean = false;
        isAnimating = false;
    }





}
