using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001246 RID: 4678
public class WarpPointHandler : BraveBehaviour
{
	// Token: 0x060068D7 RID: 26839 RVA: 0x00290EAC File Offset: 0x0028F0AC
	public void SetTarget(WarpPointHandler target)
	{
		this.warpTarget = (WarpPointHandler.WarpTargetType)(-1);
		this.ManuallyAssigned = true;
		this.m_targetWarper = target;
	}

	// Token: 0x060068D8 RID: 26840 RVA: 0x00290EC4 File Offset: 0x0028F0C4
	private IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		if (!this.ManuallyAssigned)
		{
			this.TryAcquirePairedWarp();
		}
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTriggerEntered));
		yield break;
	}

	// Token: 0x060068D9 RID: 26841 RVA: 0x00290EE0 File Offset: 0x0028F0E0
	private void TryAcquirePairedWarp()
	{
		WarpPointHandler[] array = UnityEngine.Object.FindObjectsOfType<WarpPointHandler>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].warpTarget == this.warpTarget && this != array[i])
			{
				this.m_targetWarper = array[i];
				break;
			}
		}
	}

	// Token: 0x060068DA RID: 26842 RVA: 0x00290F38 File Offset: 0x0028F138
	public Vector2 GetTargetPoint()
	{
		Vector2 vector = ((!this.ManuallyAssigned) ? this.m_targetWarper.specRigidbody.UnitCenter : this.m_targetWarper.specRigidbody.UnitBottomCenter);
		return vector + new Vector2(-0.5f, (!this.ManuallyAssigned) ? 0f : (-0.125f)) + this.spawnOffset + this.m_targetWarper.AdditionalSpawnOffset;
	}

	// Token: 0x060068DB RID: 26843 RVA: 0x00290FC0 File Offset: 0x0028F1C0
	private void HandleTriggerEntered(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (WarpPointHandler.m_justWarped)
		{
			return;
		}
		if (this.OnlyReceiver)
		{
			return;
		}
		if (this.DISABLED_TEMPORARILY)
		{
			return;
		}
		PlayerController component = specRigidbody.GetComponent<PlayerController>();
		if (component != null)
		{
			if (this.m_targetWarper == null)
			{
				this.TryAcquirePairedWarp();
			}
			if (this.m_targetWarper == null)
			{
				return;
			}
			Pixelator.Instance.StartCoroutine(this.HandleWarpCooldown(component));
		}
	}

	// Token: 0x060068DC RID: 26844 RVA: 0x00291040 File Offset: 0x0028F240
	private IEnumerator HandleWarpCooldown(PlayerController player)
	{
		WarpPointHandler.m_justWarped = true;
		if (this.OnPreWarp != null)
		{
			float additionalDelay = this.OnPreWarp(player);
			if (additionalDelay > 0f)
			{
				yield return new WaitForSeconds(additionalDelay);
			}
		}
		Pixelator.Instance.FadeToBlack(0.1f, false, 0f);
		yield return new WaitForSeconds(0.1f);
		player.SetInputOverride("arbitrary warp");
		if (this.OnWarping != null)
		{
			float additionalDelay2 = this.OnWarping(player);
			if (additionalDelay2 > 0f)
			{
				yield return new WaitForSeconds(additionalDelay2);
			}
		}
		if (this.OptionalCover)
		{
			this.OptionalCover.Break(-Vector2.up);
		}
		if (this.m_targetWarper.OptionalCover)
		{
			this.m_targetWarper.OptionalCover.Break(-Vector2.up);
		}
		Vector2 targetPoint = ((!this.ManuallyAssigned) ? this.m_targetWarper.specRigidbody.UnitCenter : this.m_targetWarper.specRigidbody.UnitBottomCenter);
		targetPoint = targetPoint + new Vector2(-0.5f, (!this.ManuallyAssigned) ? 0f : (-0.125f)) + this.spawnOffset + this.m_targetWarper.AdditionalSpawnOffset;
		Vector3 prevPlayerPosition = player.transform.position;
		player.WarpToPoint(targetPoint, false, false);
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(player);
			if (otherPlayer && otherPlayer.healthHaver.IsAlive)
			{
				otherPlayer.ReuniteWithOtherPlayer(player, false);
			}
		}
		GameManager.Instance.MainCameraController.ForceToPlayerPosition(player, prevPlayerPosition);
		if (this.OnWarpDone != null)
		{
			float additionalDelay3 = this.OnWarpDone(player);
			Pixelator.Instance.FadeToBlack(additionalDelay3 + 0.1f, true, 0f);
			if (additionalDelay3 > 0f)
			{
				yield return new WaitForSeconds(additionalDelay3);
			}
		}
		else
		{
			Pixelator.Instance.FadeToBlack(0.1f, true, 0f);
		}
		player.ClearInputOverride("arbitrary warp");
		yield return new WaitForSeconds(0.05f);
		WarpPointHandler.m_justWarped = false;
		yield break;
	}

	// Token: 0x060068DD RID: 26845 RVA: 0x00291064 File Offset: 0x0028F264
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04006529 RID: 25897
	[NonSerialized]
	public bool DISABLED_TEMPORARILY;

	// Token: 0x0400652A RID: 25898
	public WarpPointHandler.WarpTargetType warpTarget;

	// Token: 0x0400652B RID: 25899
	public bool OnlyReceiver;

	// Token: 0x0400652C RID: 25900
	public MajorBreakable OptionalCover;

	// Token: 0x0400652D RID: 25901
	public Vector2 AdditionalSpawnOffset;

	// Token: 0x0400652E RID: 25902
	[NonSerialized]
	public Vector2 spawnOffset = Vector2.zero;

	// Token: 0x0400652F RID: 25903
	[NonSerialized]
	public bool ManuallyAssigned;

	// Token: 0x04006530 RID: 25904
	public Func<PlayerController, float> OnPreWarp;

	// Token: 0x04006531 RID: 25905
	public Func<PlayerController, float> OnWarping;

	// Token: 0x04006532 RID: 25906
	public Func<PlayerController, float> OnWarpDone;

	// Token: 0x04006533 RID: 25907
	private WarpPointHandler m_targetWarper;

	// Token: 0x04006534 RID: 25908
	private static bool m_justWarped;

	// Token: 0x02001247 RID: 4679
	public enum WarpTargetType
	{
		// Token: 0x04006536 RID: 25910
		WARP_A,
		// Token: 0x04006537 RID: 25911
		WARP_B,
		// Token: 0x04006538 RID: 25912
		WARP_C,
		// Token: 0x04006539 RID: 25913
		WARP_D,
		// Token: 0x0400653A RID: 25914
		WARP_E,
		// Token: 0x0400653B RID: 25915
		WARP_F,
		// Token: 0x0400653C RID: 25916
		WARP_G,
		// Token: 0x0400653D RID: 25917
		WARP_H,
		// Token: 0x0400653E RID: 25918
		WARP_I
	}
}
