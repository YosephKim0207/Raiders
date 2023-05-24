using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C19 RID: 3097
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUIItem")]
public class tk2dUIItem : MonoBehaviour
{
	// Token: 0x14000091 RID: 145
	// (add) Token: 0x0600426B RID: 17003 RVA: 0x00157794 File Offset: 0x00155994
	// (remove) Token: 0x0600426C RID: 17004 RVA: 0x001577CC File Offset: 0x001559CC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDown;

	// Token: 0x14000092 RID: 146
	// (add) Token: 0x0600426D RID: 17005 RVA: 0x00157804 File Offset: 0x00155A04
	// (remove) Token: 0x0600426E RID: 17006 RVA: 0x0015783C File Offset: 0x00155A3C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnUp;

	// Token: 0x14000093 RID: 147
	// (add) Token: 0x0600426F RID: 17007 RVA: 0x00157874 File Offset: 0x00155A74
	// (remove) Token: 0x06004270 RID: 17008 RVA: 0x001578AC File Offset: 0x00155AAC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnClick;

	// Token: 0x14000094 RID: 148
	// (add) Token: 0x06004271 RID: 17009 RVA: 0x001578E4 File Offset: 0x00155AE4
	// (remove) Token: 0x06004272 RID: 17010 RVA: 0x0015791C File Offset: 0x00155B1C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnRelease;

	// Token: 0x14000095 RID: 149
	// (add) Token: 0x06004273 RID: 17011 RVA: 0x00157954 File Offset: 0x00155B54
	// (remove) Token: 0x06004274 RID: 17012 RVA: 0x0015798C File Offset: 0x00155B8C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnHoverOver;

	// Token: 0x14000096 RID: 150
	// (add) Token: 0x06004275 RID: 17013 RVA: 0x001579C4 File Offset: 0x00155BC4
	// (remove) Token: 0x06004276 RID: 17014 RVA: 0x001579FC File Offset: 0x00155BFC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnHoverOut;

	// Token: 0x14000097 RID: 151
	// (add) Token: 0x06004277 RID: 17015 RVA: 0x00157A34 File Offset: 0x00155C34
	// (remove) Token: 0x06004278 RID: 17016 RVA: 0x00157A6C File Offset: 0x00155C6C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dUIItem> OnDownUIItem;

	// Token: 0x14000098 RID: 152
	// (add) Token: 0x06004279 RID: 17017 RVA: 0x00157AA4 File Offset: 0x00155CA4
	// (remove) Token: 0x0600427A RID: 17018 RVA: 0x00157ADC File Offset: 0x00155CDC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dUIItem> OnUpUIItem;

	// Token: 0x14000099 RID: 153
	// (add) Token: 0x0600427B RID: 17019 RVA: 0x00157B14 File Offset: 0x00155D14
	// (remove) Token: 0x0600427C RID: 17020 RVA: 0x00157B4C File Offset: 0x00155D4C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dUIItem> OnClickUIItem;

	// Token: 0x1400009A RID: 154
	// (add) Token: 0x0600427D RID: 17021 RVA: 0x00157B84 File Offset: 0x00155D84
	// (remove) Token: 0x0600427E RID: 17022 RVA: 0x00157BBC File Offset: 0x00155DBC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dUIItem> OnReleaseUIItem;

	// Token: 0x1400009B RID: 155
	// (add) Token: 0x0600427F RID: 17023 RVA: 0x00157BF4 File Offset: 0x00155DF4
	// (remove) Token: 0x06004280 RID: 17024 RVA: 0x00157C2C File Offset: 0x00155E2C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dUIItem> OnHoverOverUIItem;

	// Token: 0x1400009C RID: 156
	// (add) Token: 0x06004281 RID: 17025 RVA: 0x00157C64 File Offset: 0x00155E64
	// (remove) Token: 0x06004282 RID: 17026 RVA: 0x00157C9C File Offset: 0x00155E9C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dUIItem> OnHoverOutUIItem;

	// Token: 0x06004283 RID: 17027 RVA: 0x00157CD4 File Offset: 0x00155ED4
	private void Awake()
	{
		if (this.isChildOfAnotherUIItem)
		{
			this.UpdateParent();
		}
	}

	// Token: 0x06004284 RID: 17028 RVA: 0x00157CE8 File Offset: 0x00155EE8
	private void Start()
	{
		if (tk2dUIManager.Instance == null)
		{
			UnityEngine.Debug.LogError("Unable to find tk2dUIManager. Please create a tk2dUIManager in the scene before proceeding.");
		}
		if (this.isChildOfAnotherUIItem && this.parentUIItem == null)
		{
			this.UpdateParent();
		}
	}

	// Token: 0x17000A12 RID: 2578
	// (get) Token: 0x06004285 RID: 17029 RVA: 0x00157D28 File Offset: 0x00155F28
	public bool IsPressed
	{
		get
		{
			return this.isPressed;
		}
	}

	// Token: 0x17000A13 RID: 2579
	// (get) Token: 0x06004286 RID: 17030 RVA: 0x00157D30 File Offset: 0x00155F30
	public tk2dUITouch Touch
	{
		get
		{
			return this.touch;
		}
	}

