using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TimPlugin;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public GameManagerContext sharedContext = new GameManagerContext();
    public SimpleStateMachine<GameLoopState, GameManagerContext> stateMachine =
        new SimpleStateMachine<GameLoopState, GameManagerContext>();

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        stateMachine.sharedContext = sharedContext;
        stateMachine.AddStateInstance(GameLoopState.Logo, new LogoSimpleState(), true);
        stateMachine.AddStateInstance(GameLoopState.SongPicker, new SongPickerState(), true);
        stateMachine.AddStateInstance(GameLoopState.GameReadyStart, new GameReadyStartState(), true);
        stateMachine.AddStateInstance(GameLoopState.GameRunning, new GameRunningState(), true);
        stateMachine.AddStateInstance(GameLoopState.GameEnd, new GameEndState(), true);
        stateMachine.AddStateInstance(GameLoopState.Conclusion, new ConclusionState(), true);
        
        stateMachine.SwitchToState(GameLoopState.Logo);
    }

    
    
    
    [Serializable]
    public struct GameManagerContext
    {
        //general references
        public Player player;
        public FarmTile[] farmTiles;
        
        //logo screen
        public Button startButton;
        public Canvas logoCanvas;
        
        //song picker
        public Button pickSongButton;
        public Canvas pickSongCanvas;
        
        //GameReadyStart
        public Image readyTextImage;
        public Image goTextImage;

        //GameEnd
        public Image endTextImage;

        //Conclusion
        public Canvas conclusionCanvas;
        public Button againButton;
        public Button homeButton;
        
    }
    
    public enum GameLoopState
    {
        Logo,
        SongPicker,
        GameReadyStart,
        GameRunning,
        GameEnd,
        Conclusion,
    }

    public class LogoSimpleState : SimpleStateInstance<GameLoopState, GameManagerContext>
    {
        public override void EnterState()
        {
            base.EnterState();
            stateMachine.sharedContext.logoCanvas.gameObject.SetActive(true);
            stateMachine.sharedContext.startButton.onClick.AddListener(OnButtonClick);
        }
        public override void ExitState()
        {
            base.ExitState();
            stateMachine.sharedContext.logoCanvas.gameObject.SetActive(false);
            stateMachine.sharedContext.startButton.onClick.RemoveListener(OnButtonClick);
        }

        void OnButtonClick()
        {
            SwitchToState(GameLoopState.SongPicker);
        }
    }
    public class SongPickerState : SimpleStateInstance<GameLoopState, GameManagerContext>
    {
        public override void EnterState()
        {
            base.EnterState();
            stateMachine.sharedContext.pickSongCanvas.gameObject.SetActive(true);
            stateMachine.sharedContext.pickSongButton.onClick.AddListener(OnButtonClick);
        }
        public override void ExitState()
        {
            base.ExitState();
            stateMachine.sharedContext.pickSongCanvas.gameObject.SetActive(false);
            stateMachine.sharedContext.pickSongButton.onClick.RemoveListener(OnButtonClick);
        }

        void OnButtonClick()
        {
            SwitchToState(GameLoopState.GameReadyStart);
        }
    }
    public class GameReadyStartState : SimpleStateInstance<GameLoopState, GameManagerContext>
    {
        public override void EnterState()
        {
            base.EnterState();
            //ResetEverything
            stateMachine.sharedContext.player.ResetPlayer();
            foreach (var t in stateMachine.sharedContext.farmTiles)
            {
                t.ResetFarmTile();
            }

            //set text tween
            stateMachine.sharedContext.readyTextImage.transform.localScale = Vector3.zero;
            stateMachine.sharedContext.readyTextImage.color = Color.white;
            stateMachine.sharedContext.goTextImage.transform.localScale = Vector3.zero;
            stateMachine.sharedContext.goTextImage.color = Color.white;
            
            
            var readyInTweener = stateMachine.sharedContext.readyTextImage.transform.DOScale(1, 0.5f).SetEase(Ease.InOutElastic);
            var readyOutTweener = stateMachine.sharedContext.readyTextImage.DOFade(0, 0.5f);
            var goInTweener = stateMachine.sharedContext.goTextImage.transform.DOScale(1, 0.5f).SetEase(Ease.InOutElastic);
            var goOutTweener = stateMachine.sharedContext.goTextImage.DOFade(0, 0.5f);
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(1f)
                .AppendCallback(()=>{SoundEffectManager.singleton.PlaySFX(SoundEffectManager.SoundEffectName.ready);})
                .Append(readyInTweener)
                .Append(readyOutTweener)
                .AppendInterval(1f)
                .AppendCallback(()=>{SoundEffectManager.singleton.PlaySFX(SoundEffectManager.SoundEffectName.go);})
                .Append(goInTweener)
                .Append(goOutTweener)
                .AppendCallback(() => { SwitchToState(GameLoopState.GameRunning); });
        }
        
        public override void ExitState()
        {
            base.ExitState();
            stateMachine.sharedContext.readyTextImage.transform.localScale = Vector3.zero;
            stateMachine.sharedContext.goTextImage.transform.localScale = Vector3.zero;
        }
    }
    public class GameRunningState : SimpleStateInstance<GameLoopState, GameManagerContext>
    {
        public override void EnterState()
        {
            base.EnterState();
            stateMachine.sharedContext.player.SetPlayerMovable(true);
            DOTween.Sequence().AppendInterval(90f)
                .AppendCallback(() => { SwitchToState(GameLoopState.GameEnd); });
        }
        
        public override void ExitState()
        {
            base.ExitState();
            stateMachine.sharedContext.player.SetPlayerMovable(false);
        }
    }
    public class GameEndState : SimpleStateInstance<GameLoopState, GameManagerContext>
    {
        public override void EnterState()
        {
            base.EnterState();
            //set text tween
            stateMachine.sharedContext.endTextImage.transform.localScale = Vector3.zero;
            stateMachine.sharedContext.endTextImage.color = Color.white;
            
            var endInTweener = stateMachine.sharedContext.endTextImage.transform.DOScale(1, 0.5f).SetEase(Ease.InOutElastic);
            var endOutTweener = stateMachine.sharedContext.endTextImage.DOFade(0, 0.5f);
            DOTween.Sequence()
                .Append(endInTweener)
                .Append(endOutTweener)
                .AppendInterval(1f)
                .AppendCallback(() => { SwitchToState(GameLoopState.Conclusion); });
        }
        
        public override void ExitState()
        {
            stateMachine.sharedContext.endTextImage.transform.localScale = Vector3.zero;
            base.ExitState();
        }
    }
    
    public class ConclusionState : SimpleStateInstance<GameLoopState, GameManagerContext>
    {
        public override void EnterState()
        {
            base.EnterState();
            stateMachine.sharedContext.conclusionCanvas.gameObject.SetActive(true);
            stateMachine.sharedContext.againButton.onClick.AddListener(OnAgainButtonClick);
            stateMachine.sharedContext.homeButton.onClick.AddListener(OnHomeButtonClick);
        }
        public override void ExitState()
        {
            stateMachine.sharedContext.conclusionCanvas.gameObject.SetActive(false);

            base.ExitState();
        }
        void OnAgainButtonClick()
        {
            SwitchToState(GameLoopState.GameReadyStart);
        }
        void OnHomeButtonClick()
        {
            SwitchToState(GameLoopState.Logo);
        }
    }
}
