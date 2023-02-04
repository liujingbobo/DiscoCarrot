﻿using MintAnimation.Core;
using UnityEngine;

namespace MintAnimation
{
    [AddComponentMenu("MintAnimation/PositionAnimation", 1)]
    public class MintAnimation_Position : MintAnimation_Base<Vector3>
    {
        [SerializeField] private MintAnimationDataVector MintAnimationData = new MintAnimationDataVector();

        public bool IsLocal;

        public bool IsRectPosition;

        public bool IsBezier;

        public Vector3 BezierP1;
        public Vector3 BezierP2;

        [SerializeField] public bool justY;
        [SerializeField] public bool justX;

        private Vector3 MyPosition
        {
            get
            {
                if (IsRectPosition)
                {
                    return ((RectTransform) this.transform).anchoredPosition3D;
                }

                return this.IsLocal ? this.transform.localPosition : transform.position;
            }

            set
            {
                if (IsRectPosition)
                {
                    var thistransform = (RectTransform) this.transform;
                    if (justY)
                    {
                        thistransform.anchoredPosition3D = new Vector3(
                            thistransform.anchoredPosition3D.x, value.y,
                            thistransform.anchoredPosition3D.z);
                    }
                    else if (justX)
                    {
                        thistransform.anchoredPosition3D = new Vector3(value.x,
                            ((RectTransform) this.transform).anchoredPosition3D.y, thistransform.anchoredPosition3D.z);
                    }
                    else ((RectTransform) this.transform).anchoredPosition3D = value;
                }

                else if (this.IsLocal)
                {
                    this.transform.localPosition = value;
                }

                else
                {
                    this.transform.position = value;
                }
            }
        }

        protected override void setter(Vector3 value)
        {
            if (this.IsBezier)
            {
                Bezier_3ref(ref value, this.mMintTweener.TweenInfo.StartValue,
                    this.mMintTweener.TweenInfo.StartValue + this.BezierP1,
                    this.mMintTweener.TweenInfo.StartValue + this.BezierP2, this.mMintTweener.TweenInfo.EndValue,
                    this.mMintTweener.GetPlayerProgress());
            }

            this.MyPosition = value;
        }

        protected override MintTweenDataBase<Vector3> getAnimationData()
        {
            return this.MintAnimationData;
        }

        protected override Vector3 getter()
        {
            return this.MyPosition;
        }

        protected override Vector3 getAutoStartValue()
        {
            return this.MyPosition;
        }

        public static void Bezier_3ref(ref Vector3 outValue, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            outValue = (1 - t) * ((1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2)) +
                       t * ((1 - t) * ((1 - t) * p1 + t * p2) + t * ((1 - t) * p2 + t * p3));
        }
    }
}