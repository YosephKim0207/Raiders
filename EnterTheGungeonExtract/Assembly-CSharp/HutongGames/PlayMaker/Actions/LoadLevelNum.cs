using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A17 RID: 2583
	[Tooltip("Loads a Level by Index number. Before you can load a level, you have to add it to the list of levels defined in File->Build Settings...")]
	[ActionCategory(ActionCategory.Level)]
	public class LoadLevelNum : FsmStateAction
	{
		// Token: 0x06003755 RID: 14165 RVA: 0x0011D2A0 File Offset: 0x0011B4A0
		public override void Reset()
		{
			this.levelIndex = null;
			this.additive = false;
			this.loadedEvent = null;
			this.dontDestroyOnLoad = false;
		}

		// Token: 0x06003756 RID: 14166 RVA: 0x0011D2C4 File Offset: 0x0011B4C4
		public override void OnEnter()
		{
			if (!Application.CanStreamedLevelBeLoaded(this.levelIndex.Value))
			{
				base.Fsm.Event(this.failedEvent);
				base.Finish();
				return;
			}
			if (this.dontDestroyOnLoad.Value)
			{
				Transform root = base.Owner.transform.root;
				UnityEngine.Object.DontDestroyOnLoad(root.gameObject);
			}
			if (this.additive)
			{
				SceneManager.LoadScene(this.levelIndex.Value, LoadSceneMode.Additive);
			}
			else
			{
				SceneManager.LoadScene(this.levelIndex.Value, LoadSceneMode.Single);
			}
			base.Fsm.Event(this.loadedEvent);
			base.Finish();
		}

		// Token: 0x0400292B RID: 10539
		[Tooltip("The level index in File->Build Settings")]
		[RequiredField]
		public FsmInt levelIndex;

		// Token: 0x0400292C RID: 10540
		[Tooltip("Load the level additively, keeping the current scene.")]
		public bool additive;

		// Token: 0x0400292D RID: 10541
		[Tooltip("Event to send after the level is loaded.")]
		public FsmEvent loadedEvent;

		// Token: 0x0400292E RID: 10542
		[Tooltip("Keep this GameObject in the new level. NOTE: The GameObject and components is disabled then enabled on load; uncheck Reset On Disable to keep the active state.")]
		public FsmBool dontDestroyOnLoad;

		// Token: 0x0400292F RID: 10543
		[Tooltip("Event to send if the level cannot be loaded.")]
		public FsmEvent failedEvent;
	}
}
