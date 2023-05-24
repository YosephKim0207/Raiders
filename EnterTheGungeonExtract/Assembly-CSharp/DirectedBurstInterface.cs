using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200144F RID: 5199
[Serializable]
public class DirectedBurstInterface
{
	// Token: 0x0600760D RID: 30221 RVA: 0x002EFADC File Offset: 0x002EDCDC
	public void DoBurst(PlayerController source, float aimAngle)
	{
		if (this.NumberWaves == 1 && !this.SpiralWaves)
		{
			this.ImmediateBurst(this.ProjectileInterface.GetProjectile(source), source, aimAngle);
		}
		else
		{
			source.StartCoroutine(this.HandleBurst(this.ProjectileInterface.GetProjectile(source), source, aimAngle));
		}
	}

	// Token: 0x0600760E RID: 30222 RVA: 0x002EFB34 File Offset: 0x002EDD34
	private void ImmediateBurst(Projectile projectileToSpawn, PlayerController source, float aimAngle)
	{
		if (projectileToSpawn == null)
		{
			return;
		}
		int num = UnityEngine.Random.Range(this.MinToSpawnPerWave, this.MaxToSpawnPerWave);
		float num2 = this.AngleSubtended / (float)num;
		float num3 = -(this.AngleSubtended / 2f);
		num3 += aimAngle;
		bool flag = projectileToSpawn.GetComponent<BeamController>() != null;
		for (int i = 0; i < num; i++)
		{
			float num4 = num3 + num2 * (float)i;
			if (flag)
			{
				source.StartCoroutine(this.HandleFireShortBeam(projectileToSpawn, source, num4, 1f * (float)this.NumberWaves));
			}
			else
			{
				this.DoSingleProjectile(projectileToSpawn, source, num4);
			}
		}
	}

	// Token: 0x0600760F RID: 30223 RVA: 0x002EFBDC File Offset: 0x002EDDDC
	private IEnumerator HandleBurst(Projectile projectileToSpawn, PlayerController source, float aimAngle)
	{
		if (projectileToSpawn == null)
		{
			yield break;
		}
		bool projectileIsBeam = projectileToSpawn.GetComponent<BeamController>() != null;
		bool projectileExplodes = projectileToSpawn.GetComponent<ExplosiveModifier>() != null;
		bool projectileSpawns = projectileToSpawn.GetComponent<SpawnProjModifier>() != null;
		bool reducedCountProjectile = projectileToSpawn.GetComponent<BlackHoleDoer>() != null;
		int modWaves = this.NumberWaves;
		if (projectileIsBeam)
		{
			modWaves = 1;
		}
		if (projectileExplodes)
		{
			modWaves = 1;
		}
		if (projectileSpawns)
		{
			modWaves = 1;
		}
		if (reducedCountProjectile)
		{
			modWaves = 1;
		}
		for (int w = 0; w < modWaves; w++)
		{
			int numToSpawn = UnityEngine.Random.Range(this.MinToSpawnPerWave, this.MaxToSpawnPerWave);
			if (reducedCountProjectile)
			{
				numToSpawn = 3;
			}
			float angleStep = this.AngleSubtended / (float)numToSpawn;
			float angleBase = -(this.AngleSubtended / 2f);
			float spiralDelay = this.TimeBetweenWaves / (float)numToSpawn;
			angleBase += aimAngle;
			for (int i = 0; i < numToSpawn; i++)
			{
				float targetAngle = angleBase + angleStep * (float)i;
				if (projectileIsBeam)
				{
					source.StartCoroutine(this.HandleFireShortBeam(projectileToSpawn, source, targetAngle, 1f * (float)this.NumberWaves));
				}
				else
				{
					this.DoSingleProjectile(projectileToSpawn, source, targetAngle);
				}
				if (this.SpiralWaves)
				{
					yield return new WaitForSeconds(spiralDelay);
				}
			}
			if (!this.SpiralWaves)
			{
				yield return new WaitForSeconds(this.TimeBetweenWaves);
			}
		}
		yield break;
	}

