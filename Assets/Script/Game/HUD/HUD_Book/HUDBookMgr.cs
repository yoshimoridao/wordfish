using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HUDBookMgr : GHUD
{
    // ================================== VARIABLES ==================================
    #region Vars
    // private vars
    [SerializeField]
    private Text m_TopicText;
    [SerializeField]
    private Text m_VocabularyText;
    [SerializeField]
    private Text m_DescriptionText;
    [SerializeField]
    private Image m_TrashImg;
    [SerializeField]
    private Button m_NextBtn;
    [SerializeField]
    private Button m_PrevBtn;
    private VocasInfo m_VocasInfo;
    private int m_TopicIndex;
    private int m_VocaIndex;
    #endregion

    // =================================== OVERRIDE func ===================================
    #region OVERRIDE func
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

    // override HUD
    public override void Init(HUDInfo a_HUDInfo)
    {
        base.Init(a_HUDInfo);

        // default topic & voca index = 0
        m_TopicIndex = m_VocaIndex = 0;
        m_VocasInfo = DbMgr.s_Instance.GetVocasInfo(m_TopicIndex);
        RefreshBookContent();
        CheckingArrowButtonState();

        // listen event of button
        m_NextBtn.onClick.AddListener(() => OnClickArrowBtn(true));
        m_PrevBtn.onClick.AddListener(() => OnClickArrowBtn(false));
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void RefreshBookContent()
    {
        // set Vocabulary & Description text
        if (m_VocaIndex >= 0 && m_VocaIndex < m_VocasInfo.m_lVocas.Count)
        {
            VocaInfo vocaInfo = m_VocasInfo.m_lVocas[m_VocaIndex];
            m_VocabularyText.text = vocaInfo.m_Voca.ToUpper();
            m_DescriptionText.text = vocaInfo.m_Def;
        }
        // set Topic text
        TopicInfo topicInfo = DbMgr.s_Instance.GetTopicInfo(m_TopicIndex);
        if (topicInfo != null)
        {
            m_TopicText.text = topicInfo.m_Topic.ToUpper();
        }

        // refresh Trash img
        Sprite trashSprite = Resources.Load<Sprite>(AssetPathConstant.FOLDER_PROGRESS_TRASH_PATH + "/" + m_TopicIndex);
        if (trashSprite)
        {
            m_TrashImg.sprite = trashSprite;
            // set size delta of trash
            RectTransform trashRt = m_TrashImg.GetComponent<RectTransform>();
            trashRt.sizeDelta = new Vector2(trashSprite.rect.width, trashSprite.rect.height);
        }
    }

    private void OnClickArrowBtn(bool a_IsNextBtn)
    {
        int nextVocaIndex = m_VocaIndex + (a_IsNextBtn ? 1 : -1);
        // change to next topic
        if (nextVocaIndex >= m_VocasInfo.m_lVocas.Count)
        {
            OnChangeNextTopic(true);
        }
        // change to prev topic
        else if (nextVocaIndex < 0)
        {
            OnChangeNextTopic(false);
        }
        else
        {
            m_VocaIndex = nextVocaIndex;
            RefreshBookContent();
        }

        CheckingArrowButtonState();
    }

    private void OnChangeNextTopic(bool a_IsNext)
    {
        int nextTopicIndex = m_TopicIndex + (a_IsNext ? 1 : -1);
        VocasInfo nextVocasInfo = DbMgr.s_Instance.GetVocasInfo(nextTopicIndex);
        if (nextVocasInfo != null)
        {
            m_TopicIndex = nextTopicIndex;
            m_VocasInfo = nextVocasInfo;
            m_VocaIndex = a_IsNext ? 0 : m_VocasInfo.m_lVocas.Count - 1;

            RefreshBookContent();
        }
    }
    private void CheckingArrowButtonState()
    {
        // Disable interactable of next Btn
        if (m_VocaIndex == m_VocasInfo.m_lVocas.Count - 1)
        {
            int nextTopicIndex = m_TopicIndex + 1;
            if (DbMgr.s_Instance.GetVocasInfo(nextTopicIndex) == null)
            {
                m_NextBtn.interactable = false;
                return;
            }
        }
        // Disable interactable of prev btn
        if (m_VocaIndex == 0)
        {
            int prevTopicIndex = m_TopicIndex - 1;
            if (DbMgr.s_Instance.GetVocasInfo(prevTopicIndex) == null)
            {
                m_PrevBtn.interactable = false;
                return;
            }
        }

        m_NextBtn.interactable = true;
        m_PrevBtn.interactable = true;
    }
    #endregion
}
