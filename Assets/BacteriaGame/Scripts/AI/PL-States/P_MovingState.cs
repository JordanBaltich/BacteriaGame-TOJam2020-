using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_MovingState : StateMachineBehaviour
{
    MinionController m_Controller;

    int AILayerMask = 9;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Controller = animator.GetComponent<MinionController>();

        m_Controller.m_Agent.SetDestination(m_Controller.Destination);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Controller.Destination = new Vector3(m_Controller.Destination.x, animator.transform.position.y, m_Controller.Destination.z);
        m_Controller.m_Agent.SetDestination(m_Controller.Destination);

        float distance = Vector3.Distance(animator.transform.position, m_Controller.m_Agent.destination);

        if (m_Controller.currentTarget != null)
        {
            if (m_Controller.currentTarget.layer == AILayerMask)
            {
                if (m_Controller.m_Data.attackRange != 0)
                {
                    if (distance <= m_Controller.m_Data.attackRange)
                    {
                        animator.SetBool("isAttacking?", true);
                    }
                }
            }
        }

        if (distance < 1)
        {
            animator.SetBool("isMoving?", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
