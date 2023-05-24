using System;
using UnityEngine;

// Token: 0x02000486 RID: 1158
[AddComponentMenu("Camera-Control/Mobile Mouse Look")]
public class dfMobileMouseLook : MonoBehaviour
{
	// Token: 0x06001A98 RID: 6808 RVA: 0x0007C3E0 File Offset: 0x0007A5E0
	private void Update()
	{
		Vector2 joystickPosition = dfTouchJoystick.GetJoystickPosition(this.joystickName);
		if (this.axes == dfMobileMouseLook.RotationAxes.MouseXAndY)
		{
			float num = base.transform.localEulerAngles.y + joystickPosition.x * this.sensitivityX;
			this.rotationY += joystickPosition.y * this.sensitivityY;
			this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
			base.transform.localEulerAngles = new Vector3(-this.rotationY, num, 0f);
		}
		else if (this.axes == dfMobileMouseLook.RotationAxes.MouseX)
		{
			base.transform.Rotate(0f, joystickPosition.x * this.sensitivityX, 0f);
		}
		else
		{
			this.rotationY += joystickPosition.y * this.sensitivityY;
			this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
			base.transform.localEulerAngles = new Vector3(-this.rotationY, base.transform.localEulerAngles.y, 0f);
		}
	}

	// Token: 0x06001A99 RID: 6809 RVA: 0x0007C51C File Offset: 0x0007A71C
	private void Start()
	{
		if (base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().freezeRotation = true;
		}
	}

	// Token: 0x040014E9 RID: 5353
	public string joystickName = "RightJoystick";

	// Token: 0x040014EA RID: 5354
	public dfMobileMouseLook.RotationAxes axes;

	// Token: 0x040014EB RID: 5355
	public float sensitivityX = 15f;

	// Token: 0x040014EC RID: 5356
	public float sensitivityY = 15f;

	// Token: 0x040014ED RID: 5357
	public float minimumX = -360f;

	// Token: 0x040014EE RID: 5358
	public float maximumX = 360f;

	// Token: 0x040014EF RID: 5359
	public float minimumY = -60f;

	// Token: 0x040014F0 RID: 5360
	public float maximumY = 60f;

	// Token: 0x040014F1 RID: 5361
	private float rotationY;

	// Token: 0x02000487 RID: 1159
	public enum RotationAxes
	{
		// Token: 0x040014F3 RID: 5363
		MouseXAndY,
		// Token: 0x040014F4 RID: 5364
		MouseX,
		// Token: 0x040014F5 RID: 5365
		MouseY
	}
}
