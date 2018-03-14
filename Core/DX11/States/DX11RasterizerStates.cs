using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

namespace FeralTic.DX11
{
    public class DX11RasterizerStates
    {
        private static RasterizerStateDescription[] descriptions;

        public static RasterizerStateDescription GetState(RasterizerStatePreset preset)
        {
            if (descriptions == null)
            {
                Initialize();
            }
            return descriptions[(int)preset];
        }

        public static RasterizerStateDescription GetState(string presetString)
        {
            RasterizerStatePreset preset;
            if (Enum.TryParse(presetString, out preset))
            {
                return GetState(preset);
            }
            else
            {
                throw new ArgumentException("preset", "Preset not found");
            }
        }

        private static void Initialize()
        {
            descriptions = new RasterizerStateDescription[Enum.GetValues(typeof(RasterizerStatePreset)).Length];
            CreateBackCull();
            CreateFrontCull();
            CreateNoCull();
            CreateLine();
        }

        private static void CreateNoCull()
        {
            RasterizerStateDescription rsd = new RasterizerStateDescription()
            {
                CullMode = CullMode.None,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterclockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f             
            };
            descriptions[(int)RasterizerStatePreset.NoCullSimple] = rsd;

            rsd.FillMode = FillMode.Wireframe;
            descriptions[(int)RasterizerStatePreset.NoCullWireframe] = rsd;
        }

        private static void CreateBackCull()
        {
            RasterizerStateDescription rsd = new RasterizerStateDescription()
            {
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterclockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };
            descriptions[(int)RasterizerStatePreset.BackCullSimple] = rsd;

            rsd.FillMode = FillMode.Wireframe;
            descriptions[(int)RasterizerStatePreset.BackCullWireframe] = rsd;
        }

        private static void CreateFrontCull()
        {
            RasterizerStateDescription rsd = new RasterizerStateDescription()
            {
                CullMode = CullMode.Front,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterclockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };
            descriptions[(int)RasterizerStatePreset.FrontCullSimple] = rsd;

            rsd.FillMode = FillMode.Wireframe;
            descriptions[(int)RasterizerStatePreset.FrontCullWireframe] = rsd;
        }

        private static void CreateLine()
        {
            RasterizerStateDescription rsd = new RasterizerStateDescription()
            {
                CullMode = CullMode.None,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = true,
                IsDepthClipEnabled = true,
                IsFrontCounterclockwise = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };
            descriptions[(int)RasterizerStatePreset.LineAlpha] = rsd;

            rsd.IsMultisampleEnabled = true;
            descriptions[(int)RasterizerStatePreset.LineQuadrilateral] = rsd;
        }
    }
}
