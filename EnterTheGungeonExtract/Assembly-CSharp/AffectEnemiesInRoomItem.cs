using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200133F RID: 4927
public abstract class AffectEnemiesInRoomItem : PlayerItem
{
	// Token: 0x06006FBC RID: 28604 RVA: 0x002C4C74 File Offset: 0x002C2E74
	protected override void DoEffect(PlayerController user)
	{
		List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (this.OnUserEffectVFX != null)
		{
			SpawnManager.SpawnVFX(this.OnUserEffectVFX, user.CenterPosition, Quaternion.identity, false);
		}
		if (this.FlashScreen)
		{
			Pixelator.Instance.FadeToColor(0.1f, Color.white, true, 0.1f);
		}
		if (activeEnemies != null)
		{
			if (this.EffectTime <= 0f)
			{
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					AIActor aiactor = activeEnemies[i];
					if (this.AffectsBosses || !aiactor.healthHaver.IsBoss)
					{
						this.AffectEnemy(aiactor);
						if (this.OnTargetEffectVFX != null)
						{
							SpawnManager.SpawnVFX(this.OnTargetEffectVFX, aiactor.CenterPosition, Quaternion.identity, false);
						}
					}
				}
				if (this.AmbientVFXTime > 0f && this.AmbientVFX != null)
				{
					user.StartCoroutine(this.HandleAmbientSpawnTime(user.CenterPosition, this.AmbientVFXTime, 10f));
				}
			}
			else
			{
				user.StartCoroutine(this.ProcessEffectOverTime(user.CenterPosition, activeEnemies));
			}
		}
	}

	// Token: 0x06006FBD RID: 28605 RVA: 0x002C4DC4 File Offset: 0x002C2FC4
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

	// Token: 0x06006FBE RID: 28606 RVA: 0x002C4E44 File Offset: 0x002C3044
	protected IEnumerator HandleAmbientSpawnTime(Vector2 centerPoint, float remainingTime, float maxEffectRadius)
	{
		float elapsed = 0f;
		while (elapsed < remainingTime)
		{
			elapsed += BraveTime.DeltaTime;
			this.HandleAmbientVFXSpawn(centerPoint, maxEffectRadius);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006FBF RID: 28607 RVA: 0x002C4E74 File Offset: 0x002C3074
	protected IEnumerator ProcessEffectOverTime(Vector2 centerPoint, List<AIActor> enemiesInRoom)
	{
		float elapsed = 0f;
		List<AIActor> processedEnemies = new List<AIActor>();
		float maxEffectRadius = 10f;
		for (int i = 0; i < enemiesInRoom.Count; i++)
		{
			maxEffectRadius = Mathf.Max(maxEffectRadius, Vector2.Distance(enemiesInRoom[i].CenterPosition, centerPoint));
		}
		while (elapsed < this.EffectTime)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / this.EffectTime;
			float CurrentRadius = Mathf.Lerp(0f, maxEffectRadius, t);
			for (int j = 0; j < enemiesInRoom.Count; j++)
			{
				AIActor aiactor = enemiesInRoom[j];
				if (!processedEnemies.Contains(aiactor))
				{
					if (!this.AffectsBosses && aiactor.healthHaver.IsBoss)
					{
						processedEnemies.Add(aiactor);
					}
					else
					{
						float num = Vector2.Distance(centerPoint, aiactor.CenterPosition);
						if (num <= CurrentRadius)
						{
							this.AffectEnemy(aiactor);
							if (this.OnTargetEffectVFX != null)
							{
								SpawnManager.SpawnVFX(this.OnTargetEffectVFX, aiactor.CenterPosition, Quaternion.identity, false);
							}
							processedEnemies.Add(aiactor);
						}
					}
				}
			}
			this.HandleAmbientVFXSpawn(centerPoint, CurrentRadius);
			yield return null;
		}
		if (this.AmbientVFXTime > this.EffectTime)
		{
			base.StartCoroutine(this.HandleAmbientSpawnTime(centerPoint, this.AmbientVFXTime - this.EffectTime, maxEffectRadius));
		}
		yield break;
	}

	// Token: 0x06006FC0 RID: 28608
	protected abstract void AffectEnemy(AIActor target);

	// Token: 0x06006FC1 RID: 28609 RVA: 0x002C4EA0 File Offset: 0x002C30A0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04006F11 RID: 28433
	public float EffectTime;

	// Token: 0x04006F12 RID: 28434
	public GameObject OnUserEffectVFX;

	// Token: 0x04006F13 RID: 28435
	public GameObject OnTargetEffectVFX;

	// Token: 0x04006F14 RID: 28436
	public float AmbientVFXTime;

	// Token: 0x04006F15 RID: 28437
	public GameObject AmbientVFX;

	// Token: 0x04006F16 RID: 28438
	public float minTimeBetweenAmbientVFX = 0.1f;

	// Token: 0x04006F17 RID: 28439
	public bool FlashScreen;

	// Token: 0x04006F18 RID: 28440
	public bool AffectsBosses;

	// Token: 0x04006F19 RID: 28441
	private float m_ambientTimer;
}
