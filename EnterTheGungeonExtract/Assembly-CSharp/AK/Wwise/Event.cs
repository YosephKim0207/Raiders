using System;
using UnityEngine;

namespace AK.Wwise
{
	// Token: 0x020018D5 RID: 6357
	[Serializable]
	public class Event : BaseType
	{
		// Token: 0x06009CDD RID: 40157 RVA: 0x003ED960 File Offset: 0x003EBB60
		private void VerifyPlayingID(uint playingId)
		{
		}

		// Token: 0x06009CDE RID: 40158 RVA: 0x003ED964 File Offset: 0x003EBB64
		public uint Post(GameObject gameObject)
		{
			if (!this.IsValid())
			{
				return 0U;
			}
			uint num = AkSoundEngine.PostEvent(base.GetID(), gameObject);
			this.VerifyPlayingID(num);
			return num;
		}

		// Token: 0x06009CDF RID: 40159 RVA: 0x003ED994 File Offset: 0x003EBB94
		public uint Post(GameObject gameObject, CallbackFlags flags, AkCallbackManager.EventCallback callback, object cookie = null)
		{
			if (!this.IsValid())
			{
				return 0U;
			}
			uint num = AkSoundEngine.PostEvent(base.GetID(), gameObject, flags.value, callback, cookie);
			this.VerifyPlayingID(num);
			return num;
		}

		// Token: 0x06009CE0 RID: 40160 RVA: 0x003ED9CC File Offset: 0x003EBBCC
		public uint Post(GameObject gameObject, uint flags, AkCallbackManager.EventCallback callback, object cookie = null)
		{
			if (!this.IsValid())
			{
				return 0U;
			}
			uint num = AkSoundEngine.PostEvent(base.GetID(), gameObject, flags, callback, cookie);
			this.VerifyPlayingID(num);
			return num;
		}

		// Token: 0x06009CE1 RID: 40161 RVA: 0x003EDA00 File Offset: 0x003EBC00
		public void Stop(GameObject gameObject, int transitionDuration = 0, AkCurveInterpolation curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear)
		{
			this.ExecuteAction(gameObject, AkActionOnEventType.AkActionOnEventType_Stop, transitionDuration, curveInterpolation);
		}

		// Token: 0x06009CE2 RID: 40162 RVA: 0x003EDA0C File Offset: 0x003EBC0C
		public void ExecuteAction(GameObject gameObject, AkActionOnEventType actionOnEventType, int transitionDuration, AkCurveInterpolation curveInterpolation)
		{
			if (this.IsValid())
			{
				AKRESULT akresult = AkSoundEngine.ExecuteActionOnEvent(base.GetID(), actionOnEventType, gameObject, transitionDuration, curveInterpolation);
				base.Verify(akresult);
			}
		}

		// Token: 0x06009CE3 RID: 40163 RVA: 0x003EDA3C File Offset: 0x003EBC3C
		public void PostMIDI(GameObject gameObject, AkMIDIPostArray array)
		{
			if (this.IsValid())
			{
				array.PostOnEvent(base.GetID(), gameObject);
			}
		}

		// Token: 0x06009CE4 RID: 40164 RVA: 0x003EDA58 File Offset: 0x003EBC58
		public void PostMIDI(GameObject gameObject, AkMIDIPostArray array, int count)
		{
			if (this.IsValid())
			{
				array.PostOnEvent(base.GetID(), gameObject, count);
			}
		}

		// Token: 0x06009CE5 RID: 40165 RVA: 0x003EDA74 File Offset: 0x003EBC74
		public void StopMIDI(GameObject gameObject)
		{
			if (this.IsValid())
			{
				AkSoundEngine.StopMIDIOnEvent(base.GetID(), gameObject);
			}
		}

		// Token: 0x06009CE6 RID: 40166 RVA: 0x003EDA90 File Offset: 0x003EBC90
		public void StopMIDI()
		{
			if (this.IsValid())
			{
				AkSoundEngine.StopMIDIOnEvent(base.GetID());
			}
		}
	}
}
