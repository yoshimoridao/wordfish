using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FishHandler
{
	/// <summary>
	/// Handler params
	/// </summary>
	FishHPHandler m_HPHandler;
	FishShapeHandler m_ShapeHandler;
	FishDamHandler m_DamHandler;
	FishMovementHandler m_MovementHandler;
	MoneyGenerationHandler m_MoneyHandler;

	/// <summary>
	/// Info for handler
	/// </summary>
	FishInfo m_Info;

	public FishHandler (FishInfo info, Transform trans)
	{
		m_Info = info;

		m_HPHandler = new FishHPHandler (info.m_FishHP);
		m_ShapeHandler = new FishShapeHandler (info.m_FishShape);
		m_DamHandler = new FishDamHandler (info.m_FishDam);
		m_MovementHandler = new FishMovementHandler (trans);
		m_MoneyHandler = new MoneyGenerationHandler (info.m_Money);
	}

    //public void UpdateFishStatus (float time)
    //{
    //    m_MovementHandler.UpdateMovement (time);
    //}

    //public void FixedUpdateFishStatus (float time)
    //{
    //    m_MovementHandler.UpdateMovement(time);
    //}

    public void UpdateHandler (float a_time)
    {
        if (m_HPHandler != null)
            m_HPHandler.UpdateHP(a_time);

        if (m_MovementHandler != null)
            m_MovementHandler.UpdateMovement(a_time);

        if (m_MoneyHandler != null)
            m_MoneyHandler.UpdateMoney(a_time);
    }

}

class FishMovementHandler
{
	Transform m_Transform;

	#region fish velocity
    float m_MaxVelocity;
	float m_Velocity;
    float m_Acceleration;
	int m_Direction;
    Vector3 m_Destination;

    Quaternion m_Quaternion;
    Vector3 m_EulerAngle;
    float m_TargetDegree;
    bool m_ShouldRotate;
	#endregion

	public FishMovementHandler (Transform tran)
	{
		m_Transform = tran;

        m_MaxVelocity = 2;
		m_Velocity = 0;
        m_Acceleration = 5;
		m_Direction = -1;

		Vector3 randompos = new Vector3 (UtilityClass.GetRandomFloat (-Constant.HALF_WIDTH, Constant.HALF_WIDTH),
			                    UtilityClass.GetRandomFloat (-Constant.HALF_HEIGHT, Constant.HALF_HEIGHT), 0);
        m_EulerAngle = Vector3.zero;
        m_TargetDegree = 0;
        m_ShouldRotate = false;

		m_Transform.position = randompos;

        m_Destination = m_Transform.position;
	}

	public void UpdateMovement (float time)
	{
        m_Velocity += time * m_Acceleration;
        if (m_Velocity >= m_MaxVelocity)
            m_Velocity = m_MaxVelocity;

        float distance = m_Velocity * m_Direction * time;

        float x = m_Destination.x + distance;

		if (x >= Constant.HALF_WIDTH) {
			x = Constant.HALF_WIDTH;
			m_Direction = -1;
            m_Velocity = 0;

            m_TargetDegree = 0;
            m_ShouldRotate = true;
		}

		if (x <= -Constant.HALF_WIDTH) {
			x = -Constant.HALF_WIDTH;
			m_Direction = 1;
            m_Velocity = 0;

            m_TargetDegree += 180;// -m_Transform.rotation.eulerAngles.y;
            m_ShouldRotate = true;
		}

        float y = UtilityClass.GetRandomFloat(m_Destination.y - Constant.FISH_MOVEMENT_OFFSETY, m_Destination.y + Constant.FISH_MOVEMENT_OFFSETY);

        if (y <= -Constant.HALF_HEIGHT)
            y = -Constant.HALF_HEIGHT;
        else if (y >= Constant.HALF_HEIGHT)
            y = Constant.HALF_HEIGHT;

        m_Destination.x = x;
        m_Destination.y = y;
        m_Destination.z = 0;

        //m_Quaternion = Quaternion.LookRotation(m_Destination);

        //m_EulerAngle = Vector3.zero;
        //int randomAxis = UtilityClass.GetRandomFromRange(0, 2);
        //switch (randomAxis)
        //{
        //    case 0:
        //        m_EulerAngle.x = m_Quaternion.eulerAngles.x;
        //        break;
        //    case 1:
        //        m_EulerAngle.y = m_Quaternion.eulerAngles.y;
        //        break;
        //    case 2:
        //        m_EulerAngle.z = m_Quaternion.eulerAngles.z;
        //        break;
        //}

        m_EulerAngle.x = m_Quaternion.eulerAngles.x;
        m_EulerAngle.y = m_Quaternion.eulerAngles.y;
        m_EulerAngle.z = m_Quaternion.eulerAngles.z;
        //m_Quaternion.eulerAngles = new Vector3(m_Quaternion.eulerAngles.x, m_Quaternion.eulerAngles.y, 0);
        //m_Quaternion = Quaternion.Euler(m_EulerAngle);
        //m_Quaternion.Set(m_EulerAngle.x, m_EulerAngle.y, m_EulerAngle.z, 0);

        m_Transform.position = Vector3.MoveTowards(m_Transform.position, m_Destination, m_Velocity * time);

        if (m_ShouldRotate)
        {
            m_Quaternion = Quaternion.Euler(m_Transform.rotation.eulerAngles.x, m_TargetDegree, m_Transform.rotation.eulerAngles.z);
            m_Transform.rotation = Quaternion.RotateTowards(m_Transform.rotation, m_Quaternion, m_Velocity);

            if (m_Transform.rotation.eulerAngles.y == m_TargetDegree)
                m_ShouldRotate = false;
        }
        //Quaternion temp = Quaternion.RotateTowards(m_Transform.rotation, m_Quaternion, 0.05f);
        //m_Transform.Rotate(new Vector3(temp.eulerAngles.x, temp.eulerAngles.y, 0));
        //m_Transform.Rotate(temp.eulerAngles.x, temp.eulerAngles.y, 0);
        //m_Transform.Rotate(Vector3.MoveTowards(m_Transform.rotation.eulerAngles, m_EulerAngle, 0.25f));

		//m_Transform.Translate (distance, 0, 0);
	}
}


