using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000488 RID: 1160
[Serializable]
public class dfTouchJoystick : MonoBehaviour
{
	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x06001A9B RID: 6811 RVA: 0x0007C570 File Offset: 0x0007A770
	public Vector2 Position
	{
		get
		{
			return this.joystickPos;
		}
	}

	// Token: 0x06001A9C RID: 6812 RVA: 0x0007C578 File Offset: 0x0007A778
	public static Vector2 GetJoystickPosition(string joystickID)
	{
		if (!dfTouchJoystick.joysticks.ContainsKey(joystickID))
		{
			throw new Exception("Joystick not registered: " + joystickID);
		}
		return dfTouchJoystick.joysticks[joystickID].Position;
	}

	// Token: 0x06001A9D RID: 6813 RVA: 0x0007C5AC File Offset: 0x0007A7AC
	public static void ResetJoystickPosition(string joystickID)
	{
		if (!dfTouchJoystick.joysticks.ContainsKey(joystickID))
		{
			throw new Exception("Joystick not registered: " + joystickID);
		}
		dfTouchJoystick dfTouchJoystick = dfTouchJoystick.joysticks[joystickID];
		if (dfTouchJoystick.JoystickType == dfTouchJoystick.TouchJoystickType.Trackpad)
		{
			dfTouchJoystick.joystickPos = Vector2.zero;
		}
		else
		{
			dfTouchJoystick.recenter();
		}
	}

	// Token: 0x06001A9E RID: 6814 RVA: 0x0007C608 File Offset: 0x0007A808
	public void Start()
	{
		this.control = base.GetComponent<dfControl>();
		if ((this.JoystickType != dfTouchJoystick.TouchJoystickType.Trackpad || !(this.control != null)) && (!(this.control != null) || !(this.ThumbControl != null) || !(this.AreaControl != null)))
		{
			Debug.LogError("Invalid virtual joystick configuration", this);
			base.enabled = false;
			return;
		}
		dfTouchJoystick.joysticks.Add(this.JoystickID, this);
		if (this.ThumbControl != null && this.HideThumb)
		{
			this.ThumbControl.Hide();
			if (this.DynamicThumb)
			{
				this.AreaControl.Hide();
			}
		}
		this.recenter();
	}

	// Token: 0x06001A9F RID: 6815 RVA: 0x0007C6E0 File Offset: 0x0007A8E0
	public void OnDestroy()
	{
		dfTouchJoystick.joysticks.Remove(this.JoystickID);
	}

