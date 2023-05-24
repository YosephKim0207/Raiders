using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200173D RID: 5949
public class AppearedInTheGungeonController : MonoBehaviour
{
	// Token: 0x06008A64 RID: 35428 RVA: 0x0039A268 File Offset: 0x00398468
	public void Appear(EncounterDatabaseEntry newPickup)
	{
		AkSoundEngine.PostEvent("Play_UI_card_open_01", base.gameObject);
		this.m_curTrackable = newPickup;
		dfPanel component = this.itemSprite.transform.parent.GetComponent<dfPanel>();
		tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.Instance.EncounterIconCollection;
		int num = encounterIconCollection.GetSpriteIdByName(newPickup.journalData.AmmonomiconSprite, 0);
		if (num < 0)
		{
			num = encounterIconCollection.GetSpriteIdByName(AmmonomiconController.AmmonomiconErrorSprite);
		}
		this.itemSprite.SetSprite(encounterIconCollection, num);
		this.itemSprite.transform.localScale = Vector3.one;
		Bounds untrimmedBounds = this.itemSprite.GetUntrimmedBounds();
		Vector2 vector = GameUIUtility.TK2DtoDF(untrimmedBounds.size.XY(), component.GUIManager.PixelsToUnits());
		this.itemSprite.scale = new Vector3(vector.x / untrimmedBounds.size.x, vector.y / untrimmedBounds.size.y, untrimmedBounds.size.z);
		this.itemSprite.ignoresTiltworldDepth = true;
		this.itemSprite.gameObject.SetLayerRecursively(LayerMask.NameToLayer("SecondaryGUI"));
		SpriteOutlineManager.AddScaledOutlineToSprite<tk2dSprite>(this.itemSprite, Color.black, 0.1f, 0.05f);
		this.itemSprite.PlaceAtLocalPositionByAnchor(Vector3.zero, tk2dBaseSprite.Anchor.MiddleCenter);
		for (int i = 0; i < this.itemNameLabels.Length; i++)
		{
			this.itemNameLabels[i].Text = newPickup.journalData.GetPrimaryDisplayName(false).ToUpperInvariant();
			float num2 = ((GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.JAPANESE) ? 1f : 3f);
			float num3 = 10f;
			if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.KOREAN || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE)
			{
				num3 = 6f;
			}
			if ((float)this.itemNameLabels[i].Text.Length > num3)
			{
				float num4 = ((float)this.itemNameLabels[i].Text.Length - num3) / num3;
				this.itemNameLabels[i].TextScale = Mathf.Lerp(2f, 1f, num4) * num2;
				this.itemNameLabels[i].RelativePosition = this.itemNameLabels[i].RelativePosition.WithY(Mathf.Lerp(51f, 72f, num4).Quantize(3f));
			}
			else
			{
				this.itemNameLabels[i].TextScale = 2f * num2;
				this.itemNameLabels[i].RelativePosition = this.itemNameLabels[i].RelativePosition.WithY(51f);
			}
		}
		this.itemNameLabels[0].PerformLayout();
		this.ShwoopOpen();
	}

	// Token: 0x06008A65 RID: 35429 RVA: 0x0039A548 File Offset: 0x00398748
	private void Update()
	{
		if (!AmmonomiconController.Instance.IsOpen && this.m_curTrackable != null && !this.m_isScalingDown)
		{
			this.ShwoopClosed();
			GameManager.Instance.AcknowledgeKnownTrackable(this.m_curTrackable);
			this.m_curTrackable = null;
		}
	}

	// Token: 0x06008A66 RID: 35430 RVA: 0x0039A598 File Offset: 0x00398798
	public void ShwoopOpen()
	{
		base.StartCoroutine(this.HandleShwoop(false));
	}

	// Token: 0x06008A67 RID: 35431 RVA: 0x0039A5A8 File Offset: 0x003987A8
	private IEnumerator HandleShwoop(bool reverse)
	{
		if (this.m_isScalingDown)
		{
			yield break;
		}
		if (reverse)
		{
			this.m_isScalingDown = true;
		}
		float timer = 0.15f;
		float elapsed = 0f;
		Vector3 smallScale = new Vector3(0.01f, 0.01f, 1f);
		Vector3 bigScale = Vector3.one;
		AnimationCurve targetCurve = ((!reverse) ? GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().ShwoopInCurve : GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().ShwoopOutCurve);
		dfPanel m_panel = base.GetComponent<dfPanel>();
		while (elapsed < timer)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = elapsed / timer;
			m_panel.transform.localScale = smallScale + bigScale * Mathf.Clamp01(targetCurve.Evaluate(t));
			m_panel.MakePixelPerfect();
			yield return null;
		}
		if (!reverse)
		{
			m_panel.transform.localScale = Vector3.one;
			m_panel.MakePixelPerfect();
		}
		if (reverse)
		{
			m_panel.IsVisible = false;
			m_panel.IsInteractive = false;
			m_panel.IsEnabled = false;
			this.m_isScalingDown = false;
			UnityEngine.Object.Destroy(m_panel.GUIManager.gameObject);
		}
		yield break;
	}

	// Token: 0x06008A68 RID: 35432 RVA: 0x0039A5CC File Offset: 0x003987CC
	public void ShwoopClosed()
	{
		base.StartCoroutine(this.HandleShwoop(true));
	}

	// Token: 0x040090E1 RID: 37089
	public dfLabel[] itemNameLabels;

	// Token: 0x040090E2 RID: 37090
	public tk2dSprite itemSprite;

	// Token: 0x040090E3 RID: 37091
	private EncounterDatabaseEntry m_curTrackable;

	// Token: 0x040090E4 RID: 37092
	private bool m_isScalingDown;
}
