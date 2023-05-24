using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dungeonator;
using UnityEngine;

// Token: 0x020014A6 RID: 5286
public class SilencerInstance : MonoBehaviour
{
	// Token: 0x06007831 RID: 30769 RVA: 0x00300A88 File Offset: 0x002FEC88
	public void TriggerSilencer(Vector2 centerPoint, float expandSpeed, float maxRadius, GameObject silencerVFX, float distIntensity, float distRadius, float pushForce, float pushRadius, float knockbackForce, float knockbackRadius, float additionalTimeAtMaxRadius, PlayerController user, bool breaksWalls = true, bool skipBreakables = false)
	{
		bool flag = true;
		float num = 10f;
		float num2 = 7f;
		float num3 = 1f;
		if (maxRadius < 5f)
		{
			flag = true;
			num = 10f;
			num2 = maxRadius;
		}
		float? num4 = SilencerInstance.s_MaxRadiusLimiter;
		if (num4 != null)
		{
			maxRadius = SilencerInstance.s_MaxRadiusLimiter.Value;
		}
		bool flag2 = false;
		if (user != null)
		{
			for (int i = 0; i < user.passiveItems.Count; i++)
			{
				BlankModificationItem blankModificationItem = user.passiveItems[i] as BlankModificationItem;
				if (blankModificationItem != null)
				{
					if (blankModificationItem.BlankReflectsEnemyBullets)
					{
						flag2 = true;
					}
					if (blankModificationItem.MakeBlankDealDamage)
					{
						flag = true;
						num += blankModificationItem.BlankDamage;
						num2 = Mathf.Max(num2, blankModificationItem.BlankDamageRadius);
					}
					num3 *= blankModificationItem.BlankForceMultiplier;
					this.ProcessBlankModificationItemAdditionalEffects(blankModificationItem, centerPoint, user);
				}
			}
		}
		if (user && user.HasActiveBonusSynergy(CustomSynergyType.ELDER_BLANK_BULLETS, false))
		{
			flag2 = true;
		}
		this.dIntensity = distIntensity;
		this.dRadius = distRadius;
		this.m_camera = GameManager.Instance.MainCameraController.GetComponent<Camera>();
		if (silencerVFX != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(silencerVFX, centerPoint.ToVector3ZUp(centerPoint.y), Quaternion.identity);
			UnityEngine.Object.Destroy(gameObject, 1f);
		}
		Exploder.DoRadialPush(centerPoint.ToVector3ZUp(0f), pushForce, pushRadius);
		Exploder.DoRadialKnockback(centerPoint.ToVector3ZUp(0f), knockbackForce * num3, knockbackRadius);
		if (!skipBreakables)
		{
			Exploder.DoRadialMinorBreakableBreak(centerPoint.ToVector3ZUp(0f), knockbackRadius);
		}
		if (breaksWalls)
		{
			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(centerPoint.ToIntVector2(VectorConversions.Floor));
			for (int j = 0; j < StaticReferenceManager.AllMajorBreakables.Count; j++)
			{
				if (StaticReferenceManager.AllMajorBreakables[j].IsSecretDoor)
				{
					RoomHandler absoluteRoomFromPosition2 = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(StaticReferenceManager.AllMajorBreakables[j].transform.position.IntXY(VectorConversions.Floor));
					if (absoluteRoomFromPosition2 == absoluteRoomFromPosition)
					{
						StaticReferenceManager.AllMajorBreakables[j].ApplyDamage(1E+10f, Vector2.zero, false, true, true);
						StaticReferenceManager.AllMajorBreakables[j].ApplyDamage(1E+10f, Vector2.zero, false, true, true);
						StaticReferenceManager.AllMajorBreakables[j].ApplyDamage(1E+10f, Vector2.zero, false, true, true);
					}
				}
			}
		}
		if (flag && !this.ForceNoDamage)
		{
			Exploder.DoRadialDamage(num, centerPoint.ToVector3ZUp(0f), num2, false, true, false, null);
		}
		if (distIntensity > 0f)
		{
			this.m_distortionMaterial = new Material(ShaderCache.Acquire("Brave/Internal/DistortionWave"));
			Vector4 centerPointInScreenUV = this.GetCenterPointInScreenUV(centerPoint);
			this.m_distortionMaterial.SetVector("_WaveCenter", centerPointInScreenUV);
			Pixelator.Instance.RegisterAdditionalRenderPass(this.m_distortionMaterial);
		}
		if (maxRadius > 10f)
		{
			List<BulletScriptSource> allBulletScriptSources = StaticReferenceManager.AllBulletScriptSources;
			for (int k = 0; k < allBulletScriptSources.Count; k++)
			{
				BulletScriptSource bulletScriptSource = allBulletScriptSources[k];
				if (!bulletScriptSource.IsEnded && bulletScriptSource.RootBullet != null && bulletScriptSource.RootBullet.EndOnBlank)
				{
					bulletScriptSource.ForceStop();
				}
			}
			ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
			for (int l = allProjectiles.Count - 1; l >= 0; l--)
			{
				Projectile projectile = allProjectiles[l];
				if (projectile.braveBulletScript && projectile.braveBulletScript.bullet != null && projectile.braveBulletScript.bullet.EndOnBlank)
				{
					if (this.UsesCustomProjectileCallback && this.OnCustomBlankedProjectile != null)
					{
						this.OnCustomBlankedProjectile(projectile);
					}
					projectile.DieInAir(false, true, true, true);
				}
			}
		}
		Pixelator.Instance.StartCoroutine(this.BackupDistortionCleanup());
		base.StartCoroutine(this.HandleSilence(centerPoint, expandSpeed, maxRadius, additionalTimeAtMaxRadius, user, flag2));
	}

