using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A21 RID: 2593
	[Tooltip("Sets the Game Object as the Audio Source associated with the Movie Texture. The Game Object must have an AudioSource Component.")]
	[ActionCategory(ActionCategory.Movie)]
	public class MovieTextureAudioSettings : FsmStateAction
	{
		// Token: 0x06003787 RID: 14215 RVA: 0x0011E4B8 File Offset: 0x0011C6B8
		public override void Reset()
		{
			this.movieTexture = null;
			this.gameObject = null;
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x0011E4C8 File Offset: 0x0011C6C8
		public override void OnEnter()
		{
			MovieTexture movieTexture = this.movieTexture.Value as MovieTexture;
			if (movieTexture != null && this.gameObject.Value != null)
			{
				AudioSource component = this.gameObject.Value.GetComponent<AudioSource>();
				if (component != null)
				{
					component.clip = movieTexture.audioClip;
				}
			}
			base.Finish();
		}

		// Token: 0x0400297D RID: 10621
		[RequiredField]
		[ObjectType(typeof(MovieTexture))]
		public FsmObject movieTexture;

		// Token: 0x0400297E RID: 10622
		[RequiredField]
		[CheckForComponent(typeof(AudioSource))]
		public FsmGameObject gameObject;
	}
}