	// Token: 0x17000A14 RID: 2580
	// (get) Token: 0x06004287 RID: 17031 RVA: 0x00157D38 File Offset: 0x00155F38
	public tk2dUIItem ParentUIItem
	{
		get
		{
			return this.parentUIItem;
		}
	}

	// Token: 0x06004288 RID: 17032 RVA: 0x00157D40 File Offset: 0x00155F40
	public void UpdateParent()
	{
		this.parentUIItem = this.GetParentUIItem();
	}

	// Token: 0x06004289 RID: 17033 RVA: 0x00157D50 File Offset: 0x00155F50
	public void ManuallySetParent(tk2dUIItem newParentUIItem)
	{
		this.parentUIItem = newParentUIItem;
	}

	// Token: 0x0600428A RID: 17034 RVA: 0x00157D5C File Offset: 0x00155F5C
	public void RemoveParent()
	{
		this.parentUIItem = null;
	}

	// Token: 0x0600428B RID: 17035 RVA: 0x00157D68 File Offset: 0x00155F68
	public bool Press(tk2dUITouch touch)
	{
		return this.Press(touch, null);
	}

	// Token: 0x0600428C RID: 17036 RVA: 0x00157D74 File Offset: 0x00155F74
	public bool Press(tk2dUITouch touch, tk2dUIItem sentFromChild)
	{
		if (this.isPressed)
		{
			return false;
		}
		if (!this.isPressed)
		{
			this.touch = touch;
			if ((this.registerPressFromChildren || sentFromChild == null) && base.enabled)
			{
				this.isPressed = true;
				if (this.OnDown != null)
				{
					this.OnDown();
				}
				if (this.OnDownUIItem != null)
				{
					this.OnDownUIItem(this);
				}
				this.DoSendMessage(this.SendMessageOnDownMethodName);
			}
			if (this.parentUIItem != null)
			{
				this.parentUIItem.Press(touch, this);
			}
		}
		return true;
	}

	// Token: 0x0600428D RID: 17037 RVA: 0x00157E24 File Offset: 0x00156024
	public void UpdateTouch(tk2dUITouch touch)
	{
		this.touch = touch;
		if (this.parentUIItem != null)
		{
			this.parentUIItem.UpdateTouch(touch);
		}
	}

	// Token: 0x0600428E RID: 17038 RVA: 0x00157E4C File Offset: 0x0015604C
	private void DoSendMessage(string methodName)
	{
		if (this.sendMessageTarget != null && methodName.Length > 0)
		{
			this.sendMessageTarget.SendMessage(methodName, this, SendMessageOptions.RequireReceiver);
		}
	}

	// Token: 0x0600428F RID: 17039 RVA: 0x00157E7C File Offset: 0x0015607C
	public void Release()
	{
		if (this.isPressed)
		{
			this.isPressed = false;
			if (this.OnUp != null)
			{
				this.OnUp();
			}
			if (this.OnUpUIItem != null)
			{
				this.OnUpUIItem(this);
			}
			this.DoSendMessage(this.SendMessageOnUpMethodName);
			if (this.OnClick != null)
			{
				this.OnClick();
			}
			if (this.OnClickUIItem != null)
			{
				this.OnClickUIItem(this);
			}
			this.DoSendMessage(this.SendMessageOnClickMethodName);
		}
		if (this.OnRelease != null)
		{
			this.OnRelease();
		}
		if (this.OnReleaseUIItem != null)
		{
			this.OnReleaseUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnReleaseMethodName);
		if (this.parentUIItem != null)
		{
			this.parentUIItem.Release();
		}
	}

	// Token: 0x06004290 RID: 17040 RVA: 0x00157F64 File Offset: 0x00156164
	public void CurrentOverUIItem(tk2dUIItem overUIItem)
	{
		if (overUIItem != this)
		{
			if (this.isPressed)
			{
				if (!this.CheckIsUIItemChildOfMe(overUIItem))
				{
					this.Exit();
					if (this.parentUIItem != null)
					{
						this.parentUIItem.CurrentOverUIItem(overUIItem);
					}
				}
			}
			else if (this.parentUIItem != null)
			{
				this.parentUIItem.CurrentOverUIItem(overUIItem);
			}
		}
	}

	// Token: 0x06004291 RID: 17041 RVA: 0x00157FDC File Offset: 0x001561DC
	public bool CheckIsUIItemChildOfMe(tk2dUIItem uiItem)
	{
		tk2dUIItem tk2dUIItem = null;
		bool flag = false;
		if (uiItem != null)
		{
			tk2dUIItem = uiItem.parentUIItem;
		}
		while (tk2dUIItem != null)
		{
			if (tk2dUIItem == this)
			{
				flag = true;
				break;
			}
			tk2dUIItem = tk2dUIItem.parentUIItem;
		}
		return flag;
	}

