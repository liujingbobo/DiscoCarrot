using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConclusionUI : MonoBehaviour
{
    public Sprite[] carrotSprites;
    
    public Image[] CarrotBarImages;
    public Image[] StarImages;
    public TMP_Text[] carrotAndMissCountTexts;
    public Text[] carrotAndMissScoreTexts;
    public Text totalScoreText;
    public Button againButton;
    public Button homeButton;

    public Image BackGround;
    public Image PanelRoot;

    public void Reset()
    {
        BackGround.color = new Color(BackGround.color.r,BackGround.color.g,BackGround.color.b ,0);
        PanelRoot.transform.localScale = Vector3.zero;
        againButton.transform.localScale = Vector3.zero;
        homeButton.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        totalScoreText.text = recordTotalScore.ToString();
    }

    private int[] recordScores = new int[5];
    private int recordTotalScore = 0;
    [ContextMenu("testOpenConclusionUI")]
    public void OpenConclusionUI()
    {
        BackGround.color = new Color(BackGround.color.r,BackGround.color.g,BackGround.color.b ,0);
        BackGround.DOFade(0, 0);
        BackGround.DOFade(0.6f, 1);
        PanelRoot.transform.localScale = Vector3.zero;
        PanelRoot.transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic);
        
        var context = GameManager.singleton.sharedContext;

        var count0 = context.runTimeValues.harvestedCarrots[0];
        var count1 = context.runTimeValues.harvestedCarrots[1];
        var count2 = context.runTimeValues.harvestedCarrots[2];
        var count3 = context.runTimeValues.harvestedCarrots[3];
        var count4 = context.runTimeValues.missedCount;
        carrotAndMissCountTexts[0].text = count0.ToString();
        carrotAndMissCountTexts[1].text = count1.ToString();
        carrotAndMissCountTexts[2].text = count2.ToString();
        carrotAndMissCountTexts[3].text = count3.ToString();
        carrotAndMissCountTexts[4].text = count4.ToString();

        for (int i = 0; i < recordScores.Length; i++)
        {
            recordScores[i] = 0;
        }

        int[] tmpScore = new int[5];
        tmpScore[0] = (count0 * Config.GetScoreByCarrotLevel(CarrotLevel.Disco));
        tmpScore[1] = (count1 * Config.GetScoreByCarrotLevel(CarrotLevel.Muscle));
        tmpScore[2] = (count2 * Config.GetScoreByCarrotLevel(CarrotLevel.Normal));
        tmpScore[3] = (count3 * Config.GetScoreByCarrotLevel(CarrotLevel.Bad));
        tmpScore[4] = (context.runTimeValues.missedCount * Config.MISSED_DEDUCT_SCORE);
        carrotAndMissScoreTexts[0].text = tmpScore[0].ToString();
        carrotAndMissScoreTexts[1].text = tmpScore[1].ToString();
        carrotAndMissScoreTexts[2].text = tmpScore[2].ToString();
        carrotAndMissScoreTexts[3].text = tmpScore[3].ToString();
        carrotAndMissScoreTexts[4].text = tmpScore[4].ToString();

        float secondTmp = 0;
        //added counting up effect
        /*for (int i = 0; i < recordScores.Length; i++)
        {
            DOTween.To(() => recordScores[i], x => recordScores[i] = x, tmpScore[i], 1);
            Debug.Log($"Timtes {recordScores[i]} {tmpScore[i]}");
        }
        secondTmp += 1;*/
        
        //calculate Harvested Carrot Score
        totalScoreText.text = 0.ToString();
        recordTotalScore = 0;
        var totalScore = tmpScore[0] + tmpScore[1] + tmpScore[2] + tmpScore[3] + tmpScore[4];
        //show count up score
        DOTween.Sequence().AppendInterval(secondTmp)
            .Append(DOTween.To(() => recordTotalScore, x => recordTotalScore = x, totalScore, 1));
        secondTmp += 1;
        
        //calculate grade
        var grade = Config.GetGradeFromTotalScore(totalScore);
        foreach (var image in CarrotBarImages)
        {
            image.sprite = carrotSprites[0];
        }
        foreach (var image in StarImages)
        {
            image.gameObject.SetActive(false);
        }

        //reset
        CarrotBarImages[0].sprite = carrotSprites[0];
        StarImages[0].transform.localScale = Vector3.zero;
        StarImages[1].transform.localScale = Vector3.zero;
        CarrotBarImages[1].sprite = carrotSprites[0];
        StarImages[2].transform.localScale = Vector3.zero;
        StarImages[3].transform.localScale = Vector3.zero;
        CarrotBarImages[2].sprite = carrotSprites[0];
        StarImages[4].transform.localScale = Vector3.zero;
        StarImages[5].transform.localScale = Vector3.zero;
        
        //star1
        if (grade >= 1)
        {
            DOTween.Sequence().AppendInterval(secondTmp)
                .AppendCallback(() =>
                {
                    CarrotBarImages[0].sprite = carrotSprites[1];
                    StarImages[0].gameObject.SetActive(true);
                    StarImages[1].gameObject.SetActive(true);
                })
                .Append(StarImages[0].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic))
                .Append(StarImages[1].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic));
            secondTmp += 0.5f;
        }
        //star2
        if (grade >= 2)
        {
            DOTween.Sequence().AppendInterval(secondTmp)
                .AppendCallback(() =>
                {
                    CarrotBarImages[1].sprite = carrotSprites[1];
                    StarImages[2].gameObject.SetActive(true);
                    StarImages[3].gameObject.SetActive(true);
                })
                .Append(StarImages[2].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic))
                .Append(StarImages[3].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic));
            secondTmp += 0.5f;
        }
        //star3
        if (grade >= 3)
        {
            DOTween.Sequence().AppendInterval(secondTmp)
                .AppendCallback(() =>
                {
                    CarrotBarImages[2].sprite = carrotSprites[1];
                    StarImages[4].gameObject.SetActive(true);
                    StarImages[5].gameObject.SetActive(true);
                })
                .Append(StarImages[4].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic))
                .Append(StarImages[5].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic));
            secondTmp += 0.5f;
        }

        DOTween.Sequence().AppendInterval(secondTmp).AppendCallback(() =>
        {
            againButton.transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic);
            homeButton.transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic);
        });
    }
    
    
    
}