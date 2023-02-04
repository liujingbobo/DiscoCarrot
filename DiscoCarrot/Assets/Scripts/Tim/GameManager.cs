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
    public GameManagerContext sharedContext = new GameManagerContext();
    public SimpleStateMachine<GameLoopState, GameManagerContext> stateMachine =
        new SimpleStateMachine<GameLoopState, GameManagerContext>();

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
                .Append(readyInTweener)
                .Append(readyOutTweener)
                .AppendInterval(1f)
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
            DOTween.Sequence().AppendInterval(2f)
                .AppendCallback(() => { SwitchToState(GameLoopState.GameEnd); });
        }
        
        public override void ExitState()
        {
            base.ExitState();
        }
    }
    public class GameEndState : SimpleStateInstance<GameLoopState, GameManagerContext>
    {
        public override void EnterState()
        {
            base.EnterState();
            DOTween.Sequence().AppendInterval(2f)
                .AppendCallback(() => { SwitchToState(GameLoopState.Conclusion); });
        }
        
        public override void ExitState()
        {
            base.ExitState();
        }
    }
    
    public class ConclusionState : SimpleStateInstance<GameLoopState, GameManagerContext>
    {
        public override void EnterState()
        {
            base.EnterState();
            DOTween.Sequence().AppendInterval(2f)
                .AppendCallback(() => { SwitchToState(GameLoopState.Logo); });
        }
        
        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
