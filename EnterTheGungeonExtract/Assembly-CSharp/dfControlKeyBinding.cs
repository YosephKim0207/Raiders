using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000376 RID: 886
[AddComponentMenu("Daikon Forge/Data Binding/Key Binding")]
[Serializable]
public class dfControlKeyBinding : MonoBehaviour, IDataBindingComponent
{
	// Token: 0x1700033E RID: 830
	// (get) Token: 0x06000E9E RID: 3742 RVA: 0x00044D28 File Offset: 0x00042F28
	// (set) Token: 0x06000E9F RID: 3743 RVA: 0x00044D30 File Offset: 0x00042F30
	public dfControl Control
	{
		get
		{
			return this.control;
		}
		set
		{
			if (this.isBound)
			{
				this.Unbind();
			}
			this.control = value;
		}
	}

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x00044D4C File Offset: 0x00042F4C
	// (set) Token: 0x06000EA1 RID: 3745 RVA: 0x00044D54 File Offset: 0x00042F54
	public KeyCode KeyCode
	{
		get
		{
			return this.keyCode;
		}
		set
		{
			this.keyCode = value;
		}
	}

	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x00044D60 File Offset: 0x00042F60
	// (set) Token: 0x06000EA3 RID: 3747 RVA: 0x00044D68 File Offset: 0x00042F68
	public bool AltPressed
	{
		get
		{
			return this.altPressed;
		}
		set
		{
			this.altPressed = value;
		}
	}

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x00044D74 File Offset: 0x00042F74
	// (set) Token: 0x06000EA5 RID: 3749 RVA: 0x00044D7C File Offset: 0x00042F7C
	public bool ControlPressed
	{
		get
		{
			return this.controlPressed;
		}
		set
		{
			this.controlPressed = value;
		}
	}

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x00044D88 File Offset: 0x00042F88
	// (set) Token: 0x06000EA7 RID: 3751 RVA: 0x00044D90 File Offset: 0x00042F90
	public bool ShiftPressed
	{
		get
		{
			return this.shiftPressed;
		}
		set
		{
			this.shiftPressed = value;
		}
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x00044D9C File Offset: 0x00042F9C
	// (set) Token: 0x06000EA9 RID: 3753 RVA: 0x00044DA4 File Offset: 0x00042FA4
	public dfComponentMemberInfo Target
	{
		get
		{
			return this.target;
		}
		set
		{
			if (this.isBound)
			{
				this.Unbind();
			}
			this.target = value;
		}
	}

	// Token: 0x17000344 RID: 836
	// (get) Token: 0x06000EAA RID: 3754 RVA: 0x00044DC0 File Offset: 0x00042FC0
	public bool IsBound
	{
		get
		{
			return this.isBound;
		}
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x00044DC8 File Offset: 0x00042FC8
	public void Awake()
	{
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x00044DCC File Offset: 0x00042FCC
	public void OnEnable()
	{
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x00044DD0 File Offset: 0x00042FD0
	public void Start()
	{
		if (this.control != null && this.target.IsValid)
		{
			this.Bind();
		}
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x00044DFC File Offset: 0x00042FFC
	public void Bind()
	{
		if (this.isBound)
		{
			this.Unbind();
		}
		if (this.control != null)
		{
			this.control.KeyDown += this.eventSource_KeyDown;
		}
		this.isBound = true;
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x00044E4C File Offset: 0x0004304C
	public void Unbind()
	{
		if (this.control != null)
		{
			this.control.KeyDown -= this.eventSource_KeyDown;
		}
		this.isBound = false;
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x00044E80 File Offset: 0x00043080
	private void eventSource_KeyDown(dfControl sourceControl, dfKeyEventArgs args)
	{
		if (args.KeyCode != this.keyCode)
		{
			return;
		}
		if (args.Shift != this.shiftPressed || args.Control != this.controlPressed || args.Alt != this.altPressed)
		{
			return;
		}
		MethodInfo method = this.target.GetMethod();
		method.Invoke(this.target.Component, null);
	}

	// Token: 0x04000E5B RID: 3675
	[SerializeField]
	protected dfControl control;

	// Token: 0x04000E5C RID: 3676
	[SerializeField]
	protected KeyCode keyCode;

	// Token: 0x04000E5D RID: 3677
	[SerializeField]
	protected bool shiftPressed;

	// Token: 0x04000E5E RID: 3678
	[SerializeField]
	protected bool altPressed;

	// Token: 0x04000E5F RID: 3679
	[SerializeField]
	protected bool controlPressed;

	// Token: 0x04000E60 RID: 3680
	[SerializeField]
	protected dfComponentMemberInfo target;

	// Token: 0x04000E61 RID: 3681
	private bool isBound;
}
