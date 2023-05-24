using System;
using UnityEngine;

// Token: 0x02001122 RID: 4386
public class CharacterCostumeSwapper : MonoBehaviour, IPlayerInteractable
{
	// Token: 0x060060CE RID: 24782 RVA: 0x00253E20 File Offset: 0x00252020
	private void Start()
	{
		bool flag = GameStatsManager.Instance.GetCharacterSpecificFlag(this.TargetCharacter, CharacterSpecificGungeonFlags.KILLED_PAST);
		if (this.HasCustomTrigger)
		{
			if (this.CustomTriggerIsFlag)
			{
				flag = GameStatsManager.Instance.GetFlag(this.TriggerFlag);
			}
			else if (this.CustomTriggerIsSpecialReserve)
			{
				flag = GameStatsManager.Instance.GetFlag(GungeonFlags.SECRET_BULLETMAN_SEEN_05) && false;
			}
		}
		if (flag)
		{
			this.m_active = true;
			if (this.TargetCharacter == PlayableCharacters.Guide)
			{
				this.CostumeSprite.HeightOffGround = 0.25f;
				this.AlternateCostumeSprite.HeightOffGround = 0.25f;
				this.CostumeSprite.UpdateZDepth();
				this.AlternateCostumeSprite.UpdateZDepth();
			}
			this.AlternateCostumeSprite.renderer.enabled = true;
			this.CostumeSprite.renderer.enabled = false;
		}
		else
		{
			this.m_active = false;
			this.AlternateCostumeSprite.renderer.enabled = false;
			this.CostumeSprite.renderer.enabled = false;
		}
	}

	// Token: 0x060060CF RID: 24783 RVA: 0x00253F38 File Offset: 0x00252138
	private void Update()
	{
		if (this.m_active)
		{
			if (GameManager.IsReturningToBreach)
			{
				return;
			}
			if (GameManager.Instance.IsSelectingCharacter)
			{
				return;
			}
			if (GameManager.Instance.IsLoadingLevel)
			{
				return;
			}
			if (GameManager.Instance.PrimaryPlayer == null)
			{
				return;
			}
			if (this.TargetCharacter != PlayableCharacters.CoopCultist && GameManager.Instance.PrimaryPlayer.characterIdentity != this.TargetCharacter)
			{
				SpriteOutlineManager.RemoveOutlineFromSprite(this.AlternateCostumeSprite, false);
				SpriteOutlineManager.RemoveOutlineFromSprite(this.CostumeSprite, false);
				this.AlternateCostumeSprite.renderer.enabled = true;
				this.CostumeSprite.renderer.enabled = false;
			}
		}
	}

	// Token: 0x060060D0 RID: 24784 RVA: 0x00253FF4 File Offset: 0x002521F4
	public float GetDistanceToPoint(Vector2 point)
	{
		if (!this.m_active)
		{
			return 1000f;
		}
		if (this.AlternateCostumeSprite.renderer.enabled)
		{
			return Vector2.Distance(point, this.AlternateCostumeSprite.WorldCenter);
		}
		return Vector2.Distance(point, this.CostumeSprite.WorldCenter);
	}

	// Token: 0x060060D1 RID: 24785 RVA: 0x0025404C File Offset: 0x0025224C
	public void OnEnteredRange(PlayerController interactor)
	{
		if (interactor.characterIdentity != this.TargetCharacter)
		{
			return;
		}
		if (this.AlternateCostumeSprite.renderer.enabled)
		{
			SpriteOutlineManager.AddOutlineToSprite(this.AlternateCostumeSprite, Color.white);
		}
		else
		{
			SpriteOutlineManager.AddOutlineToSprite(this.CostumeSprite, Color.white);
		}
	}

	// Token: 0x060060D2 RID: 24786 RVA: 0x002540A8 File Offset: 0x002522A8
	public void OnExitRange(PlayerController interactor)
	{
		if (interactor.characterIdentity != this.TargetCharacter)
		{
			return;
		}
		if (this.AlternateCostumeSprite.renderer.enabled)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(this.AlternateCostumeSprite, false);
		}
		else
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(this.CostumeSprite, false);
		}
	}

	// Token: 0x060060D3 RID: 24787 RVA: 0x002540FC File Offset: 0x002522FC
	public void Interact(PlayerController interactor)
	{
		if (interactor.characterIdentity != this.TargetCharacter)
		{
			return;
		}
		if (!this.m_active)
		{
			return;
		}
		if (interactor.IsUsingAlternateCostume)
		{
			interactor.SwapToAlternateCostume(null);
		}
		else
		{
			if (this.TargetLibrary)
			{
				interactor.AlternateCostumeLibrary = this.TargetLibrary;
			}
			interactor.SwapToAlternateCostume(null);
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(this.AlternateCostumeSprite, false);
		SpriteOutlineManager.RemoveOutlineFromSprite(this.CostumeSprite, false);
		this.AlternateCostumeSprite.renderer.enabled = !this.AlternateCostumeSprite.renderer.enabled;
		this.CostumeSprite.renderer.enabled = !this.CostumeSprite.renderer.enabled;
	}

	// Token: 0x060060D4 RID: 24788 RVA: 0x002541C0 File Offset: 0x002523C0
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060060D5 RID: 24789 RVA: 0x002541CC File Offset: 0x002523CC
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x04005B70 RID: 23408
	public PlayableCharacters TargetCharacter;

	// Token: 0x04005B71 RID: 23409
	public tk2dSprite CostumeSprite;

	// Token: 0x04005B72 RID: 23410
	public tk2dSprite AlternateCostumeSprite;

	// Token: 0x04005B73 RID: 23411
	public tk2dSpriteAnimation TargetLibrary;

	// Token: 0x04005B74 RID: 23412
	public bool HasCustomTrigger;

	// Token: 0x04005B75 RID: 23413
	public bool CustomTriggerIsFlag;

	// Token: 0x04005B76 RID: 23414
	public GungeonFlags TriggerFlag;

	// Token: 0x04005B77 RID: 23415
	public bool CustomTriggerIsSpecialReserve;

	// Token: 0x04005B78 RID: 23416
	private bool m_active;
}
