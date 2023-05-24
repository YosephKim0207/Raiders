using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using Pathfinding;
using UnityEngine;

// Token: 0x02000D36 RID: 3382
public class RemoteShootBehavior : BasicAttackBehavior
{
	// Token: 0x0600476C RID: 18284 RVA: 0x001765AC File Offset: 0x001747AC
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x0600476D RID: 18285 RVA: 0x001765C4 File Offset: 0x001747C4
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
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		this.m_previousSpawnCells.Clear();
		this.ChooseSpawnLocation();
		IntVector2? spawnCell = this.m_spawnCell;
		if (spawnCell == null)
		{
			return BehaviorResult.Continue;
		}
		if (!string.IsNullOrEmpty(this.TellAnim))
		{
			this.m_aiAnimator.PlayUntilFinished(this.TellAnim, true, null, -1f, false);
			if (this.StopDuringAnimation)
			{
				if (this.HideGun)
				{
					this.m_aiShooter.ToggleGunAndHandRenderers(false, "SummonEnemyBehavior");
				}
				this.m_aiActor.ClearPath();
			}
		}
		if (!string.IsNullOrEmpty(this.TellVfx))
		{
			this.m_aiAnimator.PlayVfx(this.TellVfx, null, null, null);
		}
		this.m_timer = this.TellTime;
		if (this.m_aiActor && this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(true, "SummonEnemyBehavior");
		}
		this.m_state = RemoteShootBehavior.State.Casting;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x0600476E RID: 18286 RVA: 0x0017671C File Offset: 0x0017491C
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_state == RemoteShootBehavior.State.Casting)
		{
			if (this.m_timer <= 0f)
			{
				this.m_shotsRemaining = ((!this.Multifire) ? 1 : UnityEngine.Random.Range(this.MinShots, this.MaxShots + 1));
				Vector2 clearanceOffset = Pathfinder.GetClearanceOffset(this.m_spawnCell.Value, this.RemoteFootprint);
				if (!string.IsNullOrEmpty(this.ShootVfx))
				{
					this.m_aiAnimator.PlayVfx(this.ShootVfx, null, null, null);
				}
				if (!string.IsNullOrEmpty(this.RemoteVfx))
				{
					AIAnimator aiAnimator = this.m_aiAnimator;
					string text = this.RemoteVfx;
					Vector2? vector = new Vector2?(clearanceOffset);
					aiAnimator.PlayVfx(text, null, null, vector);
				}
				SpawnManager.SpawnBulletScript(this.m_aiActor, this.remoteBulletScript, new Vector2?(clearanceOffset), null, false, null);
				this.m_state = RemoteShootBehavior.State.Firing;
				this.m_shotsRemaining--;
				this.m_timer = ((this.m_shotsRemaining <= 0) ? this.FireTime : this.MidShotTime);
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.m_state == RemoteShootBehavior.State.Firing)
		{
			if (this.m_timer <= 0f)
			{
				if (this.m_shotsRemaining > 0)
				{
					this.ChooseSpawnLocation();
					if (this.m_spawnCell != null)
					{
						Vector2 clearanceOffset2 = Pathfinder.GetClearanceOffset(this.m_spawnCell.Value, this.RemoteFootprint);
						if (!string.IsNullOrEmpty(this.RemoteVfx))
						{
							AIAnimator aiAnimator2 = this.m_aiAnimator;
							string text = this.RemoteVfx;
							Vector2? vector = new Vector2?(clearanceOffset2);
							aiAnimator2.PlayVfx(text, null, null, vector);
						}
						SpawnManager.SpawnBulletScript(this.m_aiActor, this.remoteBulletScript, new Vector2?(clearanceOffset2), null, false, null);
					}
					this.m_shotsRemaining--;
					this.m_timer = ((this.m_shotsRemaining <= 0) ? this.FireTime : this.MidShotTime);
					return ContinuousBehaviorResult.Continue;
				}
				if (!string.IsNullOrEmpty(this.PostFireAnim))
				{
					this.m_state = RemoteShootBehavior.State.PostFire;
					this.m_aiAnimator.PlayUntilFinished(this.PostFireAnim, false, null, -1f, false);
					return ContinuousBehaviorResult.Continue;
				}
				return ContinuousBehaviorResult.Finished;
			}
		}
		else if (this.m_state == RemoteShootBehavior.State.PostFire && !this.m_aiAnimator.IsPlaying(this.PostFireAnim))
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x0600476F RID: 18287 RVA: 0x001769AC File Offset: 0x00174BAC
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (!string.IsNullOrEmpty(this.TellAnim))
		{
			this.m_aiAnimator.EndAnimationIf(this.TellAnim);
		}
		if (!string.IsNullOrEmpty(this.TellVfx))
		{
			this.m_aiAnimator.StopVfx(this.TellVfx);
		}
		if (!string.IsNullOrEmpty(this.ShootVfx))
		{
			this.m_aiAnimator.StopVfx(this.ShootVfx);
		}
		if (!string.IsNullOrEmpty(this.RemoteVfx))
		{
			this.m_aiAnimator.StopVfx(this.RemoteVfx);
		}
		if (!string.IsNullOrEmpty(this.PostFireAnim))
		{
			this.m_aiAnimator.EndAnimationIf(this.PostFireAnim);
		}
		if (this.HideGun)
		{
			this.m_aiShooter.ToggleGunAndHandRenderers(true, "SummonEnemyBehavior");
		}
		if (this.m_aiActor && this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(false, "SummonEnemyBehavior");
		}
		this.m_state = RemoteShootBehavior.State.Idle;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004770 RID: 18288 RVA: 0x00176AD4 File Offset: 0x00174CD4
	private void ChooseSpawnLocation()
	{
		if (!this.m_aiActor.TargetRigidbody)
		{
			this.m_spawnCell = null;
			return;
		}
		Vector2 vector = BraveUtility.ViewportToWorldpoint(new Vector2(0f, 0f), ViewportType.Gameplay);
		Vector2 vector2 = BraveUtility.ViewportToWorldpoint(new Vector2(1f, 1f), ViewportType.Gameplay);
		IntVector2 bottomLeft = vector.ToIntVector2(VectorConversions.Ceil);
		IntVector2 topRight = vector2.ToIntVector2(VectorConversions.Floor) - IntVector2.One;
		Vector2 targetCenter = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
		Vector2? additionalTargetCenter = null;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && this.m_aiActor.PlayerTarget is PlayerController)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(this.m_aiActor.PlayerTarget as PlayerController);
			if (otherPlayer)
			{
				additionalTargetCenter = new Vector2?(otherPlayer.specRigidbody.GetUnitCenter(ColliderType.HitBox));
			}
		}
		float minDistanceSquared = this.MinRadius * this.MinRadius;
		float maxDistanceSquared = this.MaxRadius * this.MaxRadius;
		CellValidator cellValidator = delegate(IntVector2 c)
		{
			for (int i = 0; i < this.RemoteFootprint.x; i++)
			{
				for (int j = 0; j < this.RemoteFootprint.y; j++)
				{
					if (GameManager.Instance.Dungeon.data.isTopWall(c.x + i, c.y + j))
					{
						return false;
					}
				}
			}
			if (this.DefineRadius)
			{
				float num = (float)c.x + 0.5f - targetCenter.x;
				float num2 = (float)c.y + 0.5f - targetCenter.y;
				float num3 = num * num + num2 * num2;
				if (num3 < minDistanceSquared || num3 > maxDistanceSquared)
				{
					return false;
				}
				if (additionalTargetCenter != null)
				{
					num = (float)c.x + 0.5f - additionalTargetCenter.Value.x;
					num2 = (float)c.y + 0.5f - additionalTargetCenter.Value.y;
					num3 = num * num + num2 * num2;
					if (num3 < minDistanceSquared || num3 > maxDistanceSquared)
					{
						return false;
					}
				}
			}
			if (c.x < bottomLeft.x || c.y < bottomLeft.y || c.x + this.m_aiActor.Clearance.x - 1 > topRight.x || c.y + this.m_aiActor.Clearance.y - 1 > topRight.y)
			{
				return false;
			}
			for (int k = 0; k < this.m_previousSpawnCells.Count; k++)
			{
				if (c.x == this.m_previousSpawnCells[k].x && c.y == this.m_previousSpawnCells[k].y)
				{
					return false;
				}
			}
			return true;
		};
		this.m_spawnCell = this.m_aiActor.ParentRoom.GetRandomAvailableCell(new IntVector2?(this.RemoteFootprint), new CellTypes?(CellTypes.FLOOR | CellTypes.PIT), false, cellValidator);
		IntVector2? spawnCell = this.m_spawnCell;
		if (spawnCell != null)
		{
			this.m_previousSpawnCells.Add(this.m_spawnCell.Value);
		}
	}

	// Token: 0x04003A68 RID: 14952
	public bool DefineRadius;

	// Token: 0x04003A69 RID: 14953
	[InspectorIndent]
	[InspectorShowIf("DefineRadius")]
	public float MinRadius;

	// Token: 0x04003A6A RID: 14954
	[InspectorShowIf("DefineRadius")]
	[InspectorIndent]
	public float MaxRadius;

	// Token: 0x04003A6B RID: 14955
	public IntVector2 RemoteFootprint = new IntVector2(1, 1);

	// Token: 0x04003A6C RID: 14956
	public float TellTime;

	// Token: 0x04003A6D RID: 14957
	public BulletScriptSelector remoteBulletScript;

	// Token: 0x04003A6E RID: 14958
	public float FireTime;

	// Token: 0x04003A6F RID: 14959
	public bool Multifire;

	// Token: 0x04003A70 RID: 14960
	[InspectorShowIf("Multifire")]
	[InspectorIndent]
	public int MinShots = 2;

	// Token: 0x04003A71 RID: 14961
	[InspectorIndent]
	[InspectorShowIf("Multifire")]
	public int MaxShots = 3;

	// Token: 0x04003A72 RID: 14962
	[InspectorIndent]
	[InspectorShowIf("Multifire")]
	public float MidShotTime;

	// Token: 0x04003A73 RID: 14963
	[InspectorCategory("Visuals")]
	public bool StopDuringAnimation = true;

	// Token: 0x04003A74 RID: 14964
	[InspectorCategory("Visuals")]
	public string TellAnim;

	// Token: 0x04003A75 RID: 14965
	[InspectorCategory("Visuals")]
	public string TellVfx;

	// Token: 0x04003A76 RID: 14966
	[InspectorCategory("Visuals")]
	public string ShootVfx;

	// Token: 0x04003A77 RID: 14967
	[InspectorCategory("Visuals")]
	public string RemoteVfx;

	// Token: 0x04003A78 RID: 14968
	[InspectorCategory("Visuals")]
	public string PostFireAnim;

	// Token: 0x04003A79 RID: 14969
	[InspectorCategory("Visuals")]
	public bool HideGun;

	// Token: 0x04003A7A RID: 14970
	private RemoteShootBehavior.State m_state;

	// Token: 0x04003A7B RID: 14971
	private IntVector2? m_spawnCell;

	// Token: 0x04003A7C RID: 14972
	private List<IntVector2> m_previousSpawnCells = new List<IntVector2>();

	// Token: 0x04003A7D RID: 14973
	private float m_timer;

	// Token: 0x04003A7E RID: 14974
	private int m_shotsRemaining;

	// Token: 0x02000D37 RID: 3383
	private enum State
	{
		// Token: 0x04003A80 RID: 14976
		Idle,
		// Token: 0x04003A81 RID: 14977
		Casting,
		// Token: 0x04003A82 RID: 14978
		Firing,
		// Token: 0x04003A83 RID: 14979
		PostFire
	}
}
