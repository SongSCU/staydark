using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.SceneManagement;

public class CGameManager : MonoBehaviour
{

    public Boss boss;
    public CEndBloom endBloom;

    private Boss m_boss;
    private CEndBloom m_bloom;
    // Use this for initialization
    void Start()
    {
        m_boss = boss;
        m_boss.gameObject.SetActive(false);

        m_bloom = endBloom;
        //m_bloom.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StageClear()
    {
        SceneManager.LoadScene("StageClear");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}


