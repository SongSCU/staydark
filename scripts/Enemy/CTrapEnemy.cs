using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CTrapEnemy : MonoBehaviour
{

    private CPumpEnemy[] m_pumps;
    private float m_viewDistance = 10.0f;
    // Use this for initialization
    void Start()
    {
        m_pumps = this.GetComponentsInChildren<CPumpEnemy>();
        foreach (CPumpEnemy pump in m_pumps)
        {
            pump.viewDistance = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartTrap()
    {
        Debug.Log("start trap");
        m_pumps = this.GetComponentsInChildren<CPumpEnemy>();
        foreach (CPumpEnemy pump in m_pumps)
        {

            pump.viewDistance = m_viewDistance;
            Transform trans = pump.GetComponentInChildren<MeshRenderer>().transform;
            Debug.Log("before" + trans.position);
            //trans.Translate(0, 100.0f, 0);
            Debug.Log("after" + trans.position);
        }
    }

    public void CloseTrap()
    {

    }
}
