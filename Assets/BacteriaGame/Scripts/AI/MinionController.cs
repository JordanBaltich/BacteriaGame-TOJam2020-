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

    [SerializeField] MeshRenderer playerBody;
    [SerializeField] float outlineThickness;

    public bool isSelected = false;
    public Vector3 Destination;
    public GameObject currentTarget;
    public bool canAttack = true;

    //for combat units only
    public List<GameObject> blobs;

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody>();
        m_StateMachine = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Health = GetComponent<Health>();

        blobs = new List<GameObject>();
    }

    private void Start()
    {

        if (gameObject.tag == "Blob")
        {
            m_Health.maxHealth = m_Data.maxHealth;
            m_Health.currentHealth = m_Health.maxHealth;
        }
        else
        {
            if (blobs.Count > 0)
            {
                m_Health.GetTotalHealthPool(blobs);

                for (int i = 0; i < blobs.Count; i++)
                {
                    blobs[i].GetComponent<MinionController>().isSelected = false;
                    blobs[i].SetActive(false);
                }
            }
        }

       

        m_Agent.speed = m_Data.moveSpeed;

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
