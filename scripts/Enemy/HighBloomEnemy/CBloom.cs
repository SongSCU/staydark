using UnityEngine;
using System.Collections;

public class CBloom : MonoBehaviour
{



    public float moveTimer = 0.0f;

    private float m_rotSpeed = 30;
    private vp_PlayerDamageHandler m_player;
    private CBloomHeader m_header;
    private float m_moveTimer;
    private Transform m_bloomTrans;
    private Transform m_headerTrans;
    private Transform m_playerTrans;
    // Use this for initialization
    void Start()
    {
        m_player = this.transform.parent.GetComponent<CHighBloomEnemy>().player;
        m_header = this.transform.parent.GetComponent<CHighBloomEnemy>().bloomHeader;

        m_playerTrans = m_player.transform;
        m_headerTrans = m_header.transform;
        m_bloomTrans = this.transform;

        m_moveTimer = moveTimer;

    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (m_moveTimer > 0)
        {
            m_moveTimer -= Time.deltaTime;
            return;
        }
        else
        {
            m_moveTimer = moveTimer;
        }
        float offset = 16.7f;
        m_bloomTrans.LookAt(m_headerTrans.position + new Vector3(0, offset, 0));
        m_bloomTrans.Rotate(new Vector3(-90, 0, 0));
    }

    void OnTriggerEnter()
    {
        Debug.Log("***Enter!!!");
        m_player.Damage(1);
    }
}
