using System;
using UnityEngine;

// Token: 0x02000485 RID: 1157
[RequireComponent(typeof(dfCharacterMotorCS))]
public class dfMobileFPSInputController : MonoBehaviour
{
	// Token: 0x06001A95 RID: 6805 RVA: 0x0007C2E8 File Offset: 0x0007A4E8
	private void Awake()
	{
		this.motor = base.GetComponent<dfCharacterMotorCS>();
	}

	// Token: 0x06001A96 RID: 6806 RVA: 0x0007C2F8 File Offset: 0x0007A4F8
	private void Update()
	{
		Vector2 joystickPosition = dfTouchJoystick.GetJoystickPosition(this.joystickID);
		Vector3 vector = new Vector3(joystickPosition.x, 0f, joystickPosition.y);
		if (vector != Vector3.zero)
		{
			float num = vector.magnitude;
			vector /= num;
			num = Mathf.Min(1f, num);
			num *= num;
			vector *= num;
		}
		this.motor.inputMoveDirection = base.transform.rotation * vector;
	}

	// Token: 0x040014E7 RID: 5351
	public string joystickID = "LeftJoystick";

	// Token: 0x040014E8 RID: 5352
	private dfCharacterMotorCS motor;
}
