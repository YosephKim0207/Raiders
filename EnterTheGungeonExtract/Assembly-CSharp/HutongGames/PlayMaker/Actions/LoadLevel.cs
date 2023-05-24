using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A16 RID: 2582
	[Tooltip("Loads a Level by Name. NOTE: Before you can load a level, you have to add it to the list of levels defined in File->Build Settings...")]
	[ActionCategory(ActionCategory.Level)]
	public class LoadLevel : FsmStateAction
	{
		// Token: 0x06003751 RID: 14161 RVA: 0x0011D0D0 File Offset: 0x0011B2D0
		public override void Reset()
		{
			this.levelName = string.Empty;
			this.additive = false;
			this.async = false;
			this.loadedEvent = null;
			this.dontDestroyOnLoad = false;
		}

		// Token: 0x06003752 RID: 14162 RVA: 0x0011D104 File Offset: 0x0011B304
		public override void OnEnter()
		{
			if (!Application.CanStreamedLevelBeLoaded(this.levelName.Value))
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
				if (this.async)
				{
					this.asyncOperation = SceneManager.LoadSceneAsync(this.levelName.Value, LoadSceneMode.Additive);
					Debug.Log("LoadLevelAdditiveAsyc: " + this.levelName.Value);
					return;
				}
				SceneManager.LoadScene(this.levelName.Value, LoadSceneMode.Additive);
				Debug.Log("LoadLevelAdditive: " + this.levelName.Value);
			}
			else
			{
				if (this.async)
				{
					this.asyncOperation = SceneManager.LoadSceneAsync(this.levelName.Value, LoadSceneMode.Single);
					Debug.Log("LoadLevelAsync: " + this.levelName.Value);
					return;
				}
				SceneManager.LoadScene(this.levelName.Value, LoadSceneMode.Single);
				Debug.Log("LoadLevel: " + this.levelName.Value);
			}
			base.Log("LOAD COMPLETE");
			base.Fsm.Event(this.loadedEvent);
			base.Finish();
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x0011D26C File Offset: 0x0011B46C
		public override void OnUpdate()
		{
			if (this.asyncOperation.isDone)
			{
				base.Fsm.Event(this.loadedEvent);
				base.Finish();
			}
		}

		// Token: 0x04002924 RID: 10532
		[Tooltip("The name of the level to load. NOTE: Must be in the list of levels defined in File->Build Settings... ")]
		[RequiredField]
		public FsmString levelName;

		// Token: 0x04002925 RID: 10533
		[Tooltip("Load the level additively, keeping the current scene.")]
		public bool additive;

		// Token: 0x04002926 RID: 10534
		[Tooltip("Load the level asynchronously in the background.")]
		public bool async;

		// Token: 0x04002927 RID: 10535
		[Tooltip("Event to send when the level has loaded. NOTE: This only makes sense if the FSM is still in the scene!")]
		public FsmEvent loadedEvent;

		// Token: 0x04002928 RID: 10536
		[Tooltip("Keep this GameObject in the new level. NOTE: The GameObject and components is disabled then enabled on load; uncheck Reset On Disable to keep the active state.")]
		public FsmBool dontDestroyOnLoad;

		// Token: 0x04002929 RID: 10537
		[Tooltip("Event to send if the level cannot be loaded.")]
		public FsmEvent failedEvent;

		// Token: 0x0400292A RID: 10538
		private AsyncOperation asyncOperation;
	}
}
