using System;
using UnityEngine;

// Token: 0x02000E1A RID: 3610
public class BuffVFXAnimator : BraveBehaviour
{
	// Token: 0x06004C73 RID: 19571 RVA: 0x001A0BC4 File Offset: 0x0019EDC4
	public void InitializeTetris(GameActor target, Vector2 sourceVec)
	{
		if (target == null)
		{
			return;
		}
		this.parametricStartPoint = UnityEngine.Random.value;
		this.m_target = target;
		this.m_transform = base.transform;
		if (this.animationStyle == BuffVFXAnimator.BuffAnimationStyle.PIERCE && this.m_target && this.m_target.specRigidbody)
		{
			this.m_hitboxOriginOffset = this.m_target.specRigidbody.HitboxPixelCollider.UnitCenter - this.m_target.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
			this.m_pierceAngle = BraveMathCollege.Atan2Degrees(-sourceVec.normalized);
			this.m_hitboxOriginOffset += sourceVec.normalized * (base.sprite.GetBounds().extents.x * UnityEngine.Random.Range(0.15f, 0.5f));
			this.m_transform.position = (this.m_target.specRigidbody.HitboxPixelCollider.UnitBottomLeft + this.m_hitboxOriginOffset).ToVector3ZUp(0f);
			this.m_transform.rotation = Quaternion.Euler(0f, 0f, this.m_pierceAngle);
		}
		this.m_initialized = true;
	}

	// Token: 0x06004C74 RID: 19572 RVA: 0x001A0D20 File Offset: 0x0019EF20
	public void InitializePierce(GameActor target, Vector2 sourceVec)
	{
		if (target == null)
		{
			return;
		}
		this.parametricStartPoint = UnityEngine.Random.value;
		this.m_target = target;
		this.m_transform = base.transform;
		if (this.animationStyle == BuffVFXAnimator.BuffAnimationStyle.PIERCE && this.m_target && this.m_target.specRigidbody)
		{
			this.m_hitboxOriginOffset = this.m_target.specRigidbody.HitboxPixelCollider.UnitCenter - this.m_target.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
			this.m_pierceAngle = BraveMathCollege.Atan2Degrees(-sourceVec.normalized);
			this.m_hitboxOriginOffset += sourceVec.normalized * (base.sprite.GetBounds().extents.x * UnityEngine.Random.Range(0.15f, 0.5f));
			this.m_transform.position = (this.m_target.specRigidbody.HitboxPixelCollider.UnitBottomLeft + this.m_hitboxOriginOffset).ToVector3ZUp(0f);
			this.m_transform.rotation = Quaternion.Euler(0f, 0f, this.m_pierceAngle);
		}
		this.m_initialized = true;
	}

