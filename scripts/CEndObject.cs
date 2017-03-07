using UnityEngine;
using System.Collections;

public class CEndObject : MonoBehaviour
{
    public GameObject target;

    private CGameManager m_manager;
    private vp_PlayerDamageHandler m_player;
    private Boss m_boss;
    private CEndBloom m_bloom;
    private float m_distance = 2.0f;
    // Use this for initialization
    void Start()
    {
        m_manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CGameManager>();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<vp_PlayerDamageHandler>();
        m_boss = m_manager.boss;
        m_bloom = m_manager.endBloom;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_bloom.GetComponent<MeshRenderer>().enabled)
            return;

        float bossDis = Vector3.Distance(m_boss.transform.position, this.transform.position);
        float playerDis = Vector3.Distance(m_player.transform.position, this.transform.position);
        Debug.Log(bossDis);
        if (bossDis < m_distance)
        {

            m_boss = m_manager.boss;
            target = m_boss.gameObject;

        }
        else if (playerDis < m_distance)
        {
            target = m_player.gameObject;
        }
        else
        {
            return;
        }
        GameObject[] headers = GameObject.FindGameObjectsWithTag("BloomHeader");
        foreach (GameObject header in headers)
        {
            header.GetComponent<CBloomHeader>().AwakenBloom();
        }
    }

  
}
