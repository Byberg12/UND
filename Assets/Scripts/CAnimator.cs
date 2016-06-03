using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CAnimatorStateInfo
{
    public bool bStartPlay;             //开始播放动作
    public float aniSpeed;          
    public AnimatorStateInfo pAnimatorStateInfo;
}

public class CAnimator :MonoBehaviour
{
    public string AnimatorName;

    public Animator m_pAnimator;

    private CAnimatorStateInfo m_pAniInfo;

    private int m_nLayer;           //动作的层级
    public string m_szAniName;


    private bool m_bPlayAction;     

    private const string BaseLayerName = "Base Layer";


    public void Start()
    {
        m_pAnimator = GetComponent<Animator>();
        m_nLayer = 0;

        m_pAniInfo = new CAnimatorStateInfo();
        m_pAniInfo.bStartPlay = false;
 
        m_bPlayAction = false;

        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
        }
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(100,100,100,100),"click"))
        {
            PlayAction(AnimatorName,1.0f,true);
        }
    }

    public bool Enabled
    {
        get 
        {
            return m_pAnimator.enabled;
        }
        set
        {
            m_pAnimator.enabled = value;
        }
    }

    public string PlayActionName
    {
        get
        {
            return m_szAniName;
        }
    }

    public void Play(string aniName)
    {
        m_pAnimator.Play(aniName);
    }

    //bRepeat为true时允许重复播放相同的动作
    public void PlayAction(string aniName, float speed = 1.0f,bool bRepeat = false)
    {
        if (null == m_pAnimator || !m_pAnimator.enabled)
        {
            return;
        }
        if (string.IsNullOrEmpty(aniName) || aniName == "0")
        {
            return;
        }

        if (!bRepeat && m_szAniName == aniName)
        {
            return;
        }

        //将上一个动作停止
        if (m_szAniName != aniName && !string.IsNullOrEmpty(m_szAniName))
        {
            m_pAnimator.SetBool(Animator.StringToHash(m_szAniName), false);
        }

        //播放新动作        
        m_pAnimator.SetBool(Animator.StringToHash(aniName), true);

        if (!m_pAnimator.GetBool(Animator.StringToHash(aniName)))
        {
            Debug.LogError(" Error! can not find animation : " + aniName + "  transform name : " + m_pAnimator.transform.name);
            return;
        }

        m_pAnimator.speed = speed;
        m_bPlayAction = true;
        m_szAniName = aniName;
        m_pAniInfo.bStartPlay = false;
    }


    void Update() 
    {
        if (null == m_pAnimator || !m_pAnimator.enabled) return;

        AnimatorStateInfo state = m_pAnimator.GetNextAnimatorStateInfo(0);
       
        //避免anystate多次触发
        if (m_bPlayAction)
        {
            if (state.nameHash == Animator.StringToHash(BaseLayerName + "." + m_szAniName))            
            {
                m_pAnimator.SetBool(Animator.StringToHash(m_szAniName), false);

                m_bPlayAction = false;
                m_pAniInfo.bStartPlay = true;
                m_pAniInfo.pAnimatorStateInfo = state;
                m_pAniInfo.aniSpeed = m_pAnimator.speed;
            }
        }
    }



    public CAnimatorStateInfo GetAnimatorInfo() 
    {
        return m_pAniInfo;
    }


    public float Speed
    {
        get
        {
            if (null == m_pAnimator || !m_pAnimator.enabled ) return 0f;

            return m_pAnimator.speed;
        }
        set
        {
            if (null == m_pAnimator || !m_pAnimator.enabled) return;

            m_pAnimator.speed = value;
        }
    }

    public void StopAnimation()
    {
        if (null == m_pAnimator || !m_pAnimator.enabled) return;

//         string szAniName = stStateData.GetActionName(eActionState.Idle);
//         m_pAnimator.SetBool(szAniName, true);                        

        m_pAnimator.SetBool(m_szAniName, false);
    }


}