	// Token: 0x06007832 RID: 30770 RVA: 0x00300EC0 File Offset: 0x002FF0C0
	private void ProcessBlankModificationItemAdditionalEffects(BlankModificationItem bmi, Vector2 centerPoint, PlayerController user)
	{
		List<AIActor> activeEnemies = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(centerPoint.ToIntVector2(VectorConversions.Round)).GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (bmi.RegainAmmoFraction > 0f)
		{
			for (int i = 0; i < user.inventory.AllGuns.Count; i++)
			{
				Gun gun = user.inventory.AllGuns[i];
				if (gun.CanGainAmmo)
				{
					gun.GainAmmo(Mathf.CeilToInt((float)gun.AdjustedMaxAmmo * bmi.RegainAmmoFraction));
				}
			}
		}
		if (activeEnemies != null)
		{
			for (int j = 0; j < activeEnemies.Count; j++)
			{
				AIActor aiactor = activeEnemies[j];
				float num = Vector2.Distance(centerPoint, aiactor.CenterPosition);
				if (num <= bmi.BlankDamageRadius)
				{
					if (bmi.BlankStunTime > 0f && aiactor.behaviorSpeculator)
					{
						aiactor.behaviorSpeculator.Stun(bmi.BlankStunTime, true);
					}
					if (bmi.BlankFireChance > 0f && UnityEngine.Random.value < bmi.BlankFireChance)
					{
						Debug.Log("appling fire...");
						aiactor.ApplyEffect(bmi.BlankFireEffect, 1f, null);
					}
					if (bmi.BlankPoisonChance > 0f && UnityEngine.Random.value < bmi.BlankPoisonChance)
					{
						aiactor.ApplyEffect(bmi.BlankPoisonEffect, 1f, null);
					}
					if (bmi.BlankFreezeChance > 0f && UnityEngine.Random.value < bmi.BlankFreezeChance)
					{
						aiactor.ApplyEffect(bmi.BlankFreezeEffect, 1f, null);
					}
				}
			}
		}
	}

	// Token: 0x06007833 RID: 30771 RVA: 0x00301070 File Offset: 0x002FF270
	private Vector4 GetCenterPointInScreenUV(Vector2 centerPoint)
	{
		Vector3 vector = this.m_camera.WorldToViewportPoint(centerPoint.ToVector3ZUp(0f));
		return new Vector4(vector.x, vector.y, this.dRadius, this.dIntensity);
	}

