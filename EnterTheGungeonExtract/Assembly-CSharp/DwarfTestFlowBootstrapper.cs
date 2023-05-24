using System;
using UnityEngine;

// Token: 0x02000F75 RID: 3957
public class DwarfTestFlowBootstrapper : MonoBehaviour
{
	// Token: 0x06005545 RID: 21829 RVA: 0x002064C8 File Offset: 0x002046C8
	private void Start()
	{
		foreach (GameManager gameManager in UnityEngine.Object.FindObjectsOfType<GameManager>())
		{
			UnityEngine.Object.Destroy(gameManager);
		}
		if (this.ConvertToCoopMode)
		{
			DwarfTestFlowBootstrapper.ShouldConvertToCoopMode = true;
		}
		UnityEngine.Random.InitState(new System.Random().Next(1, 1000));
	}

	// Token: 0x04004E2E RID: 20014
	public static bool IsBootstrapping;

	// Token: 0x04004E2F RID: 20015
	public static bool ShouldConvertToCoopMode;

	// Token: 0x04004E30 RID: 20016
	public bool ConvertToCoopMode;
}
