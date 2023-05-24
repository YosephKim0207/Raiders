using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A83 RID: 2691
	[Tooltip("Plays a Movie Texture. Use the Movie Texture in a Material, or in the GUI.")]
	[ActionCategory(ActionCategory.Movie)]
	public class PlayMovieTexture : FsmStateAction
	{
		// Token: 0x0600391F RID: 14623 RVA: 0x00124D64 File Offset: 0x00122F64
		public override void Reset()
		{
			this.movieTexture = null;
			this.loop = false;
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x00124D7C File Offset: 0x00122F7C
		public override void OnEnter()
		{
			MovieTexture movieTexture = this.movieTexture.Value as MovieTexture;
			if (movieTexture != null)
			{
				movieTexture.loop = this.loop.Value;
				movieTexture.Play();
			}
			base.Finish();
		}

		// Token: 0x04002B6F RID: 11119
		[RequiredField]
		[ObjectType(typeof(MovieTexture))]
		public FsmObject movieTexture;

		// Token: 0x04002B70 RID: 11120
		public FsmBool loop;
	}
}
