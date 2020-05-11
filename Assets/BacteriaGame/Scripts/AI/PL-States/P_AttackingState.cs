using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_AttackingState : StateMachineBehaviour
{
    MinionController m_Controller;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Controller = animator.GetComponent<MinionController>();      
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Controller.m_Agent.SetDestination(m_Controller.Destination);

        float distanceToTarget = Vector3.Distance(m_Controller.m_Agent.destination, animator.transform.position);

        if (m_Controller.canAttack)
        {
            animator.GetComponent<Play3DSound>().PlayAttack();
            m_Controller.currentTarget.GetComponent<Health>().TakeDamage(m_Controller.m_Data.attackPower + m_Controller.blobs.Count, animator.gameObject);
            m_Controller.StartCoolDownTimer();
        }

        if (distanceToTarget > m_Controller.m_Data.attackRange || m_Controller.currentTarget == null)
        {
            animator.SetBool("isAttacking?", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
