using System;
using UnityEngine;

// Token: 0x020003EE RID: 1006
[ExecuteInEditMode]
[AddComponentMenu("Daikon Forge/User Interface/Resize Handle")]
[Serializable]
public class dfResizeHandle : dfControl
{
	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06001553 RID: 5459 RVA: 0x0006338C File Offset: 0x0006158C
	// (set) Token: 0x06001554 RID: 5460 RVA: 0x000633D4 File Offset: 0x000615D4
	public dfAtlas Atlas
	{
		get
		{
			if (this.atlas == null)
			{
				dfGUIManager manager = base.GetManager();
				if (manager != null)
				{
					return this.atlas = manager.DefaultAtlas;
				}
			}
			return this.atlas;
		}
		set
		{
			if (!dfAtlas.Equals(value, this.atlas))
			{
				this.atlas = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x06001555 RID: 5461 RVA: 0x000633F4 File Offset: 0x000615F4
	// (set) Token: 0x06001556 RID: 5462 RVA: 0x000633FC File Offset: 0x000615FC
	public string BackgroundSprite
	{
		get
		{
			return this.backgroundSprite;
		}
		set
		{
			if (value != this.backgroundSprite)
			{
				this.backgroundSprite = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x06001557 RID: 5463 RVA: 0x0006341C File Offset: 0x0006161C
	// (set) Token: 0x06001558 RID: 5464 RVA: 0x00063424 File Offset: 0x00061624
	public dfResizeHandle.ResizeEdge Edges
	{
		get
		{
			return this.edges;
		}
		set
		{
			this.edges = value;
		}
	}

	// Token: 0x06001559 RID: 5465 RVA: 0x00063430 File Offset: 0x00061630
	public override void Start()
	{
		base.Start();
		if (base.Size.magnitude <= 1E-45f)
		{
			base.Size = new Vector2(25f, 25f);
			if (base.Parent != null)
			{
				base.RelativePosition = base.Parent.Size - base.Size;
				base.Anchor = dfAnchorStyle.Bottom | dfAnchorStyle.Right;
			}
		}
	}

	// Token: 0x0600155A RID: 5466 RVA: 0x000634AC File Offset: 0x000616AC
	protected override void OnRebuildRenderData()
	{
		if (this.Atlas == null || string.IsNullOrEmpty(this.backgroundSprite))
		{
			return;
		}
		dfAtlas.ItemInfo itemInfo = this.Atlas[this.backgroundSprite];
		if (itemInfo == null)
		{
			return;
		}
		this.renderData.Material = this.Atlas.Material;
		Color32 color = base.ApplyOpacity((!base.IsEnabled) ? this.disabledColor : this.color);
		dfSprite.RenderOptions renderOptions = new dfSprite.RenderOptions
		{
			atlas = this.atlas,
			color = color,
			fillAmount = 1f,
			flip = dfSpriteFlip.None,
			offset = this.pivot.TransformToUpperLeft(base.Size),
			pixelsToUnits = base.PixelsToUnits(),
			size = base.Size,
			spriteInfo = itemInfo
		};
		if (itemInfo.border.horizontal == 0 && itemInfo.border.vertical == 0)
		{
			dfSprite.renderSprite(this.renderData, renderOptions);
		}
		else
		{
			dfSlicedSprite.renderSprite(this.renderData, renderOptions);
		}
	}

	// Token: 0x0600155B RID: 5467 RVA: 0x000635E0 File Offset: 0x000617E0
	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		args.Use();
		Plane plane = new Plane(this.parent.transform.TransformDirection(Vector3.back), this.parent.transform.position);
		Ray ray = args.Ray;
		float num = 0f;
		plane.Raycast(args.Ray, out num);
		this.mouseAnchorPos = ray.origin + ray.direction * num;
		this.startSize = this.parent.Size;
		this.startPosition = this.parent.RelativePosition;
		this.minEdgePos = this.startPosition;
		this.maxEdgePos = this.startPosition + this.startSize;
		Vector2 vector = this.parent.CalculateMinimumSize();
		Vector2 vector2 = this.parent.MaximumSize;
		if (vector2.magnitude <= 1E-45f)
		{
			vector2 = Vector2.one * 2048f;
		}
		if ((this.Edges & dfResizeHandle.ResizeEdge.Left) == dfResizeHandle.ResizeEdge.Left)
		{
			this.minEdgePos.x = this.maxEdgePos.x - vector2.x;
			this.maxEdgePos.x = this.maxEdgePos.x - vector.x;
		}
		else if ((this.Edges & dfResizeHandle.ResizeEdge.Right) == dfResizeHandle.ResizeEdge.Right)
		{
			this.minEdgePos.x = this.startPosition.x + vector.x;
			this.maxEdgePos.x = this.startPosition.x + vector2.x;
		}
		if ((this.Edges & dfResizeHandle.ResizeEdge.Top) == dfResizeHandle.ResizeEdge.Top)
		{
			this.minEdgePos.y = this.maxEdgePos.y - vector2.y;
			this.maxEdgePos.y = this.maxEdgePos.y - vector.y;
		}
		else if ((this.Edges & dfResizeHandle.ResizeEdge.Bottom) == dfResizeHandle.ResizeEdge.Bottom)
		{
			this.minEdgePos.y = this.startPosition.y + vector.y;
			this.maxEdgePos.y = this.startPosition.y + vector2.y;
		}
		base.OnMouseDown(args);
	}

	// Token: 0x0600155C RID: 5468 RVA: 0x00063820 File Offset: 0x00061A20
	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		if (!args.Buttons.IsSet(dfMouseButtons.Left) || this.Edges == dfResizeHandle.ResizeEdge.None)
		{
			return;
		}
		args.Use();
		Ray ray = args.Ray;
		float num = 0f;
		Vector3 vector = base.GetCamera().transform.TransformDirection(Vector3.back);
		Plane plane = new Plane(vector, this.mouseAnchorPos);
		plane.Raycast(ray, out num);
		float num2 = base.PixelsToUnits();
		Vector3 vector2 = ray.origin + ray.direction * num;
		Vector3 vector3 = (vector2 - this.mouseAnchorPos) / num2;
		vector3.y *= -1f;
		float num3 = this.startPosition.x;
		float num4 = this.startPosition.y;
		float num5 = num3 + this.startSize.x;
		float num6 = num4 + this.startSize.y;
		if ((this.Edges & dfResizeHandle.ResizeEdge.Left) == dfResizeHandle.ResizeEdge.Left)
		{
			num3 = Mathf.Min(this.maxEdgePos.x, Mathf.Max(this.minEdgePos.x, num3 + vector3.x));
		}
		else if ((this.Edges & dfResizeHandle.ResizeEdge.Right) == dfResizeHandle.ResizeEdge.Right)
		{
			num5 = Mathf.Min(this.maxEdgePos.x, Mathf.Max(this.minEdgePos.x, num5 + vector3.x));
		}
		if ((this.Edges & dfResizeHandle.ResizeEdge.Top) == dfResizeHandle.ResizeEdge.Top)
		{
			num4 = Mathf.Min(this.maxEdgePos.y, Mathf.Max(this.minEdgePos.y, num4 + vector3.y));
		}
		else if ((this.Edges & dfResizeHandle.ResizeEdge.Bottom) == dfResizeHandle.ResizeEdge.Bottom)
		{
			num6 = Mathf.Min(this.maxEdgePos.y, Mathf.Max(this.minEdgePos.y, num6 + vector3.y));
		}
		this.parent.Size = new Vector2(num5 - num3, num6 - num4);
		this.parent.RelativePosition = new Vector3(num3, num4, 0f);
		if (this.parent.GetManager().PixelPerfectMode)
		{
			this.parent.MakePixelPerfect();
		}
	}

	// Token: 0x0600155D RID: 5469 RVA: 0x00063A58 File Offset: 0x00061C58
	protected internal override void OnMouseUp(dfMouseEventArgs args)
	{
		base.Parent.MakePixelPerfect();
		args.Use();
		base.OnMouseUp(args);
	}

	// Token: 0x04001226 RID: 4646
	[SerializeField]
	protected dfAtlas atlas;

	// Token: 0x04001227 RID: 4647
	[SerializeField]
	protected string backgroundSprite = string.Empty;

	// Token: 0x04001228 RID: 4648
	[SerializeField]
	protected dfResizeHandle.ResizeEdge edges = dfResizeHandle.ResizeEdge.Right | dfResizeHandle.ResizeEdge.Bottom;

	// Token: 0x04001229 RID: 4649
	private Vector3 mouseAnchorPos;

	// Token: 0x0400122A RID: 4650
	private Vector3 startPosition;

	// Token: 0x0400122B RID: 4651
	private Vector2 startSize;

	// Token: 0x0400122C RID: 4652
	private Vector2 minEdgePos;

	// Token: 0x0400122D RID: 4653
	private Vector2 maxEdgePos;

	// Token: 0x020003EF RID: 1007
	[Flags]
	public enum ResizeEdge
	{
		// Token: 0x0400122F RID: 4655
		None = 0,
		// Token: 0x04001230 RID: 4656
		Left = 1,
		// Token: 0x04001231 RID: 4657
		Right = 2,
		// Token: 0x04001232 RID: 4658
		Top = 4,
		// Token: 0x04001233 RID: 4659
		Bottom = 8
	}
}
