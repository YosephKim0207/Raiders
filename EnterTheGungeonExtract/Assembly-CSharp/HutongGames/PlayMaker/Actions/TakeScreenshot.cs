using System;
using System.IO;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B2F RID: 2863
	[ActionCategory(ActionCategory.Application)]
	[Tooltip("Saves a Screenshot. NOTE: Does nothing in Web Player. On Android, the resulting screenshot is available some time later.")]
	public class TakeScreenshot : FsmStateAction
	{
		// Token: 0x06003C40 RID: 15424 RVA: 0x0012F320 File Offset: 0x0012D520
		public override void Reset()
		{
			this.destination = TakeScreenshot.Destination.MyPictures;
			this.filename = string.Empty;
			this.autoNumber = null;
			this.superSize = null;
			this.debugLog = null;
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x0012F350 File Offset: 0x0012D550
		public override void OnEnter()
		{
			if (string.IsNullOrEmpty(this.filename.Value))
			{
				return;
			}
			string text;
			switch (this.destination)
			{
			case TakeScreenshot.Destination.MyPictures:
				text = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
				break;
			case TakeScreenshot.Destination.PersistentDataPath:
				text = Application.persistentDataPath;
				break;
			case TakeScreenshot.Destination.CustomPath:
				text = this.customPath.Value;
				break;
			default:
				text = string.Empty;
				break;
			}
			text = text.Replace("\\", "/") + "/";
			string text2 = text + this.filename.Value + ".png";
			if (this.autoNumber.Value)
			{
				while (File.Exists(text2))
				{
					this.screenshotCount++;
					text2 = string.Concat(new object[]
					{
						text,
						this.filename.Value,
						this.screenshotCount,
						".png"
					});
				}
			}
			if (this.debugLog.Value)
			{
				Debug.Log("TakeScreenshot: " + text2);
			}
			ScreenCapture.CaptureScreenshot(text2, this.superSize.Value);
			base.Finish();
		}

		// Token: 0x04002E72 RID: 11890
		[Tooltip("Where to save the screenshot.")]
		public TakeScreenshot.Destination destination;

		// Token: 0x04002E73 RID: 11891
		[Tooltip("Path used with Custom Path Destination option.")]
		public FsmString customPath;

		// Token: 0x04002E74 RID: 11892
		[RequiredField]
		public FsmString filename;

		// Token: 0x04002E75 RID: 11893
		[Tooltip("Add an auto-incremented number to the filename.")]
		public FsmBool autoNumber;

		// Token: 0x04002E76 RID: 11894
		[Tooltip("Factor by which to increase resolution.")]
		public FsmInt superSize;

		// Token: 0x04002E77 RID: 11895
		[Tooltip("Log saved file info in Unity console.")]
		public FsmBool debugLog;

		// Token: 0x04002E78 RID: 11896
		private int screenshotCount;

		// Token: 0x02000B30 RID: 2864
		public enum Destination
		{
			// Token: 0x04002E7A RID: 11898
			MyPictures,
			// Token: 0x04002E7B RID: 11899
			PersistentDataPath,
			// Token: 0x04002E7C RID: 11900
			CustomPath
		}
	}
}
