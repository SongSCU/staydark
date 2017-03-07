using UnityEngine;
using System.Collections;

public class StartMenuBG : MonoBehaviour {
    public Material m_BG;
    public float m_fSpeed = 1.0f;
    private float m_fOffset = 0.0f;

    //Change offset per frame
    void Update()
    {
        m_BG.mainTextureOffset = new Vector2(m_fOffset, 0);
        m_fOffset += Time.deltaTime * m_fSpeed;
    }
}
