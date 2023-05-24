using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200164F RID: 5711
public class InstantDamageOneEnemyProjectile : Projectile
{
	// Token: 0x06008555 RID: 34133 RVA: 0x003704C0 File Offset: 0x0036E6C0
	protected override void Move()
	{
		if (this.DoesWhiteFlash)
		{
			Pixelator.Instance.FadeToColor(0.1f, Color.white.WithAlpha(0.25f), true, 0.1f);
		}
		if (this.DoesCameraFlash)
		{
			StickyFrictionManager.Instance.RegisterCustomStickyFriction(0.125f, 0f, false, false);
			Pixelator.Instance.TimedFreezeFrame(0.25f, 0.125f);
		}
		if (this.DoesAmbientVFX && this.AmbientVFXTime > 0f && this.AmbientVFX != null)
		{
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleAmbientSpawnTime(base.transform.position, this.AmbientVFXTime));
		}
		if (this.DoesStickyFriction)
		{
			StickyFrictionManager.Instance.RegisterCustomStickyFriction(this.StickyFrictionDuration, 0f, true, false);
		}
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		if (absoluteRoom != null)
		{
			List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null)
			{
				AIActor aiactor = null;
				float num = float.MaxValue;
				Vector2 vector = base.Owner.CenterPosition;
				if (base.Owner is PlayerController)
				{
					vector = (base.Owner as PlayerController).unadjustedAimPoint.XY();
				}
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					if (activeEnemies[i] && activeEnemies[i].IsNormalEnemy && activeEnemies[i].healthHaver && activeEnemies[i].isActiveAndEnabled)
					{
						float num2 = Vector2.Distance(activeEnemies[i].CenterPosition, vector);
						if (num2 < num)
						{
							num = num2;
							aiactor = activeEnemies[i];
						}
					}
				}
				if (aiactor)
				{
					this.ProcessEnemy(aiactor, 0f);
				}
			}
		}
		base.DieInAir(false, true, true, false);
	}

	// Token: 0x06008556 RID: 34134 RVA: 0x003706C0 File Offset: 0x0036E8C0
	protected void HandleAmbientVFXSpawn(Vector2 centerPoint, float radius)
	{
		if (this.AmbientVFX == null)
		{
			return;
		}
		bool flag = false;
		this.m_ambientTimer -= BraveTime.DeltaTime;
		if (this.m_ambientTimer <= 0f)
		{
			flag = true;
			this.m_ambientTimer = this.minTimeBetweenAmbientVFX;
		}
		if (flag)
		{
			Vector2 vector = centerPoint + UnityEngine.Random.insideUnitCircle * radius;
			SpawnManager.SpawnVFX(this.AmbientVFX, vector, Quaternion.identity);
		}
	}

	// Token: 0x06008557 RID: 34135 RVA: 0x00370740 File Offset: 0x0036E940
	protected IEnumerator HandleAmbientSpawnTime(Vector2 centerPoint, float remainingTime)
	{
		float elapsed = 0f;
		while (elapsed < remainingTime)
		{
			elapsed += BraveTime.DeltaTime;
			this.HandleAmbientVFXSpawn(centerPoint, 10f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008558 RID: 34136 RVA: 0x0037076C File Offset: 0x0036E96C
	public void ProcessEnemy(AIActor a, float b)
	{
		if (a && a.IsNormalEnemy && a.healthHaver)
		{
			if (base.Owner)
			{
				a.healthHaver.ApplyDamage(base.ModifiedDamage, Vector2.zero, base.OwnerName, this.damageTypes, DamageCategory.Normal, false, null, false);
				base.LastVelocity = (a.CenterPosition - base.Owner.CenterPosition).normalized;
				base.HandleKnockback(a.specRigidbody, base.Owner as PlayerController, true, false);
			}
			else
			{
				a.healthHaver.ApplyDamage(base.ModifiedDamage, Vector2.zero, "projectile", this.damageTypes, DamageCategory.Normal, false, null, false);
			}
			if (this.DamagedEnemyVFX != null)
			{
				a.PlayEffectOnActor(this.DamagedEnemyVFX, Vector3.zero, false, true, false);
			}
		}
	}

	// Token: 0x04008982 RID: 35202
	public bool DoesWhiteFlash;

	// Token: 0x04008983 RID: 35203
	public bool DoesCameraFlash;

	// Token: 0x04008984 RID: 35204
	public bool DoesStickyFriction;

	// Token: 0x04008985 RID: 35205
	public float StickyFrictionDuration = 0.6f;

	// Token: 0x04008986 RID: 35206
	public bool DoesAmbientVFX;

	// Token: 0x04008987 RID: 35207
	public float AmbientVFXTime;

	// Token: 0x04008988 RID: 35208
	public GameObject AmbientVFX;

	// Token: 0x04008989 RID: 35209
	public float minTimeBetweenAmbientVFX = 0.1f;

	// Token: 0x0400898A RID: 35210
	public GameObject DamagedEnemyVFX;

	// Token: 0x0400898B RID: 35211
	private float m_ambientTimer;
}
