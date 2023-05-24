using System;
using UnityEngine;

// Token: 0x0200151E RID: 5406
public class Move : MonoBehaviour
{
	// Token: 0x06007B58 RID: 31576 RVA: 0x00316620 File Offset: 0x00314820
	private void Update()
	{
		Vector3 vector = Vector3.zero;
		float axis = Input.GetAxis("Horizontal");
		vector = Vector3.right * axis;
		base.transform.Translate(vector * BraveTime.DeltaTime * 5f, Space.World);
		float axis2 = Input.GetAxis("Vertical");
		vector = Vector3.forward * axis2;
		base.transform.Translate(vector * BraveTime.DeltaTime * 5f, Space.World);
	}
}
