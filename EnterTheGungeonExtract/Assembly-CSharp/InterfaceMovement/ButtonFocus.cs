using System;
using UnityEngine;

namespace InterfaceMovement
{
	// Token: 0x0200067B RID: 1659
	public class ButtonFocus : MonoBehaviour
	{
		// Token: 0x060025C2 RID: 9666 RVA: 0x000A1BA4 File Offset: 0x0009FDA4
		private void Update()
		{
			Button focusedButton = base.transform.parent.GetComponent<ButtonManager>().focusedButton;
			base.transform.position = Vector3.MoveTowards(base.transform.position, focusedButton.transform.position, Time.deltaTime * 10f);
		}
	}
}
