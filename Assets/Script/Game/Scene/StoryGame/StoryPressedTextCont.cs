using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPressedTextCont : GElement
{
    // ================================== VARIABLES ==================================
    #region Vars
    // reference vars
    public StoryKeyboardMgr Ref_StoryKbMgr;
    // prefab vars
    public GameObject Pref_PressedLetter;
    // private vars
    private List<GameObject> m_lGenLetters = new List<GameObject>();
    private float m_SizePerLetter;
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
    }

    public override void OnDestroyObj()
    {
        base.OnDestroyObj();
    }

    // GElement Override
    public override void Init(GScene a_Scene)
    {
        base.Init(a_Scene);

        // fix POS X of Container
        FixPosXOfCont();
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void InitCurVoca(string a_Voca)
    {
        // generate Letters
        int loopTurn = a_Voca.Length - m_lGenLetters.Count;
        for (int i = 0; i < loopTurn; i++)
        {
            GameObject letterObj = Instantiate(Pref_PressedLetter, transform);
            letterObj.SetActive(false);
            m_lGenLetters.Add(letterObj);
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        m_SizePerLetter =  Mathf.Min(sr.bounds.size.y, sr.bounds.size.x / (float)a_Voca.Length);
    }

    public void ShowText(string a_Voca)
    {
        // setting SIZE, SCALE, POS for LETTERS
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector2 beginTextPos = new Vector2(sr.bounds.min.x, sr.bounds.center.y);

        for (int i = 0; i < m_lGenLetters.Count; i++)
        {
            GameObject letterObj = m_lGenLetters[i];
            // enable available letter || disable surplus letter
            letterObj.SetActive(i >= a_Voca.Length ? false : true);
            // skip invisible letter
            if (!letterObj.active)
                continue;

            // set SCALE
            SpriteRenderer letterSr = letterObj.GetComponent<SpriteRenderer>();
            Vector3 letterScale = new Vector3(m_SizePerLetter / letterSr.sprite.bounds.size.x, m_SizePerLetter / letterSr.sprite.bounds.size.y, 1.0f);
            UtilityClass.SetLossyScale(ref letterObj, letterScale);

            // set POS
            Vector3 letterPos = letterObj.transform.position;
            letterPos.x = beginTextPos.x + m_SizePerLetter * (i + 0.5f);
            letterPos.y = beginTextPos.y;
            letterObj.transform.position = letterPos;

            // change SPRITE
            Sprite sprite = Resources.Load<Sprite>(AssetPathConstant.FOLDER_KEYBOARD_PRESSED_LETTERS_PATH + "/" + a_Voca[i].ToString());
            if (sprite)
                letterSr.sprite = sprite;
        }
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void FixPosXOfCont()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // fix X for Normal mode
        if (Ref_GScene.GetType() == typeof(StoryGameMgr))
        {
            SpriteRenderer kbSr = Ref_StoryKbMgr.GetComponent<SpriteRenderer>();
            //=> left x of Cont = left x of Keyboard
            Vector2 pos = transform.position;
            pos.x = kbSr.bounds.min.x + sr.bounds.extents.x;
            transform.position = pos;
        }
        // fix X for Boss mode
        else if (Ref_GScene.GetType() == typeof(StoryGameBossMgr))
        {
        }
    }
    #endregion
}
