﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_AttackingState : StateMachineBehaviour
{
    AI_Vision m_Vision;
    AIMinionController m_Controller;

    float time = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Vision = animator.GetComponentInChildren<AI_Vision>();
        m_Controller = animator.GetComponent<AIMinionController>();

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distanceToTarget = Vector3.Distance(m_Controller.m_Agent.destination, animator.transform.position);

        if (m_Vision.currentTarget != null)
        {
            m_Controller.m_Agent.SetDestination(m_Vision.currentTarget.transform.position);
        }
        else
        {
            animator.SetBool("TargetFound?", false);
            animator.SetBool("isAttacking?", false);
        }

        if (m_Controller.canAttack)
        {
            animator.GetComponent<Play3DSound>().PlayAttack();
            m_Vision.currentTarget.GetComponent<Health>().TakeDamage(m_Controller.m_Data.attackPower, animator.gameObject);
            m_Controller.StartCoolDownTimer();
        }

        if (distanceToTarget > m_Controller.m_Data.attackRange)
        {
            animator.SetBool("isAttacking?", false);
        }


    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isAttacking?", false);
    }
}
