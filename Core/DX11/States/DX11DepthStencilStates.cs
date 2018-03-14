using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

namespace FeralTic.DX11
{
    public static class DX11DepthStencilStates
    {
        private static DepthStencilStateDescription[] descriptions;

        public static DepthStencilStateDescription GetState(DepthStencilStatePreset preset)
        {
            if (descriptions == null)
            {
                Initialize();
            }
            return descriptions[(int)preset];
        }

        public static DepthStencilStateDescription GetState(string presetString)
        {
            DepthStencilStatePreset preset;
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
            descriptions = new DepthStencilStateDescription[Enum.GetValues(typeof(DepthStencilStatePreset)).Length];
            CreateLessReadOnly();
            CreateLessRW();
            CreateNoDepth();
            CreateLessEqualReadOnly();
            CreateLessEqualRW();
            CreateWriteOnly();
            CreateLessStencilIncrement();
            CreateStencilLess();
            CreateStencilGreater();
            CreateStencilIncrement();
            CreateStencilInvert();
            CreateLessStencilZero();
            CreateStencilReplace();

        }

        private static void CreateNoDepth()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always
            };
            descriptions[(int)DepthStencilStatePreset.NoDepth] = ds;;
        }

        private static void CreateLessReadOnly()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Less
            };
            descriptions[(int)DepthStencilStatePreset.LessRead] = ds;
        }

        private static void CreateLessEqualReadOnly()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.LessEqual
            };
            descriptions[(int)DepthStencilStatePreset.LessEqualRead] = ds;
        }

        private static void CreateLessRW()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less
            };
            descriptions[(int)DepthStencilStatePreset.LessReadWrite] = ds;
        }

        private static void CreateLessEqualRW()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.LessEqual
            };
            descriptions[(int)DepthStencilStatePreset.LessEqualReadWrite] = ds;
        }

        private static void CreateWriteOnly()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Always
            };
            descriptions[(int)DepthStencilStatePreset.WriteOnly] = ds;
        }

        private static void CreateLessStencilIncrement()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
                StencilReadMask = 0,
                StencilWriteMask = 255,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.IncrementAndClamp
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.IncrementAndClamp
                }
            };
            descriptions[(int)DepthStencilStatePreset.LessReadStencilIncrement] = ds;
        }

        private static void CreateLessStencilZero()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
                StencilReadMask = 0,
                StencilWriteMask = 255,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Zero
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Zero
                }
            };
            descriptions[(int)DepthStencilStatePreset.LessReadStencilZero] = ds;
        }

        private static void CreateStencilLess()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always,
                StencilReadMask = 255,
                StencilWriteMask = 0,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Less,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Less,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                }
            };
            descriptions[(int)DepthStencilStatePreset.StencilLess] = ds;
        }

        private static void CreateStencilGreater()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always,
                StencilReadMask = 255,
                StencilWriteMask = 0,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Greater,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Greater,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                }
            };
            descriptions[(int)DepthStencilStatePreset.StencilGreater] = ds;
        }

        private static void CreateStencilIncrement()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always,
                StencilReadMask = 255,
                StencilWriteMask = 255,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.IncrementAndClamp
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.IncrementAndClamp
                }
            };
            descriptions[(int)DepthStencilStatePreset.StencilIncrement] = ds;
        }

        private static void CreateStencilInvert()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always,
                StencilReadMask = 255,
                StencilWriteMask = 255,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Invert
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Invert
                }
            };
            descriptions[(int)DepthStencilStatePreset.StencilInvert] = ds;
        }

        private static void CreateStencilReplace()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always,
                StencilReadMask = 255,
                StencilWriteMask = 255,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Replace
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Replace
                }
            };
            descriptions[(int)DepthStencilStatePreset.StencilReplace] = ds;
        }
    }
}
