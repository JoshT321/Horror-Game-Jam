using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


	public class PlayerEyeDistortion : MonoBehaviour
	{
		public Material m_Mat;
		public enum EType { Rotated = 0, Splitted, Waggle };
		public EType m_Type = EType.Rotated;
		[Range(0f, 1f)] public float m_DrunkIntensity = 0f;
		[Header("RGB Shift")]
		[Range(0f, 0.05f)] public float m_RGBShiftFactor = 0;
		[Range(1f, 16f)] public float m_RGBShiftPower = 3f;
		[Header("Ghost")]
		[Range(0f, 0.06f)] public float m_GhostSeeRadius = 0.01f;
		[Range(0.01f, 1f)] public float m_GhostSeeMix = 0.5f;
		[Range(0.01f, 0.2f)] public float m_GhostSeeAmplitude = 0.05f;
		[Header("Distortion")]
		[Range(0.5f, 8f)] public float m_Frequency = 1f;
		[Range(0.1f, 4f)] public float m_Period = 1.5f;
		[Range(1f, 16f)] public float m_Amplitude = 1f;
		[Header("Radial Blur")]
		[Range(0f, 1f)] public float m_BlurMin = 0.1f;
		[Range(0f, 1f)] public float m_BlurMax = 0.3f;
		[Range(1f, 6f)] public float m_BlurSpeed = 3f;
		[Header("SleepyEye")]
		public bool m_SleepyEye = false;
		[Range(0f, 0.9f)] public float m_EyeClose = 0.4f;

		void Start()
		{
			GraphicsSettings.renderPipelineAsset = null;   // disable URP
			QualitySettings.antiAliasing = 8;
		}
		void Update()
		{
			m_Mat.SetFloat("_RGBShiftFactor", m_RGBShiftFactor);
			m_Mat.SetFloat("_RGBShiftPower", m_RGBShiftPower);
			m_Mat.SetFloat("_GhostSeeRadius", m_GhostSeeRadius);
			m_Mat.SetFloat("_GhostSeeMix", m_GhostSeeMix);
			m_Mat.SetFloat("_GhostSeeAmplitude", m_GhostSeeAmplitude);
			
//			float strength = Mathf.Sin(Time.time) * 0.5f + 0.5f + 0.1f;
			m_Mat.SetVector("_Dimensions", new Vector4(0.8f, m_EyeClose, 0f, 0f));
			m_Mat.SetFloat("_Frequency", m_Frequency);
			m_Mat.SetFloat("_Period", m_Period);
			m_Mat.SetFloat("_RandomNumber", 1f);
			m_Mat.SetFloat("_Amplitude", m_Amplitude);
			m_Mat.SetFloat("_BlurMin", m_BlurMin);
			m_Mat.SetFloat("_BlurMax", m_BlurMax);
			m_Mat.SetFloat("_BlurSpeed", m_BlurSpeed);
		}
		void OnRenderImage(RenderTexture src, RenderTexture dst)
		{
			Shader.SetGlobalTexture("_Global_OrigScene", src);
			Shader.SetGlobalFloat("_Global_Fade", m_DrunkIntensity);
			

			RenderTexture rt1 = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
			RenderTexture rt2 = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
			Graphics.Blit(src, rt1, m_Mat, 0);   // RGB split pass
			if (EType.Rotated == m_Type)
			{
				if (m_SleepyEye)
				{
					Graphics.Blit(rt1, rt2, m_Mat, 1);
					Graphics.Blit(rt2, rt1, m_Mat, 4);
					Graphics.Blit(rt1, rt2, m_Mat, 5);
					Graphics.Blit(rt2, dst, m_Mat, 3);
				}
				else
				{
					Graphics.Blit(rt1, rt2, m_Mat, 1);
					Graphics.Blit(rt2, rt1, m_Mat, 4);
					Graphics.Blit(rt1, dst, m_Mat, 5);
				}
			}
			if (EType.Splitted == m_Type)
			{
				if (m_SleepyEye)
				{
					Graphics.Blit(rt1, rt2, m_Mat, 2);
					Graphics.Blit(rt2, rt1, m_Mat, 4);
					Graphics.Blit(rt1, rt2, m_Mat, 5);
					Graphics.Blit(rt2, dst, m_Mat, 3);
				}
				else
				{
					Graphics.Blit(rt1, rt2, m_Mat, 2);
					Graphics.Blit(rt2, rt1, m_Mat, 4);
					Graphics.Blit(rt1, dst, m_Mat, 5);
				}
			}
			if (EType.Waggle == m_Type)
			{
				if (m_SleepyEye)
				{
					Graphics.Blit(rt1, rt2, m_Mat, 4);
					Graphics.Blit(rt2, rt1, m_Mat, 6);
					Graphics.Blit(rt1, dst, m_Mat, 3);
				}
				else
				{
					Graphics.Blit(rt1, rt2, m_Mat, 4);
					Graphics.Blit(rt2, dst, m_Mat, 6);
				}
			}
			RenderTexture.ReleaseTemporary(rt1);
			RenderTexture.ReleaseTemporary(rt2);
		}
//		void OnGUI ()
//		{
//			int w = 350;
//			if (EType.ET_Rotated == m_Type)
//				GUI.Box (new Rect (Screen.width / 2 - w / 2, 10, w, 25), "Drunk Man Demo --- Rotated Double Image");
//			else
//				GUI.Box (new Rect (Screen.width / 2 - w / 2, 10, w, 25), "Drunk Man Demo --- Splitted Double Image");
//		}
	}
