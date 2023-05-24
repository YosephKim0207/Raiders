using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001452 RID: 5202
[Serializable]
public class RadialBurstInterface
{
	// Token: 0x06007623 RID: 30243 RVA: 0x002F0328 File Offset: 0x002EE528
	public void DoBurst(PlayerController source, Vector2? overrideSpawnPoint = null, Vector2? spawnPointOffset = null)
	{
		if (this.NumberWaves == 1 && !this.SpiralWaves)
		{
			this.ImmediateBurst(this.ProjectileInterface.GetProjectile(source), source, overrideSpawnPoint, spawnPointOffset);
		}
		else
		{
			source.StartCoroutine(this.HandleBurst(this.ProjectileInterface.GetProjectile(source), source, overrideSpawnPoint, spawnPointOffset));
		}
	}

	// Token: 0x06007624 RID: 30244 RVA: 0x002F0384 File Offset: 0x002EE584
	private AIActor GetNearestEnemy(Vector2 sourcePoint)
	{
		RoomHandler absoluteRoom = sourcePoint.GetAbsoluteRoom();
		float num = 0f;
		return absoluteRoom.GetNearestEnemy(sourcePoint, out num, true, true);
	}

	// Token: 0x06007625 RID: 30245 RVA: 0x002F03AC File Offset: 0x002EE5AC
	private void ImmediateBurst(Projectile projectileToSpawn, PlayerController source, Vector2? overrideSpawnPoint, Vector2? spawnPointOffset = null)
	{
		if (projectileToSpawn == null)
		{
			return;
		}
		int num = UnityEngine.Random.Range(this.MinToSpawnPerWave, this.MaxToSpawnPerWave);
		int radialBurstLimit = projectileToSpawn.GetRadialBurstLimit(source);
		if (radialBurstLimit < num)
		{
			num = radialBurstLimit;
		}
		float num2 = 360f / (float)num;
		float num3 = UnityEngine.Random.Range(0f, num2);
		if (this.AlignFirstShot && source && source.CurrentGun)
		{
			num3 = source.CurrentGun.CurrentAngle + this.AlignOffset;
		}
		if (this.AimFirstAtNearestEnemy)
		{
			Vector2 vector = ((overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value);
			vector = ((spawnPointOffset == null) ? vector : (vector + spawnPointOffset.Value));
			AIActor nearestEnemy = this.GetNearestEnemy(vector);
			if (nearestEnemy)
			{
				num3 = Vector2.Angle(Vector2.right, nearestEnemy.CenterPosition - vector);
			}
		}
		bool flag = projectileToSpawn.GetComponent<BeamController>() != null;
		for (int i = 0; i < num; i++)
		{
			float num4 = num3 + num2 * (float)i;
			if (flag)
			{
				source.StartCoroutine(this.HandleFireShortBeam(projectileToSpawn, source, num4, 1f * (float)this.NumberWaves, overrideSpawnPoint, spawnPointOffset));
			}
			else
			{
				this.DoSingleProjectile(projectileToSpawn, source, num4, overrideSpawnPoint, spawnPointOffset);
			}
		}
	}

	// Token: 0x06007626 RID: 30246 RVA: 0x002F051C File Offset: 0x002EE71C
	private IEnumerator HandleBurst(Projectile projectileToSpawn, PlayerController source, Vector2? overrideSpawnPoint, Vector2? spawnPointOffset = null)
	{
		if (projectileToSpawn == null)
		{
			yield break;
		}
		bool projectileIsBeam = projectileToSpawn.GetComponent<BeamController>() != null;
		bool projectileExplodes = projectileToSpawn.GetComponent<ExplosiveModifier>() != null;
		bool projectileSpawns = projectileToSpawn.GetComponent<SpawnProjModifier>() != null;
		bool reducedCountProjectile = projectileToSpawn.GetComponent<BlackHoleDoer>() != null;
		int limit = projectileToSpawn.GetRadialBurstLimit(source);
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
		if (limit > 0 && limit < 1000)
		{
			modWaves = 1;
		}
		int modSubwaves = Mathf.Max(1, this.NumberSubwaves);
		for (int w = 0; w < modWaves; w++)
		{
			int numToSpawn = UnityEngine.Random.Range(this.MinToSpawnPerWave, this.MaxToSpawnPerWave);
			if (limit < numToSpawn)
			{
				numToSpawn = limit;
			}
			if (reducedCountProjectile)
			{
				numToSpawn = 3;
			}
			float angleStep = 360f / (float)numToSpawn;
			float angleBase = UnityEngine.Random.Range(0f, angleStep);
			float spiralDelay = this.TimeBetweenWaves / (float)numToSpawn;
			if (this.AlignFirstShot && source && source.CurrentGun)
			{
				angleBase = source.CurrentGun.CurrentAngle;
			}
			for (int i = 0; i < numToSpawn; i++)
			{
				for (int j = 0; j < modSubwaves; j++)
				{
					float num = angleBase + angleStep * (float)i + (float)j * (360f / (float)modSubwaves);
					if (projectileIsBeam)
					{
						source.StartCoroutine(this.HandleFireShortBeam(projectileToSpawn, source, num, 1f * (float)this.NumberWaves, overrideSpawnPoint, spawnPointOffset));
					}
					else
					{
						this.DoSingleProjectile(projectileToSpawn, source, num, overrideSpawnPoint, spawnPointOffset);
					}
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

	// Token: 0x06007627 RID: 30247 RVA: 0x002F0554 File Offset: 0x002EE754
	private IEnumerator HandleFireShortBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle, float duration, Vector2? overrideSpawnPoint, Vector2? spawnPointOffset = null)
	{
		float elapsed = 0f;
		BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, targetAngle, overrideSpawnPoint, spawnPointOffset);
		yield return null;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			if (this.SweepBeams)
			{
				beam.Direction = Quaternion.Euler(0f, 0f, BraveTime.DeltaTime / duration * this.BeamSweepDegrees) * beam.Direction;
			}
			this.ContinueFiringBeam(beam, source, overrideSpawnPoint, spawnPointOffset);
			yield return null;
		}
		this.CeaseBeam(beam);
		yield break;
	}

	// Token: 0x06007628 RID: 30248 RVA: 0x002F059C File Offset: 0x002EE79C
	private void DoSingleProjectile(Projectile projectileToSpawn, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint, Vector2? spawnPointOffset = null)
	{
		Vector2 vector = ((overrideSpawnPoint == null) ? source.specRigidbody.UnitCenter : overrideSpawnPoint.Value);
		vector = ((spawnPointOffset == null) ? vector : (vector + spawnPointOffset.Value));
		GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, vector, Quaternion.Euler(0f, 0f, targetAngle), true);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Owner = source;
		component.Shooter = source.specRigidbody;
		source.DoPostProcessProjectile(component);
		if (this.CustomPostProcessProjectile != null)
		{
			this.CustomPostProcessProjectile(component);
		}
		this.InternalPostProcessProjectile(component);
	}

	// Token: 0x06007629 RID: 30249 RVA: 0x002F0650 File Offset: 0x002EE850
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
		if (this.FixOverlapCollision && proj && proj.specRigidbody)
		{
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(proj.specRigidbody, null, false);
		}
	}

