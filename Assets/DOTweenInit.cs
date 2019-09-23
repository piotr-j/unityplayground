using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DOTweenInit : MonoBehaviour
{
    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.Verbose);
    }
}
