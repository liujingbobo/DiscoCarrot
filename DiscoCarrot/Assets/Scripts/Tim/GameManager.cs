using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using TimPlugin;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public GameManagerContext sharedContext = new GameManagerContext();
    public BasicStateMachine<GameLoopState, GameManagerContext> stateMachine =
        new BasicStateMachine<GameLoopState, GameManagerContext>();

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        stateMachine.context = sharedContext;
        stateMachine.AddStateInstance(GameLoopState.Logo, new LogoBasicState());
        stateMachine.AddStateInstance(GameLoopState.SongPicker, new SongPickerState());
        stateMachine.AddStateInstance(GameLoopState.GameCutScene, new GameCutSceneState());
        stateMachine.AddStateInstance(GameLoopState.GameReadyStart, new GameReadyStartState());
        stateMachine.AddStateInstance(GameLoopState.GameRunning, new GameRunningState());
        stateMachine.AddStateInstance(GameLoopState.GameEnd, new GameEndState());
        stateMachine.AddStateInstance(GameLoopState.Conclusion, new ConclusionState());
        
        stateMachine.SwitchToState(GameLoopState.Logo);
    }

    private void Update()
    {
        stateMachine.ExecuteCurrentStateUpdate();
    }


    [Serializable]
    public class GameManagerContext
    {
        //general references
        public Player player;
        public FarmTile[] farmTiles;
        public CarrotMetronome carrotMetronome;
        
        //logo screen
        public Button startButton;
        public Canvas logoCanvas;
        
        //song picker
        public GameObject pickSongCanvas;
        public Koreography pickedSongKore;
        
        //GameCutScene
        public Canvas cutSceneCanvas;
        public Image[] cutSceneImages;
        
        //GameReadyStart
        public AudioSource source;
        public Image readyTextImage;
        public Image goTextImage;

        //GameEnd
        public Image endTextImage;

        //Conclusion
        public ConclusionUI conclusionUI;
        public Button againButton;
        public Button homeButton;
        
        //cached runtime values
        public GameRunTimeValues runTimeValues = new GameRunTimeValues();
        
        // Koreographer
        public GameSettings settings;
        public SimpleMusicPlayer musicPlayer;
        public Koreographer koreographer;
        public OperationIndicator indicator;
        public FarmingStateMachine stateMachine;
    }

    public class GameRunTimeValues
    {
        public int[] harvestedCarrots = new int[4];
        public int missedCount = 0;



        public GameRunTimeValues()
        {
            harvestedCarrots = new int[4];
            missedCount = 0;
        }
    }
    
    public enum GameLoopState
    {
        Logo,
        SongPicker,
        GameCutScene,
        GameReadyStart,
        GameRunning,
        GameEnd,
        Conclusion,
    }

    public class LogoBasicState : BasicStateInstance<GameLoopState, GameManagerContext>
    {
        public override void EnterState()
        {
            base.EnterState();
            stateMachine.context.logoCanvas.gameObject.SetActive(true);
            stateMachine.context.startButton.onClick.AddListener(OnButtonClick);

            G.Settings = stateMachine.context.settings;
            K.koreographer = stateMachine.context.koreographer;
            K.musicPlayer = stateMachine.context.musicPlayer;
            G.Indicator = stateMachine.context.indicator;
            G.StateMachine = stateMachine.context.stateMachine;
        }
        public override void ExitState()
        {
            base.ExitState();
            stateMachine.context.logoCanvas.gameObject.SetActive(false);
            stateMachine.context.startButton.onClick.RemoveListener(OnButtonClick);
        }

        void OnButtonClick()
        {
            SwitchToState(GameLoopState.SongPicker);
        }
    }
    public class SongPickerState : BasicStateInstance<GameLoopState, GameManagerContext>
    {
        public override void EnterState()
        {
            base.EnterState();
            stateMachine.context.pickSongCanvas.SetActive(true);
        }
        public override void ExitState()
        {
            base.ExitState();
            stateMachine.context.pickSongCanvas.SetActive(false);
        }
    }
    public class GameCutSceneState : BasicStateInstance<GameLoopState, GameManagerContext>
    {
        private int currentPicIndex = 0;
        public override void EnterState()
        {
            base.EnterState();
            currentPicIndex = 0;
            stateMachine.context.cutSceneCanvas.gameObject.SetActive(true);
            stateMachine.context.cutSceneImages[currentPicIndex].gameObject.SetActive(true);
        }

        public override void StateUpdate()
        {
            base.StateUpdate();
            if (currentPicIndex >= stateMachine.context.cutSceneImages.Length)
            {
                SwitchToState(GameLoopState.GameReadyStart);
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentPicIndex++;
                    if (currentPicIndex < stateMachine.context.cutSceneImages.Length)
                    {
                        stateMachine.context.cutSceneImages[currentPicIndex].gameObject.SetActive(true);
                    }
                    if (currentPicIndex == stateMachine.context.cutSceneImages.Length - 1)
                    {
                        SFXManager.singleton.PlaySFX(SFXManager.SoundEffectName.door);
                    }
                }
            }
        }

        public override void ExitState()
        {
            base.ExitState();
            stateMachine.context.cutSceneImages[stateMachine.context.cutSceneImages.Length-1].gameObject.SetActive(true);
            stateMachine.context.cutSceneCanvas.gameObject.SetActive(false);

        }
    }
    public class GameReadyStartState : BasicStateInstance<GameLoopState, GameManagerContext>
    {
        public override void EnterState()
        {
            base.EnterState();
            //play song
            K.musicPlayer.Stop();
            K.musicPlayer.LoadSong(stateMachine.context.pickedSongKore);
            
            //ResetEverything
            stateMachine.context.player.ResetPlayer();
            foreach (var t in stateMachine.context.farmTiles)
            {
                t.ResetFarmTile();
            }
            
            stateMachine.context.runTimeValues = new GameRunTimeValues();
            stateMachine.context.conclusionUI.Reset();
            stateMachine.context.conclusionUI.gameObject.SetActive(false);

            //set text tween
            stateMachine.context.readyTextImage.gameObject.SetActive(true);
            stateMachine.context.goTextImage.gameObject.SetActive(true);
            stateMachine.context.readyTextImage.transform.localScale = Vector3.zero;
            stateMachine.context.readyTextImage.color = Color.white;
            stateMachine.context.goTextImage.transform.localScale = Vector3.zero;
            stateMachine.context.goTextImage.color = Color.white;
            
            //reset metronome
            stateMachine.context.carrotMetronome.Reset();
            stateMachine.context.carrotMetronome.ShowMetronomePanel();
            stateMachine.context.carrotMetronome.ConfigMetronome((float) K.BeatsPerMinute);
            
            Koreographer.Instance.ClearEventRegister();

            singleton.sharedContext.source.Stop();
            K.musicPlayer.Play();

            Koreographer.Instance.RegisterForEvents("Ready", DoReady);
            Koreographer.Instance.RegisterForEvents("Start", DoStart);

            void DoReady(KoreographyEvent kEvent)
            {
                SFXManager.singleton.PlaySFX(SFXManager.SoundEffectName.ready);
                var sequence = DOTween.Sequence();
                var readyInTweener = stateMachine.context.readyTextImage.transform.DOScale(1, 0.5f).SetEase(Ease.InOutElastic);
                var readyOutTweener = stateMachine.context.readyTextImage.DOFade(0, 0.5f);
                sequence.Append(readyInTweener).Append(readyOutTweener);
            }

            void DoStart(KoreographyEvent kEvent)
            {
                SFXManager.singleton.PlaySFX(SFXManager.SoundEffectName.go);
                var sequence = DOTween.Sequence();
                var goInTweener = stateMachine.context.goTextImage.transform.DOScale(1, 0.5f).SetEase(Ease.InOutElastic);
                var goOutTweener = stateMachine.context.goTextImage.DOFade(0, 0.5f);
                sequence.Append(goInTweener).Append(goOutTweener);
                SwitchToState(GameLoopState.GameRunning); 
            }
        }
        
        public override void ExitState()
        {
            base.ExitState();
            stateMachine.context.readyTextImage.transform.localScale = Vector3.zero;
            stateMachine.context.goTextImage.transform.localScale = Vector3.zero;
        }
    }
    public class GameRunningState : BasicStateInstance<GameLoopState, GameManagerContext>
    {
        public override void EnterState()
        {
            base.EnterState();

            Koreographer.Instance.RegisterForEvents("DownBeat", _ =>
            {
                if(GameEvents.OnDownBeat != null) GameEvents.OnDownBeat.Invoke();
                if(GameEvents.OnOneBeatPassed != null) GameEvents.OnOneBeatPassed.Invoke();
            });      
            Koreographer.Instance.RegisterForEvents("UpBeat", _ =>
            {
                if(GameEvents.OnUpBeat != null) GameEvents.OnUpBeat.Invoke();
            });
            Koreographer.Instance.RegisterForEvents("End", _ =>
            {
                SwitchToState(GameLoopState.GameEnd);
            });
            
            GameEvents.OnReachedFarmTile += OnReachedFarmTile;
            GameEvents.OnLeaveFarmTile += OnLeaveFarmTile;
            GameEvents.OnHarvestCarrot += OnHarvestCarrot;
            stateMachine.context.player.SetPlayerMovable(true);
        }
        
        public override void ExitState()
        {
            base.ExitState();
            GameEvents.OnReachedFarmTile -= OnReachedFarmTile;
            GameEvents.OnLeaveFarmTile -= OnLeaveFarmTile;
            GameEvents.OnHarvestCarrot -= OnHarvestCarrot;
            stateMachine.context.player.SetPlayerMovable(false);
        }

        private void OnReachedFarmTile(FarmTile arg1, PlayerFarmAction arg2)
        {
            stateMachine.context.player.CurTile = arg1;
            Debug.Log($"OnReachedFarmTile, detected need action {arg2}");
        }
        private void OnLeaveFarmTile(FarmTile obj)
        {
            if (stateMachine.context.player.CurTile == obj)
                stateMachine.context.player.CurTile = null;
            Debug.Log($"OnLeaveFarmTile");
        }
        
        public void OnHarvestCarrot(CarrotLevel level)
        {
            stateMachine.context.runTimeValues.harvestedCarrots[(int) level] += 1;
        }
    }
    public class GameEndState : BasicStateInstance<GameLoopState, GameManagerContext>
    {
        public override void EnterState()
        {
            base.EnterState();
            //hide carrot
            stateMachine.context.carrotMetronome.HideMetronomePanel();
            //set text tween
            stateMachine.context.endTextImage.transform.localScale = Vector3.zero;
            stateMachine.context.endTextImage.color = Color.white;
            //reset
            stateMachine.context.indicator.Reset();

            Koreographer.Instance.ClearEventRegister();
            
            var endInTweener = stateMachine.context.endTextImage.transform.DOScale(1, 0.5f).SetEase(Ease.InOutElastic);
            var endOutTweener = stateMachine.context.endTextImage.DOFade(0, 0.5f);
            DOTween.Sequence()
                //.Append(endInTweener)
                //.Append(endOutTweener)
                .AppendInterval(1f)
                .AppendCallback(() => {  SwitchToState(GameLoopState.Conclusion); });
        }
        
        public override void ExitState()
        {
            stateMachine.context.endTextImage.transform.localScale = Vector3.zero;
            base.ExitState();
        }
    }
    
    public class ConclusionState : BasicStateInstance<GameLoopState, GameManagerContext>
    {
        public override void EnterState()
        {
            base.EnterState();
            stateMachine.context.conclusionUI.gameObject.SetActive(true);
            stateMachine.context.conclusionUI.OpenConclusionUI();
            stateMachine.context.againButton.onClick.AddListener(OnAgainButtonClick);
            stateMachine.context.homeButton.onClick.AddListener(OnHomeButtonClick);
        }
        public override void ExitState()
        {
            stateMachine.context.conclusionUI.Reset();
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
