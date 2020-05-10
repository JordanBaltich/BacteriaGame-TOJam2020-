using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMinionController : MonoBehaviour
{
    Rigidbody m_Body;
    Animator m_StateMachine;
    public NavMeshAgent m_Agent;
    public MinionData m_Data;
    public Transform forwardTarget;

    public bool canAttack;

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody>();
        m_StateMachine = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        canAttack = true;
    }

    private void Update()
    {
        if (m_Agent.destination == null || m_Agent.destination == transform.position)
        {
            m_StateMachine.SetBool("TargetFound?", false);
        }
    }

    public void StartCoolDownTimer()
    {
        StartCoroutine(AttackCooldownTimer());
    }

    IEnumerator AttackCooldownTimer()
    {
        canAttack = false;
        print("in cooldown");

        yield return new WaitForSeconds(m_Data.attackCooldown);

        canAttack = true;
    }
}