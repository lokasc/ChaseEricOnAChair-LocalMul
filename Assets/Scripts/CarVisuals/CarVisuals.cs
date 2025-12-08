using UnityEngine;
using System.Collections;

public class CarVisuals : MonoBehaviour
{
    public Transform model;

    public float stretchAmount = 0.8f; 
    public float returnSpeed = 10f; 

    public GameObject particleContainer;
    public float particleDuration = 0.2f;

    private Chair chair;
    private Vector3 defaultScale;
    private bool prevCanMash;
    private bool isStretched;

    void Awake()
    {
        chair = GetComponent<Chair>();
        defaultScale = model.localScale;
        prevCanMash = chair.GetCanMash();

        if (particleContainer != null)
            particleContainer.SetActive(false);
    }

    void Update()
    {
        if (!chair.canDrive)
            return;



        //bool mash = chair.playerInput.AccelPressed();
        bool cooldownReady = chair.GetCanMash();
        
        //Debug.Log(mash);
        //Debug.Log(cooldownReady);


        //if it was true, and is false now, it means button pressed
        if (prevCanMash == true && cooldownReady == false)
        {
            ApplyStretch();
        }
        prevCanMash = cooldownReady;


        if (isStretched)
        {
            model.localScale = Vector3.Lerp(
                model.localScale,
                defaultScale,
                returnSpeed * Time.deltaTime
            );

            if (Vector3.Distance(model.localScale, defaultScale) < 0.001f)
            {
                model.localScale = defaultScale;
                isStretched = false;
            }
        }
    }

    private void ApplyStretch()
    {
        Debug.Log("Mashed!");
        Vector3 s = defaultScale;
        s.y = stretchAmount;
        model.localScale = s;
        isStretched = true;

        if (particleContainer != null)
            StartCoroutine(ShowParticles());
    }

    private IEnumerator ShowParticles()
    {
        particleContainer.SetActive(true);
        yield return new WaitForSeconds(particleDuration);
        particleContainer.SetActive(false);
    }
}
