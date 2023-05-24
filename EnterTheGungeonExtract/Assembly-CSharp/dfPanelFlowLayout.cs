using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003EA RID: 1002
[AddComponentMenu("Daikon Forge/User Interface/Panel Addon/Flow Layout")]
[ExecuteInEditMode]
public class dfPanelFlowLayout : MonoBehaviour
{
	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x06001500 RID: 5376 RVA: 0x00061768 File Offset: 0x0005F968
	// (set) Token: 0x06001501 RID: 5377 RVA: 0x00061770 File Offset: 0x0005F970
	public dfControlOrientation Direction
	{
		get
		{
			return this.flowDirection;
		}
		set
		{
			if (value != this.flowDirection)
			{
				this.flowDirection = value;
				this.PerformLayout();
			}
		}
	}

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x06001502 RID: 5378 RVA: 0x0006178C File Offset: 0x0005F98C
	// (set) Token: 0x06001503 RID: 5379 RVA: 0x00061794 File Offset: 0x0005F994
	public Vector2 ItemSpacing
	{
		get
		{
			return this.itemSpacing;
		}
		set
		{
			value = Vector2.Max(value, Vector2.zero);
			if (!object.Equals(value, this.itemSpacing))
			{
				this.itemSpacing = value;
				this.PerformLayout();
			}
		}
	}

	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x06001504 RID: 5380 RVA: 0x000617CC File Offset: 0x0005F9CC
	// (set) Token: 0x06001505 RID: 5381 RVA: 0x000617EC File Offset: 0x0005F9EC
	public RectOffset BorderPadding
	{
		get
		{
			if (this.borderPadding == null)
			{
				this.borderPadding = new RectOffset();
			}
			return this.borderPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.borderPadding))
			{
				this.borderPadding = value;
				this.PerformLayout();
			}
		}
	}

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x06001506 RID: 5382 RVA: 0x00061814 File Offset: 0x0005FA14
	// (set) Token: 0x06001507 RID: 5383 RVA: 0x0006181C File Offset: 0x0005FA1C
	public bool HideClippedControls
	{
		get
		{
			return this.hideClippedControls;
		}
		set
		{
			if (value != this.hideClippedControls)
			{
				this.hideClippedControls = value;
				this.PerformLayout();
			}
		}
	}

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x06001508 RID: 5384 RVA: 0x00061838 File Offset: 0x0005FA38
	// (set) Token: 0x06001509 RID: 5385 RVA: 0x00061840 File Offset: 0x0005FA40
	public int MaxLayoutSize
	{
		get
		{
			return this.maxLayoutSize;
		}
		set
		{
			if (value != this.maxLayoutSize)
			{
				this.maxLayoutSize = value;
				this.PerformLayout();
			}
		}
	}

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x0600150A RID: 5386 RVA: 0x0006185C File Offset: 0x0005FA5C
	public List<dfControl> ExcludedControls
	{
		get
		{
			return this.excludedControls;
		}
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x00061864 File Offset: 0x0005FA64
	public void OnEnable()
	{
		this.panel = base.GetComponent<dfPanel>();
		if (this.panel == null)
		{
			Debug.LogError("The " + base.GetType().Name + " component requires a dfPanel component.", base.gameObject);
			base.enabled = false;
			return;
		}
		this.panel.SizeChanged += this.OnSizeChanged;
	}

	// Token: 0x0600150C RID: 5388 RVA: 0x000618D4 File Offset: 0x0005FAD4
	public void OnDisable()
	{
		if (this.panel != null)
		{
			this.panel.SizeChanged -= this.OnSizeChanged;
			this.panel = null;
		}
	}

	// Token: 0x0600150D RID: 5389 RVA: 0x00061908 File Offset: 0x0005FB08
	public void OnControlAdded(dfControl container, dfControl child)
	{
		child.ZOrderChanged += this.child_ZOrderChanged;
		child.SizeChanged += this.child_SizeChanged;
		this.PerformLayout();
	}

	// Token: 0x0600150E RID: 5390 RVA: 0x00061934 File Offset: 0x0005FB34
	public void OnControlRemoved(dfControl container, dfControl child)
	{
		child.ZOrderChanged -= this.child_ZOrderChanged;
		child.SizeChanged -= this.child_SizeChanged;
		this.PerformLayout();
	}

	// Token: 0x0600150F RID: 5391 RVA: 0x00061960 File Offset: 0x0005FB60
	public void OnSizeChanged(dfControl control, Vector2 value)
	{
		this.PerformLayout();
	}

	// Token: 0x06001510 RID: 5392 RVA: 0x00061968 File Offset: 0x0005FB68
	private void child_SizeChanged(dfControl control, Vector2 value)
	{
		this.PerformLayout();
	}

	// Token: 0x06001511 RID: 5393 RVA: 0x00061970 File Offset: 0x0005FB70
	private void child_ZOrderChanged(dfControl control, int value)
	{
		this.PerformLayout();
	}

	// Token: 0x06001512 RID: 5394 RVA: 0x00061978 File Offset: 0x0005FB78
	public void PerformLayout()
	{
		if (this.panel == null)
		{
			this.panel = base.GetComponent<dfPanel>();
		}
		Vector3 vector = new Vector3((float)this.borderPadding.left, (float)this.borderPadding.top);
		bool flag = true;
		float num = ((this.flowDirection != dfControlOrientation.Horizontal || this.maxLayoutSize <= 0) ? (this.panel.Width - (float)this.borderPadding.right) : ((float)this.maxLayoutSize));
		float num2 = ((this.flowDirection != dfControlOrientation.Vertical || this.maxLayoutSize <= 0) ? (this.panel.Height - (float)this.borderPadding.bottom) : ((float)this.maxLayoutSize));
		int num3 = 0;
		dfList<dfControl> controls = this.panel.Controls;
		int i = 0;
		while (i < controls.Count)
		{
			dfControl dfControl = controls[i];
			if (dfControl.enabled && dfControl.gameObject.activeSelf && !this.excludedControls.Contains(dfControl))
			{
				if (!flag)
				{
					if (this.flowDirection == dfControlOrientation.Horizontal)
					{
						vector.x += this.itemSpacing.x;
					}
					else
					{
						vector.y += this.itemSpacing.y;
					}
				}
				if (this.flowDirection == dfControlOrientation.Horizontal)
				{
					if (!flag && vector.x + dfControl.Width > num + 1E-45f)
					{
						vector.x = (float)this.borderPadding.left;
						vector.y += (float)num3;
						num3 = 0;
					}
				}
				else if (!flag && vector.y + dfControl.Height > num2 + 1E-45f)
				{
					vector.y = (float)this.borderPadding.top;
					vector.x += (float)num3;
					num3 = 0;
				}
				dfControl.RelativePosition = vector;
				if (this.flowDirection == dfControlOrientation.Horizontal)
				{
					vector.x += dfControl.Width;
					num3 = Mathf.Max(Mathf.CeilToInt(dfControl.Height + this.itemSpacing.y), num3);
				}
				else
				{
					vector.y += dfControl.Height;
					num3 = Mathf.Max(Mathf.CeilToInt(dfControl.Width + this.itemSpacing.x), num3);
				}
				dfControl.IsVisible = this.canShowControlUnclipped(dfControl);
			}
			i++;
			flag = false;
		}
	}

	// Token: 0x06001513 RID: 5395 RVA: 0x00061C28 File Offset: 0x0005FE28
	private bool canShowControlUnclipped(dfControl control)
	{
		if (!this.hideClippedControls)
		{
			return true;
		}
		Vector3 relativePosition = control.RelativePosition;
		return relativePosition.x + control.Width < this.panel.Width - (float)this.borderPadding.right && relativePosition.y + control.Height < this.panel.Height - (float)this.borderPadding.bottom;
	}

	// Token: 0x04001204 RID: 4612
	[SerializeField]
	protected RectOffset borderPadding = new RectOffset();

	// Token: 0x04001205 RID: 4613
	[SerializeField]
	protected Vector2 itemSpacing = default(Vector2);

	// Token: 0x04001206 RID: 4614
	[SerializeField]
	protected dfControlOrientation flowDirection;

	// Token: 0x04001207 RID: 4615
	[SerializeField]
	protected bool hideClippedControls;

	// Token: 0x04001208 RID: 4616
	[SerializeField]
	protected int maxLayoutSize;

	// Token: 0x04001209 RID: 4617
	[SerializeField]
	protected List<dfControl> excludedControls = new List<dfControl>();

	// Token: 0x0400120A RID: 4618
	private dfPanel panel;
}
