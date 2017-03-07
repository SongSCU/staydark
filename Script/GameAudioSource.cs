using UnityEngine;
using System.Collections;

public class GameAudioSource : MonoBehaviour {
    static public bool m_musicMute;
    static public bool m_soundMute;

    // Use this for initialization
    void Start () {
        m_musicMute = false;
        m_soundMute = false;
        Object.DontDestroyOnLoad(this.gameObject);
    }
}
