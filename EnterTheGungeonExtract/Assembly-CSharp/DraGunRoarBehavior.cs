using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DB0 RID: 3504
[InspectorDropdownName("Bosses/DraGun/RoarBehavior")]
public class DraGunRoarBehavior : BasicAttackBehavior
{
	// Token: 0x06004A2D RID: 18989 RVA: 0x0018D5E0 File Offset: 0x0018B7E0
	public override void Start()
	{
		base.Start();
		this.m_dragun = this.m_aiActor.GetComponent<DraGunController>();
		this.m_roarDummy = this.m_aiActor.transform.Find("RoarDummy").GetComponent<tk2dSpriteAnimator>();
	}

	// Token: 0x06004A2E RID: 18990 RVA: 0x0018D61C File Offset: 0x0018B81C
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x06004A2F RID: 18991 RVA: 0x0018D634 File Offset: 0x0018B834
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		this.m_aiActor.ToggleRenderers(false);
		this.m_dragun.head.OverrideDesiredPosition = new Vector2?(this.m_aiActor.transform.position + new Vector3(3.63f, 10.8f));
		this.m_roarDummy.gameObject.SetActive(true);
		this.m_roarDummy.GetComponent<Renderer>().enabled = true;
		this.m_roarDummy.Play("roar");
		this.Fire();
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004A30 RID: 18992 RVA: 0x0018D6E8 File Offset: 0x0018B8E8
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (!this.m_roarDummy.IsPlaying("roar"))
		{
			this.m_roarDummy.Play("blank");
			this.m_roarDummy.gameObject.SetActive(false);
			this.m_aiActor.ToggleRenderers(true);
			this.m_dragun.head.OverrideDesiredPosition = null;
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A31 RID: 18993 RVA: 0x0018D75C File Offset: 0x0018B95C
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_roarDummy.Play("blank");
		this.m_roarDummy.gameObject.SetActive(false);
		this.m_aiActor.ToggleRenderers(true);
		this.m_dragun.head.OverrideDesiredPosition = null;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004A32 RID: 18994 RVA: 0x0018D7C4 File Offset: 0x0018B9C4
	private void Fire()
	{
		if (!this.m_bulletSource)
		{
			this.m_bulletSource = this.ShootPoint.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_bulletSource.BulletScript = this.BulletScript;
		this.m_bulletSource.Initialize();
	}

	// Token: 0x04003EDD RID: 16093
	public GameObject ShootPoint;

	// Token: 0x04003EDE RID: 16094
	public BulletScriptSelector BulletScript;

	// Token: 0x04003EDF RID: 16095
	private DraGunController m_dragun;

	// Token: 0x04003EE0 RID: 16096
	private tk2dSpriteAnimator m_roarDummy;

	// Token: 0x04003EE1 RID: 16097
	private BulletScriptSource m_bulletSource;

	// Token: 0x04003EE2 RID: 16098
	private float m_timer;
}
