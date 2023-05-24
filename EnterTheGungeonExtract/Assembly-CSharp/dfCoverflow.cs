using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000449 RID: 1097
[ExecuteInEditMode]
[RequireComponent(typeof(dfPanel))]
[AddComponentMenu("Daikon Forge/Examples/Coverflow/Scroller")]
[Serializable]
public class dfCoverflow : MonoBehaviour
{
	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06001931 RID: 6449 RVA: 0x000764E0 File Offset: 0x000746E0
	// (remove) Token: 0x06001932 RID: 6450 RVA: 0x00076518 File Offset: 0x00074718
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ValueChangedEventHandler<int> SelectedIndexChanged;

	// Token: 0x06001933 RID: 6451 RVA: 0x00076550 File Offset: 0x00074750
	public void OnEnable()
	{
		this.container = base.GetComponent<dfPanel>();
		this.container.Pivot = dfPivotPoint.MiddleCenter;
		this.container.ControlAdded += this.container_ControlCollectionChanged;
		this.container.ControlRemoved += this.container_ControlCollectionChanged;
		this.controls = new dfList<dfControl>(this.container.Controls);
		if (this.rotationCurve.keys.Length == 0)
		{
			this.rotationCurve.AddKey(0f, 0f);
			this.rotationCurve.AddKey(1f, 1f);
		}
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x000765F8 File Offset: 0x000747F8
	public void OnDisable()
	{
		if (this.container != null)
		{
			this.container.ControlAdded -= this.container_ControlCollectionChanged;
			this.container.ControlRemoved -= this.container_ControlCollectionChanged;
		}
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x00076644 File Offset: 0x00074844
	public void Update()
	{
		if (this.controls == null || this.controls.Count == 0)
		{
			this.setSelectedIndex(0);
			return;
		}
		if (this.isMouseDown)
		{
			dfControl dfControl = this.findClosestItemToCenter();
			if (dfControl != null)
			{
				this.setSelectedIndex(this.controls.IndexOf(dfControl));
				this.lastSelected = this.selectedIndex;
			}
		}
		int num = Mathf.Max(0, this.selectedIndex);
		num = Mathf.Min(this.controls.Count - 1, num);
		this.setSelectedIndex(num);
		if (Application.isPlaying)
		{
			this.updateSlides();
		}
		else
		{
			this.layoutSlidesForEditor();
		}
	}

	// Token: 0x06001936 RID: 6454 RVA: 0x000766F4 File Offset: 0x000748F4
	public void OnMouseEnter(dfControl control, dfMouseEventArgs args)
	{
		this.touchStartPosition = args.Position;
	}

	// Token: 0x06001937 RID: 6455 RVA: 0x00076704 File Offset: 0x00074904
	public void OnMouseDown(dfControl control, dfMouseEventArgs args)
	{
		this.touchStartPosition = args.Position;
		this.isMouseDown = true;
	}

	// Token: 0x06001938 RID: 6456 RVA: 0x0007671C File Offset: 0x0007491C
	public void OnDragStart(dfControl control, dfDragEventArgs args)
	{
		if (args.Used)
		{
			this.isMouseDown = false;
		}
	}

	// Token: 0x06001939 RID: 6457 RVA: 0x00076730 File Offset: 0x00074930
	public void OnMouseUp(dfControl control, dfMouseEventArgs args)
	{
		if (this.isMouseDown)
		{
			this.isMouseDown = false;
			dfControl dfControl = this.findClosestItemToCenter();
			if (dfControl != null)
			{
				this.lastSelected = -1;
				this.setSelectedIndex(this.controls.IndexOf(dfControl));
			}
		}
	}

	// Token: 0x0600193A RID: 6458 RVA: 0x0007677C File Offset: 0x0007497C
	public void OnMouseMove(dfControl control, dfMouseEventArgs args)
	{
		if (!(args is dfTouchEventArgs) && !this.isMouseDown)
		{
			return;
		}
		if (args.Used || (args.Position - this.touchStartPosition).magnitude <= 5f)
		{
			return;
		}
		this.currentX += args.MoveDelta.x;
		args.Use();
	}

	// Token: 0x0600193B RID: 6459 RVA: 0x000767FC File Offset: 0x000749FC
	public void OnResolutionChanged(dfControl control, Vector2 previousResolution, Vector2 currentResolution)
	{
		this.lastSelected = -1;
	}

	// Token: 0x0600193C RID: 6460 RVA: 0x00076808 File Offset: 0x00074A08
	private void container_ControlCollectionChanged(dfControl panel, dfControl child)
	{
		this.controls = new dfList<dfControl>(panel.Controls);
		if (this.autoSelectOnStart && Application.isPlaying)
		{
			this.setSelectedIndex(this.controls.Count / 2);
		}
	}

	// Token: 0x0600193D RID: 6461 RVA: 0x00076844 File Offset: 0x00074A44
	public void OnKeyDown(dfControl sender, dfKeyEventArgs args)
	{
		if (!args.Used)
		{
			if (args.KeyCode == KeyCode.RightArrow)
			{
				this.setSelectedIndex(this.selectedIndex + 1);
			}
			else if (args.KeyCode == KeyCode.LeftArrow)
			{
				this.setSelectedIndex(this.selectedIndex - 1);
			}
			else if (args.KeyCode == KeyCode.Home)
			{
				this.setSelectedIndex(0);
			}
			else if (args.KeyCode == KeyCode.End)
			{
				this.setSelectedIndex(this.controls.Count - 1);
			}
		}
	}

	// Token: 0x0600193E RID: 6462 RVA: 0x000768E4 File Offset: 0x00074AE4
	public void OnMouseWheel(dfControl sender, dfMouseEventArgs args)
	{
		if (args.Used)
		{
			return;
		}
		args.Use();
		this.container.Focus(true);
		this.setSelectedIndex(this.selectedIndex - (int)Mathf.Sign(args.WheelDelta));
	}

	// Token: 0x0600193F RID: 6463 RVA: 0x00076920 File Offset: 0x00074B20
	public void OnClick(dfControl sender, dfMouseEventArgs args)
	{
		if (args.Source == this.container)
		{
			return;
		}
		if (Vector2.Distance(args.Position, this.touchStartPosition) > 20f)
		{
			return;
		}
		dfControl dfControl = args.Source;
		while (dfControl != null && !this.controls.Contains(dfControl))
		{
			dfControl = dfControl.Parent;
		}
		if (dfControl != null)
		{
			this.lastSelected = -1;
			this.setSelectedIndex(this.controls.IndexOf(dfControl));
			this.isMouseDown = false;
		}
	}

	// Token: 0x06001940 RID: 6464 RVA: 0x000769BC File Offset: 0x00074BBC
	private void setSelectedIndex(int value)
	{
		if (value != this.selectedIndex)
		{
			this.selectedIndex = value;
			if (this.SelectedIndexChanged != null)
			{
				this.SelectedIndexChanged(this, value);
			}
			base.gameObject.Signal("OnSelectedIndexChanged", new object[] { this, value });
		}
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x00076A18 File Offset: 0x00074C18
	private dfControl findClosestItemToCenter()
	{
		float num = float.MaxValue;
		dfControl dfControl = null;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl dfControl2 = this.controls[i];
			float sqrMagnitude = (dfControl2.transform.position - this.container.transform.position).sqrMagnitude;
			if (sqrMagnitude <= num)
			{
				num = sqrMagnitude;
				dfControl = dfControl2;
			}
		}
		return dfControl;
	}

	// Token: 0x06001942 RID: 6466 RVA: 0x00076A90 File Offset: 0x00074C90
	private void layoutSlidesForEditor()
	{
		dfList<dfControl> dfList = this.container.Controls;
		int num = 0;
		float num2 = (this.container.Height - (float)this.itemSize) * 0.5f;
		Vector2 vector = Vector2.one * (float)this.itemSize;
		for (int i = 0; i < dfList.Count; i++)
		{
			dfList[i].Size = vector;
			dfList[i].RelativePosition = new Vector3((float)num, num2);
			num += this.itemSize + Mathf.Max(0, this.spacing);
		}
	}

	// Token: 0x06001943 RID: 6467 RVA: 0x00076B2C File Offset: 0x00074D2C
	private void updateSlides()
	{
		if (this.currentX == null || this.selectedIndex != this.lastSelected)
		{
			float num = ((this.currentX == null) ? 0f : this.currentX.Value);
			this.currentX = new dfAnimatedFloat(num, this.calculateTargetPosition(), this.time)
			{
				EasingType = dfEasingType.SineEaseOut
			};
			this.lastSelected = this.selectedIndex;
		}
		float num2 = (this.container.Height - (float)this.itemSize) * 0.5f;
		Vector3 vector = new Vector3(this.currentX, num2);
		int count = this.controls.Count;
		for (int i = 0; i < count; i++)
		{
			dfControl dfControl = this.controls[i];
			dfControl.Size = new Vector2((float)this.itemSize, (float)this.itemSize);
			dfControl.RelativePosition = vector;
			dfControl.Pivot = dfPivotPoint.MiddleCenter;
			if (Application.isPlaying)
			{
				Quaternion quaternion = Quaternion.Euler(0f, this.calcHorzRotation(vector.x), 0f);
				dfControl.transform.localRotation = quaternion;
				float num3 = this.calcScale(vector.x);
				dfControl.transform.localScale = Vector3.one * num3;
				dfControl.Opacity = this.calcItemOpacity(vector.x);
			}
			else
			{
				dfControl.transform.localScale = Vector3.one;
				dfControl.transform.localRotation = Quaternion.identity;
			}
			vector.x += (float)(this.itemSize + this.spacing);
		}
		if (Application.isPlaying)
		{
			int num4 = 0;
			for (int j = 0; j < this.selectedIndex; j++)
			{
				this.controls[j].ZOrder = num4++;
			}
			for (int k = count - 1; k >= this.selectedIndex; k--)
			{
				this.controls[k].ZOrder = num4++;
			}
		}
	}

	// Token: 0x06001944 RID: 6468 RVA: 0x00076D5C File Offset: 0x00074F5C
	private float calcScale(float offset)
	{
		float num = (this.container.Width - (float)this.itemSize) * 0.5f;
		float num2 = Mathf.Abs(num - offset);
		int totalSize = this.getTotalSize();
		return Mathf.Max(1f - num2 / (float)totalSize, 0.85f);
	}

	// Token: 0x06001945 RID: 6469 RVA: 0x00076DA8 File Offset: 0x00074FA8
	private float calcItemOpacity(float offset)
	{
		float num = (this.container.Width - (float)this.itemSize) * 0.5f;
		float num2 = Mathf.Abs(num - offset);
		int totalSize = this.getTotalSize();
		float num3 = num2 / (float)totalSize;
		return 1f - this.opacityCurve.Evaluate(num3);
	}

	// Token: 0x06001946 RID: 6470 RVA: 0x00076DF8 File Offset: 0x00074FF8
	private float calcHorzRotation(float offset)
	{
		float num = (this.container.Width - (float)this.itemSize) * 0.5f;
		float num2 = Mathf.Abs(num - offset);
		float num3 = Mathf.Sign(num - offset);
		int totalSize = this.getTotalSize();
		float num4 = num2 / (float)totalSize;
		num4 = this.rotationCurve.Evaluate(num4);
		return num4 * 90f * -num3;
	}

	// Token: 0x06001947 RID: 6471 RVA: 0x00076E5C File Offset: 0x0007505C
	private int getTotalSize()
	{
		int count = this.controls.Count;
		return count * this.itemSize + Mathf.Max(count, 0) * this.spacing;
	}

	// Token: 0x06001948 RID: 6472 RVA: 0x00076E90 File Offset: 0x00075090
	private float calculateTargetPosition()
	{
		float num = (this.container.Width - (float)this.itemSize) * 0.5f;
		float num2 = num - (float)(this.selectedIndex * this.itemSize);
		if (this.selectedIndex > 0)
		{
			num2 -= (float)(this.selectedIndex * this.spacing);
		}
		return num2;
	}

	// Token: 0x040013D0 RID: 5072
	[SerializeField]
	public int selectedIndex;

	// Token: 0x040013D1 RID: 5073
	[SerializeField]
	public int itemSize = 200;

	// Token: 0x040013D2 RID: 5074
	[SerializeField]
	public float time = 0.33f;

	// Token: 0x040013D3 RID: 5075
	[SerializeField]
	public int spacing = 5;

	// Token: 0x040013D4 RID: 5076
	[SerializeField]
	protected AnimationCurve rotationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x040013D5 RID: 5077
	[SerializeField]
	protected AnimationCurve opacityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x040013D6 RID: 5078
	[SerializeField]
	protected bool autoSelectOnStart = true;

	// Token: 0x040013D7 RID: 5079
	private dfPanel container;

	// Token: 0x040013D8 RID: 5080
	private dfList<dfControl> controls;

	// Token: 0x040013D9 RID: 5081
	private dfAnimatedFloat currentX;

	// Token: 0x040013DA RID: 5082
	private Vector2 touchStartPosition;

	// Token: 0x040013DB RID: 5083
	private int lastSelected = -1;

	// Token: 0x040013DC RID: 5084
	private bool isMouseDown;
}
