using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dungeonator;
using UnityEngine;

// Token: 0x020012CA RID: 4810
public class Exploder : MonoBehaviour
{
	// Token: 0x06006B9C RID: 27548 RVA: 0x002A47F8 File Offset: 0x002A29F8
	public static bool IsExplosionOccurring()
	{
		return Exploder.ExplosionIsExtant || ExplosionManager.Instance.QueueCount > 0;
	}

	// Token: 0x06006B9D RID: 27549 RVA: 0x002A4814 File Offset: 0x002A2A14
	public static void Explode(Vector3 position, ExplosionData data, Vector2 sourceNormal, Action onExplosionBegin = null, bool ignoreQueues = false, CoreDamageTypes damageTypes = CoreDamageTypes.None, bool ignoreDamageCaps = false)
	{
		if (data.useDefaultExplosion && data != GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultExplosionData)
		{
			Exploder.DoDefaultExplosion(position, sourceNormal, onExplosionBegin, ignoreQueues, damageTypes, ignoreDamageCaps);
		}
		else
		{
			GameObject gameObject = new GameObject("temp_explosion_processor", new Type[] { typeof(Exploder) });
			gameObject.GetComponent<Exploder>().DoExplode(position, data, sourceNormal, onExplosionBegin, ignoreQueues, damageTypes, ignoreDamageCaps);
		}
	}

	// Token: 0x06006B9E RID: 27550 RVA: 0x002A488C File Offset: 0x002A2A8C
	public static void DoDefaultExplosion(Vector3 position, Vector2 sourceNormal, Action onExplosionBegin = null, bool ignoreQueues = false, CoreDamageTypes damageTypes = CoreDamageTypes.None, bool ignoreDamageCaps = false)
	{
		Exploder.Explode(position, GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultExplosionData, sourceNormal, onExplosionBegin, ignoreQueues, damageTypes, false);
	}

	// Token: 0x06006B9F RID: 27551 RVA: 0x002A48B0 File Offset: 0x002A2AB0
	protected void DoExplode(Vector3 position, ExplosionData data, Vector2 sourceNormal, Action onExplosionBegin = null, bool ignoreQueues = false, CoreDamageTypes damageTypes = CoreDamageTypes.None, bool ignoreDamageCaps = false)
	{
		base.StartCoroutine(this.HandleExplosion(position, data, sourceNormal, onExplosionBegin, ignoreQueues, damageTypes, ignoreDamageCaps));
	}

