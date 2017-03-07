using UnityEngine;
using System.Collections;

public class CBlueLightEnemy : MonoBehaviour
{


    public float viewDistance = 5.0f;
    public Vector3 direction;

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
        m_transform = this.transform;
        m_ray = this.GetComponent<CCastRay>();
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
            this.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            m_ray.StopAttack();
        }
    }

    private bool CanReach()
    {
        RaycastHit hitInfo;
        //Vector3 direction = m_playerTrans.position - m_transform.position;
        bool isHit = Physics.Raycast(m_transform.position, direction, out hitInfo, viewDistance);
        if (isHit)
        {
            if (hitInfo.transform.parent.tag.CompareTo("Player") == 0)
            {
                return true;
            }
        }
        return false;
    }

}
