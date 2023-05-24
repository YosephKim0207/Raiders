using System;
using UnityEngine.SceneManagement;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AAA RID: 2730
	[Tooltip("Restarts current level.")]
	[ActionCategory(ActionCategory.Level)]
	public class RestartLevel : FsmStateAction
	{
		// Token: 0x060039E6 RID: 14822 RVA: 0x00127470 File Offset: 0x00125670
		public override void OnEnter()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
			base.Finish();
		}
	}
}
