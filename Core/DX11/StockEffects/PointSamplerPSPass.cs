using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FeralTic.DX11.Resources;
using SlimDX;
using SlimDX.Direct3D11;

namespace FeralTic.DX11.StockEffects
{
    public class PointSamplerPSPass : IDisposable
    {
        private DX11RenderContext context;
        private Effect effect;
        private EffectPass pass;

        private EffectResourceVariable textureVariable;

        public PointSamplerPSPass(DX11RenderContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            this.context = context;

            using (var cef = DX11Effect.FromResource(Assembly.GetExecutingAssembly(), "FeralTic.Effects.PSPassPoint.fx"))
            {
                this.effect = new Effect(context.Device, cef.ByteCode);
                this.pass = this.effect.GetTechniqueByIndex(0).GetPassByIndex(0);

                this.textureVariable = this.effect.GetVariableByName("inputTexture").AsResource();
            }
        }

        public EffectPass EffectPass
        {
            get { return this.pass; }
        }
        

        public void Apply(ShaderResourceView view)
        {
            this.textureVariable.SetResource(view);
            this.pass.Apply(this.context.CurrentDeviceContext);
        }

        public void Dispose()
        {
            if (this.effect != null)
            {
                this.effect.Dispose();
                this.effect = null;
                this.pass = null;
            }
        }
    }
}