class FishHPHandler
{
    #region vars
    private FishHP m_HP;

    float m_LostAmount;
    //float m_StartTime = 0;
    #endregion

    #region public methods
    public FishHPHandler (FishHP hp)
	{
		m_HP = hp;
        //m_StartTime = hp.m_CurrentTime;
	}

    public void UpdateHP(float time)
    {
        m_HP.m_CurrentTime -= time;
        if (m_HP.m_CurrentTime < 0)
            m_HP.m_CurrentTime = 0;

        float ratio = time / m_HP.m_MaxTime;
        m_HP.m_CurrentHP -= ratio * m_HP.m_MaxHP;
        if (m_HP.m_CurrentHP < 0)
            m_HP.m_CurrentHP = 0;
    }


    public void EatFood (float amount)
    {
        m_HP.m_CurrentHP += amount;
        if (m_HP.m_CurrentHP > m_HP.m_MaxHP)
            m_HP.m_CurrentHP = m_HP.m_MaxHP;

        float ratio = amount / m_HP.m_MaxHP;
        m_HP.m_CurrentTime += ratio * m_HP.m_MaxTime;
        if (m_HP.m_CurrentTime > m_HP.m_MaxTime)
            m_HP.m_CurrentTime = m_HP.m_MaxTime;
    }
    #endregion

    #region  private methods

    #endregion
}


class FishShapeHandler
{
	FishShape m_Shape;

	public FishShapeHandler (FishShape shape)
	{
		m_Shape = shape;
	}

//	public void InitShape (FishShape shape)
//	{
//		m_Shape = shape;
//	}
}



class FishDamHandler
{
	FishDam m_Dam;

	public FishDamHandler (FishDam dam)
	{
		m_Dam = dam;
	}

//	public void InitDam (FishDam dam)
//	{
//		m_Dam = dam;
//	}
}



class MoneyGenerationHandler
{
    #region vars
    MoneyGeneration m_Money;

    float m_StartTime = 0;
    int m_Golds = 0;
    #endregion

    #region public methods
    public MoneyGenerationHandler(MoneyGeneration money)
	{
		m_Money = money;
        ResetTime();
        //m_StartTime = 0;
	}

    public void UpdateMoney (float a_dt)
    {
        m_StartTime += a_dt;

        if (m_StartTime >= m_Money.m_ProductTime)
        {
            IncreaseGolds();
            ResetTime();
        }
    }
    #endregion

    #region private methods
    void IncreaseGolds ()
    {
        m_Golds += m_Money.m_Money;
    }

    void ResetTime ()
    {
        m_StartTime = 0;
    }

    void ResetGolds ()
    {
        m_Golds = 0;
    }
    #endregion

    //	public void InitMoneyHandler (MoneyGeneration money)
//	{
//		m_Money = money;
//	}
}