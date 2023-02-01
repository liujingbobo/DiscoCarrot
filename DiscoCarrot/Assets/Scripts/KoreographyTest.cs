using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SonicBloom.Koreo;
using UnityEngine;

public class KoreographyTest : MonoBehaviour
{
    [EventID] public string sphere;
    [EventID] public string cube;

    public Transform SphereOBJ;
    public Transform CubeOBJ;

    public Tweener c;
    public Tweener s;
    
    
    private void Awake()
    {
        Koreographer.Instance.RegisterForEvents(sphere, Sphere);
        
        Koreographer.Instance.RegisterForEvents(cube, CubeEvent);
        
        Koreographer.Instance.RegisterForEventsWithTime(cube, CubeEventWithTime);
    }

    void CubeEvent(KoreographyEvent a)
    {
        if (c != null) c.Complete();
        c = CubeOBJ.DOPunchScale(Vector3.one, 0.1f);
    }

    void CubeEventWithTime(KoreographyEvent a, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        print($"Start Sample: {a.StartSample}, End Sample: {a.EndSample}");
        print($"sampleTime: {sampleTime}, sampleDelta: {sampleDelta}, deltaSlice: {deltaSlice}");
    }
    
    void Sphere(KoreographyEvent a)
    {
        if (s != null) s.Complete();
        s = SphereOBJ.DOPunchScale(Vector3.one, 0.1f);
    }
}
