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
    
    [ContextMenu("testOpenConclusionUI")]
    public void OpenConclusionUI()
    {
        var context = GameManager.singleton.sharedContext;

        var count0 = context.harvestedCarrots[0];
        var count1 = context.harvestedCarrots[1];
        var count2 = context.harvestedCarrots[2];
        var count3 = context.harvestedCarrots[3];
        var count4 = context.missedCount;
        carrotAndMissCountTexts[0].text = count0.ToString();
        carrotAndMissCountTexts[1].text = count1.ToString();
        carrotAndMissCountTexts[2].text = count2.ToString();
        carrotAndMissCountTexts[3].text = count3.ToString();
        carrotAndMissCountTexts[4].text = count4.ToString();

        var score0 = (context.harvestedCarrots[0] * Config.GetScoreByCarrotLevel(CarrotLevel.Disco));
        var score1 = (context.harvestedCarrots[1] * Config.GetScoreByCarrotLevel(CarrotLevel.Muscle));
        var score2 = (context.harvestedCarrots[2] * Config.GetScoreByCarrotLevel(CarrotLevel.Normal));
        var score3 = (context.harvestedCarrots[3] * Config.GetScoreByCarrotLevel(CarrotLevel.Bad));
        var score4 = (context.missedCount * Config.MISSED_DEDUCT_SCORE);
        carrotAndMissScoreTexts[0].text = score0.ToString();
        carrotAndMissScoreTexts[1].text = score1.ToString();
        carrotAndMissScoreTexts[2].text = score2.ToString();
        carrotAndMissScoreTexts[3].text = score3.ToString();
        carrotAndMissScoreTexts[4].text = score4.ToString();

        //calculate Harvested Carrot Score
        var totalScore = score0 + score1 + score2 + score3 + score4;
        totalScoreText.text = totalScore.ToString();
        
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
        
        var sequence = DOTween.Sequence();
        if (grade >= 1)
        {
            sequence.AppendCallback(() =>
            {
                CarrotBarImages[0].sprite = carrotSprites[1];
                StarImages[0].transform.localScale = Vector3.zero;
                StarImages[0].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic);
                StarImages[1].transform.localScale = Vector3.zero;
                StarImages[1].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic);
            }).AppendInterval(0.5f);
        }
        if (grade >= 2)
        {
            sequence.AppendCallback(() =>
            {
                CarrotBarImages[1].sprite = carrotSprites[1];
                StarImages[2].transform.localScale = Vector3.zero;
                StarImages[2].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic);
                StarImages[3].transform.localScale = Vector3.zero;
                StarImages[3].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic);
            }).AppendInterval(0.5f);
        }
        if (grade >= 3)
        {
            sequence.AppendCallback(() =>
            {
                CarrotBarImages[2].sprite = carrotSprites[1];
                StarImages[4].transform.localScale = Vector3.zero;
                StarImages[4].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic);
                StarImages[5].transform.localScale = Vector3.zero;
                StarImages[5].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic);
            }).AppendInterval(0.5f);
        }
        sequence.AppendCallback(() =>
        {
            againButton.transform.localScale = Vector3.zero;
            againButton.transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic);
            homeButton.transform.localScale = Vector3.zero;
            homeButton.transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic);
        }).AppendInterval(0.5f);
    }
    
    
    
}