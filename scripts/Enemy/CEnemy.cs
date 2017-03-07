using UnityEngine;
using System.Collections;

public class CEnemy : MonoBehaviour
{

    public float viewDistance = 5.0f;

    private const int Check_View_Timer = 5;
    private float m_viewDistance;
    private int m_checkViewTimer = Check_View_Timer;
    private Transform m_playerTrans;
    private Transform m_transform;

    // Use this for initialization
    void Start()
    {
        m_viewDistance = viewDistance;
        m_playerTrans = this.GetComponent<CCastRay>().player.transform;
        m_transform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {

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
           
        }
        else
        {
           
        }
    }

    private bool CanReach()
    {
        return true;
    }

}
