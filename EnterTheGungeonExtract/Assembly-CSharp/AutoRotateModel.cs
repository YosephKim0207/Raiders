using System;
using UnityEngine;

// Token: 0x0200043D RID: 1085
[AddComponentMenu("Daikon Forge/Examples/Color Picker/Auto-rotate Model")]
public class AutoRotateModel : MonoBehaviour
{
	// Token: 0x060018DD RID: 6365 RVA: 0x0007537C File Offset: 0x0007357C
	private void Update()
	{
		base.transform.Rotate(Vector3.up * BraveTime.DeltaTime * 45f);
	}
}
