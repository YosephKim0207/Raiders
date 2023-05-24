using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020010DA RID: 4314
public class EnemyFactorySpawnPoint : DungeonPlaceableBehaviour
{
	// Token: 0x06005F07 RID: 24327 RVA: 0x002482A0 File Offset: 0x002464A0
	public void OnSpawn(AIActor actorToSpawn, IntVector2 spawnPosition, RoomHandler room)
	{
		base.StartCoroutine(this.HandleSpawnAnimations(actorToSpawn, spawnPosition, room));
	}

	// Token: 0x06005F08 RID: 24328 RVA: 0x002482B4 File Offset: 0x002464B4
	private IEnumerator HandleSpawnAnimations(AIActor actorToSpawn, IntVector2 spawnPosition, RoomHandler room)
	{
		if (!string.IsNullOrEmpty(this.spawnAnimationOpen))
		{
			this.animator.Play(this.spawnAnimationOpen);
		}
		yield return new WaitForSeconds(this.preSpawnDelay);
		if (this.spawnVFX != null)
		{
			GameObject gameObject = SpawnManager.SpawnVFX(this.spawnVFX, spawnPosition.ToVector3(), Quaternion.identity);
			gameObject.GetComponent<tk2dSprite>().PlaceAtPositionByAnchor(spawnPosition.ToVector3(), tk2dBaseSprite.Anchor.LowerCenter);
		}
		AIActor.Spawn(actorToSpawn, spawnPosition, room, false, AIActor.AwakenAnimationType.Default, true);
		yield return new WaitForSeconds(this.postSpawnDelay);
		if (!string.IsNullOrEmpty(this.spawnAnimationClose))
		{
			this.animator.Play(this.spawnAnimationClose);
		}
		yield break;
	}

	// Token: 0x06005F09 RID: 24329 RVA: 0x002482E4 File Offset: 0x002464E4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400593C RID: 22844
	public tk2dSpriteAnimator animator;

	// Token: 0x0400593D RID: 22845
	public string spawnAnimationOpen = string.Empty;

	// Token: 0x0400593E RID: 22846
	public string spawnAnimationClose = string.Empty;

	// Token: 0x0400593F RID: 22847
	public float preSpawnDelay = 1f;

	// Token: 0x04005940 RID: 22848
	public float postSpawnDelay = 0.5f;

	// Token: 0x04005941 RID: 22849
	public GameObject spawnVFX;
}
