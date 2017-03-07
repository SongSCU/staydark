using UnityEngine;
using System.Collections;


public class Enemy : MonoBehaviour
{
    public float m_fleeHealthThreshold = 0.3F;
    public float m_attackHealthThreshold = 0.7F;
    public float m_hitThreshold = 5F;
    public float m_attackSpeed = 3F;
    public float m_fleeSpeed = 2.5F;
    public float m_searchSpeed = 2F;
    public float m_recoverySpeed = 1.0F;
    public float m_foundDistance = 10.0F;
    public float m_forceFoundDistance = 5.0F;
    public float m_tooCloseDistance = 3.0F;
    public State m_state = State.Wait;


    protected GameObject[] m_recoverPoints;
    protected GameObject m_recoverPoint;
    protected GameObject m_player;
    protected Animation m_animation;
    protected vp_PlayerDamageHandler m_playerStatus;
    protected vp_DamageHandler m_status;
    protected CapsuleCollider m_collider;
    protected Rigidbody m_rigidbody;

    private bool m_changedState;
    private float m_preHeath;
    private State m_preState;
    private bool m_beHit;
    private float m_searchMoveTime;
    private bool m_canMove;
    private float m_speedRate;
    private Vector3 m_movement;
    private NavMeshAgent m_agent;//
    private CCastRay m_ray;//
    private Light m_light;//
    private static int g_count = 0;
    private CGameManager m_manager;


    class MoveParam
    {
        public float speedRate;
        public float time;

        public MoveParam(float pSpeedRate, float pTime)
        {
            speedRate = pSpeedRate;
            time = pTime;
        }
    };

    public enum State
    {
        Ambush, Wait, Search, Attack, Flee, Recovery, Dying, Hiden
    };

