using System;
using System.Collections;
using System.Collections.Generic;
using MintAnimation;
using MintAnimation.Core;
using UnityEngine;

public class MintAnimationGroup : MonoBehaviour
{
    public List<MintAnimationComponent> Bases = new List<MintAnimationComponent>();
    private bool _isFristInit = true;

    public Action OnComplete;

    public bool IsAutoPlay = true;
    public bool ResetValueOnEnable = true;
    public bool FinalizeValueOnDisable = true;

    public PlayEndAction CompleteAction = PlayEndAction.None;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        if (ResetValueOnEnable)
        {
            Reset();
        }

        if (IsAutoPlay)
        {
            Play();
        }
    }

    private void OnDisable()
    {
        if (FinalizeValueOnDisable) Stop();
    }

    protected void OnCompleteAction()
    {
        this.OnComplete?.Invoke();
        switch (CompleteAction)
        {
            case PlayEndAction.Destory:
                Destroy(this.gameObject);
                break;
            case PlayEndAction.Disable:
                this.gameObject.SetActive(false);
                break;
            case PlayEndAction.DestoryAnimation:
                Destroy(this);
                break;
        }
    }

    public void Play()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(PlayAnim());
    }

    IEnumerator PlayAnim()
    {
        int i = Bases.Count;
        foreach (var component in Bases)
        {
            component.OnComplete = () => { i--; };
            component.Play();
        }

        yield return new WaitUntil(() => i == 0);
        OnCompleteAction();
    }

    public void Pause()
    {
        foreach (var component in Bases)
        {
            component.Pause();
        }
    }

    public void Stop()
    {
        foreach (var component in Bases)
        {
            component.Stop();
        }
    }

    public void Reset()
    {
        foreach (var component in Bases)
        {
            component.Reset();
        }
    }

    private void OnValidate()
    {
        if (Bases != null)
        {
            foreach (var component in Bases)
            {
                component.IsAutoPlay = false;
                component.FinalizeValueOnDisable = false;
                component.ResetValueOnEnable = false;
                component.OnComplete = default;
            }
        }
    }
}