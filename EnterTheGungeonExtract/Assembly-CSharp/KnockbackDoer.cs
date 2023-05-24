using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020010AB RID: 4267
public class KnockbackDoer : BraveBehaviour
{
	// Token: 0x06005E0F RID: 24079 RVA: 0x00241474 File Offset: 0x0023F674
	private void Awake()
	{
		this.m_player = base.GetComponent<PlayerController>();
		this.m_reaper = base.GetComponent<SuperReaperController>();
		this.m_activeKnockbacks = new List<ActiveKnockbackData>();
		this.m_activeContinuousKnockbacks = new List<Vector2>();
	}

	// Token: 0x06005E10 RID: 24080 RVA: 0x002414A4 File Offset: 0x0023F6A4
	private void Start()
	{
		if (base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
		}
	}

	// Token: 0x06005E11 RID: 24081 RVA: 0x002414E0 File Offset: 0x0023F6E0
	private void Update()
	{
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = Vector2.zero;
		for (int i = this.m_activeKnockbacks.Count - 1; i >= 0; i--)
		{
			ActiveKnockbackData activeKnockbackData = this.m_activeKnockbacks[i];
			if (activeKnockbackData.curveFalloff != null)
			{
				activeKnockbackData.elapsedTime += BraveTime.DeltaTime * this.timeScalar;
				float num = activeKnockbackData.elapsedTime / activeKnockbackData.curveTime;
				float num2 = activeKnockbackData.curveFalloff.Evaluate(num);
				if (num >= 1f)
				{
					this.m_activeKnockbacks.RemoveAt(i);
				}
				else if (activeKnockbackData.immutable)
				{
					vector2 += activeKnockbackData.initialKnockback * num2;
				}
				else
				{
					vector += activeKnockbackData.initialKnockback * num2;
				}
			}
			else
			{
				activeKnockbackData.elapsedTime += BraveTime.DeltaTime * this.timeScalar;
				float num3 = activeKnockbackData.elapsedTime / activeKnockbackData.curveTime;
				float num4 = 1f - num3 * num3;
				activeKnockbackData.knockback = activeKnockbackData.initialKnockback * num4;
				if (activeKnockbackData.immutable)
				{
					vector2 += activeKnockbackData.knockback;
				}
				else
				{
					vector += activeKnockbackData.knockback;
				}
				if (activeKnockbackData.elapsedTime >= activeKnockbackData.curveTime)
				{
					this.m_activeKnockbacks.RemoveAt(i);
				}
			}
		}
		bool flag = true;
		for (int j = 0; j < this.m_activeContinuousKnockbacks.Count; j++)
		{
			if (this.m_activeContinuousKnockbacks[j] != Vector2.zero)
			{
				vector += this.m_activeContinuousKnockbacks[j];
				flag = false;
			}
		}
		if (flag && this.m_activeContinuousKnockbacks.Count > 0)
		{
			this.m_activeContinuousKnockbacks.Clear();
		}
		vector *= this.knockbackMultiplier;
		if (this.m_isImmobile.Value)
		{
			vector = Vector2.zero;
		}
		if (this.m_reaper != null)
		{
			this.m_reaper.knockbackComponent = vector + vector2;
		}
		if (this.m_player != null)
		{
			this.m_player.knockbackComponent = vector;
			this.m_player.immutableKnockbackComponent = vector2;
		}
		if (base.aiActor != null)
		{
			vector += vector2;
			float magnitude = vector.magnitude;
			vector = vector.normalized * Mathf.Min(magnitude, 30f);
			base.aiActor.KnockbackVelocity = vector;
		}
	}

	// Token: 0x06005E12 RID: 24082 RVA: 0x00241780 File Offset: 0x0023F980
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005E13 RID: 24083 RVA: 0x00241788 File Offset: 0x0023F988
	public void SetImmobile(bool value, string reason = "")
	{
		if (string.IsNullOrEmpty(reason))
		{
			this.m_isImmobile.BaseValue = value;
			if (!value)
			{
				this.m_isImmobile.ClearOverrides();
			}
		}
		else
		{
			this.m_isImmobile.SetOverride(reason, value, null);
		}
		if (value && base.specRigidbody)
		{
			base.specRigidbody.Velocity = Vector2.zero;
		}
	}

	// Token: 0x06005E14 RID: 24084 RVA: 0x00241800 File Offset: 0x0023FA00
	public void TriggerTemporaryKnockbackInvulnerability(float duration)
	{
		base.StartCoroutine(this.HandleKnockbackInvulnerabilityPeriod(duration));
	}

