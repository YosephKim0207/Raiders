using System;
using System.Collections;
using UnityEngine;

// Token: 0x020014A4 RID: 5284
public class ShrinkEnemiesInRoomItem : AffectEnemiesInRoomItem
{
	// Token: 0x06007827 RID: 30759 RVA: 0x003005F4 File Offset: 0x002FE7F4
	protected override void AffectEnemy(AIActor target)
	{
		target.StartCoroutine(this.HandleShrink(target));
	}

	// Token: 0x06007828 RID: 30760 RVA: 0x00300604 File Offset: 0x002FE804
	private IEnumerator HandleShrink(AIActor target)
	{
		AkSoundEngine.PostEvent("Play_OBJ_lightning_flash_01", base.gameObject);
		float elapsed = 0f;
		Vector2 startScale = target.EnemyScale;
		int cachedLayer = target.gameObject.layer;
		int cachedOutlineLayer = cachedLayer;
		if (this.DepixelatesTargets)
		{
			target.gameObject.layer = LayerMask.NameToLayer("Unpixelated");
			cachedOutlineLayer = SpriteOutlineManager.ChangeOutlineLayer(target.sprite, LayerMask.NameToLayer("Unpixelated"));
		}
		target.ClearPath();
		DazedBehavior db = new DazedBehavior();
		db.PointReachedPauseTime = 0.5f;
		db.PathInterval = 0.5f;
		if (target.knockbackDoer)
		{
			target.knockbackDoer.weight /= 3f;
		}
		if (target.healthHaver)
		{
			target.healthHaver.AllDamageMultiplier *= this.DamageMultiplier;
		}
		target.behaviorSpeculator.OverrideBehaviors.Insert(0, db);
		target.behaviorSpeculator.RefreshBehaviors();
		this.m_isCurrentlyActive = true;
		while (elapsed < this.ShrinkTime)
		{
			elapsed += target.LocalDeltaTime;
			target.EnemyScale = Vector2.Lerp(startScale, this.TargetScale, elapsed / this.ShrinkTime);
			yield return null;
		}
		elapsed = 0f;
		while (elapsed < this.HoldTime)
		{
			this.m_activeElapsed = elapsed;
			this.m_activeDuration = this.HoldTime;
			elapsed += target.LocalDeltaTime;
			yield return null;
		}
		elapsed = 0f;
		while (elapsed < this.RegrowTime)
		{
			elapsed += target.LocalDeltaTime;
			target.EnemyScale = Vector2.Lerp(this.TargetScale, startScale, elapsed / this.RegrowTime);
			yield return null;
		}
		if (target.knockbackDoer)
		{
			target.knockbackDoer.weight *= 3f;
		}
		if (target.healthHaver)
		{
			target.healthHaver.AllDamageMultiplier /= this.DamageMultiplier;
		}
		target.behaviorSpeculator.OverrideBehaviors.Remove(db);
		target.behaviorSpeculator.RefreshBehaviors();
		this.m_isCurrentlyActive = false;
		if (this.DepixelatesTargets)
		{
			target.gameObject.layer = cachedLayer;
			SpriteOutlineManager.ChangeOutlineLayer(target.sprite, cachedOutlineLayer);
		}
		yield break;
	}

	// Token: 0x06007829 RID: 30761 RVA: 0x00300628 File Offset: 0x002FE828
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007A4E RID: 31310
	public Vector2 TargetScale;

	// Token: 0x04007A4F RID: 31311
	public float ShrinkTime = 0.1f;

	// Token: 0x04007A50 RID: 31312
	public float HoldTime = 3f;

	// Token: 0x04007A51 RID: 31313
	public float RegrowTime = 3f;

	// Token: 0x04007A52 RID: 31314
	public float DamageMultiplier = 2f;

	// Token: 0x04007A53 RID: 31315
	public bool DepixelatesTargets;
}
