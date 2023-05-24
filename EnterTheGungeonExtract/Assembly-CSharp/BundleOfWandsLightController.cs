using System;
using UnityEngine;

// Token: 0x0200162B RID: 5675
public class BundleOfWandsLightController : MonoBehaviour
{
	// Token: 0x06008480 RID: 33920 RVA: 0x00368BF4 File Offset: 0x00366DF4
	private void Start()
	{
		this.m_light = base.GetComponent<Light>();
	}

	// Token: 0x06008481 RID: 33921 RVA: 0x00368C04 File Offset: 0x00366E04
	private Vector3 shift_col(Vector3 RGB, Vector3 shift)
	{
		Vector3 vector = new Vector3(RGB.x, RGB.y, RGB.z);
		float num = shift.z * shift.y * Mathf.Cos(shift.x * 3.1415927f / 180f);
		float num2 = shift.z * shift.y * Mathf.Sin(shift.x * 3.1415927f / 180f);
		vector.x = (0.299f * shift.z + 0.701f * num + 0.168f * num2) * RGB.x + (0.587f * shift.z - 0.587f * num + 0.33f * num2) * RGB.y + (0.114f * shift.z - 0.114f * num - 0.497f * num2) * RGB.z;
		vector.y = (0.299f * shift.z - 0.299f * num - 0.328f * num2) * RGB.x + (0.587f * shift.z + 0.413f * num + 0.035f * num2) * RGB.y + (0.114f * shift.z - 0.114f * num + 0.292f * num2) * RGB.z;
		vector.z = (0.299f * shift.z - 0.3f * num + 1.25f * num2) * RGB.x + (0.587f * shift.z - 0.588f * num - 1.05f * num2) * RGB.y + (0.114f * shift.z + 0.886f * num - 0.203f * num2) * RGB.z;
		return vector;
	}

	// Token: 0x06008482 RID: 33922 RVA: 0x00368DE8 File Offset: 0x00366FE8
	private void Update()
	{
		Vector3 vector = new Vector3(this.baseColor.r, this.baseColor.g, this.baseColor.b);
		Vector3 vector2 = this.shift_col(vector, new Vector3(1.5f * Time.time * 360f, 1f, 1.5f));
		this.m_light.color = new Color(vector2.x, vector2.y, vector2.z);
	}

	// Token: 0x0400881F RID: 34847
	public Color baseColor;

	// Token: 0x04008820 RID: 34848
	private Light m_light;
}