	// Token: 0x06004292 RID: 17042 RVA: 0x0015802C File Offset: 0x0015622C
	public void Exit()
	{
		if (this.isPressed)
		{
			this.isPressed = false;
			if (this.OnUp != null)
			{
				this.OnUp();
			}
			if (this.OnUpUIItem != null)
			{
				this.OnUpUIItem(this);
			}
			this.DoSendMessage(this.SendMessageOnUpMethodName);
		}
	}

	// Token: 0x06004293 RID: 17043 RVA: 0x00158084 File Offset: 0x00156284
	public bool HoverOver(tk2dUIItem prevHover)
	{
		bool flag = false;
		tk2dUIItem tk2dUIItem = null;
		if (!this.isHoverOver)
		{
			if (this.OnHoverOver != null)
			{
				this.OnHoverOver();
			}
			if (this.OnHoverOverUIItem != null)
			{
				this.OnHoverOverUIItem(this);
			}
			this.isHoverOver = true;
		}
		if (prevHover == this)
		{
			flag = true;
		}
		if (this.parentUIItem != null && this.parentUIItem.isHoverEnabled)
		{
			tk2dUIItem = this.parentUIItem;
		}
		if (tk2dUIItem == null)
		{
			return flag;
		}
		return tk2dUIItem.HoverOver(prevHover) || flag;
	}

	// Token: 0x06004294 RID: 17044 RVA: 0x00158128 File Offset: 0x00156328
	public void HoverOut(tk2dUIItem currHoverButton)
	{
		if (this.isHoverOver)
		{
			if (this.OnHoverOut != null)
			{
				this.OnHoverOut();
			}
			if (this.OnHoverOutUIItem != null)
			{
				this.OnHoverOutUIItem(this);
			}
			this.isHoverOver = false;
		}
		if (this.parentUIItem != null && this.parentUIItem.isHoverEnabled)
		{
			if (currHoverButton == null)
			{
				this.parentUIItem.HoverOut(currHoverButton);
			}
			else if (!this.parentUIItem.CheckIsUIItemChildOfMe(currHoverButton) && currHoverButton != this.parentUIItem)
			{
				this.parentUIItem.HoverOut(currHoverButton);
			}
		}
	}

	// Token: 0x06004295 RID: 17045 RVA: 0x001581E0 File Offset: 0x001563E0
	private tk2dUIItem GetParentUIItem()
	{
		Transform transform = base.transform.parent;
		while (transform != null)
		{
			tk2dUIItem component = transform.GetComponent<tk2dUIItem>();
			if (component != null)
			{
				return component;
			}
			transform = transform.parent;
		}
		return null;
	}

	// Token: 0x06004296 RID: 17046 RVA: 0x00158228 File Offset: 0x00156428
	public void SimulateClick()
	{
		if (this.OnDown != null)
		{
			this.OnDown();
		}
		if (this.OnDownUIItem != null)
		{
			this.OnDownUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnDownMethodName);
		if (this.OnUp != null)
		{
			this.OnUp();
		}
		if (this.OnUpUIItem != null)
		{
			this.OnUpUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnUpMethodName);
		if (this.OnClick != null)
		{
			this.OnClick();
		}
		if (this.OnClickUIItem != null)
		{
			this.OnClickUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnClickMethodName);
		if (this.OnRelease != null)
		{
			this.OnRelease();
		}
		if (this.OnReleaseUIItem != null)
		{
			this.OnReleaseUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnReleaseMethodName);
	}

	// Token: 0x06004297 RID: 17047 RVA: 0x0015831C File Offset: 0x0015651C
	public void InternalSetIsChildOfAnotherUIItem(bool state)
	{
		this.isChildOfAnotherUIItem = state;
	}

	// Token: 0x06004298 RID: 17048 RVA: 0x00158328 File Offset: 0x00156528
	public bool InternalGetIsChildOfAnotherUIItem()
	{
		return this.isChildOfAnotherUIItem;
	}

	// Token: 0x040034CE RID: 13518
	public GameObject sendMessageTarget;

	// Token: 0x040034CF RID: 13519
	public string SendMessageOnDownMethodName = string.Empty;

	// Token: 0x040034D0 RID: 13520
	public string SendMessageOnUpMethodName = string.Empty;

	// Token: 0x040034D1 RID: 13521
	public string SendMessageOnClickMethodName = string.Empty;

	// Token: 0x040034D2 RID: 13522
	public string SendMessageOnReleaseMethodName = string.Empty;

	// Token: 0x040034D3 RID: 13523
	[SerializeField]
	private bool isChildOfAnotherUIItem;

	// Token: 0x040034D4 RID: 13524
	public bool registerPressFromChildren;

	// Token: 0x040034D5 RID: 13525
	public bool isHoverEnabled;

	// Token: 0x040034D6 RID: 13526
	public Transform[] editorExtraBounds = new Transform[0];

	// Token: 0x040034D7 RID: 13527
	public Transform[] editorIgnoreBounds = new Transform[0];

	// Token: 0x040034D8 RID: 13528
	private bool isPressed;

	// Token: 0x040034D9 RID: 13529
	private bool isHoverOver;

	// Token: 0x040034DA RID: 13530
	private tk2dUITouch touch;

	// Token: 0x040034DB RID: 13531
	private tk2dUIItem parentUIItem;
}
