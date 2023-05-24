using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A4F RID: 2639
	[Tooltip("Pauses a Movie Texture.")]
	[ActionCategory(ActionCategory.Movie)]
	public class PauseMovieTexture : FsmStateAction
	{
		// Token: 0x0600382E RID: 14382 RVA: 0x00120380 File Offset: 0x0011E580
		public override void Reset()
		{
			this.movieTexture = null;
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x0012038C File Offset: 0x0011E58C
		public override void OnEnter()
		{
			MovieTexture movieTexture = this.movieTexture.Value as MovieTexture;
			if (movieTexture != null)
			{
				movieTexture.Pause();
			}
			base.Finish();
		}

		// Token: 0x04002A2C RID: 10796
		[ObjectType(typeof(MovieTexture))]
		[RequiredField]
		public FsmObject movieTexture;
	}
}
