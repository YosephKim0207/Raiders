using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200110C RID: 4364
public class BulletKingWallOpenDoer : BraveBehaviour
{
	// Token: 0x0600603D RID: 24637 RVA: 0x00251024 File Offset: 0x0024F224
	private void Start()
	{
		RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
		RoomHandler roomHandler = absoluteRoomFromPosition;
		roomHandler.OnEnemiesCleared = (Action)Delegate.Combine(roomHandler.OnEnemiesCleared, new Action(this.OnBossKill));
	}

	// Token: 0x0600603E RID: 24638 RVA: 0x0025107C File Offset: 0x0024F27C
	private void OnBossKill()
	{
		base.specRigidbody.PixelColliders[4].Enabled = false;
		base.specRigidbody.PixelColliders[5].Enabled = false;
		base.spriteAnimator.Play();
		Vector2 unitBottomLeft = base.specRigidbody.PixelColliders[4].UnitBottomLeft;
		Vector2 unitTopRight = base.specRigidbody.PixelColliders[5].UnitTopRight;
		SpawnManager.Instance.ClearRectOfDecals(unitBottomLeft, unitTopRight);
	}

	// Token: 0x0600603F RID: 24639 RVA: 0x002510FC File Offset: 0x0024F2FC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
