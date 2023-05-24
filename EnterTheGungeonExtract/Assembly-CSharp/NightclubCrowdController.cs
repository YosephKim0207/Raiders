using System;
using System.Collections;
using UnityEngine;

// Token: 0x020011C0 RID: 4544
public class NightclubCrowdController : MonoBehaviour
{
	// Token: 0x06006559 RID: 25945 RVA: 0x00276AB8 File Offset: 0x00274CB8
	private IEnumerator Start()
	{
		IntVector2 minPos = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round)).area.basePosition;
		IntVector2 maxPos = minPos + new IntVector2(42, 42);
		Vector2 primaryCenter = GameManager.Instance.PrimaryPlayer.CenterPosition;
		while (primaryCenter.x > (float)maxPos.x || primaryCenter.y > (float)maxPos.y)
		{
			primaryCenter = GameManager.Instance.PrimaryPlayer.CenterPosition;
			yield return null;
		}
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].PostProcessProjectile += this.NightclubCrowdController_PostProcessProjectile;
			GameManager.Instance.AllPlayers[i].PostProcessBeam += this.NightclubCrowdController_PostProcessBeam;
		}
		yield break;
	}

	// Token: 0x0600655A RID: 25946 RVA: 0x00276AD4 File Offset: 0x00274CD4
	private void NightclubCrowdController_PostProcessBeam(BeamController obj)
	{
		this.Panic();
	}

	// Token: 0x0600655B RID: 25947 RVA: 0x00276ADC File Offset: 0x00274CDC
	private void NightclubCrowdController_PostProcessProjectile(Projectile obj, float effectChanceScalar)
	{
		this.Panic();
	}

	// Token: 0x0600655C RID: 25948 RVA: 0x00276AE4 File Offset: 0x00274CE4
	public void Panic()
	{
		if (this.m_departed)
		{
			return;
		}
		if (this.OnPanic != null)
		{
			this.OnPanic();
		}
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].PostProcessProjectile -= this.NightclubCrowdController_PostProcessProjectile;
			GameManager.Instance.AllPlayers[i].PostProcessBeam -= this.NightclubCrowdController_PostProcessBeam;
		}
		this.m_departed = true;
		base.StartCoroutine(this.HandlePanic());
		base.StartCoroutine(this.HandleDanceFloors());
	}

	// Token: 0x0600655D RID: 25949 RVA: 0x00276B8C File Offset: 0x00274D8C
	private IEnumerator HandleDanceFloors()
	{
		float ela = 0f;
		float cachedEmissivePower = this.DanceFloors[0].renderer.material.GetFloat("_EmissivePower");
		while (ela < 0.5f)
		{
			bool wasBelow = ela < 0.2f;
			ela += BraveTime.DeltaTime;
			if (ela > 0.2f && wasBelow)
			{
				for (int i = 0; i < this.DanceFloors.Length; i++)
				{
					this.DanceFloors[i].renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutoutEmissive");
				}
			}
			if (ela < 0.2f)
			{
				if (ela % 0.1f < 0.05f)
				{
					for (int j = 0; j < this.DanceFloors.Length; j++)
					{
						this.DanceFloors[j].usesOverrideMaterial = true;
						this.DanceFloors[j].renderer.material.SetFloat("_EmissivePower", 0f);
					}
				}
				else
				{
					for (int k = 0; k < this.DanceFloors.Length; k++)
					{
						this.DanceFloors[k].renderer.material.SetFloat("_EmissivePower", cachedEmissivePower);
					}
				}
			}
			else
			{
				for (int l = 0; l < this.DanceFloors.Length; l++)
				{
					this.DanceFloors[l].spriteAnimator.Stop();
					this.DanceFloors[l].renderer.material.SetFloat("_EmissivePower", Mathf.Lerp(cachedEmissivePower, 0f, (ela - 0.2f) / 0.3f));
				}
			}
			yield return null;
		}
		for (int m = 0; m < this.DanceFloors.Length; m++)
		{
			this.DanceFloors[m].renderer.material.SetFloat("_EmissivePower", 0f);
		}
		yield break;
	}

	// Token: 0x0600655E RID: 25950 RVA: 0x00276BA8 File Offset: 0x00274DA8
	private IEnumerator HandlePanic()
	{
		Vector3 exitLocation = base.transform.position.Quantize(0.0625f);
		float[] targetXCoords = new float[this.Dancers.Length];
		bool[] hasReachedCenter = new bool[this.Dancers.Length];
		for (int i = 0; i < this.Dancers.Length; i++)
		{
			hasReachedCenter[i] = false;
			targetXCoords[i] = UnityEngine.Random.Range(exitLocation.x - 1.25f, exitLocation.x);
		}
		for (;;)
		{
			bool allDancersDestroyed = true;
			for (int j = 0; j < this.Dancers.Length; j++)
			{
				if (this.Dancers[j])
				{
					allDancersDestroyed = false;
					if (hasReachedCenter[j])
					{
						this.Dancers[j].position = this.Dancers[j].position + Vector3.down * 10f * BraveTime.DeltaTime;
						if (this.Dancers[j].position.y < exitLocation.y - 10f)
						{
							UnityEngine.Object.Destroy(this.Dancers[j].gameObject);
						}
					}
					else
					{
						this.Dancers[j].position = Vector3.MoveTowards(this.Dancers[j].position, exitLocation.WithX(targetXCoords[j]), 10f * BraveTime.DeltaTime);
						if (Vector2.Distance(this.Dancers[j].PositionVector2(), exitLocation.WithX(targetXCoords[j]).XY()) < 1f)
						{
							hasReachedCenter[j] = true;
						}
					}
				}
			}
			if (allDancersDestroyed)
			{
				break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04006111 RID: 24849
	public Transform[] Dancers;

	// Token: 0x04006112 RID: 24850
	public Action OnPanic;

	// Token: 0x04006113 RID: 24851
	public tk2dBaseSprite[] DanceFloors;

	// Token: 0x04006114 RID: 24852
	private bool m_departed;
}
