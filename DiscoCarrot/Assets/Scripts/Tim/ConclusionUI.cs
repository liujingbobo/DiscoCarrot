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

        var score0 = (context.runTimeValues.harvestedCarrots[0] * Config.GetScoreByCarrotLevel(CarrotLevel.Disco));
        var score1 = (context.runTimeValues.harvestedCarrots[1] * Config.GetScoreByCarrotLevel(CarrotLevel.Muscle));
        var score2 = (context.runTimeValues.harvestedCarrots[2] * Config.GetScoreByCarrotLevel(CarrotLevel.Normal));
        var score3 = (context.runTimeValues.harvestedCarrots[3] * Config.GetScoreByCarrotLevel(CarrotLevel.Bad));
        var score4 = (context.runTimeValues.missedCount * Config.MISSED_DEDUCT_SCORE);
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
            DOTween.Sequence().AppendInterval(1)
                .AppendCallback(() =>
                {
                    CarrotBarImages[0].sprite = carrotSprites[1];
                    StarImages[0].gameObject.SetActive(true);
                    StarImages[1].gameObject.SetActive(true);
                })
                .Append(StarImages[0].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic))
                .Append(StarImages[1].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic));
        }
        //star2
        if (grade >= 2)
        {
            DOTween.Sequence().AppendInterval(2)
                .AppendCallback(() =>
                {
                    CarrotBarImages[1].sprite = carrotSprites[1];
                    StarImages[2].gameObject.SetActive(true);
                    StarImages[3].gameObject.SetActive(true);
                })
                .Append(StarImages[2].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic))
                .Append(StarImages[3].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic));
        }
        //star3
        if (grade >= 3)
        {
            DOTween.Sequence().AppendInterval(3)
                .AppendCallback(() =>
                {
                    CarrotBarImages[2].sprite = carrotSprites[1];
                    StarImages[4].gameObject.SetActive(true);
                    StarImages[5].gameObject.SetActive(true);
                })
                .Append(StarImages[4].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic))
                .Append(StarImages[5].transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic));
        }

        DOTween.Sequence().AppendInterval(4).AppendCallback(() =>
        {
            againButton.transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic);
            homeButton.transform.DOScale(1, 0.8f).SetEase(Ease.InOutElastic);
        });
    }
    
    
    
}