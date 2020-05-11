using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMinionController : MonoBehaviour
{
    Rigidbody m_Body;
    Animator m_StateMachine;
    Health m_Health;
    AI_Vision m_Vision;
    public NavMeshAgent m_Agent;
    public MinionData m_Data;
    public Transform forwardTarget;
    [SerializeField] MeshRenderer AIBody;
    [SerializeField] float outlineThickness;

    public bool canAttack;
    public bool isSelected;

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody>();
        m_StateMachine = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Health = GetComponent<Health>();
        m_Vision = GetComponentInChildren<AI_Vision>();
    }

    private void Start()
    {

        m_Health.maxHealth = m_Data.maxHealth;
        m_Health.currentHealth = m_Health.maxHealth;

        canAttack = true;
    }

    private void Update()
    {
        if (m_Agent.destination == null || m_Agent.destination == transform.position)
        {
            m_StateMachine.SetBool("TargetFound?", false);
        }

        if (m_Vision.currentTarget != null)
        {
            if (m_Health.greaterThreatFound)
            {
                m_Vision.currentTarget = m_Health.currentThreat;
                m_Agent.SetDestination(m_Vision.currentTarget.transform.position);
                m_Health.greaterThreatFound = false;
            }
        }

        if (isSelected)
        {
            AIBody.material.SetFloat("_OutlineWidth", outlineThickness);
        }
        else
        {
            AIBody.material.SetFloat("_OutlineWidth", 0.0f);
        }

    }

    public void StartCoolDownTimer()
    {
        StartCoroutine(AttackCooldownTimer());
    }

    IEnumerator AttackCooldownTimer()
    {
        canAttack = false;
        print(gameObject.name  + " in cooldown");

        yield return new WaitForSeconds(m_Data.attackCooldown);

        canAttack = true;
    }
}