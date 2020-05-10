﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionController : MonoBehaviour
{
    Rigidbody m_Body;
    Animator m_StateMachine;
    Health m_Health;
    public NavMeshAgent m_Agent;
    public MinionData m_Data;

    [SerializeField] MeshRenderer playerBody;
    [SerializeField] float outlineThickness;

    public bool isSelected = false;

    public Vector3 Destination;

    public GameObject currentTarget;

    public bool canAttack = true;

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody>();
        m_StateMachine = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Health = GetComponent<Health>();
    }

    private void Start()
    {
        m_Health.maxHealth = m_Data.maxHealth;
        m_Health.currentHealth = m_Health.maxHealth;

        m_Agent.speed = m_Data.moveSpeed;
        isSelected = false;

        Destination = transform.position;
    }

    private void Update()
    {
        if (isSelected)
        {
            playerBody.material.SetFloat("_OutlineWidth", outlineThickness);
        }
        else
        {
            playerBody.material.SetFloat("_OutlineWidth", 0.0f);
        }

        if (currentTarget != null)
        {
            Destination = currentTarget.transform.position;

            if (m_Health.greaterThreatFound)
            {
                currentTarget = m_Health.currentThreat;
                m_Health.greaterThreatFound = false;
            }

        }
    }

    public void StartCoolDownTimer()
    {
        StartCoroutine(AttackCooldownTimer());
    }

    IEnumerator AttackCooldownTimer()
    {
        canAttack = false;
        print(gameObject.name + " in cooldown");

        yield return new WaitForSeconds(m_Data.attackCooldown);

        canAttack = true;
    }
}
