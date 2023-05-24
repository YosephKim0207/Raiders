using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000408 RID: 1032
[dfTooltip("Downloads an image from a web URL and displays it on-screen like a sprite")]
[ExecuteInEditMode]
[dfCategory("Basic Controls")]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_web_sprite.html")]
[AddComponentMenu("Daikon Forge/User Interface/Sprite/Web")]
[Serializable]
public class dfWebSprite : dfTextureSprite
{
	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x06001770 RID: 6000 RVA: 0x0006FE38 File Offset: 0x0006E038
	// (set) Token: 0x06001771 RID: 6001 RVA: 0x0006FE40 File Offset: 0x0006E040
	public string URL
	{
		get
		{
			return this.url;
		}
		set
		{
			if (value != this.url)
			{
				this.url = value;
				if (Application.isPlaying && this.AutoDownload)
				{
					this.LoadImage();
				}
			}
		}
	}

	// Token: 0x17000511 RID: 1297
	// (get) Token: 0x06001772 RID: 6002 RVA: 0x0006FE78 File Offset: 0x0006E078
	// (set) Token: 0x06001773 RID: 6003 RVA: 0x0006FE80 File Offset: 0x0006E080
	public Texture2D LoadingImage
	{
		get
		{
			return this.loadingImage;
		}
		set
		{
			this.loadingImage = value;
		}
	}

	// Token: 0x17000512 RID: 1298
	// (get) Token: 0x06001774 RID: 6004 RVA: 0x0006FE8C File Offset: 0x0006E08C
	// (set) Token: 0x06001775 RID: 6005 RVA: 0x0006FE94 File Offset: 0x0006E094
	public Texture2D ErrorImage
	{
		get
		{
			return this.errorImage;
		}
		set
		{
			this.errorImage = value;
		}
	}

	// Token: 0x17000513 RID: 1299
	// (get) Token: 0x06001776 RID: 6006 RVA: 0x0006FEA0 File Offset: 0x0006E0A0
	// (set) Token: 0x06001777 RID: 6007 RVA: 0x0006FEA8 File Offset: 0x0006E0A8
	public bool AutoDownload
	{
		get
		{
			return this.autoDownload;
		}
		set
		{
			this.autoDownload = value;
		}
	}

	// Token: 0x06001778 RID: 6008 RVA: 0x0006FEB4 File Offset: 0x0006E0B4
	public override void OnEnable()
	{
		base.OnEnable();
		if (base.Texture == null)
		{
			base.Texture = this.LoadingImage;
		}
		if (base.Texture == null && this.AutoDownload && Application.isPlaying)
		{
			this.LoadImage();
		}
	}

	// Token: 0x06001779 RID: 6009 RVA: 0x0006FF10 File Offset: 0x0006E110
	public void LoadImage()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.downloadTexture());
	}

	// Token: 0x0600177A RID: 6010 RVA: 0x0006FF28 File Offset: 0x0006E128
	private IEnumerator downloadTexture()
	{
		base.Texture = this.loadingImage;
		if (string.IsNullOrEmpty(this.url))
		{
			yield break;
		}
		using (WWW request = new WWW(this.url))
		{
			yield return request;
			if (!string.IsNullOrEmpty(request.error))
			{
				base.Texture = this.errorImage ?? this.loadingImage;
				if (this.DownloadError != null)
				{
					this.DownloadError(this, request.error);
				}
				base.Signal("OnDownloadError", this, request.error);
			}
			else
			{
				base.Texture = request.texture;
				if (this.DownloadComplete != null)
				{
					this.DownloadComplete(this, base.Texture);
				}
				base.Signal("OnDownloadComplete", this, base.Texture);
			}
		}
		yield break;
	}

	// Token: 0x040012E9 RID: 4841
	public PropertyChangedEventHandler<Texture> DownloadComplete;

	// Token: 0x040012EA RID: 4842
	public PropertyChangedEventHandler<string> DownloadError;

	// Token: 0x040012EB RID: 4843
	[SerializeField]
	protected string url = string.Empty;

	// Token: 0x040012EC RID: 4844
	[SerializeField]
	protected Texture2D loadingImage;

	// Token: 0x040012ED RID: 4845
	[SerializeField]
	protected Texture2D errorImage;

	// Token: 0x040012EE RID: 4846
	[SerializeField]
	protected bool autoDownload = true;
}
