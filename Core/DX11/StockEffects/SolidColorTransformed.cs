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
    public class SolidColorTransformed : IDisposable
    {
        private DX11RenderContext context;
        private Effect effect;
        private EffectPass pass;

        private EffectMatrixVariable viewProjVariable;
        private EffectMatrixVariable worldVariable;
        private EffectVectorVariable colorVariable;

        public SolidColorTransformed(DX11RenderContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            this.context = context;

            using (var cef = DX11Effect.FromResource(Assembly.GetExecutingAssembly(), "FeralTic.Effects.TransformColor.fx"))
            {
                this.effect = new Effect(context.Device, cef.ByteCode);
                this.pass = this.effect.GetTechniqueByIndex(0).GetPassByIndex(0);

                this.viewProjVariable = this.effect.GetVariableByName("tVP").AsMatrix();
                this.worldVariable = this.effect.GetVariableByName("tW").AsMatrix();
                this.colorVariable = this.effect.GetVariableByName("color").AsVector();
            }
        }

        public EffectPass EffectPass
        {
            get { return this.pass; }
        }
        
        public void ApplyCamera(Matrix viewProjection)
        {
            this.viewProjVariable.SetMatrix(viewProjection);
        }

        public void ApplyWorld(Matrix world)
        {
            this.worldVariable.SetMatrix(world);
        }

        public void ApplyColor(Color4 color)
        {
            this.colorVariable.Set(color);
        }

        public void ApplyPass()
        {
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
