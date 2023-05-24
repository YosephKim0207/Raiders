using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001169 RID: 4457
public class FoyerCostumeController : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x060062FE RID: 25342 RVA: 0x002660BC File Offset: 0x002642BC
	private IEnumerator Start()
	{
		while (Foyer.DoIntroSequence || Foyer.DoMainMenu)
		{
			yield return null;
		}
		if (!GameStatsManager.Instance.GetFlag(this.RequiredFlag))
		{
			this.m_active = false;
			base.gameObject.SetActive(false);
		}
		else
		{
			this.m_active = true;
		}
		yield break;
	}

	// Token: 0x060062FF RID: 25343 RVA: 0x002660D8 File Offset: 0x002642D8
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x06006300 RID: 25344 RVA: 0x002660E4 File Offset: 0x002642E4
	public float GetDistanceToPoint(Vector2 point)
	{
		if (!this.m_active)
		{
			return 1000f;
		}
		return Vector2.Distance(point, base.sprite.WorldCenter);
	}

	// Token: 0x06006301 RID: 25345 RVA: 0x00266108 File Offset: 0x00264308
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06006302 RID: 25346 RVA: 0x00266110 File Offset: 0x00264310
	public void Interact(PlayerController interactor)
	{
		if (!this.m_active)
		{
			return;
		}
		if (interactor.IsUsingAlternateCostume)
		{
			if (interactor.AlternateCostumeLibrary == this.TargetLibrary)
			{
				interactor.SwapToAlternateCostume(null);
			}
			else
			{
				interactor.SwapToAlternateCostume(null);
				interactor.AlternateCostumeLibrary = this.TargetLibrary;
				interactor.SwapToAlternateCostume(null);
			}
		}
		else
		{
			if (this.TargetLibrary)
			{
				interactor.AlternateCostumeLibrary = this.TargetLibrary;
			}
			interactor.SwapToAlternateCostume(null);
		}
	}

	// Token: 0x06006303 RID: 25347 RVA: 0x00266198 File Offset: 0x00264398
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this.m_active)
		{
			return;
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
	}

	// Token: 0x06006304 RID: 25348 RVA: 0x002661B8 File Offset: 0x002643B8
	public void OnExitRange(PlayerController interactor)
	{
		if (!this.m_active)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
	}

	// Token: 0x04005E23 RID: 24099
	[LongEnum]
	public GungeonFlags RequiredFlag;

	// Token: 0x04005E24 RID: 24100
	public tk2dSpriteAnimation TargetLibrary;

	// Token: 0x04005E25 RID: 24101
	private bool m_active;
}
