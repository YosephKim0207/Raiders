using System;
using UnityEngine;

// Token: 0x02000C0F RID: 3087
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/UI/tk2dUITextInput")]
public class tk2dUITextInput : MonoBehaviour
{
	// Token: 0x17000A00 RID: 2560
	// (get) Token: 0x06004203 RID: 16899 RVA: 0x00155D68 File Offset: 0x00153F68
	// (set) Token: 0x06004204 RID: 16900 RVA: 0x00155D70 File Offset: 0x00153F70
	public tk2dUILayout LayoutItem
	{
		get
		{
			return this.layoutItem;
		}
		set
		{
			if (this.layoutItem != value)
			{
				if (this.layoutItem != null)
				{
					this.layoutItem.OnReshape -= this.LayoutReshaped;
				}
				this.layoutItem = value;
				if (this.layoutItem != null)
				{
					this.layoutItem.OnReshape += this.LayoutReshaped;
				}
			}
		}
	}

	// Token: 0x17000A01 RID: 2561
	// (get) Token: 0x06004205 RID: 16901 RVA: 0x00155DE8 File Offset: 0x00153FE8
	// (set) Token: 0x06004206 RID: 16902 RVA: 0x00155E08 File Offset: 0x00154008
	public GameObject SendMessageTarget
	{
		get
		{
			if (this.selectionBtn != null)
			{
				return this.selectionBtn.sendMessageTarget;
			}
			return null;
		}
		set
		{
			if (this.selectionBtn != null && this.selectionBtn.sendMessageTarget != value)
			{
				this.selectionBtn.sendMessageTarget = value;
			}
		}
	}

	// Token: 0x17000A02 RID: 2562
	// (get) Token: 0x06004207 RID: 16903 RVA: 0x00155E40 File Offset: 0x00154040
	public bool IsFocus
	{
		get
		{
			return this.isSelected;
		}
	}

