using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;

namespace FeralTic.DX11
{

    public partial class DX11ShaderInstance : IDisposable
    {
        public class CounterResetUAV
        {
            public CounterResetUAV(UnorderedAccessView uav, int counter)
            {
                this.UAV = uav;
                this.Counter = counter;
            }

            public UnorderedAccessView UAV;
            public int Counter;
        }

        private Effect effect;
        private DX11RenderContext context;

        private EffectTechnique currenttechnique;

        private ShaderResourceView[] nullsrvs = new ShaderResourceView[128];
        private UnorderedAccessView[] nulluavs = new UnorderedAccessView[8];

        private List<CounterResetUAV> resetuavs = new List<CounterResetUAV>();

        public Effect Effect { get { return this.effect; } }

        public EffectTechnique CurrentTechnique { get { return this.currenttechnique; } }
        public DX11RenderContext RenderContext { get { return this.context; } }


        public DX11ShaderInstance(DX11RenderContext context, DX11Effect effect)
        {
            this.context = context;
            this.effect = new Effect(context.Device, effect.ByteCode);
            this.currenttechnique = this.effect.GetTechniqueByIndex(0);
        }

        public DX11ShaderInstance(DX11RenderContext context, ShaderBytecode bytecode)
        {
            this.context = context;
            this.effect = new Effect(context.Device, bytecode);
            this.currenttechnique = this.effect.GetTechniqueByIndex(0);
        }

        public DX11ShaderInstance(DX11RenderContext context,Effect effect)
        {
            this.context = context;
            this.effect = effect;
            this.currenttechnique = this.effect.GetTechniqueByIndex(0);
        }

        public bool HasTechnique(string name)
        {
            return this.effect.GetTechniqueByName(name).IsValid;
        }

        public void SelectTechnique(string name)
        {
            this.currenttechnique = this.effect.GetTechniqueByName(name);
        }

        public void SelectTechnique(int index)
        {
            this.currenttechnique = this.effect.GetTechniqueByIndex(index);
        }

        public void ApplyPass(int index)
        {
            this.currenttechnique.GetPassByIndex(index).Apply(this.context.CurrentDeviceContext);

            //Reapply UAVs with a reset counter
            this.ApplyCounterUAVS();
        }

        public void ApplyPass(EffectPass pass)
        {
            pass.Apply(this.context.CurrentDeviceContext);
            this.ApplyCounterUAVS();
        }

        public EffectPass GetPass(int index)
        {
            return this.currenttechnique.GetPassByIndex(index);
        }

        public void CleanShaderStages()
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            ctx.HullShader.Set(null);
            ctx.DomainShader.Set(null);
            ctx.VertexShader.Set(null);
            ctx.PixelShader.Set(null);
            ctx.GeometryShader.Set(null);
            ctx.ComputeShader.Set(null);
        }

        public void CleanUp()
        {
            this.context.CurrentDeviceContext.ComputeShader.SetShaderResources(nullsrvs, 0, 128);

            if (this.context.IsFeatureLevel11)
            {
                this.context.CurrentDeviceContext.ComputeShader.SetUnorderedAccessViews(nulluavs, 0, 8);
            }
            else
            {
                this.context.CurrentDeviceContext.ComputeShader.SetUnorderedAccessViews(nulluavs, 0, 1);
            }
        }

        #region Apply Counter UAVS
        private void ApplyCounterUAVS()
        {
            foreach (CounterResetUAV ru in this.resetuavs)
            {
                
                int i = 0;
                bool found = false;

                //Get currently bound UAVs
                UnorderedAccessView[] uavs = this.context.CurrentDeviceContext.ComputeShader.GetUnorderedAccessViews(0, 8);
                
                //Search for uav slot, if found, reapply with counter value
                for (i = 0; i < 8 && !found; i++)
                {
                    if (uavs[i] == ru.UAV)
                    {
                        found = true;
                    }
                }
                if (found)
                {
                    this.context.CurrentDeviceContext.ComputeShader.SetUnorderedAccessView(ru.UAV, i-1, ru.Counter);
                }
            }
            this.resetuavs.Clear();
        }
        #endregion

        public void Dispose()
        {
            this.effect.Dispose();
        }
    }
}
