using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

// Token: 0x02000404 RID: 1028
[dfCategory("Basic Controls")]
[dfTooltip("Implements a text entry control")]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_textbox.html")]
[AddComponentMenu("Daikon Forge/User Interface/Textbox")]
[ExecuteInEditMode]
[Serializable]
public class dfTextbox : dfInteractiveBase, IDFMultiRender, IRendersText
{
	// Token: 0x14000040 RID: 64
	// (add) Token: 0x060016C2 RID: 5826 RVA: 0x0006BDE4 File Offset: 0x00069FE4
	// (remove) Token: 0x060016C3 RID: 5827 RVA: 0x0006BE1C File Offset: 0x0006A01C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<bool> ReadOnlyChanged;

	// Token: 0x14000041 RID: 65
	// (add) Token: 0x060016C4 RID: 5828 RVA: 0x0006BE54 File Offset: 0x0006A054
	// (remove) Token: 0x060016C5 RID: 5829 RVA: 0x0006BE8C File Offset: 0x0006A08C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<string> PasswordCharacterChanged;

	// Token: 0x14000042 RID: 66
	// (add) Token: 0x060016C6 RID: 5830 RVA: 0x0006BEC4 File Offset: 0x0006A0C4
	// (remove) Token: 0x060016C7 RID: 5831 RVA: 0x0006BEFC File Offset: 0x0006A0FC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<string> TextChanged;

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x060016C8 RID: 5832 RVA: 0x0006BF34 File Offset: 0x0006A134
	// (remove) Token: 0x060016C9 RID: 5833 RVA: 0x0006BF6C File Offset: 0x0006A16C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<string> TextSubmitted;

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x060016CA RID: 5834 RVA: 0x0006BFA4 File Offset: 0x0006A1A4
	// (remove) Token: 0x060016CB RID: 5835 RVA: 0x0006BFDC File Offset: 0x0006A1DC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<string> TextCancelled;

	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x060016CC RID: 5836 RVA: 0x0006C014 File Offset: 0x0006A214
	// (set) Token: 0x060016CD RID: 5837 RVA: 0x0006C058 File Offset: 0x0006A258
	public dfFontBase Font
	{
		get
		{
			if (this.font == null)
			{
				dfGUIManager manager = base.GetManager();
				if (manager != null)
				{
					this.font = manager.DefaultFont;
				}
			}
			return this.font;
		}
		set
		{
			if (value != this.font)
			{
				this.unbindTextureRebuildCallback();
				this.font = value;
				this.bindTextureRebuildCallback();
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x060016CE RID: 5838 RVA: 0x0006C084 File Offset: 0x0006A284
	// (set) Token: 0x060016CF RID: 5839 RVA: 0x0006C08C File Offset: 0x0006A28C
	public int SelectionStart
	{
		get
		{
			return this.selectionStart;
		}
		set
		{
			if (value != this.selectionStart)
			{
				this.selectionStart = Mathf.Max(0, Mathf.Min(value, this.text.Length));
				this.selectionEnd = Mathf.Max(this.selectionEnd, this.selectionStart);
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x060016D0 RID: 5840 RVA: 0x0006C0E0 File Offset: 0x0006A2E0
	// (set) Token: 0x060016D1 RID: 5841 RVA: 0x0006C0E8 File Offset: 0x0006A2E8
	public int SelectionEnd
	{
		get
		{
			return this.selectionEnd;
		}
		set
		{
			if (value != this.selectionEnd)
			{
				this.selectionEnd = Mathf.Max(0, Mathf.Min(value, this.text.Length));
				this.selectionStart = Mathf.Max(this.selectionStart, this.selectionEnd);
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x060016D2 RID: 5842 RVA: 0x0006C13C File Offset: 0x0006A33C
	public int SelectionLength
	{
		get
		{
			return this.selectionEnd - this.selectionStart;
		}
	}

	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x060016D3 RID: 5843 RVA: 0x0006C14C File Offset: 0x0006A34C
	public string SelectedText
	{
		get
		{
			if (this.selectionEnd == this.selectionStart)
			{
				return string.Empty;
			}
			return this.text.Substring(this.selectionStart, this.selectionEnd - this.selectionStart);
		}
	}

	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x060016D4 RID: 5844 RVA: 0x0006C184 File Offset: 0x0006A384
	// (set) Token: 0x060016D5 RID: 5845 RVA: 0x0006C18C File Offset: 0x0006A38C
	public bool SelectOnFocus
	{
		get
		{
			return this.selectOnFocus;
		}
		set
		{
			this.selectOnFocus = value;
		}
	}

	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x060016D6 RID: 5846 RVA: 0x0006C198 File Offset: 0x0006A398
	// (set) Token: 0x060016D7 RID: 5847 RVA: 0x0006C1B8 File Offset: 0x0006A3B8
	public RectOffset Padding
	{
		get
		{
			if (this.padding == null)
			{
				this.padding = new RectOffset();
			}
			return this.padding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.padding))
			{
				this.padding = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x060016D8 RID: 5848 RVA: 0x0006C1E0 File Offset: 0x0006A3E0
	// (set) Token: 0x060016D9 RID: 5849 RVA: 0x0006C1E8 File Offset: 0x0006A3E8
	public bool IsPasswordField
	{
		get
		{
			return this.displayAsPassword;
		}
		set
		{
			if (value != this.displayAsPassword)
			{
				this.displayAsPassword = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x060016DA RID: 5850 RVA: 0x0006C204 File Offset: 0x0006A404
	// (set) Token: 0x060016DB RID: 5851 RVA: 0x0006C20C File Offset: 0x0006A40C
	public string PasswordCharacter
	{
		get
		{
			return this.passwordChar;
		}
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				this.passwordChar = value[0].ToString();
			}
			else
			{
				this.passwordChar = value;
			}
			this.OnPasswordCharacterChanged();
			this.Invalidate();
		}
	}

	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x060016DC RID: 5852 RVA: 0x0006C258 File Offset: 0x0006A458
	// (set) Token: 0x060016DD RID: 5853 RVA: 0x0006C260 File Offset: 0x0006A460
	public float CursorBlinkTime
	{
		get
		{
			return this.cursorBlinkTime;
		}
		set
		{
			this.cursorBlinkTime = value;
		}
	}

	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x060016DE RID: 5854 RVA: 0x0006C26C File Offset: 0x0006A46C
	// (set) Token: 0x060016DF RID: 5855 RVA: 0x0006C274 File Offset: 0x0006A474
	public int CursorWidth
	{
		get
		{
			return this.cursorWidth;
		}
		set
		{
			this.cursorWidth = value;
		}
	}

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x060016E0 RID: 5856 RVA: 0x0006C280 File Offset: 0x0006A480
	// (set) Token: 0x060016E1 RID: 5857 RVA: 0x0006C288 File Offset: 0x0006A488
	public int CursorIndex
	{
		get
		{
			return this.cursorIndex;
		}
		set
		{
			this.setCursorPos(value);
		}
	}

	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x060016E2 RID: 5858 RVA: 0x0006C294 File Offset: 0x0006A494
	// (set) Token: 0x060016E3 RID: 5859 RVA: 0x0006C29C File Offset: 0x0006A49C
	public bool ReadOnly
	{
		get
		{
			return this.readOnly;
		}
		set
		{
			if (value != this.readOnly)
			{
				this.readOnly = value;
				this.OnReadOnlyChanged();
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x060016E4 RID: 5860 RVA: 0x0006C2C0 File Offset: 0x0006A4C0
	// (set) Token: 0x060016E5 RID: 5861 RVA: 0x0006C2C8 File Offset: 0x0006A4C8
	public string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			value = value ?? string.Empty;
			if (value.Length > this.MaxLength)
			{
				value = value.Substring(0, this.MaxLength);
			}
			value = value.Replace("\t", " ");
			if (value != this.text)
			{
				this.text = value;
				this.scrollIndex = (this.cursorIndex = 0);
				this.OnTextChanged();
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x060016E6 RID: 5862 RVA: 0x0006C34C File Offset: 0x0006A54C
	// (set) Token: 0x060016E7 RID: 5863 RVA: 0x0006C354 File Offset: 0x0006A554
	public Color32 TextColor
	{
		get
		{
			return this.textColor;
		}
		set
		{
			this.textColor = value;
			this.Invalidate();
		}
	}

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x060016E8 RID: 5864 RVA: 0x0006C364 File Offset: 0x0006A564
	// (set) Token: 0x060016E9 RID: 5865 RVA: 0x0006C36C File Offset: 0x0006A56C
	public string SelectionSprite
	{
		get
		{
			return this.selectionSprite;
		}
		set
		{
			if (value != this.selectionSprite)
			{
				this.selectionSprite = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x060016EA RID: 5866 RVA: 0x0006C38C File Offset: 0x0006A58C
	// (set) Token: 0x060016EB RID: 5867 RVA: 0x0006C394 File Offset: 0x0006A594
	public Color32 SelectionBackgroundColor
	{
		get
		{
			return this.selectionBackground;
		}
		set
		{
			this.selectionBackground = value;
			this.Invalidate();
		}
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x060016EC RID: 5868 RVA: 0x0006C3A4 File Offset: 0x0006A5A4
	// (set) Token: 0x060016ED RID: 5869 RVA: 0x0006C3AC File Offset: 0x0006A5AC
	public Color32 CursorColor
	{
		get
		{
			return this.cursorColor;
		}
		set
		{
			this.cursorColor = value;
			this.Invalidate();
		}
	}

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x060016EE RID: 5870 RVA: 0x0006C3BC File Offset: 0x0006A5BC
	// (set) Token: 0x060016EF RID: 5871 RVA: 0x0006C3C4 File Offset: 0x0006A5C4
	public float TextScale
	{
		get
		{
			return this.textScale;
		}
		set
		{
			value = Mathf.Max(0.1f, value);
			if (!Mathf.Approximately(this.textScale, value))
			{
				dfFontManager.Invalidate(this.Font);
				this.textScale = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x060016F0 RID: 5872 RVA: 0x0006C3FC File Offset: 0x0006A5FC
	// (set) Token: 0x060016F1 RID: 5873 RVA: 0x0006C404 File Offset: 0x0006A604
	public dfTextScaleMode TextScaleMode
	{
		get
		{
			return this.textScaleMode;
		}
		set
		{
			this.textScaleMode = value;
			this.Invalidate();
		}
	}

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x060016F2 RID: 5874 RVA: 0x0006C414 File Offset: 0x0006A614
	// (set) Token: 0x060016F3 RID: 5875 RVA: 0x0006C41C File Offset: 0x0006A61C
	public int MaxLength
	{
		get
		{
			return this.maxLength;
		}
		set
		{
			if (value != this.maxLength)
			{
				this.maxLength = Mathf.Max(0, value);
				if (this.maxLength < this.text.Length)
				{
					this.Text = this.text.Substring(0, this.maxLength);
				}
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x060016F4 RID: 5876 RVA: 0x0006C478 File Offset: 0x0006A678
	// (set) Token: 0x060016F5 RID: 5877 RVA: 0x0006C480 File Offset: 0x0006A680
	public TextAlignment TextAlignment
	{
		get
		{
			return this.textAlign;
		}
		set
		{
			if (value != this.textAlign)
			{
				this.textAlign = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x060016F6 RID: 5878 RVA: 0x0006C49C File Offset: 0x0006A69C
	// (set) Token: 0x060016F7 RID: 5879 RVA: 0x0006C4A4 File Offset: 0x0006A6A4
	public bool Shadow
	{
		get
		{
			return this.shadow;
		}
		set
		{
			if (value != this.shadow)
			{
				this.shadow = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x060016F8 RID: 5880 RVA: 0x0006C4C0 File Offset: 0x0006A6C0
	// (set) Token: 0x060016F9 RID: 5881 RVA: 0x0006C4C8 File Offset: 0x0006A6C8
	public Color32 ShadowColor
	{
		get
		{
			return this.shadowColor;
		}
		set
		{
			if (!value.Equals(this.shadowColor))
			{
				this.shadowColor = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x060016FA RID: 5882 RVA: 0x0006C4F4 File Offset: 0x0006A6F4
	// (set) Token: 0x060016FB RID: 5883 RVA: 0x0006C4FC File Offset: 0x0006A6FC
	public Vector2 ShadowOffset
	{
		get
		{
			return this.shadowOffset;
		}
		set
		{
			if (value != this.shadowOffset)
			{
				this.shadowOffset = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x060016FC RID: 5884 RVA: 0x0006C51C File Offset: 0x0006A71C
	// (set) Token: 0x060016FD RID: 5885 RVA: 0x0006C524 File Offset: 0x0006A724
	public bool UseMobileKeyboard
	{
		get
		{
			return this.useMobileKeyboard;
		}
		set
		{
			this.useMobileKeyboard = value;
		}
	}

	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x060016FE RID: 5886 RVA: 0x0006C530 File Offset: 0x0006A730
	// (set) Token: 0x060016FF RID: 5887 RVA: 0x0006C538 File Offset: 0x0006A738
	public bool MobileAutoCorrect
	{
		get
		{
			return this.mobileAutoCorrect;
		}
		set
		{
			this.mobileAutoCorrect = value;
		}
	}

	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x06001700 RID: 5888 RVA: 0x0006C544 File Offset: 0x0006A744
	// (set) Token: 0x06001701 RID: 5889 RVA: 0x0006C54C File Offset: 0x0006A74C
	public bool HideMobileInputField
	{
		get
		{
			return this.mobileHideInputField;
		}
		set
		{
			this.mobileHideInputField = value;
		}
	}

	// Token: 0x17000502 RID: 1282
	// (get) Token: 0x06001702 RID: 5890 RVA: 0x0006C558 File Offset: 0x0006A758
	// (set) Token: 0x06001703 RID: 5891 RVA: 0x0006C560 File Offset: 0x0006A760
	public dfMobileKeyboardTrigger MobileKeyboardTrigger
	{
		get
		{
			return this.mobileKeyboardTrigger;
		}
		set
		{
			this.mobileKeyboardTrigger = value;
		}
	}

	// Token: 0x06001704 RID: 5892 RVA: 0x0006C56C File Offset: 0x0006A76C
	protected override void OnTabKeyPressed(dfKeyEventArgs args)
	{
		if (this.acceptsTab)
		{
			base.OnKeyPress(args);
			if (args.Used)
			{
				return;
			}
			args.Character = '\t';
			this.processKeyPress(args);
		}
		else
		{
			base.OnTabKeyPressed(args);
		}
	}

	// Token: 0x06001705 RID: 5893 RVA: 0x0006C5A8 File Offset: 0x0006A7A8
	protected internal override void OnKeyPress(dfKeyEventArgs args)
	{
		if (this.ReadOnly || char.IsControl(args.Character))
		{
			base.OnKeyPress(args);
			return;
		}
		base.OnKeyPress(args);
		if (args.Used)
		{
			return;
		}
		this.processKeyPress(args);
	}

	// Token: 0x06001706 RID: 5894 RVA: 0x0006C5E8 File Offset: 0x0006A7E8
	private void processKeyPress(dfKeyEventArgs args)
	{
		this.DeleteSelection();
		if (this.text.Length < this.MaxLength)
		{
			if (this.cursorIndex == this.text.Length)
			{
				this.text += args.Character;
			}
			else
			{
				this.text = this.text.Insert(this.cursorIndex, args.Character.ToString());
			}
			this.cursorIndex++;
			this.OnTextChanged();
			this.Invalidate();
		}
		args.Use();
	}

	// Token: 0x06001707 RID: 5895 RVA: 0x0006C694 File Offset: 0x0006A894
	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		if (this.ReadOnly)
		{
			return;
		}
		base.OnKeyDown(args);
		if (args.Used)
		{
			return;
		}
		KeyCode keyCode = args.KeyCode;
		switch (keyCode)
		{
		case KeyCode.RightArrow:
			if (args.Control)
			{
				if (args.Shift)
				{
					this.moveSelectionPointRightWord();
				}
				else
				{
					this.MoveCursorToNextWord();
				}
			}
			else if (args.Shift)
			{
				this.moveSelectionPointRight();
			}
			else
			{
				this.MoveCursorToNextChar();
			}
			break;
		case KeyCode.LeftArrow:
			if (args.Control)
			{
				if (args.Shift)
				{
					this.moveSelectionPointLeftWord();
				}
				else
				{
					this.MoveCursorToPreviousWord();
				}
			}
			else if (args.Shift)
			{
				this.moveSelectionPointLeft();
			}
			else
			{
				this.MoveCursorToPreviousChar();
			}
			break;
		case KeyCode.Insert:
			if (args.Shift)
			{
				string clipBoard = dfClipboardHelper.clipBoard;
				if (!string.IsNullOrEmpty(clipBoard))
				{
					this.PasteAtCursor(clipBoard);
				}
			}
			break;
		case KeyCode.Home:
			if (args.Shift)
			{
				this.SelectToStart();
			}
			else
			{
				this.MoveCursorToStart();
			}
			break;
		case KeyCode.End:
			if (args.Shift)
			{
				this.SelectToEnd();
			}
			else
			{
				this.MoveCursorToEnd();
			}
			break;
		default:
			switch (keyCode)
			{
			case KeyCode.A:
				if (args.Control)
				{
					this.SelectAll();
				}
				break;
			default:
				switch (keyCode)
				{
				case KeyCode.V:
					if (args.Control)
					{
						string clipBoard2 = dfClipboardHelper.clipBoard;
						if (!string.IsNullOrEmpty(clipBoard2))
						{
							this.PasteAtCursor(clipBoard2);
						}
					}
					break;
				default:
					if (keyCode != KeyCode.Backspace)
					{
						if (keyCode != KeyCode.Return)
						{
							if (keyCode != KeyCode.Escape)
							{
								if (keyCode != KeyCode.Delete)
								{
									base.OnKeyDown(args);
									return;
								}
								if (this.selectionStart != this.selectionEnd)
								{
									this.DeleteSelection();
								}
								else if (args.Control)
								{
									this.DeleteNextWord();
								}
								else
								{
									this.DeleteNextChar();
								}
							}
							else
							{
								this.ClearSelection();
								this.cursorIndex = (this.scrollIndex = 0);
								this.Invalidate();
								this.OnCancel();
							}
						}
						else
						{
							this.OnSubmit();
						}
					}
					else if (args.Control)
					{
						this.DeletePreviousWord();
					}
					else
					{
						this.DeletePreviousChar();
					}
					break;
				case KeyCode.X:
					if (args.Control)
					{
						this.CutSelectionToClipboard();
					}
					break;
				}
				break;
			case KeyCode.C:
				if (args.Control)
				{
					this.CopySelectionToClipboard();
				}
				break;
			}
			break;
		}
		args.Use();
	}

	// Token: 0x06001708 RID: 5896 RVA: 0x0006C944 File Offset: 0x0006AB44
	public override void OnEnable()
	{
		if (this.padding == null)
		{
			this.padding = new RectOffset();
		}
		base.OnEnable();
		if (this.size.magnitude == 0f)
		{
			base.Size = new Vector2(100f, 20f);
		}
		this.cursorShown = false;
		this.cursorIndex = (this.scrollIndex = 0);
		bool flag = this.Font != null && this.Font.IsValid;
		if (Application.isPlaying && !flag)
		{
			this.Font = base.GetManager().DefaultFont;
		}
		this.bindTextureRebuildCallback();
	}

	// Token: 0x06001709 RID: 5897 RVA: 0x0006C9F8 File Offset: 0x0006ABF8
	public override void OnDisable()
	{
		base.OnDisable();
		this.unbindTextureRebuildCallback();
	}

	// Token: 0x0600170A RID: 5898 RVA: 0x0006CA08 File Offset: 0x0006AC08
	public override void Awake()
	{
		base.Awake();
		this.startSize = base.Size;
	}

	// Token: 0x0600170B RID: 5899 RVA: 0x0006CA1C File Offset: 0x0006AC1C
	protected internal override void OnEnterFocus(dfFocusEventArgs args)
	{
		base.OnEnterFocus(args);
		this.undoText = this.Text;
		if (!this.ReadOnly)
		{
			this.whenGotFocus = Time.realtimeSinceStartup;
			base.StopAllCoroutines();
			base.StartCoroutine(this.doCursorBlink());
			if (this.selectOnFocus)
			{
				this.selectionStart = 0;
				this.selectionEnd = this.text.Length;
			}
			else
			{
				this.selectionStart = (this.selectionEnd = 0);
			}
		}
		this.Invalidate();
	}

	// Token: 0x0600170C RID: 5900 RVA: 0x0006CAA4 File Offset: 0x0006ACA4
	protected internal override void OnLeaveFocus(dfFocusEventArgs args)
	{
		base.OnLeaveFocus(args);
		base.StopAllCoroutines();
		this.cursorShown = false;
		this.ClearSelection();
		this.Invalidate();
		this.whenGotFocus = 0f;
	}

	// Token: 0x0600170D RID: 5901 RVA: 0x0006CAD4 File Offset: 0x0006ACD4
	protected internal override void OnDoubleClick(dfMouseEventArgs args)
	{
		this.tripleClickTimer = Time.realtimeSinceStartup;
		if (args.Source != this)
		{
			base.OnDoubleClick(args);
			return;
		}
		if (!this.ReadOnly && this.HasFocus && args.Buttons.IsSet(dfMouseButtons.Left) && Time.realtimeSinceStartup - this.whenGotFocus > 0.5f)
		{
			int charIndexOfMouse = this.getCharIndexOfMouse(args);
			this.SelectWordAtIndex(charIndexOfMouse);
		}
		base.OnDoubleClick(args);
	}

	// Token: 0x0600170E RID: 5902 RVA: 0x0006CB58 File Offset: 0x0006AD58
	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		if (args.Source != this)
		{
			base.OnMouseDown(args);
			return;
		}
		bool flag = !this.ReadOnly && args.Buttons.IsSet(dfMouseButtons.Left) && ((!this.HasFocus && !this.SelectOnFocus) || Time.realtimeSinceStartup - this.whenGotFocus > 0.25f);
		if (flag)
		{
			int charIndexOfMouse = this.getCharIndexOfMouse(args);
			if (charIndexOfMouse != this.cursorIndex)
			{
				this.cursorIndex = charIndexOfMouse;
				this.cursorShown = true;
				this.Invalidate();
				args.Use();
			}
			this.mouseSelectionAnchor = this.cursorIndex;
			this.selectionStart = (this.selectionEnd = this.cursorIndex);
			if (Time.realtimeSinceStartup - this.tripleClickTimer < 0.25f)
			{
				this.SelectAll();
				this.tripleClickTimer = 0f;
			}
		}
		base.OnMouseDown(args);
	}

	// Token: 0x0600170F RID: 5903 RVA: 0x0006CC50 File Offset: 0x0006AE50
	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		if (args.Source != this)
		{
			base.OnMouseMove(args);
			return;
		}
		if (!this.ReadOnly && this.HasFocus && args.Buttons.IsSet(dfMouseButtons.Left))
		{
			int charIndexOfMouse = this.getCharIndexOfMouse(args);
			if (charIndexOfMouse != this.cursorIndex)
			{
				this.cursorIndex = charIndexOfMouse;
				this.cursorShown = true;
				this.Invalidate();
				args.Use();
				this.selectionStart = Mathf.Min(this.mouseSelectionAnchor, charIndexOfMouse);
				this.selectionEnd = Mathf.Max(this.mouseSelectionAnchor, charIndexOfMouse);
				return;
			}
		}
		base.OnMouseMove(args);
	}

	// Token: 0x06001710 RID: 5904 RVA: 0x0006CCF8 File Offset: 0x0006AEF8
	protected internal virtual void OnTextChanged()
	{
		base.SignalHierarchy("OnTextChanged", new object[] { this, this.text });
		if (this.TextChanged != null)
		{
			this.TextChanged(this, this.text);
		}
	}

	// Token: 0x06001711 RID: 5905 RVA: 0x0006CD38 File Offset: 0x0006AF38
	protected internal virtual void OnReadOnlyChanged()
	{
		if (this.ReadOnlyChanged != null)
		{
			this.ReadOnlyChanged(this, this.readOnly);
		}
	}

	// Token: 0x06001712 RID: 5906 RVA: 0x0006CD58 File Offset: 0x0006AF58
	protected internal virtual void OnPasswordCharacterChanged()
	{
		if (this.PasswordCharacterChanged != null)
		{
			this.PasswordCharacterChanged(this, this.passwordChar);
		}
	}

	// Token: 0x06001713 RID: 5907 RVA: 0x0006CD78 File Offset: 0x0006AF78
	protected internal virtual void OnSubmit()
	{
		base.SignalHierarchy("OnTextSubmitted", new object[] { this, this.text });
		if (this.TextSubmitted != null)
		{
			this.TextSubmitted(this, this.text);
		}
	}

	// Token: 0x06001714 RID: 5908 RVA: 0x0006CDB8 File Offset: 0x0006AFB8
	protected internal virtual void OnCancel()
	{
		this.text = this.undoText;
		base.SignalHierarchy("OnTextCancelled", new object[] { this, this.text });
		if (this.TextCancelled != null)
		{
			this.TextCancelled(this, this.text);
		}
	}

	// Token: 0x06001715 RID: 5909 RVA: 0x0006CE10 File Offset: 0x0006B010
	public void ClearSelection()
	{
		this.selectionStart = 0;
		this.selectionEnd = 0;
		this.mouseSelectionAnchor = 0;
	}

	// Token: 0x06001716 RID: 5910 RVA: 0x0006CE28 File Offset: 0x0006B028
	public void SelectAll()
	{
		this.selectionStart = 0;
		this.selectionEnd = this.text.Length;
		this.scrollIndex = 0;
		this.setCursorPos(0);
	}

	// Token: 0x06001717 RID: 5911 RVA: 0x0006CE50 File Offset: 0x0006B050
	private void CutSelectionToClipboard()
	{
		this.CopySelectionToClipboard();
		this.DeleteSelection();
	}

	// Token: 0x06001718 RID: 5912 RVA: 0x0006CE60 File Offset: 0x0006B060
	private void CopySelectionToClipboard()
	{
		if (this.selectionStart == this.selectionEnd)
		{
			return;
		}
		dfClipboardHelper.clipBoard = this.text.Substring(this.selectionStart, this.selectionEnd - this.selectionStart);
	}

	// Token: 0x06001719 RID: 5913 RVA: 0x0006CE98 File Offset: 0x0006B098
	public void PasteAtCursor(string clipData)
	{
		this.DeleteSelection();
		StringBuilder stringBuilder = new StringBuilder(this.text.Length + clipData.Length);
		stringBuilder.Append(this.text);
		foreach (char c in clipData)
		{
			if (c >= ' ')
			{
				stringBuilder.Insert(this.cursorIndex++, c);
			}
		}
		stringBuilder.Length = Mathf.Min(stringBuilder.Length, this.maxLength);
		this.text = stringBuilder.ToString();
		this.setCursorPos(this.cursorIndex);
		this.OnTextChanged();
		this.Invalidate();
	}

	// Token: 0x0600171A RID: 5914 RVA: 0x0006CF4C File Offset: 0x0006B14C
	public void SelectWordAtIndex(int index)
	{
		if (string.IsNullOrEmpty(this.text))
		{
			return;
		}
		index = Mathf.Max(Mathf.Min(this.text.Length - 1, index), 0);
		char c = this.text[index];
		if (!char.IsLetterOrDigit(c))
		{
			this.selectionStart = index;
			this.selectionEnd = index + 1;
			this.mouseSelectionAnchor = 0;
		}
		else
		{
			this.selectionStart = index;
			for (int i = index; i > 0; i--)
			{
				if (!char.IsLetterOrDigit(this.text[i - 1]))
				{
					break;
				}
				this.selectionStart--;
			}
			this.selectionEnd = index;
			for (int j = index; j < this.text.Length; j++)
			{
				if (!char.IsLetterOrDigit(this.text[j]))
				{
					break;
				}
				this.selectionEnd = j + 1;
			}
		}
		this.cursorIndex = this.selectionStart;
		this.Invalidate();
	}

	// Token: 0x0600171B RID: 5915 RVA: 0x0006D060 File Offset: 0x0006B260
	public void DeletePreviousChar()
	{
		if (this.selectionStart != this.selectionEnd)
		{
			int num = this.selectionStart;
			this.DeleteSelection();
			this.setCursorPos(num);
			return;
		}
		this.ClearSelection();
		if (this.cursorIndex == 0)
		{
			return;
		}
		this.text = this.text.Remove(this.cursorIndex - 1, 1);
		this.cursorIndex--;
		this.cursorShown = true;
		this.OnTextChanged();
		this.Invalidate();
	}

	// Token: 0x0600171C RID: 5916 RVA: 0x0006D0E0 File Offset: 0x0006B2E0
	public void DeletePreviousWord()
	{
		this.ClearSelection();
		if (this.cursorIndex == 0)
		{
			return;
		}
		int num = this.findPreviousWord(this.cursorIndex);
		if (num == this.cursorIndex)
		{
			num = 0;
		}
		this.text = this.text.Remove(num, this.cursorIndex - num);
		this.setCursorPos(num);
		this.OnTextChanged();
		this.Invalidate();
	}

	// Token: 0x0600171D RID: 5917 RVA: 0x0006D148 File Offset: 0x0006B348
	public void DeleteSelection()
	{
		if (this.selectionStart == this.selectionEnd)
		{
			return;
		}
		this.text = this.text.Remove(this.selectionStart, this.selectionEnd - this.selectionStart);
		this.setCursorPos(this.selectionStart);
		this.ClearSelection();
		this.OnTextChanged();
		this.Invalidate();
	}

	// Token: 0x0600171E RID: 5918 RVA: 0x0006D1AC File Offset: 0x0006B3AC
	public void DeleteNextChar()
	{
		this.ClearSelection();
		if (this.cursorIndex >= this.text.Length)
		{
			return;
		}
		this.text = this.text.Remove(this.cursorIndex, 1);
		this.cursorShown = true;
		this.OnTextChanged();
		this.Invalidate();
	}

	// Token: 0x0600171F RID: 5919 RVA: 0x0006D204 File Offset: 0x0006B404
	public void DeleteNextWord()
	{
		this.ClearSelection();
		if (this.cursorIndex == this.text.Length)
		{
			return;
		}
		int num = this.findNextWord(this.cursorIndex);
		if (num == this.cursorIndex)
		{
			num = this.text.Length;
		}
		this.text = this.text.Remove(this.cursorIndex, num - this.cursorIndex);
		this.OnTextChanged();
		this.Invalidate();
	}

	// Token: 0x06001720 RID: 5920 RVA: 0x0006D280 File Offset: 0x0006B480
	public void SelectToStart()
	{
		if (this.cursorIndex == 0)
		{
			return;
		}
		if (this.selectionEnd == this.selectionStart)
		{
			this.selectionEnd = this.cursorIndex;
		}
		else if (this.selectionEnd == this.cursorIndex)
		{
			this.selectionEnd = this.selectionStart;
		}
		this.selectionStart = 0;
		this.setCursorPos(0);
	}

	// Token: 0x06001721 RID: 5921 RVA: 0x0006D2E8 File Offset: 0x0006B4E8
	public void SelectToEnd()
	{
		if (this.cursorIndex == this.text.Length)
		{
			return;
		}
		if (this.selectionEnd == this.selectionStart)
		{
			this.selectionStart = this.cursorIndex;
		}
		else if (this.selectionStart == this.cursorIndex)
		{
			this.selectionStart = this.selectionEnd;
		}
		this.selectionEnd = this.text.Length;
		this.setCursorPos(this.text.Length);
	}

	// Token: 0x06001722 RID: 5922 RVA: 0x0006D370 File Offset: 0x0006B570
	public void MoveCursorToNextWord()
	{
		this.ClearSelection();
		if (this.cursorIndex == this.text.Length)
		{
			return;
		}
		int num = this.findNextWord(this.cursorIndex);
		this.setCursorPos(num);
	}

	// Token: 0x06001723 RID: 5923 RVA: 0x0006D3B0 File Offset: 0x0006B5B0
	public void MoveCursorToPreviousWord()
	{
		this.ClearSelection();
		if (this.cursorIndex == 0)
		{
			return;
		}
		int num = this.findPreviousWord(this.cursorIndex);
		this.setCursorPos(num);
	}

	// Token: 0x06001724 RID: 5924 RVA: 0x0006D3E4 File Offset: 0x0006B5E4
	public void MoveCursorToEnd()
	{
		this.ClearSelection();
		this.setCursorPos(this.text.Length);
	}

	// Token: 0x06001725 RID: 5925 RVA: 0x0006D400 File Offset: 0x0006B600
	public void MoveCursorToStart()
	{
		this.ClearSelection();
		this.setCursorPos(0);
	}

	// Token: 0x06001726 RID: 5926 RVA: 0x0006D410 File Offset: 0x0006B610
	public void MoveCursorToNextChar()
	{
		this.ClearSelection();
		this.setCursorPos(this.cursorIndex + 1);
	}

	// Token: 0x06001727 RID: 5927 RVA: 0x0006D428 File Offset: 0x0006B628
	public void MoveCursorToPreviousChar()
	{
		this.ClearSelection();
		this.setCursorPos(this.cursorIndex - 1);
	}

	// Token: 0x06001728 RID: 5928 RVA: 0x0006D440 File Offset: 0x0006B640
	private void moveSelectionPointRightWord()
	{
		if (this.cursorIndex == this.text.Length)
		{
			return;
		}
		int num = this.findNextWord(this.cursorIndex);
		if (this.selectionEnd == this.selectionStart)
		{
			this.selectionStart = this.cursorIndex;
			this.selectionEnd = num;
		}
		else if (this.selectionEnd == this.cursorIndex)
		{
			this.selectionEnd = num;
		}
		else if (this.selectionStart == this.cursorIndex)
		{
			this.selectionStart = num;
		}
		this.setCursorPos(num);
	}

	// Token: 0x06001729 RID: 5929 RVA: 0x0006D4D8 File Offset: 0x0006B6D8
	private void moveSelectionPointLeftWord()
	{
		if (this.cursorIndex == 0)
		{
			return;
		}
		int num = this.findPreviousWord(this.cursorIndex);
		if (this.selectionEnd == this.selectionStart)
		{
			this.selectionEnd = this.cursorIndex;
			this.selectionStart = num;
		}
		else if (this.selectionEnd == this.cursorIndex)
		{
			this.selectionEnd = num;
		}
		else if (this.selectionStart == this.cursorIndex)
		{
			this.selectionStart = num;
		}
		this.setCursorPos(num);
	}

	// Token: 0x0600172A RID: 5930 RVA: 0x0006D564 File Offset: 0x0006B764
	private void moveSelectionPointRight()
	{
		if (this.cursorIndex == this.text.Length)
		{
			return;
		}
		if (this.selectionEnd == this.selectionStart)
		{
			this.selectionEnd = this.cursorIndex + 1;
			this.selectionStart = this.cursorIndex;
		}
		else if (this.selectionEnd == this.cursorIndex)
		{
			this.selectionEnd++;
		}
		else if (this.selectionStart == this.cursorIndex)
		{
			this.selectionStart++;
		}
		this.setCursorPos(this.cursorIndex + 1);
	}

	// Token: 0x0600172B RID: 5931 RVA: 0x0006D60C File Offset: 0x0006B80C
	private void moveSelectionPointLeft()
	{
		if (this.cursorIndex == 0)
		{
			return;
		}
		if (this.selectionEnd == this.selectionStart)
		{
			this.selectionEnd = this.cursorIndex;
			this.selectionStart = this.cursorIndex - 1;
		}
		else if (this.selectionEnd == this.cursorIndex)
		{
			this.selectionEnd--;
		}
		else if (this.selectionStart == this.cursorIndex)
		{
			this.selectionStart--;
		}
		this.setCursorPos(this.cursorIndex - 1);
	}

	// Token: 0x0600172C RID: 5932 RVA: 0x0006D6A8 File Offset: 0x0006B8A8
	private void setCursorPos(int index)
	{
		index = Mathf.Max(0, Mathf.Min(this.text.Length, index));
		if (index == this.cursorIndex)
		{
			return;
		}
		this.cursorIndex = index;
		this.cursorShown = this.HasFocus;
		this.scrollIndex = Mathf.Min(this.scrollIndex, this.cursorIndex);
		this.Invalidate();
	}

	// Token: 0x0600172D RID: 5933 RVA: 0x0006D70C File Offset: 0x0006B90C
	private int findPreviousWord(int startIndex)
	{
		int i;
		for (i = startIndex; i > 0; i--)
		{
			char c = this.text[i - 1];
			if (!char.IsWhiteSpace(c) && !char.IsSeparator(c) && !char.IsPunctuation(c))
			{
				break;
			}
		}
		for (int j = i; j >= 0; j--)
		{
			if (j == 0)
			{
				i = 0;
				break;
			}
			char c2 = this.text[j - 1];
			if (char.IsWhiteSpace(c2) || char.IsSeparator(c2) || char.IsPunctuation(c2))
			{
				i = j;
				break;
			}
		}
		return i;
	}

	// Token: 0x0600172E RID: 5934 RVA: 0x0006D7BC File Offset: 0x0006B9BC
	private int findNextWord(int startIndex)
	{
		int length = this.text.Length;
		int i = startIndex;
		for (int j = i; j < length; j++)
		{
			char c = this.text[j];
			if (char.IsWhiteSpace(c) || char.IsSeparator(c) || char.IsPunctuation(c))
			{
				i = j;
				break;
			}
		}
		while (i < length)
		{
			char c2 = this.text[i];
			if (!char.IsWhiteSpace(c2) && !char.IsSeparator(c2) && !char.IsPunctuation(c2))
			{
				break;
			}
			i++;
		}
		return i;
	}

	// Token: 0x0600172F RID: 5935 RVA: 0x0006D86C File Offset: 0x0006BA6C
	private IEnumerator doCursorBlink()
	{
		if (!Application.isPlaying)
		{
			yield break;
		}
		this.cursorShown = true;
		while (this.ContainsFocus)
		{
			yield return new WaitForSeconds(this.cursorBlinkTime);
			this.cursorShown = !this.cursorShown;
			this.Invalidate();
		}
		this.cursorShown = false;
		yield break;
	}

	// Token: 0x06001730 RID: 5936 RVA: 0x0006D888 File Offset: 0x0006BA88
	private void renderText(dfRenderData textBuffer)
	{
		float num = base.PixelsToUnits();
		Vector2 vector = new Vector2(this.size.x - (float)this.padding.horizontal, this.size.y - (float)this.padding.vertical);
		Vector3 vector2 = this.pivot.TransformToUpperLeft(base.Size);
		Vector3 vector3 = new Vector3(vector2.x + (float)this.padding.left, vector2.y - (float)this.padding.top, 0f) * num;
		string text = ((!this.IsPasswordField || string.IsNullOrEmpty(this.passwordChar)) ? this.text : this.passwordDisplayText());
		Color32 color = ((!base.IsEnabled) ? base.DisabledColor : this.TextColor);
		float textScaleMultiplier = this.getTextScaleMultiplier();
		using (dfFontRendererBase dfFontRendererBase = this.font.ObtainRenderer())
		{
			dfFontRendererBase.WordWrap = false;
			dfFontRendererBase.MaxSize = vector;
			dfFontRendererBase.PixelRatio = num;
			dfFontRendererBase.TextScale = this.TextScale * textScaleMultiplier;
			dfFontRendererBase.VectorOffset = vector3;
			dfFontRendererBase.MultiLine = false;
			dfFontRendererBase.TextAlign = TextAlignment.Left;
			dfFontRendererBase.ProcessMarkup = false;
			dfFontRendererBase.DefaultColor = color;
			dfFontRendererBase.BottomColor = new Color32?(color);
			dfFontRendererBase.OverrideMarkupColors = false;
			dfFontRendererBase.Opacity = base.CalculateOpacity();
			dfFontRendererBase.Shadow = this.Shadow;
			dfFontRendererBase.ShadowColor = this.ShadowColor;
			dfFontRendererBase.ShadowOffset = this.ShadowOffset;
			this.cursorIndex = Mathf.Min(this.cursorIndex, text.Length);
			this.scrollIndex = Mathf.Min(Mathf.Min(this.scrollIndex, this.cursorIndex), text.Length);
			this.charWidths = dfFontRendererBase.GetCharacterWidths(text);
			Vector2 vector4 = vector * num;
			this.leftOffset = 0f;
			if (this.textAlign == TextAlignment.Left)
			{
				float num2 = 0f;
				for (int i = this.scrollIndex; i < this.cursorIndex; i++)
				{
					num2 += this.charWidths[i];
				}
				while (num2 >= vector4.x && this.scrollIndex < this.cursorIndex)
				{
					num2 -= this.charWidths[this.scrollIndex++];
				}
			}
			else
			{
				this.scrollIndex = Mathf.Max(0, Mathf.Min(this.cursorIndex, text.Length - 1));
				float num3 = 0f;
				float num4 = (float)this.font.FontSize * 1.25f * num;
				while (this.scrollIndex > 0 && num3 < vector4.x - num4)
				{
					num3 += this.charWidths[this.scrollIndex--];
				}
				float num5 = ((text.Length <= 0) ? 0f : dfFontRendererBase.GetCharacterWidths(text.Substring(this.scrollIndex)).Sum());
				TextAlignment textAlignment = this.textAlign;
				if (textAlignment != TextAlignment.Center)
				{
					if (textAlignment == TextAlignment.Right)
					{
						this.leftOffset = Mathf.Max(0f, vector4.x - num5);
					}
				}
				else
				{
					this.leftOffset = Mathf.Max(0f, (vector4.x - num5) * 0.5f);
				}
				vector3.x += this.leftOffset;
				dfFontRendererBase.VectorOffset = vector3;
			}
			if (this.selectionEnd != this.selectionStart)
			{
				this.renderSelection(this.scrollIndex, this.charWidths, this.leftOffset);
			}
			else if (this.cursorShown)
			{
				this.renderCursor(this.scrollIndex, this.cursorIndex, this.charWidths, this.leftOffset);
			}
			dfFontRendererBase.Render(text.Substring(this.scrollIndex), textBuffer);
		}
	}

	// Token: 0x06001731 RID: 5937 RVA: 0x0006DCCC File Offset: 0x0006BECC
	private float getTextScaleMultiplier()
	{
		if (this.textScaleMode == dfTextScaleMode.None || !Application.isPlaying)
		{
			return 1f;
		}
		if (this.textScaleMode == dfTextScaleMode.ScreenResolution)
		{
			return (float)Screen.height / (float)this.cachedManager.FixedHeight;
		}
		return base.Size.y / this.startSize.y;
	}

	// Token: 0x06001732 RID: 5938 RVA: 0x0006DD30 File Offset: 0x0006BF30
	private string passwordDisplayText()
	{
		return new string(this.passwordChar[0], this.text.Length);
	}

	// Token: 0x06001733 RID: 5939 RVA: 0x0006DD50 File Offset: 0x0006BF50
	private void renderSelection(int scrollIndex, float[] charWidths, float leftOffset)
	{
		if (string.IsNullOrEmpty(this.SelectionSprite) || base.Atlas == null)
		{
			return;
		}
		float num = base.PixelsToUnits();
		float num2 = (this.size.x - (float)this.padding.horizontal) * num;
		int num3 = scrollIndex;
		float num4 = 0f;
		for (int i = scrollIndex; i < this.text.Length; i++)
		{
			num3++;
			num4 += charWidths[i];
			if (num4 > num2)
			{
				break;
			}
		}
		if (this.selectionStart > num3 || this.selectionEnd < scrollIndex)
		{
			return;
		}
		int num5 = Mathf.Max(scrollIndex, this.selectionStart);
		if (num5 > num3)
		{
			return;
		}
		int num6 = Mathf.Min(this.selectionEnd, num3);
		if (num6 <= scrollIndex)
		{
			return;
		}
		float num7 = 0f;
		float num8 = 0f;
		num4 = 0f;
		for (int j = scrollIndex; j <= num3; j++)
		{
			if (j == num5)
			{
				num7 = num4;
			}
			if (j == num6)
			{
				num8 = num4;
				break;
			}
			num4 += charWidths[j];
		}
		float num9 = base.Size.y * num;
		this.addQuadIndices(this.renderData.Vertices, this.renderData.Triangles);
		RectOffset selectionPadding = this.getSelectionPadding();
		float num10 = num7 + leftOffset + (float)this.padding.left * num;
		float num11 = num10 + Mathf.Min(num8 - num7, num2);
		float num12 = (float)(-(float)(selectionPadding.top + 1)) * num;
		float num13 = num12 - num9 + (float)(selectionPadding.vertical + 2) * num;
		Vector3 vector = this.pivot.TransformToUpperLeft(base.Size) * num;
		Vector3 vector2 = new Vector3(num10, num12) + vector;
		Vector3 vector3 = new Vector3(num11, num12) + vector;
		Vector3 vector4 = new Vector3(num10, num13) + vector;
		Vector3 vector5 = new Vector3(num11, num13) + vector;
		this.renderData.Vertices.Add(vector2);
		this.renderData.Vertices.Add(vector3);
		this.renderData.Vertices.Add(vector5);
		this.renderData.Vertices.Add(vector4);
		Color32 color = base.ApplyOpacity(this.SelectionBackgroundColor);
		this.renderData.Colors.Add(color);
		this.renderData.Colors.Add(color);
		this.renderData.Colors.Add(color);
		this.renderData.Colors.Add(color);
		dfAtlas.ItemInfo itemInfo = base.Atlas[this.SelectionSprite];
		Rect region = itemInfo.region;
		float num14 = region.width / itemInfo.sizeInPixels.x;
		float num15 = region.height / itemInfo.sizeInPixels.y;
		this.renderData.UV.Add(new Vector2(region.x + num14, region.yMax - num15));
		this.renderData.UV.Add(new Vector2(region.xMax - num14, region.yMax - num15));
		this.renderData.UV.Add(new Vector2(region.xMax - num14, region.y + num15));
		this.renderData.UV.Add(new Vector2(region.x + num14, region.y + num15));
	}

	// Token: 0x06001734 RID: 5940 RVA: 0x0006E0E0 File Offset: 0x0006C2E0
	private RectOffset getSelectionPadding()
	{
		if (base.Atlas == null)
		{
			return this.padding;
		}
		dfAtlas.ItemInfo backgroundSprite = this.getBackgroundSprite();
		if (backgroundSprite == null)
		{
			return this.padding;
		}
		return backgroundSprite.border;
	}

	// Token: 0x06001735 RID: 5941 RVA: 0x0006E128 File Offset: 0x0006C328
	private void renderCursor(int startIndex, int cursorIndex, float[] charWidths, float leftOffset)
	{
		if (string.IsNullOrEmpty(this.SelectionSprite) || base.Atlas == null)
		{
			return;
		}
		float num = 0f;
		for (int i = startIndex; i < cursorIndex; i++)
		{
			num += charWidths[i];
		}
		float num2 = base.PixelsToUnits();
		float num3 = (num + leftOffset + (float)this.padding.left * num2).Quantize(num2);
		float num4 = (float)(-(float)this.padding.top) * num2;
		float num5 = num2 * (float)this.cursorWidth;
		float num6 = (this.size.y - (float)this.padding.vertical) * num2;
		Vector3 vector = new Vector3(num3, num4);
		Vector3 vector2 = new Vector3(num3 + num5, num4);
		Vector3 vector3 = new Vector3(num3 + num5, num4 - num6);
		Vector3 vector4 = new Vector3(num3, num4 - num6);
		dfList<Vector3> vertices = this.renderData.Vertices;
		dfList<int> triangles = this.renderData.Triangles;
		dfList<Vector2> uv = this.renderData.UV;
		dfList<Color32> colors = this.renderData.Colors;
		Vector3 vector5 = this.pivot.TransformToUpperLeft(this.size) * num2;
		this.addQuadIndices(vertices, triangles);
		vertices.Add(vector + vector5);
		vertices.Add(vector2 + vector5);
		vertices.Add(vector3 + vector5);
		vertices.Add(vector4 + vector5);
		Color32 color = base.ApplyOpacity(this.CursorColor);
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
		dfAtlas.ItemInfo itemInfo = base.Atlas[this.SelectionSprite];
		Rect region = itemInfo.region;
		uv.Add(new Vector2(region.x, region.yMax));
		uv.Add(new Vector2(region.xMax, region.yMax));
		uv.Add(new Vector2(region.xMax, region.y));
		uv.Add(new Vector2(region.x, region.y));
	}

	// Token: 0x06001736 RID: 5942 RVA: 0x0006E350 File Offset: 0x0006C550
	private void addQuadIndices(dfList<Vector3> verts, dfList<int> triangles)
	{
		int count = verts.Count;
		int[] array = new int[] { 0, 1, 3, 3, 1, 2 };
		for (int i = 0; i < array.Length; i++)
		{
			triangles.Add(count + array[i]);
		}
	}

	// Token: 0x06001737 RID: 5943 RVA: 0x0006E398 File Offset: 0x0006C598
	private int getCharIndexOfMouse(dfMouseEventArgs args)
	{
		Vector2 hitPosition = base.GetHitPosition(args);
		float num = base.PixelsToUnits();
		int num2 = this.scrollIndex;
		float num3 = this.leftOffset / num;
		for (int i = this.scrollIndex; i < this.charWidths.Length; i++)
		{
			num3 += this.charWidths[i] / num;
			if (num3 < hitPosition.x)
			{
				num2++;
			}
		}
		return num2;
	}

	// Token: 0x06001738 RID: 5944 RVA: 0x0006E408 File Offset: 0x0006C608
	public dfList<dfRenderData> RenderMultiple()
	{
		if (base.Atlas == null || this.Font == null)
		{
			return null;
		}
		if (!this.isVisible)
		{
			return null;
		}
		if (this.renderData == null)
		{
			this.renderData = dfRenderData.Obtain();
			this.textRenderData = dfRenderData.Obtain();
			this.isControlInvalidated = true;
		}
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		if (!this.isControlInvalidated)
		{
			for (int i = 0; i < this.buffers.Count; i++)
			{
				this.buffers[i].Transform = localToWorldMatrix;
			}
			return this.buffers;
		}
		this.buffers.Clear();
		this.renderData.Clear();
		this.renderData.Material = base.Atlas.Material;
		this.renderData.Transform = localToWorldMatrix;
		this.buffers.Add(this.renderData);
		this.textRenderData.Clear();
		this.textRenderData.Material = base.Atlas.Material;
		this.textRenderData.Transform = localToWorldMatrix;
		this.buffers.Add(this.textRenderData);
		this.renderBackground();
		this.renderText(this.textRenderData);
		this.isControlInvalidated = false;
		base.updateCollider();
		return this.buffers;
	}

	// Token: 0x06001739 RID: 5945 RVA: 0x0006E568 File Offset: 0x0006C768
	private void bindTextureRebuildCallback()
	{
		if (this.isFontCallbackAssigned || this.Font == null)
		{
			return;
		}
		if (this.Font is dfDynamicFont)
		{
			UnityEngine.Font.textureRebuilt += this.onFontTextureRebuilt;
			this.isFontCallbackAssigned = true;
		}
	}

	// Token: 0x0600173A RID: 5946 RVA: 0x0006E5BC File Offset: 0x0006C7BC
	private void unbindTextureRebuildCallback()
	{
		if (!this.isFontCallbackAssigned || this.Font == null)
		{
			return;
		}
		if (this.Font is dfDynamicFont)
		{
			UnityEngine.Font.textureRebuilt -= this.onFontTextureRebuilt;
		}
		this.isFontCallbackAssigned = false;
	}

	// Token: 0x0600173B RID: 5947 RVA: 0x0006E610 File Offset: 0x0006C810
	private void requestCharacterInfo()
	{
		dfDynamicFont dfDynamicFont = this.Font as dfDynamicFont;
		if (dfDynamicFont == null)
		{
			return;
		}
		if (!dfFontManager.IsDirty(this.Font))
		{
			return;
		}
		if (string.IsNullOrEmpty(this.text))
		{
			return;
		}
		float num = this.TextScale * this.getTextScaleMultiplier();
		int num2 = Mathf.CeilToInt((float)this.font.FontSize * num);
		dfDynamicFont.AddCharacterRequest(this.text, num2, FontStyle.Normal);
	}

	// Token: 0x0600173C RID: 5948 RVA: 0x0006E688 File Offset: 0x0006C888
	private void onFontTextureRebuilt(Font font)
	{
		if (this.Font is dfDynamicFont && font == (this.Font as dfDynamicFont).BaseFont)
		{
			this.requestCharacterInfo();
			this.Invalidate();
		}
	}

	// Token: 0x0600173D RID: 5949 RVA: 0x0006E6C4 File Offset: 0x0006C8C4
	public void UpdateFontInfo()
	{
		this.requestCharacterInfo();
	}

	// Token: 0x040012AA RID: 4778
	[SerializeField]
	protected dfFontBase font;

	// Token: 0x040012AB RID: 4779
	[SerializeField]
	protected bool acceptsTab;

	// Token: 0x040012AC RID: 4780
	[SerializeField]
	protected bool displayAsPassword;

	// Token: 0x040012AD RID: 4781
	[SerializeField]
	protected string passwordChar = "*";

	// Token: 0x040012AE RID: 4782
	[SerializeField]
	protected bool readOnly;

	// Token: 0x040012AF RID: 4783
	[SerializeField]
	protected string text = string.Empty;

	// Token: 0x040012B0 RID: 4784
	[SerializeField]
	protected Color32 textColor = UnityEngine.Color.white;

	// Token: 0x040012B1 RID: 4785
	[SerializeField]
	protected Color32 selectionBackground = new Color32(0, 105, 210, byte.MaxValue);

	// Token: 0x040012B2 RID: 4786
	[SerializeField]
	protected Color32 cursorColor = UnityEngine.Color.white;

	// Token: 0x040012B3 RID: 4787
	[SerializeField]
	protected string selectionSprite = string.Empty;

	// Token: 0x040012B4 RID: 4788
	[SerializeField]
	protected float textScale = 1f;

	// Token: 0x040012B5 RID: 4789
	[SerializeField]
	protected dfTextScaleMode textScaleMode;

	// Token: 0x040012B6 RID: 4790
	[SerializeField]
	protected RectOffset padding = new RectOffset();

	// Token: 0x040012B7 RID: 4791
	[SerializeField]
	protected float cursorBlinkTime = 0.45f;

	// Token: 0x040012B8 RID: 4792
	[SerializeField]
	protected int cursorWidth = 1;

	// Token: 0x040012B9 RID: 4793
	[SerializeField]
	protected int maxLength = 1024;

	// Token: 0x040012BA RID: 4794
	[SerializeField]
	protected bool selectOnFocus;

	// Token: 0x040012BB RID: 4795
	[SerializeField]
	protected bool shadow;

	// Token: 0x040012BC RID: 4796
	[SerializeField]
	protected Color32 shadowColor = UnityEngine.Color.black;

	// Token: 0x040012BD RID: 4797
	[SerializeField]
	protected Vector2 shadowOffset = new Vector2(1f, -1f);

	// Token: 0x040012BE RID: 4798
	[SerializeField]
	protected bool useMobileKeyboard;

	// Token: 0x040012BF RID: 4799
	[SerializeField]
	protected int mobileKeyboardType;

	// Token: 0x040012C0 RID: 4800
	[SerializeField]
	protected bool mobileAutoCorrect;

	// Token: 0x040012C1 RID: 4801
	[SerializeField]
	protected bool mobileHideInputField;

	// Token: 0x040012C2 RID: 4802
	[SerializeField]
	protected dfMobileKeyboardTrigger mobileKeyboardTrigger;

	// Token: 0x040012C3 RID: 4803
	[SerializeField]
	protected TextAlignment textAlign;

	// Token: 0x040012C4 RID: 4804
	private Vector2 startSize = Vector2.zero;

	// Token: 0x040012C5 RID: 4805
	private int selectionStart;

	// Token: 0x040012C6 RID: 4806
	private int selectionEnd;

	// Token: 0x040012C7 RID: 4807
	private int mouseSelectionAnchor;

	// Token: 0x040012C8 RID: 4808
	private int scrollIndex;

	// Token: 0x040012C9 RID: 4809
	private int cursorIndex;

	// Token: 0x040012CA RID: 4810
	private float leftOffset;

	// Token: 0x040012CB RID: 4811
	private bool cursorShown;

	// Token: 0x040012CC RID: 4812
	private float[] charWidths;

	// Token: 0x040012CD RID: 4813
	private float whenGotFocus;

	// Token: 0x040012CE RID: 4814
	private string undoText = string.Empty;

	// Token: 0x040012CF RID: 4815
	private float tripleClickTimer;

	// Token: 0x040012D0 RID: 4816
	private bool isFontCallbackAssigned;

	// Token: 0x040012D1 RID: 4817
	private dfRenderData textRenderData;

	// Token: 0x040012D2 RID: 4818
	private dfList<dfRenderData> buffers = dfList<dfRenderData>.Obtain();
}