	// Token: 0x06005E15 RID: 24085 RVA: 0x00241810 File Offset: 0x0023FA10
	private IEnumerator HandleKnockbackInvulnerabilityPeriod(float duration)
	{
		this.SetImmobile(true, "HandleKnockbackInvulnerabilityPeriod");
		yield return new WaitForSeconds(duration);
		this.SetImmobile(false, "HandleKnockbackInvulnerabilityPeriod");
		yield break;
	}

	// Token: 0x06005E16 RID: 24086 RVA: 0x00241834 File Offset: 0x0023FA34
	public ActiveKnockbackData ApplyKnockback(Vector2 direction, float force, bool immutable = false)
	{
		if (this.m_isImmobile.Value)
		{
			return null;
		}
		return this.ApplyKnockback(direction, force, 0.5f, immutable);
	}

	// Token: 0x06005E17 RID: 24087 RVA: 0x00241858 File Offset: 0x0023FA58
	public ActiveKnockbackData ApplyKnockback(Vector2 direction, float force, float time, bool immutable = false)
	{
		if (this.m_isImmobile.Value)
		{
			return null;
		}
		ActiveKnockbackData activeKnockbackData = new ActiveKnockbackData(direction.normalized * (force / (this.weight / 10f)), time, immutable);
		this.m_activeKnockbacks.Add(activeKnockbackData);
		return activeKnockbackData;
	}

	// Token: 0x06005E18 RID: 24088 RVA: 0x002418A8 File Offset: 0x0023FAA8
	public ActiveKnockbackData ApplyKnockback(Vector2 direction, float force, AnimationCurve customFalloff, float time, bool immutable = false)
	{
		if (this.m_isImmobile.Value)
		{
			return null;
		}
		ActiveKnockbackData activeKnockbackData = new ActiveKnockbackData(direction.normalized * (force / (this.weight / 10f)), customFalloff, time, immutable);
		this.m_activeKnockbacks.Add(activeKnockbackData);
		return activeKnockbackData;
	}

	// Token: 0x06005E19 RID: 24089 RVA: 0x002418FC File Offset: 0x0023FAFC
	public ActiveKnockbackData ApplySourcedKnockback(Vector2 direction, float force, GameObject source, bool immutable = false)
	{
		if (this.m_isImmobile.Value)
		{
			return null;
		}
		return this.ApplySourcedKnockback(direction, force, 0.5f, source, immutable);
	}

	// Token: 0x06005E1A RID: 24090 RVA: 0x00241920 File Offset: 0x0023FB20
	public ActiveKnockbackData ApplySourcedKnockback(Vector2 direction, float force, float time, GameObject source, bool immutable = false)
	{
		if (this.m_isImmobile.Value)
		{
			return null;
		}
		if (this.CheckSourceInKnockbacks(source))
		{
			return null;
		}
		ActiveKnockbackData activeKnockbackData = this.ApplyKnockback(direction, force, time, immutable);
		activeKnockbackData.sourceObject = source;
		return activeKnockbackData;
	}

	// Token: 0x06005E1B RID: 24091 RVA: 0x00241964 File Offset: 0x0023FB64
	public ActiveKnockbackData ApplySourcedKnockback(Vector2 direction, float force, AnimationCurve customFalloff, float time, GameObject source, bool immutable = false)
	{
		if (this.m_isImmobile.Value)
		{
			return null;
		}
		if (this.CheckSourceInKnockbacks(source))
		{
			return null;
		}
		ActiveKnockbackData activeKnockbackData = this.ApplyKnockback(direction, force, customFalloff, time, immutable);
		activeKnockbackData.sourceObject = source;
		return activeKnockbackData;
	}

	// Token: 0x06005E1C RID: 24092 RVA: 0x002419AC File Offset: 0x0023FBAC
	public int ApplyContinuousKnockback(Vector2 direction, float force)
	{
		this.m_activeContinuousKnockbacks.Add(direction.normalized * (force / (this.weight / 10f)));
		return this.m_activeContinuousKnockbacks.Count - 1;
	}

	// Token: 0x06005E1D RID: 24093 RVA: 0x002419E0 File Offset: 0x0023FBE0
	public void UpdateContinuousKnockback(Vector2 direction, float force, int id)
	{
		if (this.m_activeContinuousKnockbacks.Count > id)
		{
			this.m_activeContinuousKnockbacks[id] = direction.normalized * (force / (this.weight / 10f));
		}
	}

