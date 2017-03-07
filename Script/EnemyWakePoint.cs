using UnityEngine;
using System.Collections;

public class EnemyWakePoint : MonoBehaviour {
    public GameObject[] m_enemies;
    public GameObject m_player;
    public float m_distance = 3F;

    private bool m_wake;

	// Use this for initialization
	void Start () {
        m_wake = false;
    }
	
	// Update is called once per frame
	void Update () {
	    if (!m_wake && Vector3.Distance(m_player.transform.position, transform.position) < m_distance)
        {
            m_wake = true;
            foreach(GameObject enemy in m_enemies)
            {
                Enemy controler = enemy.GetComponent<Enemy>();
                controler.WakeUp();
            }
        }
	}
}
