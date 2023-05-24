using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001484 RID: 5252
public class RelodestoneItem : PlayerItem
{
	// Token: 0x0600776A RID: 30570 RVA: 0x002F98E4 File Offset: 0x002F7AE4
	protected override void DoEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Play_OBJ_ammo_suck_01", base.gameObject);
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			return;
		}
		this.m_owner = user;
		this.m_totalAbsorbedDuringCycle = 0;
		base.IsCurrentlyActive = true;
		this.m_activeElapsed = 0f;
		this.m_activeDuration = this.duration;
		this.m_instanceVFX = SpawnManager.SpawnVFX(this.ContinuousVFX, false);
		this.m_instanceVFX.transform.parent = user.transform;
		this.m_instanceVFX.transform.position = user.CenterPosition.ToVector3ZisY(0f);
	}

	// Token: 0x0600776B RID: 30571 RVA: 0x002F9988 File Offset: 0x002F7B88
	public override void OnItemSwitched(PlayerController user)
	{
		this.BecomeInactive();
		base.OnItemSwitched(user);
	}

	// Token: 0x0600776C RID: 30572 RVA: 0x002F9998 File Offset: 0x002F7B98
	protected override void OnPreDrop(PlayerController user)
	{
		this.BecomeInactive();
		base.OnPreDrop(user);
	}

	// Token: 0x0600776D RID: 30573 RVA: 0x002F99A8 File Offset: 0x002F7BA8
	private void BecomeInactive()
	{
		base.IsCurrentlyActive = false;
		if (this.m_totalAbsorbedDuringCycle > 0 && this.m_owner && this.m_owner.HasActiveBonusSynergy(CustomSynergyType.RELODESTAR, false))
		{
			int num = Mathf.CeilToInt((float)this.m_totalAbsorbedDuringCycle / 20f);
			int num2 = Mathf.CeilToInt((float)this.m_totalAbsorbedDuringCycle / (float)num);
			this.RelodestarBurst.MinToSpawnPerWave = num2;
			this.RelodestarBurst.MaxToSpawnPerWave = num2;
			this.RelodestarBurst.NumberWaves = num;
			this.RelodestarBurst.TimeBetweenWaves = 1f;
			this.RelodestarBurst.DoBurst(this.m_owner, null, null);
		}
		this.m_totalAbsorbedDuringCycle = 0;
		if (this.m_instanceVFX)
		{
			SpawnManager.Despawn(this.m_instanceVFX);
			this.m_instanceVFX = null;
		}
	}

	// Token: 0x0600776E RID: 30574 RVA: 0x002F9A90 File Offset: 0x002F7C90
	private void LateUpdate()
	{
		if (Dungeon.IsGenerating)
		{
			return;
		}
		if (base.IsActive && this.m_owner)
		{
			Vector2 centerPosition = this.m_owner.CenterPosition;
			int num = 0;
			for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
			{
				Projectile projectile = StaticReferenceManager.AllProjectiles[i];
				if (projectile && projectile.specRigidbody)
				{
					bool flag = this.AdjustRigidbodyVelocity(projectile.specRigidbody, centerPosition);
					if (flag)
					{
						num++;
					}
				}
			}
			this.m_totalAbsorbedDuringCycle += num;
			if (num > 0 && this.m_owner.CurrentGun && this.m_owner.CurrentGun.CanGainAmmo)
			{
				this.m_owner.CurrentGun.GainAmmo(num);
			}
			if (this.m_activeElapsed >= this.m_activeDuration)
			{
				this.BecomeInactive();
			}
		}
	}

	// Token: 0x0600776F RID: 30575 RVA: 0x002F9B9C File Offset: 0x002F7D9C
	private Vector2 GetFrameAccelerationForRigidbody(Vector2 unitCenter, Vector2 myCenter, float currentDistance, float g)
	{
		Vector2 zero = Vector2.zero;
		float num = Mathf.Clamp01(1f - currentDistance / this.EffectRadius);
		float num2 = g * num * num;
		Vector2 normalized = (myCenter - unitCenter).normalized;
		return normalized * num2;
	}

	// Token: 0x06007770 RID: 30576 RVA: 0x002F9BE4 File Offset: 0x002F7DE4
	private bool AdjustRigidbodyVelocity(SpeculativeRigidbody other, Vector2 myCenter)
	{
		bool flag = false;
		Vector2 vector = other.UnitCenter - myCenter;
		float effectRadius = this.EffectRadius;
		float num = Vector2.SqrMagnitude(vector);
		if (num >= effectRadius)
		{
			return flag;
		}
		float gravitationalForce = this.GravitationalForce;
		Vector2 vector2 = other.Velocity;
		Projectile projectile = other.projectile;
		if (!projectile || projectile.Owner is PlayerController)
		{
			return false;
		}
		if (projectile.GetComponent<ChainLightningModifier>())
		{
			UnityEngine.Object.Destroy(projectile.GetComponent<ChainLightningModifier>());
		}
		projectile.collidesWithPlayer = false;
		if (other.GetComponent<BlackHoleDoer>() != null)
		{
			return false;
		}
		if (vector2 == Vector2.zero)
		{
			if (!projectile.IsBulletScript)
			{
				return false;
			}
			Vector2 vector3 = myCenter - other.UnitCenter;
			if (vector3 == Vector2.zero)
			{
				vector3 = new Vector2(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f);
			}
			vector2 = vector3.normalized * 3f;
		}
		if (num < 2f)
		{
			projectile.DieInAir(false, true, true, false);
			flag = true;
		}
		Vector2 frameAccelerationForRigidbody = this.GetFrameAccelerationForRigidbody(other.UnitCenter, myCenter, Mathf.Sqrt(num), gravitationalForce);
		float num2 = Mathf.Clamp(BraveTime.DeltaTime, 0f, 0.02f);
		Vector2 vector4 = frameAccelerationForRigidbody * num2;
		Vector2 vector5 = vector2 + vector4;
		if (BraveTime.DeltaTime > 0.02f)
		{
			vector5 *= 0.02f / BraveTime.DeltaTime;
		}
		other.Velocity = vector5;
		if (projectile != null)
		{
			projectile.collidesWithPlayer = false;
			if (projectile.IsBulletScript)
			{
				projectile.RemoveBulletScriptControl();
			}
			if (vector5 != Vector2.zero)
			{
				projectile.Direction = vector5.normalized;
				projectile.Speed = Mathf.Max(3f, vector5.magnitude);
				other.Velocity = projectile.Direction * projectile.Speed;
				if (projectile.shouldRotate && (vector5.x != 0f || vector5.y != 0f))
				{
					float num3 = BraveMathCollege.Atan2Degrees(projectile.Direction);
					if (!float.IsNaN(num3) && !float.IsInfinity(num3))
					{
						Quaternion quaternion = Quaternion.Euler(0f, 0f, num3);
						if (!float.IsNaN(quaternion.x) && !float.IsNaN(quaternion.y))
						{
							projectile.transform.rotation = quaternion;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x04007961 RID: 31073
	public float EffectRadius = 10f;

	// Token: 0x04007962 RID: 31074
	public float duration = 3f;

	// Token: 0x04007963 RID: 31075
	public float GravitationalForce = 500f;

	// Token: 0x04007964 RID: 31076
	public GameObject ContinuousVFX;

	// Token: 0x04007965 RID: 31077
	public RadialBurstInterface RelodestarBurst;

	// Token: 0x04007966 RID: 31078
	private PlayerController m_owner;

	// Token: 0x04007967 RID: 31079
	private GameObject m_instanceVFX;

	// Token: 0x04007968 RID: 31080
	private int m_totalAbsorbedDuringCycle;
}
