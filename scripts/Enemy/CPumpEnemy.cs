using UnityEngine;
using System.Collections;

public class CPumpEnemy : MonoBehaviour
{


    public float viewDistance = 5.0f;

    private Light m_light;
    private MeshRenderer m_pumpMesh;
    private const int Check_View_Timer = 5;
    private float m_viewDistance;
    private int m_checkViewTimer = Check_View_Timer;
    private Transform m_playerTrans;
    private Transform m_transform;
    private CCastRay m_ray;

    // Use this for initialization
    void Start()
    {
        m_viewDistance = viewDistance;
        m_playerTrans = this.GetComponent<CCastRay>().player.transform;
        m_light = this.GetComponentInChildren<Light>();
        m_pumpMesh = this.GetComponentInChildren<MeshRenderer>();
        m_ray = this.GetComponent<CCastRay>();
        m_transform = this.transform;

        m_pumpMesh.enabled = false;
        m_light.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckView();
    }

    /// <summary>
    /// Check if the player can be seen
    /// </summary>
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

        if ((distance >= 0) && (distance <= viewDistance) && CanReach())
        {
            m_ray.StartAttack();
            m_light.enabled = true;
            m_pumpMesh.enabled = true;
        }
        else
        {
            m_ray.StopAttack();
            m_light.enabled = false;
            m_pumpMesh.enabled = false;
        }
    }

    private bool CanReach()
    {
        return true;
    }

}
