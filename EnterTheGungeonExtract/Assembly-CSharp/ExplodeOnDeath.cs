using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001091 RID: 4241
public class ExplodeOnDeath : OnDeathBehavior
{
	// Token: 0x06005D4F RID: 23887 RVA: 0x0023C5A0 File Offset: 0x0023A7A0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005D50 RID: 23888 RVA: 0x0023C5A8 File Offset: 0x0023A7A8
	protected override void OnTrigger(Vector2 dirVec)
	{
		if (base.enabled)
		{
			Exploder.Explode(base.specRigidbody.GetUnitCenter(ColliderType.HitBox), this.explosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
			if (this.LinearChainExplosion)
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleChainExplosion());
			}
		}
	}

	// Token: 0x06005D51 RID: 23889 RVA: 0x0023C608 File Offset: 0x0023A808
	public IEnumerator HandleChainExplosion()
	{
		Vector2 startPosition = base.behaviorSpeculator.aiActor.CenterPosition;
		Vector2 endPosition = ((!base.behaviorSpeculator.aiActor.TargetRigidbody) ? base.behaviorSpeculator.aiActor.AimCenter : base.behaviorSpeculator.aiActor.TargetRigidbody.UnitCenter);
		Vector2 dir = (endPosition - startPosition).normalized;
		if (this.ChainIsReversed)
		{
			dir = dir.Rotate(180f);
		}
		endPosition = startPosition + dir * this.ChainDistance;
		float perExplosionTime = this.ChainDuration / (float)(this.ChainNumExplosions + 3);
		float[] explosionTimes = new float[this.ChainNumExplosions];
		explosionTimes[0] = perExplosionTime * 3f;
		explosionTimes[1] = perExplosionTime * 5f;
		for (int i = 2; i < this.ChainNumExplosions; i++)
		{
			explosionTimes[i] = explosionTimes[i - 1] + perExplosionTime;
		}
		Vector2 lastValidPosition = startPosition;
		bool hitWall = false;
		List<GameObject> landingTargets = null;
		if (this.ChainTargetSprite)
		{
			landingTargets = new List<GameObject>(this.ChainNumExplosions);
			for (int j = 0; j < this.ChainNumExplosions; j++)
			{
				Vector2 vector = Vector2.Lerp(startPosition, endPosition, (float)(j + 1) / (float)this.ChainNumExplosions);
				if (!this.ValidExplosionPosition(vector))
				{
					hitWall = true;
				}
				if (!hitWall)
				{
					lastValidPosition = vector;
				}
				GameObject gameObject = SpawnManager.SpawnVFX(this.ChainTargetSprite, lastValidPosition, Quaternion.identity);
				gameObject.GetComponentInChildren<tk2dSprite>().UpdateZDepth();
				tk2dSpriteAnimator componentInChildren = gameObject.GetComponentInChildren<tk2dSpriteAnimator>();
				float num = explosionTimes[j];
				componentInChildren.Play(componentInChildren.DefaultClip, 0f, (float)componentInChildren.DefaultClip.frames.Length / num, false);
				landingTargets.Add(gameObject);
			}
		}
		int index = 0;
		float elapsed = 0f;
		lastValidPosition = startPosition;
		hitWall = false;
		while (elapsed < this.ChainDuration)
		{
			elapsed += BraveTime.DeltaTime;
			while (index < this.ChainNumExplosions && elapsed >= explosionTimes[index])
			{
				Vector2 vector2 = Vector2.Lerp(startPosition, endPosition, ((float)index + 1f) / (float)this.ChainNumExplosions);
				if (!this.ValidExplosionPosition(vector2))
				{
					hitWall = true;
				}
				if (!hitWall)
				{
					lastValidPosition = vector2;
				}
				Exploder.Explode(lastValidPosition, this.LinearChainExplosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
				if (landingTargets != null && landingTargets.Count > 0)
				{
					SpawnManager.Despawn(landingTargets[0]);
					landingTargets.RemoveAt(0);
				}
				index++;
			}
			yield return null;
		}
		if (landingTargets != null && landingTargets.Count > 0)
		{
			for (int k = 0; k < landingTargets.Count; k++)
			{
				SpawnManager.Despawn(landingTargets[k]);
			}
		}
		yield break;
	}

	// Token: 0x06005D52 RID: 23890 RVA: 0x0023C624 File Offset: 0x0023A824
	private bool ValidExplosionPosition(Vector2 pos)
	{
		IntVector2 intVector = pos.ToIntVector2(VectorConversions.Floor);
		return GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector) && GameManager.Instance.Dungeon.data[intVector].type != CellType.WALL;
	}

	// Token: 0x0400572B RID: 22315
	public ExplosionData explosionData;

	// Token: 0x0400572C RID: 22316
	public bool immuneToIBombApp;

	// Token: 0x0400572D RID: 22317
	public bool LinearChainExplosion;

	// Token: 0x0400572E RID: 22318
	public float ChainDuration = 1f;

	// Token: 0x0400572F RID: 22319
	public float ChainDistance = 10f;

	// Token: 0x04005730 RID: 22320
	public int ChainNumExplosions = 5;

	// Token: 0x04005731 RID: 22321
	public bool ChainIsReversed;

	// Token: 0x04005732 RID: 22322
	public GameObject ChainTargetSprite;

	// Token: 0x04005733 RID: 22323
	public ExplosionData LinearChainExplosionData;
}