	// Token: 0x06005E1E RID: 24094 RVA: 0x00241A1C File Offset: 0x0023FC1C
	public void EndContinuousKnockback(int id)
	{
		if (id >= 0 && id < this.m_activeContinuousKnockbacks.Count)
		{
			this.m_activeContinuousKnockbacks[id] = Vector2.zero;
		}
	}

	// Token: 0x06005E1F RID: 24095 RVA: 0x00241A48 File Offset: 0x0023FC48
	public void ClearContinuousKnockbacks()
	{
		if (this.m_activeContinuousKnockbacks != null)
		{
			for (int i = 0; i < this.m_activeContinuousKnockbacks.Count; i++)
			{
				this.EndContinuousKnockback(i);
			}
		}
	}

	// Token: 0x06005E20 RID: 24096 RVA: 0x00241A84 File Offset: 0x0023FC84
	protected virtual void OnCollision(CollisionData collision)
	{
		if (collision.collisionType == CollisionData.CollisionType.Rigidbody && collision.OtherRigidbody.Velocity != Vector2.zero)
		{
			return;
		}
		if (base.healthHaver != null && base.healthHaver.IsDead)
		{
			for (int i = 0; i < this.m_activeKnockbacks.Count; i++)
			{
				if (Mathf.Sign(collision.Normal.x) != Mathf.Sign(this.m_activeKnockbacks[i].initialKnockback.x) && collision.Normal.x != 0f)
				{
					if (this.shouldBounce)
					{
						this.m_activeKnockbacks[i].initialKnockback = Vector2.Scale(this.m_activeKnockbacks[i].initialKnockback, new Vector2(-1f, 1f));
					}
					this.m_activeKnockbacks[i].initialKnockback = Vector2.Scale(this.m_activeKnockbacks[i].initialKnockback, new Vector2(1f - this.collisionDecay, 1f));
				}
				if (Mathf.Sign(collision.Normal.y) != Mathf.Sign(this.m_activeKnockbacks[i].initialKnockback.y) && collision.Normal.y != 0f)
				{
					if (this.shouldBounce)
					{
						this.m_activeKnockbacks[i].initialKnockback = Vector2.Scale(this.m_activeKnockbacks[i].initialKnockback, new Vector2(1f, -1f));
					}
					this.m_activeKnockbacks[i].initialKnockback = Vector2.Scale(this.m_activeKnockbacks[i].initialKnockback, new Vector2(1f, 1f - this.collisionDecay));
				}
			}
		}
	}

	// Token: 0x06005E21 RID: 24097 RVA: 0x00241C7C File Offset: 0x0023FE7C
	private bool CheckSourceInKnockbacks(GameObject source)
	{
		if (source == null)
		{
			return false;
		}
		for (int i = 0; i < this.m_activeKnockbacks.Count; i++)
		{
			if (this.m_activeKnockbacks[i].sourceObject == source)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04005822 RID: 22562
	private const float MAX_ENEMY_KNOCKBACK_MAGNITUDE = 30f;

	// Token: 0x04005823 RID: 22563
	private const float DEFAULT_KNOCKBACK_TIME = 0.5f;

	// Token: 0x04005824 RID: 22564
	public float weight = 10f;

	// Token: 0x04005825 RID: 22565
	public float deathMultiplier = 5f;

	// Token: 0x04005826 RID: 22566
	public bool knockbackWhileReflecting;

	// Token: 0x04005827 RID: 22567
	public bool shouldBounce = true;

	// Token: 0x04005828 RID: 22568
	public float collisionDecay = 0.5f;

	// Token: 0x04005829 RID: 22569
	[NonSerialized]
	public float knockbackMultiplier = 1f;

	// Token: 0x0400582A RID: 22570
	[NonSerialized]
	public float timeScalar = 1f;

	// Token: 0x0400582B RID: 22571
	private SuperReaperController m_reaper;

	// Token: 0x0400582C RID: 22572
	private PlayerController m_player;

	// Token: 0x0400582D RID: 22573
	private List<ActiveKnockbackData> m_activeKnockbacks;

	// Token: 0x0400582E RID: 22574
	private List<Vector2> m_activeContinuousKnockbacks;

	// Token: 0x0400582F RID: 22575
	private OverridableBool m_isImmobile = new OverridableBool(false);
}
