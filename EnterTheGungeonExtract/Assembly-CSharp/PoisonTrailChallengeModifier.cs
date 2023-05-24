using System;
using UnityEngine;

// Token: 0x02001286 RID: 4742
public class PoisonTrailChallengeModifier : ChallengeModifier
{
	// Token: 0x06006A26 RID: 27174 RVA: 0x00299D60 File Offset: 0x00297F60
	private void Update()
	{
		if (BraveTime.DeltaTime <= 0f)
		{
			return;
		}
		if (this.m_goopPoints == null || this.m_goopPoints.Length != GameManager.Instance.AllPlayers.Length)
		{
			this.InitializePoints();
		}
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			this.m_goopPoints[i] = Vector2.SmoothDamp(this.m_goopPoints[i], GameManager.Instance.AllPlayers[i].specRigidbody.UnitCenter, ref this.m_goopVelocities[i], this.DampSmoothTime, this.MaxSmoothSpeed, Time.deltaTime);
			if (!GameManager.Instance.AllPlayers[i].IsGhost)
			{
				this.DoGoop(this.m_goopPoints[i]);
			}
		}
	}

	// Token: 0x06006A27 RID: 27175 RVA: 0x00299E4C File Offset: 0x0029804C
	private void InitializePoints()
	{
		this.m_goopPoints = new Vector2[GameManager.Instance.AllPlayers.Length];
		this.m_goopVelocities = new Vector2[GameManager.Instance.AllPlayers.Length];
		for (int i = 0; i < this.m_goopPoints.Length; i++)
		{
			this.m_goopPoints[i] = GameManager.Instance.AllPlayers[i].CenterPosition;
		}
	}

	// Token: 0x06006A28 RID: 27176 RVA: 0x00299EC4 File Offset: 0x002980C4
	private void DoGoop(Vector2 position)
	{
		if (BossKillCam.BossDeathCamRunning)
		{
			return;
		}
		DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.Goop).AddGoopCircle(position, this.GoopRadius, -1, false, -1);
	}

	// Token: 0x04006699 RID: 26265
	public GoopDefinition Goop;

	// Token: 0x0400669A RID: 26266
	public float GoopRadius = 1f;

	// Token: 0x0400669B RID: 26267
	public float DampSmoothTime = 0.25f;

	// Token: 0x0400669C RID: 26268
	private float MaxSmoothSpeed = 20f;

	// Token: 0x0400669D RID: 26269
	private Vector2[] m_goopVelocities;

	// Token: 0x0400669E RID: 26270
	private Vector2[] m_goopPoints;
}
