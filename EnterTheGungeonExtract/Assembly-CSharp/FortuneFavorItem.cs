using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

// Token: 0x02001406 RID: 5126
public class FortuneFavorItem : PlayerItem
{
	// Token: 0x0600745D RID: 29789 RVA: 0x002E4C70 File Offset: 0x002E2E70
	protected override void DoEffect(PlayerController user)
	{
		base.StartCoroutine(this.HandleShield(user));
		AkSoundEngine.PostEvent("Play_OBJ_fortune_shield_01", base.gameObject);
	}

	// Token: 0x0600745E RID: 29790 RVA: 0x002E4C94 File Offset: 0x002E2E94
	private IEnumerator HandleShield(PlayerController user)
	{
		base.IsCurrentlyActive = true;
		this.m_activeElapsed = 0f;
		this.m_activeDuration = this.duration;
		float innerRadiusSqrDistance = this.pushRadius * this.pushRadius;
		float outerRadiusSqrDistance = this.secondRadius * this.secondRadius;
		float finalRadiusSqrDistance = this.finalRadius * this.finalRadius;
		float pushStrengthRadians = this.pushStrength * 0.017453292f;
		List<Projectile> ensnaredProjectiles = new List<Projectile>();
		List<Vector2> initialDirections = new List<Vector2>();
		GameObject[] octantVFX = new GameObject[8];
		while (this.m_activeElapsed < this.m_activeDuration)
		{
			Vector2 playerCenter = user.CenterPosition;
			ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
			for (int i = 0; i < allProjectiles.Count; i++)
			{
				Projectile projectile = allProjectiles[i];
				if (projectile.Owner != user && !(projectile.Owner is PlayerController))
				{
					Vector2 worldCenter = projectile.sprite.WorldCenter;
					Vector2 vector = worldCenter - playerCenter;
					float num = Vector2.SqrMagnitude(vector);
					if (num < innerRadiusSqrDistance && !ensnaredProjectiles.Contains(projectile))
					{
						projectile.RemoveBulletScriptControl();
						ensnaredProjectiles.Add(projectile);
						initialDirections.Add(projectile.Direction);
						int num2 = BraveMathCollege.VectorToOctant(vector);
						if (octantVFX[num2] == null)
						{
							octantVFX[num2] = user.PlayEffectOnActor(this.sparkOctantVFX, Vector3.zero, true, true, false);
							octantVFX[num2].transform.rotation = Quaternion.Euler(0f, 0f, (float)(-45 + -45 * num2));
						}
					}
				}
			}
			for (int j = 0; j < ensnaredProjectiles.Count; j++)
			{
				Projectile projectile2 = ensnaredProjectiles[j];
				if (!projectile2)
				{
					ensnaredProjectiles.RemoveAt(j);
					initialDirections.RemoveAt(j);
					j--;
				}
				else
				{
					Vector2 worldCenter2 = projectile2.sprite.WorldCenter;
					Vector2 vector2 = playerCenter - worldCenter2;
					float num3 = Vector2.SqrMagnitude(vector2);
					if (num3 > finalRadiusSqrDistance)
					{
						ensnaredProjectiles.RemoveAt(j);
						initialDirections.RemoveAt(j);
						j--;
					}
					else if (num3 > outerRadiusSqrDistance)
					{
						projectile2.Direction = Vector3.RotateTowards(projectile2.Direction, initialDirections[j], pushStrengthRadians * BraveTime.DeltaTime * 0.5f, 0f).XY().normalized;
					}
					else
					{
						Vector2 vector3 = vector2 * -1f;
						float num4 = 1f;
						if (num3 / innerRadiusSqrDistance < 0.75f)
						{
							num4 = 3f;
						}
						vector3 = ((vector3.normalized + initialDirections[j].normalized) / 2f).normalized;
						projectile2.Direction = Vector3.RotateTowards(projectile2.Direction, vector3, pushStrengthRadians * BraveTime.DeltaTime * num4, 0f).XY().normalized;
					}
				}
			}
			for (int k = 0; k < 8; k++)
			{
				if (octantVFX[k] != null && !octantVFX[k])
				{
					octantVFX[k] = null;
				}
			}
			yield return null;
		}
		base.IsCurrentlyActive = false;
		yield break;
	}

	// Token: 0x0600745F RID: 29791 RVA: 0x002E4CB8 File Offset: 0x002E2EB8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040075FE RID: 30206
	public float pushRadius = 4f;

	// Token: 0x040075FF RID: 30207
	public float secondRadius = 6f;

	// Token: 0x04007600 RID: 30208
	public float finalRadius = 8f;

	// Token: 0x04007601 RID: 30209
	public float pushStrength = 10f;

	// Token: 0x04007602 RID: 30210
	public float duration = 5f;

	// Token: 0x04007603 RID: 30211
	public GameObject sparkOctantVFX;
}