	// Token: 0x0600762A RID: 30250 RVA: 0x002F06CC File Offset: 0x002EE8CC
	private BeamController BeginFiringBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint, Vector2? spawnPointOffset = null)
	{
		Vector2 vector = ((overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value);
		vector = ((spawnPointOffset == null) ? vector : (vector + spawnPointOffset.Value));
		GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, vector, Quaternion.identity, true);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Owner = source;
		BeamController component2 = gameObject.GetComponent<BeamController>();
		component2.Owner = source;
		component2.HitsPlayers = false;
		component2.HitsEnemies = true;
		Vector3 vector2 = BraveMathCollege.DegreesToVector(targetAngle, 1f);
		component2.Direction = vector2;
		component2.Origin = vector;
		this.InternalPostProcessProjectile(component);
		return component2;
	}

	// Token: 0x0600762B RID: 30251 RVA: 0x002F0788 File Offset: 0x002EE988
	private void ContinueFiringBeam(BeamController beam, PlayerController source, Vector2? overrideSpawnPoint, Vector2? spawnPointOffset = null)
	{
		Vector2 vector = ((overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value);
		vector = ((spawnPointOffset == null) ? vector : (vector + spawnPointOffset.Value));
		beam.Origin = vector;
		beam.LateUpdatePosition(vector);
	}

	// Token: 0x0600762C RID: 30252 RVA: 0x002F07E8 File Offset: 0x002EE9E8
	private void CeaseBeam(BeamController beam)
	{
		beam.CeaseAttack();
	}

	// Token: 0x040077ED RID: 30701
	public PlayerItemProjectileInterface ProjectileInterface;

	// Token: 0x040077EE RID: 30702
	public int MinToSpawnPerWave = 10;

	// Token: 0x040077EF RID: 30703
	public int MaxToSpawnPerWave = 10;

	// Token: 0x040077F0 RID: 30704
	public int NumberWaves = 1;

	// Token: 0x040077F1 RID: 30705
	public int NumberSubwaves = 1;

	// Token: 0x040077F2 RID: 30706
	public float TimeBetweenWaves = 1f;

	// Token: 0x040077F3 RID: 30707
	public bool SpiralWaves;

	// Token: 0x040077F4 RID: 30708
	public bool AlignFirstShot;

	// Token: 0x040077F5 RID: 30709
	public float AlignOffset;

	// Token: 0x040077F6 RID: 30710
	public bool SweepBeams;

	// Token: 0x040077F7 RID: 30711
	public float BeamSweepDegrees = 360f;

	// Token: 0x040077F8 RID: 30712
	public bool AimFirstAtNearestEnemy;

	// Token: 0x040077F9 RID: 30713
	public bool FixOverlapCollision;

	// Token: 0x040077FA RID: 30714
	public bool ForceAllowGoop;

	// Token: 0x040077FB RID: 30715
	public Action<Projectile> CustomPostProcessProjectile;
}
