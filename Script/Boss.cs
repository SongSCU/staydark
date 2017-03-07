using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    public float m_speed = 5F;
    public float m_visionDistance = 10F;

    private GameObject m_player;
    private State m_state;
    private Animation m_animation;
    private Rigidbody m_rigidbody;
    private bool m_canMove;
    private vp_DamageHandler m_status;


    private float m_viewDistance;
    private NavMeshAgent m_agent;
    private CCastRay m_ray;
    private Light m_light;
    private Transform m_playerTrans;
    private Transform m_transform;

    enum State
    {
        Wait, Attack, Dying
    };

    // Use this for initialization
    void Start()
    {
        m_animation = gameObject.GetComponent<Animation>();
        m_rigidbody = gameObject.GetComponent<Rigidbody>();
        m_status = gameObject.GetComponent<vp_DamageHandler>();
        m_state = State.Wait;
        m_player = GameObject.FindGameObjectWithTag("Player");

        m_animation.wrapMode = WrapMode.Loop;
        m_animation.Play("Idle");
        m_canMove = false;
        //m_rigidbody.isKinematic = true;

        m_agent = GetComponent<NavMeshAgent>(); //
        m_agent.speed = m_speed;        // 
        m_ray = this.GetComponent<CCastRay>();  // 
        m_light = this.GetComponentInChildren<Light>();//
        m_playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        m_transform = this.transform;
        m_CloseLight();//
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case State.Wait:
                if (Vector3.Distance(transform.position, m_player.transform.position) < m_visionDistance)
                {
                    m_CloseLight();
                    m_state = State.Attack;
                    m_canMove = true;
                    //m_rigidbody.isKinematic = false;
                }
                break;

            case State.Attack:
                m_agent.SetDestination(m_playerTrans.position);
                m_OpenLight();
                m_animation.Play("Charge");
                break;

            case State.Dying:
                break;
        }
    }

    public void Kill()
    {
        m_animation.Play("DyingA");
        StartCoroutine(m_Die());
        m_state = State.Dying;
    }

    private IEnumerator m_Die()
    {
        yield return new WaitForSeconds(2.0F);
        GameObject.FindGameObjectWithTag("MainLand").SetActive(false);
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }

        yield return new WaitForSeconds(3F);
        Debug.Log("Stage Clear");
        Time.timeScale = 0F;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<CGameManager>().StageClear();
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

}


