using System;
using UnityEngine;

// Token: 0x02000472 RID: 1138
[AddComponentMenu("Daikon Forge/Examples/General/Quit On Click")]
public class QuitOnClick : MonoBehaviour
{
	// Token: 0x06001A33 RID: 6707 RVA: 0x0007A49C File Offset: 0x0007869C
	private void OnClick()
	{
		Application.Quit();
	}
}
