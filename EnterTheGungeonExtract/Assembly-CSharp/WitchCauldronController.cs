using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200124E RID: 4686
public class WitchCauldronController : MonoBehaviour
{
	// Token: 0x17000F9B RID: 3995
	// (get) Token: 0x06006910 RID: 26896 RVA: 0x00291FB4 File Offset: 0x002901B4
	// (set) Token: 0x06006911 RID: 26897 RVA: 0x00291FBC File Offset: 0x002901BC
	public bool IsGunInPot { get; private set; }

	// Token: 0x06006912 RID: 26898 RVA: 0x00291FC8 File Offset: 0x002901C8
	public void Start()
	{
		base.StartCoroutine(this.HandleBackgroundBubblin());
	}

	// Token: 0x06006913 RID: 26899 RVA: 0x00291FD8 File Offset: 0x002901D8
	private IEnumerator HandleBackgroundBubblin()
	{
		tk2dSpriteAnimationClip mainIdleClip = this.cauldronSprite.spriteAnimator.GetClipByName("cauldron_idle");
		for (;;)
		{
			while (this.cauldronSprite.spriteAnimator.IsPlaying("cauldron_splash"))
			{
				yield return null;
			}
			int idleMultiplex = UnityEngine.Random.Range(4, 12);
			float timeToIdle = (float)idleMultiplex * ((float)mainIdleClip.frames.Length / mainIdleClip.fps);
			this.cauldronSprite.spriteAnimator.Play(mainIdleClip);
			yield return new WaitForSeconds(timeToIdle);
			int randomIndex = UnityEngine.Random.Range(0, this.cauldronIns.Length);
			tk2dSpriteAnimationClip inClip = this.cauldronSprite.spriteAnimator.GetClipByName(this.cauldronIns[randomIndex]);
			tk2dSpriteAnimationClip idleClip = this.cauldronSprite.spriteAnimator.GetClipByName(this.cauldronIdles[randomIndex]);
			tk2dSpriteAnimationClip outClip = this.cauldronSprite.spriteAnimator.GetClipByName(this.cauldronOuts[randomIndex]);
			if (!this.cauldronSprite.spriteAnimator.IsPlaying("cauldron_splash"))
			{
				this.cauldronSprite.spriteAnimator.Play(inClip);
				yield return new WaitForSeconds((float)inClip.frames.Length / inClip.fps);
				if (!this.cauldronSprite.spriteAnimator.IsPlaying("cauldron_splash"))
				{
					this.cauldronSprite.spriteAnimator.Play(idleClip);
					yield return new WaitForSeconds((float)idleClip.frames.Length / idleClip.fps);
					if (!this.cauldronSprite.spriteAnimator.IsPlaying("cauldron_splash"))
					{
						this.cauldronSprite.spriteAnimator.Play(outClip);
						yield return new WaitForSeconds((float)outClip.frames.Length / outClip.fps);
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x06006914 RID: 26900 RVA: 0x00291FF4 File Offset: 0x002901F4
	public bool TossPlayerEquippedGun(PlayerController player)
	{
		if (player.CurrentGun != null && player.CurrentGun.CanActuallyBeDropped(player) && !player.CurrentGun.InfiniteAmmo)
		{
			this.IsGunInPot = true;
			Gun currentGun = player.CurrentGun;
			this.TossObjectIntoPot(currentGun.GetSprite(), player.CenterPosition);
			player.inventory.RemoveGunFromInventory(currentGun);
			PickupObject.ItemQuality itemQuality = currentGun.quality;
			if (itemQuality < PickupObject.ItemQuality.S && UnityEngine.Random.value < this.baseChanceOfImprovingItem)
			{
				itemQuality++;
			}
			Gun itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<Gun>(itemQuality, this.lootTableToUse, false);
			if (itemOfTypeAndQuality != null)
			{
				base.StartCoroutine(this.DelayedItemSpawn(itemOfTypeAndQuality.gameObject, 3f));
			}
			else
			{
				base.StartCoroutine(this.DelayedItemSpawn(currentGun.gameObject, 3f));
			}
			UnityEngine.Object.Destroy(currentGun.gameObject);
			return true;
		}
		return false;
	}

	// Token: 0x06006915 RID: 26901 RVA: 0x002920E4 File Offset: 0x002902E4
	private IEnumerator DelayedItemSpawn(GameObject item, float delay)
	{
		yield return new WaitForSeconds(delay);
		Vector3 spawnPoint = this.cauldronSprite.WorldCenter - item.GetComponent<tk2dBaseSprite>().GetRelativePositionFromAnchor(tk2dBaseSprite.Anchor.MiddleCenter);
		this.cauldronSprite.spriteAnimator.Play("cauldron_splash");
		AkSoundEngine.PostEvent("Play_OBJ_cauldron_splash_01", base.gameObject);
		tk2dSpriteAnimator spriteAnimator = this.cauldronSprite.spriteAnimator;
		spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnAnimComplete));
		LootEngine.SpawnItem(item, spawnPoint, new Vector2(0f, -1f), 2f, true, false, false);
		if (this.CurseToGive > 0f)
		{
			StatModifier statModifier = new StatModifier();
			statModifier.statToBoost = PlayerStats.StatType.Curse;
			statModifier.amount = this.CurseToGive;
			statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			PlayerController bestActivePlayer = GameManager.Instance.BestActivePlayer;
			if (bestActivePlayer)
			{
				bestActivePlayer.ownerlessStatModifiers.Add(statModifier);
				bestActivePlayer.stats.RecalculateStats(bestActivePlayer, false, false);
			}
		}
		this.IsGunInPot = false;
		yield break;
	}

	// Token: 0x06006916 RID: 26902 RVA: 0x00292110 File Offset: 0x00290310
	public void TossObjectIntoPot(tk2dBaseSprite spriteSource, Vector3 startPosition)
	{
		base.StartCoroutine(this.HandleObjectPotToss(spriteSource, startPosition));
	}

	// Token: 0x06006917 RID: 26903 RVA: 0x00292124 File Offset: 0x00290324
	private IEnumerator HandleObjectPotToss(tk2dBaseSprite spriteSource, Vector3 startPosition)
	{
		GameObject fakeObject = new GameObject("cauldron temp object");
		tk2dSprite sprite = tk2dBaseSprite.AddComponent<tk2dSprite>(fakeObject, spriteSource.Collection, spriteSource.spriteId);
		sprite.HeightOffGround = 2f;
		sprite.PlaceAtPositionByAnchor(startPosition, tk2dBaseSprite.Anchor.MiddleCenter);
		Vector3 endPosition = this.cauldronSprite.WorldCenter.ToVector3ZUp(0f);
		float duration = 0.4f;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			Vector3 targetPosition = Vector3.Lerp(startPosition, endPosition, t);
			sprite.PlaceAtPositionByAnchor(targetPosition, tk2dBaseSprite.Anchor.MiddleCenter);
			sprite.UpdateZDepth();
			yield return null;
		}
		AkSoundEngine.PostEvent("Play_OBJ_cauldron_use_01", base.gameObject);
		this.cauldronSprite.spriteAnimator.Play("cauldron_splash");
		tk2dSpriteAnimator spriteAnimator = this.cauldronSprite.spriteAnimator;
		spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnAnimComplete));
		elapsed = 0f;
		duration = 0.25f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float num = 1f - elapsed / duration;
			sprite.scale = Vector3.one * num;
		}
		UnityEngine.Object.Destroy(fakeObject);
		yield break;
	}

	// Token: 0x06006918 RID: 26904 RVA: 0x00292150 File Offset: 0x00290350
	private void OnAnimComplete(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2)
	{
		arg1.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(arg1.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnAnimComplete));
		arg1.Play("cauldron_idle");
	}

	// Token: 0x04006561 RID: 25953
	public tk2dBaseSprite cauldronSprite;

	// Token: 0x04006562 RID: 25954
	public GenericLootTable lootTableToUse;

	// Token: 0x04006563 RID: 25955
	public float baseChanceOfImprovingItem = 0.5f;

	// Token: 0x04006564 RID: 25956
	public float CurseToGive = 2f;

	// Token: 0x04006565 RID: 25957
	public string[] cauldronIns;

	// Token: 0x04006566 RID: 25958
	public string[] cauldronIdles;

	// Token: 0x04006567 RID: 25959
	public string[] cauldronOuts;
}
