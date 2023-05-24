using System;
using UnityEngine;

// Token: 0x020010C5 RID: 4293
public class SpawnShardsArbitrarily : BraveBehaviour
{
	// Token: 0x06005E8C RID: 24204 RVA: 0x00244C90 File Offset: 0x00242E90
	private void Start()
	{
		if (this.TriggerOnDamaged)
		{
			base.healthHaver.OnDamaged += this.HandleDamaged;
		}
	}

	// Token: 0x06005E8D RID: 24205 RVA: 0x00244CB4 File Offset: 0x00242EB4
	private void HandleDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		this.HandleShardSpawns(damageDirection.normalized);
	}

	// Token: 0x06005E8E RID: 24206 RVA: 0x00244CC4 File Offset: 0x00242EC4
	protected override void OnDestroy()
	{
		if (this.TriggerOnDestroy)
		{
			this.HandleShardSpawns(Vector2.zero);
		}
		base.OnDestroy();
	}

	// Token: 0x06005E8F RID: 24207 RVA: 0x00244CE4 File Offset: 0x00242EE4
	private void HandleShardSpawns(Vector2 sourceVelocity)
	{
		MinorBreakable.BreakStyle breakStyle = this.breakStyle;
		if (sourceVelocity == Vector2.zero)
		{
			breakStyle = MinorBreakable.BreakStyle.BURST;
		}
		float num = 1.5f;
		switch (breakStyle)
		{
		case MinorBreakable.BreakStyle.CONE:
			this.SpawnShards(sourceVelocity, -45f, 45f, num, sourceVelocity.magnitude * 0.5f, sourceVelocity.magnitude * 1.5f);
			break;
		case MinorBreakable.BreakStyle.BURST:
			this.SpawnShards(Vector2.right, -180f, 180f, num, 1f, 2f);
			break;
		case MinorBreakable.BreakStyle.JET:
			this.SpawnShards(sourceVelocity, -15f, 15f, num, sourceVelocity.magnitude * 0.5f, sourceVelocity.magnitude * 1.5f);
			break;
		}
	}

	// Token: 0x06005E90 RID: 24208 RVA: 0x00244DB4 File Offset: 0x00242FB4
	public void SpawnShards(Vector2 direction, float minAngle, float maxAngle, float verticalSpeed, float minMagnitude, float maxMagnitude)
	{
		Vector3 vector = ((!base.specRigidbody) ? ((!base.sprite) ? base.transform.position : base.sprite.WorldCenter.ToVector3ZisY(0f)) : base.specRigidbody.GetUnitCenter(ColliderType.HitBox).ToVector3ZisY(0f));
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
					if (gameObject)
					{
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
	}

	// Token: 0x040058B3 RID: 22707
	public MinorBreakable.BreakStyle breakStyle;

	// Token: 0x040058B4 RID: 22708
	public ShardCluster[] shardClusters;

	// Token: 0x040058B5 RID: 22709
	public float heightOffGround = 0.1f;

	// Token: 0x040058B6 RID: 22710
	public bool TriggerOnDestroy;

	// Token: 0x040058B7 RID: 22711
	public bool TriggerOnDamaged;
}