	// Token: 0x06007610 RID: 30224 RVA: 0x002EFC0C File Offset: 0x002EDE0C
	private IEnumerator HandleFireShortBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle, float duration)
	{
		float elapsed = 0f;
		BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, targetAngle);
		yield return null;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			this.ContinueFiringBeam(beam, source);
			yield return null;
		}
		this.CeaseBeam(beam);
		yield break;
	}

	// Token: 0x06007611 RID: 30225 RVA: 0x002EFC44 File Offset: 0x002EDE44
	private void DoSingleProjectile(Projectile projectileToSpawn, PlayerController source, float targetAngle)
	{
		Vector2 vector = ((!source.CurrentGun || !source.CurrentGun.barrelOffset) ? source.specRigidbody.UnitCenter : source.CurrentGun.barrelOffset.position.XY());
		GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, vector, Quaternion.Euler(0f, 0f, targetAngle), true);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Owner = source;
		component.Shooter = source.specRigidbody;
		if (this.MinToSpawnPerWave == 1 && this.MaxToSpawnPerWave == 1 && this.NumberWaves == 1 && !this.SpiralWaves && this.ProjectileInterface.UseCurrentGunProjectile && source.HasActiveBonusSynergy(CustomSynergyType.DOUBLE_HOLSTER, false))
		{
			HomingModifier homingModifier = component.gameObject.GetComponent<HomingModifier>();
			if (homingModifier == null)
			{
				homingModifier = component.gameObject.AddComponent<HomingModifier>();
				homingModifier.HomingRadius = 0f;
				homingModifier.AngularVelocity = 0f;
			}
			homingModifier.HomingRadius += 20f;
			homingModifier.AngularVelocity += 1080f;
		}
		if (this.UseShotgunStyleVelocityModifier)
		{
			component.baseData.speed = component.baseData.speed * (1f + UnityEngine.Random.Range(-15f, 15f) / 100f);
		}
		source.DoPostProcessProjectile(component);
		this.InternalPostProcessProjectile(component);
	}

	// Token: 0x06007612 RID: 30226 RVA: 0x002EFDD0 File Offset: 0x002EDFD0
	private void InternalPostProcessProjectile(Projectile proj)
	{
		if (proj && !this.ForceAllowGoop)
		{
			GoopModifier component = proj.GetComponent<GoopModifier>();
			if (component)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
	}

	// Token: 0x06007613 RID: 30227 RVA: 0x002EFE0C File Offset: 0x002EE00C
	private BeamController BeginFiringBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle)
	{
		GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, source.CenterPosition, Quaternion.identity, true);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Owner = source;
		BeamController component2 = gameObject.GetComponent<BeamController>();
		component2.Owner = source;
		component2.HitsPlayers = false;
		component2.HitsEnemies = true;
		Vector3 vector = BraveMathCollege.DegreesToVector(targetAngle, 1f);
		component2.Direction = vector;
		component2.Origin = source.CenterPosition;
		this.InternalPostProcessProjectile(component);
		return component2;
	}

	// Token: 0x06007614 RID: 30228 RVA: 0x002EFE94 File Offset: 0x002EE094
	private void ContinueFiringBeam(BeamController beam, PlayerController source)
	{
		beam.Origin = source.CenterPosition;
		beam.LateUpdatePosition(source.CenterPosition);
	}

	// Token: 0x06007615 RID: 30229 RVA: 0x002EFEB4 File Offset: 0x002EE0B4
	private void CeaseBeam(BeamController beam)
	{
		beam.CeaseAttack();
	}

	// Token: 0x040077C7 RID: 30663
	public PlayerItemProjectileInterface ProjectileInterface;

	// Token: 0x040077C8 RID: 30664
	public int MinToSpawnPerWave = 10;

	// Token: 0x040077C9 RID: 30665
	public int MaxToSpawnPerWave = 10;

	// Token: 0x040077CA RID: 30666
	public int NumberWaves = 1;

	// Token: 0x040077CB RID: 30667
	public float TimeBetweenWaves = 1f;

	// Token: 0x040077CC RID: 30668
	public bool SpiralWaves;

	// Token: 0x040077CD RID: 30669
	public float AngleSubtended = 30f;

	// Token: 0x040077CE RID: 30670
	public bool UseShotgunStyleVelocityModifier;

	// Token: 0x040077CF RID: 30671
	public bool ForceAllowGoop;
}
