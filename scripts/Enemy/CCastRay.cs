/**************************************************************************
File:         CCastRay.cs
Author:       Song Xiaofeng
Date:         2016-06-11
Description:  
**************************************************************************/
using UnityEngine;
using System.Collections;

public class CCastRay : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////////
    ///all public attributes are as below
    //////////////////////////////////////////////////////////////////////////
    public vp_PlayerDamageHandler player = null;
    public int rayCastTimer = 10;
    public float rayDistance = 5.0f;
    public float maxDamage = 1.0f;
    public float minDamage = 0.2f;

    //////////////////////////////////////////////////////////////////////////
    ///all private attributes are as below
    //////////////////////////////////////////////////////////////////////////


    private Transform m_playerTransform;
    private Transform m_transform;
    private int m_rayCastTimer;
    private bool m_onAttack = false;


    //////////////////////////////////////////////////////////////////////////
    ///all public functions are as below
    //////////////////////////////////////////////////////////////////////////
    public void StartAttack()
    {
        m_onAttack = true;

    }
    public void StopAttack()
    {
        m_onAttack = false;
    }

    //////////////////////////////////////////////////////////////////////////
    ///all private functions are as below
    //////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {

        m_rayCastTimer = rayCastTimer;
        m_transform = this.transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<vp_PlayerDamageHandler>();
        m_playerTransform = player.GetComponent<Transform>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        CastRay();
    }

    /// <summary>
    /// 
    /// </summary>
    private void CastRay()
    {
        if (!m_onAttack)
        {
            return;
        }
        if (m_rayCastTimer > 0)
        {
            m_rayCastTimer--;
            return;
        }
        else
        {
            m_rayCastTimer = rayCastTimer;
        }
        RaycastHit hitInfo;
        Vector3 direction = m_playerTransform.position - m_transform.position;
        bool isHit = Physics.Raycast(m_transform.position, direction, out hitInfo, rayDistance);
        if (isHit)
        {
            if (hitInfo.transform.parent.tag.CompareTo("Player") == 0)
            {

                float distance = Vector3.Distance(m_playerTransform.position, m_transform.position);
                float damage = ComputeDamage(distance);
                player.Damage(damage);
            }
        }

    }

    /// <summary>
    /// when player is hit by the ray, ComputeDamage will be called to compute the damage
    /// </summary>
    /// <param name="distance"> </param>
    /// <returns></returns>
    private float ComputeDamage(float distance)
    {
        if ((distance < 0) || (distance > rayDistance))
        {
            return 0;
        }

        float rate = (rayDistance - distance) / (rayDistance + 0.01f);
        float damage = (maxDamage - minDamage) * rate + minDamage;

        return damage / 100;
    }
}
