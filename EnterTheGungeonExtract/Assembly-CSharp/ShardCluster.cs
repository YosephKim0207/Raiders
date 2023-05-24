using System;
using UnityEngine;

// Token: 0x02001205 RID: 4613
[Serializable]
public class ShardCluster
{
	// Token: 0x06006731 RID: 26417 RVA: 0x00286328 File Offset: 0x00284528
	public void SpawnShards(Vector2 position, Vector2 direction, float minAngle, float maxAngle, float verticalSpeed, float minMagnitude, float maxMagnitude, tk2dSprite sourceSprite)
	{
		int num = UnityEngine.Random.Range(this.minFromCluster, this.maxFromCluster + 1);
		int num2 = UnityEngine.Random.Range(0, this.clusterObjects.Length);
		int num3 = 0;
		for (int i = 0; i < num; i++)
		{
			float lowDiscrepancyRandom = BraveMathCollege.GetLowDiscrepancyRandom(num3);
			num3++;
			float num4 = Mathf.Lerp(minAngle, maxAngle, lowDiscrepancyRandom);
			Vector3 vector = Quaternion.Euler(0f, 0f, num4) * (direction.normalized * UnityEngine.Random.Range(minMagnitude, maxMagnitude)).ToVector3ZUp(verticalSpeed);
			int num5 = (num2 + i) % this.clusterObjects.Length;
			GameObject gameObject = SpawnManager.SpawnDebris(this.clusterObjects[num5].gameObject, position, Quaternion.identity);
			tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
			if (sourceSprite != null && sourceSprite.attachParent != null && component != null)
			{
				component.attachParent = sourceSprite.attachParent;
				component.HeightOffGround = sourceSprite.HeightOffGround;
			}
			DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
			vector = Vector3.Scale(vector, this.forceAxialMultiplier) * this.forceMultiplier;
			component2.Trigger(vector, 0.5f, this.rotationMultiplier);
		}
	}

	// Token: 0x040062FE RID: 25342
	public int minFromCluster = 1;

	// Token: 0x040062FF RID: 25343
	public int maxFromCluster = 3;

	// Token: 0x04006300 RID: 25344
	public float forceMultiplier = 1f;

	// Token: 0x04006301 RID: 25345
	public Vector3 forceAxialMultiplier = Vector3.one;

	// Token: 0x04006302 RID: 25346
	public float rotationMultiplier = 1f;

	// Token: 0x04006303 RID: 25347
	public DebrisObject[] clusterObjects;
}
