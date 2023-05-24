using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020011C4 RID: 4548
public class NoteDoer : DungeonPlaceableBehaviour, IPlayerInteractable
{
	// Token: 0x06006572 RID: 25970 RVA: 0x002772F0 File Offset: 0x002754F0
	public void Start()
	{
		if (base.majorBreakable != null)
		{
			MajorBreakable majorBreakable = base.majorBreakable;
			majorBreakable.OnBreak = (Action)Delegate.Combine(majorBreakable.OnBreak, new Action(this.OnBroken));
		}
	}

	// Token: 0x06006573 RID: 25971 RVA: 0x0027732C File Offset: 0x0027552C
	private void OnBroken()
	{
		GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor)).DeregisterInteractable(this);
	}

	// Token: 0x06006574 RID: 25972 RVA: 0x0027735C File Offset: 0x0027555C
	private void OnDisable()
	{
		if (this.m_boxIsExtant)
		{
			GameUIRoot.Instance.ShowCoreUI(string.Empty);
			this.m_boxIsExtant = false;
			TextBoxManager.ClearTextBoxImmediate(this.textboxSpawnPoint);
		}
	}

	// Token: 0x06006575 RID: 25973 RVA: 0x0027738C File Offset: 0x0027558C
	public float GetDistanceToPoint(Vector2 point)
	{
		if (!base.sprite)
		{
			if (this.m_boxIsExtant)
			{
				this.ClearBox();
			}
			return 1000f;
		}
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x06006576 RID: 25974 RVA: 0x00277494 File Offset: 0x00275694
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06006577 RID: 25975 RVA: 0x0027749C File Offset: 0x0027569C
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x06006578 RID: 25976 RVA: 0x002774DC File Offset: 0x002756DC
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (this.m_boxIsExtant)
		{
			this.ClearBox();
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x06006579 RID: 25977 RVA: 0x00277538 File Offset: 0x00275738
	private void ClearBox()
	{
		GameUIRoot.Instance.ShowCoreUI(string.Empty);
		this.m_boxIsExtant = false;
		TextBoxManager.ClearTextBox(this.textboxSpawnPoint);
		if (this.DestroyedOnFinish)
		{
			base.GetAbsoluteParentRoom().DeregisterInteractable(this);
			RoomHandler.unassignedInteractableObjects.Remove(this);
			LootEngine.DoDefaultItemPoof(base.sprite.WorldCenter, false, false);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600657A RID: 25978 RVA: 0x002775A8 File Offset: 0x002757A8
	public void Interact(PlayerController interactor)
	{
		if (this.m_boxIsExtant)
		{
			this.ClearBox();
		}
		else
		{
			GameUIRoot.Instance.HideCoreUI(string.Empty);
			this.m_boxIsExtant = true;
			string text = this.m_selectedDisplayString;
			if (this.m_selectedDisplayString == null)
			{
				text = ((!this.alreadyLocalized) ? ((!this.useItemsTable) ? StringTableManager.GetLongString(this.stringKey) : StringTableManager.GetItemsLongDescription(this.stringKey)) : this.stringKey);
				if (this.useAdditionalStrings)
				{
					if (this.isNormalNote)
					{
						text = ((!this.alreadyLocalized) ? ((!this.useItemsTable) ? StringTableManager.GetLongString(this.additionalStrings[UnityEngine.Random.Range(0, this.additionalStrings.Length)]) : StringTableManager.GetItemsLongDescription(this.additionalStrings[UnityEngine.Random.Range(0, this.additionalStrings.Length)])) : this.additionalStrings[UnityEngine.Random.Range(0, this.additionalStrings.Length)]);
					}
					else if (GameStatsManager.Instance.GetFlag(GungeonFlags.SECRET_NOTE_ENCOUNTERED))
					{
						text = ((!this.alreadyLocalized) ? ((!this.useItemsTable) ? StringTableManager.GetLongString(this.additionalStrings[UnityEngine.Random.Range(0, this.additionalStrings.Length)]) : StringTableManager.GetItemsLongDescription(this.additionalStrings[UnityEngine.Random.Range(0, this.additionalStrings.Length)])) : this.additionalStrings[UnityEngine.Random.Range(0, this.additionalStrings.Length)]);
					}
				}
				if (this.stringKey == "#IRONCOIN_SHORTDESC")
				{
					text = " \n" + text + "\n ";
				}
				this.m_selectedDisplayString = text;
			}
			switch (this.noteBackgroundType)
			{
			case NoteDoer.NoteBackgroundType.LETTER:
				TextBoxManager.ShowLetterBox(this.textboxSpawnPoint.position, this.textboxSpawnPoint, -1f, text, true, false);
				break;
			case NoteDoer.NoteBackgroundType.STONE:
				TextBoxManager.ShowStoneTablet(this.textboxSpawnPoint.position, this.textboxSpawnPoint, -1f, text, true, false);
				break;
			case NoteDoer.NoteBackgroundType.WOOD:
				TextBoxManager.ShowWoodPanel(this.textboxSpawnPoint.position, this.textboxSpawnPoint, -1f, text, true, false);
				break;
			case NoteDoer.NoteBackgroundType.NOTE:
				TextBoxManager.ShowNote(this.textboxSpawnPoint.position, this.textboxSpawnPoint, -1f, text, true, false);
				break;
			}
			if (this.useAdditionalStrings && !this.isNormalNote)
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.SECRET_NOTE_ENCOUNTERED, true);
			}
		}
	}

	// Token: 0x0600657B RID: 25979 RVA: 0x0027783C File Offset: 0x00275A3C
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x0600657C RID: 25980 RVA: 0x00277848 File Offset: 0x00275A48
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400612B RID: 24875
	public NoteDoer.NoteBackgroundType noteBackgroundType;

	// Token: 0x0400612C RID: 24876
	public string stringKey;

	// Token: 0x0400612D RID: 24877
	public bool useAdditionalStrings;

	// Token: 0x0400612E RID: 24878
	public string[] additionalStrings;

	// Token: 0x0400612F RID: 24879
	public bool isNormalNote;

	// Token: 0x04006130 RID: 24880
	public bool useItemsTable;

	// Token: 0x04006131 RID: 24881
	[NonSerialized]
	public bool alreadyLocalized;

	// Token: 0x04006132 RID: 24882
	public Transform textboxSpawnPoint;

	// Token: 0x04006133 RID: 24883
	private bool m_boxIsExtant;

	// Token: 0x04006134 RID: 24884
	public bool DestroyedOnFinish;

	// Token: 0x04006135 RID: 24885
	private string m_selectedDisplayString;

	// Token: 0x020011C5 RID: 4549
	public enum NoteBackgroundType
	{
		// Token: 0x04006137 RID: 24887
		LETTER,
		// Token: 0x04006138 RID: 24888
		STONE,
		// Token: 0x04006139 RID: 24889
		WOOD,
		// Token: 0x0400613A RID: 24890
		NOTE
	}
}
