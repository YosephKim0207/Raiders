using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000937 RID: 2359
	[ActionCategory(ActionCategory.Level)]
	[Tooltip("Makes the Game Object not be destroyed automatically when loading a new scene.")]
	public class DontDestroyOnLoad : FsmStateAction
	{
		// Token: 0x060033B0 RID: 13232 RVA: 0x0010DCEC File Offset: 0x0010BEEC
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x0010DCF8 File Offset: 0x0010BEF8
		public override void OnEnter()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.Owner.transform.root.gameObject);
			base.Finish();
		}

		// Token: 0x040024CF RID: 9423
		[Tooltip("GameObject to mark as DontDestroyOnLoad.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;
	}
}
