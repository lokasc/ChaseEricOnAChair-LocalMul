using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class CharacterManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera overviewCamera;
    //public string[] characterNames;

    public string player1Character;
    public string player2Character;

    public float legPower1;
    public float cooldown1;
    public float steerAngle1;

    public float legPower2;
    public float cooldown2;
    public float steerAngle2;

    public string[] characterNames;    
    List<Character> availableCharacters = new List<Character>();

    //actually these two might not be needed
    bool isAnimating; //if camera is moving
    float transitionTime = 0.5f;//how fast we want the camera movements to be
    public int currentIndex = -1;//-1 is overview


    //inputs

     public CharacterSelectControls input; 


    //overview camera movement
    float overviewOscillateSpeed = 0.5f;
    float overviewMinZ = 52f;
    float overviewMaxZ = 68f;

    float overviewOscillateTime = 0f;


    /*void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }*/

    void Start()
    {
        for (int i = 0; i < characterNames.Length; i++)
        {
            GameObject b = GameObject.Find(characterNames[i]);
            if (b != null)
            {
                Character bc = b.GetComponent<Character>();
                if (bc != null)
                    availableCharacters.Add(bc);
            }
        }

        MoveToOverview();
    }

    void MoveToOverview()
    {
        if (overviewCamera == null) return;
        StartCoroutine(MoveCamera(mainCamera.transform, overviewCamera.transform, overviewCamera.fieldOfView));
        currentIndex = -1;



        overviewOscillateTime = 0f;
    }


    void Update()
    {
        if (isAnimating) return;

        
        Vector2 nav = input.GetNavigate();
        if (currentIndex != -1)//if not zoomed in
        {
            if (nav.x > 0.5f)
            {
                int next = (currentIndex + 1) % availableCharacters.Count;
                MoveToCharacter(next);
                return;
            }
            if (nav.x < -0.5f)
            {
                int prev = (currentIndex - 1 + availableCharacters.Count) % availableCharacters.Count;
                MoveToCharacter(prev);
                return;
            }
        }

        
        if (input.SelectPressed())//select button
        {
            if (currentIndex == -1 && availableCharacters.Count > 0)//select from overview into first character
            {
                MoveToCharacter(0);
            }
            else if (currentIndex != -1)//selecting a character
            {
                //TODO!!!! PLAYER TWO STUFF!!!
                Character selected = availableCharacters[currentIndex];

                CharacterInfo info = CharacterInfo.Instance;

                info.player1Character = selected.name;
                info.legPower1 = selected.legPower;
                info.cooldown1 = selected.cooldown;
                info.steerAngle1 = selected.steerAngle;

                
                UnityEngine.SceneManagement.SceneManager.LoadScene("Map1");
            }
        }

        if (input.CancelPressed())//back out back to main screen overview
        {
            if (currentIndex != -1)
            {
                MoveToOverview();
            }
        }

        //overview camera movement
        if (currentIndex == -1)
        {
            OscillateOverviewCamera();
        }

    }

    void MoveToCharacter(int index)
    {
        if (index < 0 || index >= availableCharacters.Count) return;

        Camera targetCam = availableCharacters[index].DefaultCamera;
        StartCoroutine(MoveCamera(mainCamera.transform, targetCam.transform, targetCam.fieldOfView));
        currentIndex = index;
    }

    //helper coroutine for moving camera stuff
    IEnumerator MoveCamera(Transform cam, Transform target, float targetFOV)
    {
        isAnimating = true;

        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;
        float startFOV = mainCamera.fieldOfView;

        float elapsed = 0f;

        while (elapsed < transitionTime)
        {
            float t = elapsed / transitionTime;
            t = t * t * (3f - 2f * t);

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

//for moving camera back and forth during title screen
    void OscillateOverviewCamera()
    {
        if (overviewCamera == null) return;

        overviewOscillateTime += Time.deltaTime * overviewOscillateSpeed;

        float z = Mathf.Lerp(
            overviewMinZ,
            overviewMaxZ,
            (Mathf.Sin(overviewOscillateTime) + 1f) * 0.5f
        );

        Vector3 pos = mainCamera.transform.position;
        pos.z = z;
        mainCamera.transform.position = pos;
    }


}
