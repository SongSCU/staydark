using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public Texture2D m_pausePic;
    public Texture2D m_resumePic;
    public Texture2D m_backToMenuPic;
    public Texture2D m_overPic;
    public Texture2D m_tryAgainPic;
    public Texture2D m_stageClearPic;
    public Texture2D m_stageClearInfoPic;
    public GUISkin m_customSkin;
    public State m_state;

    private Camera m_camera;


    public enum State
    {
        Game, Menu, GameOver, StageClear
    };

    // Use this for initialization
    void Start()
    {
        m_camera = gameObject.GetComponent<Camera>();

    }

    // Update is called once per frame, tackle input
    void Update()
    {
       // if (Input.GetKeyDown(KeyCode.Escape) && m_state == State.Game)
       // {
       //     m_state = State.Menu;
        //    Time.timeScale = 0F;
       // }
    }

    void OnGUI()
    {
        GUI.skin = m_customSkin;
        switch (m_state)
        {
            case State.Menu:
                Cursor.lockState = CursorLockMode.None;
                GUI.Label(new Rect(10F, 10F, m_pausePic.width, m_pausePic.height), m_pausePic);
                if (GUI.Button(new Rect(1024F, 464F, m_resumePic.width / 2, m_resumePic.height / 2), m_resumePic))
                {
                   // Debug.Log("Resume");
                   // m_state = State.Game;
                   // Time.timeScale = 1F;
                }
                if (GUI.Button(new Rect(1024F, 528F, m_backToMenuPic.width / 2, m_backToMenuPic.height / 2), m_backToMenuPic))
                {
                  //  m_state = State.Game;
                  //  Time.timeScale = 1F;
                  //  SceneManager.LoadScene("Menu");
                }
                break;

            case State.GameOver:
                Cursor.lockState = CursorLockMode.None;
                GUI.Label(new Rect(10F, 10F, m_overPic.width * 2, m_overPic.height * 2), m_pausePic);
                if (GUI.Button(new Rect(1024F, 464F, m_tryAgainPic.width / 2, m_tryAgainPic.height / 2), m_tryAgainPic))
                {
                    SceneManager.LoadScene("Main");
                }
                if (GUI.Button(new Rect(1024F, 528F, m_backToMenuPic.width / 2, m_backToMenuPic.height / 2), m_backToMenuPic))
                {
                    SceneManager.LoadScene("Menu");
                }
                break;

            case State.StageClear:
                Cursor.lockState = CursorLockMode.None;
                GUI.Label(new Rect(395F, 264F, m_stageClearPic.width, m_stageClearPic.height), m_stageClearPic);
                GUI.Label(new Rect(415F, 392F, m_stageClearInfoPic.width, m_stageClearInfoPic.height), m_stageClearInfoPic);

                break;
        }
    }

    public void GameOver()
    {
        m_state = State.GameOver;
    }

    public void StageClear()
    {
        m_state = State.StageClear;
    }
}
