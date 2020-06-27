using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryProgressCont : GElement
{
    // ================================== VARIABLES ==================================
    #region Vars
    // prefab vars
    public GameObject Pre_NoPref;
    // const vars
    const int NO_OBJ_COUNTER = 6;
    // private vars
    [SerializeField]
    float m_TrashScaleSpeed = 10.0f;
    float m_CurTrashScale = 0;
    float m_NoSize = 0; // size of number object
    bool m_IsPlayingTrashAnim;
    [SerializeField]
    Vector2 m_TrashAnimScale = new Vector2(1.0f, 2.0f);
    List<GameObject> m_lNoObjs = new List<GameObject>();
    #endregion

    // =================================== OVERRIDE func ===================================
    #region Override func
    // Object Override
    public override void OnCreateObj()
    {
        base.OnCreateObj();
    }

    public override void OnUpdateObj(float a_dt)
    {
        base.OnUpdateObj(a_dt);

        UpdateTrashAnim(a_dt);
    }

    public override void OnDestroyObj()
    {
        base.OnDestroyObj();
    }

    // GElement Override
    public override void Init(GScene a_Scene)
    {
        base.Init(a_Scene);
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void ShowProgress(int a_trashIndex, Vector2 a_progress, bool a_isActiveTrashAnim = false)
    {
        if (!gameObject.active)
            gameObject.SetActive(true);

        // set enable trash anim following flag
        if (a_isActiveTrashAnim)
        {
            m_IsPlayingTrashAnim = true;
            m_CurTrashScale = m_TrashAnimScale.y;
        }

        // parse PROGRESS string
        string strProgress = "";
        strProgress += (a_progress.x < 10) ? "0" + a_progress.x.ToString() : a_progress.x.ToString();
        strProgress += "/";
        strProgress += (a_progress.y < 10) ? "0" + a_progress.y.ToString() : a_progress.y.ToString();
        strProgress += a_trashIndex.ToString();

        // gen NO objs
        if (m_lNoObjs.Count < NO_OBJ_COUNTER)
        {
            for (int i = 0; i < NO_OBJ_COUNTER; i++)
            {
                GameObject noObj = Instantiate(Pre_NoPref);
                noObj.SetActive(false);
                m_lNoObjs.Add(noObj);
            }
        }

        // progress format: xx/xx_trash
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector2 lastPos = new Vector2(sr.bounds.min.x, sr.bounds.center.y);
        if (m_NoSize == 0)
            m_NoSize = Mathf.Min(sr.bounds.size.x / (float)NO_OBJ_COUNTER, sr.bounds.size.y);
        for (int i = 0; i < m_lNoObjs.Count; i++)
        {
            GameObject noObj = m_lNoObjs[i];
            noObj.SetActive(true);
            SpriteRenderer noSr = noObj.GetComponent<SpriteRenderer>();

            // set SPRITE of no
            string spritePath = i == m_lNoObjs.Count - 1 ? AssetPathConstant.FOLDER_PROGRESS_TRASH_PATH : AssetPathConstant.FOLDER_PROGRESS_NO_PATH;
            spritePath += "/" + strProgress[i];
            Sprite noSprite = Resources.Load<Sprite>(spritePath);
            if (noSprite)
            {
                noSr.sprite = noSprite;
            }

            // set SCALE of no
            noObj.transform.parent = null;
            Vector3 noScale = noObj.transform.localScale;
            noScale.x = Mathf.Min(m_NoSize / noSr.sprite.bounds.size.x, m_NoSize / noSr.sprite.bounds.size.y);
            noScale.y = noScale.x;
            noObj.transform.localScale = noScale;
            noObj.transform.parent = transform;

            // set POS of no
            Vector3 noPos = noObj.transform.position;
            noPos.x = lastPos.x + noSr.bounds.extents.x;
            noPos.y = lastPos.y;
            noPos.z += -0.001f * i; // set depth from far to near
            noObj.transform.position = noPos;
            lastPos.x += noSr.bounds.size.x;

            // skip for "/" letter
            if (strProgress[i].ToString() == "/")
            {
                noObj.SetActive(false);
            }
        }
    }

    public void HideProgress()
    {
        if (gameObject.active)
            gameObject.SetActive(false);
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void UpdateTrashAnim(float a_dt)
    {
        if (m_CurTrashScale > m_TrashAnimScale.x)
        {
            m_CurTrashScale -= m_TrashScaleSpeed * a_dt;
            // disable trash anim
            if (m_CurTrashScale <= m_TrashAnimScale.x)
            {
                m_IsPlayingTrashAnim = false;
                m_CurTrashScale = m_TrashAnimScale.x;

                (Ref_GScene as StoryGameMgr).ShowDescriptionBalloon();
            }
            GameObject trashObj = m_lNoObjs[m_lNoObjs.Count - 1];
            SpriteRenderer trashSr = trashObj.GetComponent<SpriteRenderer>();

            // set Scale for trash obj
            trashObj.transform.parent = null;
            Vector2 trashScale = trashObj.transform.localScale;
            trashScale.x = m_NoSize * m_CurTrashScale;
            trashScale.x = Mathf.Min(trashScale.x / trashSr.sprite.bounds.size.x, trashScale.x / trashSr.sprite.bounds.size.y);
            trashScale.y = trashScale.x;
            trashObj.transform.localScale = trashScale;
            trashObj.transform.parent = transform;
        }
    }
    #endregion
}
