using UnityEngine;
using System.Collections;

public class CTrapTrigger : MonoBehaviour
{


    private CTrapEnemy m_trap;
    // Use this for initialization
    void Start()
    {
        m_trap = GetComponentInParent<CTrapEnemy>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter()
    {
        Debug.Log("enter!!!");
        m_trap.StartTrap();
    }
}
