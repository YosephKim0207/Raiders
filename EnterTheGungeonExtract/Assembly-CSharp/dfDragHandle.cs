using System;
using UnityEngine;

// Token: 0x0200039B RID: 923
[AddComponentMenu("Daikon Forge/User Interface/Drag Handle")]
[ExecuteInEditMode]
[Serializable]
public class dfDragHandle : dfControl
{
	// Token: 0x0600115E RID: 4446 RVA: 0x000510D8 File Offset: 0x0004F2D8
	public override void Start()
	{
		base.Start();
		if (base.Size.magnitude <= 1E-45f)
		{
			if (base.Parent != null)
			{
				base.Size = new Vector2(base.Parent.Width, 30f);
				base.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Left | dfAnchorStyle.Right;
				base.RelativePosition = Vector2.zero;
			}
			else
			{
				base.Size = new Vector2(200f, 25f);
			}
		}
	}

	// Token: 0x0600115F RID: 4447 RVA: 0x00051164 File Offset: 0x0004F364
	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		base.GetRootContainer().BringToFront();
		base.Parent.BringToFront();
		args.Use();
		Plane plane = new Plane(this.parent.transform.TransformDirection(Vector3.back), this.parent.transform.position);
		Ray ray = args.Ray;
		float num = 0f;
		plane.Raycast(args.Ray, out num);
		this.lastPosition = ray.origin + ray.direction * num;
		base.OnMouseDown(args);
	}

	// Token: 0x06001160 RID: 4448 RVA: 0x000511FC File Offset: 0x0004F3FC
	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		args.Use();
		if (args.Buttons.IsSet(dfMouseButtons.Left))
		{
			Ray ray = args.Ray;
			float num = 0f;
			Vector3 vector = base.GetCamera().transform.TransformDirection(Vector3.back);
			Plane plane = new Plane(vector, this.lastPosition);
			plane.Raycast(ray, out num);
			Vector3 vector2 = (ray.origin + ray.direction * num).Quantize(this.parent.PixelsToUnits());
			Vector3 vector3 = vector2 - this.lastPosition;
			Vector3 vector4 = (this.parent.transform.position + vector3).Quantize(this.parent.PixelsToUnits());
			this.parent.transform.position = vector4;
			this.lastPosition = vector2;
		}
		base.OnMouseMove(args);
	}

	// Token: 0x06001161 RID: 4449 RVA: 0x000512E0 File Offset: 0x0004F4E0
	protected internal override void OnMouseUp(dfMouseEventArgs args)
	{
		base.OnMouseUp(args);
		base.Parent.MakePixelPerfect();
	}

	// Token: 0x04000F92 RID: 3986
	private Vector3 lastPosition;
}
