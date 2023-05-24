using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B71 RID: 2929
	[Tooltip("Gets data from a url and store it in variables. See Unity WWW docs for more details.")]
	[ActionCategory("Web Player")]
	public class WWWObject : FsmStateAction
	{
		// Token: 0x06003D4D RID: 15693 RVA: 0x00132C50 File Offset: 0x00130E50
		public override void Reset()
		{
			this.url = null;
			this.storeText = null;
			this.storeTexture = null;
			this.errorString = null;
			this.progress = null;
			this.isDone = null;
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x00132C7C File Offset: 0x00130E7C
		public override void OnEnter()
		{
			if (string.IsNullOrEmpty(this.url.Value))
			{
				base.Finish();
				return;
			}
			this.wwwObject = new WWW(this.url.Value);
		}

		// Token: 0x06003D4F RID: 15695 RVA: 0x00132CB0 File Offset: 0x00130EB0
		public override void OnUpdate()
		{
			if (this.wwwObject == null)
			{
				this.errorString.Value = "WWW Object is Null!";
				base.Finish();
				return;
			}
			this.errorString.Value = this.wwwObject.error;
			if (!string.IsNullOrEmpty(this.wwwObject.error))
			{
				base.Finish();
				base.Fsm.Event(this.isError);
				return;
			}
			this.progress.Value = this.wwwObject.progress;
			if (this.progress.Value.Equals(1f))
			{
				this.storeText.Value = this.wwwObject.text;
				this.storeTexture.Value = this.wwwObject.texture;
				this.storeMovieTexture.Value = this.wwwObject.GetMovieTexture();
				this.errorString.Value = this.wwwObject.error;
				base.Fsm.Event((!string.IsNullOrEmpty(this.errorString.Value)) ? this.isError : this.isDone);
				base.Finish();
			}
		}

		// Token: 0x04002F9F RID: 12191
		[Tooltip("Url to download data from.")]
		[RequiredField]
		public FsmString url;

		// Token: 0x04002FA0 RID: 12192
		[ActionSection("Results")]
		[UIHint(UIHint.Variable)]
		[Tooltip("Gets text from the url.")]
		public FsmString storeText;

		// Token: 0x04002FA1 RID: 12193
		[Tooltip("Gets a Texture from the url.")]
		[UIHint(UIHint.Variable)]
		public FsmTexture storeTexture;

		// Token: 0x04002FA2 RID: 12194
		[Tooltip("Gets a Texture from the url.")]
		[ObjectType(typeof(MovieTexture))]
		[UIHint(UIHint.Variable)]
		public FsmObject storeMovieTexture;

		// Token: 0x04002FA3 RID: 12195
		[Tooltip("Error message if there was an error during the download.")]
		[UIHint(UIHint.Variable)]
		public FsmString errorString;

		// Token: 0x04002FA4 RID: 12196
		[Tooltip("How far the download progressed (0-1).")]
		[UIHint(UIHint.Variable)]
		public FsmFloat progress;

		// Token: 0x04002FA5 RID: 12197
		[ActionSection("Events")]
		[Tooltip("Event to send when the data has finished loading (progress = 1).")]
		public FsmEvent isDone;

		// Token: 0x04002FA6 RID: 12198
		[Tooltip("Event to send if there was an error.")]
		public FsmEvent isError;

		// Token: 0x04002FA7 RID: 12199
		private WWW wwwObject;
	}
}
