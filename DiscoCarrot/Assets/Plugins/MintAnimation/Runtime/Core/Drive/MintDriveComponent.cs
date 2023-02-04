using System;
using System.Collections;
using UnityEngine;

namespace MintAnimation.Core {

    /// <summary>
    /// 动画驱动事件
    /// </summary>
    /// <param name="timeScale">时间间隔 (s)</param>
    /// <returns></returns>
    public delegate bool DriveUpdater(float timeScale);

    /// <summary>
    /// 动画驱动器
    /// </summary>
    public class MintDriveComponent : MonoBehaviour
    {
        private DriveUpdater _updateDrive = null;
        private DriveUpdater _fixedUpdateDrive = null;
        private DriveUpdater _enumeratorDrive = null;

        [Tooltip("时间缩放系数，改变此属性将影响所有使用改驱动器的动画速率")]
        public float TimeOffset = 1;
        
        public bool UseUnscaledDeltaTime = false;

        private float DT => UseUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
        private float FixedDT => UseUnscaledDeltaTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;

        #region Public

        public void AddDriveAction(DriveUpdater driveAction , UpdaterTypeEnum driveEnum = UpdaterTypeEnum.Update) {
            switch (driveEnum)
            {
                case UpdaterTypeEnum.Update:
                    if (null == _updateDrive)
                        _updateDrive = driveAction;
                    else
                        _updateDrive += driveAction;
                    break;
                case UpdaterTypeEnum.FixedUpdate:
                    if (null == _fixedUpdateDrive)
                        _fixedUpdateDrive = driveAction;
                    else
                        _fixedUpdateDrive += driveAction;
                    break;
                case UpdaterTypeEnum.Coroutine:
                    if (null == _enumeratorDrive)
                        _enumeratorDrive = driveAction;
                    else
                        _enumeratorDrive += driveAction;
                    break;
            }
        }

        public void RemoveDriveAction(DriveUpdater driveAction, UpdaterTypeEnum driveEnum = UpdaterTypeEnum.Update) {
            switch (driveEnum)
            {
                case UpdaterTypeEnum.Update:
                    if (_updateDrive != null)
                        _updateDrive -= driveAction;
                    break;
                case UpdaterTypeEnum.FixedUpdate:
                    if (_fixedUpdateDrive != null)
                        _fixedUpdateDrive -= driveAction;
                    break;
                case UpdaterTypeEnum.Coroutine:
                    if (_enumeratorDrive != null)
                        _enumeratorDrive -= driveAction;
                    break;
            }
        }
        #endregion

        #region Updater
        private void Update(){_updateDrive?.Invoke(DT * TimeOffset);}
        private void FixedUpdate(){ _fixedUpdateDrive?.Invoke(FixedDT * TimeOffset);}
        IEnumerator Updater() {
            while (true)
            {
                _enumeratorDrive?.Invoke(DT * TimeOffset);
                yield return 0;
            }
        }
        #endregion

        private void OnEnable()
        {
            StartCoroutine(Updater());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public static MintDriveComponent CreateDriveComponent() 
        {
            var drive = GameObject.Instantiate(new MintDriveComponent());
            drive.name = "[ MintAnimationDrive ]";
            DontDestroyOnLoad(drive);
            return drive;
        }
    }
}
