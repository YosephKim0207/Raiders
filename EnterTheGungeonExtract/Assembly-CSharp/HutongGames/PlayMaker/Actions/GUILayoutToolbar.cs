using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009E6 RID: 2534
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("GUILayout Toolbar. NOTE: Arrays must be the same length as NumButtons or empty.")]
	public class GUILayoutToolbar : GUILayoutAction
	{
		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06003672 RID: 13938 RVA: 0x001167CC File Offset: 0x001149CC
		public GUIContent[] Contents
		{
			get
			{
				if (this.contents == null)
				{
					this.SetButtonsContent();
				}
				return this.contents;
			}
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x001167E8 File Offset: 0x001149E8
		private void SetButtonsContent()
		{
			if (this.contents == null)
			{
				this.contents = new GUIContent[this.numButtons.Value];
			}
			for (int i = 0; i < this.numButtons.Value; i++)
			{
				this.contents[i] = new GUIContent();
			}
			for (int j = 0; j < this.imagesArray.Length; j++)
			{
				this.contents[j].image = this.imagesArray[j].Value;
			}
			for (int k = 0; k < this.textsArray.Length; k++)
			{
				this.contents[k].text = this.textsArray[k].Value;
			}
			for (int l = 0; l < this.tooltipsArray.Length; l++)
			{
				this.contents[l].tooltip = this.tooltipsArray[l].Value;
			}
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x001168D8 File Offset: 0x00114AD8
		public override void Reset()
		{
			base.Reset();
			this.numButtons = 0;
			this.selectedButton = null;
			this.buttonEventsArray = new FsmEvent[0];
			this.imagesArray = new FsmTexture[0];
			this.tooltipsArray = new FsmString[0];
			this.style = "Button";
			this.everyFrame = false;
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x0011693C File Offset: 0x00114B3C
		public override void OnEnter()
		{
			string text = this.ErrorCheck();
			if (!string.IsNullOrEmpty(text))
			{
				base.LogError(text);
				base.Finish();
			}
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x00116968 File Offset: 0x00114B68
		public override void OnGUI()
		{
			if (this.everyFrame)
			{
				this.SetButtonsContent();
			}
			bool changed = GUI.changed;
			GUI.changed = false;
			this.selectedButton.Value = GUILayout.Toolbar(this.selectedButton.Value, this.Contents, this.style.Value, base.LayoutOptions);
			if (GUI.changed)
			{
				if (this.selectedButton.Value < this.buttonEventsArray.Length)
				{
					base.Fsm.Event(this.buttonEventsArray[this.selectedButton.Value]);
					GUIUtility.ExitGUI();
				}
			}
			else
			{
				GUI.changed = changed;
			}
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x00116A18 File Offset: 0x00114C18
		public override string ErrorCheck()
		{
			string text = string.Empty;
			if (this.imagesArray.Length > 0 && this.imagesArray.Length != this.numButtons.Value)
			{
				text += "Images array doesn't match NumButtons.\n";
			}
			if (this.textsArray.Length > 0 && this.textsArray.Length != this.numButtons.Value)
			{
				text += "Texts array doesn't match NumButtons.\n";
			}
			if (this.tooltipsArray.Length > 0 && this.tooltipsArray.Length != this.numButtons.Value)
			{
				text += "Tooltips array doesn't match NumButtons.\n";
			}
			return text;
		}

		// Token: 0x040027CB RID: 10187
		[Tooltip("The number of buttons in the toolbar")]
		public FsmInt numButtons;

		// Token: 0x040027CC RID: 10188
		[Tooltip("Store the index of the selected button in an Integer Variable")]
		[UIHint(UIHint.Variable)]
		public FsmInt selectedButton;

		// Token: 0x040027CD RID: 10189
		[Tooltip("Event to send when each button is pressed.")]
		public FsmEvent[] buttonEventsArray;

		// Token: 0x040027CE RID: 10190
		[Tooltip("Image to use on each button.")]
		public FsmTexture[] imagesArray;

		// Token: 0x040027CF RID: 10191
		[Tooltip("Text to use on each button.")]
		public FsmString[] textsArray;

		// Token: 0x040027D0 RID: 10192
		[Tooltip("Tooltip to use for each button.")]
		public FsmString[] tooltipsArray;

		// Token: 0x040027D1 RID: 10193
		[Tooltip("A named GUIStyle to use for the toolbar buttons. Default is Button.")]
		public FsmString style;

		// Token: 0x040027D2 RID: 10194
		[Tooltip("Update the content of the buttons every frame. Useful if the buttons are using variables that change.")]
		public bool everyFrame;

		// Token: 0x040027D3 RID: 10195
		private GUIContent[] contents;
	}
}
