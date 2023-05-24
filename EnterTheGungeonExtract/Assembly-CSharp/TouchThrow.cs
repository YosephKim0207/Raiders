using System;
using UnityEngine;

// Token: 0x02000492 RID: 1170
[AddComponentMenu("Daikon Forge/Examples/Touch/Touch Throw")]
public class TouchThrow : MonoBehaviour
{
	// Token: 0x06001AEA RID: 6890 RVA: 0x0007DC5C File Offset: 0x0007BE5C
	public void Start()
	{
		this.control = base.GetComponent<dfControl>();
		this.manager = this.control.GetManager();
	}

	// Token: 0x06001AEB RID: 6891 RVA: 0x0007DC7C File Offset: 0x0007BE7C
	public void Update()
	{
		Vector2 screenSize = this.control.GetManager().GetScreenSize();
		Vector2 vector = this.control.RelativePosition;
		Vector2 vector2 = vector;
		if (this.animating)
		{
			if (vector.x + this.momentum.x < 0f || vector.x + this.momentum.x + this.control.Width > screenSize.x)
			{
				this.momentum.x = this.momentum.x * -1f;
			}
			if (vector.y + this.momentum.y < 0f || vector.y + this.momentum.y + this.control.Height > screenSize.y)
			{
				this.momentum.y = this.momentum.y * -1f;
			}
			vector2 += this.momentum;
			this.momentum *= 1f - Time.fixedDeltaTime;
		}
		vector2 = Vector2.Max(Vector2.zero, vector2);
		vector2 = Vector2.Min(screenSize - this.control.Size, vector2);
		if (Vector2.Distance(vector2, vector) > 1E-45f)
		{
			this.control.RelativePosition = vector2;
		}
	}

	// Token: 0x06001AEC RID: 6892 RVA: 0x0007DDE8 File Offset: 0x0007BFE8
	public void OnMultiTouch(dfControl control, dfTouchEventArgs touchData)
	{
		this.momentum = Vector2.zero;
		control.Color = Color.yellow;
		dfTouchInfo dfTouchInfo = touchData.Touches[0];
		dfTouchInfo dfTouchInfo2 = touchData.Touches[1];
		Vector2 vector = (dfTouchInfo.deltaPosition * (BraveTime.DeltaTime / dfTouchInfo.deltaTime)).Scale(1f, -1f);
		Vector2 vector2 = (dfTouchInfo2.deltaPosition * (BraveTime.DeltaTime / dfTouchInfo2.deltaTime)).Scale(1f, -1f);
		Vector2 vector3 = this.screenToGUI(dfTouchInfo.position);
		Vector2 vector4 = this.screenToGUI(dfTouchInfo2.position);
		Vector2 vector5 = vector3 - vector4;
		Vector2 vector6 = vector3 - vector - (vector4 - vector2);
		float num = vector5.magnitude - vector6.magnitude;
		if (Mathf.Abs(num) > 1E-45f)
		{
			Vector3 vector7 = Vector3.Min(vector3, vector4);
			Vector3 vector8 = vector7 - control.RelativePosition;
			control.Size += Vector2.one * num;
			control.RelativePosition = vector7 + vector8;
		}
	}

	// Token: 0x06001AED RID: 6893 RVA: 0x0007DF30 File Offset: 0x0007C130
	private Vector2 screenToGUI(Vector2 position)
	{
		position.y = this.manager.GetScreenSize().y - position.y;
		return position;
	}

	// Token: 0x06001AEE RID: 6894 RVA: 0x0007DF60 File Offset: 0x0007C160
	public void OnMouseMove(dfControl control, dfMouseEventArgs args)
	{
		if (this.animating || !this.dragging)
		{
			return;
		}
		this.momentum = (this.momentum + args.MoveDelta.Scale(1f, -1f)) * 0.5f;
		args.Use();
		if (args.Buttons.IsSet(dfMouseButtons.Left))
		{
			Ray ray = args.Ray;
			float num = 0f;
			Vector3 vector = Camera.main.transform.TransformDirection(Vector3.back);
			Plane plane = new Plane(vector, this.lastPosition);
			plane.Raycast(ray, out num);
			Vector3 vector2 = (ray.origin + ray.direction * num).Quantize(control.PixelsToUnits());
			Vector3 vector3 = vector2 - this.lastPosition;
			Vector3 vector4 = (control.transform.position + vector3).Quantize(control.PixelsToUnits());
			control.transform.position = vector4;
			this.lastPosition = vector2;
		}
	}

	// Token: 0x06001AEF RID: 6895 RVA: 0x0007E070 File Offset: 0x0007C270
	public void OnMouseEnter(dfControl control, dfMouseEventArgs args)
	{
		control.Color = Color.white;
	}

	// Token: 0x06001AF0 RID: 6896 RVA: 0x0007E084 File Offset: 0x0007C284
	public void OnMouseDown(dfControl control, dfMouseEventArgs args)
	{
		control.BringToFront();
		this.animating = false;
		this.momentum = Vector2.zero;
		this.dragging = true;
		args.Use();
		Plane plane = new Plane(control.transform.TransformDirection(Vector3.back), control.transform.position);
		Ray ray = args.Ray;
		float num = 0f;
		plane.Raycast(args.Ray, out num);
		this.lastPosition = ray.origin + ray.direction * num;
	}

	// Token: 0x06001AF1 RID: 6897 RVA: 0x0007E114 File Offset: 0x0007C314
	public void OnMouseUp()
	{
		this.animating = true;
		this.dragging = false;
		this.control.Color = Color.white;
	}

	// Token: 0x04001531 RID: 5425
	private dfControl control;

	// Token: 0x04001532 RID: 5426
	private dfGUIManager manager;

	// Token: 0x04001533 RID: 5427
	private Vector2 momentum;

	// Token: 0x04001534 RID: 5428
	private Vector3 lastPosition;

	// Token: 0x04001535 RID: 5429
	private bool animating;

	// Token: 0x04001536 RID: 5430
	private bool dragging;
}
