using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001759 RID: 5977
public class DefaultLabelController : BraveBehaviour
{
	// Token: 0x06008B24 RID: 35620 RVA: 0x0039F2A4 File Offset: 0x0039D4A4
	public void Trigger()
	{
		base.StartCoroutine(this.Expand_CR());
	}

	// Token: 0x06008B25 RID: 35621 RVA: 0x0039F2B4 File Offset: 0x0039D4B4
	public void Trigger(Transform aTarget, Vector3 anOffset)
	{
		this.offset = anOffset;
		this.targetObject = aTarget;
		this.Trigger();
	}

	// Token: 0x06008B26 RID: 35622 RVA: 0x0039F2CC File Offset: 0x0039D4CC
	private IEnumerator Expand_CR()
	{
		this.panel.Width = 1f;
		float elapsed = 0f;
		float duration = 0.3f;
		float targetWidth = this.label.Width + 1f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			this.panel.Width = Mathf.Lerp(1f, targetWidth, elapsed / duration);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008B27 RID: 35623 RVA: 0x0039F2E8 File Offset: 0x0039D4E8
	private void LateUpdate()
	{
		this.UpdatePosition();
		this.UpdateForLanguage();
	}

	// Token: 0x06008B28 RID: 35624 RVA: 0x0039F2F8 File Offset: 0x0039D4F8
	public void UpdateForLanguage()
	{
		if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.KOREAN)
		{
			this.label.Padding.top = 0;
		}
		else
		{
			this.label.Padding.top = -6;
		}
	}

	// Token: 0x06008B29 RID: 35625 RVA: 0x0039F360 File Offset: 0x0039D560
	public void UpdatePosition()
	{
		if (this.m_manager == null)
		{
			this.m_manager = this.panel.GetManager();
		}
		if (!this.targetObject)
		{
			return;
		}
		base.transform.position = dfFollowObject.ConvertWorldSpaces(this.targetObject.transform.position + this.offset, GameManager.Instance.MainCameraController.Camera, this.m_manager.RenderCamera).WithZ(0f);
		base.transform.position = base.transform.position.QuantizeFloor(this.panel.PixelsToUnits() / (Pixelator.Instance.ScaleTileScale / Pixelator.Instance.CurrentTileScale));
	}

	// Token: 0x06008B2A RID: 35626 RVA: 0x0039F42C File Offset: 0x0039D62C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040091F7 RID: 37367
	public dfLabel label;

	// Token: 0x040091F8 RID: 37368
	public dfPanel panel;

	// Token: 0x040091F9 RID: 37369
	public Transform targetObject;

	// Token: 0x040091FA RID: 37370
	public Vector3 offset;

	// Token: 0x040091FB RID: 37371
	private dfGUIManager m_manager;
}
