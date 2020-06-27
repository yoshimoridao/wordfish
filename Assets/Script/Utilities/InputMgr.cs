using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : MonoBehaviour
{
    // ================================== VARIABLES ==================================
    #region Vars
    // delegate vars
    public delegate void EventProcessSwipeUp();
    public delegate void EventProcessSwipeDown();
    public delegate void EventProcessSwipeRight();
    public delegate void EventProcessSwipeLeft();
    // static vars
    public static InputMgr s_Instance;
    public static EventProcessSwipeUp m_eventProcessSwipeUp;
    public static EventProcessSwipeDown m_eventProcessSwipeDown;
    public static EventProcessSwipeRight m_eventProcessSwipeRight;
    public static EventProcessSwipeLeft m_eventProcessSwipeLeft;
    // private vars
    [SerializeField]
    private Vector2 m_SwipeRange = new Vector2(0.25f, 0.2f); // range = percent of Screen
    private Vector2 m_StartSwipePos = Vector2.zero;
    #endregion

    // ================================== UNITY FUNCS ==================================
    #region Unity funcs
    private void Awake()
    {
        s_Instance = this;
        m_eventProcessSwipeUp += ProcessSwipeUp;
        m_eventProcessSwipeDown += ProcessSwipeDown;
        m_eventProcessSwipeRight += ProcessSwipeRight;
        m_eventProcessSwipeLeft += ProcessSwipeLeft;
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            if (m_StartSwipePos == Vector2.zero)
                m_StartSwipePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Input.mousePosition;
            if (Mathf.Abs(mousePos.x - m_StartSwipePos.x) >= m_SwipeRange.x * Screen.width)
            {
                if (mousePos.x > m_StartSwipePos.x)
                {
                    m_eventProcessSwipeRight();
                }
                else
                {
                    m_eventProcessSwipeLeft();
                }
            }
            if (Mathf.Abs(mousePos.y - m_StartSwipePos.y) >= m_SwipeRange.y * Screen.height)
            {
                if (mousePos.y > m_StartSwipePos.y)
                {
                    m_eventProcessSwipeUp();
                }
                else
                {
                    m_eventProcessSwipeDown();
                }
            }
            m_StartSwipePos = Vector2.zero;
        }
    }
    #endregion

    // =========================== DELEGATE EVENT FUNCS ===========================
    #region Delegate Funcs
    void ProcessSwipeRight()
    {

    }

    void ProcessSwipeLeft()
    {

    }

    void ProcessSwipeUp()
    {

    }

    void ProcessSwipeDown()
    {

    }
    #endregion
}