	// Token: 0x06007834 RID: 30772 RVA: 0x003010B4 File Offset: 0x002FF2B4
	private IEnumerator HandleSilence(Vector2 centerPoint, float expandSpeed, float maxRadius, float additionalTimeAtMaxRadius, PlayerController user, bool shouldReflectInstead)
	{
		float currentRadius = 0f;
		float previousRadius = 0f;
		while (currentRadius < maxRadius)
		{
			currentRadius += expandSpeed * BraveTime.DeltaTime;
			previousRadius = Mathf.Max(0f, currentRadius - expandSpeed * 0.05f);
			SilencerInstance.DestroyBulletsInRange(centerPoint, currentRadius, true, GameManager.PVP_ENABLED, user, shouldReflectInstead, new float?(previousRadius), this.UsesCustomProjectileCallback, this.OnCustomBlankedProjectile);
			if (this.m_distortionMaterial != null)
			{
				Vector4 centerPointInScreenUV = this.GetCenterPointInScreenUV(centerPoint);
				this.m_distortionMaterial.SetVector("_WaveCenter", centerPointInScreenUV);
				this.m_distortionMaterial.SetFloat("_DistortProgress", currentRadius / maxRadius);
			}
			yield return null;
		}
		this.CleanupDistortion();
		float elapsed = 0f;
		while (elapsed < additionalTimeAtMaxRadius)
		{
			elapsed += BraveTime.DeltaTime;
			bool flag = true;
			bool pvp_ENABLED = GameManager.PVP_ENABLED;
			float? num = new float?(maxRadius);
			SilencerInstance.DestroyBulletsInRange(centerPoint, maxRadius, flag, pvp_ENABLED, user, false, num, this.UsesCustomProjectileCallback, this.OnCustomBlankedProjectile);
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06007835 RID: 30773 RVA: 0x003010FC File Offset: 0x002FF2FC
	private void CleanupDistortion()
	{
		if (Pixelator.Instance != null && this.m_distortionMaterial != null)
		{
			Pixelator.Instance.DeregisterAdditionalRenderPass(this.m_distortionMaterial);
			UnityEngine.Object.Destroy(this.m_distortionMaterial);
			this.m_distortionMaterial = null;
		}
	}

	// Token: 0x06007836 RID: 30774 RVA: 0x0030114C File Offset: 0x002FF34C
	private void OnDestroy()
	{
		this.CleanupDistortion();
	}

	// Token: 0x06007837 RID: 30775 RVA: 0x00301154 File Offset: 0x002FF354
	private IEnumerator BackupDistortionCleanup()
	{
		yield return new WaitForSeconds(3f);
		this.CleanupDistortion();
		yield break;
	}

	// Token: 0x06007838 RID: 30776 RVA: 0x00301170 File Offset: 0x002FF370
	public static void DestroyBulletsInRange(Vector2 centerPoint, float radius, bool destroysEnemyBullets, bool destroysPlayerBullets, PlayerController user = null, bool reflectsBullets = false, float? previousRadius = null, bool useCallback = false, Action<Projectile> callback = null)
	{
		float num = radius * radius;
		float num2 = ((previousRadius == null) ? 0f : (previousRadius.Value * previousRadius.Value));
		List<Projectile> list = new List<Projectile>();
		ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
		for (int i = 0; i < allProjectiles.Count; i++)
		{
			Projectile projectile = allProjectiles[i];
			if (projectile && projectile.sprite)
			{
				float sqrMagnitude = (projectile.sprite.WorldCenter - centerPoint).sqrMagnitude;
				if (sqrMagnitude <= num)
				{
					if (!projectile.ImmuneToBlanks)
					{
						if (previousRadius == null || !projectile.ImmuneToSustainedBlanks || sqrMagnitude >= num2)
						{
							if (projectile.Owner != null)
							{
								if (projectile.isFakeBullet || projectile.Owner is AIActor || (projectile.Shooter != null && projectile.Shooter.aiActor != null) || projectile.ForcePlayerBlankable)
								{
									if (destroysEnemyBullets)
									{
										list.Add(projectile);
									}
								}
								else if (projectile.Owner is PlayerController)
								{
									if (destroysPlayerBullets && projectile.Owner != user)
									{
										list.Add(projectile);
									}
								}
								else
								{
									Debug.LogError("Silencer is trying to process a bullet that is owned by something that is neither man nor beast!");
								}
							}
							else if (destroysEnemyBullets)
							{
								list.Add(projectile);
							}
						}
					}
				}
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (!destroysPlayerBullets && reflectsBullets)
			{
				PassiveReflectItem.ReflectBullet(list[j], true, user, 10f, 1f, 1f, 0f);
			}
			else
			{
				if (list[j] && list[j].GetComponent<ChainLightningModifier>())
				{
					ChainLightningModifier component = list[j].GetComponent<ChainLightningModifier>();
					UnityEngine.Object.Destroy(component);
				}
				if (useCallback && callback != null)
				{
					callback(list[j]);
				}
				list[j].DieInAir(false, true, true, true);
			}
		}
		List<BasicTrapController> allTriggeredTraps = StaticReferenceManager.AllTriggeredTraps;
		for (int k = allTriggeredTraps.Count - 1; k >= 0; k--)
		{
			BasicTrapController basicTrapController = allTriggeredTraps[k];
			if (basicTrapController && basicTrapController.triggerOnBlank)
			{
				float sqrMagnitude2 = (basicTrapController.CenterPoint() - centerPoint).sqrMagnitude;
				if (sqrMagnitude2 < num)
				{
					basicTrapController.Trigger();
				}
			}
		}
	}

	// Token: 0x04007A5E RID: 31326
	public static float? s_MaxRadiusLimiter;

	// Token: 0x04007A5F RID: 31327
	private Camera m_camera;

	// Token: 0x04007A60 RID: 31328
	private Material m_distortionMaterial;

	// Token: 0x04007A61 RID: 31329
	private float dIntensity;

	// Token: 0x04007A62 RID: 31330
	private float dRadius;

	// Token: 0x04007A63 RID: 31331
	public bool ForceNoDamage;

	// Token: 0x04007A64 RID: 31332
	public bool UsesCustomProjectileCallback;

	// Token: 0x04007A65 RID: 31333
	public Action<Projectile> OnCustomBlankedProjectile;
}
