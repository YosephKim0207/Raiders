using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FF7 RID: 4087
public class BossStatuesController : BraveBehaviour
{
	// Token: 0x17000CDE RID: 3294
	// (get) Token: 0x06005948 RID: 22856 RVA: 0x00221884 File Offset: 0x0021FA84
	public Vector2 PatternCenter
	{
		get
		{
			return this.m_patternCenter;
		}
	}

	// Token: 0x17000CDF RID: 3295
	// (get) Token: 0x06005949 RID: 22857 RVA: 0x0022188C File Offset: 0x0021FA8C
	// (set) Token: 0x0600594A RID: 22858 RVA: 0x00221894 File Offset: 0x0021FA94
	public int NumLivingStatues { get; set; }

	// Token: 0x17000CE0 RID: 3296
	// (get) Token: 0x0600594B RID: 22859 RVA: 0x002218A0 File Offset: 0x0021FAA0
	// (set) Token: 0x0600594C RID: 22860 RVA: 0x002218A8 File Offset: 0x0021FAA8
	public float MoveHopSpeed { get; set; }

	// Token: 0x17000CE1 RID: 3297
	// (get) Token: 0x0600594D RID: 22861 RVA: 0x002218B4 File Offset: 0x0021FAB4
	// (set) Token: 0x0600594E RID: 22862 RVA: 0x002218BC File Offset: 0x0021FABC
	public float MoveGravity { get; set; }

	// Token: 0x17000CE2 RID: 3298
	// (get) Token: 0x0600594F RID: 22863 RVA: 0x002218C8 File Offset: 0x0021FAC8
	// (set) Token: 0x06005950 RID: 22864 RVA: 0x002218D0 File Offset: 0x0021FAD0
	public float AttackHopSpeed { get; set; }

	// Token: 0x17000CE3 RID: 3299
	// (get) Token: 0x06005951 RID: 22865 RVA: 0x002218DC File Offset: 0x0021FADC
	// (set) Token: 0x06005952 RID: 22866 RVA: 0x002218E4 File Offset: 0x0021FAE4
	public float AttackGravity { get; set; }

	// Token: 0x17000CE4 RID: 3300
	// (get) Token: 0x06005953 RID: 22867 RVA: 0x002218F0 File Offset: 0x0021FAF0
	// (set) Token: 0x06005954 RID: 22868 RVA: 0x002218F8 File Offset: 0x0021FAF8
	public float? OverrideMoveSpeed { get; set; }

	// Token: 0x17000CE5 RID: 3301
	// (get) Token: 0x06005955 RID: 22869 RVA: 0x00221904 File Offset: 0x0021FB04
	public float CurrentMoveSpeed
	{
		get
		{
			if (this.IsTransitioning)
			{
				return this.transitionMoveSpeed;
			}
			if (this.OverrideMoveSpeed != null)
			{
				return this.OverrideMoveSpeed.Value;
			}
			return this.moveSpeed;
		}
	}

	// Token: 0x17000CE6 RID: 3302
	// (get) Token: 0x06005956 RID: 22870 RVA: 0x0022194C File Offset: 0x0021FB4C
	// (set) Token: 0x06005957 RID: 22871 RVA: 0x00221954 File Offset: 0x0021FB54
	public bool IsTransitioning { get; set; }

	// Token: 0x06005958 RID: 22872 RVA: 0x00221960 File Offset: 0x0021FB60
	public void Awake()
	{
		for (int i = 0; i < this.allStatues.Count; i++)
		{
			this.allStatues[i].healthHaver.OnPreDeath += this.OnStatueDeath;
		}
		this.NumLivingStatues = this.allStatues.Count;
		base.bulletBank.CollidesWithEnemies = false;
		this.m_patternCenter = base.transform.position.XY() + new Vector2((float)base.dungeonPlaceable.placeableWidth / 2f, (float)base.dungeonPlaceable.placeableHeight / 2f);
		this.RecalculateHopSpeeds();
		if (TurboModeController.IsActive)
		{
			this.moveSpeed *= TurboModeController.sEnemyMovementSpeedMultiplier;
		}
	}

	// Token: 0x06005959 RID: 22873 RVA: 0x00221A30 File Offset: 0x0021FC30
	public void Update()
	{
	}

	// Token: 0x0600595A RID: 22874 RVA: 0x00221A34 File Offset: 0x0021FC34
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600595B RID: 22875 RVA: 0x00221A3C File Offset: 0x0021FC3C
	public void RecalculateHopSpeeds()
	{
		this.MoveHopSpeed = 2f * (this.moveHopHeight / (0.5f * this.moveHopTime));
		this.MoveGravity = -this.MoveHopSpeed / (0.5f * this.moveHopTime);
		this.AttackHopSpeed = 2f * (this.attackHopHeight / (0.5f * this.attackHopTime));
		this.AttackGravity = -this.AttackHopSpeed / (0.5f * this.attackHopTime);
	}

	// Token: 0x0600595C RID: 22876 RVA: 0x00221ABC File Offset: 0x0021FCBC
	public float GetEffectiveMoveSpeed(float speed)
	{
		float num = this.moveHopTime + this.groundedTime;
		return speed * (this.moveHopTime / num);
	}

	// Token: 0x0600595D RID: 22877 RVA: 0x00221AE4 File Offset: 0x0021FCE4
	public void ClearBullets(Vector2 centerPoint)
	{
		base.StartCoroutine(this.HandleSilence(centerPoint, 30f, 30f));
	}

	// Token: 0x0600595E RID: 22878 RVA: 0x00221B00 File Offset: 0x0021FD00
	private IEnumerator HandleSilence(Vector2 centerPoint, float expandSpeed, float maxRadius)
	{
		float currentRadius = 0f;
		while (currentRadius < maxRadius)
		{
			currentRadius += expandSpeed * BraveTime.DeltaTime;
			SilencerInstance.DestroyBulletsInRange(centerPoint, currentRadius, true, false, null, false, null, false, null);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600595F RID: 22879 RVA: 0x00221B2C File Offset: 0x0021FD2C
	private void OnStatueDeath(Vector2 finalDeathDir)
	{
		for (int i = 0; i < this.allStatues.Count; i++)
		{
			if (this.allStatues[i])
			{
				if (!this.allStatues[i].healthHaver.IsDead)
				{
					this.allStatues[i].LevelUp();
				}
			}
		}
		this.NumLivingStatues--;
		if (this.NumLivingStatues == 0)
		{
			EncounterTrackable component = base.GetComponent<EncounterTrackable>();
			if (component != null)
			{
				GameStatsManager.Instance.HandleEncounteredObject(component);
			}
			GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_STATUES, true);
		}
	}

	// Token: 0x0400529A RID: 21146
	public List<BossStatueController> allStatues;

	// Token: 0x0400529B RID: 21147
	public float groundedTime = 0.15f;

	// Token: 0x0400529C RID: 21148
	public float moveSpeed = 5f;

	// Token: 0x0400529D RID: 21149
	public float transitionMoveSpeed = 10f;

	// Token: 0x0400529E RID: 21150
	public float moveHopHeight = 1.5f;

	// Token: 0x0400529F RID: 21151
	public float moveHopTime = 0.33f;

	// Token: 0x040052A0 RID: 21152
	public float attackHopHeight = 1.5f;

	// Token: 0x040052A1 RID: 21153
	public float attackHopTime = 0.33f;

	// Token: 0x040052A9 RID: 21161
	private Vector2 m_patternCenter;
}
