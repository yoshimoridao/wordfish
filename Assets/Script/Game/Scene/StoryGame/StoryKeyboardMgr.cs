using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryKeyboardMgr : GElement
{
    [System.Serializable]
    public class Button
    {
        public int m_Index;
        // character of btn
        public string m_Text;
        public Vector2 m_Coord;
        public GameObject m_Btn;
    }

    // ================================== VARIABLES ==================================
    #region Vars
    // reference vars
    public StoryPressedTextCont Ref_StoryPressedTextCont;
    public StoryAnswerCont Ref_StoryAnswerCont;
    // prefab vars
    public GameObject Pre_Btn;
    // const vars
    const float PRESSED_BTN_SCALE_RATIO = 0.15f;
    // private vars
    int m_JustPressedIndex = -1;
    [SerializeField]
    float m_ShrinkBtnRatio = 0.5f;
    string m_CurVoca = "";
    bool m_IsHoldModeActive = false;
    bool m_IsChangeNextVoca = false;
    [SerializeField]
    Vector2 m_HoldTime = new Vector2(0, 0.15f);
    List<Button> m_lBtns = new List<Button>();
    List<int> m_ListAvailPathIndex = new List<int>();
    List<int> m_ListPressedBtnIndex = new List<int>();
    // yoshimori added
#if _DEBUG
    [SerializeField]
    List<string> m_ListStrAvailPath = new List<string>();
#endif
    // yoshimori end
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

        if (!m_IsChangeNextVoca)
            UpdateControl(a_dt);
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
    public void GenKb(string voca, int[,] btnsTemplate)
    {
        // clear flag vars
        m_IsChangeNextVoca = false;
        // gen ANSWER
        m_CurVoca = voca;
        // update new vocabulary for "Pressed Text Cont"
        Ref_StoryPressedTextCont.InitCurVoca(voca);
        Ref_StoryAnswerCont.InitAnswer(voca);
        // gen KEYBOARD
        GenKeyboardZone(voca, btnsTemplate);
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void GenKeyboardZone(string voca, int[,] btnsTemplate)
    {
        // 3. GEN btns
        GenBtns(btnsTemplate);

        // 4. find all of available paths
        List<int> l_leftBtns = new List<int>();
        for (int i = 0; i < m_lBtns.Count; i++)
            l_leftBtns.Add(i);

        List<List<int>> l_availPath = new List<List<int>>();
        List<int> l_path = new List<int>();
        for (int i = 0; i < l_leftBtns.Count; i++)
        {
            l_path.Add(l_leftBtns[i]);
            List<int> lleftBtnsClone = new List<int>(l_leftBtns);
            lleftBtnsClone.RemoveAt(i);
            FindAllAvailPath(ref l_path, ref lleftBtnsClone, ref l_availPath);
            l_path.RemoveAt(l_path.Count - 1);
        }

        // 5. pick one PATH & set SPRITE
        m_ListAvailPathIndex = l_availPath[Random.RandomRange(0, l_availPath.Count)];
        for (int i = 0; i < m_ListAvailPathIndex.Count; i++)
        {
            int btnIndex = m_ListAvailPathIndex[i];
            if (btnIndex >= m_lBtns.Count)
                continue;
            Button btn = m_lBtns[btnIndex];
            btn.m_Text = voca[i].ToString();
            Sprite btnSprite = Resources.Load<Sprite>(AssetPathConstant.FOLDER_KEYBOARD_LETTERS_PATH + "/" + btn.m_Text);
            if (btnSprite)
            {
                btn.m_Btn.GetComponent<SpriteRenderer>().sprite = btnSprite;
                m_lBtns[btnIndex] = btn;
            }
        }
    }

    private void GenBtns(int[,] btnsTemplate)
    {
        Vector2 kbSize = new Vector2(btnsTemplate.GetUpperBound(0) + 1, btnsTemplate.GetUpperBound(1) + 1);
        float btnSizePercent = Mathf.Min(1 / kbSize.x, 1 / kbSize.y);
        Vector2 boundBtnsSize = btnSizePercent * new Vector2(kbSize.y, kbSize.x);
        // top left local pos
        Vector2 snapPos = new Vector2(-boundBtnsSize.x / 2.0f, boundBtnsSize.y / 2.0f);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        int btnIndex = 0;
        for (int i = 0; i < kbSize.x; i++)
        {
            for (int j = 0; j < kbSize.y; j++)
            {
                if (btnsTemplate[i, j] == 0)
                    continue;

                // Get || Gen BTN class
                Button genbtn = null;
                if (btnIndex < m_lBtns.Count)
                {
                    genbtn = m_lBtns[btnIndex];
                }
                else
                {
                    genbtn = new Button();
                    genbtn.m_Btn = Instantiate(Pre_Btn);
                    m_lBtns.Add(genbtn);
                }
                genbtn.m_Coord = new Vector2(i, j);
                genbtn.m_Index = btnIndex;

                // set NAME of btn (use NAME as INDEX)
                GameObject btnObj = m_lBtns[btnIndex].m_Btn;
                btnObj.name = btnIndex.ToString();
                // set SCALE of btns
                btnObj.transform.parent = null;
                SpriteRenderer btnSr = btnObj.GetComponent<SpriteRenderer>();
                Vector2 btnScale = btnObj.transform.localScale;
                btnScale.x = (sr.bounds.size.x * btnSizePercent) / btnSr.sprite.bounds.size.x;
                btnScale.y = (sr.bounds.size.y * btnSizePercent) / btnSr.sprite.bounds.size.y;
                btnScale.x *= m_ShrinkBtnRatio;
                btnScale.y *= m_ShrinkBtnRatio;
                btnObj.transform.localScale = btnScale;
                btnObj.transform.parent = transform;
                // set LOCAL POS of btns
                Vector3 btnPos = btnObj.transform.localPosition;
                btnPos = Vector2.zero;
                btnPos.x = snapPos.x + btnSizePercent * (j + 0.5f);
                btnPos.y = snapPos.y - btnSizePercent * (i + 0.5f);
                btnPos.z = Pre_Btn.transform.localPosition.z;
                btnObj.transform.localPosition = btnPos;

                btnIndex++;
            }
        }

        // REMOVE EXCESS btns
        for (int i = btnIndex; i < m_lBtns.Count; i++)
        {
            Button btnObj = m_lBtns[i];
            if (btnObj.m_Btn)
                Destroy(btnObj.m_Btn);
        }
        m_lBtns.RemoveRange(btnIndex, m_lBtns.Count - btnIndex);
    }

    private void FindAllAvailPath(ref List<int> l_path, ref List<int> l_leftBtn, ref List<List<int>> l_availPath)
    {
        // store available path
        if (l_leftBtn.Count == 0 && l_path.Count == m_lBtns.Count)
        {
            l_availPath.Add(new List<int>(l_path));

            // yoshimori added
#if _DEBUG
            string strPath = "";
            foreach (var path in l_path)
                strPath += path + "_";
            strPath.Remove(strPath.Length - 1);
            m_ListStrAvailPath.Add(strPath);
#endif
            // yoshimori end

            return;
        }

        // current btn
        int currBtn = 0;
        Vector2 currBtnCoord = Vector2.zero;
        if (l_path.Count > 0)
        {
            currBtn = l_path[l_path.Count - 1];
            currBtnCoord = m_lBtns[currBtn].m_Coord;
        }

        // finding next btn
        for (int i = 0; i < l_leftBtn.Count; i++)
        {
            int nextBtn = l_leftBtn[i];
            Vector2 nextBtnCoord = m_lBtns[nextBtn].m_Coord;
            if (IsCoord2BtnAvailable(currBtnCoord, nextBtnCoord))
            {
                // store path
                l_path.Add(nextBtn);

                List<int> lleftBtnClone = new List<int>(l_leftBtn);
                lleftBtnClone.RemoveAt(i);

                FindAllAvailPath(ref l_path, ref lleftBtnClone, ref l_availPath);

                // revert path
                if (l_path.Count > 0)
                    l_path.RemoveAt(l_path.Count - 1);
            }
        }
    }

    private bool IsCoord2BtnAvailable(Vector2 btnCoord1, Vector2 btnCoord2)
    {
        if (Mathf.Abs(btnCoord1.x - btnCoord2.x) <= 1 &&
              Mathf.Abs(btnCoord1.y - btnCoord2.y) <= 1)
            return true;
        return false;
    }

    // ============================== UPDATE ==============================
    private void UpdateControl(float a_dt)
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            // counting time to active HOLD mode
            if (!m_IsHoldModeActive)
            {
                m_HoldTime.x += a_dt;
                if (m_HoldTime.x >= m_HoldTime.y)
                {
                    m_HoldTime.x = 0;
                    m_IsHoldModeActive = true;
                }
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward);
            if (hit.collider != null)
            {
                int btnIndex = int.Parse(hit.collider.gameObject.name);
                if (btnIndex == m_JustPressedIndex)
                    return;
                m_JustPressedIndex = btnIndex;
                if (btnIndex < m_lBtns.Count)
                {
                    Button btn = m_lBtns[btnIndex];
                    if (m_ListPressedBtnIndex.Count > 0)
                    {
                        Button lastBtn = m_lBtns[m_ListPressedBtnIndex[m_ListPressedBtnIndex.Count - 1]];
                        if (!IsCoord2BtnAvailable(btn.m_Coord, lastBtn.m_Coord))
                        {
                            // release all btns in case of TOUCHING OUT (TOUCHING mode)
                            if (!m_IsHoldModeActive)
                            {
                                RefreshBtns(0, m_ListPressedBtnIndex.Count, true);
                            }
                            return;
                        }
                    }

                    // REVERT btns
                    if (m_ListPressedBtnIndex.Contains(btnIndex))
                    {
                        int startIndex = m_ListPressedBtnIndex.FindIndex(x => x == btnIndex);
                        startIndex += m_IsHoldModeActive ? 1 : 0;
                        RefreshBtns(startIndex, m_ListPressedBtnIndex.Count - startIndex, true);
                    }
                    // TOUCHING btns
                    else
                    {
                        m_ListPressedBtnIndex.Add(btnIndex);
                        RefreshBtns(m_ListPressedBtnIndex.Count - 1, 1);
                    }
                }
            }
            else
            {
                // release all btns in case of TOUCHING OUT (TOUCHING mode)
                if (!m_IsHoldModeActive && Input.GetMouseButtonDown(0))
                {
                    RefreshBtns(0, m_ListPressedBtnIndex.Count, true);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // clear pressed btns (if is in HOLD mode || or CLICK mode in case of full answer)
            if (m_ListPressedBtnIndex.Count == m_CurVoca.Length)
            {
                CheckAnswer();
                if (!m_IsHoldModeActive)
                    RefreshBtns(0, m_ListPressedBtnIndex.Count, true);
            }
            if (m_IsHoldModeActive)
            {
                RefreshBtns(0, m_ListPressedBtnIndex.Count, true);
            }
            // clear time flag
            m_JustPressedIndex = -1;
            m_HoldTime.x = 0;
            m_IsHoldModeActive = false;
        }
    }

    // ============================== TRIGGER EVENT ==============================
    private void RefreshBtns(int startIndex, int count, bool isRevert = false)
    {
        if (startIndex >= m_ListPressedBtnIndex.Count)
            return;
        for (int i = startIndex; i < startIndex + count; i++)
        {
            if (i >= m_ListPressedBtnIndex.Count)
                break;

            int btnIndex = m_ListPressedBtnIndex[i];
            if (btnIndex >= m_lBtns.Count)
                break;

            GameObject btnObj = m_lBtns[btnIndex].m_Btn;
            Vector3 localScale = btnObj.transform.localScale;
            localScale.x += (PRESSED_BTN_SCALE_RATIO * (!isRevert ? 1 : -1));
            localScale.y += (PRESSED_BTN_SCALE_RATIO * (!isRevert ? 1 : -1));
            btnObj.transform.localScale = localScale;
        }

        // clear same btns in list
        if (isRevert)
            m_ListPressedBtnIndex.RemoveRange(startIndex, count);

        // show pressed text
        ShowPressedText();
    }

    private void ShowPressedText()
    {
        string pressedText = GetPressedText();
        Ref_StoryPressedTextCont.ShowText(pressedText);
    }

    private void CheckAnswer()
    {
        string pressedText = GetPressedText();
        m_IsChangeNextVoca = Ref_StoryAnswerCont.CheckAnswer(pressedText);
    }

    private string GetPressedText()
    {
        string pressText = "";
        for (int i = 0; i < m_ListPressedBtnIndex.Count; i++)
        {
            int pressedBtnIndex = m_ListPressedBtnIndex[i];
            if (pressedBtnIndex >= m_lBtns.Count)
                continue;
            pressText += m_lBtns[pressedBtnIndex].m_Text;
        }
        return pressText;
    }
    #endregion
}
