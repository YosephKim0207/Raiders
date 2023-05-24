using System;
using UnityEngine;

// Token: 0x020018E8 RID: 6376
[RequireComponent(typeof(BoxCollider))]
[AddComponentMenu("Wwise/AkEnvironmentPortal")]
[ExecuteInEditMode]
public class AkEnvironmentPortal : MonoBehaviour
{
	// Token: 0x06009D2D RID: 40237 RVA: 0x003EE6F4 File Offset: 0x003EC8F4
	public float GetAuxSendValueForPosition(Vector3 in_position, int index)
	{
		float num = Vector3.Dot(Vector3.Scale(base.GetComponent<BoxCollider>().size, base.transform.lossyScale), this.axis);
		Vector3 vector = Vector3.Normalize(base.transform.rotation * this.axis);
		float num2 = Vector3.Dot(in_position - (base.transform.position - num * 0.5f * vector), vector);
		if (index == 0)
		{
			return (num - num2) * (num - num2) / (num * num);
		}
		return num2 * num2 / (num * num);
	}

	// Token: 0x04009EA4 RID: 40612
	public const int MAX_ENVIRONMENTS_PER_PORTAL = 2;

	// Token: 0x04009EA5 RID: 40613
	public Vector3 axis = new Vector3(1f, 0f, 0f);

	// Token: 0x04009EA6 RID: 40614
	public AkEnvironment[] environments = new AkEnvironment[2];
}
