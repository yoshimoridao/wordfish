using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryAnswerCont : GElement 
{
    // ================================== VARIABLES ==================================
    #region Vars
    // prefab vars
    public GameObject Pre_AnsElement;

    // private vars
    string m_Result = "";
    List<GameObject> m_lLetterObj = new List<GameObject>();
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
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void InitAnswer(string a_Answer)
    {
        m_Result = a_Answer;
        // Generate all letters for answer text
        int turn = a_Answer.Length - m_lLetterObj.Count;
        for (int i = 0; i < turn; i++)
        {
            GameObject ansObj = Instantiate(Pre_AnsElement, transform);
            ansObj.SetActive(false);    // default hide letters
            m_lLetterObj.Add(ansObj);
        }

        // Order letters
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float btnSize = Mathf.Min(sr.bounds.size.y, sr.bounds.size.x / (float)a_Answer.Length);
        Vector2 beginPos = transform.position;
        beginPos.x -= (btnSize * a_Answer.Length) / 2.0f;
        for (int i = 0; i < m_lLetterObj.Count; i++)
        {
            GameObject ansBtn = m_lLetterObj[i];
            // invisible surplus letter
            if (i >= a_Answer.Length)
            {
                ansBtn.SetActive(false);
                continue;
            }

            ansBtn.SetActive(true);
            GameObject letterOfBtnObj = ansBtn.transform.GetChild(0).gameObject;
            letterOfBtnObj.SetActive(false); //=> hide answer
            SpriteRenderer btnSr = ansBtn.GetComponent<SpriteRenderer>();
            // set SCALE of btn
            Vector3 btnScale = new Vector3(btnSize / btnSr.sprite.bounds.size.x,
                btnSize / btnSr.sprite.bounds.size.y, 1.0f);
            UtilityClass.SetLossyScale(ref ansBtn, btnScale);

            // set POS of btn
            Vector3 btnPos = ansBtn.transform.position;
            btnPos.x = beginPos.x + btnSize * (i + 0.5f);
            btnPos.y = beginPos.y;
            ansBtn.transform.position = btnPos;

            // set SPRITE of btn
            Sprite btnSprite = Resources.Load<Sprite>(AssetPathConstant.FOLDER_ANSWER_LETTERS_PATH + "/" + a_Answer[i].ToString());
            if (btnSprite)
            {
                //=> set alpha sprite for the letter
                SpriteRenderer letterSr = letterOfBtnObj.transform.GetComponent<SpriteRenderer>();
                letterSr.sprite = btnSprite;
            }
        }
    }

    public bool CheckAnswer(string a_Ans)
    {
        if (a_Ans.Length != a_Ans.Length)
            return false;

        for (int i = 0; i < a_Ans.Length; i++)
        {
            if (string.Compare(m_Result[i].ToString(), a_Ans[i].ToString()) == 0)
            {
                GameObject ansObj = m_lLetterObj[i];
                ansObj.transform.GetChild(0).gameObject.SetActive(true); //=> Visible letter of btn
            }
        }

        // The ans match 100% vs result -> change next voca
        if (string.Compare(m_Result, a_Ans) == 0)
        {
            if (Ref_GScene.GetType() == typeof(StoryGameMgr)) //=> this for Normal mode
                ((StoryGameMgr)Ref_GScene).PIsChangeNextVoca = true;
            else if (Ref_GScene.GetType() == typeof(StoryGameBossMgr)) //=> this for Boss mode
                ((StoryGameBossMgr)Ref_GScene).PIsChangeNextVoca = true;
            else if (Ref_GScene.GetType() == typeof(StoryMultiplayer)) //=> this for Multiplayer mode
                ((StoryMultiplayer)Ref_GScene).PIsChangeNextVoca = true;

            return true;
        }
        return false;
    }
    #endregion
}
