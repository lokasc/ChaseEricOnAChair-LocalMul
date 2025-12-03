using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.Events;

public class ChairUI : MonoBehaviour
{
    public Chair myChair;
    
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI countDownText;
    public TextMeshProUGUI lapText;
    public UnityEvent onCountDownComplete;

    private Vector3 countDownInitialScale;

    private void Start()
    {
        countDownInitialScale = countDownText.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        positionText.text = GameManager.Instance.GetCurrentPosition(myChair).ToString();
        lapText.text = "Lap " + GameManager.Instance.GetCurrentLaps(myChair).ToString() + "/3";
    }

    public void StartCountDown()
    {
        Sequence s = DOTween.Sequence();
        s.Append(countDownText.DOFade(0, 1f))
            .Join(countDownText.transform.DOScale(Vector3.one, 1))
            .AppendCallback(() =>
            {
                countDownText.transform.localScale = countDownInitialScale;
                countDownText.text = "2";
                countDownText.alpha = 1;
            })
            .Append(countDownText.DOFade(0, 1f))
            .Join(countDownText.transform.DOScale(Vector3.one, 1))
            .AppendCallback(() =>
            {
                countDownText.transform.localScale = countDownInitialScale;
                countDownText.text = "1";
                countDownText.alpha = 1;
            })
            .Append(countDownText.DOFade(0, 1f))
            .Join(countDownText.transform.DOScale(Vector3.one, 1))
            .AppendCallback(() =>
            {
                // What happens when it completes?, event will decide
                onCountDownComplete.Invoke();
            });
    }
    
    public void HideUI(bool isOn = false) => transform.gameObject.SetActive(isOn);
}
