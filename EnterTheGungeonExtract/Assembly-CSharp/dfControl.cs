using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x02000395 RID: 917
[ExecuteInEditMode]
[Serializable]
public abstract class dfControl : MonoBehaviour, IDFControlHost, IComparable<dfControl>, IAmmonomiconFocusable
{
	// Token: 0x1400000A RID: 10
	// (add) Token: 0x0600101E RID: 4126 RVA: 0x0004B3C0 File Offset: 0x000495C0
	// (remove) Token: 0x0600101F RID: 4127 RVA: 0x0004B3F8 File Offset: 0x000495F8
	[HideInInspector]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ChildControlEventHandler ControlAdded;

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06001020 RID: 4128 RVA: 0x0004B430 File Offset: 0x00049630
	// (remove) Token: 0x06001021 RID: 4129 RVA: 0x0004B468 File Offset: 0x00049668
	[HideInInspector]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ChildControlEventHandler ControlRemoved;

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06001022 RID: 4130 RVA: 0x0004B4A0 File Offset: 0x000496A0
	// (remove) Token: 0x06001023 RID: 4131 RVA: 0x0004B4D8 File Offset: 0x000496D8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event FocusEventHandler GotFocus;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06001024 RID: 4132 RVA: 0x0004B510 File Offset: 0x00049710
	// (remove) Token: 0x06001025 RID: 4133 RVA: 0x0004B548 File Offset: 0x00049748
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event FocusEventHandler EnterFocus;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x06001026 RID: 4134 RVA: 0x0004B580 File Offset: 0x00049780
	// (remove) Token: 0x06001027 RID: 4135 RVA: 0x0004B5B8 File Offset: 0x000497B8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event FocusEventHandler LostFocus;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06001028 RID: 4136 RVA: 0x0004B5F0 File Offset: 0x000497F0
	// (remove) Token: 0x06001029 RID: 4137 RVA: 0x0004B628 File Offset: 0x00049828
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event FocusEventHandler LeaveFocus;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x0600102A RID: 4138 RVA: 0x0004B660 File Offset: 0x00049860
	// (remove) Token: 0x0600102B RID: 4139 RVA: 0x0004B698 File Offset: 0x00049898
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<bool> ControlShown;

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x0600102C RID: 4140 RVA: 0x0004B6D0 File Offset: 0x000498D0
	// (remove) Token: 0x0600102D RID: 4141 RVA: 0x0004B708 File Offset: 0x00049908
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<bool> ControlHidden;

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x0600102E RID: 4142 RVA: 0x0004B740 File Offset: 0x00049940
	// (remove) Token: 0x0600102F RID: 4143 RVA: 0x0004B778 File Offset: 0x00049978
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<bool> ControlClippingChanged;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06001030 RID: 4144 RVA: 0x0004B7B0 File Offset: 0x000499B0
	// (remove) Token: 0x06001031 RID: 4145 RVA: 0x0004B7E8 File Offset: 0x000499E8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<int> TabIndexChanged;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06001032 RID: 4146 RVA: 0x0004B820 File Offset: 0x00049A20
	// (remove) Token: 0x06001033 RID: 4147 RVA: 0x0004B858 File Offset: 0x00049A58
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<Vector2> PositionChanged;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x06001034 RID: 4148 RVA: 0x0004B890 File Offset: 0x00049A90
	// (remove) Token: 0x06001035 RID: 4149 RVA: 0x0004B8C8 File Offset: 0x00049AC8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<Vector2> SizeChanged;

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x06001036 RID: 4150 RVA: 0x0004B900 File Offset: 0x00049B00
	// (remove) Token: 0x06001037 RID: 4151 RVA: 0x0004B938 File Offset: 0x00049B38
	[HideInInspector]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<Color32> ColorChanged;

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x06001038 RID: 4152 RVA: 0x0004B970 File Offset: 0x00049B70
	// (remove) Token: 0x06001039 RID: 4153 RVA: 0x0004B9A8 File Offset: 0x00049BA8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<bool> IsVisibleChanged;

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x0600103A RID: 4154 RVA: 0x0004B9E0 File Offset: 0x00049BE0
	// (remove) Token: 0x0600103B RID: 4155 RVA: 0x0004BA18 File Offset: 0x00049C18
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<bool> IsEnabledChanged;

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x0600103C RID: 4156 RVA: 0x0004BA50 File Offset: 0x00049C50
	// (remove) Token: 0x0600103D RID: 4157 RVA: 0x0004BA88 File Offset: 0x00049C88
	[HideInInspector]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<float> OpacityChanged;

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x0600103E RID: 4158 RVA: 0x0004BAC0 File Offset: 0x00049CC0
	// (remove) Token: 0x0600103F RID: 4159 RVA: 0x0004BAF8 File Offset: 0x00049CF8
	[HideInInspector]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<dfAnchorStyle> AnchorChanged;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06001040 RID: 4160 RVA: 0x0004BB30 File Offset: 0x00049D30
	// (remove) Token: 0x06001041 RID: 4161 RVA: 0x0004BB68 File Offset: 0x00049D68
	[HideInInspector]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<dfPivotPoint> PivotChanged;

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06001042 RID: 4162 RVA: 0x0004BBA0 File Offset: 0x00049DA0
	// (remove) Token: 0x06001043 RID: 4163 RVA: 0x0004BBD8 File Offset: 0x00049DD8
	[HideInInspector]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<int> ZOrderChanged;

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x06001044 RID: 4164 RVA: 0x0004BC10 File Offset: 0x00049E10
	// (remove) Token: 0x06001045 RID: 4165 RVA: 0x0004BC48 File Offset: 0x00049E48
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DragEventHandler DragStart;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x06001046 RID: 4166 RVA: 0x0004BC80 File Offset: 0x00049E80
	// (remove) Token: 0x06001047 RID: 4167 RVA: 0x0004BCB8 File Offset: 0x00049EB8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DragEventHandler DragEnd;

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x06001048 RID: 4168 RVA: 0x0004BCF0 File Offset: 0x00049EF0
	// (remove) Token: 0x06001049 RID: 4169 RVA: 0x0004BD28 File Offset: 0x00049F28
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DragEventHandler DragDrop;

	// Token: 0x14000020 RID: 32
	// (add) Token: 0x0600104A RID: 4170 RVA: 0x0004BD60 File Offset: 0x00049F60
	// (remove) Token: 0x0600104B RID: 4171 RVA: 0x0004BD98 File Offset: 0x00049F98
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DragEventHandler DragEnter;

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x0600104C RID: 4172 RVA: 0x0004BDD0 File Offset: 0x00049FD0
	// (remove) Token: 0x0600104D RID: 4173 RVA: 0x0004BE08 File Offset: 0x0004A008
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DragEventHandler DragLeave;

	// Token: 0x14000022 RID: 34
	// (add) Token: 0x0600104E RID: 4174 RVA: 0x0004BE40 File Offset: 0x0004A040
	// (remove) Token: 0x0600104F RID: 4175 RVA: 0x0004BE78 File Offset: 0x0004A078
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DragEventHandler DragOver;

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06001050 RID: 4176 RVA: 0x0004BEB0 File Offset: 0x0004A0B0
	// (remove) Token: 0x06001051 RID: 4177 RVA: 0x0004BEE8 File Offset: 0x0004A0E8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event KeyPressHandler KeyPress;

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x06001052 RID: 4178 RVA: 0x0004BF20 File Offset: 0x0004A120
	// (remove) Token: 0x06001053 RID: 4179 RVA: 0x0004BF58 File Offset: 0x0004A158
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event KeyPressHandler KeyDown;

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x06001054 RID: 4180 RVA: 0x0004BF90 File Offset: 0x0004A190
	// (remove) Token: 0x06001055 RID: 4181 RVA: 0x0004BFC8 File Offset: 0x0004A1C8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event KeyPressHandler KeyUp;

	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06001056 RID: 4182 RVA: 0x0004C000 File Offset: 0x0004A200
	// (remove) Token: 0x06001057 RID: 4183 RVA: 0x0004C038 File Offset: 0x0004A238
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ControlMultiTouchEventHandler MultiTouch;

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06001058 RID: 4184 RVA: 0x0004C070 File Offset: 0x0004A270
	// (remove) Token: 0x06001059 RID: 4185 RVA: 0x0004C0A8 File Offset: 0x0004A2A8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ControlCallbackHandler MultiTouchEnd;

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x0600105A RID: 4186 RVA: 0x0004C0E0 File Offset: 0x0004A2E0
	// (remove) Token: 0x0600105B RID: 4187 RVA: 0x0004C118 File Offset: 0x0004A318
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MouseEventHandler MouseEnter;

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x0600105C RID: 4188 RVA: 0x0004C150 File Offset: 0x0004A350
	// (remove) Token: 0x0600105D RID: 4189 RVA: 0x0004C188 File Offset: 0x0004A388
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MouseEventHandler MouseMove;

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x0600105E RID: 4190 RVA: 0x0004C1C0 File Offset: 0x0004A3C0
	// (remove) Token: 0x0600105F RID: 4191 RVA: 0x0004C1F8 File Offset: 0x0004A3F8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MouseEventHandler MouseHover;

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x06001060 RID: 4192 RVA: 0x0004C230 File Offset: 0x0004A430
	// (remove) Token: 0x06001061 RID: 4193 RVA: 0x0004C268 File Offset: 0x0004A468
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MouseEventHandler MouseLeave;

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x06001062 RID: 4194 RVA: 0x0004C2A0 File Offset: 0x0004A4A0
	// (remove) Token: 0x06001063 RID: 4195 RVA: 0x0004C2D8 File Offset: 0x0004A4D8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MouseEventHandler MouseDown;

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06001064 RID: 4196 RVA: 0x0004C310 File Offset: 0x0004A510
	// (remove) Token: 0x06001065 RID: 4197 RVA: 0x0004C348 File Offset: 0x0004A548
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MouseEventHandler MouseUp;

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06001066 RID: 4198 RVA: 0x0004C380 File Offset: 0x0004A580
	// (remove) Token: 0x06001067 RID: 4199 RVA: 0x0004C3B8 File Offset: 0x0004A5B8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MouseEventHandler MouseWheel;

	// Token: 0x1400002F RID: 47
	// (add) Token: 0x06001068 RID: 4200 RVA: 0x0004C3F0 File Offset: 0x0004A5F0
	// (remove) Token: 0x06001069 RID: 4201 RVA: 0x0004C428 File Offset: 0x0004A628
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MouseEventHandler Click;

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x0600106A RID: 4202 RVA: 0x0004C460 File Offset: 0x0004A660
	// (remove) Token: 0x0600106B RID: 4203 RVA: 0x0004C498 File Offset: 0x0004A698
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MouseEventHandler DoubleClick;

	// Token: 0x0600106C RID: 4204 RVA: 0x0004C4D0 File Offset: 0x0004A6D0
	public dfLanguageManager GetLanguageManager()
	{
		if (this.languageManager == null)
		{
			if (this.languageManagerChecked)
			{
				return null;
			}
			this.languageManagerChecked = true;
			this.languageManager = this.GetManager().GetComponent<dfLanguageManager>();
			if (this.languageManager == null)
			{
				this.languageManager = GameUIRoot.Instance.GetComponent<dfLanguageManager>();
			}
		}
		return this.languageManager;
	}

	// Token: 0x0600106D RID: 4205 RVA: 0x0004C53C File Offset: 0x0004A73C
	public void ForceUpdateCachedParentTransform()
	{
		this.cachedParentTransform = base.transform.parent;
	}

