using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000491 RID: 1169
[AddComponentMenu("Daikon Forge/Examples/Touch/Display Events")]
public class TouchStateDisplayEvents : MonoBehaviour
{
	// Token: 0x06001ADB RID: 6875 RVA: 0x0007DA34 File Offset: 0x0007BC34
	public void Start()
	{
		if (this._label == null)
		{
			this._label = base.GetComponent<dfLabel>();
			this._label.Text = "Touch State";
		}
	}

	// Token: 0x06001ADC RID: 6876 RVA: 0x0007DA64 File Offset: 0x0007BC64
	public void OnDragDrop(dfControl control, dfDragEventArgs dragEvent)
	{
		string text = ((dragEvent.Data != null) ? dragEvent.Data.ToString() : "(null)");
		this.display("DragDrop: " + text);
		dragEvent.State = dfDragDropState.Dropped;
		dragEvent.Use();
	}

	// Token: 0x06001ADD RID: 6877 RVA: 0x0007DAB0 File Offset: 0x0007BCB0
	public void OnEnterFocus(dfControl control, dfFocusEventArgs args)
	{
		this.display("EnterFocus");
	}

	// Token: 0x06001ADE RID: 6878 RVA: 0x0007DAC0 File Offset: 0x0007BCC0
	public void OnLeaveFocus(dfControl control, dfFocusEventArgs args)
	{
		this.display("LeaveFocus");
	}

	// Token: 0x06001ADF RID: 6879 RVA: 0x0007DAD0 File Offset: 0x0007BCD0
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.display("Click");
	}

	// Token: 0x06001AE0 RID: 6880 RVA: 0x0007DAE0 File Offset: 0x0007BCE0
	public void OnDoubleClick(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.display("DoubleClick");
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x0007DAF0 File Offset: 0x0007BCF0
	public void OnMouseDown(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.display("MouseDown");
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x0007DB00 File Offset: 0x0007BD00
	public void OnMouseEnter(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.display("MouseEnter");
	}

	// Token: 0x06001AE3 RID: 6883 RVA: 0x0007DB10 File Offset: 0x0007BD10
	public void OnMouseLeave(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.display("MouseLeave");
	}

	// Token: 0x06001AE4 RID: 6884 RVA: 0x0007DB20 File Offset: 0x0007BD20
	public void OnMouseMove(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.display("MouseMove: " + this.screenToGUI(mouseEvent.Position));
	}

	// Token: 0x06001AE5 RID: 6885 RVA: 0x0007DB44 File Offset: 0x0007BD44
	public void OnMouseUp(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.display("MouseUp");
	}

	// Token: 0x06001AE6 RID: 6886 RVA: 0x0007DB54 File Offset: 0x0007BD54
	public void OnMultiTouch(dfControl control, dfTouchEventArgs touchData)
	{
		string text = "Multi-Touch:\n";
		for (int i = 0; i < touchData.Touches.Count; i++)
		{
			dfTouchInfo dfTouchInfo = touchData.Touches[i];
			text += string.Format("\tFinger {0}: {1}\n", i + 1, this.screenToGUI(dfTouchInfo.position));
		}
		this.display(text);
	}

	// Token: 0x06001AE7 RID: 6887 RVA: 0x0007DBC4 File Offset: 0x0007BDC4
	private void display(string text)
	{
		this.messages.Add(text);
		if (this.messages.Count > 6)
		{
			this.messages.RemoveAt(0);
		}
		this._label.Text = string.Join("\n", this.messages.ToArray());
	}

	// Token: 0x06001AE8 RID: 6888 RVA: 0x0007DC1C File Offset: 0x0007BE1C
	private Vector2 screenToGUI(Vector2 position)
	{
		position.y = this._label.GetManager().GetScreenSize().y - position.y;
		return position;
	}

	// Token: 0x0400152F RID: 5423
	public dfLabel _label;

	// Token: 0x04001530 RID: 5424
	private List<string> messages = new List<string>();
}
