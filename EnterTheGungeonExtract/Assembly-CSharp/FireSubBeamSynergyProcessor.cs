using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020016E7 RID: 5863
public class FireSubBeamSynergyProcessor : MonoBehaviour
{
	// Token: 0x06008857 RID: 34903 RVA: 0x0038823C File Offset: 0x0038643C
	public void Awake()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		this.m_beam = base.GetComponent<BasicBeamController>();
	}

	// Token: 0x06008858 RID: 34904 RVA: 0x00388258 File Offset: 0x00386458
	public void Update()
	{
		bool flag = true;
		if (this.Mode == FireSubBeamSynergyProcessor.SubBeamMode.FROM_BEAM)
		{
			if (!(this.m_beam.Owner is PlayerController) || !(this.m_beam.Owner as PlayerController).HasActiveBonusSynergy(this.SynergyToCheck, false))
			{
				return;
			}
			flag = this.m_beam.State == BasicBeamController.BeamState.Firing;
		}
		else if (!(this.m_projectile.Owner is PlayerController) || !(this.m_projectile.Owner as PlayerController).HasActiveBonusSynergy(this.SynergyToCheck, false))
		{
			return;
		}
		float num = ((!this.m_beam) ? 0f : Vector2.Distance(this.m_beam.GetPointOnBeam(0f), this.m_beam.GetPointOnBeam(1f)));
		if (flag && (this.Mode != FireSubBeamSynergyProcessor.SubBeamMode.FROM_BEAM || num > 1.5f))
		{
			if (this.m_subbeams.Count > 0)
			{
				for (int i = 0; i < this.m_subbeams.Count; i++)
				{
					Vector2 vector;
					if (this.Mode == FireSubBeamSynergyProcessor.SubBeamMode.FROM_BEAM)
					{
						this.m_subbeams[i].subbeam.Origin = this.m_beam.GetPointOnBeam(this.m_subbeams[i].percent);
						vector = this.m_beam.Direction;
					}
					else
					{
						this.m_subbeams[i].subbeam.Origin = this.m_projectile.specRigidbody.UnitCenter;
						vector = this.m_projectile.Direction;
					}
					this.m_subbeams[i].subbeam.Direction = Quaternion.Euler(0f, 0f, this.m_subbeams[i].angle) * vector;
					this.m_subbeams[i].subbeam.LateUpdatePosition(this.m_subbeams[i].subbeam.Origin);
				}
			}
			else
			{
				for (int j = 0; j < this.NumberBeams; j++)
				{
					SubbeamData subbeamData = new SubbeamData();
					float num2 = 1f / (float)(this.NumberBeams + 1) * (float)(j + 1);
					float num3 = this.BeamAngle;
					if (this.Mode == FireSubBeamSynergyProcessor.SubBeamMode.FROM_PROJECTILE_CENTER)
					{
						num3 = 360f / (float)this.NumberBeams * (float)j;
					}
					Vector2 vector2 = ((this.Mode != FireSubBeamSynergyProcessor.SubBeamMode.FROM_BEAM) ? this.m_projectile.specRigidbody.UnitCenter : this.m_beam.GetPointOnBeam(num2));
					Vector2 vector3 = ((this.Mode != FireSubBeamSynergyProcessor.SubBeamMode.FROM_BEAM) ? this.m_projectile.Direction : this.m_beam.Direction);
					subbeamData.subbeam = this.CreateSubBeam(this.SubBeamProjectile, vector2, Quaternion.Euler(0f, 0f, num3) * vector3);
					subbeamData.angle = num3;
					subbeamData.percent = num2;
					this.m_subbeams.Add(subbeamData);
					if (this.Mode == FireSubBeamSynergyProcessor.SubBeamMode.FROM_BEAM)
					{
						SubbeamData subbeamData2 = new SubbeamData();
						num3 = -this.BeamAngle;
						subbeamData2.subbeam = this.CreateSubBeam(this.SubBeamProjectile, this.m_beam.GetPointOnBeam(num2), Quaternion.Euler(0f, 0f, num3) * this.m_beam.Direction);
						subbeamData2.percent = num2;
						subbeamData2.angle = num3;
						this.m_subbeams.Add(subbeamData2);
					}
				}
				if (this.m_projectile && this.m_projectile.sprite)
				{
					this.m_projectile.sprite.ForceRotationRebuild();
				}
			}
		}
		else if (this.m_subbeams.Count > 0)
		{
			for (int k = 0; k < this.m_subbeams.Count; k++)
			{
				this.m_subbeams[k].subbeam.CeaseAttack();
				this.m_subbeams.RemoveAt(k);
				k--;
			}
		}
	}

	// Token: 0x06008859 RID: 34905 RVA: 0x003886A0 File Offset: 0x003868A0
	private void OnDestroy()
	{
		if (this.m_subbeams.Count > 0)
		{
			for (int i = 0; i < this.m_subbeams.Count; i++)
			{
				this.m_subbeams[i].subbeam.CeaseAttack();
				this.m_subbeams.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x0600885A RID: 34906 RVA: 0x00388700 File Offset: 0x00386900
	private BeamController CreateSubBeam(Projectile subBeamProjectilePrefab, Vector2 pos, Vector2 dir)
	{
		BeamController component = subBeamProjectilePrefab.GetComponent<BeamController>();
		if (component is BasicBeamController)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(subBeamProjectilePrefab.gameObject);
			gameObject.name = base.gameObject.name + " (Subbeam)";
			BasicBeamController component2 = gameObject.GetComponent<BasicBeamController>();
			component2.State = BasicBeamController.BeamState.Firing;
			component2.HitsPlayers = false;
			component2.HitsEnemies = true;
			component2.Origin = pos;
			component2.Direction = dir;
			component2.usesChargeDelay = false;
			component2.muzzleAnimation = string.Empty;
			component2.chargeAnimation = string.Empty;
			component2.beamStartAnimation = string.Empty;
			component2.projectile.Owner = this.m_projectile.Owner;
			if (this.Mode == FireSubBeamSynergyProcessor.SubBeamMode.FROM_BEAM)
			{
				component2.Owner = this.m_beam.Owner;
				component2.Gun = this.m_beam.Gun;
				component2.DamageModifier = this.m_beam.DamageModifier;
				component2.playerStatsModified = this.m_beam.playerStatsModified;
			}
			else
			{
				component2.Owner = this.m_projectile.Owner;
				component2.Gun = this.m_projectile.PossibleSourceGun;
				component2.DamageModifier = this.FromProjectileDamageModifier;
			}
			component2.HeightOffset = -0.25f;
			return component2;
		}
		if (component is RaidenBeamController)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(subBeamProjectilePrefab.gameObject);
			gameObject2.name = base.gameObject.name + " (Subbeam)";
			RaidenBeamController component3 = gameObject2.GetComponent<RaidenBeamController>();
			component3.SelectRandomTarget = true;
			component3.HitsPlayers = false;
			component3.HitsEnemies = true;
			component3.Origin = pos;
			component3.Direction = dir;
			component3.usesChargeDelay = false;
			component3.projectile.Owner = this.m_projectile.Owner;
			if (this.Mode == FireSubBeamSynergyProcessor.SubBeamMode.FROM_BEAM)
			{
				component3.Owner = this.m_beam.Owner;
				component3.Gun = this.m_beam.Gun;
				component3.DamageModifier = this.m_beam.DamageModifier;
			}
			else
			{
				component3.Owner = this.m_projectile.Owner;
				component3.Gun = this.m_projectile.PossibleSourceGun;
				component3.DamageModifier = this.FromProjectileDamageModifier;
			}
			return component3;
		}
		return null;
	}

	// Token: 0x04008DAF RID: 36271
	[LongNumericEnum]
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04008DB0 RID: 36272
	public FireSubBeamSynergyProcessor.SubBeamMode Mode;

	// Token: 0x04008DB1 RID: 36273
	public Projectile SubBeamProjectile;

	// Token: 0x04008DB2 RID: 36274
	public int NumberBeams = 3;

	// Token: 0x04008DB3 RID: 36275
	public float BeamAngle = 90f;

	// Token: 0x04008DB4 RID: 36276
	private BasicBeamController m_beam;

	// Token: 0x04008DB5 RID: 36277
	private Projectile m_projectile;

	// Token: 0x04008DB6 RID: 36278
	public float FromProjectileDamageModifier = 0.5f;

	// Token: 0x04008DB7 RID: 36279
	private List<SubbeamData> m_subbeams = new List<SubbeamData>();

	// Token: 0x020016E8 RID: 5864
	public enum SubBeamMode
	{
		// Token: 0x04008DB9 RID: 36281
		FROM_BEAM,
		// Token: 0x04008DBA RID: 36282
		FROM_PROJECTILE_CENTER
	}
}
