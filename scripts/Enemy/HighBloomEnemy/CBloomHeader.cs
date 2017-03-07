using UnityEngine;
using System.Collections;
using System;

public class CBloomHeader : MonoBehaviour
{

    public enum State
    {
        TraceState,
        AwakenState,
        WonderState
    };

    public State state = State.WonderState;

    public float moveSpeed = 2.0f;
    public int updateTimer = 1;


    private const float WonderTimer = 3.0f;
    private const int CheckViewTimer = 5;

    private vp_PlayerDamageHandler m_player;
    private float m_wonderTimer = WonderTimer;
    private int m_checkViewTimer = CheckViewTimer;
    private Boss m_boss;
    private State m_state;
    private int m_updateTimer;
    private float m_damage;
    private System.Random m_random = new System.Random();
    private Vector3 m_wonderDir;
    private bool m_find = false;
    private CGameManager m_manager;
    private GameObject m_target;
    private static int damage = 0;
    // Use this for initialization

    public void AwakenBloom()
    {
        this.state = State.AwakenState;
    }
    void Start()
    {
        m_player = this.transform.parent.GetComponent<CHighBloomEnemy>().player;

        m_state = state;
        m_damage = this.transform.parent.GetComponent<CHighBloomEnemy>().damage;
        m_updateTimer = updateTimer;
        m_manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CGameManager>();
        m_boss = m_manager.boss;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_updateTimer > 0)
        {
            m_updateTimer--;
            return;
        }
        else
        {
            m_updateTimer = updateTimer;
        }
        m_state = state;
        switch (m_state)
        {
            case State.AwakenState:
                Awaken();
                break;
            case State.TraceState:
                Trace();
                break;
            case State.WonderState:
                Wander();
                break;
        }
        CheckView();
    }

    /// <summary>
    /// 
    /// </summary>
    private void Trace()
    {
        float distance = Vector3.Distance(m_player.transform.position, this.transform.position);
        Vector3 move = (m_player.transform.position - this.transform.position) / distance;
        this.transform.Translate(moveSpeed * Time.deltaTime * move);
    }

    private void Wander()
    {
        if (m_wonderTimer > 0)
        {
            m_wonderTimer -= Time.deltaTime;
            this.transform.Translate(moveSpeed * Time.deltaTime * m_wonderDir);
            return;
        }
        else
        {
            m_wonderTimer = WonderTimer;
        }
        double p = m_random.NextDouble();
        if (p < 0.6)
        {
            Vector3 dir = new Vector3((float)m_random.NextDouble(), 0, (float)m_random.NextDouble());
            m_wonderDir = dir;
        }
        else
        {
            float distance = Vector3.Distance(m_player.transform.position, this.transform.position);
            Vector3 dir = (m_player.transform.position - this.transform.position) / distance;
            m_wonderDir = dir;
        }
    }

    private void Awaken()
    {
        m_target = GameObject.FindGameObjectWithTag("EndObject").GetComponent<CEndObject>().target;
        float distance = Vector3.Distance(m_target.transform.position, this.transform.position);
        Vector3 move = (m_target.transform.position - this.transform.position) / distance;
        this.transform.Translate(60 * Time.deltaTime * move);
    }

    private void CheckView()
    {

        if (m_checkViewTimer > 0)
        {
            m_checkViewTimer--;
            return;
        }
        else
        {
            m_checkViewTimer = CheckViewTimer;
        }
        float distance = Vector3.Distance(m_player.transform.position, this.transform.position);
        float bosDis = Vector3.Distance(m_boss.transform.position, this.transform.position);
        if ((distance >= 0) && (distance <= 0.6f))
        {
            m_player.Damage(m_damage / 100);
            GameObject[] headers = GameObject.FindGameObjectsWithTag("BloomHeader");
            foreach (GameObject header in headers)
            {
                header.GetComponent<CBloomHeader>().state = State.TraceState;
            }
        }
        if ((bosDis >= 0) && (bosDis <= 1.0f))
        {
            damage++;
            if (damage >= 100)
            {
                m_boss.Kill();
            }
        }
    }
}
