using System;
using UnityEngine;

// Token: 0x020017A7 RID: 6055
public class LoadingScreenController : MonoBehaviour
{
	// Token: 0x06008DB8 RID: 36280 RVA: 0x003BA080 File Offset: 0x003B8280
	private void Start()
	{
		this.ItemDescriptionLabel.SizeChanged += this.OnDescriptionLabelSizeChanged;
		this.ItemSprite.ignoresTiltworldDepth = true;
	}

	// Token: 0x06008DB9 RID: 36281 RVA: 0x003BA0A8 File Offset: 0x003B82A8
	private void Update()
	{
	}

	// Token: 0x06008DBA RID: 36282 RVA: 0x003BA0AC File Offset: 0x003B82AC
	private void OnDescriptionLabelSizeChanged(dfControl control, Vector2 value)
	{
		if (control == this.ItemDescriptionLabel)
		{
			Vector2 vector = this.ItemDescriptionLabel.Font.ObtainRenderer().MeasureString(this.ItemDescriptionLabel.Text);
			float num = (float)Mathf.CeilToInt(vector.x / this.ItemDescriptionLabel.Size.x) * vector.y;
			this.ItemDescriptionBox.Size = new Vector2(this.ItemDescriptionBox.Size.x, num + 66f);
			Vector2 vector2 = this.ItemNameLabel.Font.ObtainRenderer().MeasureString(this.ItemNameLabel.Text);
			this.ItemDescriptionBox.Size = new Vector2(Mathf.Max(vector2.x, this.ItemDescriptionBox.Size.x), this.ItemDescriptionBox.Size.y);
		}
	}

	// Token: 0x06008DBB RID: 36283 RVA: 0x003BA1A8 File Offset: 0x003B83A8
	public void ChangeToNewItem(tk2dBaseSprite sourceSprite, JournalEntry entry)
	{
	}

	// Token: 0x04009576 RID: 38262
	public tk2dBaseSprite DEBUG_SPRITE;

	// Token: 0x04009577 RID: 38263
	public dfSprite ItemBowlSprite;

	// Token: 0x04009578 RID: 38264
	public dfSprite ItemDescriptionBox;

	// Token: 0x04009579 RID: 38265
	public dfLabel ItemNameLabel;

	// Token: 0x0400957A RID: 38266
	public dfLabel ItemDescriptionLabel;

	// Token: 0x0400957B RID: 38267
	public tk2dBaseSprite ItemSprite;
}