	// Token: 0x0600106E RID: 4206 RVA: 0x0004C550 File Offset: 0x0004A750
	public string GetLocalizationKey()
	{
		return this.localizationKey;
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x0600106F RID: 4207 RVA: 0x0004C558 File Offset: 0x0004A758
	// (set) Token: 0x06001070 RID: 4208 RVA: 0x0004C560 File Offset: 0x0004A760
	public bool AllowSignalEvents
	{
		get
		{
			return this.allowSignalEvents;
		}
		set
		{
			this.allowSignalEvents = value;
		}
	}

	// Token: 0x17000389 RID: 905
	// (get) Token: 0x06001071 RID: 4209 RVA: 0x0004C56C File Offset: 0x0004A76C
	internal bool IsInvalid
	{
		get
		{
			return this.isControlInvalidated;
		}
	}

	// Token: 0x1700038A RID: 906
	// (get) Token: 0x06001072 RID: 4210 RVA: 0x0004C574 File Offset: 0x0004A774
	internal bool IsControlClipped
	{
		get
		{
			return this.isControlClipped;
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x06001073 RID: 4211 RVA: 0x0004C57C File Offset: 0x0004A77C
	public dfGUIManager GUIManager
	{
		get
		{
			return this.GetManager();
		}
	}

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x06001074 RID: 4212 RVA: 0x0004C584 File Offset: 0x0004A784
	// (set) Token: 0x06001075 RID: 4213 RVA: 0x0004C5F8 File Offset: 0x0004A7F8
	public bool IsEnabled
	{
		get
		{
			return base.enabled && (!(base.gameObject != null) || base.gameObject.activeSelf) && ((!(this.parent != null)) ? this.isEnabled : (this.isEnabled && this.parent.IsEnabled));
		}
		set
		{
			if (value != this.isEnabled)
			{
				this.isEnabled = value;
				this.OnIsEnabledChanged();
			}
		}
	}

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x06001076 RID: 4214 RVA: 0x0004C614 File Offset: 0x0004A814
	// (set) Token: 0x06001077 RID: 4215 RVA: 0x0004C64C File Offset: 0x0004A84C
	[SerializeField]
	public bool IsVisible
	{
		get
		{
			return (!(this.parent == null)) ? (this.isVisible && this.parent.IsVisible) : this.isVisible;
		}
		set
		{
			if (value != this.isVisible)
			{
				if (Application.isPlaying && !this.IsInteractive)
				{
					if (base.GetComponent<Collider>())
					{
						base.GetComponent<Collider>().enabled = false;
					}
				}
				else if (base.GetComponent<Collider>())
				{
					base.GetComponent<Collider>().enabled = value;
				}
				this.isVisible = value;
				this.OnIsVisibleChanged();
			}
		}
	}

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x06001078 RID: 4216 RVA: 0x0004C6C4 File Offset: 0x0004A8C4
	// (set) Token: 0x06001079 RID: 4217 RVA: 0x0004C6CC File Offset: 0x0004A8CC
	public virtual bool IsInteractive
	{
		get
		{
			return this.isInteractive;
		}
		set
		{
			if (this.HasFocus && !value)
			{
				dfGUIManager.SetFocus(null, true);
			}
			this.isInteractive = value;
		}
	}

	// Token: 0x1700038F RID: 911
	// (get) Token: 0x0600107A RID: 4218 RVA: 0x0004C6F0 File Offset: 0x0004A8F0
	// (set) Token: 0x0600107B RID: 4219 RVA: 0x0004C6F8 File Offset: 0x0004A8F8
	[SerializeField]
	public string Tooltip
	{
		get
		{
			return this.tooltip;
		}
		set
		{
			if (value != this.tooltip)
			{
				this.tooltip = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000390 RID: 912
	// (get) Token: 0x0600107C RID: 4220 RVA: 0x0004C718 File Offset: 0x0004A918
	// (set) Token: 0x0600107D RID: 4221 RVA: 0x0004C728 File Offset: 0x0004A928
	[SerializeField]
	public dfAnchorStyle Anchor
	{
		get
		{
			this.ensureLayoutExists();
			return this.anchorStyle;
		}
		set
		{
			if (value != this.anchorStyle)
			{
				this.anchorStyle = value;
				this.OnAnchorChanged();
			}
		}
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x0600107E RID: 4222 RVA: 0x0004C744 File Offset: 0x0004A944
	// (set) Token: 0x0600107F RID: 4223 RVA: 0x0004C758 File Offset: 0x0004A958
	public float Opacity
	{
		get
		{
			return (float)this.color.a / 255f;
		}
		set
		{
			value = Mathf.Max(0f, Mathf.Min(1f, value));
			float num = (float)this.color.a / 255f;
			if (!Mathf.Approximately(value, num))
			{
				this.color.a = (byte)(value * 255f);
				this.OnOpacityChanged();
			}
		}
	}

	// Token: 0x17000392 RID: 914
	// (get) Token: 0x06001080 RID: 4224 RVA: 0x0004C7B4 File Offset: 0x0004A9B4
	// (set) Token: 0x06001081 RID: 4225 RVA: 0x0004C7BC File Offset: 0x0004A9BC
	public Color32 Color
	{
		get
		{
			return this.color;
		}
		set
		{
			value.a = (byte)(this.Opacity * 255f);
			if (!this.color.EqualsNonAlloc(value))
			{
				this.color = value;
				this.OnColorChanged();
			}
		}
	}

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x06001082 RID: 4226 RVA: 0x0004C7F0 File Offset: 0x0004A9F0
	// (set) Token: 0x06001083 RID: 4227 RVA: 0x0004C7F8 File Offset: 0x0004A9F8
	public Color32 DisabledColor
	{
		get
		{
			return this.disabledColor;
		}
		set
		{
			if (!value.Equals(this.disabledColor))
			{
				this.disabledColor = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x06001084 RID: 4228 RVA: 0x0004C824 File Offset: 0x0004AA24
	// (set) Token: 0x06001085 RID: 4229 RVA: 0x0004C82C File Offset: 0x0004AA2C
	public dfPivotPoint Pivot
	{
		get
		{
			return this.pivot;
		}
		set
		{
			if (value != this.pivot)
			{
				Vector3 position = this.Position;
				this.pivot = value;
				Vector3 vector = this.Position - position;
				this.SuspendLayout();
				this.Position = position;
				for (int i = 0; i < this.controls.Count; i++)
				{
					this.controls[i].Position += vector;
				}
				this.ResumeLayout();
				this.OnPivotChanged();
			}
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x06001086 RID: 4230 RVA: 0x0004C8B4 File Offset: 0x0004AAB4
	// (set) Token: 0x06001087 RID: 4231 RVA: 0x0004C8BC File Offset: 0x0004AABC
	public Vector3 RelativePosition
	{
		get
		{
			return this.getRelativePosition();
		}
		set
		{
			this.setRelativePosition(ref value);
		}
	}

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x06001088 RID: 4232 RVA: 0x0004C8C8 File Offset: 0x0004AAC8
	// (set) Token: 0x06001089 RID: 4233 RVA: 0x0004C904 File Offset: 0x0004AB04
	public Vector3 Position
	{
		get
		{
			Vector3 vector = base.transform.localPosition / this.PixelsToUnits();
			return vector + this.pivot.TransformToUpperLeft(this.Size);
		}
		set
		{
			this.setPositionInternal(value);
		}
	}

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x0600108A RID: 4234 RVA: 0x0004C910 File Offset: 0x0004AB10
	// (set) Token: 0x0600108B RID: 4235 RVA: 0x0004C918 File Offset: 0x0004AB18
	public Vector2 Size
	{
		get
		{
			return this.size;
		}
		set
		{
			value = Vector2.Max(this.CalculateMinimumSize(), value);
			value.x = ((this.maxSize.x <= 0f) ? value.x : Mathf.Min(value.x, this.maxSize.x));
			value.y = ((this.maxSize.y <= 0f) ? value.y : Mathf.Min(value.y, this.maxSize.y));
			if ((value - this.size).sqrMagnitude <= 1f)
			{
				return;
			}
			this.size = value;
			this.OnSizeChanged();
		}
	}

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x0600108C RID: 4236 RVA: 0x0004C9E0 File Offset: 0x0004ABE0
	// (set) Token: 0x0600108D RID: 4237 RVA: 0x0004C9F0 File Offset: 0x0004ABF0
	public float Width
	{
		get
		{
			return this.size.x;
		}
		set
		{
			this.Size = new Vector2(value, this.size.y);
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x0600108E RID: 4238 RVA: 0x0004CA0C File Offset: 0x0004AC0C
	// (set) Token: 0x0600108F RID: 4239 RVA: 0x0004CA1C File Offset: 0x0004AC1C
	public float Height
	{
		get
		{
			return this.size.y;
		}
		set
		{
			this.Size = new Vector2(this.size.x, value);
		}
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x06001090 RID: 4240 RVA: 0x0004CA38 File Offset: 0x0004AC38
	// (set) Token: 0x06001091 RID: 4241 RVA: 0x0004CA40 File Offset: 0x0004AC40
	public Vector2 MinimumSize
	{
		get
		{
			return this.minSize;
		}
		set
		{
			value = Vector2.Max(Vector2.zero, value.RoundToInt());
			if (value != this.minSize)
			{
				this.minSize = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x06001092 RID: 4242 RVA: 0x0004CA74 File Offset: 0x0004AC74
	// (set) Token: 0x06001093 RID: 4243 RVA: 0x0004CA7C File Offset: 0x0004AC7C
	public Vector2 MaximumSize
	{
		get
		{
			return this.maxSize;
		}
		set
		{
			value = Vector2.Max(Vector2.zero, value.RoundToInt());
			if (value != this.maxSize)
			{
				this.maxSize = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x06001094 RID: 4244 RVA: 0x0004CAB0 File Offset: 0x0004ACB0
	// (set) Token: 0x06001095 RID: 4245 RVA: 0x0004CAB8 File Offset: 0x0004ACB8
	[HideInInspector]
	public int ZOrder
	{
		get
		{
			return this.zindex;
		}
		set
		{
			if (value != this.zindex)
			{
				if (this.parent != null)
				{
					this.parent.SetControlIndex(this, value);
				}
				else
				{
					this.zindex = Mathf.Max(-1, value);
				}
				this.OnZOrderChanged();
			}
		}
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x06001096 RID: 4246 RVA: 0x0004CB08 File Offset: 0x0004AD08
	// (set) Token: 0x06001097 RID: 4247 RVA: 0x0004CB10 File Offset: 0x0004AD10
	[HideInInspector]
	public int TabIndex
	{
		get
		{
			return this.tabIndex;
		}
		set
		{
			if (value != this.tabIndex)
			{
				this.tabIndex = Mathf.Max(-1, value);
				this.OnTabIndexChanged();
			}
		}
	}

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06001098 RID: 4248 RVA: 0x0004CB34 File Offset: 0x0004AD34
	public dfList<dfControl> Controls
	{
		get
		{
			return this.controls;
		}
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x06001099 RID: 4249 RVA: 0x0004CB3C File Offset: 0x0004AD3C
	public dfControl Parent
	{
		get
		{
			return this.parent;
		}
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x0600109A RID: 4250 RVA: 0x0004CB44 File Offset: 0x0004AD44
	// (set) Token: 0x0600109B RID: 4251 RVA: 0x0004CB4C File Offset: 0x0004AD4C
	public bool ClipChildren
	{
		get
		{
			return this.clipChildren;
		}
		set
		{
			if (value != this.clipChildren)
			{
				this.clipChildren = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x0600109C RID: 4252 RVA: 0x0004CB68 File Offset: 0x0004AD68
	// (set) Token: 0x0600109D RID: 4253 RVA: 0x0004CB70 File Offset: 0x0004AD70
	public bool InverseClipChildren
	{
		get
		{
			return this.inverseClipChildren;
		}
		set
		{
			if (value != this.inverseClipChildren)
			{
				this.inverseClipChildren = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x0600109E RID: 4254 RVA: 0x0004CB8C File Offset: 0x0004AD8C
	protected bool IsLayoutSuspended
	{
		get
		{
			return this.ForceSuspendLayout || this.performingLayout || (this.layout != null && this.layout.IsLayoutSuspended);
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x0600109F RID: 4255 RVA: 0x0004CBC0 File Offset: 0x0004ADC0
	protected bool IsPerformingLayout
	{
		get
		{
			return this.performingLayout || (this.layout != null && this.layout.IsPerformingLayout);
		}
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x060010A0 RID: 4256 RVA: 0x0004CBF0 File Offset: 0x0004ADF0
	// (set) Token: 0x060010A1 RID: 4257 RVA: 0x0004CBF8 File Offset: 0x0004ADF8
	public object Tag
	{
		get
		{
			return this.tag;
		}
		set
		{
			this.tag = value;
		}
	}

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x060010A2 RID: 4258 RVA: 0x0004CC04 File Offset: 0x0004AE04
	internal uint Version
	{
		get
		{
			return this.version;
		}
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x060010A3 RID: 4259 RVA: 0x0004CC0C File Offset: 0x0004AE0C
	// (set) Token: 0x060010A4 RID: 4260 RVA: 0x0004CC14 File Offset: 0x0004AE14
	public bool IsLocalized
	{
		get
		{
			return this.isLocalized;
		}
		set
		{
			this.isLocalized = value;
			if (value)
			{
				this.Localize();
			}
		}
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x060010A5 RID: 4261 RVA: 0x0004CC2C File Offset: 0x0004AE2C
	// (set) Token: 0x060010A6 RID: 4262 RVA: 0x0004CC34 File Offset: 0x0004AE34
	public Vector2 HotZoneScale
	{
		get
		{
			return this.hotZoneScale;
		}
		set
		{
			this.hotZoneScale = Vector2.Max(value, Vector2.zero);
			this.Invalidate();
		}
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x060010A7 RID: 4263 RVA: 0x0004CC50 File Offset: 0x0004AE50
	// (set) Token: 0x060010A8 RID: 4264 RVA: 0x0004CC58 File Offset: 0x0004AE58
	public bool AutoFocus
	{
		get
		{
			return this.autoFocus;
		}
		set
		{
			if (value != this.autoFocus)
			{
				this.autoFocus = value;
				if (value && this.IsEnabled && this.CanFocus)
				{
					this.Focus(true);
				}
			}
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x060010A9 RID: 4265 RVA: 0x0004CC90 File Offset: 0x0004AE90
	// (set) Token: 0x060010AA RID: 4266 RVA: 0x0004CCA8 File Offset: 0x0004AEA8
	public virtual bool CanFocus
	{
		get
		{
			return this.canFocus && this.IsInteractive;
		}
		set
		{
			this.canFocus = value;
		}
	}

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x060010AB RID: 4267 RVA: 0x0004CCB4 File Offset: 0x0004AEB4
	public virtual bool ContainsFocus
	{
		get
		{
			return dfGUIManager.ContainsFocus(this);
		}
	}

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x060010AC RID: 4268 RVA: 0x0004CCBC File Offset: 0x0004AEBC
	public virtual bool HasFocus
	{
		get
		{
			return dfGUIManager.HasFocus(this);
		}
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x060010AD RID: 4269 RVA: 0x0004CCC4 File Offset: 0x0004AEC4
	public bool ContainsMouse
	{
		get
		{
			return this.isMouseHovering;
		}
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x0004CCCC File Offset: 0x0004AECC
	internal void setRenderOrder(ref int order)
	{
		this.renderOrder = ++order;
		int count = this.controls.Count;
		dfControl[] items = this.controls.Items;
		for (int i = 0; i < count; i++)
		{
			if (items[i] != null)
			{
				items[i].setRenderOrder(ref order);
			}
		}
	}

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x060010AF RID: 4271 RVA: 0x0004CD2C File Offset: 0x0004AF2C
	[HideInInspector]
	public int RenderOrder
	{
		get
		{
			return this.renderOrder;
		}
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x0004CD34 File Offset: 0x0004AF34
	internal virtual void OnDragStart(dfDragEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnDragStart", this, args);
			if (!args.Used && this.DragStart != null)
			{
				this.DragStart(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnDragStart(args);
		}
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x0004CD9C File Offset: 0x0004AF9C
	internal virtual void OnDragEnd(dfDragEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnDragEnd", this, args);
			if (!args.Used && this.DragEnd != null)
			{
				this.DragEnd(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnDragEnd(args);
		}
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x0004CE04 File Offset: 0x0004B004
	internal virtual void OnDragDrop(dfDragEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnDragDrop", this, args);
			if (!args.Used && this.DragDrop != null)
			{
				this.DragDrop(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnDragDrop(args);
		}
	}

	// Token: 0x060010B3 RID: 4275 RVA: 0x0004CE6C File Offset: 0x0004B06C
	internal virtual void OnDragEnter(dfDragEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnDragEnter", this, args);
			if (!args.Used && this.DragEnter != null)
			{
				this.DragEnter(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnDragEnter(args);
		}
	}

	// Token: 0x060010B4 RID: 4276 RVA: 0x0004CED4 File Offset: 0x0004B0D4
	internal virtual void OnDragLeave(dfDragEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnDragLeave", this, args);
			if (!args.Used && this.DragLeave != null)
			{
				this.DragLeave(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnDragLeave(args);
		}
	}

	// Token: 0x060010B5 RID: 4277 RVA: 0x0004CF3C File Offset: 0x0004B13C
	internal virtual void OnDragOver(dfDragEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnDragOver", this, args);
			if (!args.Used && this.DragOver != null)
			{
				this.DragOver(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnDragOver(args);
		}
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x0004CFA4 File Offset: 0x0004B1A4
	protected internal virtual void OnMultiTouchEnd()
	{
		this.Signal("OnMultiTouchEnd", this);
		if (this.MultiTouchEnd != null)
		{
			this.MultiTouchEnd(this);
		}
		if (this.parent != null)
		{
			this.parent.OnMultiTouchEnd();
		}
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x0004CFF4 File Offset: 0x0004B1F4
	protected internal virtual void OnMultiTouch(dfTouchEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnMultiTouch", this, args);
			if (this.MultiTouch != null)
			{
				this.MultiTouch(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMultiTouch(args);
		}
	}

	// Token: 0x060010B8 RID: 4280 RVA: 0x0004D050 File Offset: 0x0004B250
	protected internal virtual void OnMouseEnter(dfMouseEventArgs args)
	{
		this.isMouseHovering = true;
		if (!args.Used)
		{
			this.Signal("OnMouseEnter", this, args);
			if (this.MouseEnter != null)
			{
				this.MouseEnter(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMouseEnter(args);
		}
	}

	// Token: 0x060010B9 RID: 4281 RVA: 0x0004D0B4 File Offset: 0x0004B2B4
	protected internal virtual void OnMouseLeave(dfMouseEventArgs args)
	{
		this.isMouseHovering = false;
		if (!args.Used)
		{
			this.Signal("OnMouseLeave", this, args);
			if (this.MouseLeave != null)
			{
				this.MouseLeave(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMouseLeave(args);
		}
	}

	// Token: 0x060010BA RID: 4282 RVA: 0x0004D118 File Offset: 0x0004B318
	protected internal virtual void OnMouseMove(dfMouseEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnMouseMove", this, args);
			if (this.MouseMove != null)
			{
				this.MouseMove(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMouseMove(args);
		}
	}

	// Token: 0x060010BB RID: 4283 RVA: 0x0004D174 File Offset: 0x0004B374
	protected internal virtual void OnMouseHover(dfMouseEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnMouseHover", this, args);
			if (this.MouseHover != null)
			{
				this.MouseHover(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMouseHover(args);
		}
	}

	// Token: 0x060010BC RID: 4284 RVA: 0x0004D1D0 File Offset: 0x0004B3D0
	protected internal virtual void OnMouseWheel(dfMouseEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnMouseWheel", this, args);
			if (this.MouseWheel != null)
			{
				this.MouseWheel(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMouseWheel(args);
		}
	}

	// Token: 0x060010BD RID: 4285 RVA: 0x0004D22C File Offset: 0x0004B42C
	protected internal virtual void OnMouseDown(dfMouseEventArgs args)
	{
		bool flag = this.IsInteractive && this.IsEnabled && this.IsVisible && this.CanFocus && !this.ContainsFocus;
		if (flag)
		{
			this.Focus(true);
		}
		if (!args.Used)
		{
			this.Signal("OnMouseDown", this, args);
			if (this.MouseDown != null)
			{
				this.MouseDown(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMouseDown(args);
		}
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x0004D2D0 File Offset: 0x0004B4D0
	protected internal virtual void OnMouseUp(dfMouseEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnMouseUp", this, args);
			if (this.MouseUp != null)
			{
				this.MouseUp(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMouseUp(args);
		}
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x0004D32C File Offset: 0x0004B52C
	protected internal virtual void OnClick(dfMouseEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnClick", this, args);
			if (this.Click != null)
			{
				this.Click(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnClick(args);
		}
	}

	// Token: 0x060010C0 RID: 4288 RVA: 0x0004D388 File Offset: 0x0004B588
	protected internal virtual void OnDoubleClick(dfMouseEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnDoubleClick", this, args);
			if (this.DoubleClick != null)
			{
				this.DoubleClick(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnDoubleClick(args);
		}
	}

	// Token: 0x060010C1 RID: 4289 RVA: 0x0004D3E4 File Offset: 0x0004B5E4
	protected internal virtual void OnKeyPress(dfKeyEventArgs args)
	{
		if (this.IsInteractive && !args.Used)
		{
			this.Signal("OnKeyPress", this, args);
			if (this.KeyPress != null)
			{
				this.KeyPress(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnKeyPress(args);
		}
	}

	// Token: 0x060010C2 RID: 4290 RVA: 0x0004D44C File Offset: 0x0004B64C
	protected internal virtual void OnKeyDown(dfKeyEventArgs args)
	{
		if (this.IsInteractive && !args.Used)
		{
			if (args.KeyCode == KeyCode.Tab)
			{
				this.OnTabKeyPressed(args);
			}
			if (!args.Used)
			{
				this.Signal("OnKeyDown", this, args);
				if (this.KeyDown != null)
				{
					this.KeyDown(this, args);
				}
			}
		}
		if (this.parent != null)
		{
			this.parent.OnKeyDown(args);
		}
	}

	// Token: 0x060010C3 RID: 4291 RVA: 0x0004D4D4 File Offset: 0x0004B6D4
	protected virtual void OnTabKeyPressed(dfKeyEventArgs args)
	{
		List<dfControl> list = (from c in this.GetManager().GetComponentsInChildren<dfControl>()
			where c != this && c.TabIndex >= 0 && c.IsInteractive && c.CanFocus && c.IsVisible
			select c).ToList<dfControl>();
		if (list.Count == 0)
		{
			return;
		}
		list.Sort(delegate(dfControl lhs, dfControl rhs)
		{
			if (lhs.TabIndex == rhs.TabIndex)
			{
				return lhs.RenderOrder.CompareTo(rhs.RenderOrder);
			}
			return lhs.TabIndex.CompareTo(rhs.TabIndex);
		});
		if (!args.Shift)
		{
			for (int i = 0; i < list.Count; i++)
			{
				dfControl dfControl = list[i];
				if (dfControl.TabIndex >= this.TabIndex)
				{
					list[i].Focus(true);
					args.Use();
					return;
				}
			}
			list[0].Focus(true);
			args.Use();
			return;
		}
		for (int j = list.Count - 1; j >= 0; j--)
		{
			dfControl dfControl2 = list[j];
			if (dfControl2.TabIndex <= this.TabIndex)
			{
				list[j].Focus(true);
				args.Use();
				return;
			}
		}
		list[list.Count - 1].Focus(true);
		args.Use();
	}

	// Token: 0x060010C4 RID: 4292 RVA: 0x0004D5F8 File Offset: 0x0004B7F8
	protected internal virtual void OnKeyUp(dfKeyEventArgs args)
	{
		if (this.IsInteractive)
		{
			this.Signal("OnKeyUp", this, args);
			if (this.KeyUp != null)
			{
				this.KeyUp(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnKeyUp(args);
		}
	}

	// Token: 0x060010C5 RID: 4293 RVA: 0x0004D654 File Offset: 0x0004B854
	protected internal virtual void OnEnterFocus(dfFocusEventArgs args)
	{
		this.Signal("OnEnterFocus", this, args);
		if (this.EnterFocus != null)
		{
			this.EnterFocus(this, args);
		}
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x0004D67C File Offset: 0x0004B87C
	protected internal virtual void OnLeaveFocus(dfFocusEventArgs args)
	{
		this.Signal("OnLeaveFocus", this, args);
		if (this.LeaveFocus != null)
		{
			this.LeaveFocus(this, args);
		}
	}

	// Token: 0x060010C7 RID: 4295 RVA: 0x0004D6A4 File Offset: 0x0004B8A4
	protected internal virtual void OnGotFocus(dfFocusEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnGotFocus", this, args);
			if (this.GotFocus != null)
			{
				this.GotFocus(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnGotFocus(args);
		}
	}

	// Token: 0x060010C8 RID: 4296 RVA: 0x0004D700 File Offset: 0x0004B900
	protected internal virtual void OnLostFocus(dfFocusEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnLostFocus", this, args);
			if (this.LostFocus != null)
			{
				this.LostFocus(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnLostFocus(args);
		}
	}

	// Token: 0x060010C9 RID: 4297 RVA: 0x0004D75C File Offset: 0x0004B95C
	protected internal bool Signal(string eventName, object arg)
	{
		dfControl.signal1[0] = arg;
		return this.Signal(base.gameObject, eventName, dfControl.signal1);
	}

	// Token: 0x060010CA RID: 4298 RVA: 0x0004D778 File Offset: 0x0004B978
	protected internal bool Signal(string eventName, object arg1, object arg2)
	{
		dfControl.signal2[0] = arg1;
		dfControl.signal2[1] = arg2;
		return this.Signal(base.gameObject, eventName, dfControl.signal2);
	}

	// Token: 0x060010CB RID: 4299 RVA: 0x0004D79C File Offset: 0x0004B99C
	protected internal bool Signal(string eventName, object arg1, object arg2, object arg3)
	{
		dfControl.signal3[0] = arg1;
		dfControl.signal3[1] = arg2;
		dfControl.signal3[2] = arg3;
		return this.Signal(base.gameObject, eventName, dfControl.signal3);
	}

	// Token: 0x060010CC RID: 4300 RVA: 0x0004D7CC File Offset: 0x0004B9CC
	protected internal bool Signal(string eventName, object[] args)
	{
		return this.Signal(base.gameObject, eventName, args);
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x0004D7DC File Offset: 0x0004B9DC
	protected internal bool SignalHierarchy(string eventName, params object[] args)
	{
		if (!this.allowSignalEvents)
		{
			return false;
		}
		bool flag = false;
		Transform transform = base.transform;
		while (!flag && transform != null)
		{
			flag = this.Signal(transform.gameObject, eventName, args);
			transform = transform.parent;
		}
		return flag;
	}

	// Token: 0x060010CE RID: 4302 RVA: 0x0004D830 File Offset: 0x0004BA30
	[HideInInspector]
	protected internal bool Signal(GameObject target, string eventName, object arg)
	{
		dfControl.signal1[0] = arg;
		return this.Signal(target, eventName, dfControl.signal1);
	}

	// Token: 0x060010CF RID: 4303 RVA: 0x0004D848 File Offset: 0x0004BA48
	[HideInInspector]
	protected internal bool Signal(GameObject target, string eventName, object[] args)
	{
		if (!this.allowSignalEvents || target == null || this.shutdownInProgress || !Application.isPlaying)
		{
			return false;
		}
		MonoBehaviour[] components = target.GetComponents<MonoBehaviour>();
		if (components == null || (target == base.gameObject && components.Length == 1))
		{
			return false;
		}
		if (args.Length == 0 || !object.ReferenceEquals(args[0], this))
		{
			object[] array = new object[args.Length + 1];
			Array.Copy(args, 0, array, 1, args.Length);
			array[0] = this;
			args = array;
		}
		bool flag = false;
		foreach (MonoBehaviour monoBehaviour in components)
		{
			if (!(monoBehaviour == null) && monoBehaviour.GetType() != null)
			{
				if (!(monoBehaviour == this))
				{
					if (monoBehaviour == null || monoBehaviour.enabled)
					{
						object obj = null;
						bool flag2 = dfControl.SignalCache.Invoke(monoBehaviour, eventName, args, out obj);
						if (flag2)
						{
							flag = true;
							if (obj is IEnumerator && monoBehaviour != null)
							{
								monoBehaviour.StartCoroutine((IEnumerator)obj);
							}
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x0004D980 File Offset: 0x0004BB80
	internal bool IsTopLevelControl(dfGUIManager manager)
	{
		return this.parent == null && this.cachedManager == manager;
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x0004D9A4 File Offset: 0x0004BBA4
	internal bool GetIsVisibleRaw()
	{
		return this.isVisible;
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x0004D9AC File Offset: 0x0004BBAC
	public void Localize()
	{
		if (!this.IsLocalized)
		{
			return;
		}
		if (this.languageManager == null)
		{
			this.languageManager = this.GetManager().GetComponent<dfLanguageManager>();
			if (this.languageManager == null)
			{
				this.languageManager = GameUIRoot.Instance.GetComponent<dfLanguageManager>();
			}
			if (this.languageManager == null)
			{
				return;
			}
		}
		this.OnLocalize();
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x0004DA20 File Offset: 0x0004BC20
	public void DoClick()
	{
		Camera camera = this.GetCamera();
		Vector3 vector = camera.WorldToScreenPoint(this.GetCenter());
		Ray ray = camera.ScreenPointToRay(vector);
		this.OnClick(new dfMouseEventArgs(this, dfMouseButtons.Left, 1, ray, vector, 0f));
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x0004DA64 File Offset: 0x0004BC64
	[HideInInspector]
	protected internal void RemoveEventHandlers(string eventName)
	{
		FieldInfo fieldInfo = base.GetType().GetAllFields().FirstOrDefault((FieldInfo f) => typeof(Delegate).IsAssignableFrom(f.FieldType) && f.Name == eventName);
		if (fieldInfo != null)
		{
			fieldInfo.SetValue(this, null);
		}
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x0004DAAC File Offset: 0x0004BCAC
	[HideInInspector]
	internal void RemoveAllEventHandlers()
	{
		FieldInfo[] array = (from f in base.GetType().GetAllFields()
			where typeof(Delegate).IsAssignableFrom(f.FieldType)
			select f).ToArray<FieldInfo>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetValue(this, null);
		}
	}

	// Token: 0x060010D6 RID: 4310 RVA: 0x0004DB0C File Offset: 0x0004BD0C
	public void Show()
	{
		this.IsVisible = true;
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x0004DB18 File Offset: 0x0004BD18
	public void Hide()
	{
		this.IsVisible = false;
	}

	// Token: 0x060010D8 RID: 4312 RVA: 0x0004DB24 File Offset: 0x0004BD24
	public void Enable()
	{
		this.IsEnabled = true;
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x0004DB30 File Offset: 0x0004BD30
	public void Disable()
	{
		this.IsEnabled = false;
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x0004DB3C File Offset: 0x0004BD3C
	public bool HitTest(Ray ray)
	{
		Plane plane = new Plane(base.transform.TransformDirection(Vector3.back), base.transform.position);
		float num = 0f;
		if (!plane.Raycast(ray, out num))
		{
			return false;
		}
		Vector3 vector = ray.origin + ray.direction * num;
		Plane[] array = ((!this.ClipChildren) ? null : this.GetClippingPlanes());
		if (array != null && array.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].GetSide(vector))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x0004DBF4 File Offset: 0x0004BDF4
	public Vector2 GetHitPosition(Ray ray)
	{
		Vector2 vector;
		if (!this.GetHitPosition(ray, out vector, false))
		{
			return Vector2.one * float.MinValue;
		}
		return vector;
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x0004DC24 File Offset: 0x0004BE24
	public bool GetHitPosition(Ray ray, out Vector2 position)
	{
		return this.GetHitPosition(ray, out position, true);
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x0004DC30 File Offset: 0x0004BE30
	public bool GetHitPosition(Ray ray, out Vector2 position, bool clamp)
	{
		position = Vector2.one * float.MinValue;
		Plane plane = new Plane(base.transform.TransformDirection(Vector3.back), base.transform.position);
		float num = 0f;
		if (!plane.Raycast(ray, out num))
		{
			return false;
		}
		Vector3 point = ray.GetPoint(num);
		Plane[] array = ((!this.ClipChildren) ? null : this.GetClippingPlanes());
		if (array != null && array.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].GetSide(point))
				{
					return false;
				}
			}
		}
		Vector3[] corners = this.GetCorners();
		Vector3 vector = corners[0];
		Vector3 vector2 = corners[1];
		Vector3 vector3 = corners[2];
		Vector3 vector4 = dfControl.closestPointOnLine(vector, vector2, point, clamp);
		float num2 = (vector4 - vector).magnitude / (vector2 - vector).magnitude;
		float num3 = this.size.x * num2;
		vector4 = dfControl.closestPointOnLine(vector, vector3, point, clamp);
		num2 = (vector4 - vector).magnitude / (vector3 - vector).magnitude;
		float num4 = this.size.y * num2;
		position = new Vector2(num3, num4);
		return true;
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x0004DDB0 File Offset: 0x0004BFB0
	public T Find<T>(string controlName) where T : dfControl
	{
		if (base.name == controlName && this is T)
		{
			return (T)((object)this);
		}
		this.updateControlHierarchy(true);
		for (int i = 0; i < this.controls.Count; i++)
		{
			T t = this.controls[i] as T;
			if (t != null && t.name == controlName)
			{
				return t;
			}
		}
		for (int j = 0; j < this.controls.Count; j++)
		{
			T t2 = this.controls[j].Find<T>(controlName);
			if (t2 != null)
			{
				return t2;
			}
		}
		return (T)((object)null);
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x0004DE90 File Offset: 0x0004C090
	public dfControl Find(string controlName)
	{
		if (base.name == controlName)
		{
			return this;
		}
		this.updateControlHierarchy(true);
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl dfControl = this.controls[i];
			if (dfControl.name == controlName)
			{
				return dfControl;
			}
		}
		for (int j = 0; j < this.controls.Count; j++)
		{
			dfControl dfControl2 = this.controls[j].Find(controlName);
			if (dfControl2 != null)
			{
				return dfControl2;
			}
		}
		return null;
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x0004DF34 File Offset: 0x0004C134
	public void Focus(bool allowScrolling = true)
	{
		if (!this.CanFocus || this.HasFocus || !this.IsEnabled || !this.IsVisible)
		{
			return;
		}
		dfGUIManager.SetFocus(this, allowScrolling);
		this.Invalidate();
	}

	// Token: 0x060010E1 RID: 4321 RVA: 0x0004DF70 File Offset: 0x0004C170
	public void Unfocus()
	{
		if (this.ContainsFocus)
		{
			dfGUIManager.SetFocus(null, true);
		}
	}

	// Token: 0x060010E2 RID: 4322 RVA: 0x0004DF84 File Offset: 0x0004C184
	public dfControl GetRootContainer()
	{
		dfControl dfControl = this;
		while (dfControl.Parent != null)
		{
			dfControl = dfControl.Parent;
		}
		return dfControl;
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x0004DFB4 File Offset: 0x0004C1B4
	public virtual void BringToFront()
	{
		if (this.parent == null)
		{
			this.GetManager().BringToFront(this);
		}
		else
		{
			this.parent.SetControlIndex(this, int.MaxValue);
		}
		this.Invalidate();
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x0004DFF0 File Offset: 0x0004C1F0
	public virtual void SendToBack()
	{
		if (this.parent == null)
		{
			this.GetManager().SendToBack(this);
		}
		else
		{
			this.parent.SetControlIndex(this, 0);
		}
		this.Invalidate();
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x0004E028 File Offset: 0x0004C228
	internal dfRenderData Render()
	{
		if (this.rendering)
		{
			return this.renderData;
		}
		dfRenderData dfRenderData;
		try
		{
			this.rendering = true;
			bool flag = this.isVisible;
			bool flag2 = base.enabled && base.gameObject.activeSelf;
			if (!flag || !flag2)
			{
				dfRenderData = null;
			}
			else
			{
				if (this.renderData == null)
				{
					this.renderData = dfRenderData.Obtain();
					this.isControlInvalidated = true;
				}
				if (this.isControlInvalidated)
				{
					this.renderData.Clear();
					this.OnRebuildRenderData();
					this.updateCollider();
				}
				this.renderData.Transform = base.transform.localToWorldMatrix;
				dfRenderData = this.renderData;
			}
		}
		finally
		{
			this.isControlInvalidated = false;
			this.rendering = false;
		}
		return dfRenderData;
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x0004E104 File Offset: 0x0004C304
	[HideInInspector]
	public virtual void Invalidate()
	{
		if (this.shutdownInProgress)
		{
			return;
		}
		this.updateVersion();
		this.isControlInvalidated = true;
		this.cachedBounds = null;
		dfGUIManager manager = this.GetManager();
		if (manager != null)
		{
			manager.Invalidate();
		}
		dfRenderGroup.InvalidateGroupForControl(this);
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x0004E158 File Offset: 0x0004C358
	[HideInInspector]
	public void ResetLayout()
	{
		this.ResetLayout(false, false);
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x0004E164 File Offset: 0x0004C364
	[HideInInspector]
	public void ResetLayout(bool recursive, bool force)
	{
		if (this.shutdownInProgress)
		{
			return;
		}
		bool flag = this.IsPerformingLayout || this.IsLayoutSuspended;
		if (!force && flag)
		{
			return;
		}
		if (this.layout == null)
		{
			this.layout = new dfControl.AnchorLayout(this.anchorStyle, this);
		}
		else
		{
			this.layout.Attach(this);
			this.layout.Reset(true);
		}
		if (recursive)
		{
			int count = this.controls.Count;
			dfControl[] items = this.controls.Items;
			for (int i = 0; i < count; i++)
			{
				items[i].ResetLayout();
			}
		}
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x0004E214 File Offset: 0x0004C414
	[HideInInspector]
	public void PerformLayout()
	{
		if (this.shutdownInProgress)
		{
			return;
		}
		if (this.isDisposing || this.performingLayout)
		{
			return;
		}
		try
		{
			this.performingLayout = true;
			this.ensureLayoutExists();
			this.layout.PerformLayout();
			if (GameUIRoot.Instance != null && this.GUIManager == GameUIRoot.Instance.Manager)
			{
				this.updateVersion();
				Vector3 vector = this.RelativePosition;
				vector = vector.Quantize(3f);
				this.RelativePosition = vector;
			}
			this.Invalidate();
		}
		finally
		{
			this.performingLayout = false;
		}
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x0004E2CC File Offset: 0x0004C4CC
	[HideInInspector]
	public void SuspendLayout()
	{
		this.ensureLayoutExists();
		this.layout.SuspendLayout();
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].SuspendLayout();
		}
	}

	// Token: 0x060010EB RID: 4331 RVA: 0x0004E318 File Offset: 0x0004C518
	[HideInInspector]
	public void ResumeLayout()
	{
		this.ensureLayoutExists();
		this.layout.ResumeLayout();
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].ResumeLayout();
		}
	}

	// Token: 0x060010EC RID: 4332 RVA: 0x0004E364 File Offset: 0x0004C564
	public virtual Vector2 CalculateMinimumSize()
	{
		return this.MinimumSize;
	}

	// Token: 0x060010ED RID: 4333 RVA: 0x0004E36C File Offset: 0x0004C56C
	[HideInInspector]
	public void MakePixelPerfect()
	{
		this.MakePixelPerfect(true);
	}

	// Token: 0x060010EE RID: 4334 RVA: 0x0004E378 File Offset: 0x0004C578
	[HideInInspector]
	public void MakePixelPerfect(bool recursive)
	{
	}

	// Token: 0x060010EF RID: 4335 RVA: 0x0004E37C File Offset: 0x0004C57C
	public Bounds GetBounds()
	{
		if (this.isInteractive && base.GetComponent<Collider>() != null)
		{
			this.cachedBounds = new Bounds?(base.GetComponent<Collider>().bounds);
			return this.cachedBounds.Value;
		}
		if (this.cachedBounds != null)
		{
			return this.cachedBounds.Value;
		}
		Vector3[] corners = this.GetCorners();
		Vector3 vector = corners[0] + (corners[3] - corners[0]) * 0.5f;
		Vector3 vector2 = vector;
		Vector3 vector3 = vector;
		for (int i = 0; i < corners.Length; i++)
		{
			vector2 = Vector3.Min(vector2, corners[i]);
			vector3 = Vector3.Max(vector3, corners[i]);
		}
		this.cachedBounds = new Bounds?(new Bounds(vector, vector3 - vector2));
		return this.cachedBounds.Value;
	}

	// Token: 0x060010F0 RID: 4336 RVA: 0x0004E48C File Offset: 0x0004C68C
	public Vector3 GetCenter()
	{
		Vector3[] corners = this.GetCorners();
		return corners[0] + (corners[3] - corners[0]) * 0.5f;
	}

	// Token: 0x060010F1 RID: 4337 RVA: 0x0004E4D8 File Offset: 0x0004C6D8
	public Vector3 GetAbsolutePosition()
	{
		Vector3 vector = Vector3.zero;
		dfControl dfControl = this;
		while (dfControl != null)
		{
			vector += dfControl.getRelativePosition();
			dfControl = dfControl.Parent;
		}
		return vector;
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x0004E514 File Offset: 0x0004C714
	public Vector3[] GetCorners()
	{
		float num = this.PixelsToUnits();
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		Vector3 vector = this.pivot.TransformToUpperLeft(this.size);
		Vector3 vector2 = vector + new Vector3(this.size.x, 0f);
		Vector3 vector3 = vector + new Vector3(0f, -this.size.y);
		Vector3 vector4 = vector2 + new Vector3(0f, -this.size.y);
		if (this.cachedCorners == null)
		{
			this.cachedCorners = new Vector3[4];
		}
		this.cachedCorners[0] = localToWorldMatrix.MultiplyPoint(vector * num);
		this.cachedCorners[1] = localToWorldMatrix.MultiplyPoint(vector2 * num);
		this.cachedCorners[2] = localToWorldMatrix.MultiplyPoint(vector3 * num);
		this.cachedCorners[3] = localToWorldMatrix.MultiplyPoint(vector4 * num);
		return this.cachedCorners;
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x0004E63C File Offset: 0x0004C83C
	public Camera GetCamera()
	{
		dfGUIManager manager = this.GetManager();
		if (manager == null)
		{
			UnityEngine.Debug.LogError("The Manager hosting this control could not be determined");
			return null;
		}
		return manager.RenderCamera;
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x0004E670 File Offset: 0x0004C870
	protected internal virtual RectOffset GetClipPadding()
	{
		return dfRectOffsetExtensions.Empty;
	}

	// Token: 0x060010F5 RID: 4341 RVA: 0x0004E678 File Offset: 0x0004C878
	public Rect GetScreenRect()
	{
		Camera camera = this.GetCamera();
		Vector3[] corners = this.GetCorners();
		Vector2 vector = Vector2.one * float.MaxValue;
		Vector2 vector2 = Vector2.one * float.MinValue;
		int num = corners.Length;
		for (int i = 0; i < num; i++)
		{
			Vector3 vector3 = camera.WorldToScreenPoint(corners[i]);
			vector = Vector2.Min(vector, vector3);
			vector2 = Vector2.Max(vector2, vector3);
		}
		return new Rect(vector.x, (float)Screen.height - vector2.y, vector2.x - vector.x, vector2.y - vector.y);
	}

	// Token: 0x060010F6 RID: 4342 RVA: 0x0004E738 File Offset: 0x0004C938
	public dfGUIManager GetManager()
	{
		if (this.cachedManager != null || !base.gameObject.activeInHierarchy)
		{
			return this.cachedManager;
		}
		if (this.parent != null && this.parent.cachedManager != null)
		{
			return this.cachedManager = this.parent.cachedManager;
		}
		GameObject gameObject = base.gameObject;
		while (gameObject != null)
		{
			dfGUIManager component = gameObject.GetComponent<dfGUIManager>();
			if (component != null)
			{
				return this.cachedManager = component;
			}
			if (gameObject.transform.parent == null)
			{
				break;
			}
			gameObject = gameObject.transform.parent.gameObject;
		}
		return dfGUIManager.ActiveManagers.FirstOrDefault<dfGUIManager>();
	}

	// Token: 0x060010F7 RID: 4343 RVA: 0x0004E818 File Offset: 0x0004CA18
	public float PixelsToUnits()
	{
		if (this.cachedPixelSize > 1E-45f)
		{
			return this.cachedPixelSize;
		}
		dfGUIManager manager = this.GetManager();
		if (manager == null)
		{
			return 0.0026f;
		}
		return this.cachedPixelSize = manager.PixelsToUnits();
	}

	// Token: 0x060010F8 RID: 4344 RVA: 0x0004E864 File Offset: 0x0004CA64
	protected internal virtual Plane[] GetClippingPlanes()
	{
		Vector3[] corners = this.GetCorners();
		Vector3 vector = base.transform.TransformDirection(Vector3.right);
		Vector3 vector2 = base.transform.TransformDirection(Vector3.left);
		Vector3 vector3 = base.transform.TransformDirection(Vector3.up);
		Vector3 vector4 = base.transform.TransformDirection(Vector3.down);
		this.cachedClippingPlanes[0] = new Plane(vector, corners[0]);
		this.cachedClippingPlanes[1] = new Plane(vector2, corners[1]);
		this.cachedClippingPlanes[2] = new Plane(vector3, corners[2]);
		this.cachedClippingPlanes[3] = new Plane(vector4, corners[0]);
		return this.cachedClippingPlanes;
	}

	// Token: 0x060010F9 RID: 4345 RVA: 0x0004E950 File Offset: 0x0004CB50
	public bool Contains(dfControl child)
	{
		return child != null && child.transform.IsChildOf(base.transform);
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x0004E974 File Offset: 0x0004CB74
	[HideInInspector]
	protected internal virtual void OnLocalize()
	{
		this.PerformLayout();
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x0004E97C File Offset: 0x0004CB7C
	public string ForceGetLocalizedValue(string key)
	{
		dfLanguageManager component = GameUIRoot.Instance.GetComponent<dfLanguageManager>();
		return component.GetValue(key);
	}

	// Token: 0x060010FC RID: 4348 RVA: 0x0004E99C File Offset: 0x0004CB9C
	[HideInInspector]
	protected internal string getLocalizedValue(string key)
	{
		if (!this.IsLocalized || !Application.isPlaying)
		{
			return key;
		}
		if (this.languageManager == null)
		{
			if (this.languageManagerChecked)
			{
				return key;
			}
			this.languageManagerChecked = true;
			this.languageManager = this.GetManager().GetComponent<dfLanguageManager>();
			if (this.languageManager == null)
			{
				this.languageManager = GameUIRoot.Instance.GetComponent<dfLanguageManager>();
			}
			if (this.languageManager == null)
			{
				return key;
			}
		}
		string value = this.languageManager.GetValue(key);
		return value.Replace("\\n", "\n");
	}

	// Token: 0x060010FD RID: 4349 RVA: 0x0004EA4C File Offset: 0x0004CC4C
	[HideInInspector]
	protected internal void updateCollider()
	{
		BoxCollider boxCollider = base.GetComponent<Collider>() as BoxCollider;
		if (boxCollider == null)
		{
			if (base.GetComponent<Collider>() != null)
			{
				throw new Exception("Invalid collider type on control: " + base.GetComponent<Collider>().GetType().Name);
			}
			boxCollider = base.gameObject.AddComponent<BoxCollider>();
		}
		if (Application.isPlaying && !this.isInteractive)
		{
			boxCollider.enabled = false;
			return;
		}
		float num = this.PixelsToUnits();
		Vector2 vector = this.size * num;
		Vector3 vector2 = this.pivot.TransformToCenter(vector);
		Vector3 vector3 = new Vector3(vector.x * this.hotZoneScale.x, vector.y * this.hotZoneScale.y, 0.001f);
		bool flag = base.enabled && this.IsVisible;
		boxCollider.isTrigger = false;
		boxCollider.enabled = flag;
		boxCollider.size = vector3;
		boxCollider.center = vector2;
	}

	// Token: 0x060010FE RID: 4350 RVA: 0x0004EB54 File Offset: 0x0004CD54
	[HideInInspector]
	protected virtual void OnRebuildRenderData()
	{
	}

	// Token: 0x060010FF RID: 4351 RVA: 0x0004EB58 File Offset: 0x0004CD58
	[HideInInspector]
	protected internal virtual void OnControlAdded(dfControl child)
	{
		this.Invalidate();
		if (this.ControlAdded != null)
		{
			this.ControlAdded(this, child);
		}
		this.Signal("OnControlAdded", this, child);
	}

	// Token: 0x06001100 RID: 4352 RVA: 0x0004EB88 File Offset: 0x0004CD88
	[HideInInspector]
	protected internal virtual void OnControlRemoved(dfControl child)
	{
		this.Invalidate();
		if (this.ControlRemoved != null)
		{
			this.ControlRemoved(this, child);
		}
		this.Signal("OnControlRemoved", this, child);
	}

	// Token: 0x06001101 RID: 4353 RVA: 0x0004EBB8 File Offset: 0x0004CDB8
	[HideInInspector]
	protected internal virtual void OnPositionChanged()
	{
		this.updateVersion();
		this.GetManager().Invalidate();
		dfRenderGroup.InvalidateGroupForControl(this);
		this.cachedPosition = base.transform.localPosition;
		if (this.isControlInitialized && Application.isPlaying && base.GetComponent<Rigidbody>() == null)
		{
			Rigidbody rigidbody = base.gameObject.AddComponent<Rigidbody>();
			rigidbody.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset;
			rigidbody.isKinematic = true;
			base.GetComponent<Rigidbody>().useGravity = false;
			rigidbody.detectCollisions = false;
		}
		this.ResetLayout();
		if (this.PositionChanged != null)
		{
			this.PositionChanged(this, this.Position);
		}
	}

	// Token: 0x06001102 RID: 4354 RVA: 0x0004EC6C File Offset: 0x0004CE6C
	[HideInInspector]
	protected internal virtual void OnSizeChanged()
	{
		this.updateCollider();
		this.Invalidate();
		this.ResetLayout();
		if (this.Anchor.IsAnyFlagSet(dfAnchorStyle.CenterHorizontal | dfAnchorStyle.CenterVertical))
		{
			this.PerformLayout();
		}
		this.raiseSizeChangedEvent();
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].PerformLayout();
		}
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x0004ECDC File Offset: 0x0004CEDC
	[HideInInspector]
	protected internal virtual void OnPivotChanged()
	{
		this.Invalidate();
		if (this.Anchor.IsAnyFlagSet(dfAnchorStyle.CenterHorizontal | dfAnchorStyle.CenterVertical))
		{
			this.ResetLayout();
			this.PerformLayout();
		}
		if (this.PivotChanged != null)
		{
			this.PivotChanged(this, this.pivot);
		}
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x0004ED30 File Offset: 0x0004CF30
	[HideInInspector]
	protected internal virtual void OnAnchorChanged()
	{
		this.ResetLayout();
		if (this.anchorStyle.IsAnyFlagSet(dfAnchorStyle.CenterHorizontal | dfAnchorStyle.CenterVertical))
		{
			this.PerformLayout();
		}
		if (this.AnchorChanged != null)
		{
			this.AnchorChanged(this, this.anchorStyle);
		}
		this.Invalidate();
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x0004ED84 File Offset: 0x0004CF84
	[HideInInspector]
	protected internal virtual void OnOpacityChanged()
	{
		this.Invalidate();
		float opacity = this.Opacity;
		if (this.OpacityChanged != null)
		{
			this.OpacityChanged(this, opacity);
		}
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].OnOpacityChanged();
		}
	}

	// Token: 0x06001106 RID: 4358 RVA: 0x0004EDE4 File Offset: 0x0004CFE4
	[HideInInspector]
	protected internal virtual void OnColorChanged()
	{
		this.Invalidate();
		Color32 color = this.Color;
		if (this.ColorChanged != null)
		{
			this.ColorChanged(this, color);
		}
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].OnColorChanged();
		}
	}

	// Token: 0x06001107 RID: 4359 RVA: 0x0004EE44 File Offset: 0x0004D044
	[HideInInspector]
	protected internal virtual void OnZOrderChanged()
	{
		if (this.ZOrderChanged != null)
		{
			this.ZOrderChanged(this, this.zindex);
		}
		this.Invalidate();
	}

	// Token: 0x06001108 RID: 4360 RVA: 0x0004EE6C File Offset: 0x0004D06C
	[HideInInspector]
	protected internal virtual void OnTabIndexChanged()
	{
		this.Invalidate();
		if (this.TabIndexChanged != null)
		{
			this.TabIndexChanged(this, this.tabIndex);
		}
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x0004EE94 File Offset: 0x0004D094
	[HideInInspector]
	protected virtual void OnControlClippingChanged()
	{
		if (this.ControlClippingChanged != null)
		{
			this.ControlClippingChanged(this, this.isControlClipped);
		}
		this.Signal("OnControlClippingChanged", this, this.isControlClipped);
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x0004EECC File Offset: 0x0004D0CC
	[HideInInspector]
	protected internal virtual void OnIsVisibleChanged()
	{
		this.updateCollider();
		bool flag = this.IsVisible;
		if (this.HasFocus && !flag)
		{
			dfGUIManager.SetFocus(null, true);
		}
		this.Signal("OnIsVisibleChanged", this, flag);
		if (this.IsVisibleChanged != null)
		{
			this.IsVisibleChanged(this, flag);
		}
		dfControl[] items = this.controls.Items;
		int count = this.controls.Count;
		for (int i = 0; i < count; i++)
		{
			items[i].OnIsVisibleChanged();
		}
		if (flag)
		{
			if (this.ControlShown != null)
			{
				this.ControlShown(this, true);
			}
			this.Signal("OnControlShown", this, true);
		}
		else
		{
			if (this.ControlHidden != null)
			{
				this.ControlHidden(this, true);
			}
			this.Signal("OnControlHidden", this, false);
		}
		this.Invalidate();
		if (flag)
		{
			this.doAutoFocus();
		}
		else if (!Application.isPlaying)
		{
			BoxCollider boxCollider = base.GetComponent<Collider>() as BoxCollider;
			boxCollider.size = Vector3.zero;
		}
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x0004EFF8 File Offset: 0x0004D1F8
	[HideInInspector]
	protected internal virtual void OnIsEnabledChanged()
	{
		if (this.shutdownInProgress)
		{
			return;
		}
		bool flag = this.IsEnabled && base.enabled;
		this.updateCollider();
		if (dfGUIManager.ContainsFocus(this) && !flag)
		{
			dfGUIManager.SetFocus(null, true);
		}
		this.Invalidate();
		this.Signal("OnIsEnabledChanged", this, flag);
		if (this.IsEnabledChanged != null)
		{
			this.IsEnabledChanged(this, flag);
		}
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].OnIsEnabledChanged();
		}
		this.doAutoFocus();
	}

	// Token: 0x0600110C RID: 4364 RVA: 0x0004F0A8 File Offset: 0x0004D2A8
	protected internal float CalculateOpacity()
	{
		if (this.parent == null)
		{
			return this.Opacity;
		}
		return this.Opacity * this.parent.CalculateOpacity();
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x0004F0D4 File Offset: 0x0004D2D4
	protected internal Color32 ApplyOpacity(Color32 rawColor)
	{
		float num = this.CalculateOpacity();
		rawColor.a = (byte)((float)rawColor.a * num);
		return rawColor;
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x0004F0FC File Offset: 0x0004D2FC
	protected internal Vector2 GetHitPosition(dfMouseEventArgs args)
	{
		Vector2 vector;
		this.GetHitPosition(args.Ray, out vector);
		return vector;
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x0004F11C File Offset: 0x0004D31C
	protected internal Vector3 getScaledDirection(Vector3 direction)
	{
		Vector3 localScale = this.GetManager().transform.localScale;
		direction = base.transform.TransformDirection(direction);
		return Vector3.Scale(direction, localScale);
	}

	// Token: 0x06001110 RID: 4368 RVA: 0x0004F150 File Offset: 0x0004D350
	protected internal Vector3 transformOffset(Vector3 offset)
	{
		Vector3 vector = offset.x * this.getScaledDirection(Vector3.right);
		Vector3 vector2 = offset.y * this.getScaledDirection(Vector3.down);
		return (vector + vector2) * this.PixelsToUnits();
	}

	// Token: 0x06001111 RID: 4369 RVA: 0x0004F1A0 File Offset: 0x0004D3A0
	protected internal virtual void OnResolutionChanged(Vector2 previousResolution, Vector2 currentResolution)
	{
		this.updateControlHierarchy();
		this.cachedPixelSize = 0f;
		Vector3 vector = base.transform.localPosition / (2f / previousResolution.y);
		Vector3 vector2 = vector * (2f / currentResolution.y);
		base.transform.localPosition = vector2;
		this.cachedPosition = vector2;
		Vector3 relativePosition = this.RelativePosition;
		if (this.Anchor.IsAnyFlagSet(dfAnchorStyle.CenterHorizontal | dfAnchorStyle.CenterVertical | dfAnchorStyle.Proportional))
		{
			this.PerformLayout();
		}
		this.updateCollider();
		if (this.ResolutionChangedPostLayout != null)
		{
			this.ResolutionChangedPostLayout(this, relativePosition, this.RelativePosition);
		}
		this.Signal("OnResolutionChanged", this, previousResolution, currentResolution);
		this.Invalidate();
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x0004F268 File Offset: 0x0004D468
	[HideInInspector]
	public virtual void Awake()
	{
		this.cachedParentTransform = base.transform.parent;
		if (this.anchorStyle == dfAnchorStyle.None && this.layout != null)
		{
			this.anchorStyle = this.layout.AnchorStyle;
		}
		if (base.GetComponent<Collider>() == null)
		{
			base.gameObject.AddComponent<BoxCollider>();
		}
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x0004F2CC File Offset: 0x0004D4CC
	[HideInInspector]
	public virtual void Start()
	{
	}

	// Token: 0x06001114 RID: 4372 RVA: 0x0004F2D0 File Offset: 0x0004D4D0
	[HideInInspector]
	public virtual void OnEnable()
	{
		this.cachedManager = null;
		this.cachedBounds = null;
		this.cachedChildCount = 0;
		this.cachedParentTransform = base.transform.parent;
		this.cachedPosition = Vector3.zero;
		this.cachedRelativePosition = Vector3.zero;
		this.cachedRotation = Quaternion.identity;
		this.cachedScale = Vector3.one;
		dfControl.ActiveInstances.Add(this);
		this.initializeControl();
		if (Application.isPlaying && this.IsLocalized)
		{
			this.Localize();
		}
		this.OnIsEnabledChanged();
	}

	// Token: 0x06001115 RID: 4373 RVA: 0x0004F36C File Offset: 0x0004D56C
	[HideInInspector]
	public virtual void OnApplicationQuit()
	{
		this.shutdownInProgress = true;
		this.RemoveAllEventHandlers();
	}

	// Token: 0x06001116 RID: 4374 RVA: 0x0004F37C File Offset: 0x0004D57C
	[HideInInspector]
	public virtual void OnDisable()
	{
		dfControl.ActiveInstances.Remove(this);
		try
		{
			this.Invalidate();
			if (dfGUIManager.HasFocus(this))
			{
				dfGUIManager.SetFocus(null, true);
			}
			else if (dfGUIManager.GetModalControl() == this)
			{
				dfGUIManager.PopModal();
			}
			this.OnIsEnabledChanged();
		}
		catch
		{
		}
		finally
		{
			this.isControlInitialized = false;
		}
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x0004F3FC File Offset: 0x0004D5FC
	[HideInInspector]
	public virtual void OnDestroy()
	{
		this.isDisposing = true;
		this.isControlInitialized = false;
		if (Application.isPlaying)
		{
			this.RemoveAllEventHandlers();
		}
		if (dfGUIManager.GetModalControl() == this)
		{
			dfGUIManager.PopModal();
		}
		if (this.layout != null)
		{
			this.layout.Dispose();
		}
		if (this.parent != null && this.parent.controls != null && !this.parent.isDisposing && this.parent.controls.Remove(this))
		{
			this.parent.cachedChildCount--;
			this.parent.OnControlRemoved(this);
		}
		for (int i = 0; i < this.controls.Count; i++)
		{
			if (this.controls[i].layout != null)
			{
				this.controls[i].layout.Dispose();
				this.controls[i].layout = null;
			}
			this.controls[i].parent = null;
		}
		this.controls.Release();
		if (this.cachedManager != null)
		{
			this.cachedManager.Invalidate();
		}
		if (this.renderData != null)
		{
			this.renderData.Release();
		}
		this.layout = null;
		this.cachedManager = null;
		this.parent = null;
		this.cachedClippingPlanes = null;
		this.cachedCorners = null;
		this.renderData = null;
		this.controls = null;
	}

	// Token: 0x06001118 RID: 4376 RVA: 0x0004F594 File Offset: 0x0004D794
	[HideInInspector]
	public virtual void LateUpdate()
	{
		if (this.layout != null && this.layout.HasPendingLayoutRequest)
		{
			this.layout.PerformLayout();
		}
	}

	// Token: 0x06001119 RID: 4377 RVA: 0x0004F5BC File Offset: 0x0004D7BC
	[HideInInspector]
	public virtual void Update()
	{
		if (this.PrecludeUpdateCycle)
		{
			return;
		}
		if (!this.isControlInitialized)
		{
			this.initializeControl();
		}
		if (this.m_transform == null)
		{
			this.m_transform = base.transform;
		}
		if (this.m_transform.parent != this.cachedParentTransform)
		{
			this.cachedManager = null;
			this.GetManager();
			this.cachedParentTransform = this.m_transform.parent;
			this.ResetLayout(false, true);
		}
		this.updateControlHierarchy();
		if (this.m_transform.hasChanged)
		{
			this.cachedBounds = null;
			if (this.cachedScale != this.m_transform.localScale)
			{
				this.cachedScale = this.m_transform.localScale;
				this.Invalidate();
			}
			if (Vector3.Distance(this.cachedPosition, this.m_transform.localPosition) > 1E-45f)
			{
				this.cachedPosition = this.m_transform.localPosition;
				this.OnPositionChanged();
			}
			if (this.cachedRotation != this.m_transform.localRotation)
			{
				this.cachedRotation = this.m_transform.localRotation;
				this.Invalidate();
			}
			this.m_transform.hasChanged = false;
		}
	}

	// Token: 0x0600111A RID: 4378 RVA: 0x0004F714 File Offset: 0x0004D914
	protected internal void SetControlIndex(dfControl child, int zorder)
	{
		if (zorder < 0)
		{
			zorder = int.MaxValue;
		}
		this.controls.Sort();
		this.controls.Remove(child);
		if (zorder >= this.controls.Count)
		{
			this.controls.Add(child);
		}
		else
		{
			this.controls.Insert(zorder, child);
		}
		child.zindex = zorder;
		for (int i = 0; i < this.controls.Count; i++)
		{
			if (this.controls[i].zindex != i)
			{
				dfControl dfControl = this.controls[i];
				dfControl.zindex = i;
				dfControl.OnZOrderChanged();
			}
		}
	}

	// Token: 0x0600111B RID: 4379 RVA: 0x0004F7CC File Offset: 0x0004D9CC
	public T AddControl<T>() where T : dfControl
	{
		return (T)((object)this.AddControl(typeof(T)));
	}

	// Token: 0x0600111C RID: 4380 RVA: 0x0004F7E4 File Offset: 0x0004D9E4
	public dfControl AddControl(Type controlType)
	{
		if (!typeof(dfControl).IsAssignableFrom(controlType))
		{
			throw new InvalidCastException(string.Format("Type {0} does not inherit from {1}", controlType.Name, typeof(dfControl).Name));
		}
		GameObject gameObject = new GameObject(controlType.Name);
		gameObject.transform.parent = base.transform;
		gameObject.layer = base.gameObject.layer;
		Vector2 vector = this.Size * this.PixelsToUnits() * 0.5f;
		gameObject.transform.localPosition = new Vector3(vector.x, vector.y, 0f);
		dfControl dfControl = gameObject.AddComponent(controlType) as dfControl;
		dfControl.parent = this;
		dfControl.cachedManager = this.cachedManager;
		dfControl.zindex = -1;
		this.AddControl(dfControl);
		return dfControl;
	}

	// Token: 0x0600111D RID: 4381 RVA: 0x0004F8CC File Offset: 0x0004DACC
	public void AddControl(dfControl child)
	{
		if (child.transform == null)
		{
			throw new NullReferenceException("The child control does not have a Transform");
		}
		if (child.parent != null && child.parent != this)
		{
			child.parent.RemoveControl(child);
		}
		if (!this.controls.Contains(child))
		{
			this.controls.Add(child);
			child.parent = this;
			child.transform.parent = base.transform;
			child.cachedManager = this.cachedManager;
			child.cachedParentTransform = base.transform;
		}
		if (child.zindex == -1 || child.zindex == 2147483647)
		{
			this.SetControlIndex(child, int.MaxValue);
		}
		else
		{
			this.controls.Sort();
		}
		this.OnControlAdded(child);
		child.Invalidate();
	}

	// Token: 0x0600111E RID: 4382 RVA: 0x0004F9B8 File Offset: 0x0004DBB8
	public dfControl AddPrefab(GameObject prefab)
	{
		if (prefab.GetComponent<dfControl>() == null)
		{
			throw new InvalidCastException();
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		gameObject.transform.parent = base.transform;
		gameObject.layer = base.gameObject.layer;
		dfControl component = gameObject.GetComponent<dfControl>();
		component.parent = this;
		component.zindex = -1;
		this.AddControl(component);
		return component;
	}

	// Token: 0x0600111F RID: 4383 RVA: 0x0004FA24 File Offset: 0x0004DC24
	private int getMaxZOrder()
	{
		int num = -1;
		for (int i = 0; i < this.controls.Count; i++)
		{
			num = Mathf.Max(this.controls[i].zindex, num);
		}
		return num;
	}

	// Token: 0x06001120 RID: 4384 RVA: 0x0004FA68 File Offset: 0x0004DC68
	public void RemoveControl(dfControl child)
	{
		if (this.isDisposing)
		{
			return;
		}
		if (child.Parent == this)
		{
			child.parent = null;
		}
		if (this.controls.Remove(child))
		{
			this.OnControlRemoved(child);
			child.Invalidate();
			this.Invalidate();
		}
	}

	// Token: 0x06001121 RID: 4385 RVA: 0x0004FAC0 File Offset: 0x0004DCC0
	[HideInInspector]
	public void RebuildControlOrder()
	{
		this.controls.Sort();
		for (int i = 0; i < this.controls.Count; i++)
		{
			if (this.controls[i].zindex != i)
			{
				dfControl dfControl = this.controls[i];
				dfControl.zindex = i;
				dfControl.OnZOrderChanged();
			}
		}
	}

	// Token: 0x06001122 RID: 4386 RVA: 0x0004FB28 File Offset: 0x0004DD28
	internal void setClippingState(bool isClipped)
	{
		if (isClipped == this.isControlClipped)
		{
			return;
		}
		this.isControlClipped = isClipped;
		this.OnControlClippingChanged();
	}

	// Token: 0x06001123 RID: 4387 RVA: 0x0004FB44 File Offset: 0x0004DD44
	private void doAutoFocus()
	{
		bool flag = Application.isPlaying && this.IsEnabled && base.enabled && this.AutoFocus && this.CanFocus && this.IsVisible && base.gameObject.activeSelf && base.gameObject.activeInHierarchy;
		if (flag)
		{
			base.StartCoroutine(this.focusOnNextFrame());
		}
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x0004FBC4 File Offset: 0x0004DDC4
	private IEnumerator focusOnNextFrame()
	{
		yield return null;
		this.Focus(true);
		yield break;
	}

	// Token: 0x06001125 RID: 4389 RVA: 0x0004FBE0 File Offset: 0x0004DDE0
	protected void raiseSizeChangedEvent()
	{
		if (this.SizeChanged != null)
		{
			this.SizeChanged(this, this.Size);
		}
	}

	// Token: 0x06001126 RID: 4390 RVA: 0x0004FC00 File Offset: 0x0004DE00
	protected void raiseMouseDownEvent(dfMouseEventArgs args)
	{
		if (this.MouseDown != null)
		{
			this.MouseDown(this, args);
		}
	}

	// Token: 0x06001127 RID: 4391 RVA: 0x0004FC1C File Offset: 0x0004DE1C
	protected void raiseMouseMoveEvent(dfMouseEventArgs args)
	{
		if (this.MouseMove != null)
		{
			this.MouseMove(this, args);
		}
	}

	// Token: 0x06001128 RID: 4392 RVA: 0x0004FC38 File Offset: 0x0004DE38
	protected void raiseMouseWheelEvent(dfMouseEventArgs args)
	{
		if (this.MouseWheel != null)
		{
			this.MouseWheel(this, args);
		}
	}

	// Token: 0x06001129 RID: 4393 RVA: 0x0004FC54 File Offset: 0x0004DE54
	private void initializeControl()
	{
		Transform transform = base.transform.parent;
		if (transform == null || transform.GetComponent(typeof(IDFControlHost)) == null)
		{
			return;
		}
		if (transform != null || this.cachedParentTransform != transform)
		{
			dfControl component = transform.GetComponent<dfControl>();
			if (component != null)
			{
				this.parent = component;
				component.AddControl(this);
			}
			if (this.controls == null)
			{
				this.updateControlHierarchy();
			}
		}
		if (this.renderOrder == -1)
		{
			this.renderOrder = this.ZOrder;
		}
		this.updateCollider();
		this.ensureLayoutExists();
		this.layout.Attach(this);
		if (!Application.isPlaying)
		{
			this.PerformLayout();
		}
		this.Invalidate();
		this.isControlInitialized = true;
	}

	// Token: 0x0600112A RID: 4394 RVA: 0x0004FD34 File Offset: 0x0004DF34
	internal void updateControlHierarchy()
	{
		this.updateControlHierarchy(false);
	}

	// Token: 0x0600112B RID: 4395 RVA: 0x0004FD40 File Offset: 0x0004DF40
	internal void updateControlHierarchy(bool force)
	{
		int childCount = base.transform.childCount;
		if (!force && childCount == this.cachedChildCount)
		{
			return;
		}
		this.cachedChildCount = childCount;
		dfList<dfControl> childControls = this.getChildControls();
		for (int i = 0; i < childControls.Count; i++)
		{
			dfControl dfControl = childControls[i];
			if (!this.controls.Contains(dfControl))
			{
				dfControl.parent = this;
				dfControl.cachedParentTransform = base.transform;
				if (!Application.isPlaying)
				{
					dfControl.ResetLayout();
				}
				this.OnControlAdded(dfControl);
				dfControl.updateControlHierarchy();
			}
		}
		for (int j = 0; j < this.controls.Count; j++)
		{
			dfControl dfControl2 = this.controls[j];
			if (dfControl2 == null || !childControls.Contains(dfControl2))
			{
				this.OnControlRemoved(dfControl2);
				if (dfControl2 != null && dfControl2.parent == this)
				{
					dfControl2.parent = null;
				}
			}
		}
		this.controls.Release();
		this.controls = childControls;
		this.RebuildControlOrder();
	}

	// Token: 0x0600112C RID: 4396 RVA: 0x0004FE70 File Offset: 0x0004E070
	private dfList<dfControl> getChildControls()
	{
		int childCount = base.transform.childCount;
		dfList<dfControl> dfList = dfList<dfControl>.Obtain(childCount);
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			if (child.gameObject.activeSelf)
			{
				dfControl component = child.GetComponent<dfControl>();
				if (component != null)
				{
					dfList.Add(component);
				}
			}
		}
		return dfList;
	}

	// Token: 0x0600112D RID: 4397 RVA: 0x0004FEE4 File Offset: 0x0004E0E4
	private void ensureLayoutExists()
	{
		if (this.layout == null)
		{
			dfAnchorStyle dfAnchorStyle = ((this.anchorStyle == dfAnchorStyle.None) ? (dfAnchorStyle.Top | dfAnchorStyle.Left) : this.anchorStyle);
			this.layout = new dfControl.AnchorLayout(dfAnchorStyle, this);
			this.anchorStyle = dfAnchorStyle;
		}
		else
		{
			this.layout.Attach(this);
		}
		int num = 0;
		while (this.Controls != null && num < this.Controls.Count)
		{
			if (this.controls[num] != null)
			{
				this.controls[num].ensureLayoutExists();
			}
			num++;
		}
	}

	// Token: 0x0600112E RID: 4398 RVA: 0x0004FF88 File Offset: 0x0004E188
	protected internal void updateVersion()
	{
		this.version = (dfControl.versionCounter += 1U);
	}

	// Token: 0x0600112F RID: 4399 RVA: 0x0004FFA0 File Offset: 0x0004E1A0
	private Vector3 getRelativePosition()
	{
		if (this.relativePositionCacheVersion == this.version)
		{
			return this.cachedRelativePosition;
		}
		this.relativePositionCacheVersion = this.version;
		if (base.transform.parent == null)
		{
			return Vector3.zero;
		}
		if (this.parent != null)
		{
			float num = this.PixelsToUnits();
			Vector3 position = base.transform.parent.position;
			Vector3 position2 = base.transform.position;
			Transform transform = base.transform.parent;
			Vector3 vector = transform.InverseTransformPoint(position / num);
			vector += this.parent.pivot.TransformToUpperLeft(this.parent.size);
			Vector3 vector2 = transform.InverseTransformPoint(position2 / num);
			vector2 += this.pivot.TransformToUpperLeft(this.size);
			Vector3 vector3 = vector2 - vector;
			return this.cachedRelativePosition = vector3.Scale(1f, -1f, 1f);
		}
		dfGUIManager manager = this.GetManager();
		if (manager == null)
		{
			UnityEngine.Debug.LogError("Cannot get position: View not found");
			this.relativePositionCacheVersion = uint.MaxValue;
			return Vector3.zero;
		}
		float num2 = this.PixelsToUnits();
		Vector3 vector4 = base.transform.position + this.pivot.TransformToUpperLeft(this.size) * num2;
		Plane[] clippingPlanes = manager.GetClippingPlanes();
		float num3 = clippingPlanes[0].GetDistanceToPoint(vector4) / num2;
		float num4 = clippingPlanes[3].GetDistanceToPoint(vector4) / num2;
		this.cachedRelativePosition = new Vector3(num3, num4).RoundToInt();
		return this.cachedRelativePosition;
	}

	// Token: 0x06001130 RID: 4400 RVA: 0x00050160 File Offset: 0x0004E360
	private void setPositionInternal(Vector3 value)
	{
		value += this.pivot.UpperLeftToTransform(this.Size);
		value *= this.PixelsToUnits();
		if (Vector3.Distance(value, this.cachedPosition) <= 1E-45f)
		{
			return;
		}
		Vector3 vector = value;
		base.transform.localPosition = vector;
		this.cachedPosition = vector;
		this.OnPositionChanged();
	}

	// Token: 0x06001131 RID: 4401 RVA: 0x000501C8 File Offset: 0x0004E3C8
	private void setRelativePosition(ref Vector3 value)
	{
		if (base.transform.parent == null)
		{
			UnityEngine.Debug.LogError("Cannot set relative position without a parent Transform.");
			return;
		}
		if ((value - this.getRelativePosition()).magnitude <= 1E-45f)
		{
			return;
		}
		if (this.parent != null)
		{
			Vector3 vector = value.Scale(1f, -1f, 1f) + this.pivot.UpperLeftToTransform(this.size) - this.parent.pivot.UpperLeftToTransform(this.parent.size);
			vector *= this.PixelsToUnits();
			if ((vector - base.transform.localPosition).sqrMagnitude >= 1E-45f)
			{
				base.transform.localPosition = vector;
				this.cachedPosition = vector;
				this.OnPositionChanged();
			}
			return;
		}
		dfGUIManager manager = this.GetManager();
		if (manager == null)
		{
			UnityEngine.Debug.LogError("Cannot get position: View not found");
			return;
		}
		Vector3[] corners = manager.GetCorners();
		Vector3 vector2 = corners[0];
		float num = this.PixelsToUnits();
		value = value.Scale(1f, -1f, 1f) * num;
		Vector3 vector3 = this.pivot.UpperLeftToTransform(this.Size) * num;
		Vector3 vector4 = vector2 + manager.transform.TransformDirection(value) + vector3;
		if (Vector3.Distance(vector4, this.cachedPosition) > 1E-45f)
		{
			base.transform.position = vector4;
			this.cachedPosition = base.transform.localPosition;
			this.OnPositionChanged();
		}
	}

	// Token: 0x06001132 RID: 4402 RVA: 0x000503A0 File Offset: 0x0004E5A0
	private static Vector3 closestPointOnLine(Vector3 start, Vector3 end, Vector3 test, bool clamp)
	{
		Vector3 vector = test - start;
		Vector3 vector2 = (end - start).normalized;
		float magnitude = (end - start).magnitude;
		float num = Vector3.Dot(vector2, vector);
		if (clamp)
		{
			if (num < 0f)
			{
				return start;
			}
			if (num > magnitude)
			{
				return end;
			}
		}
		vector2 *= num;
		return start + vector2;
	}

	// Token: 0x06001133 RID: 4403 RVA: 0x00050410 File Offset: 0x0004E610
	public int CompareTo(dfControl other)
	{
		if (this.ZOrder < 0)
		{
			return (other.ZOrder >= 0) ? 1 : 0;
		}
		return this.ZOrder.CompareTo(other.ZOrder);
	}

	// Token: 0x04000F2C RID: 3884
	[HideInInspector]
	public Action<dfControl> LanguageChanged;

	// Token: 0x04000F43 RID: 3907
	public Action<dfControl, Vector3, Vector3> ResolutionChangedPostLayout;

	// Token: 0x04000F44 RID: 3908
	private const float MINIMUM_OPACITY = 0.0125f;

	// Token: 0x04000F45 RID: 3909
	private static uint versionCounter;

	// Token: 0x04000F46 RID: 3910
	[SerializeField]
	protected dfAnchorStyle anchorStyle;

	// Token: 0x04000F47 RID: 3911
	[SerializeField]
	protected bool isEnabled = true;

	// Token: 0x04000F48 RID: 3912
	[SerializeField]
	protected bool isVisible = true;

	// Token: 0x04000F49 RID: 3913
	[SerializeField]
	protected bool isInteractive = true;

	// Token: 0x04000F4A RID: 3914
	[SerializeField]
	protected string tooltip;

	// Token: 0x04000F4B RID: 3915
	[SerializeField]
	protected dfPivotPoint pivot;

	// Token: 0x04000F4C RID: 3916
	[SerializeField]
	[HideInInspector]
	public int zindex = int.MaxValue;

	// Token: 0x04000F4D RID: 3917
	[SerializeField]
	protected Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04000F4E RID: 3918
	[SerializeField]
	protected Color32 disabledColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04000F4F RID: 3919
	[SerializeField]
	protected Vector2 size = Vector2.zero;

	// Token: 0x04000F50 RID: 3920
	[SerializeField]
	protected Vector2 minSize = Vector2.zero;

	// Token: 0x04000F51 RID: 3921
	[SerializeField]
	protected Vector2 maxSize = Vector2.zero;

	// Token: 0x04000F52 RID: 3922
	[SerializeField]
	protected bool clipChildren;

	// Token: 0x04000F53 RID: 3923
	[SerializeField]
	protected bool inverseClipChildren;

	// Token: 0x04000F54 RID: 3924
	[SerializeField]
	[HideInInspector]
	protected int tabIndex = -1;

	// Token: 0x04000F55 RID: 3925
	[HideInInspector]
	[SerializeField]
	protected bool canFocus;

	// Token: 0x04000F56 RID: 3926
	[SerializeField]
	protected bool autoFocus;

	// Token: 0x04000F57 RID: 3927
	[SerializeField]
	[HideInInspector]
	protected dfControl.AnchorLayout layout;

	// Token: 0x04000F58 RID: 3928
	[SerializeField]
	[HideInInspector]
	protected int renderOrder = -1;

	// Token: 0x04000F59 RID: 3929
	[SerializeField]
	protected bool isLocalized;

	// Token: 0x04000F5A RID: 3930
	[SerializeField]
	protected Vector2 hotZoneScale = Vector2.one;

	// Token: 0x04000F5B RID: 3931
	[SerializeField]
	protected bool allowSignalEvents = true;

	// Token: 0x04000F5C RID: 3932
	private static object[] signal1 = new object[1];

	// Token: 0x04000F5D RID: 3933
	private static object[] signal2 = new object[2];

	// Token: 0x04000F5E RID: 3934
	private static object[] signal3 = new object[3];

	// Token: 0x04000F5F RID: 3935
	protected bool isControlInvalidated = true;

	// Token: 0x04000F60 RID: 3936
	protected bool isControlClipped;

	// Token: 0x04000F61 RID: 3937
	protected dfControl parent;

	// Token: 0x04000F62 RID: 3938
	protected dfList<dfControl> controls = dfList<dfControl>.Obtain();

	// Token: 0x04000F63 RID: 3939
	protected dfGUIManager cachedManager;

	// Token: 0x04000F64 RID: 3940
	protected dfLanguageManager languageManager;

	// Token: 0x04000F65 RID: 3941
	protected bool languageManagerChecked;

	// Token: 0x04000F66 RID: 3942
	protected int cachedChildCount;

	// Token: 0x04000F67 RID: 3943
	protected Vector3 cachedPosition = Vector3.one * float.MinValue;

	// Token: 0x04000F68 RID: 3944
	protected Quaternion cachedRotation = Quaternion.identity;

	// Token: 0x04000F69 RID: 3945
	protected Vector3 cachedScale = Vector3.one;

	// Token: 0x04000F6A RID: 3946
	protected Bounds? cachedBounds;

	// Token: 0x04000F6B RID: 3947
	protected Transform cachedParentTransform;

	// Token: 0x04000F6C RID: 3948
	protected float cachedPixelSize;

	// Token: 0x04000F6D RID: 3949
	protected Vector3 cachedRelativePosition = Vector3.one * float.MinValue;

	// Token: 0x04000F6E RID: 3950
	protected uint relativePositionCacheVersion = uint.MaxValue;

	// Token: 0x04000F6F RID: 3951
	protected dfRenderData renderData;

	// Token: 0x04000F70 RID: 3952
	protected bool isMouseHovering;

	// Token: 0x04000F71 RID: 3953
	private new object tag;

	// Token: 0x04000F72 RID: 3954
	protected bool isDisposing;

	// Token: 0x04000F73 RID: 3955
	private bool performingLayout;

	// Token: 0x04000F74 RID: 3956
	protected Vector3[] cachedCorners = new Vector3[4];

	// Token: 0x04000F75 RID: 3957
	protected Plane[] cachedClippingPlanes = new Plane[4];

	// Token: 0x04000F76 RID: 3958
	private bool shutdownInProgress;

	// Token: 0x04000F77 RID: 3959
	private uint version;

	// Token: 0x04000F78 RID: 3960
	protected bool isControlInitialized;

	// Token: 0x04000F79 RID: 3961
	private bool rendering;

	// Token: 0x04000F7A RID: 3962
	protected string localizationKey;

	// Token: 0x04000F7B RID: 3963
	public static readonly dfList<dfControl> ActiveInstances = new dfList<dfControl>();

	// Token: 0x04000F7C RID: 3964
	[NonSerialized]
	public bool ForceSuspendLayout;

	// Token: 0x04000F7D RID: 3965
	public bool PrecludeUpdateCycle;

	// Token: 0x04000F7E RID: 3966
	private Transform m_transform;

	// Token: 0x02000396 RID: 918
	protected class SignalCache
	{
		// Token: 0x06001139 RID: 4409 RVA: 0x00050528 File Offset: 0x0004E728
		public static bool Invoke(Component target, string eventName, object[] arguments, out object returnValue)
		{
			returnValue = null;
			if (target == null)
			{
				return false;
			}
			Type type = target.GetType();
			dfControl.SignalCache.SignalCacheItem signalCacheItem = dfControl.SignalCache.getItem(type, eventName);
			if (signalCacheItem == null)
			{
				Type[] array = new Type[arguments.Length];
				for (int i = 0; i < array.Length; i++)
				{
					if (arguments[i] == null)
					{
						array[i] = typeof(object);
					}
					else
					{
						array[i] = arguments[i].GetType();
					}
				}
				signalCacheItem = new dfControl.SignalCache.SignalCacheItem(type, eventName, array);
				dfControl.SignalCache.cache.Add(signalCacheItem);
			}
			return signalCacheItem.Invoke(target, arguments, out returnValue);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x000505BC File Offset: 0x0004E7BC
		private static dfControl.SignalCache.SignalCacheItem getItem(Type componentType, string eventName)
		{
			for (int i = 0; i < dfControl.SignalCache.cache.Count; i++)
			{
				dfControl.SignalCache.SignalCacheItem signalCacheItem = dfControl.SignalCache.cache[i];
				if (signalCacheItem.ComponentType == componentType && signalCacheItem.EventName == eventName)
				{
					return signalCacheItem;
				}
			}
			return null;
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00050610 File Offset: 0x0004E810
		private static MethodInfo getMethod(Type type, string name, Type[] paramTypes)
		{
			return type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0005062C File Offset: 0x0004E82C
		private static bool matchesParameterTypes(MethodInfo method, Type[] types)
		{
			ParameterInfo[] parameters = method.GetParameters();
			if (parameters.Length != types.Length)
			{
				return false;
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (!parameters[i].ParameterType.IsAssignableFrom(types[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000F81 RID: 3969
		private static readonly List<dfControl.SignalCache.SignalCacheItem> cache = new List<dfControl.SignalCache.SignalCacheItem>();

		// Token: 0x02000397 RID: 919
		private class SignalCacheItem
		{
			// Token: 0x0600113E RID: 4414 RVA: 0x00050688 File Offset: 0x0004E888
			public SignalCacheItem(Type componentType, string eventName, Type[] paramTypes)
			{
				this.ComponentType = componentType;
				this.EventName = eventName;
				MethodInfo methodInfo = dfControl.SignalCache.getMethod(componentType, eventName, paramTypes);
				if (methodInfo != null)
				{
					this.method = methodInfo;
					this.usesParameters = true;
					return;
				}
				this.method = dfControl.SignalCache.getMethod(componentType, eventName, dfReflectionExtensions.EmptyTypes);
				this.usesParameters = false;
			}

			// Token: 0x0600113F RID: 4415 RVA: 0x000506E0 File Offset: 0x0004E8E0
			public bool Invoke(object target, object[] arguments, out object returnValue)
			{
				if (this.method == null)
				{
					returnValue = null;
					return false;
				}
				if (!this.usesParameters)
				{
					arguments = null;
				}
				returnValue = this.method.Invoke(target, arguments);
				return true;
			}

			// Token: 0x04000F82 RID: 3970
			public readonly Type ComponentType;

			// Token: 0x04000F83 RID: 3971
			public readonly string EventName;

			// Token: 0x04000F84 RID: 3972
			private readonly MethodInfo method;

			// Token: 0x04000F85 RID: 3973
			private readonly bool usesParameters;
		}
	}

	// Token: 0x02000398 RID: 920
	[Serializable]
	protected class AnchorLayout
	{
		// Token: 0x06001140 RID: 4416 RVA: 0x00050710 File Offset: 0x0004E910
		internal AnchorLayout(dfAnchorStyle anchorStyle)
		{
			this.anchorStyle = anchorStyle;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00050720 File Offset: 0x0004E920
		internal AnchorLayout(dfAnchorStyle anchorStyle, dfControl owner)
			: this(anchorStyle)
		{
			this.Attach(owner);
			this.Reset();
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x00050738 File Offset: 0x0004E938
		// (set) Token: 0x06001143 RID: 4419 RVA: 0x00050740 File Offset: 0x0004E940
		internal dfAnchorStyle AnchorStyle
		{
			get
			{
				return this.anchorStyle;
			}
			set
			{
				if (value != this.anchorStyle)
				{
					this.anchorStyle = value;
					this.Reset();
				}
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x0005075C File Offset: 0x0004E95C
		internal bool IsPerformingLayout
		{
			get
			{
				return this.performingLayout;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x00050764 File Offset: 0x0004E964
		internal bool IsLayoutSuspended
		{
			get
			{
				return this.suspendLayoutCounter > 0;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x00050770 File Offset: 0x0004E970
		internal bool HasPendingLayoutRequest
		{
			get
			{
				return this.pendingLayoutRequest;
			}
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x00050778 File Offset: 0x0004E978
		internal void Dispose()
		{
			if (!this.disposed)
			{
				this.disposed = true;
				this.owner = null;
			}
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x00050794 File Offset: 0x0004E994
		internal void SuspendLayout()
		{
			this.suspendLayoutCounter++;
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x000507A4 File Offset: 0x0004E9A4
		internal void ResumeLayout()
		{
			bool flag = this.suspendLayoutCounter > 0;
			this.suspendLayoutCounter = Mathf.Max(0, this.suspendLayoutCounter - 1);
			if (flag && this.suspendLayoutCounter == 0 && this.pendingLayoutRequest)
			{
				this.PerformLayout();
			}
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x000507F4 File Offset: 0x0004E9F4
		internal void PerformLayout()
		{
			if (this.disposed)
			{
				return;
			}
			if (this.suspendLayoutCounter > 0)
			{
				this.pendingLayoutRequest = true;
			}
			else
			{
				this.performLayoutInternal();
			}
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x00050820 File Offset: 0x0004EA20
		internal void Attach(dfControl ownerControl)
		{
			this.owner = ownerControl;
			if (ownerControl != null)
			{
				this.anchorStyle = ownerControl.anchorStyle;
			}
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x00050844 File Offset: 0x0004EA44
		internal void Reset()
		{
			this.Reset(false);
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00050850 File Offset: 0x0004EA50
		internal void Reset(bool force)
		{
			if (this.owner == null || this.owner.transform.parent == null || this.anchorStyle == dfAnchorStyle.None)
			{
				return;
			}
			bool flag = (!force && (this.IsPerformingLayout || this.IsLayoutSuspended)) || this.owner == null || !this.owner.gameObject.activeSelf;
			if (flag)
			{
				return;
			}
			if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Proportional))
			{
				this.resetLayoutProportional();
			}
			else
			{
				this.resetLayoutAbsolute();
			}
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00050910 File Offset: 0x0004EB10
		private void resetLayoutProportional()
		{
			Vector3 relativePosition = this.owner.RelativePosition;
			Vector2 size = this.owner.Size;
			Vector2 parentSize = this.getParentSize();
			float x = relativePosition.x;
			float y = relativePosition.y;
			float num = x + size.x;
			float num2 = y + size.y;
			if (this.margins == null)
			{
				this.margins = new dfAnchorMargins();
			}
			this.margins.left = x / parentSize.x;
			this.margins.right = num / parentSize.x;
			this.margins.top = y / parentSize.y;
			this.margins.bottom = num2 / parentSize.y;
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x000509D0 File Offset: 0x0004EBD0
		private void resetLayoutAbsolute()
		{
			Vector3 relativePosition = this.owner.RelativePosition;
			Vector2 size = this.owner.Size;
			Vector2 parentSize = this.getParentSize();
			float x = relativePosition.x;
			float y = relativePosition.y;
			float num = parentSize.x - size.x - x;
			float num2 = parentSize.y - size.y - y;
			if (this.margins == null)
			{
				this.margins = new dfAnchorMargins();
			}
			this.margins.left = x;
			this.margins.right = num;
			this.margins.top = y;
			this.margins.bottom = num2;
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x00050A80 File Offset: 0x0004EC80
		protected void performLayoutInternal()
		{
			if (this.anchorStyle == dfAnchorStyle.None)
			{
				return;
			}
			bool flag = this.owner == null || this.owner.transform.parent == null;
			if (flag)
			{
				this.pendingLayoutRequest = true;
				return;
			}
			bool flag2 = this.margins == null || this.IsPerformingLayout || this.IsLayoutSuspended || !this.owner.gameObject.activeSelf;
			if (flag2)
			{
				return;
			}
			try
			{
				this.performingLayout = true;
				this.pendingLayoutRequest = false;
				Vector2 parentSize = this.getParentSize();
				Vector2 size = this.owner.Size;
				if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Proportional))
				{
					this.performLayoutProportional(parentSize, size);
				}
				else
				{
					this.performLayoutAbsolute(parentSize, size);
				}
			}
			finally
			{
				this.performingLayout = false;
			}
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00050B7C File Offset: 0x0004ED7C
		private void performLayoutProportional(Vector2 parentSize, Vector2 controlSize)
		{
			float num = this.margins.left * parentSize.x;
			float num2 = this.margins.right * parentSize.x;
			float num3 = this.margins.top * parentSize.y;
			float num4 = this.margins.bottom * parentSize.y;
			Vector3 relativePosition = this.owner.RelativePosition;
			Vector2 vector = controlSize;
			if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Left))
			{
				relativePosition.x = num;
				if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Right))
				{
					vector.x = (this.margins.right - this.margins.left) * parentSize.x;
				}
			}
			else if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Right))
			{
				relativePosition.x = num2 - controlSize.x;
			}
			else if (this.anchorStyle.IsFlagSet(dfAnchorStyle.CenterHorizontal))
			{
				relativePosition.x = (parentSize.x - controlSize.x) * 0.5f;
			}
			if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Top))
			{
				relativePosition.y = num3;
				if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Bottom))
				{
					vector.y = (this.margins.bottom - this.margins.top) * parentSize.y;
				}
			}
			else if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Bottom))
			{
				relativePosition.y = num4 - controlSize.y;
			}
			else if (this.anchorStyle.IsFlagSet(dfAnchorStyle.CenterVertical))
			{
				relativePosition.y = (parentSize.y - controlSize.y) * 0.5f;
			}
			this.owner.Size = vector;
			this.owner.RelativePosition = relativePosition;
			dfGUIManager manager = this.owner.GetManager();
			if (manager != null && manager.PixelPerfectMode)
			{
				this.owner.MakePixelPerfect(false);
			}
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x00050D8C File Offset: 0x0004EF8C
		private void performLayoutAbsolute(Vector2 parentSize, Vector2 controlSize)
		{
			float num = this.margins.left;
			float num2 = this.margins.top;
			float num3 = num + controlSize.x;
			float num4 = num2 + controlSize.y;
			if (this.anchorStyle.IsFlagSet(dfAnchorStyle.CenterHorizontal))
			{
				num = (float)Mathf.RoundToInt((parentSize.x - controlSize.x) * 0.5f);
				num3 = (float)Mathf.RoundToInt(num + controlSize.x);
			}
			else
			{
				if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Left))
				{
					num = this.margins.left;
					num3 = num + controlSize.x;
				}
				if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Right))
				{
					num3 = parentSize.x - this.margins.right;
					if (!this.anchorStyle.IsFlagSet(dfAnchorStyle.Left))
					{
						num = num3 - controlSize.x;
					}
				}
			}
			if (this.anchorStyle.IsFlagSet(dfAnchorStyle.CenterVertical))
			{
				num2 = (float)Mathf.RoundToInt((parentSize.y - controlSize.y) * 0.5f);
				num4 = (float)Mathf.RoundToInt(num2 + controlSize.y);
			}
			else
			{
				if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Top))
				{
					num2 = this.margins.top;
					num4 = num2 + controlSize.y;
				}
				if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Bottom))
				{
					num4 = parentSize.y - this.margins.bottom;
					if (!this.anchorStyle.IsFlagSet(dfAnchorStyle.Top))
					{
						num2 = num4 - controlSize.y;
					}
				}
			}
			Vector2 vector = new Vector2(Mathf.Max(0f, num3 - num), Mathf.Max(0f, num4 - num2));
			Vector3 vector2 = new Vector3(num, num2);
			this.owner.Size = vector;
			this.owner.setRelativePosition(ref vector2);
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x00050F60 File Offset: 0x0004F160
		private Vector2 getParentSize()
		{
			dfControl parent = this.owner.parent;
			if (parent != null)
			{
				return parent.Size;
			}
			dfGUIManager manager = this.owner.GetManager();
			return manager.GetScreenSize();
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00050FA0 File Offset: 0x0004F1A0
		public override string ToString()
		{
			if (this.owner == null)
			{
				return "NO OWNER FOR ANCHOR";
			}
			dfControl parent = this.owner.parent;
			return string.Format("{0}.{1} - {2}", (!(parent != null)) ? "SCREEN" : parent.name, this.owner.name, this.margins);
		}

		// Token: 0x04000F86 RID: 3974
		[SerializeField]
		protected dfAnchorStyle anchorStyle;

		// Token: 0x04000F87 RID: 3975
		[SerializeField]
		protected dfAnchorMargins margins;

		// Token: 0x04000F88 RID: 3976
		[SerializeField]
		protected dfControl owner;

		// Token: 0x04000F89 RID: 3977
		private int suspendLayoutCounter;

		// Token: 0x04000F8A RID: 3978
		private bool performingLayout;

		// Token: 0x04000F8B RID: 3979
		private bool disposed;

		// Token: 0x04000F8C RID: 3980
		private bool pendingLayoutRequest;
	}
}
