using System;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    [SerializeField] private string idleParamaterName = "Idle";
    [SerializeField] private string walkParamaterName = "Run";
    [SerializeField] private string attackParamaterName = "Shot";


    public int IdleParamaterHash { get; private set; }
    public int WalkParamaterHash { get; private set; }
    public int AttackParamaterHash { get; private set; }

    public void Initialize()
    {
        IdleParamaterHash = Animator.StringToHash(idleParamaterName);
        WalkParamaterHash = Animator.StringToHash(walkParamaterName);
        AttackParamaterHash = Animator.StringToHash(attackParamaterName);
    }
}
