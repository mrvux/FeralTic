using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11
{
    public static class DX11BlendStates
    {
        private static BlendStateDescription[] descriptions;

        public static BlendStateDescription GetState(BlendStatePreset preset)
        {
            if (descriptions == null)
            {
                Initialize();
            }
            return descriptions[(int)preset];
        }

        public static BlendStateDescription GetState(string presetString)
        {
            BlendStatePreset preset;
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
            descriptions = new BlendStateDescription[Enum.GetValues(typeof(BlendStatePreset)).Length];
            CreateNoBlend();
            CreateAddivite();
            CreateBlend();
            CreateMultiply();
            CreateAlphaAdd();
            CreateTextDefault();
            CreateKeep();
            CreateConstantFactor();
            CreateBlendDestination();
            CreateReplaceAlpha();
        }

        private static void CreateNoBlend()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                bs.RenderTargets[i] = new RenderTargetBlendDescription()
                {
                    BlendEnable = false,
                    BlendOperation = SlimDX.Direct3D11.BlendOperation.Add,
                    BlendOperationAlpha = SlimDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.Zero,
                    DestinationBlendAlpha = BlendOption.Zero,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    SourceBlend = BlendOption.One,
                    SourceBlendAlpha = BlendOption.One
                };
            }
            descriptions[(int)BlendStatePreset.Disabled] = bs;
        }

        private static void CreateAddivite()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                //bs.IndependentBlendEnable 

                bs.RenderTargets[i] = new RenderTargetBlendDescription()
                {
                    BlendEnable = true,
                    BlendOperation = SlimDX.Direct3D11.BlendOperation.Add,
                    BlendOperationAlpha = SlimDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.One,
                    DestinationBlendAlpha = BlendOption.One,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    SourceBlend = BlendOption.One,
                    SourceBlendAlpha = BlendOption.One
                };
            }
            descriptions[(int)BlendStatePreset.Add] = bs;
        }

        private static void CreateBlend()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                bs.RenderTargets[i] = new RenderTargetBlendDescription()
                {
                    BlendEnable = true,
                    BlendOperation = SlimDX.Direct3D11.BlendOperation.Add,
                    BlendOperationAlpha = SlimDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.InverseSourceAlpha,
                    DestinationBlendAlpha = BlendOption.One,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    SourceBlend = BlendOption.SourceAlpha,
                    SourceBlendAlpha = BlendOption.One
                };
            }
            descriptions[(int)BlendStatePreset.Blend] = bs;
        }

        private static void CreateMultiply()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                bs.RenderTargets[i] = new RenderTargetBlendDescription()
                {
                    BlendEnable = true,
                    BlendOperation = SlimDX.Direct3D11.BlendOperation.Add,
                    BlendOperationAlpha = SlimDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.Zero,
                    DestinationBlendAlpha = BlendOption.Zero,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    SourceBlend = BlendOption.DestinationColor,
                    SourceBlendAlpha = BlendOption.DestinationAlpha
                };
            }
            descriptions[(int)BlendStatePreset.Multiply] = bs;
        }

        private static void CreateAlphaAdd()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                bs.RenderTargets[i] = new RenderTargetBlendDescription()
                {
                    BlendEnable = true,
                    BlendOperation = SlimDX.Direct3D11.BlendOperation.Add,
                    BlendOperationAlpha = SlimDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.One,
                    DestinationBlendAlpha = BlendOption.Zero,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    SourceBlend = BlendOption.SourceAlpha,
                    SourceBlendAlpha = BlendOption.Zero
                };
            }
            descriptions[(int)BlendStatePreset.AlphaAdd] = bs;
        }

        private static void CreateTextDefault()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                bs.RenderTargets[i] = new RenderTargetBlendDescription()
                {
                    BlendEnable = true,
                    BlendOperation = SlimDX.Direct3D11.BlendOperation.Add,
                    BlendOperationAlpha = SlimDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.InverseSourceAlpha,
                    DestinationBlendAlpha = BlendOption.Zero,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    SourceBlend = BlendOption.One,
                    SourceBlendAlpha = BlendOption.One
                };
            }
            descriptions[(int)BlendStatePreset.TextDefault] = bs;
        }

        private static void CreateKeep()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                //bs.IndependentBlendEnable 

                bs.RenderTargets[i] = new RenderTargetBlendDescription()
                {
                    BlendEnable = true,
                    BlendOperation = SlimDX.Direct3D11.BlendOperation.Add,
                    BlendOperationAlpha = SlimDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.One,
                    DestinationBlendAlpha = BlendOption.One,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    SourceBlend = BlendOption.Zero,
                    SourceBlendAlpha = BlendOption.Zero
                };
            }
            descriptions[(int)BlendStatePreset.Keep] = bs;
        }

        private static void CreateConstantFactor()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                bs.RenderTargets[i] = new RenderTargetBlendDescription()
                {
                    BlendEnable = true,
                    BlendOperation = SlimDX.Direct3D11.BlendOperation.Add,
                    BlendOperationAlpha = SlimDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.InverseBlendFactor,
                    DestinationBlendAlpha = BlendOption.Zero,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    SourceBlend = BlendOption.BlendFactor,
                    SourceBlendAlpha = BlendOption.One
                };
            }
            descriptions[(int)BlendStatePreset.ConstantFactor] = bs;
        }

        private static void CreateBlendDestination()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                bs.RenderTargets[i] = new RenderTargetBlendDescription()
                {
                    BlendEnable = true,
                    BlendOperation = SlimDX.Direct3D11.BlendOperation.Add,
                    BlendOperationAlpha = SlimDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.InverseDestinationAlpha,
                    DestinationBlendAlpha = BlendOption.One,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    SourceBlend = BlendOption.DestinationAlpha,
                    SourceBlendAlpha = BlendOption.One
                };
            }
            descriptions[(int)BlendStatePreset.BlendDestination] = bs;
        }

        private static void CreateReplaceAlpha()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                bs.RenderTargets[i] = new RenderTargetBlendDescription()
                {
                    BlendEnable = true,
                    BlendOperation = SlimDX.Direct3D11.BlendOperation.Add,
                    BlendOperationAlpha = SlimDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.Zero,
                    DestinationBlendAlpha = BlendOption.Zero,
                    RenderTargetWriteMask = ColorWriteMaskFlags.Alpha,
                    SourceBlend = BlendOption.Zero,
                    SourceBlendAlpha = BlendOption.One
                };
            }
            descriptions[(int)BlendStatePreset.ReplaceAlpha] = bs;
        }

        private static void CreateMultiplyAlpha()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                bs.RenderTargets[i] = new RenderTargetBlendDescription()
                {
                    BlendEnable = true,
                    BlendOperation = SlimDX.Direct3D11.BlendOperation.Add,
                    BlendOperationAlpha = SlimDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.Zero,
                    DestinationBlendAlpha = BlendOption.Zero,
                    RenderTargetWriteMask = ColorWriteMaskFlags.Alpha,
                    SourceBlend = BlendOption.Zero,
                    SourceBlendAlpha = BlendOption.DestinationAlpha
                };
            }
            descriptions[(int)BlendStatePreset.MultiplyAlpha] = bs;
        }
    }
}
