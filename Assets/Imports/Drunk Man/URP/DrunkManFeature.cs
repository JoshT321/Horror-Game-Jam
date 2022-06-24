using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DrunkMan
{
	public class DrunkManFeature : ScriptableRendererFeature
	{
		public class BlitPass : ScriptableRenderPass
		{
			Material m_Mat;
			RenderTargetIdentifier m_Source;
			RenderTargetHandle m_RtHandle1;
			RenderTargetHandle m_RtHandle2;
			string m_Tag;
			bool m_SleepyEye = true;
			EType m_Type = EType.Rotated;
			float m_Intensity = 0f;

			public BlitPass(string tag)
			{
				this.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
				m_Tag = tag;
				m_RtHandle1.Init("_Rt1");
				m_RtHandle2.Init("_Rt2");
			}
			public void Setup(RenderTargetIdentifier src, Material mat, bool sleepyEye, EType tp, float intensity)
			{
				m_Source = src;
				m_Mat = mat;
				m_SleepyEye = sleepyEye;
				m_Type = tp;
				m_Intensity = intensity;
			}
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer cmd = CommandBufferPool.Get(m_Tag);

				RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
				desc.depthBufferBits = 0;
				cmd.GetTemporaryRT(m_RtHandle1.id, desc, FilterMode.Bilinear);
				cmd.GetTemporaryRT(m_RtHandle2.id, desc, FilterMode.Bilinear);

				cmd.SetGlobalTexture("_Global_OrigScene", m_Source);
				cmd.SetGlobalFloat("_Global_Fade", m_Intensity);

				Blit(cmd, m_Source, m_RtHandle1.Identifier(), m_Mat, 0);
				if (EType.Rotated == m_Type)
				{
					if (m_SleepyEye)
					{
						Blit(cmd, m_RtHandle1.Identifier(), m_RtHandle2.Identifier(), m_Mat, 1);
						Blit(cmd, m_RtHandle2.Identifier(), m_RtHandle1.Identifier(), m_Mat, 4);
						Blit(cmd, m_RtHandle1.Identifier(), m_RtHandle2.Identifier(), m_Mat, 5);
						Blit(cmd, m_RtHandle2.Identifier(), m_Source, m_Mat, 3);
					}
					else
					{
						Blit(cmd, m_RtHandle1.Identifier(), m_RtHandle2.Identifier(), m_Mat, 1);
						Blit(cmd, m_RtHandle2.Identifier(), m_RtHandle1.Identifier(), m_Mat, 4);
						Blit(cmd, m_RtHandle1.Identifier(), m_Source, m_Mat, 5);
					}
				}
				if (EType.Splitted == m_Type)
				{
					if (m_SleepyEye)
					{
						Blit(cmd, m_RtHandle1.Identifier(), m_RtHandle2.Identifier(), m_Mat, 2);
						Blit(cmd, m_RtHandle2.Identifier(), m_RtHandle1.Identifier(), m_Mat, 4);
						Blit(cmd, m_RtHandle1.Identifier(), m_RtHandle2.Identifier(), m_Mat, 5);
						Blit(cmd, m_RtHandle2.Identifier(), m_Source, m_Mat, 3);
					}
					else
					{
						Blit(cmd, m_RtHandle1.Identifier(), m_RtHandle2.Identifier(), m_Mat, 2);
						Blit(cmd, m_RtHandle2.Identifier(), m_RtHandle1.Identifier(), m_Mat, 4);
						Blit(cmd, m_RtHandle1.Identifier(), m_Source, m_Mat, 5);
					}
				}
				if (EType.Waggle == m_Type)
				{
					if (m_SleepyEye)
					{
						Blit(cmd, m_RtHandle1.Identifier(), m_RtHandle2.Identifier(), m_Mat, 4);
						Blit(cmd, m_RtHandle2.Identifier(), m_RtHandle1.Identifier(), m_Mat, 6);
						Blit(cmd, m_RtHandle1.Identifier(), m_Source, m_Mat, 3);
					}
					else
					{
						Blit(cmd, m_RtHandle1.Identifier(), m_RtHandle2.Identifier(), m_Mat, 4);
						Blit(cmd, m_RtHandle2.Identifier(), m_Source, m_Mat, 6);
					}
				}
				context.ExecuteCommandBuffer(cmd);
				CommandBufferPool.Release(cmd);
			}
			public override void FrameCleanup(CommandBuffer cmd)
			{
				cmd.ReleaseTemporaryRT(m_RtHandle1.id);
				cmd.ReleaseTemporaryRT(m_RtHandle2.id);
			}
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public enum EType { Rotated = 0, Splitted, Waggle };

		[System.Serializable]
		public class Settings
		{
			[Header("Basic")]
			public Material m_Mat;
			public EType m_Type = EType.Rotated;
			[Range(0f, 1f)] public float m_Intensity = 1f;
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
		}
		public bool m_Enable = false;
		public Settings m_Settings = new Settings();
		BlitPass m_BlitPass;

		public override void Create()
		{
			m_BlitPass = new BlitPass(name);
		}
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (!m_Enable)
				return;

			var src = renderer.cameraColorTarget;
			if (m_Settings.m_Mat == null)
			{
				Debug.LogWarningFormat("Missing material. {0} blit pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
				return;
			}

			// setup material params
			Material m = m_Settings.m_Mat;
			m.SetFloat("_RGBShiftFactor", m_Settings.m_RGBShiftFactor);
			m.SetFloat("_RGBShiftPower", m_Settings.m_RGBShiftPower);
			m.SetFloat("_GhostSeeRadius", m_Settings.m_GhostSeeRadius);
			m.SetFloat("_GhostSeeMix", m_Settings.m_GhostSeeMix);
			m.SetFloat("_GhostSeeAmplitude", m_Settings.m_GhostSeeAmplitude);
			m.SetVector("_Dimensions", new Vector4(0.8f, m_Settings.m_EyeClose, 0f, 0f));
			m.SetFloat("_Frequency", m_Settings.m_Frequency);
			m.SetFloat("_Period", m_Settings.m_Period);
			m.SetFloat("_RandomNumber", 1f);
			m.SetFloat("_Amplitude", m_Settings.m_Amplitude);
			m.SetFloat("_BlurMin", m_Settings.m_BlurMin);
			m.SetFloat("_BlurMax", m_Settings.m_BlurMax);
			m.SetFloat("_BlurSpeed", m_Settings.m_BlurSpeed);

			m_BlitPass.Setup(src, m_Settings.m_Mat, m_Settings.m_SleepyEye, m_Settings.m_Type, m_Settings.m_Intensity);
			renderer.EnqueuePass(m_BlitPass);
		}
	}
}