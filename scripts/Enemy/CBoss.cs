using UnityEngine;
using System.Collections;

public class CBoss : MonoBehaviour
{

    public float viewDistance = 5.0f;

    private const int Check_View_Timer = 5;
    private float m_viewDistance;
    private int m_checkViewTimer = Check_View_Timer;
    private NavMeshAgent m_agent;
    private CCastRay m_ray;
    private Light m_light;
    private Transform m_playerTrans;
    private Transform m_transform;

    // Use this for initialization
    void Start()
    {
        GameObject.FindGameObjectWithTag("EndBloom").GetComponent<MeshRenderer>().enabled = false;
        m_agent = GetComponent<NavMeshAgent>(); //
        m_agent.speed = 3.0f;        // 
        m_ray = this.GetComponent<CCastRay>();  // 
        m_light = this.GetComponentInChildren<Light>();//
        m_viewDistance = viewDistance;
        m_playerTrans = this.GetComponent<CCastRay>().player.transform;
        m_transform = this.transform;
        m_CloseLight();//
    }

    // Update is called once per frame
    void Update()
    {
        m_agent.SetDestination(m_playerTrans.position);
    }

    void OnDestroy()
    {
        GameObject.FindGameObjectWithTag("EndBloom").GetComponent<MeshRenderer>().enabled = true;
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

    private void CheckView()
    {
        if (m_checkViewTimer > 0)
        {
            m_checkViewTimer--;
            return;
        }
        else
        {
            m_checkViewTimer = Check_View_Timer;
        }
        float distance = Vector3.Distance(m_playerTrans.position, m_transform.position);

        if ((distance >= 0) && (distance <= viewDistance))
        {
            m_OpenLight();
        }
        else
        {
            m_ray.StopAttack();
            m_CloseLight();
        }
    }
}
