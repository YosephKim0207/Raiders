using System;
using UnityEngine;

// Token: 0x02000477 RID: 1143
[ExecuteInEditMode]
[AddComponentMenu("Daikon Forge/Examples/General/Textbox Prompt")]
public class TextboxPrompt : MonoBehaviour
{
	// Token: 0x06001A43 RID: 6723 RVA: 0x0007A690 File Offset: 0x00078890
	public void OnEnable()
	{
		this._textbox = base.GetComponent<dfTextbox>();
		if (string.IsNullOrEmpty(this._textbox.Text) || this._textbox.Text == this.promptText)
		{
			this._textbox.Text = this.promptText;
			this._textbox.TextColor = this.promptColor;
		}
	}

	// Token: 0x06001A44 RID: 6724 RVA: 0x0007A6FC File Offset: 0x000788FC
	public void OnDisable()
	{
		if (this._textbox != null && this._textbox.Text == this.promptText)
		{
			this._textbox.Text = string.Empty;
		}
	}

	// Token: 0x06001A45 RID: 6725 RVA: 0x0007A73C File Offset: 0x0007893C
	public void OnEnterFocus(dfControl control, dfFocusEventArgs args)
	{
		if (this._textbox.Text == this.promptText)
		{
			this._textbox.Text = string.Empty;
		}
		this._textbox.TextColor = this.textColor;
	}

	// Token: 0x06001A46 RID: 6726 RVA: 0x0007A77C File Offset: 0x0007897C
	public void OnLeaveFocus(dfControl control, dfFocusEventArgs args)
	{
		if (string.IsNullOrEmpty(this._textbox.Text))
		{
			this._textbox.Text = this.promptText;
			this._textbox.TextColor = this.promptColor;
		}
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x0007A7B8 File Offset: 0x000789B8
	public void OnTextChanged(dfControl control, string value)
	{
		if (value != this.promptText)
		{
			this._textbox.TextColor = this.textColor;
		}
	}

	// Token: 0x04001498 RID: 5272
	public Color32 promptColor = Color.gray;

	// Token: 0x04001499 RID: 5273
	public Color32 textColor = Color.white;

	// Token: 0x0400149A RID: 5274
	public string promptText = "(enter some text)";

	// Token: 0x0400149B RID: 5275
	private dfTextbox _textbox;
}
