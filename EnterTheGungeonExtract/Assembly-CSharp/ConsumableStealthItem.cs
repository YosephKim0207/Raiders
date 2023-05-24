using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001389 RID: 5001
public class ConsumableStealthItem : PlayerItem
{
	// Token: 0x06007164 RID: 29028 RVA: 0x002D1028 File Offset: 0x002CF228
	protected override void DoEffect(PlayerController user)
	{
		base.DoEffect(user);
		AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
		user.StartCoroutine(this.HandleStealth(user));
	}

	// Token: 0x06007165 RID: 29029 RVA: 0x002D1050 File Offset: 0x002CF250
	private IEnumerator HandleStealth(PlayerController user)
	{
		float elapsed = 0f;
		user.ChangeSpecialShaderFlag(1, 1f);
		user.SetIsStealthed(true, "smoke");
		user.SetCapableOfStealing(true, "ConsumableStealthItem", null);
		user.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
		user.PlayEffectOnActor(this.poofVfx, Vector3.zero, false, true, false);
		user.OnDidUnstealthyAction += this.BreakStealth;
		user.OnItemStolen += this.BreakStealthOnSteal;
		while (elapsed < this.Duration)
		{
			elapsed += BraveTime.DeltaTime;
			if (!user.IsStealthed)
			{
				break;
			}
			yield return null;
		}
		if (user.IsStealthed)
		{
			this.BreakStealth(user);
		}
		yield break;
	}

	// Token: 0x06007166 RID: 29030 RVA: 0x002D1074 File Offset: 0x002CF274
	private void BreakStealthOnSteal(PlayerController arg1, ShopItemController arg2)
	{
		this.BreakStealth(arg1);
	}

	// Token: 0x06007167 RID: 29031 RVA: 0x002D1080 File Offset: 0x002CF280
	private void BreakStealth(PlayerController obj)
	{
		obj.PlayEffectOnActor(this.poofVfx, Vector3.zero, false, true, false);
		obj.OnDidUnstealthyAction -= this.BreakStealth;
		obj.OnItemStolen -= this.BreakStealthOnSteal;
		obj.specRigidbody.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
		obj.ChangeSpecialShaderFlag(1, 0f);
		obj.SetIsStealthed(false, "smoke");
		obj.SetCapableOfStealing(false, "ConsumableStealthItem", null);
		AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
	}

	// Token: 0x0400712C RID: 28972
	public float Duration = 10f;

	// Token: 0x0400712D RID: 28973
	public GameObject poofVfx;
}