    // Use this for initialization
    void Start()
    {
        m_animation = gameObject.GetComponent<Animation>();
        m_status = gameObject.GetComponent<vp_DamageHandler>();
        m_collider = gameObject.GetComponent<CapsuleCollider>();
        m_player = GameObject.FindWithTag("Player");
        m_recoverPoints = GameObject.FindGameObjectsWithTag("RecoverPoint");
        m_playerStatus = m_player.GetComponent<vp_PlayerDamageHandler>();
        m_rigidbody = gameObject.GetComponent<Rigidbody>();
        m_changedState = false;
        m_beHit = false;
        m_canMove = true;
        m_preHeath = m_status.m_CurrentHealth;
        m_searchMoveTime = Time.time;
        m_speedRate = 1F;
        m_movement = Vector3.zero;

        ++EnemyManager.EnemyCount;
        m_animation.wrapMode = WrapMode.Loop;

        m_rigidbody.isKinematic = false;

        m_agent = GetComponent<NavMeshAgent>(); //
        m_agent.speed = m_attackSpeed;        // 
        m_ray = this.GetComponent<CCastRay>();  // 
        m_light = this.GetComponentInChildren<Light>();//
        m_CloseLight();//
        m_manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CGameManager>();

        switch (m_state)
        {
            case State.Hiden:
                gameObject.GetComponentInChildren<Renderer>().enabled = false;
                m_canMove = false;
                m_rigidbody.isKinematic = true;
                break;

            case State.Ambush:
                m_animation.Play("crawl idle");
                m_canMove = false;
                m_rigidbody.isKinematic = true;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Check if dying
        if (m_status.m_CurrentHealth <= 0)
        {
            m_canMove = false;
            m_rigidbody.isKinematic = true;
            m_changedState = true;
            m_state = State.Dying;
        }

        //Check if be hit

        if (m_preHeath - m_hitThreshold > m_status.m_CurrentHealth)
        {
            m_preHeath = m_status.m_CurrentHealth;
            m_animation.wrapMode = WrapMode.Once;
            m_canMove = false;
            m_rigidbody.isKinematic = true;
            m_animation.Play("hit reaction1");
            m_beHit = true;
        }

        //Move
        m_Move();

        //FSM check, change state if necessary
        m_CheckFSM();

        //Play animation and change collider
        if (m_changedState)
        {
            m_changedState = false;
            m_animation.wrapMode = WrapMode.Once;
            m_canMove = false;
            m_rigidbody.isKinematic = true;
            MoveParam[] param;

            switch (m_state)
            {
                case State.Search:
                case State.Attack:
                case State.Flee:
                case State.Wait:
                    m_collider.center = new Vector3(0, 1F, 0);
                    m_collider.height = 2F;
                    m_collider.direction = 1;
                    break;

                case State.Recovery:
                    m_collider.center = new Vector3(0, 0.5F, 0);
                    m_collider.height = 1F;
                    m_collider.direction = 1;
                    break;
            }

            switch (m_state)
            {
                case State.Search:
                    switch (m_preState)
                    {
                        case State.Recovery:
                            m_animation.PlayQueued("eat end", QueueMode.PlayNow);
                            m_animation.PlayQueued("Idle3", QueueMode.CompleteOthers);
                            break;

                        case State.Wait:
                            m_animation.PlayQueued("Idle3", QueueMode.PlayNow);
                            break;
                    }
                    break;

                case State.Wait:
                    m_animation.PlayQueued("Idle1", QueueMode.PlayNow);
                    break;

                case State.Attack:
                    switch (m_preState)
                    {
                        case State.Recovery:
                            m_animation.PlayQueued("eat end", QueueMode.PlayNow);
                            m_animation.PlayQueued("Idle3", QueueMode.CompleteOthers);
                            param = new MoveParam[4];
                            param[0] = new MoveParam(0F, 2F);
                            param[1] = new MoveParam(0F, 2.7F);
                            param[2] = new MoveParam(0.5F, 1.7F);
                            param[3] = new MoveParam(0.8F, 1.3F);
                            StartCoroutine(m_MoveStart(param));
                            break;

                        case State.Wait:
                            m_animation.PlayQueued("Idle3", QueueMode.PlayNow);
                            param = new MoveParam[3];
                            param[0] = new MoveParam(0F, 2.7F);
                            param[1] = new MoveParam(0.5F, 1.7F);
                            param[2] = new MoveParam(0.8F, 1.3F);
                            StartCoroutine(m_MoveStart(param));
                            break;

                        case State.Search:
                            m_animation.PlayQueued("enraged", QueueMode.PlayNow);
                            param = new MoveParam[3];
                            param[0] = new MoveParam(0F, 3.7F);
                            param[1] = new MoveParam(0.5F, 1.7F);
                            param[2] = new MoveParam(0.8F, 1.3F);
                            StartCoroutine(m_MoveStart(param));
                            break;
                    }
                    m_animation.PlayQueued("walk", QueueMode.CompleteOthers);
                    m_animation.PlayQueued("run", QueueMode.CompleteOthers);
                    break;

                case State.Flee:
                    m_animation.PlayQueued("walk", QueueMode.PlayNow);
                    param = new MoveParam[1];
                    param[0] = new MoveParam(0.5F, 1.7F);
                    StartCoroutine(m_MoveStart(param));
                    break;

                case State.Recovery:
                    m_animation.PlayQueued("eat start", QueueMode.PlayNow);
                    break;

                case State.Dying:
                    m_animation.Play("death2");
                    break;
            }
        }

        if (!m_animation.isPlaying)
        {
            m_animation.wrapMode = WrapMode.Loop;
            m_speedRate = 1F;

            switch (m_state)
            {
                case State.Ambush:
                    m_animation.Play("crawl idle");
                    break;

                case State.Search:
                    m_animation.Play("walk");
                    break;

                case State.Wait:
                    m_animation.Play("Idle1");
                    break;

                case State.Attack:
                    m_animation.Play("run fast");
                    break;

                case State.Flee:
                    m_animation.Play("run");
                    break;

                case State.Recovery:
                    m_animation.Play("eat loop");
                    break;
            }
        }
    }

    public void WakeUp()
    {
        if (m_state == State.Hiden || m_state == State.Ambush)
        {
            m_animation.wrapMode = WrapMode.Once;
            switch (m_state)
            {
                case State.Ambush:
                    m_animation.Play("stand up3");
                    StartCoroutine(m_ChangeTo(State.Wait, 2.2F));
                    break;

                case State.Hiden:
                    gameObject.GetComponentInChildren<Renderer>().enabled = true;
                    m_animation.Play("arise-2");
                    StartCoroutine(m_ChangeTo(State.Wait, 3.6F));
                    break;
            }
            //plus
        }
    }

    private IEnumerator m_ChangeTo(State state, float time)
    {
        yield return new WaitForSeconds(time);
        m_preState = m_state;
        m_state = state;
        m_changedState = true;
    }

    private IEnumerator m_MoveStart(MoveParam[] param)
    {
        foreach (MoveParam p in param)
        {
            yield return new WaitForSeconds(p.time);
            m_canMove = true;
            m_rigidbody.isKinematic = false;
            m_speedRate = p.speedRate;
        }
    }

    private void m_Move()
    {
        switch (m_state)
        {
            case State.Search:
                if (m_searchMoveTime < Time.time - 5)
                {
                    m_searchMoveTime = Time.time;
                    m_movement = new Vector3(Random.Range(-100.0F, 100.0F), 0, Random.Range(-100.0F, 100.0F));
                    // m_movement = m_movement.normalized * m_searchSpeed * Time.deltaTime * m_speedRate;
                }
                transform.LookAt(m_movement + transform.position);

                if (m_canMove)
                {
                    //transform.Translate(m_movement, Space.World);
                    m_agent.speed = m_searchSpeed;
                    m_agent.SetDestination(m_player.transform.position + m_movement);
                }
                break;

            case State.Attack:
                transform.LookAt(m_player.transform);

                if (m_canMove)
                {
                    //m_movement = m_player.transform.position - transform.position;
                    //m_movement = m_movement.normalized * m_attackSpeed * Time.deltaTime * m_speedRate;
                    //transform.Translate(m_movement, Space.World);
                    m_agent.speed = m_attackSpeed;
                    m_agent.SetDestination(m_player.transform.position);

                }
                break;

            case State.Flee:
                transform.LookAt(m_recoverPoint.transform);

                if (m_canMove)
                {
                    //m_movement = m_recoverPoint.transform.position - transform.position;
                    //m_movement = m_movement.normalized * m_fleeSpeed * Time.deltaTime * m_speedRate;
                    //transform.Translate(m_movement, Space.World);
                    m_agent.speed = m_fleeSpeed;
                    m_agent.SetDestination(m_recoverPoint.transform.position);

                }
                break;
        }
    }

    private void m_CheckFSM()
    {
        switch (m_state)
        {
            case State.Search:
                if ((Vector3.Distance(transform.position, m_player.transform.position) < m_forceFoundDistance &&
                    Vector3.Distance(transform.position, m_player.transform.position) > m_tooCloseDistance) || m_beHit)
                {
                    m_preState = m_state;
                    m_state = State.Attack;
                    m_changedState = true;
                    m_beHit = false;
                    m_OpenLight();//
                    Debug.Log("Enter Attack State");
                    break;
                }
                break;

            case State.Attack:
                if (m_status.m_CurrentHealth < m_status.MaxHealth * m_fleeHealthThreshold)
                {
                    m_preState = m_state;
                    m_state = State.Flee;
                    m_changedState = true;
                    m_recoverPoint = m_recoverPoints[Random.Range(0, m_recoverPoints.Length)];
                    m_CloseLight();//
                    Debug.Log("Enter Flee State");
                    break;
                }

                if (Vector3.Distance(transform.position, m_player.transform.position) < m_tooCloseDistance)
                {
                    m_preState = m_state;
                    m_state = State.Wait;
                    m_changedState = true;
                    m_OpenLight();//
                    Debug.Log("Enter Wait State");
                    break;
                }
                break;

            case State.Flee:
                if (Vector3.Distance(transform.position, m_recoverPoint.transform.position) < 2.0F)
                {
                    m_preState = m_state;
                    m_state = State.Recovery;
                    m_changedState = true;
                    m_CloseLight();//
                    Debug.Log("Enter Recovery State");
                    break;
                }
                break;

            case State.Recovery:
                m_status.m_CurrentHealth += m_recoverySpeed * Time.deltaTime;

                if (m_status.m_CurrentHealth >= m_status.MaxHealth * m_attackHealthThreshold)
                {
                    m_preState = m_state;
                    if (EnemyManager.EnemyCount * 0.2F > m_playerStatus.m_CurrentHealth)
                    {
                        m_state = State.Search;
                        m_CloseLight();//
                        Debug.Log("Enter Search State");
                    }
                    else
                    {
                        m_state = State.Wait;
                        m_CloseLight();//
                        Debug.Log("Enter Wait State");
                    }

                    m_changedState = true;
                    break;
                }
                break;

            case State.Wait:
                if ((Vector3.Distance(transform.position, m_player.transform.position) < m_forceFoundDistance &&
                    Vector3.Distance(transform.position, m_player.transform.position) > m_tooCloseDistance) || m_beHit)
                {
                    m_preState = m_state;
                    m_state = State.Attack;
                    m_changedState = true;
                    m_beHit = false;
                    m_OpenLight();//
                    Debug.Log("Enter Attack State");
                    break;
                }

                if (EnemyManager.EnemyCount * 0.2F > m_playerStatus.m_CurrentHealth &&
                    Vector3.Distance(transform.position, m_player.transform.position) > m_tooCloseDistance)
                {
                    m_preState = m_state;
                    m_state = State.Search;
                    m_changedState = true;
                    m_CloseLight();//
                    Debug.Log("Enter Search State");
                    break;
                }
                break;
        }
    }

    private void m_OpenLight()//
    {
        m_light.enabled = true;
        m_ray.StartAttack();
    }

    private void m_CloseLight()//
    {
        m_light.enabled = false;
        m_ray.StopAttack();
    }

    void OnDestroy()
    {

        if (this.tag.CompareTo("SmallBoss") == 0)
        {
            g_count++;
        }
        if (g_count >= 3)
        {

            m_manager.boss.gameObject.SetActive(true);
            //m_manager.endBloom.GetComponent<MeshRenderer>().enabled = true;
           // Debug.Log("$$$" + m_manager.endBloom.GetComponent<MeshRenderer>().enabled);

        }
       // Debug.Log("***" + g_count);
    }
}
