using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000774 RID: 1908
	public class UnityGyroAxisSource : InputControlSource
	{
		// Token: 0x06002AAE RID: 10926 RVA: 0x000C1DD0 File Offset: 0x000BFFD0
		public UnityGyroAxisSource()
		{
			UnityGyroAxisSource.Calibrate();
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x000C1DE0 File Offset: 0x000BFFE0
		public UnityGyroAxisSource(UnityGyroAxisSource.GyroAxis axis)
		{
			this.Axis = (int)axis;
			UnityGyroAxisSource.Calibrate();
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x000C1DF4 File Offset: 0x000BFFF4
		public float GetValue(InputDevice inputDevice)
		{
			return UnityGyroAxisSource.GetAxis()[this.Axis];
		}

		// Token: 0x06002AB1 RID: 10929 RVA: 0x000C1E14 File Offset: 0x000C0014
		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x000C1E24 File Offset: 0x000C0024
		private static Quaternion GetAttitude()
		{
			return Quaternion.Inverse(UnityGyroAxisSource.zeroAttitude) * Input.gyro.attitude;
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x000C1E40 File Offset: 0x000C0040
		private static Vector3 GetAxis()
		{
			Vector3 vector = UnityGyroAxisSource.GetAttitude() * Vector3.forward;
			float num = UnityGyroAxisSource.ApplyDeadZone(Mathf.Clamp(vector.x, -1f, 1f));
			float num2 = UnityGyroAxisSource.ApplyDeadZone(Mathf.Clamp(vector.y, -1f, 1f));
			return new Vector3(num, num2);
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x000C1E9C File Offset: 0x000C009C
		private static float ApplyDeadZone(float value)
		{
			return Mathf.InverseLerp(0.05f, 1f, Utility.Abs(value)) * Mathf.Sign(value);
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x000C1EBC File Offset: 0x000C00BC
		public static void Calibrate()
		{
			UnityGyroAxisSource.zeroAttitude = Input.gyro.attitude;
		}

		// Token: 0x04001D83 RID: 7555
		private static Quaternion zeroAttitude;

		// Token: 0x04001D84 RID: 7556
		public int Axis;

		// Token: 0x02000775 RID: 1909
		public enum GyroAxis
		{
			// Token: 0x04001D86 RID: 7558
			X,
			// Token: 0x04001D87 RID: 7559
			Y
		}
	}
}
