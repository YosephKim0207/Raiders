using System;
using System.Collections.Generic;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x02000DEB RID: 3563
public class KeybulletFleeBehavior : MovementBehaviorBase
{
	// Token: 0x06004B7B RID: 19323 RVA: 0x0019A08C File Offset: 0x0019828C
	public override void Start()
	{
		base.Start();
		this.m_aiActor.healthHaver.OnDamaged += this.HandleDamaged;
		this.m_aiActor.healthHaver.OnPreDeath += this.OnPreDeath;
		tk2dSpriteAnimator spriteAnimator = this.m_aiActor.spriteAnimator;
		spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnAnimationCompleted));
		tk2dSpriteAnimator spriteAnimator2 = this.m_aiActor.spriteAnimator;
		spriteAnimator2.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator2.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.OnAnimationEvent));
		this.m_aiActor.DoDustUps = false;
		this.m_aiActor.IsWorthShootingAt = true;
	}

	// Token: 0x06004B7C RID: 19324 RVA: 0x0019A148 File Offset: 0x00198348
	private void OnAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (clip.GetFrame(frame).eventInfo == "blackPhantomPoof" && this.m_aiActor && this.m_aiActor.IsBlackPhantom)
		{
			this.DoBlackPhantomPoof();
		}
	}

	// Token: 0x06004B7D RID: 19325 RVA: 0x0019A198 File Offset: 0x00198398
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
		base.DecrementTimer(ref this.m_timer, false);
		this.m_awakeTime += this.m_deltaTime;
	}

	// Token: 0x06004B7E RID: 19326 RVA: 0x0019A1D0 File Offset: 0x001983D0
	public override BehaviorResult Update()
	{
		if (this.m_shadowSprite == null)
		{
			this.m_shadowSprite = this.m_aiActor.ShadowObject.GetComponent<tk2dSprite>();
		}
		if (this.m_state == KeybulletFleeBehavior.State.Idle)
		{
			PixelCollider hitboxPixelCollider = this.m_aiActor.specRigidbody.HitboxPixelCollider;
			CameraController mainCameraController = GameManager.Instance.MainCameraController;
			if (mainCameraController && hitboxPixelCollider != null && (BraveUtility.PointIsVisible(hitboxPixelCollider.UnitTopCenter, -0.074074075f, ViewportType.Gameplay) || BraveUtility.PointIsVisible(hitboxPixelCollider.UnitBottomCenter, -0.074074075f, ViewportType.Gameplay) || BraveUtility.PointIsVisible(hitboxPixelCollider.UnitCenterLeft, -0.033333335f, ViewportType.Gameplay) || BraveUtility.PointIsVisible(hitboxPixelCollider.UnitCenterRight, -0.033333335f, ViewportType.Gameplay)))
			{
				this.m_onScreenTime += this.m_deltaTime;
				this.m_aiActor.ClearPath();
			}
			if (this.m_onScreenTime > this.TimeOnScreenToFlee || (this.m_onScreenTime > 0f && this.m_awakeTime < 1.5f))
			{
				this.Flee();
				return BehaviorResult.SkipRemainingClassBehaviors;
			}
		}
		else if (this.m_state == KeybulletFleeBehavior.State.Fleeing)
		{
			if (this.m_aiActor.PathComplete)
			{
				this.m_timer = this.PreDisappearTime;
				this.m_state = KeybulletFleeBehavior.State.WaitingToDisappear;
				this.m_aiActor.SetResistance(EffectResistanceType.Freeze, 1f);
				this.m_aiActor.behaviorSpeculator.ImmuneToStun = true;
				if (this.m_aiActor.knockbackDoer)
				{
					this.m_aiActor.knockbackDoer.SetImmobile(true, "My people need me");
				}
				return BehaviorResult.SkipRemainingClassBehaviors;
			}
			if (this.m_repathTimer <= 0f)
			{
				this.m_aiActor.PathfindToPosition(this.m_targetPos.ToCenterVector2(), null, true, null, null, null, false);
				this.m_repathTimer = this.PathInterval;
			}
		}
		else if (this.m_state == KeybulletFleeBehavior.State.WaitingToDisappear)
		{
			if (this.m_timer <= 0f)
			{
				if (!string.IsNullOrEmpty(this.DisappearAnimation))
				{
					if (this.m_aiActor.IsBlackPhantom)
					{
						this.m_aiAnimator.FpsScale *= this.BlackPhantomMultiplier;
					}
					this.m_aiAnimator.PlayUntilFinished(this.DisappearAnimation, true, null, -1f, false);
				}
				if (this.ChangeColliderOnDisappear)
				{
					List<PixelCollider> pixelColliders = this.m_aiActor.specRigidbody.PixelColliders;
					for (int i = 0; i < pixelColliders.Count; i++)
					{
						PixelCollider pixelCollider = pixelColliders[i];
						if (pixelCollider.Enabled && pixelCollider.CollisionLayer == CollisionLayer.EnemyHitBox)
						{
							pixelCollider.Enabled = false;
							break;
						}
					}
					for (int j = pixelColliders.Count - 1; j >= 0; j--)
					{
						PixelCollider pixelCollider2 = pixelColliders[j];
						if (!pixelCollider2.Enabled && pixelCollider2.CollisionLayer == CollisionLayer.EnemyHitBox)
						{
							pixelCollider2.Enabled = true;
							break;
						}
					}
				}
				if (!this.m_aiActor.IsBlackPhantom)
				{
					this.m_cachedShader = this.m_aiActor.renderer.material.shader;
					this.m_aiActor.sprite.usesOverrideMaterial = true;
					this.m_aiActor.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
					this.m_aiActor.renderer.material.SetFloat("_VertexColor", 1f);
				}
				this.m_aiActor.sprite.HeightOffGround = 1f;
				this.m_aiActor.sprite.UpdateZDepth();
				this.m_state = KeybulletFleeBehavior.State.Disappearing;
			}
		}
		else if (this.m_state == KeybulletFleeBehavior.State.Disappearing)
		{
			if (!this.m_aiActor.IsBlackPhantom)
			{
				float num = Mathf.Clamp01(Mathf.Lerp(1.5f, 0f, this.m_aiAnimator.CurrentClipProgress));
				this.m_aiActor.sprite.color = this.m_aiActor.sprite.color.WithAlpha(num);
				if (this.m_shadowSprite)
				{
					this.m_shadowSprite.color = this.m_aiActor.sprite.color.WithAlpha(num);
				}
			}
			if (!this.m_aiAnimator.IsPlaying(this.DisappearAnimation))
			{
				this.m_aiActor.EraseFromExistence(true);
			}
		}
		return (this.m_state != KeybulletFleeBehavior.State.Idle || this.m_onScreenTime > 0f) ? BehaviorResult.SkipRemainingClassBehaviors : BehaviorResult.Continue;
	}

	// Token: 0x06004B7F RID: 19327 RVA: 0x0019A668 File Offset: 0x00198868
	private void HandleDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		if (this.m_state == KeybulletFleeBehavior.State.Idle)
		{
			this.Flee();
		}
	}

	// Token: 0x06004B80 RID: 19328 RVA: 0x0019A67C File Offset: 0x0019887C
	private void OnPreDeath(Vector2 obj)
	{
		this.m_aiActor.sprite.HeightOffGround = 1f;
		this.m_aiActor.sprite.UpdateZDepth();
		if (this.m_state == KeybulletFleeBehavior.State.Disappearing && !this.m_aiActor.IsBlackPhantom)
		{
			this.m_aiActor.sprite.usesOverrideMaterial = false;
			this.m_aiActor.renderer.material.shader = this.m_cachedShader;
		}
	}

	// Token: 0x06004B81 RID: 19329 RVA: 0x0019A6F8 File Offset: 0x001988F8
	private void OnAnimationCompleted(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
	{
		if (this.m_state == KeybulletFleeBehavior.State.Disappearing)
		{
			this.m_aiActor.EraseFromExistence(true);
		}
	}

	// Token: 0x06004B82 RID: 19330 RVA: 0x0019A714 File Offset: 0x00198914
	private void Flee()
	{
		this.m_aiActor.ClearPath();
		this.m_aiActor.DoDustUps = true;
		IntVector2? fleePoint = this.GetFleePoint();
		if (fleePoint != null)
		{
			this.m_targetPos = fleePoint.Value;
			if (this.FleeMoveSpeed > 0f)
			{
				this.m_aiActor.MovementSpeed = TurboModeController.MaybeModifyEnemyMovementSpeed(this.FleeMoveSpeed);
			}
			this.m_aiActor.PathfindToPosition(this.m_targetPos.ToCenterVector2(), null, true, null, null, null, false);
			this.m_repathTimer = this.PathInterval;
		}
		this.m_state = KeybulletFleeBehavior.State.Fleeing;
	}

	// Token: 0x06004B83 RID: 19331 RVA: 0x0019A7C0 File Offset: 0x001989C0
	private IntVector2? GetFleePoint()
	{
		PlayerController bestActivePlayer = GameManager.Instance.BestActivePlayer;
		this.m_playerPos = bestActivePlayer.specRigidbody.UnitCenter;
		this.m_player2Pos = null;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(bestActivePlayer);
			if (otherPlayer && otherPlayer.healthHaver && otherPlayer.healthHaver.IsAlive)
			{
				this.m_player2Pos = new Vector2?(otherPlayer.specRigidbody.UnitCenter);
			}
		}
		FloodFillUtility.PreprocessContiguousCells(this.m_aiActor.ParentRoom, this.m_aiActor.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor), 0);
		IntVector2? intVector = null;
		RoomHandler parentRoom = this.m_aiActor.ParentRoom;
		IntVector2? intVector2 = parentRoom.GetRandomAvailableCell(new IntVector2?(this.m_aiActor.Clearance), new CellTypes?(this.m_aiActor.PathableTiles), false, new CellValidator(this.CellValidator));
		if (intVector2 == null)
		{
			intVector2 = parentRoom.GetRandomWeightedAvailableCell(new IntVector2?(this.m_aiActor.Clearance), new CellTypes?(this.m_aiActor.PathableTiles), false, null, new Func<IntVector2, float>(this.CellWeighter), 1f);
		}
		return intVector2;
	}

	// Token: 0x06004B84 RID: 19332 RVA: 0x0019A914 File Offset: 0x00198B14
	private bool CellValidator(IntVector2 c)
	{
		if (!FloodFillUtility.WasFilled(c))
		{
			return false;
		}
		bool flag = false;
		DungeonData data = GameManager.Instance.Dungeon.data;
		int num = 0;
		while (num < this.m_aiActor.Clearance.x && !flag)
		{
			int num2 = 0;
			while (num2 < this.m_aiActor.Clearance.y && !flag)
			{
				if (data.isWall(c.x + num - 1, c.y + num2))
				{
					flag = true;
					break;
				}
				if (data.isWall(c.x + num + 1, c.y + num2))
				{
					flag = true;
					break;
				}
				if (data.isWall(c.x + num, c.y + num2 + 1))
				{
					flag = true;
					break;
				}
				num2++;
			}
			num++;
		}
		if (!flag)
		{
			return false;
		}
		Vector2 clearanceOffset = Pathfinder.GetClearanceOffset(c, this.m_aiActor.Clearance);
		if (Vector2.Distance(clearanceOffset, this.m_playerPos) < this.MinGoalDistFromPlayer)
		{
			return false;
		}
		Vector2? player2Pos = this.m_player2Pos;
		return player2Pos == null || Vector2.Distance(clearanceOffset, this.m_player2Pos.Value) >= this.MinGoalDistFromPlayer;
	}

	// Token: 0x06004B85 RID: 19333 RVA: 0x0019AA74 File Offset: 0x00198C74
	private float CellWeighter(IntVector2 c)
	{
		if (!FloodFillUtility.WasFilled(c))
		{
			return 0f;
		}
		bool flag = false;
		DungeonData data = GameManager.Instance.Dungeon.data;
		int num = 0;
		while (num < this.m_aiActor.Clearance.x && !flag)
		{
			int num2 = 0;
			while (num2 < this.m_aiActor.Clearance.y && !flag)
			{
				if (data.isWall(c.x + num - 1, c.y + num2))
				{
					flag = true;
					break;
				}
				if (data.isWall(c.x + num + 1, c.y + num2))
				{
					flag = true;
					break;
				}
				if (data.isWall(c.x + num, c.y + num2 + 1))
				{
					flag = true;
					break;
				}
				num2++;
			}
			num++;
		}
		bool flag2 = false;
		if (!flag)
		{
			int num3 = 0;
			while (num3 < this.m_aiActor.Clearance.x && !flag)
			{
				int num4 = 0;
				while (num4 < this.m_aiActor.Clearance.y && !flag)
				{
					if (data.isPit(c.x + num3 - 1, c.y + num4))
					{
						flag2 = true;
						break;
					}
					if (data.isPit(c.x + num3 + 1, c.y + num4))
					{
						flag2 = true;
						break;
					}
					if (data.isPit(c.x + num3, c.y + num4 + 1))
					{
						flag2 = true;
						break;
					}
					if (data.isPit(c.x + num3, c.y + num4 - 1))
					{
						flag2 = true;
						break;
					}
					num4++;
				}
				num3++;
			}
		}
		Vector2 clearanceOffset = Pathfinder.GetClearanceOffset(c, this.m_aiActor.Clearance);
		float num5 = Vector2.Distance(clearanceOffset, this.m_playerPos);
		Vector2? player2Pos = this.m_player2Pos;
		if (player2Pos != null)
		{
			num5 = Mathf.Min(num5, Vector2.Distance(clearanceOffset, this.m_player2Pos.Value));
		}
		return num5 + (float)((!flag) ? ((!flag2) ? 0 : 50) : 100);
	}

	// Token: 0x06004B86 RID: 19334 RVA: 0x0019ACE8 File Offset: 0x00198EE8
	private void DoBlackPhantomPoof()
	{
		if (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW)
		{
			Vector3 vector = this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitBottomLeft.ToVector3ZisY(0f);
			Vector3 vector2 = this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitTopRight.ToVector3ZisY(0f);
			vector.z = vector.y - 6f;
			vector2.z = vector2.y - 6f;
			float num = (vector2.y - vector.y) * (vector2.x - vector.x);
			int num2 = (int)(50f * num);
			int num3 = num2;
			Vector3 vector3 = vector;
			Vector3 vector4 = vector2;
			Vector3 vector5 = Vector3.up / 2f;
			float num4 = 120f;
			float num5 = 0.2f;
			float? num6 = new float?(UnityEngine.Random.Range(1f, 1.65f));
			GlobalSparksDoer.DoRandomParticleBurst(num3, vector3, vector4, vector5, num4, num5, null, num6, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
			num3 = num2;
			vector5 = vector;
			vector4 = vector2;
			vector3 = Vector3.up / 2f;
			num5 = 120f;
			num4 = 0.2f;
			num6 = new float?(UnityEngine.Random.Range(1f, 1.65f));
			GlobalSparksDoer.DoRandomParticleBurst(num3, vector5, vector4, vector3, num5, num4, null, num6, null, GlobalSparksDoer.SparksType.DARK_MAGICKS);
			if (UnityEngine.Random.value < 0.5f)
			{
				num3 = 1;
				vector3 = vector;
				vector4 = vector2.WithY(vector.y + 0.1f);
				vector5 = Vector3.right / 2f;
				num4 = 25f;
				num5 = 0.2f;
				num6 = new float?(UnityEngine.Random.Range(1f, 1.65f));
				GlobalSparksDoer.DoRandomParticleBurst(num3, vector3, vector4, vector5, num4, num5, null, num6, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
			}
			else
			{
				num3 = 1;
				vector5 = vector;
				vector4 = vector2.WithY(vector.y + 0.1f);
				vector3 = Vector3.left / 2f;
				num5 = 25f;
				num4 = 0.2f;
				num6 = new float?(UnityEngine.Random.Range(1f, 1.65f));
				GlobalSparksDoer.DoRandomParticleBurst(num3, vector5, vector4, vector3, num5, num4, null, num6, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
			}
		}
	}

	// Token: 0x0400411D RID: 16669
	private const float c_screenXBuffer = 0.033333335f;

	// Token: 0x0400411E RID: 16670
	private const float c_screenYBuffer = 0.074074075f;

	// Token: 0x0400411F RID: 16671
	public float PathInterval = 0.25f;

	// Token: 0x04004120 RID: 16672
	public float TimeOnScreenToFlee = 1.25f;

	// Token: 0x04004121 RID: 16673
	public float FleeMoveSpeed = 9.5f;

	// Token: 0x04004122 RID: 16674
	public float PreDisappearTime = 1f;

	// Token: 0x04004123 RID: 16675
	public string DisappearAnimation;

	// Token: 0x04004124 RID: 16676
	public bool ChangeColliderOnDisappear = true;

	// Token: 0x04004125 RID: 16677
	public float BlackPhantomMultiplier = 1f;

	// Token: 0x04004126 RID: 16678
	public float MinGoalDistFromPlayer = 10f;

	// Token: 0x04004127 RID: 16679
	private float m_repathTimer;

	// Token: 0x04004128 RID: 16680
	private float m_timer;

	// Token: 0x04004129 RID: 16681
	private float m_onScreenTime;

	// Token: 0x0400412A RID: 16682
	private float m_awakeTime;

	// Token: 0x0400412B RID: 16683
	private IntVector2 m_targetPos;

	// Token: 0x0400412C RID: 16684
	private Shader m_cachedShader;

	// Token: 0x0400412D RID: 16685
	private tk2dSprite m_shadowSprite;

	// Token: 0x0400412E RID: 16686
	private KeybulletFleeBehavior.State m_state;

	// Token: 0x0400412F RID: 16687
	private Vector2 m_playerPos;

	// Token: 0x04004130 RID: 16688
	private Vector2? m_player2Pos;

	// Token: 0x02000DEC RID: 3564
	private enum State
	{
		// Token: 0x04004132 RID: 16690
		Idle,
		// Token: 0x04004133 RID: 16691
		Fleeing,
		// Token: 0x04004134 RID: 16692
		WaitingToDisappear,
		// Token: 0x04004135 RID: 16693
		Disappearing
	}
}