	// Token: 0x06001AA0 RID: 6816 RVA: 0x0007C6F4 File Offset: 0x0007A8F4
	public void OnMouseDown(dfControl control, dfMouseEventArgs args)
	{
		if (this.JoystickType == dfTouchJoystick.TouchJoystickType.Trackpad)
		{
			return;
		}
		Vector2 vector;
		control.GetHitPosition(args.Ray, out vector, true);
		if (this.HideThumb)
		{
			this.ThumbControl.Show();
			this.AreaControl.Show();
		}
		if (this.DynamicThumb)
		{
			this.AreaControl.RelativePosition = vector - this.AreaControl.Size * 0.5f;
			this.centerThumbInArea();
		}
		else
		{
			this.recenter();
		}
		this.processTouch(args);
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x0007C790 File Offset: 0x0007A990
	public void OnMouseHover()
	{
		if (this.JoystickType == dfTouchJoystick.TouchJoystickType.Trackpad)
		{
			this.joystickPos = Vector2.zero;
		}
	}

	// Token: 0x06001AA2 RID: 6818 RVA: 0x0007C7AC File Offset: 0x0007A9AC
	public void OnMouseMove(dfControl control, dfMouseEventArgs args)
	{
		if (this.JoystickType == dfTouchJoystick.TouchJoystickType.Trackpad && args.Buttons.IsSet(dfMouseButtons.Left))
		{
			this.joystickPos = args.MoveDelta * 0.25f;
			return;
		}
		if (args.Buttons.IsSet(dfMouseButtons.Left))
		{
			this.processTouch(args);
		}
	}

	// Token: 0x06001AA3 RID: 6819 RVA: 0x0007C808 File Offset: 0x0007AA08
	public void OnMouseUp(dfControl control, dfMouseEventArgs args)
	{
		if (this.JoystickType == dfTouchJoystick.TouchJoystickType.Trackpad)
		{
			this.joystickPos = Vector2.zero;
			return;
		}
		this.recenter();
		if (this.HideThumb)
		{
			this.ThumbControl.Hide();
			if (this.DynamicThumb)
			{
				this.AreaControl.Hide();
			}
		}
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x0007C860 File Offset: 0x0007AA60
	private void recenter()
	{
		if (this.JoystickType == dfTouchJoystick.TouchJoystickType.Trackpad)
		{
			return;
		}
		this.AreaControl.RelativePosition = (this.control.Size - this.AreaControl.Size) * 0.5f;
		Vector3 vector = this.AreaControl.RelativePosition + this.AreaControl.Size * 0.5f;
		Vector3 vector2 = this.ThumbControl.Size * 0.5f;
		this.ThumbControl.RelativePosition = vector - vector2;
		this.joystickPos = Vector2.zero;
	}

	// Token: 0x06001AA5 RID: 6821 RVA: 0x0007C914 File Offset: 0x0007AB14
	private void centerThumbInArea()
	{
		this.ThumbControl.RelativePosition = this.AreaControl.RelativePosition + (this.AreaControl.Size - this.ThumbControl.Size) * 0.5f;
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x0007C968 File Offset: 0x0007AB68
	private void processTouch(dfMouseEventArgs evt)
	{
		Vector2 vector = this.raycast(evt.Ray);
		Vector3 vector2 = this.AreaControl.RelativePosition + this.AreaControl.Size * 0.5f;
		Vector3 vector3 = vector - vector2;
		if (vector3.magnitude > (float)this.Radius)
		{
			vector3 = vector3.normalized * (float)this.Radius;
		}
		Vector3 vector4 = this.ThumbControl.Size * 0.5f;
		this.ThumbControl.RelativePosition = vector2 - vector4 + vector3;
		vector3 /= (float)this.Radius;
		if (vector3.magnitude <= this.DeadzoneRadius)
		{
			this.joystickPos = Vector2.zero;
			return;
		}
		this.joystickPos = new Vector3(vector3.x, -vector3.y);
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x0007CA60 File Offset: 0x0007AC60
	private Vector2 raycast(Ray ray)
	{
		Vector3[] corners = this.control.GetCorners();
		Plane plane = new Plane(corners[0], corners[1], corners[3]);
		float num = 0f;
		plane.Raycast(ray, out num);
		Vector3 point = ray.GetPoint(num);
		Vector3 vector = (point - corners[0]).Scale(1f, -1f, 0f) / this.control.GetManager().PixelsToUnits();
		return vector;
	}

	// Token: 0x040014F6 RID: 5366
	private static Dictionary<string, dfTouchJoystick> joysticks = new Dictionary<string, dfTouchJoystick>();

	// Token: 0x040014F7 RID: 5367
	[SerializeField]
	public string JoystickID = "Joystick";

	// Token: 0x040014F8 RID: 5368
	[SerializeField]
	public dfTouchJoystick.TouchJoystickType JoystickType;

	// Token: 0x040014F9 RID: 5369
	[SerializeField]
	public int Radius = 80;

	// Token: 0x040014FA RID: 5370
	[SerializeField]
	public float DeadzoneRadius = 0.25f;

	// Token: 0x040014FB RID: 5371
	[SerializeField]
	public bool DynamicThumb;

	// Token: 0x040014FC RID: 5372
	[SerializeField]
	public bool HideThumb;

	// Token: 0x040014FD RID: 5373
	[SerializeField]
	public dfControl ThumbControl;

	// Token: 0x040014FE RID: 5374
	[SerializeField]
	public dfControl AreaControl;

	// Token: 0x040014FF RID: 5375
	private dfControl control;

	// Token: 0x04001500 RID: 5376
	private Vector2 joystickPos = Vector2.zero;

	// Token: 0x02000489 RID: 1161
	public enum TouchJoystickType
	{
		// Token: 0x04001502 RID: 5378
		Joystick,
		// Token: 0x04001503 RID: 5379
		Trackpad
	}
}
