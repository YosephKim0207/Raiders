using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001357 RID: 4951
public class BlinkPassiveItem : PassiveItem
{
	// Token: 0x0600704A RID: 28746 RVA: 0x002C8670 File Offset: 0x002C6870
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (player.IsDodgeRolling)
		{
			player.ForceStopDodgeRoll();
		}
		if (this.ScarfPrefab)
		{
			this.m_scarf = UnityEngine.Object.Instantiate<GameObject>(this.ScarfPrefab.gameObject).GetComponent<ScarfAttachmentDoer>();
			this.m_scarf.Initialize(player);
		}
		if (this.ModifiesDodgeRoll)
		{
			player.rollStats.rollDistanceMultiplier *= this.DodgeRollDistanceMultiplier;
			player.rollStats.rollTimeMultiplier *= this.DodgeRollTimeMultiplier;
			player.rollStats.additionalInvulnerabilityFrames += this.AdditionalInvulnerabilityFrames;
		}
		if (!PassiveItem.ActiveFlagItems.ContainsKey(player))
		{
			PassiveItem.ActiveFlagItems.Add(player, new Dictionary<Type, int>());
		}
		if (!PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player].Add(base.GetType(), 1);
		}
		else
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = PassiveItem.ActiveFlagItems[player][base.GetType()] + 1;
		}
		this.afterimage = player.gameObject.AddComponent<AfterImageTrailController>();
		this.afterimage.spawnShadows = false;
		this.afterimage.shadowTimeDelay = 0.05f;
		this.afterimage.shadowLifetime = 0.3f;
		this.afterimage.minTranslation = 0.05f;
		this.afterimage.dashColor = Color.black;
		this.afterimage.maxEmission = 0f;
		this.afterimage.minEmission = 0f;
		this.afterimage.OverrideImageShader = ShaderCache.Acquire("Brave/Internal/DownwellAfterImage");
		player.OnRollStarted += this.OnRollStarted;
		player.OnBlinkShadowCreated = (Action<tk2dSprite>)Delegate.Combine(player.OnBlinkShadowCreated, new Action<tk2dSprite>(this.OnBlinkCloneCreated));
		base.Pickup(player);
	}

	// Token: 0x0600704B RID: 28747 RVA: 0x002C8878 File Offset: 0x002C6A78
	public void OnBlinkCloneCreated(tk2dSprite cloneSprite)
	{
		SpawnManager.SpawnVFX(this.BlinkpoofVfx, cloneSprite.WorldCenter, Quaternion.identity);
	}

	// Token: 0x0600704C RID: 28748 RVA: 0x002C8898 File Offset: 0x002C6A98
	private void OnRollStarted(PlayerController obj, Vector2 dirVec)
	{
		if (GameManager.Instance.Dungeon && GameManager.Instance.Dungeon.IsEndTimes)
		{
			return;
		}
		obj.StartCoroutine(this.HandleAfterImageStop(obj));
	}

	// Token: 0x0600704D RID: 28749 RVA: 0x002C88D4 File Offset: 0x002C6AD4
	private IEnumerator HandleAfterImageStop(PlayerController player)
	{
		player.PlayEffectOnActor(this.BlinkpoofVfx, Vector3.zero, false, true, false);
		AkSoundEngine.PostEvent("Play_CHR_ninja_dash_01", base.gameObject);
		if (!player.IsDodgeRolling)
		{
			yield return null;
		}
		else
		{
			this.afterimage.spawnShadows = true;
			while (player.IsDodgeRolling)
			{
				yield return null;
			}
			if (this.afterimage)
			{
				this.afterimage.spawnShadows = false;
			}
		}
		player.PlayEffectOnActor(this.BlinkpoofVfx, Vector3.zero, false, true, false);
		AkSoundEngine.PostEvent("Play_CHR_ninja_dash_01", base.gameObject);
		yield break;
	}

	// Token: 0x0600704E RID: 28750 RVA: 0x002C88F8 File Offset: 0x002C6AF8
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		if (this.ModifiesDodgeRoll)
		{
			player.rollStats.rollDistanceMultiplier /= this.DodgeRollDistanceMultiplier;
			player.rollStats.rollTimeMultiplier /= this.DodgeRollTimeMultiplier;
			player.rollStats.additionalInvulnerabilityFrames -= this.AdditionalInvulnerabilityFrames;
			player.rollStats.additionalInvulnerabilityFrames = Mathf.Max(player.rollStats.additionalInvulnerabilityFrames, 0);
		}
		if (PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[player][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[player].Remove(base.GetType());
			}
		}
		if (this.m_scarf)
		{
			UnityEngine.Object.Destroy(this.m_scarf.gameObject);
			this.m_scarf = null;
		}
		player.OnRollStarted -= this.OnRollStarted;
		player.OnBlinkShadowCreated = (Action<tk2dSprite>)Delegate.Remove(player.OnBlinkShadowCreated, new Action<tk2dSprite>(this.OnBlinkCloneCreated));
		if (this.afterimage)
		{
			UnityEngine.Object.Destroy(this.afterimage);
		}
		this.afterimage = null;
		debrisObject.GetComponent<BlinkPassiveItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x0600704F RID: 28751 RVA: 0x002C8A88 File Offset: 0x002C6C88
	protected override void OnDestroy()
	{
		if (this.m_scarf)
		{
			UnityEngine.Object.Destroy(this.m_scarf.gameObject);
			this.m_scarf = null;
		}
		if (this.m_pickedUp && this.m_owner && PassiveItem.ActiveFlagItems != null && PassiveItem.ActiveFlagItems.ContainsKey(this.m_owner) && PassiveItem.ActiveFlagItems[this.m_owner].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[this.m_owner].Remove(base.GetType());
			}
		}
		if (this.m_owner != null)
		{
			this.m_owner.OnRollStarted -= this.OnRollStarted;
			PlayerController owner = this.m_owner;
			owner.OnBlinkShadowCreated = (Action<tk2dSprite>)Delegate.Remove(owner.OnBlinkShadowCreated, new Action<tk2dSprite>(this.OnBlinkCloneCreated));
			if (this.afterimage)
			{
				UnityEngine.Object.Destroy(this.afterimage);
			}
			this.afterimage = null;
		}
		base.OnDestroy();
	}

	// Token: 0x04006FC3 RID: 28611
	public bool ModifiesDodgeRoll;

	// Token: 0x04006FC4 RID: 28612
	[ShowInInspectorIf("ModifiesDodgeRoll", false)]
	public float DodgeRollTimeMultiplier = 0.9f;

	// Token: 0x04006FC5 RID: 28613
	[ShowInInspectorIf("ModifiesDodgeRoll", false)]
	public float DodgeRollDistanceMultiplier = 1.25f;

	// Token: 0x04006FC6 RID: 28614
	[ShowInInspectorIf("ModifiesDodgeRoll", false)]
	public int AdditionalInvulnerabilityFrames;

	// Token: 0x04006FC7 RID: 28615
	public ScarfAttachmentDoer ScarfPrefab;

	// Token: 0x04006FC8 RID: 28616
	public GameObject BlinkpoofVfx;

	// Token: 0x04006FC9 RID: 28617
	private ScarfAttachmentDoer m_scarf;

	// Token: 0x04006FCA RID: 28618
	private AfterImageTrailController afterimage;
}
