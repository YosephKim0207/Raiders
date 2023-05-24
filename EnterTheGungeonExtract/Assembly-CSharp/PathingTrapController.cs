using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001725 RID: 5925
public class PathingTrapController : TrapController
{
	// Token: 0x06008998 RID: 35224 RVA: 0x003933A4 File Offset: 0x003915A4
	public override void Start()
	{
		base.Start();
		this.m_pathMover = base.GetComponent<PathMover>();
		this.m_parentRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
		if (base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTriggerCollision));
			SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
			specRigidbody2.OnPathTargetReached = (Action)Delegate.Combine(specRigidbody2.OnPathTargetReached, new Action(this.OnPathTargetReached));
			List<CollisionData> list = new List<CollisionData>();
			if (PhysicsEngine.Instance.OverlapCast(base.specRigidbody, list, false, true, null, null, false, null, null, new SpeculativeRigidbody[0]))
			{
				for (int i = 0; i < list.Count; i++)
				{
					SpeculativeRigidbody otherRigidbody = list[i].OtherRigidbody;
					if (otherRigidbody && otherRigidbody.minorBreakable)
					{
						otherRigidbody.minorBreakable.Break();
					}
				}
			}
		}
		this.m_startingAnimation = base.spriteAnimator.CurrentClip;
		if (this.shadowAnimator)
		{
			this.m_startingShadowAnimation = base.spriteAnimator.CurrentClip;
		}
		if (GameManager.Instance.PlayerIsNearRoom(this.m_parentRoom))
		{
			AkSoundEngine.PostEvent("Play_ENV_trap_active", base.gameObject);
			this.m_IsSoundPlaying = true;
		}
		this.m_isAnimating = true;
		if (this.Sparks_A != null)
		{
			this.m_sparksAStartPos = this.Sparks_A.localPosition;
		}
		if (this.Sparks_B != null)
		{
			this.m_sparksBStartPos = this.Sparks_B.localPosition;
		}
	}

	// Token: 0x06008999 RID: 35225 RVA: 0x00393588 File Offset: 0x00391788
	protected void UpdateSparks()
	{
		float num = 5f;
		float num2 = 5f;
		if (this.m_pathMover != null)
		{
			num2 = Vector2.Distance(base.specRigidbody.Position.UnitPosition, this.m_pathMover.GetCurrentTargetPosition());
			num = Vector2.Distance(base.specRigidbody.Position.UnitPosition, this.m_pathMover.GetPreviousTargetPosition());
		}
		if (this.m_pathMover.Path.wrapMode != SerializedPath.SerializedPathWrapMode.Loop)
		{
			if (this.m_pathMover.CurrentIndex == 0 || this.m_pathMover.CurrentIndex == this.m_pathMover.Path.nodes.Count - 1)
			{
				num2 = 1f;
			}
			if (this.m_pathMover.PreviousIndex == 0 || this.m_pathMover.PreviousIndex == this.m_pathMover.Path.nodes.Count - 1)
			{
				num = 1f;
			}
		}
		if (base.specRigidbody.Velocity == Vector2.zero)
		{
			return;
		}
		if (this.Sparks_A != null)
		{
			Vector2 vector = ((!(base.specRigidbody.Velocity == Vector2.zero)) ? base.specRigidbody.Velocity : this.m_cachedSparkVelocity);
			if (base.specRigidbody.Velocity != Vector2.zero)
			{
				this.m_cachedSparkVelocity = vector;
			}
			if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
			{
				if (!this.Sparks_A.gameObject.activeSelf)
				{
					this.Sparks_A.gameObject.SetActive(true);
				}
				this.Sparks_A.localPosition = ((vector.x <= 0f) ? this.m_sparksBStartPos : this.m_sparksAStartPos);
				Vector3 vector2 = ((this.m_pathMover.GetPreviousSourcePosition().y <= base.specRigidbody.Position.UnitPosition.y) ? new Vector3(0f, this.m_sparksAStartPos.x, 0f) : new Vector3(0f, this.m_sparksBStartPos.x, 0f));
				this.Sparks_A.localPosition = Vector3.Lerp(vector2, this.Sparks_A.localPosition, num);
			}
			else
			{
				if (!this.Sparks_A.gameObject.activeSelf)
				{
					this.Sparks_A.gameObject.SetActive(true);
				}
				this.Sparks_A.localPosition = ((vector.y <= 0f) ? new Vector3(0f, this.m_sparksBStartPos.x, 0f) : new Vector3(0f, this.m_sparksAStartPos.x, 0f));
				Vector3 vector3 = ((this.m_pathMover.GetPreviousSourcePosition().x >= base.specRigidbody.Position.UnitPosition.x) ? this.m_sparksBStartPos : this.m_sparksAStartPos);
				this.Sparks_A.localPosition = Vector3.Lerp(vector3, this.Sparks_A.localPosition, num);
			}
		}
		if (this.Sparks_B != null)
		{
			Vector2 vector4 = ((!(base.specRigidbody.Velocity == Vector2.zero)) ? base.specRigidbody.Velocity : this.m_cachedSparkVelocity);
			if (base.specRigidbody.Velocity != Vector2.zero)
			{
				this.m_cachedSparkVelocity = vector4;
			}
			if (Mathf.Abs(vector4.x) > Mathf.Abs(vector4.y))
			{
				if (!this.Sparks_B.gameObject.activeSelf)
				{
					this.Sparks_B.gameObject.SetActive(true);
				}
				this.Sparks_B.localPosition = ((vector4.x <= 0f) ? this.m_sparksAStartPos : this.m_sparksBStartPos);
				Vector3 vector5 = ((this.m_pathMover.GetNextTargetPosition().y <= base.specRigidbody.Position.UnitPosition.y) ? new Vector3(0f, this.m_sparksAStartPos.x, 0f) : new Vector3(0f, this.m_sparksBStartPos.x, 0f));
				this.Sparks_B.localPosition = Vector3.Lerp(vector5, this.Sparks_B.localPosition, num2);
			}
			else
			{
				if (!this.Sparks_B.gameObject.activeSelf)
				{
					this.Sparks_B.gameObject.SetActive(true);
				}
				this.Sparks_B.localPosition = ((vector4.y <= 0f) ? new Vector3(0f, this.m_sparksAStartPos.x, 0f) : new Vector3(0f, this.m_sparksBStartPos.x, 0f));
				Vector3 vector6 = ((this.m_pathMover.GetNextTargetPosition().x <= base.specRigidbody.Position.UnitPosition.x) ? this.m_sparksAStartPos : this.m_sparksBStartPos);
				this.Sparks_B.localPosition = Vector3.Lerp(vector6, this.Sparks_B.localPosition, num2);
			}
		}
	}

	// Token: 0x0600899A RID: 35226 RVA: 0x00393B3C File Offset: 0x00391D3C
	public virtual void Update()
	{
		if (this.m_IsSoundPlaying)
		{
			if (!GameManager.Instance.PlayerIsNearRoom(this.m_parentRoom) || (this.pauseAnimationOnRest && base.specRigidbody.Velocity == Vector2.zero))
			{
				this.m_IsSoundPlaying = false;
				AkSoundEngine.PostEvent("Stop_ENV_trap_active", base.gameObject);
			}
		}
		else if (GameManager.Instance.PlayerIsNearRoom(this.m_parentRoom) && (!this.pauseAnimationOnRest || !(base.specRigidbody.Velocity == Vector2.zero)))
		{
			this.m_IsSoundPlaying = true;
			AkSoundEngine.PostEvent("Play_ENV_trap_active", base.gameObject);
		}
		this.UpdateSparks();
		if (base.specRigidbody.Velocity == Vector2.zero)
		{
			if (this.pauseAnimationOnRest && this.m_isAnimating)
			{
				base.spriteAnimator.Stop();
				if (this.shadowAnimator)
				{
					this.shadowAnimator.Stop();
				}
				this.m_isAnimating = false;
			}
		}
		else
		{
			this.m_isAnimating = true;
			if (base.spriteAnimator != null)
			{
				base.spriteAnimator.Sprite.UpdateZDepth();
			}
			if (this.usesDirectionalAnimations)
			{
				IntVector2 intMajorAxis = BraveUtility.GetIntMajorAxis(base.specRigidbody.Velocity);
				if (intMajorAxis == IntVector2.North)
				{
					base.spriteAnimator.Play(this.northAnimation);
				}
				else if (intMajorAxis == IntVector2.East)
				{
					base.spriteAnimator.Play(this.eastAnimation);
				}
				else if (intMajorAxis == IntVector2.South)
				{
					base.spriteAnimator.Play(this.southAnimation);
				}
				else if (intMajorAxis == IntVector2.West)
				{
					base.spriteAnimator.Play(this.westAnimation);
				}
				if (this.usesDirectionalShadowAnimations)
				{
					if (intMajorAxis == IntVector2.North)
					{
						this.shadowAnimator.Play(this.northShadowAnimation);
					}
					else if (intMajorAxis == IntVector2.East)
					{
						this.shadowAnimator.Play(this.eastShadowAnimation);
					}
					else if (intMajorAxis == IntVector2.South)
					{
						this.shadowAnimator.Play(this.southShadowAnimation);
					}
					else if (intMajorAxis == IntVector2.West)
					{
						this.shadowAnimator.Play(this.westShadowAnimation);
					}
				}
			}
			else
			{
				if (this.m_startingAnimation != null)
				{
					base.spriteAnimator.Play(this.m_startingAnimation);
				}
				if (this.shadowAnimator && this.m_startingShadowAnimation != null)
				{
					this.shadowAnimator.Play(this.m_startingShadowAnimation);
				}
			}
		}
	}

	// Token: 0x0600899B RID: 35227 RVA: 0x00393E28 File Offset: 0x00392028
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600899C RID: 35228 RVA: 0x00393E30 File Offset: 0x00392030
	public override GameObject InstantiateObject(RoomHandler targetRoom, IntVector2 loc, bool deferConfiguration = false)
	{
		this.m_markCellOccupied = false;
		return base.InstantiateObject(targetRoom, loc, deferConfiguration);
	}

	// Token: 0x0600899D RID: 35229 RVA: 0x00393E44 File Offset: 0x00392044
	private void OnTriggerCollision(SpeculativeRigidbody rigidbody, SpeculativeRigidbody source, CollisionData collisionData)
	{
		if (rigidbody.gameActor is PlayerController)
		{
			if (!(rigidbody.gameActor as PlayerController).IsEthereal)
			{
				this.Damage(rigidbody, this.damage, this.knockbackStrength);
			}
		}
		else if (this.hitsEnemies && rigidbody.aiActor)
		{
			this.Damage(rigidbody, this.enemyDamage, this.enemyKnockbackStrength);
		}
		else
		{
			Chest component = rigidbody.GetComponent<Chest>();
			if (component != null && !component.IsBroken && !component.TemporarilyUnopenable)
			{
				component.majorBreakable.Break(source.Velocity);
			}
		}
	}

	// Token: 0x0600899E RID: 35230 RVA: 0x00393EFC File Offset: 0x003920FC
	private void OnPathTargetReached()
	{
		if (this.m_IsSoundPlaying)
		{
			AkSoundEngine.PostEvent("Play_ENV_trap_turn", base.gameObject);
		}
	}

	// Token: 0x0600899F RID: 35231 RVA: 0x00393F1C File Offset: 0x0039211C
	protected virtual void Damage(SpeculativeRigidbody rigidbody, float damage, float knockbackStrength)
	{
		if (damage <= 0f)
		{
			return;
		}
		if (knockbackStrength > 0f && rigidbody.knockbackDoer)
		{
			rigidbody.knockbackDoer.ApplySourcedKnockback(rigidbody.UnitCenter - base.specRigidbody.UnitCenter, knockbackStrength, base.gameObject, false);
		}
		if (rigidbody.healthHaver.IsVulnerable || this.ignoreInvulnerabilityFrames)
		{
			HealthHaver healthHaver = rigidbody.healthHaver;
			Vector2 zero = Vector2.zero;
			string enemiesString = StringTableManager.GetEnemiesString("#TRAP", -1);
			bool flag = this.ignoreInvulnerabilityFrames;
			healthHaver.ApplyDamage(damage, zero, enemiesString, CoreDamageTypes.None, DamageCategory.Normal, flag, null, false);
			if (!this.m_isBloodied && this.usesBloodyAnimation && !string.IsNullOrEmpty(this.bloodyAnimation))
			{
				base.spriteAnimator.Play(this.bloodyAnimation);
			}
			this.m_isBloodied = true;
		}
	}

	// Token: 0x04008FE4 RID: 36836
	public float damage;

	// Token: 0x04008FE5 RID: 36837
	public bool ignoreInvulnerabilityFrames;

	// Token: 0x04008FE6 RID: 36838
	public float knockbackStrength;

	// Token: 0x04008FE7 RID: 36839
	public bool hitsEnemies;

	// Token: 0x04008FE8 RID: 36840
	public float enemyDamage;

	// Token: 0x04008FE9 RID: 36841
	public float enemyKnockbackStrength;

	// Token: 0x04008FEA RID: 36842
	[TogglesProperty("bloodyAnimation", "Bloody Animation")]
	public bool usesBloodyAnimation;

	// Token: 0x04008FEB RID: 36843
	[HideInInspector]
	public string bloodyAnimation;

	// Token: 0x04008FEC RID: 36844
	public bool usesDirectionalAnimations;

	// Token: 0x04008FED RID: 36845
	[ShowInInspectorIf("usesDirectionalAnimations", true)]
	public string northAnimation;

	// Token: 0x04008FEE RID: 36846
	[ShowInInspectorIf("usesDirectionalAnimations", true)]
	public string eastAnimation;

	// Token: 0x04008FEF RID: 36847
	[ShowInInspectorIf("usesDirectionalAnimations", true)]
	public string southAnimation;

	// Token: 0x04008FF0 RID: 36848
	[ShowInInspectorIf("usesDirectionalAnimations", true)]
	public string westAnimation;

	// Token: 0x04008FF1 RID: 36849
	public bool usesDirectionalShadowAnimations;

	// Token: 0x04008FF2 RID: 36850
	[ShowInInspectorIf("usesDirectionalShadowAnimations", true)]
	public tk2dSpriteAnimator shadowAnimator;

	// Token: 0x04008FF3 RID: 36851
	[ShowInInspectorIf("usesDirectionalShadowAnimations", true)]
	public string northShadowAnimation;

	// Token: 0x04008FF4 RID: 36852
	[ShowInInspectorIf("usesDirectionalShadowAnimations", true)]
	public string eastShadowAnimation;

	// Token: 0x04008FF5 RID: 36853
	[ShowInInspectorIf("usesDirectionalShadowAnimations", true)]
	public string southShadowAnimation;

	// Token: 0x04008FF6 RID: 36854
	[ShowInInspectorIf("usesDirectionalShadowAnimations", true)]
	public string westShadowAnimation;

	// Token: 0x04008FF7 RID: 36855
	public bool pauseAnimationOnRest = true;

	// Token: 0x04008FF8 RID: 36856
	[Header("Sawblade Options")]
	public Transform Sparks_A;

	// Token: 0x04008FF9 RID: 36857
	public Transform Sparks_B;

	// Token: 0x04008FFA RID: 36858
	private Vector3 m_sparksAStartPos;

	// Token: 0x04008FFB RID: 36859
	private Vector3 m_sparksBStartPos;

	// Token: 0x04008FFC RID: 36860
	private bool m_IsSoundPlaying;

	// Token: 0x04008FFD RID: 36861
	private RoomHandler m_parentRoom;

	// Token: 0x04008FFE RID: 36862
	protected Vector2 m_cachedSparkVelocity;

	// Token: 0x04008FFF RID: 36863
	private bool m_isBloodied;

	// Token: 0x04009000 RID: 36864
	private bool m_isAnimating;

	// Token: 0x04009001 RID: 36865
	private PathMover m_pathMover;

	// Token: 0x04009002 RID: 36866
	private tk2dSpriteAnimationClip m_startingAnimation;

	// Token: 0x04009003 RID: 36867
	private tk2dSpriteAnimationClip m_startingShadowAnimation;
}
