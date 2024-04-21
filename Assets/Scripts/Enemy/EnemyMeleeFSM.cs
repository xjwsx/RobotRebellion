using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeFSM : EnemyBase
{
    public enum State
    {
        Idle,
        Move,
        Attack,
    };

    public State currentState = State.Idle;

    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);

    protected void Start()
    {
        base.Start();
        if (transform.parent != null && transform.parent.parent != null)
        {
            parentRoom = transform.parent.parent.gameObject;
        }

        StartCoroutine(nameof(DelayedStartFSM));
    }
    private IEnumerator DelayedStartFSM()
    {
        yield return null;
        StartCoroutine(nameof(FSM));
    }
    protected virtual void InitMonster() { }

    protected virtual IEnumerator FSM()
    {
        if (!parentRoom || !parentRoom.GetComponent<RoomCondition>())
        {
            Debug.LogError("FSM: parentRoom or RoomCondition not set correctly.", this);
            yield break; // Exit the coroutine if setup is not correct
        }
        while (!parentRoom.GetComponent<RoomCondition>().playerInThisRoom)
        {
            yield return Delay500;
        }

        InitMonster();

        while(true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    protected virtual IEnumerator Idle()
    {
        yield return null;
        if(!Anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            Anim.SetTrigger("Idle");
        }
        if(CanAtkStateFun())
        {
            if(canAtk)
            {
                currentState = State.Attack;
            }
            else
            {
                currentState = State.Idle;
                transform.LookAt(Player.transform.position);
            }
        }
        else
        {
            currentState = State.Move;
        }
    }
    protected virtual void AtkEffect() { }
    protected virtual IEnumerator Attack()
    {
        yield return null;

        nvAgent.stoppingDistance = 0f;
        nvAgent.isStopped = true;
        nvAgent.SetDestination(Player.transform.position);
        yield return Delay500;

        nvAgent.isStopped = false;
        nvAgent.speed = 30f;
        canAtk = false;

        if(!Anim.GetCurrentAnimatorStateInfo(0).IsName("stun"))
        {
            Anim.SetTrigger("Attack");
        }
        AtkEffect();
        yield return Delay500;

        nvAgent.speed = moveSpeed;
        nvAgent.stoppingDistance = attackRange;
        currentState = State.Idle;
    }

    protected virtual IEnumerator Move()
    {
        yield return null;
        if(!Anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            Anim.SetTrigger("Walk");
        }
        if(CanAtkStateFun() && canAtk)
        {
            currentState = State.Attack;
        }
        else if(distance > playerRealizeRange)
        {
            nvAgent.SetDestination(transform.parent.position - Vector3.forward * 5f);
        }
        else
        {
            nvAgent.SetDestination(Player.transform.position);
        }
    }
}
