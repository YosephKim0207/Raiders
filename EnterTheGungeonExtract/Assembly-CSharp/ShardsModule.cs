using System;
using UnityEngine;

// Token: 0x020010C6 RID: 4294
[Serializable]
public class ShardsModule
{
	// Token: 0x06005E92 RID: 24210 RVA: 0x00244FC8 File Offset: 0x002431C8
	public void HandleShardSpawns(Vector2 spawnPos, Vector2 sourceVelocity)
	{
		MinorBreakable.BreakStyle breakStyle = this.breakStyle;
		if (sourceVelocity == Vector2.zero && this.breakStyle != MinorBreakable.BreakStyle.CUSTOM)
		{
			breakStyle = MinorBreakable.BreakStyle.BURST;
		}
		float num = 1.5f;
		switch (breakStyle)
		{
		case MinorBreakable.BreakStyle.CONE:
			this.SpawnShards(spawnPos, sourceVelocity, -45f, 45f, num, sourceVelocity.magnitude * 0.5f, sourceVelocity.magnitude * 1.5f);
			break;
		case MinorBreakable.BreakStyle.BURST:
			this.SpawnShards(spawnPos, Vector2.right, -180f, 180f, num, 1f, 2f);
			break;
		case MinorBreakable.BreakStyle.JET:
			this.SpawnShards(spawnPos, sourceVelocity, -15f, 15f, num, sourceVelocity.magnitude * 0.5f, sourceVelocity.magnitude * 1.5f);
			break;
		default:
			if (breakStyle == MinorBreakable.BreakStyle.CUSTOM)
			{
				this.SpawnShards(spawnPos, this.direction, this.minAngle, this.maxAngle, this.verticalSpeed, this.minMagnitude, this.maxMagnitude);
			}
			break;
		}
	}

	// Token: 0x06005E93 RID: 24211 RVA: 0x002450E0 File Offset: 0x002432E0
	private void SpawnShards(Vector2 spawnPos, Vector2 direction, float minAngle, float maxAngle, float verticalSpeed, float minMagnitude, float maxMagnitude)
	{
		Vector3 vector = spawnPos;
		if (this.shardClusters != null && this.shardClusters.Length > 0)
		{
			int num = UnityEngine.Random.Range(0, 10);
			for (int i = 0; i < this.shardClusters.Length; i++)
			{
				ShardCluster shardCluster = this.shardClusters[i];
				int num2 = UnityEngine.Random.Range(shardCluster.minFromCluster, shardCluster.maxFromCluster + 1);
				int num3 = UnityEngine.Random.Range(0, shardCluster.clusterObjects.Length);
				for (int j = 0; j < num2; j++)
				{
					float lowDiscrepancyRandom = BraveMathCollege.GetLowDiscrepancyRandom(num);
					num++;
					float num4 = Mathf.Lerp(minAngle, maxAngle, lowDiscrepancyRandom);
					Vector3 vector2 = Quaternion.Euler(0f, 0f, num4) * (direction.normalized * UnityEngine.Random.Range(minMagnitude, maxMagnitude)).ToVector3ZUp(verticalSpeed);
					int num5 = (num3 + j) % shardCluster.clusterObjects.Length;
					GameObject gameObject = SpawnManager.SpawnDebris(shardCluster.clusterObjects[num5].gameObject, vector, Quaternion.identity);
					DebrisObject component = gameObject.GetComponent<DebrisObject>();
					vector2 = Vector3.Scale(vector2, shardCluster.forceAxialMultiplier) * shardCluster.forceMultiplier;
					component.Trigger(vector2, this.heightOffGround, shardCluster.rotationMultiplier);
				}
			}
		}
	}

	// Token: 0x040058B8 RID: 22712
	public MinorBreakable.BreakStyle breakStyle;

	// Token: 0x040058B9 RID: 22713
	[ShowInInspectorIf("breakStyle", 4, true)]
	public Vector2 direction;

	// Token: 0x040058BA RID: 22714
	[ShowInInspectorIf("breakStyle", 4, true)]
	public float minAngle;

	// Token: 0x040058BB RID: 22715
	[ShowInInspectorIf("breakStyle", 4, true)]
	public float maxAngle;

	// Token: 0x040058BC RID: 22716
	[ShowInInspectorIf("breakStyle", 4, true)]
	public float verticalSpeed;

	// Token: 0x040058BD RID: 22717
	[ShowInInspectorIf("breakStyle", 4, true)]
	public float minMagnitude;

	// Token: 0x040058BE RID: 22718
	[ShowInInspectorIf("breakStyle", 4, true)]
	public float maxMagnitude;

	// Token: 0x040058BF RID: 22719
	public ShardCluster[] shardClusters;

	// Token: 0x040058C0 RID: 22720
	public float heightOffGround = 0.1f;
}
