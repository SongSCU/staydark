using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {
    public Texture2D m_titlePic;
    public Texture2D m_startGamePic;
    public Texture2D m_continuePic;
    public Texture2D m_optionPic;
    public Texture2D m_exitPic;

    public Texture2D m_backPic;
    public Texture2D m_musicOnPic;
    public Texture2D m_soundOnPic;
    public Texture2D m_musicOffPic;
    public Texture2D m_soundOffPic;

    public Texture2D[] m_stagesPics;
    public Texture2D[] m_stagesInfoPics;

    public GUISkin m_customSkin;
    public AudioClip m_choosenSound;

    private State m_state = State.Main;
    private AudioSource m_audio;
    private float m_loadTime;
    private bool m_load;
    private bool m_picMoving;

    enum State
    {
        Main, Option, StartGame
    };

    //Initialize, set screen
    void Start() {
        Debug.Log("Initialize?");
        Screen.SetResolution(1280, 720, true);
        m_audio = gameObject.GetComponent<AudioSource>();

        m_loadTime = 0F;
        m_load = false;
        m_picMoving = false;
    }

    //Start Menu UI settings
    void OnGUI() {
        GUI.skin = m_customSkin;

        switch (m_state)
        {
            case State.Main:
                GUI.Label(new Rect(10F, 10F, m_titlePic.width, m_titlePic.height), m_titlePic);

                if (GUI.Button(new Rect(1024F, 464F, m_startGamePic.width / 2, m_startGamePic.height / 2), m_startGamePic))
                {
                    PlayChoosenSound();
                    m_state = State.StartGame;
                }
                if (GUI.Button(new Rect(1024F, 592F, m_optionPic.width / 2, m_optionPic.height / 2), m_optionPic))
                {
                    PlayChoosenSound();
                    m_state = State.Option;
                }
                if (GUI.Button(new Rect(1024F, 656F, m_exitPic.width / 2, m_exitPic.height / 2), m_exitPic))
                {
                    PlayChoosenSound();
                    Application.Quit();
                }

                if (CheckSave())
                {
                    if (GUI.Button(new Rect(1024F, 528F, m_continuePic.width / 2, m_continuePic.height / 2), m_continuePic))
                    {
                        PlayChoosenSound();
                    }
                }
                break;

            case State.Option:
                GUI.Label(new Rect(10F, 10F, m_optionPic.width, m_optionPic.height), m_optionPic);

                if (GameAudioSource.m_musicMute)
                {
                    if (GUI.Button(new Rect(10F, 256F, m_musicOnPic.width / 2, m_musicOnPic.height / 2), m_musicOnPic))
                    {
                        PlayChoosenSound();
                        m_audio.Play();
                        GameAudioSource.m_musicMute = false;
                    }
                    
                }
                else
                {
                    if (GUI.Button(new Rect(10F, 256F, m_musicOffPic.width / 2, m_musicOffPic.height / 2), m_musicOffPic))
                    {
                        PlayChoosenSound();
                        m_audio.Stop();
                        GameAudioSource.m_musicMute = true;
                    }
                }

                if (GameAudioSource.m_soundMute)
                {
                    if (GUI.Button(new Rect(10F, 384F, m_soundOnPic.width / 2, m_soundOnPic.height / 2), m_soundOnPic))
                    {
                        PlayChoosenSound();
                        GameAudioSource.m_soundMute = false;
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(10F, 384F, m_soundOffPic.width / 2, m_soundOffPic.height / 2), m_soundOffPic))
                    {
                        PlayChoosenSound();
                        GameAudioSource.m_soundMute = true;
                    }
                }

                if (GUI.Button(new Rect(1024F, 656F, m_backPic.width / 2, m_backPic.height / 2), m_backPic))
                {
                    PlayChoosenSound();
                    m_state = State.Main;
                }
                break;

            case State.StartGame:
                if (!m_load)
                {
                    m_picMoving = true;
                    m_load = true;
                    StartCoroutine(m_ToLoadLevel());
                    m_loadTime = Time.time;
                }

                float stagePos, infoPos;
                if (m_picMoving)
                {
                    stagePos = 10F + (Time.time - m_loadTime) * 100F;
                    infoPos = 748F - (Time.time - m_loadTime) * 60F;
                }
                else
                {
                    stagePos = 510F;
                    infoPos = 448F;
                }

                GUI.Label(new Rect(stagePos, 192F, m_stagesPics[2].width, m_stagesPics[2].height), m_stagesPics[2]);
                GUI.Label(new Rect(infoPos, 320F, m_stagesInfoPics[2].width, m_stagesInfoPics[2].height), m_stagesInfoPics[2]);
                break;
        }

        
    }
    
    void PlayChoosenSound()
    {
        if (!GameAudioSource.m_soundMute)
        {
            m_audio.PlayOneShot(m_choosenSound);
        }
    }

    bool CheckSave()
    {
        return false;
    }

    private IEnumerator m_ToLoadLevel()
    {
        yield return new WaitForSeconds(5F);
        m_picMoving = false;
        yield return new WaitForSeconds(2F);
        Application.LoadLevel(1);
    }
}