	// Token: 0x06004C75 RID: 19573 RVA: 0x001A0E7C File Offset: 0x0019F07C
	public void Initialize(GameActor target)
	{
		if (target == null)
		{
			return;
		}
		this.parametricStartPoint = UnityEngine.Random.value;
		this.m_target = target;
		this.m_transform = base.transform;
		if (this.animationStyle == BuffVFXAnimator.BuffAnimationStyle.PIERCE)
		{
			if (this.m_target && this.m_target.specRigidbody)
			{
				this.m_hitboxOriginOffset = new Vector2(UnityEngine.Random.Range(0f, this.m_target.specRigidbody.HitboxPixelCollider.UnitDimensions.x), UnityEngine.Random.Range(0f, this.m_target.specRigidbody.HitboxPixelCollider.UnitDimensions.y));
				this.m_pierceAngle = BraveMathCollege.Atan2Degrees(this.m_hitboxOriginOffset + this.m_target.specRigidbody.HitboxPixelCollider.UnitBottomLeft - this.m_target.specRigidbody.HitboxPixelCollider.UnitCenter);
				Vector2 vector = this.m_target.specRigidbody.HitboxPixelCollider.UnitCenter - (this.m_hitboxOriginOffset + this.m_target.specRigidbody.HitboxPixelCollider.UnitBottomLeft);
				this.m_hitboxOriginOffset += vector.normalized * (base.sprite.GetBounds().extents.x * UnityEngine.Random.Range(0.15f + this.AdditionalPierceDepth, 0.5f + this.AdditionalPierceDepth));
				this.m_transform.position = (this.m_target.specRigidbody.HitboxPixelCollider.UnitBottomLeft + this.m_hitboxOriginOffset).ToVector3ZUp(0f);
				this.m_transform.rotation = Quaternion.Euler(0f, 0f, this.m_pierceAngle);
			}
		}
		else if (this.animationStyle == BuffVFXAnimator.BuffAnimationStyle.TETRIS && this.m_target && this.m_target.specRigidbody)
		{
			this.m_hitboxOriginOffset = new Vector2(UnityEngine.Random.Range(0f, this.m_target.specRigidbody.HitboxPixelCollider.UnitDimensions.x), UnityEngine.Random.Range(0f, this.m_target.specRigidbody.HitboxPixelCollider.UnitDimensions.y));
			this.m_pierceAngle = (float)(90 * UnityEngine.Random.Range(0, 4));
			Vector2 vector2 = this.m_target.specRigidbody.HitboxPixelCollider.UnitCenter - (this.m_hitboxOriginOffset + this.m_target.specRigidbody.HitboxPixelCollider.UnitBottomLeft);
			this.m_hitboxOriginOffset += vector2.normalized * (base.sprite.GetBounds().extents.x * UnityEngine.Random.Range(0.15f + this.AdditionalPierceDepth, 0.5f + this.AdditionalPierceDepth));
			this.m_hitboxOriginOffset = this.m_hitboxOriginOffset.Quantize(0.375f);
			if (this.m_pierceAngle == 0f || this.m_pierceAngle == 180f)
			{
				Vector2 vector3 = base.sprite.GetBounds().extents.XY();
				this.m_hitboxOriginOffset -= vector3;
			}
			else
			{
				Vector2 vector4 = base.sprite.GetBounds().extents.XY();
				vector4 = new Vector2(vector4.y, vector4.x);
				this.m_hitboxOriginOffset -= vector4;
			}
			this.m_hitboxOriginOffset += new Vector2(0.375f, 0.375f);
			this.m_transform.position = (this.m_target.specRigidbody.HitboxPixelCollider.UnitBottomLeft + this.m_hitboxOriginOffset).ToVector3ZUp(0f);
			this.m_transform.rotation = Quaternion.Euler(0f, 0f, this.m_pierceAngle);
			base.sprite.HeightOffGround = UnityEngine.Random.Range(0f, 3f).Quantize(0.05f);
			base.sprite.IsPerpendicular = true;
			base.sprite.UpdateZDepth();
		}
		if (UnityEngine.Random.value > this.ChanceOfApplication)
		{
			this.ForceFailure = true;
		}
		this.m_initialized = true;
	}

	// Token: 0x06004C76 RID: 19574 RVA: 0x001A131C File Offset: 0x0019F51C
	public void ForceDrop()
	{
		this.ForceFailure = true;
	}

	// Token: 0x06004C77 RID: 19575 RVA: 0x001A1328 File Offset: 0x0019F528
	public void ClearData()
	{
		this.m_initialized = false;
		this.m_target = null;
		this.ForceFailure = false;
	}

	// Token: 0x06004C78 RID: 19576 RVA: 0x001A1340 File Offset: 0x0019F540
	private void OnDespawned()
	{
		this.m_initialized = false;
		this.m_target = null;
	}