	// Token: 0x06006BA0 RID: 27552 RVA: 0x002A48D8 File Offset: 0x002A2AD8
	public static void DoRadialMajorBreakableDamage(float damage, Vector3 position, float radius)
	{
		List<MajorBreakable> allMajorBreakables = StaticReferenceManager.AllMajorBreakables;
		float num = radius * radius;
		if (allMajorBreakables != null)
		{
			for (int i = 0; i < allMajorBreakables.Count; i++)
			{
				MajorBreakable majorBreakable = allMajorBreakables[i];
				if (majorBreakable)
				{
					if (majorBreakable.enabled)
					{
						if (!majorBreakable.IgnoreExplosions)
						{
							Vector2 vector = majorBreakable.CenterPoint - position.XY();
							if (vector.sqrMagnitude < num)
							{
								majorBreakable.ApplyDamage(damage, vector, false, true, false);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06006BA1 RID: 27553 RVA: 0x002A4970 File Offset: 0x002A2B70
	public static void DoRadialIgnite(GameActorFireEffect fire, Vector3 position, float radius, VFXPool hitVFX = null)
	{
		List<HealthHaver> allHealthHavers = StaticReferenceManager.AllHealthHavers;
		if (allHealthHavers != null)
		{
			float num = radius * radius;
			for (int i = 0; i < allHealthHavers.Count; i++)
			{
				HealthHaver healthHaver = allHealthHavers[i];
				if (healthHaver)
				{
					if (healthHaver.gameObject.activeSelf)
					{
						if (healthHaver.aiActor)
						{
							AIActor aiActor = healthHaver.aiActor;
							if (!aiActor.IsGone)
							{
								if (aiActor.isActiveAndEnabled)
								{
									if ((aiActor.CenterPosition - position.XY()).sqrMagnitude <= num)
									{
										aiActor.ApplyEffect(fire, 1f, null);
										if (hitVFX != null)
										{
											if (aiActor.specRigidbody.HitboxPixelCollider != null)
											{
												PixelCollider pixelCollider = aiActor.specRigidbody.GetPixelCollider(ColliderType.HitBox);
												Vector2 vector = BraveMathCollege.ClosestPointOnRectangle(position, pixelCollider.UnitBottomLeft, pixelCollider.UnitDimensions);
												hitVFX.SpawnAtPosition(vector, 0f, null, null, null, null, false, null, null, false);
											}
											else
											{
												hitVFX.SpawnAtPosition(aiActor.CenterPosition, 0f, null, null, null, null, false, null, null, false);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06006BA2 RID: 27554 RVA: 0x002A4AFC File Offset: 0x002A2CFC
	public static void DoRadialDamage(float damage, Vector3 position, float radius, bool damagePlayers, bool damageEnemies, bool ignoreDamageCaps = false, VFXPool hitVFX = null)
	{
		List<HealthHaver> allHealthHavers = StaticReferenceManager.AllHealthHavers;
		if (allHealthHavers != null)
		{
			for (int i = 0; i < allHealthHavers.Count; i++)
			{
				HealthHaver healthHaver = allHealthHavers[i];
				if (healthHaver)
				{
					if (healthHaver.gameObject.activeSelf)
					{
						if (!healthHaver.aiActor || !healthHaver.aiActor.IsGone)
						{
							if (!healthHaver.aiActor || healthHaver.aiActor.isActiveAndEnabled)
							{
								for (int j = 0; j < healthHaver.NumBodyRigidbodies; j++)
								{
									SpeculativeRigidbody bodyRigidbody = healthHaver.GetBodyRigidbody(j);
									Vector2 vector = healthHaver.transform.position.XY();
									Vector2 vector2 = vector - position.XY();
									bool flag = false;
									bool flag2 = false;
									float num;
									if (bodyRigidbody.HitboxPixelCollider != null)
									{
										vector = bodyRigidbody.HitboxPixelCollider.UnitCenter;
										vector2 = vector - position.XY();
										num = BraveMathCollege.DistToRectangle(position.XY(), bodyRigidbody.HitboxPixelCollider.UnitBottomLeft, bodyRigidbody.HitboxPixelCollider.UnitDimensions);
									}
									else
									{
										vector = healthHaver.transform.position.XY();
										vector2 = vector - position.XY();
										num = vector2.magnitude;
									}
									if (num < radius)
									{
										PlayerController component = healthHaver.GetComponent<PlayerController>();
										if (component != null)
										{
											bool flag3 = true;
											if (PassiveItem.ActiveFlagItems.ContainsKey(component) && PassiveItem.ActiveFlagItems[component].ContainsKey(typeof(HelmetItem)) && num > radius * HelmetItem.EXPLOSION_RADIUS_MULTIPLIER)
											{
												flag3 = false;
											}
											if (Exploder.IsPlayerBlockedByWall(component, position))
											{
												flag3 = false;
											}
											if (damagePlayers && flag3 && !component.IsEthereal)
											{
												HealthHaver healthHaver2 = healthHaver;
												float num2 = 0.5f;
												Vector2 vector3 = vector2;
												string text = StringTableManager.GetEnemiesString("#EXPLOSION", -1);
												CoreDamageTypes coreDamageTypes = CoreDamageTypes.None;
												DamageCategory damageCategory = DamageCategory.Normal;
												healthHaver2.ApplyDamage(num2, vector3, text, coreDamageTypes, damageCategory, false, null, ignoreDamageCaps);
												flag2 = true;
											}
										}
										else if (damageEnemies)
										{
											AIActor aiActor = healthHaver.aiActor;
											if (damagePlayers || !aiActor || aiActor.IsNormalEnemy)
											{
												HealthHaver healthHaver3 = healthHaver;
												Vector2 vector3 = vector2;
												string text = StringTableManager.GetEnemiesString("#EXPLOSION", -1);
												CoreDamageTypes coreDamageTypes = CoreDamageTypes.None;
												DamageCategory damageCategory = DamageCategory.Normal;
												healthHaver3.ApplyDamage(damage, vector3, text, coreDamageTypes, damageCategory, false, null, ignoreDamageCaps);
												flag2 = true;
											}
										}
										flag = true;
									}
									if (flag2 && hitVFX != null)
									{
										if (bodyRigidbody.HitboxPixelCollider != null)
										{
											PixelCollider pixelCollider = bodyRigidbody.GetPixelCollider(ColliderType.HitBox);
											Vector2 vector4 = BraveMathCollege.ClosestPointOnRectangle(position, pixelCollider.UnitBottomLeft, pixelCollider.UnitDimensions);
											hitVFX.SpawnAtPosition(vector4, 0f, null, null, null, null, false, null, null, false);
										}
										else
										{
											hitVFX.SpawnAtPosition(healthHaver.transform.position.XY(), 0f, null, null, null, null, false, null, null, false);
										}
									}
									if (flag)
									{
										break;
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06006BA3 RID: 27555 RVA: 0x002A4E6C File Offset: 0x002A306C
	private static bool IsPlayerBlockedByWall(PlayerController attachedPlayer, Vector2 explosionPos)
	{
		Vector2 vector = attachedPlayer.CenterPosition;
		RaycastResult raycastResult;
		bool flag = PhysicsEngine.Instance.Raycast(explosionPos, vector - explosionPos, Vector2.Distance(vector, explosionPos), out raycastResult, true, false, int.MaxValue, null, false, null, null);
		RaycastResult.Pool.Free(ref raycastResult);
		if (!flag)
		{
			return false;
		}
		vector = attachedPlayer.specRigidbody.HitboxPixelCollider.UnitTopCenter;
		flag = PhysicsEngine.Instance.Raycast(explosionPos, vector - explosionPos, Vector2.Distance(vector, explosionPos), out raycastResult, true, false, int.MaxValue, null, false, null, null);
		RaycastResult.Pool.Free(ref raycastResult);
		if (!flag)
		{
			return false;
		}
		vector = attachedPlayer.specRigidbody.PrimaryPixelCollider.UnitBottomCenter;
		flag = PhysicsEngine.Instance.Raycast(explosionPos, vector - explosionPos, Vector2.Distance(vector, explosionPos), out raycastResult, true, false, int.MaxValue, null, false, null, null);
		RaycastResult.Pool.Free(ref raycastResult);
		return flag;
	}

	// Token: 0x06006BA4 RID: 27556 RVA: 0x002A4F6C File Offset: 0x002A316C
	public static void DoRadialMinorBreakableBreak(Vector3 position, float radius)
	{
		float num = radius * radius;
		List<MinorBreakable> allMinorBreakables = StaticReferenceManager.AllMinorBreakables;
		if (allMinorBreakables != null)
		{
			for (int i = 0; i < allMinorBreakables.Count; i++)
			{
				MinorBreakable minorBreakable = allMinorBreakables[i];
				if (minorBreakable)
				{
					if (!minorBreakable.resistsExplosions)
					{
						if (!minorBreakable.OnlyBrokenByCode)
						{
							Vector2 vector = minorBreakable.CenterPoint - position.XY();
							if (vector.sqrMagnitude < num)
							{
								minorBreakable.Break(vector.normalized);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06006BA5 RID: 27557 RVA: 0x002A5008 File Offset: 0x002A3208
	public static void DoRadialPush(Vector3 position, float force, float radius)
	{
		float num = radius * radius;
		for (int i = 0; i < StaticReferenceManager.AllDebris.Count; i++)
		{
			Vector2 vector = StaticReferenceManager.AllDebris[i].transform.position.XY();
			Vector2 vector2 = vector - position.XY();
			float sqrMagnitude = vector2.sqrMagnitude;
			if (sqrMagnitude < num)
			{
				float num2 = 1f - vector2.magnitude / radius;
				StaticReferenceManager.AllDebris[i].ApplyVelocity(vector2.normalized * num2 * force * (1f + UnityEngine.Random.value / 5f));
			}
		}
	}

	// Token: 0x06006BA6 RID: 27558 RVA: 0x002A50B8 File Offset: 0x002A32B8
	public static void DoRadialKnockback(Vector3 position, float force, float radius)
	{
		List<AIActor> allEnemies = StaticReferenceManager.AllEnemies;
		if (allEnemies != null)
		{
			for (int i = 0; i < allEnemies.Count; i++)
			{
				Vector2 centerPosition = allEnemies[i].CenterPosition;
				Vector2 vector = centerPosition - position.XY();
				float magnitude = vector.magnitude;
				if (magnitude < radius)
				{
					KnockbackDoer knockbackDoer = allEnemies[i].knockbackDoer;
					if (knockbackDoer)
					{
						float num = 1f - magnitude / radius;
						knockbackDoer.ApplyKnockback(vector.normalized, num * force, false);
					}
				}
			}
		}
	}

	// Token: 0x06006BA7 RID: 27559 RVA: 0x002A5150 File Offset: 0x002A3350
	public static void DoDistortionWave(Vector2 center, float distortionIntensity, float distortionRadius, float maxRadius, float duration)
	{
		Exploder component = new GameObject("temp_explosion_processor", new Type[] { typeof(Exploder) }).GetComponent<Exploder>();
		component.StartCoroutine(component.DoDistortionWaveLocal(center, distortionIntensity, distortionRadius, maxRadius, duration));
	}

	// Token: 0x06006BA8 RID: 27560 RVA: 0x002A5194 File Offset: 0x002A3394
	private Vector4 GetCenterPointInScreenUV(Vector2 centerPoint, float dIntensity, float dRadius)
	{
		Vector3 vector = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(centerPoint.ToVector3ZUp(0f));
		return new Vector4(vector.x, vector.y, dRadius, dIntensity);
	}

	// Token: 0x06006BA9 RID: 27561 RVA: 0x002A51D8 File Offset: 0x002A33D8
	private IEnumerator DoDistortionWaveLocal(Vector2 center, float distortionIntensity, float distortionRadius, float maxRadius, float duration)
	{
		Material distMaterial = new Material(ShaderCache.Acquire("Brave/Internal/DistortionWave"));
		Vector4 distortionSettings = this.GetCenterPointInScreenUV(center, distortionIntensity, distortionRadius);
		distMaterial.SetVector("_WaveCenter", distortionSettings);
		Pixelator.Instance.RegisterAdditionalRenderPass(distMaterial);
		float elapsed = 0f;
		while (elapsed < duration)
		{
			if (BraveUtility.isLoadingLevel && GameManager.Instance.IsLoadingLevel)
			{
				break;
			}
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			t = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 1f, t);
			distortionSettings = this.GetCenterPointInScreenUV(center, distortionIntensity, distortionRadius);
			distortionSettings.w = Mathf.Lerp(distortionSettings.w, 0f, t);
			distMaterial.SetVector("_WaveCenter", distortionSettings);
			float currentRadius = Mathf.Lerp(0f, maxRadius, t);
			distMaterial.SetFloat("_DistortProgress", currentRadius / maxRadius * (maxRadius / 33.75f));
			yield return null;
		}
		Pixelator.Instance.DeregisterAdditionalRenderPass(distMaterial);
		UnityEngine.Object.Destroy(distMaterial);
		yield break;
	}

	// Token: 0x06006BAA RID: 27562 RVA: 0x002A5218 File Offset: 0x002A3418
	public static void DoLinearPush(Vector2 p1, Vector2 p2, float force, float radius)
	{
		float num = radius * radius;
		for (int i = 0; i < StaticReferenceManager.AllDebris.Count; i++)
		{
			Vector2 vector = StaticReferenceManager.AllDebris[i].transform.position.XY();
			float num2 = vector.x - p1.x;
			float num3 = vector.y - p1.y;
			float num4 = p2.x - p1.x;
			float num5 = p2.y - p1.y;
			float num6 = num2 * num4 + num3 * num5;
			float num7 = num4 * num4 + num5 * num5;
			float num8 = -1f;
			if (num7 != 0f)
			{
				num8 = num6 / num7;
			}
			float num9;
			float num10;
			if (num8 < 0f)
			{
				num9 = p1.x;
				num10 = p1.y;
			}
			else if (num8 > 1f)
			{
				num9 = p2.x;
				num10 = p2.y;
			}
			else
			{
				num9 = p1.x + num8 * num4;
				num10 = p1.y + num8 * num5;
			}
			float num11 = vector.x - num9;
			float num12 = vector.y - num10;
			Vector2 vector2 = new Vector2(num11, num12);
			float sqrMagnitude = vector2.sqrMagnitude;
			if (sqrMagnitude < num)
			{
				float num13 = 1f - vector2.magnitude / radius;
				StaticReferenceManager.AllDebris[i].ApplyVelocity(vector2.normalized * num13 * force * (1f + UnityEngine.Random.value / 5f));
			}
		}
	}

	// Token: 0x06006BAB RID: 27563 RVA: 0x002A53B8 File Offset: 0x002A35B8
	private IEnumerator HandleCurrentExplosionNotification(float t)
	{
		float elapsed = 0f;
		while (elapsed < t)
		{
			elapsed += BraveTime.DeltaTime;
			Exploder.ExplosionIsExtant = true;
			yield return null;
		}
		Exploder.ExplosionIsExtant = false;
		yield break;
	}

	// Token: 0x06006BAC RID: 27564 RVA: 0x002A53D4 File Offset: 0x002A35D4
	private IEnumerator HandleBulletDeletionFrames(Vector3 centerPosition, float bulletDeletionSqrRadius, float duration)
	{
		float elapsed = 0f;
		if (GameManager.HasInstance && GameManager.Instance.Dungeon)
		{
			Dungeon dungeon = GameManager.Instance.Dungeon;
			bulletDeletionSqrRadius *= Mathf.InverseLerp(0.66f, 1f, dungeon.ExplosionBulletDeletionMultiplier);
			if (!dungeon.IsExplosionBulletDeletionRecovering)
			{
				dungeon.ExplosionBulletDeletionMultiplier = Mathf.Clamp01(dungeon.ExplosionBulletDeletionMultiplier - 0.8f);
			}
			if (bulletDeletionSqrRadius <= 0f)
			{
				yield break;
			}
		}
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
			for (int i = allProjectiles.Count - 1; i >= 0; i--)
			{
				Projectile projectile = allProjectiles[i];
				if (projectile)
				{
					if (!(projectile.Owner is PlayerController))
					{
						Vector2 vector = (projectile.transform.position - centerPosition).XY();
						if (projectile.CanBeKilledByExplosions && vector.sqrMagnitude < bulletDeletionSqrRadius)
						{
							projectile.DieInAir(false, true, true, false);
						}
					}
				}
			}
			List<BasicTrapController> allTraps = StaticReferenceManager.AllTriggeredTraps;
			for (int j = allTraps.Count - 1; j >= 0; j--)
			{
				BasicTrapController basicTrapController = allTraps[j];
				if (basicTrapController && basicTrapController.triggerOnBlank)
				{
					float sqrMagnitude = (basicTrapController.CenterPoint() - centerPosition.XY()).sqrMagnitude;
					if (sqrMagnitude < bulletDeletionSqrRadius)
					{
						basicTrapController.Trigger();
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006BAD RID: 27565 RVA: 0x002A5400 File Offset: 0x002A3600
	private IEnumerator HandleCirc(tk2dSprite AdditiveCircSprite, float targetScale, float duration)
	{
		AdditiveCircSprite.transform.parent = null;
		AdditiveCircSprite.color = Color.white;
		AdditiveCircSprite.transform.localScale = targetScale * Vector3.one * 0.5f;
		yield return null;
		AdditiveCircSprite.transform.localScale = targetScale * Vector3.one;
		yield return null;
		float ela = 0f;
		while (ela < duration)
		{
			ela += BraveTime.DeltaTime;
			float t = ela / duration;
			AdditiveCircSprite.color = Color.Lerp(new Color(1f, 1f, 1f, 1f), new Color(1f, 1f, 1f, 0f), t);
			yield return null;
		}
		UnityEngine.Object.Destroy(AdditiveCircSprite.gameObject);
		yield break;
	}

	// Token: 0x06006BAE RID: 27566 RVA: 0x002A542C File Offset: 0x002A362C
	private IEnumerator HandleExplosion(Vector3 position, ExplosionData data, Vector2 sourceNormal, Action onExplosionBegin, bool ignoreQueues, CoreDamageTypes damageTypes, bool ignoreDamageCaps)
	{
		if (data.usesComprehensiveDelay)
		{
			yield return new WaitForSeconds(data.comprehensiveDelay);
		}
		if (Exploder.OnExplosionTriggered != null)
		{
			Exploder.OnExplosionTriggered();
		}
		bool addFireGoop = (damageTypes | CoreDamageTypes.Fire) == damageTypes;
		bool addFreezeGoop = (damageTypes | CoreDamageTypes.Ice) == damageTypes;
		bool addPoisonGoop = (damageTypes | CoreDamageTypes.Poison) == damageTypes;
		if (!GameManager.HasInstance || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			addFireGoop = false;
			addFreezeGoop = false;
			addPoisonGoop = false;
		}
		bool isFreezeExplosion = data.isFreezeExplosion;
		if (!data.isFreezeExplosion && addFreezeGoop)
		{
			isFreezeExplosion = true;
			data.freezeRadius = data.damageRadius;
			data.freezeEffect = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultFreezeExplosionEffect;
		}
		if (!ignoreQueues)
		{
			ExplosionManager.Instance.Queue(this);
			while (!ExplosionManager.Instance.IsExploderReady(this))
			{
				yield return null;
			}
			ExplosionManager.Instance.Dequeue();
			if (ExplosionManager.Instance.QueueCount == 0)
			{
				ExplosionManager.Instance.StartCoroutine(this.HandleCurrentExplosionNotification(0.5f));
			}
		}
		if (onExplosionBegin != null)
		{
			onExplosionBegin();
		}
		float damageRadius = data.GetDefinedDamageRadius();
		float pushSqrRadius = data.pushRadius * data.pushRadius;
		float bulletDeletionSqrRadius = damageRadius * damageRadius;
		if (addFreezeGoop)
		{
			DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultFreezeGoop).TimedAddGoopCircle(position.XY(), damageRadius, 0.5f, false);
			DeadlyDeadlyGoopManager.FreezeGoopsCircle(position.XY(), damageRadius);
		}
		if (addFireGoop)
		{
			DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultFireGoop).TimedAddGoopCircle(position.XY(), damageRadius, 0.5f, false);
		}
		if (addPoisonGoop)
		{
			DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPoisonGoop).TimedAddGoopCircle(position.XY(), damageRadius, 0.5f, false);
		}
		if (!isFreezeExplosion)
		{
			DeadlyDeadlyGoopManager.IgniteGoopsCircle(position.XY(), damageRadius);
		}
		if (data.effect)
		{
			GameObject gameObject;
			if (data.effect.GetComponent<ParticleSystem>() != null || data.effect.GetComponentInChildren<ParticleSystem>() != null)
			{
				gameObject = SpawnManager.SpawnVFX(data.effect, position, Quaternion.identity);
			}
			else
			{
				gameObject = SpawnManager.SpawnVFX(data.effect, position, Quaternion.identity);
			}
			if (data.rotateEffectToNormal && gameObject)
			{
				gameObject.transform.rotation = Quaternion.Euler(0f, 0f, sourceNormal.ToAngle());
			}
			tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
			if (component)
			{
				component.HeightOffGround += UnityEngine.Random.Range(-0.1f, 0.2f);
				component.UpdateZDepth();
			}
			ExplosionDebrisLauncher[] componentsInChildren = gameObject.GetComponentsInChildren<ExplosionDebrisLauncher>();
			Vector3 vector = gameObject.transform.position.WithZ(gameObject.transform.position.y);
			GameObject gameObject2 = new GameObject("SoundSource");
			gameObject2.transform.position = vector;
			if (data.playDefaultSFX)
			{
				AkSoundEngine.PostEvent("Play_WPN_grenade_blast_01", gameObject2);
			}
			UnityEngine.Object.Destroy(gameObject2, 5f);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i])
				{
					if (sourceNormal == Vector2.zero)
					{
						componentsInChildren[i].Launch();
					}
					else
					{
						componentsInChildren[i].Launch(sourceNormal);
					}
				}
			}
			if (gameObject)
			{
				Transform transform = gameObject.transform.Find("scorch");
				if (transform)
				{
					transform.gameObject.SetLayerRecursively(LayerMask.NameToLayer("BG_Critical"));
				}
			}
			if (data.doExplosionRing)
			{
			}
		}
		yield return new WaitForSeconds(data.explosionDelay);
		List<HealthHaver> allHealth = StaticReferenceManager.AllHealthHavers;
		if (allHealth != null && (data.doDamage || data.doForce))
		{
			for (int j = 0; j < allHealth.Count; j++)
			{
				HealthHaver healthHaver = allHealth[j];
				if (healthHaver)
				{
					if (healthHaver && healthHaver.aiActor)
					{
						if (!healthHaver.aiActor.HasBeenEngaged)
						{
							goto IL_AB1;
						}
						if (healthHaver.aiActor.CompanionOwner && data.damageToPlayer == 0f)
						{
							goto IL_AB1;
						}
					}
					if (!data.ignoreList.Contains(healthHaver.specRigidbody))
					{
						if (position.GetAbsoluteRoom() == allHealth[j].transform.position.GetAbsoluteRoom())
						{
							for (int k = 0; k < healthHaver.NumBodyRigidbodies; k++)
							{
								SpeculativeRigidbody bodyRigidbody = healthHaver.GetBodyRigidbody(k);
								PlayerController playerController = ((!bodyRigidbody) ? null : (bodyRigidbody.gameActor as PlayerController));
								Vector2 vector2 = healthHaver.transform.position.XY();
								Vector2 vector3 = vector2 - position.XY();
								bool flag = false;
								float num;
								if (bodyRigidbody.HitboxPixelCollider != null)
								{
									vector2 = bodyRigidbody.HitboxPixelCollider.UnitCenter;
									vector3 = vector2 - position.XY();
									num = BraveMathCollege.DistToRectangle(position.XY(), bodyRigidbody.HitboxPixelCollider.UnitBottomLeft, bodyRigidbody.HitboxPixelCollider.UnitDimensions);
								}
								else
								{
									vector2 = healthHaver.transform.position.XY();
									vector3 = vector2 - position.XY();
									num = vector3.magnitude;
								}
								if (!playerController || ((!data.doDamage || num >= damageRadius) && (!isFreezeExplosion || num >= data.freezeRadius) && (!data.doForce || num >= data.pushRadius)) || !Exploder.IsPlayerBlockedByWall(playerController, position))
								{
									if (playerController)
									{
										if (!bodyRigidbody.CollideWithOthers)
										{
											goto IL_A9D;
										}
										if (playerController.DodgeRollIsBlink && playerController.IsDodgeRolling)
										{
											goto IL_A9D;
										}
									}
									if (data.doDamage && num < damageRadius)
									{
										if (playerController)
										{
											bool flag2 = true;
											if (PassiveItem.ActiveFlagItems.ContainsKey(playerController) && PassiveItem.ActiveFlagItems[playerController].ContainsKey(typeof(HelmetItem)) && num > damageRadius * HelmetItem.EXPLOSION_RADIUS_MULTIPLIER)
											{
												flag2 = false;
											}
											if (flag2 && !playerController.IsEthereal)
											{
												HealthHaver healthHaver2 = healthHaver;
												float num2 = data.damageToPlayer;
												Vector2 vector4 = vector3;
												string text = StringTableManager.GetEnemiesString("#EXPLOSION", -1);
												CoreDamageTypes coreDamageTypes = CoreDamageTypes.None;
												DamageCategory damageCategory = DamageCategory.Normal;
												healthHaver2.ApplyDamage(num2, vector4, text, coreDamageTypes, damageCategory, false, null, ignoreDamageCaps);
											}
										}
										else
										{
											HealthHaver healthHaver3 = healthHaver;
											float num2 = data.damage;
											Vector2 vector4 = vector3;
											string text = StringTableManager.GetEnemiesString("#EXPLOSION", -1);
											CoreDamageTypes coreDamageTypes = CoreDamageTypes.None;
											DamageCategory damageCategory = DamageCategory.Normal;
											healthHaver3.ApplyDamage(num2, vector4, text, coreDamageTypes, damageCategory, false, null, ignoreDamageCaps);
											if (data.IsChandelierExplosion && (!healthHaver || healthHaver.healthHaver.IsDead))
											{
												GameStatsManager.Instance.RegisterStatChange(TrackedStats.ENEMIES_KILLED_WITH_CHANDELIERS, 1f);
											}
										}
										flag = true;
									}
									if (isFreezeExplosion && num < data.freezeRadius)
									{
										if (healthHaver && healthHaver.gameActor != null && !healthHaver.IsDead && (!healthHaver.aiActor || !healthHaver.aiActor.IsGone))
										{
											healthHaver.gameActor.ApplyEffect(data.freezeEffect, 1f, null);
										}
										flag = true;
									}
									if (data.doForce && num < data.pushRadius)
									{
										KnockbackDoer knockbackDoer = healthHaver.knockbackDoer;
										if (knockbackDoer)
										{
											float num3 = 1f - num / data.pushRadius;
											if (data.preventPlayerForce && healthHaver.GetComponent<PlayerController>())
											{
												num3 = 0f;
											}
											knockbackDoer.ApplyKnockback(vector3.normalized, num3 * data.force, false);
										}
										flag = true;
									}
									if (flag)
									{
										break;
									}
								}
								IL_A9D:;
							}
						}
					}
				}
				IL_AB1:;
			}
		}
		List<MinorBreakable> allBreakables = StaticReferenceManager.AllMinorBreakables;
		if (allBreakables != null)
		{
			for (int l = 0; l < allBreakables.Count; l++)
			{
				MinorBreakable minorBreakable = allBreakables[l];
				if (minorBreakable)
				{
					if (!minorBreakable.resistsExplosions)
					{
						if (!minorBreakable.OnlyBrokenByCode)
						{
							Vector2 vector5 = minorBreakable.CenterPoint - position.XY();
							if (vector5.sqrMagnitude < pushSqrRadius)
							{
								minorBreakable.Break(vector5.normalized);
							}
						}
					}
				}
			}
		}
		if (data.doDestroyProjectiles)
		{
			float num4 = 0.2f;
			PlayerController bestActivePlayer = GameManager.Instance.BestActivePlayer;
			if (bestActivePlayer && bestActivePlayer.CurrentRoom != null && bestActivePlayer.CurrentRoom.area != null && bestActivePlayer.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS)
			{
				num4 = 0.035f;
			}
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleBulletDeletionFrames(position, bulletDeletionSqrRadius, num4));
		}
		if (data.doDamage || data.breakSecretWalls)
		{
			List<MajorBreakable> allMajorBreakables = StaticReferenceManager.AllMajorBreakables;
			if (allMajorBreakables != null)
			{
				for (int m = 0; m < allMajorBreakables.Count; m++)
				{
					MajorBreakable majorBreakable = allMajorBreakables[m];
					if (majorBreakable)
					{
						if (majorBreakable.enabled)
						{
							if (!majorBreakable.IgnoreExplosions)
							{
								Vector2 vector6 = majorBreakable.CenterPoint - position.XY();
								if (vector6.sqrMagnitude < pushSqrRadius && (!majorBreakable.IsSecretDoor || !data.forcePreventSecretWallDamage))
								{
									if (data.doDamage)
									{
										majorBreakable.ApplyDamage(data.damage, vector6, false, true, false);
									}
									if (data.breakSecretWalls && majorBreakable.IsSecretDoor)
									{
										StaticReferenceManager.AllMajorBreakables[m].ApplyDamage(1E+10f, Vector2.zero, false, true, true);
										StaticReferenceManager.AllMajorBreakables[m].ApplyDamage(1E+10f, Vector2.zero, false, true, true);
										StaticReferenceManager.AllMajorBreakables[m].ApplyDamage(1E+10f, Vector2.zero, false, true, true);
									}
								}
							}
						}
					}
				}
			}
		}
		if (data.doForce)
		{
			Exploder.DoRadialPush(position, data.debrisForce, data.pushRadius);
		}
		if (data.doScreenShake && GameManager.Instance.MainCameraController != null)
		{
			GameManager.Instance.MainCameraController.DoScreenShake(data.ss, new Vector2?(position), false);
		}
		if (data.doStickyFriction && GameManager.Instance.MainCameraController != null)
		{
			StickyFrictionManager.Instance.RegisterExplosionStickyFriction();
		}
		for (int n = 0; n < StaticReferenceManager.AllRatTrapdoors.Count; n++)
		{
			if (StaticReferenceManager.AllRatTrapdoors[n])
			{
				StaticReferenceManager.AllRatTrapdoors[n].OnNearbyExplosion(position);
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400688D RID: 26765
	public static Action OnExplosionTriggered;

	// Token: 0x0400688E RID: 26766
	private static bool ExplosionIsExtant;
}
