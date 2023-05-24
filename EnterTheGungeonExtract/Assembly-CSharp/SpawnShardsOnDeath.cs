using System;
using UnityEngine;

// Token: 0x020010C7 RID: 4295
public class SpawnShardsOnDeath : OnDeathBehavior
{
	// Token: 0x06005E95 RID: 24213 RVA: 0x00245238 File Offset: 0x00243438
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005E96 RID: 24214 RVA: 0x00245240 File Offset: 0x00243440
	protected override void OnTrigger(Vector2 deathVelocity)
	{
		this.HandleShardSpawns(deathVelocity, null);
	}

	// Token: 0x06005E97 RID: 24215 RVA: 0x00245260 File Offset: 0x00243460
	public void HandleShardSpawns(Vector2 sourceVelocity, Vector2? spawnPos = null)
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
			this.SpawnShards(sourceVelocity, -45f, 45f, num, sourceVelocity.magnitude * 0.5f, sourceVelocity.magnitude * 1.5f, null);
			break;
		case MinorBreakable.BreakStyle.BURST:
			this.SpawnShards(Vector2.right, -180f, 180f, num, 1f, 2f, null);
			break;
		case MinorBreakable.BreakStyle.JET:
			this.SpawnShards(sourceVelocity, -15f, 15f, num, sourceVelocity.magnitude * 0.5f, sourceVelocity.magnitude * 1.5f, null);
			break;
		default:
			if (breakStyle == MinorBreakable.BreakStyle.CUSTOM)
			{
				this.SpawnShards(this.direction, this.minAngle, this.maxAngle, this.verticalSpeed, this.minMagnitude, this.maxMagnitude, spawnPos);
			}
			break;
		}
	}

	// Token: 0x06005E98 RID: 24216 RVA: 0x00245390 File Offset: 0x00243590
	public void SpawnShards(Vector2 direction, float minAngle, float maxAngle, float verticalSpeed, float minMagnitude, float maxMagnitude, Vector2? spawnPos = null)
	{
		Vector3 vector = ((spawnPos == null) ? base.specRigidbody.GetUnitCenter(ColliderType.HitBox) : spawnPos.Value);
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
					tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
					if (base.sprite.attachParent != null && component != null)
					{
						component.attachParent = base.sprite.attachParent;
						component.HeightOffGround = base.sprite.HeightOffGround;
					}
					DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
					vector2 = Vector3.Scale(vector2, shardCluster.forceAxialMultiplier) * shardCluster.forceMultiplier;
					component2.Trigger(vector2, this.heightOffGround, shardCluster.rotationMultiplier);
				}
			}
		}
	}

	// Token: 0x040058C1 RID: 22721
	public MinorBreakable.BreakStyle breakStyle;

	// Token: 0x040058C2 RID: 22722
	[ShowInInspectorIf("breakStyle", 4, true)]
	public Vector2 direction;

	// Token: 0x040058C3 RID: 22723
	[ShowInInspectorIf("breakStyle", 4, true)]
	public float minAngle;

	// Token: 0x040058C4 RID: 22724
	[ShowInInspectorIf("breakStyle", 4, true)]
	public float maxAngle;

	// Token: 0x040058C5 RID: 22725
	[ShowInInspectorIf("breakStyle", 4, true)]
	public float verticalSpeed;

	// Token: 0x040058C6 RID: 22726
	[ShowInInspectorIf("breakStyle", 4, true)]
	public float minMagnitude;

	// Token: 0x040058C7 RID: 22727
	[ShowInInspectorIf("breakStyle", 4, true)]
	public float maxMagnitude;

	// Token: 0x040058C8 RID: 22728
	public ShardCluster[] shardClusters;

	// Token: 0x040058C9 RID: 22729
	public float heightOffGround = 0.1f;
}