	// Token: 0x06004C79 RID: 19577 RVA: 0x001A1350 File Offset: 0x0019F550
	private void Update()
	{
		if (!this.m_initialized)
		{
			SpawnManager.Despawn(base.gameObject);
			return;
		}
		if (!this.m_target || this.m_target.healthHaver.IsDead || this.ForceFailure)
		{
			if (this.UsesVFXToSpawnOnDeath && this.m_target && this.m_target.specRigidbody)
			{
				this.VFXToSpawnOnDeath.SpawnAtPosition(base.transform.position, 0f, null, new Vector2?(Vector2.zero), new Vector2?(this.m_target.specRigidbody.HitboxPixelCollider.UnitCenter - (this.m_target.specRigidbody.HitboxPixelCollider.UnitBottomLeft + this.m_hitboxOriginOffset)), new float?(1f), false, null, null, false);
				if (this.NonPoolVFX)
				{
					GameObject gameObject = SpawnManager.SpawnVFX(this.NonPoolVFX, base.transform.position, base.transform.rotation);
					Vector2 vector = this.m_target.specRigidbody.HitboxPixelCollider.UnitCenter - (this.m_target.specRigidbody.HitboxPixelCollider.UnitBottomLeft + this.m_hitboxOriginOffset);
					DebrisObject component = gameObject.GetComponent<DebrisObject>();
					if (component)
					{
						tk2dBaseSprite sprite = component.sprite;
						sprite.IsPerpendicular = false;
						sprite.usesOverrideMaterial = true;
						Vector2 vector2 = (-vector).normalized;
						float num = UnityEngine.Random.Range(-20f, 20f);
						vector2 = Quaternion.Euler(0f, 0f, num) * vector2 * 10f;
						component.Trigger(vector2.ToVector3ZUp(0.1f), 2f, 1f);
					}
				}
			}
			SpawnManager.Despawn(base.gameObject);
			return;
		}
		this.m_elapsed += BraveTime.DeltaTime;
		float num2 = this.m_elapsed / this.motionPeriod + this.parametricStartPoint;
		Vector3 vector3 = this.m_transform.position;
		if (this.DoesSparks)
		{
			this.m_sparksAccum += BraveTime.DeltaTime * this.SparksModule.RatePerSecond;
			if (this.m_sparksAccum > 0f)
			{
				int num3 = Mathf.FloorToInt(this.m_sparksAccum);
				this.m_sparksAccum -= (float)num3;
				int num4 = num3;
				Vector3 vector4 = vector3;
				Vector3 vector5 = vector3;
				Vector3 up = Vector3.up;
				float num5 = 30f;
				float num6 = 0.5f;
				float? num7 = new float?(0.1f);
				float? num8 = new float?(1f);
				GlobalSparksDoer.SparksType sparksType = this.SparksModule.sparksType;
				GlobalSparksDoer.DoRandomParticleBurst(num4, vector4, vector5, up, num5, num6, num7, num8, null, sparksType);
			}
		}
		switch (this.animationStyle)
		{
		case BuffVFXAnimator.BuffAnimationStyle.CIRCLE:
			vector3 = this.m_target.specRigidbody.UnitCenter.ToVector3ZUp(0f) + Quaternion.Euler(0f, 0f, num2 * 360f) * Vector3.right;
			break;
		case BuffVFXAnimator.BuffAnimationStyle.SWARM:
			num2 *= 3.1415927f;
			vector3 = this.m_target.specRigidbody.UnitCenter.ToVector3ZUp(0f) + new Vector3(Mathf.Sin(num2) + 2f * Mathf.Sin(2f * num2), Mathf.Cos(num2) - 2f * Mathf.Cos(2f * num2), 0f) / 2f;
			break;
		case BuffVFXAnimator.BuffAnimationStyle.PIERCE:
			this.m_transform.position = (this.m_target.specRigidbody.HitboxPixelCollider.UnitBottomLeft + this.m_hitboxOriginOffset).ToVector3ZUp(0f);
			return;
		case BuffVFXAnimator.BuffAnimationStyle.TETRIS:
			this.m_transform.position = (this.m_target.specRigidbody.HitboxPixelCollider.UnitBottomLeft + this.m_hitboxOriginOffset).ToVector3ZUp(0f);
			return;
		}
		this.m_transform.position = vector3;
	}

	// Token: 0x04004250 RID: 16976
	[SerializeField]
	protected BuffVFXAnimator.BuffAnimationStyle animationStyle;

	// Token: 0x04004251 RID: 16977
	public float motionPeriod = 1f;

	// Token: 0x04004252 RID: 16978
	public float ChanceOfApplication = 1f;

	// Token: 0x04004253 RID: 16979
	public bool persistsOnDeath = true;

	// Token: 0x04004254 RID: 16980
	public float AdditionalPierceDepth;

	// Token: 0x04004255 RID: 16981
	public bool UsesVFXToSpawnOnDeath;

	// Token: 0x04004256 RID: 16982
	public VFXPool VFXToSpawnOnDeath;

	// Token: 0x04004257 RID: 16983
	public GameObject NonPoolVFX;

	// Token: 0x04004258 RID: 16984
	public bool DoesSparks;

	// Token: 0x04004259 RID: 16985
	public GlobalSparksModule SparksModule;

	// Token: 0x0400425A RID: 16986
	[Header("Tetris")]
	public TetrisBuff.TetrisType tetrominoType;

	// Token: 0x0400425B RID: 16987
	private bool m_initialized;

	// Token: 0x0400425C RID: 16988
	private GameActor m_target;

	// Token: 0x0400425D RID: 16989
	private Transform m_transform;

	// Token: 0x0400425E RID: 16990
	private float m_elapsed;

	// Token: 0x0400425F RID: 16991
	private float parametricStartPoint;

	// Token: 0x04004260 RID: 16992
	private float m_pierceAngle;

	// Token: 0x04004261 RID: 16993
	private Vector2 m_hitboxOriginOffset;

	// Token: 0x04004262 RID: 16994
	private bool ForceFailure;

	// Token: 0x04004263 RID: 16995
	private float m_sparksAccum;

	// Token: 0x02000E1B RID: 3611
	protected enum BuffAnimationStyle
	{
		// Token: 0x04004265 RID: 16997
		CIRCLE,
		// Token: 0x04004266 RID: 16998
		SWARM,
		// Token: 0x04004267 RID: 16999
		PIERCE,
		// Token: 0x04004268 RID: 17000
		TETRIS
	}
}
