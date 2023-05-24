using System;
using System.Collections;
using System.Text;
using Dungeonator;
using UnityEngine;

// Token: 0x0200116D RID: 4461
public class FoyerTimeHeroStatue : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x06006314 RID: 25364 RVA: 0x002664BC File Offset: 0x002646BC
	public IEnumerator Start()
	{
		yield return null;
		if (base.gameObject.activeSelf)
		{
			RoomHandler.unassignedInteractableObjects.Add(this);
		}
		yield break;
	}

	// Token: 0x06006315 RID: 25365 RVA: 0x002664D8 File Offset: 0x002646D8
	public float GetDistanceToPoint(Vector2 point)
	{
		if (base.sprite == null)
		{
			return 100f;
		}
		Vector3 vector = BraveMathCollege.ClosestPointOnRectangle(point, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions);
		return Vector2.Distance(point, vector) / 1.5f;
	}

	// Token: 0x06006316 RID: 25366 RVA: 0x00266530 File Offset: 0x00264730
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06006317 RID: 25367 RVA: 0x00266538 File Offset: 0x00264738
	public void OnEnteredRange(PlayerController interactor)
	{
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
	}

	// Token: 0x06006318 RID: 25368 RVA: 0x0026654C File Offset: 0x0026474C
	public void OnExitRange(PlayerController interactor)
	{
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		TextBoxManager.ClearTextBox(this.talkPoint);
	}

	// Token: 0x06006319 RID: 25369 RVA: 0x00266568 File Offset: 0x00264768
	public void Interact(PlayerController interactor)
	{
		if (TextBoxManager.HasTextBox(this.talkPoint))
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(StringTableManager.GetLongString(this.targetDisplayKey));
		stringBuilder.Append("\n");
		stringBuilder.Append("\n");
		stringBuilder.Append(StringTableManager.EvaluateReplacementToken("%BTCKTP_PRIMER") + " ");
		stringBuilder.Append(StringTableManager.EvaluateReplacementToken("%BTCKTP_POWDER") + " ");
		stringBuilder.Append(StringTableManager.EvaluateReplacementToken("%BTCKTP_SLUG") + " ");
		stringBuilder.Append(StringTableManager.EvaluateReplacementToken("%BTCKTP_CASING"));
		TextBoxManager.ShowStoneTablet(this.talkPoint.position, this.talkPoint, -1f, stringBuilder.ToString(), true, false);
		tk2dTextMesh[] componentsInChildren = this.talkPoint.GetComponentsInChildren<tk2dTextMesh>();
		if (componentsInChildren != null && componentsInChildren.Length > 0)
		{
			foreach (tk2dTextMesh tk2dTextMesh in componentsInChildren)
			{
				tk2dTextMesh.LineSpacing = -0.25f;
				tk2dTextMesh.transform.localPosition = tk2dTextMesh.transform.localPosition + new Vector3(0f, -0.375f, 0f);
				tk2dTextMesh.ForceBuild();
			}
		}
		tk2dBaseSprite[] componentsInChildren2 = this.talkPoint.GetComponentsInChildren<tk2dSprite>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			if (componentsInChildren2[j].CurrentSprite.name.StartsWith("forged_bullet"))
			{
				if (componentsInChildren2[j].CurrentSprite.name.Contains("primer"))
				{
					componentsInChildren2[j].renderer.material.SetFloat("_SaturationModifier", (float)((!GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_ELEMENT1)) ? 0 : 1));
				}
				if (componentsInChildren2[j].CurrentSprite.name.Contains("powder"))
				{
					componentsInChildren2[j].renderer.material.SetFloat("_SaturationModifier", (float)((!GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_ELEMENT2)) ? 0 : 1));
				}
				if (componentsInChildren2[j].CurrentSprite.name.Contains("slug"))
				{
					componentsInChildren2[j].renderer.material.SetFloat("_SaturationModifier", (float)((!GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_ELEMENT3)) ? 0 : 1));
				}
				if (componentsInChildren2[j].CurrentSprite.name.Contains("case"))
				{
					componentsInChildren2[j].renderer.material.SetFloat("_SaturationModifier", (float)((!GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_ELEMENT4)) ? 0 : 1));
				}
			}
		}
	}

	// Token: 0x0600631A RID: 25370 RVA: 0x00266840 File Offset: 0x00264A40
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x04005E37 RID: 24119
	public string targetDisplayKey;

	// Token: 0x04005E38 RID: 24120
	public Transform talkPoint;
}
