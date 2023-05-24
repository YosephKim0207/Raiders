using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001398 RID: 5016
[Serializable]
public class BarrageModule
{
	// Token: 0x0600719E RID: 29086 RVA: 0x002D262C File Offset: 0x002D082C
	public void DoBarrage(Vector2 startPoint, Vector2 direction, MonoBehaviour coroutineTarget)
	{
		List<Vector2> list = this.AcquireBarrageTargets(startPoint, direction);
		coroutineTarget.StartCoroutine(this.HandleBarrage(list));
	}

	// Token: 0x0600719F RID: 29087 RVA: 0x002D2650 File Offset: 0x002D0850
	protected List<Vector2> AcquireBarrageTargets(Vector2 startPoint, Vector2 direction)
	{
		List<Vector2> list = new List<Vector2>();
		float num = -this.barrageRadius / 2f;
		float num2 = BraveMathCollege.Atan2Degrees(direction);
		Quaternion quaternion = Quaternion.Euler(0f, 0f, num2);
		while (num < this.BarrageLength)
		{
			float num3 = Mathf.Clamp01(num / this.BarrageLength);
			float barrageWidth = this.BarrageWidth;
			float num4 = Mathf.Clamp(num, 0f, this.BarrageLength);
			for (int i = 0; i < this.BarrageColumns; i++)
			{
				float num5 = Mathf.Lerp(-barrageWidth, barrageWidth, ((float)i + 1f) / ((float)this.BarrageColumns + 1f));
				float num6 = UnityEngine.Random.Range(-barrageWidth / (4f * (float)this.BarrageColumns), barrageWidth / (4f * (float)this.BarrageColumns));
				Vector2 vector = new Vector2(num4, num5 + num6);
				Vector2 vector2 = (quaternion * vector).XY();
				list.Add(startPoint + vector2);
			}
			num += this.barrageRadius;
		}
		return list;
	}

	// Token: 0x060071A0 RID: 29088 RVA: 0x002D2764 File Offset: 0x002D0964
	private IEnumerator HandleBarrage(List<Vector2> targets)
	{
		while (targets.Count > 0)
		{
			Vector2 currentTarget = targets[0];
			targets.RemoveAt(0);
			Exploder.Explode(currentTarget, this.barrageExplosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
			yield return new WaitForSeconds(this.delayBetweenStrikes / (float)this.BarrageColumns);
		}
		yield break;
	}

	// Token: 0x04007307 RID: 29447
	public int BarrageColumns = 1;

	// Token: 0x04007308 RID: 29448
	public GameObject barrageVFX;

	// Token: 0x04007309 RID: 29449
	public ExplosionData barrageExplosionData;

	// Token: 0x0400730A RID: 29450
	public float barrageRadius = 1.5f;

	// Token: 0x0400730B RID: 29451
	public float delayBetweenStrikes = 0.25f;

	// Token: 0x0400730C RID: 29452
	public float BarrageWidth = 3f;

	// Token: 0x0400730D RID: 29453
	public float BarrageLength = 5f;
}
