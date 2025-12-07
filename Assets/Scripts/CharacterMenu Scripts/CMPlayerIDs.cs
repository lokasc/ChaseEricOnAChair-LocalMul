using UnityEngine;
using UnityEngine.UI;

public class CMPlayerIDs : MonoBehaviour
{
    public Image P1Char;
    public Image P2Char;

    public float moveTime = 0.5f;
    public float yOffset = 150f; 

    private string lastP1Name = "";
    private string lastP2Name = "";

    private RectTransform p1Rect;
    private RectTransform p2Rect;

    private float p1Lerp = 1f;
    private float p2Lerp = 1f;

    private float p1StartY;
    private float p2StartY;

    void Awake()
    {
        if (P1Char != null)
        {
            p1Rect = P1Char.GetComponent<RectTransform>();
            p1StartY = p1Rect.localPosition.y;
            Vector3 pos = p1Rect.localPosition;
            pos.y = p1StartY - yOffset;
            p1Rect.localPosition = pos;
        }

        if (P2Char != null)
        {
            p2Rect = P2Char.GetComponent<RectTransform>();
            p2StartY = p2Rect.localPosition.y;
            Vector3 pos = p2Rect.localPosition;
            pos.y = p2StartY - yOffset;
            p2Rect.localPosition = pos;
        }
    }

    void Update()
    {
        var manager = FindObjectOfType<CharacterManager>();
        if (manager == null) return;

        string p1Name = "";
        string p2Name = "";

        if (manager.characterSelectors.Count >= 1)
        {
            var cs = manager.characterSelectors[0].GetComponent<CharacterSelector>();
            if (cs.isSelected) p1Name = cs.characterName;
        }

        if (manager.characterSelectors.Count >= 2)
        {
            var cs = manager.characterSelectors[1].GetComponent<CharacterSelector>();
            if (cs.isSelected) p2Name = cs.characterName;
        }

        UpdatePlayerID(P1Char, ref lastP1Name, p1Name, ref p1Lerp);
        UpdatePlayerID(P2Char, ref lastP2Name, p2Name, ref p2Lerp);

        MoveUI(p1Rect, ref p1Lerp, p1StartY, p1Name);
        MoveUI(p2Rect, ref p2Lerp, p2StartY, p2Name);
    }

    void UpdatePlayerID(Image img, ref string lastName, string newName, ref float lerp)
    {
        if (img == null) return;

        if (lastName != newName)
        {
            if (!string.IsNullOrEmpty(newName))
            {
                Sprite s = Resources.Load<Sprite>("CharacterIDs/" + newName + "ID");
                if (s != null) img.sprite = s;
            }
            lerp = 0f; 
            lastName = newName;
        }
    }

    void MoveUI(RectTransform rect, ref float lerp, float startY, string currentName)
    {
        if (rect == null) return;

        lerp += Time.deltaTime / moveTime;
        lerp = Mathf.Clamp01(lerp);

        float endY = string.IsNullOrEmpty(currentName) ? startY - yOffset : startY;

        float newY = Mathf.SmoothStep(rect.localPosition.y, endY, lerp);
        Vector3 pos = rect.localPosition;
        pos.y = newY;
        rect.localPosition = pos;
    }
}
