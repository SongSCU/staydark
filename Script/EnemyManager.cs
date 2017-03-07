using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
    static public int EnemyCount = 0;
    static public int EnemyCapacity = 30;
    public GameObject m_spawnPointsParent;
    public GameObject m_enemy;
    public float m_spawnTime = 5.0F;

    private GameObject[] m_spawnPoints;
    private float m_lastSpwanTime;

	// Use this for initialization
	void Start () {
        Transform[] transforms = m_spawnPointsParent.GetComponentsInChildren<Transform>();
        m_spawnPoints = new GameObject[transforms.Length];
        for(int i = 0; i < transforms.Length; ++i)
        {
            m_spawnPoints[i] = transforms[i].gameObject;
        }
        m_lastSpwanTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
	    if (EnemyCount < EnemyCapacity - 3 && Time.time - m_lastSpwanTime > m_spawnTime)
        {
            m_lastSpwanTime = Time.time;
            --EnemyCapacity;

            Instantiate(m_enemy, m_spawnPoints[Random.Range(0, m_spawnPoints.Length)].transform.position, Quaternion.identity);
            Instantiate(m_enemy, m_spawnPoints[Random.Range(0, m_spawnPoints.Length)].transform.position, Quaternion.identity);
        }
	}
}
