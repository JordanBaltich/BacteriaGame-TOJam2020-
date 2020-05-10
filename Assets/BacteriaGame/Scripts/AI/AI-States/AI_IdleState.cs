using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_IdleState : StateMachineBehaviour
{
    AI_Vision m_Vision;
    AIMinionController m_Controller;

    Vector3 direction;

    float rotationSpeed = 20f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Vision = animator.GetComponentInChildren<AI_Vision>();
        m_Controller = animator.GetComponent<AIMinionController>();

        m_Controller.m_Agent.speed = m_Controller.m_Data.moveSpeed;

        if (m_Vision.CheckForPlayerUnits() != Vector3.zero)
        {
            m_Controller.m_Agent.speed = m_Controller.m_Data.moveSpeed;
            m_Controller.m_Agent.SetDestination(m_Vision.CheckForPlayerUnits());
            animator.SetBool("TargetFound?", true);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (m_Vision.FindNextDestination(animator.transform.position) != animator.transform.position)
        {
            m_Controller.m_Agent.SetDestination(m_Vision.FindNextDestination(animator.transform.position));
        }
        else
        {
            RaycastHit hitInfo;
            direction = m_Controller.forwardTarget.position - animator.transform.position;
            Debug.DrawRay(animator.transform.position, direction.normalized * 5, Color.red);

            if (Physics.Raycast(animator.transform.position, direction.normalized, out hitInfo, 5f, LayerMask.GetMask("Ground")))
            {
                float angle = Vector3.SignedAngle(animator.transform.position, hitInfo.point, Vector3.up);
                Debug.Log(angle);

                if (angle > 0)
                {
                    m_Controller.forwardTarget.transform.RotateAround(animator.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
                }
                else
                {
                    m_Controller.forwardTarget.transform.RotateAround(animator.transform.position, Vector3.up, -rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                m_Controller.forwardTarget.transform.rotation = Quaternion.Euler(Vector3.zero);
                m_Controller.forwardTarget.transform.localPosition = Vector3.forward * 2f;
            }
            
                m_Controller.m_Agent.SetDestination(m_Controller.forwardTarget.position);
        }
       
        if (m_Vision.currentTarget != null)
        {
            animator.SetBool("TargetFound?", true);
        }
      
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
