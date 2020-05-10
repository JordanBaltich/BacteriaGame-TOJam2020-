using System.Collections;
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

    [Header("Movement")]
    public Transform MoveTarget;
    [SerializeField] string moveTargetTag = "MoveTarget";

    [SerializeField] MeshRenderer playerBody;
    Shader OutlineShader;

    public bool isSelected = false;

    public Vector3 Destination;

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody>();
        m_StateMachine = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Health = GetComponent<Health>();
    }

    private void Start()
    {
       MoveTarget = GameObject.FindGameObjectWithTag(moveTargetTag).transform;

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
            playerBody.material.SetFloat("_OutlineWidth", 0.1f);
        }
        else
        {
            playerBody.material.SetFloat("_OutlineWidth", 0.0f);
        }
    }
}
