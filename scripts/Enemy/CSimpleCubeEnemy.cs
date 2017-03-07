using UnityEngine;
using System.Collections;

public class CSimpleCubeEnemy : MonoBehaviour
{

    public float transTimer = 0.2f;

    private vp_PlayerDamageHandler m_player = null;
    private float m_transTimer;
    private NavMeshAgent m_agent;
    float m_moveSpeed = 2.5f;
    // Use this for initialization
    void Start()
    {
        m_transTimer = transTimer;
        m_player = this.GetComponent<CCastRay>().player;
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.speed = m_moveSpeed;
        m_agent.SetDestination(m_player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_player.m_CurrentHealth < 0)
        {
            return;
        }
        if (m_transTimer > 0)
        {
            m_transTimer -= Time.deltaTime;
            return;
        }
        else
        {
            m_transTimer = transTimer;
        }
        m_agent.SetDestination(m_player.transform.position);

    }
}
