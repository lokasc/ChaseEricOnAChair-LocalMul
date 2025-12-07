using UnityEngine;

public class CMUI : MonoBehaviour
{
    public GameObject player1UI;
    public GameObject player2UI;
    public GameObject arrowsUI; 

    RectTransform p1Rect;
    RectTransform p2Rect;

    Vector2 p1TargetPos;
    Vector2 p2TargetPos;

    CharacterManager manager;

    bool p1Active;
    bool p2Active;

    public float slideDistance = 1000f;
    public float slideDuration = 0.8f;

    void Awake()
    {
        p1Rect = player1UI.GetComponent<RectTransform>();
        p2Rect = player2UI.GetComponent<RectTransform>();

        p1TargetPos = p1Rect.anchoredPosition;
        p2TargetPos = p2Rect.anchoredPosition;

        player1UI.SetActive(false);
        player2UI.SetActive(false);

        if (arrowsUI != null)
        {
            arrowsUI.SetActive(false);
        }
    }

/*
       void PrintChar()
{
    string p1 = "None";
    string p2 = "None";

    if (manager.characterSelectors.Count >= 1)
    {
        var cs = manager.characterSelectors[0].GetComponent<CharacterSelector>();
        if (cs.isSelected) p1 = cs.characterName;
    }

    if (manager.characterSelectors.Count >= 2)
    {
        var cs = manager.characterSelectors[1].GetComponent<CharacterSelector>();
        if (cs.isSelected) p2 = cs.characterName;
    }

    Debug.Log("P1 Character: " + p1);
    Debug.Log("P2 Character: " + p2);
}
*/
    void Start()
    {
        manager = FindObjectOfType<CharacterManager>();
        if (manager != null && arrowsUI != null)
        {
            arrowsUI.SetActive(manager.currentIndex != -1);
        }
    }

    void Update()
    {
        if (manager == null) return;

        //arrows UI stuffs
        if (arrowsUI != null)
        {
            arrowsUI.SetActive(manager.currentIndex != -1);
        }

        if (manager.characterSelectors.Count >= 1 && !p1Active)
        {
            var c = manager.characterSelectors[0];
            if (c.SelectPressed() || c.CancelPressed())
            {
                p1Active = true;
                StartCoroutine(SlideIn(player1UI, p1Rect, p1TargetPos, -slideDistance));
            }
        }

        if (manager.characterSelectors.Count >= 2 && !p2Active)
        {
            var c = manager.characterSelectors[1];
            if (c.SelectPressed() || c.CancelPressed())
            {
                p2Active = true;
                StartCoroutine(SlideIn(player2UI, p2Rect, p2TargetPos, slideDistance));
            }
        }
    }

    System.Collections.IEnumerator SlideIn(GameObject ui, RectTransform rect, Vector2 target, float fromOffset)
    {
        Vector2 startPos = new Vector2(target.x + fromOffset, target.y);
        rect.anchoredPosition = startPos;
        ui.SetActive(true);

        float t = 0f;

        while (t < slideDuration)
        {
            float x = t / slideDuration;
            x = x * x * (3f - 2f * x);
            rect.anchoredPosition = Vector2.Lerp(startPos, target, x);

            t += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = target;
    }
}