	// Token: 0x17000A03 RID: 2563
	// (get) Token: 0x06004208 RID: 16904 RVA: 0x00155E48 File Offset: 0x00154048
	// (set) Token: 0x06004209 RID: 16905 RVA: 0x00155E50 File Offset: 0x00154050
	public string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			if (this.text != value)
			{
				this.text = value;
				if (this.text.Length > this.maxCharacterLength)
				{
					this.text = this.text.Substring(0, this.maxCharacterLength);
				}
				this.FormatTextForDisplay(this.text);
				if (this.isSelected)
				{
					this.SetCursorPosition();
				}
			}
		}
	}

	// Token: 0x0600420A RID: 16906 RVA: 0x00155EC0 File Offset: 0x001540C0
	private void Awake()
	{
		this.SetState();
		this.ShowDisplayText();
	}

	// Token: 0x0600420B RID: 16907 RVA: 0x00155ED0 File Offset: 0x001540D0
	private void Start()
	{
		this.wasStartedCalled = true;
		if (tk2dUIManager.Instance__NoCreate != null)
		{
			tk2dUIManager.Instance.OnAnyPress += this.AnyPress;
		}
		this.wasOnAnyPressEventAttached = true;
	}

	// Token: 0x0600420C RID: 16908 RVA: 0x00155F08 File Offset: 0x00154108
	private void OnEnable()
	{
		if (this.wasStartedCalled && !this.wasOnAnyPressEventAttached && tk2dUIManager.Instance__NoCreate != null)
		{
			tk2dUIManager.Instance.OnAnyPress += this.AnyPress;
		}
		if (this.layoutItem != null)
		{
			this.layoutItem.OnReshape += this.LayoutReshaped;
		}
		this.selectionBtn.OnClick += this.InputSelected;
	}

	// Token: 0x0600420D RID: 16909 RVA: 0x00155F90 File Offset: 0x00154190
	private void OnDisable()
	{
		if (tk2dUIManager.Instance__NoCreate != null)
		{
			tk2dUIManager.Instance.OnAnyPress -= this.AnyPress;
			if (this.listenForKeyboardText)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.ListenForKeyboardTextUpdate;
			}
		}
		this.wasOnAnyPressEventAttached = false;
		this.selectionBtn.OnClick -= this.InputSelected;
		this.listenForKeyboardText = false;
		if (this.layoutItem != null)
		{
			this.layoutItem.OnReshape -= this.LayoutReshaped;
		}
	}

	// Token: 0x0600420E RID: 16910 RVA: 0x00156034 File Offset: 0x00154234
	public void SetFocus()
	{
		this.SetFocus(true);
	}

	// Token: 0x0600420F RID: 16911 RVA: 0x00156040 File Offset: 0x00154240
	public void SetFocus(bool focus)
	{
		if (!this.IsFocus && focus)
		{
			this.InputSelected();
		}
		else if (this.IsFocus && !focus)
		{
			this.InputDeselected();
		}
	}

	// Token: 0x06004210 RID: 16912 RVA: 0x00156078 File Offset: 0x00154278
	private void FormatTextForDisplay(string modifiedText)
	{
		if (this.isPasswordField)
		{
			int length = modifiedText.Length;
			char c = ((this.passwordChar.Length <= 0) ? '*' : this.passwordChar[0]);
			modifiedText = string.Empty;
			modifiedText = modifiedText.PadRight(length, c);
		}
		this.inputLabel.text = modifiedText;
		this.inputLabel.Commit();
		while (this.inputLabel.GetComponent<Renderer>().bounds.extents.x * 2f > this.fieldLength)
		{
			modifiedText = modifiedText.Substring(1, modifiedText.Length - 1);
			this.inputLabel.text = modifiedText;
			this.inputLabel.Commit();
		}
		if (modifiedText.Length == 0 && !this.listenForKeyboardText)
		{
			this.ShowDisplayText();
		}
		else
		{
			this.HideDisplayText();
		}
	}

	// Token: 0x06004211 RID: 16913 RVA: 0x0015616C File Offset: 0x0015436C
	private void ListenForKeyboardTextUpdate()
	{
		bool flag = false;
		string text = this.text;
		foreach (char c in Input.inputString)
		{
			if (c == "\b"[0])
			{
				if (this.text.Length != 0)
				{
					text = this.text.Substring(0, this.text.Length - 1);
					flag = true;
				}
			}
			else if (c != "\n"[0] && c != "\r"[0])
			{
				if (c != '\t' && c != '\u001b')
				{
					text += c;
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.Text = text;
			if (this.OnTextChange != null)
			{
				this.OnTextChange(this);
			}
			if (this.SendMessageTarget != null && this.SendMessageOnTextChangeMethodName.Length > 0)
			{
				this.SendMessageTarget.SendMessage(this.SendMessageOnTextChangeMethodName, this, SendMessageOptions.RequireReceiver);
			}
		}
	}

	// Token: 0x06004212 RID: 16914 RVA: 0x00156290 File Offset: 0x00154490
	private void InputSelected()
	{
		if (this.text.Length == 0)
		{
			this.HideDisplayText();
		}
		this.isSelected = true;
		if (!this.listenForKeyboardText)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.ListenForKeyboardTextUpdate;
		}
		this.listenForKeyboardText = true;
		this.SetState();
		this.SetCursorPosition();
	}

	// Token: 0x06004213 RID: 16915 RVA: 0x001562F0 File Offset: 0x001544F0
	private void InputDeselected()
	{
		if (this.text.Length == 0)
		{
			this.ShowDisplayText();
		}
		this.isSelected = false;
		if (this.listenForKeyboardText)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.ListenForKeyboardTextUpdate;
		}
		this.listenForKeyboardText = false;
		this.SetState();
	}

	// Token: 0x06004214 RID: 16916 RVA: 0x00156348 File Offset: 0x00154548
	private void AnyPress()
	{
		if (this.isSelected && tk2dUIManager.Instance.PressedUIItem != this.selectionBtn)
		{
			this.InputDeselected();
		}
	}

	// Token: 0x06004215 RID: 16917 RVA: 0x00156378 File Offset: 0x00154578
	private void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.unSelectedStateGO, !this.isSelected);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.selectedStateGO, this.isSelected);
		tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.cursor, this.isSelected);
	}

	// Token: 0x06004216 RID: 16918 RVA: 0x001563B0 File Offset: 0x001545B0
	private void SetCursorPosition()
	{
		float num = 1f;
		float num2 = 0.002f;
		if (this.inputLabel.anchor == TextAnchor.MiddleLeft || this.inputLabel.anchor == TextAnchor.LowerLeft || this.inputLabel.anchor == TextAnchor.UpperLeft)
		{
			num = 2f;
		}
		else if (this.inputLabel.anchor == TextAnchor.MiddleRight || this.inputLabel.anchor == TextAnchor.LowerRight || this.inputLabel.anchor == TextAnchor.UpperRight)
		{
			num = -2f;
			num2 = 0.012f;
		}
		if (this.text.EndsWith(" "))
		{
			tk2dFontChar tk2dFontChar;
			if (this.inputLabel.font.useDictionary)
			{
				tk2dFontChar = this.inputLabel.font.charDict[32];
			}
			else
			{
				tk2dFontChar = this.inputLabel.font.chars[32];
			}
			num2 += tk2dFontChar.advance * this.inputLabel.scale.x / 2f;
		}
		this.cursor.transform.localPosition = new Vector3(this.inputLabel.transform.localPosition.x + (this.inputLabel.GetComponent<Renderer>().bounds.extents.x + num2) * num, this.cursor.transform.localPosition.y, this.cursor.transform.localPosition.z);
	}

	// Token: 0x06004217 RID: 16919 RVA: 0x0015654C File Offset: 0x0015474C
	private void ShowDisplayText()
	{
		if (!this.isDisplayTextShown)
		{
			this.isDisplayTextShown = true;
			if (this.emptyDisplayLabel != null)
			{
				this.emptyDisplayLabel.text = this.emptyDisplayText;
				this.emptyDisplayLabel.Commit();
				tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.emptyDisplayLabel.gameObject, true);
			}
			tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.inputLabel.gameObject, false);
		}
	}

	// Token: 0x06004218 RID: 16920 RVA: 0x001565BC File Offset: 0x001547BC
	private void HideDisplayText()
	{
		if (this.isDisplayTextShown)
		{
			this.isDisplayTextShown = false;
			tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.emptyDisplayLabel.gameObject, false);
			tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.inputLabel.gameObject, true);
		}
	}

	// Token: 0x06004219 RID: 16921 RVA: 0x001565F4 File Offset: 0x001547F4
	private void LayoutReshaped(Vector3 dMin, Vector3 dMax)
	{
		this.fieldLength += dMax.x - dMin.x;
		string text = this.text;
		this.text = string.Empty;
		this.Text = text;
	}

	// Token: 0x04003484 RID: 13444
	public tk2dUIItem selectionBtn;

	// Token: 0x04003485 RID: 13445
	public tk2dTextMesh inputLabel;

	// Token: 0x04003486 RID: 13446
	public tk2dTextMesh emptyDisplayLabel;

	// Token: 0x04003487 RID: 13447
	public GameObject unSelectedStateGO;

	// Token: 0x04003488 RID: 13448
	public GameObject selectedStateGO;

	// Token: 0x04003489 RID: 13449
	public GameObject cursor;

	// Token: 0x0400348A RID: 13450
	public float fieldLength = 1f;

	// Token: 0x0400348B RID: 13451
	public int maxCharacterLength = 30;

	// Token: 0x0400348C RID: 13452
	public string emptyDisplayText;

	// Token: 0x0400348D RID: 13453
	public bool isPasswordField;

	// Token: 0x0400348E RID: 13454
	public string passwordChar = "*";

	// Token: 0x0400348F RID: 13455
	[HideInInspector]
	[SerializeField]
	private tk2dUILayout layoutItem;

	// Token: 0x04003490 RID: 13456
	private bool isSelected;

	// Token: 0x04003491 RID: 13457
	private bool wasStartedCalled;

	// Token: 0x04003492 RID: 13458
	private bool wasOnAnyPressEventAttached;

	// Token: 0x04003493 RID: 13459
	private bool listenForKeyboardText;

	// Token: 0x04003494 RID: 13460
	private bool isDisplayTextShown;

	// Token: 0x04003495 RID: 13461
	public Action<tk2dUITextInput> OnTextChange;

	// Token: 0x04003496 RID: 13462
	public string SendMessageOnTextChangeMethodName = string.Empty;

	// Token: 0x04003497 RID: 13463
	private string text = string.Empty;
}
