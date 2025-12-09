using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class VictoryUIManager : MonoBehaviour
{
    public Image flags;
    public float flagMoveAmount = 80f;
    public float flagMoveSpeed = 0.5f;

    public RectTransform victoryText;
    public RectTransform finalScore;
    public float textScaleAmount = 0.2f;
    public float textScaleSpeed = 1f;

    [Header("score stuff")]
    public TextMeshProUGUI score1;
    public TextMeshProUGUI score2;
    public Transform scoreHolder;
    public float scoreIncrement = 1f;
    public float scoreInterval = 0.05f;

    public Image B;
    public float BScaleDownSpeed = 2f;

    [Header("BOTTOMZIMMERTEXT")] public TextMeshProUGUI zimmerText;
    
    private Vector3 scoreHolderInitialScale;
    private Vector3 flagsInitialPos;
    private Vector3 victoryTextInitialScale;
    private Vector3 finalScoreInitialScale;
    private Vector3 BInitialScale;

    private bool startBScaleDown = false;
    private GameManager gm;
    
    void Start()
    {
/*store B and scoreholder size, set initial size, and 0 for score boxes*/
        scoreHolderInitialScale = scoreHolder.localScale;
        scoreHolder.localScale = Vector3.one * 0.1f;

        score1.text = "0";
        score2.text = "0";

        flagsInitialPos = flags.rectTransform.anchoredPosition;
        victoryTextInitialScale = victoryText.localScale;
        finalScoreInitialScale = finalScore.localScale;

        BInitialScale = B.rectTransform.localScale;
        B.gameObject.SetActive(false);
        StartCoroutine(AnimateScores());
        
        // Set name of winning player
        gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            zimmerText.text = gm.winningPlayer.characterName;
        }
       
    }

    void Update()
    {
        AnimateFlags();
        AnimateText();


        //B shrink triggered
        if (startBScaleDown)
        {
            Vector3 currentScale = B.rectTransform.localScale;
            B.rectTransform.localScale = Vector3.Lerp(currentScale, BInitialScale, BScaleDownSpeed * Time.deltaTime);

            if (Vector3.Distance(B.rectTransform.localScale, BInitialScale) < 0.001f)
            {
                B.rectTransform.localScale = BInitialScale;
                //LUCAS, IT WOULD BE HERE WHERE YOU WOULD START CHECKING FOR PLAYER INPUT
                startBScaleDown = false;
            }
        }
    }

    private void AnimateFlags()
    {
        float offset = Mathf.Sin(Time.time * flagMoveSpeed) * flagMoveAmount;
        flags.rectTransform.anchoredPosition = flagsInitialPos + new Vector3(offset, offset, 0);
    }

    private void AnimateText()
    {
        float scaleOffset = Mathf.Sin(Time.time * textScaleSpeed) * textScaleAmount;
        victoryText.localScale = victoryTextInitialScale * (1 + scaleOffset);
        finalScore.localScale = finalScoreInitialScale * (1 + scaleOffset);
    }

    private IEnumerator AnimateScores()
    {
        float displayedScore = 0f;

        while (true)
        {
            yield return new WaitForSeconds(scoreInterval);

            displayedScore += scoreIncrement;

            if (displayedScore >= 100f)
            {
                score1.text = "85";
                score2.text = "85";
                scoreHolder.localScale = scoreHolderInitialScale;


                B.gameObject.SetActive(true);
                B.rectTransform.localScale = Vector3.one * 100f;
                startBScaleDown = true;

                break;
            }

            score1.text = Mathf.FloorToInt(displayedScore).ToString();
            score2.text = Mathf.FloorToInt(displayedScore).ToString();

            scoreHolder.localScale += Vector3.one * 0.1f;
        }
    }
}
