using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    

    // ================================== VARIABLES ==================================
    #region Vars
    // static vars
    public static CameraController s_Instance;
    // private vars
    private Vector2 m_WorldUnitSize = Vector2.zero;
    private Camera m_Camera;
    #endregion

    // ================================== UNITY FUNCS ==================================
    #region Unity funcs
    private void Awake()
    {
        s_Instance = this;
        m_Camera = Camera.main;

        int height = Screen.height;
        int width = Screen.width;
        m_Camera.orthographicSize = Constant.HALF_HEIGHT;
        Constant.HALF_WIDTH = width * (Constant.HALF_HEIGHT / height);

        m_WorldUnitSize = new Vector2(Constant.HALF_WIDTH, Constant.HALF_HEIGHT) * 2.0f;
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
    #endregion

    // ================================== STATIC FUNCS ==================================
    #region Static Funcs
    public static Rect GetCamRectInEditor()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Rect camRect = new Rect();
        float orthographicSize = Camera.main.orthographicSize;
        camRect.height = orthographicSize * 2;
        camRect.width = camRect.height * (screenSize.x / screenSize.y);
        camRect.x = Camera.main.transform.position.x - camRect.width / 2.0f;
        camRect.y = Camera.main.transform.position.y + camRect.height / 2.0f;

        return camRect;
    }

    public static Vector2 GetScreenSize()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        return screenSize;
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public Vector2 GetWorldUnitSize()
    {
        return m_WorldUnitSize;
    }
    public Vector2 GetBottomCamPos()
    {
        Vector2 resultPos = Camera.main.transform.position;
        resultPos.y -= m_WorldUnitSize.y / 2.0f;
        return resultPos;
    }
    public Vector2 GetTopCamPos()
    {
        Vector2 resultPos = Camera.main.transform.position;
        resultPos.y += m_WorldUnitSize.y / 2.0f;
        return resultPos;
    }
    public Vector2 GetLeftCamPos()
    {
        Vector2 resultPos = Camera.main.transform.position;
        resultPos.x -= m_WorldUnitSize.x / 2.0f;
        return resultPos;
    }
    public Vector2 GetTopLeftCamPos()
    {
        Vector2 resultPos = Camera.main.transform.position;
        resultPos.x -= m_WorldUnitSize.x / 2.0f;
        resultPos.y += m_WorldUnitSize.y / 2.0f;
        return resultPos;
    }
    public Vector2 GetRightCamPos()
    {
        Vector2 resultPos = Camera.main.transform.position;
        resultPos.x += m_WorldUnitSize.x / 2.0f;
        return resultPos;
    }
    #endregion
}
