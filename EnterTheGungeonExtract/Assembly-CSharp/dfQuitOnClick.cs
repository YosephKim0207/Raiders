using System;
using UnityEngine;

// Token: 0x02000469 RID: 1129
[AddComponentMenu("Daikon Forge/Examples/General/Quit On Click")]
public class dfQuitOnClick : MonoBehaviour
{
	// Token: 0x06001A17 RID: 6679 RVA: 0x00079CCC File Offset: 0x00077ECC
	private void OnClick()
	{
		Application.Quit();
	}
}
