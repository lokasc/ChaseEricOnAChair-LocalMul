using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChairUI : MonoBehaviour
{
    public Chair myChair;
    
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI countDownText;
    public TextMeshProUGUI lapText;


    public TextMeshProUGUI coin1;
    public TextMeshProUGUI coin2;
    public UnityEvent onCountDownComplete;

    private Vector3 countDownInitialScale;

    [Header("ZimmerUI")] 
    public Image winnie1;
    public Image winnie2;
    public Image winnie3;
    
    
    private void Start()
    {
        countDownInitialScale = winnie1.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        positionText.text = KartManager.Instance.GetCurrentPosition(myChair).ToString();
        //lapText.text = "Lap " + KartManager.Instance.GetCurrentLaps(myChair).ToString() + "/3";
        //Debug.Log(KartManager.Instance.GetCoins(myChair).ToString());
        coin1.text = KartManager.Instance.GetCoins(myChair).ToString() + "";
        coin2.text = KartManager.Instance.GetCoins(myChair).ToString() + "";
    }

    public void StartCountDown()
    {
        winnie1.gameObject.SetActive(true);
        Sequence s = DOTween.Sequence();
        s.Append(winnie1.DOFade(0, 1f).SetEase(Ease.InQuad))
            .Join(winnie1.transform.DOScale(Vector3.one, 1.0f))
            .AppendCallback(() =>
            {
                winnie1.gameObject.SetActive(false);
                winnie2.gameObject.SetActive(true);
                winnie2.transform.localScale = countDownInitialScale;
            })
            .Append(winnie2.DOFade(0, 1f).SetEase(Ease.InQuad))
            .Join(winnie2.transform.DOScale(Vector3.one, 1.0f))
            .AppendCallback(() =>
            {
                winnie2.gameObject.SetActive(false);
                winnie3.gameObject.SetActive(true);
                winnie3.transform.localScale = countDownInitialScale;
            })
            .Append(winnie3.DOFade(0, 1f).SetEase(Ease.InQuad))
            .Join(winnie3.transform.DOScale(Vector3.one, 1.0f))
            .AppendCallback(() =>
            {
                // What happens when it completes?, event will decide
                winnie3.gameObject.SetActive(false);
                onCountDownComplete.Invoke();
            });
    }
    
    public void HideUI(bool isOn = false) => transform.gameObject.SetActive(isOn);
}
