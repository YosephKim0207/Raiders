using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B26 RID: 2854
	[ActionCategory(ActionCategory.Movie)]
	[Tooltip("Stops playing the Movie Texture, and rewinds it to the beginning.")]
	public class StopMovieTexture : FsmStateAction
	{
		// Token: 0x06003C1B RID: 15387 RVA: 0x0012EB04 File Offset: 0x0012CD04
		public override void Reset()
		{
			this.movieTexture = null;
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x0012EB10 File Offset: 0x0012CD10
		public override void OnEnter()
		{
			MovieTexture movieTexture = this.movieTexture.Value as MovieTexture;
			if (movieTexture != null)
			{
				movieTexture.Stop();
			}
			base.Finish();
		}

		// Token: 0x04002E47 RID: 11847
		[ObjectType(typeof(MovieTexture))]
		[RequiredField]
		public FsmObject movieTexture;
	}
}
