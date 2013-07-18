using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.DXGI;
using SlimDX.Direct3D11;

namespace FeralTic.DX11
{
    public partial class DX11RenderContext
    {
        private Dictionary<Format, List<SampleDescription>> multisampleformats = new Dictionary<Format, List<SampleDescription>>();
        private Dictionary<FormatSupport, List<Format>> usageformats = new Dictionary<FormatSupport, List<Format>>();
        
        public List<SampleDescription> GetMultisampleFormatInfo(Format format)
        {
            return this.multisampleformats[format];
        }

        public List<Format> GetAllowedFormats(FormatSupport format)
        {
            return usageformats[format];
        }

        #region Build Formats Sampling
        private void BuildFormatSampling()
        {
            multisampleformats.Clear();

            foreach (string s in Enum.GetNames(typeof(Format)))
            {
                Format fmt = (Format)Enum.Parse(typeof(Format), s);

                List<SampleDescription> sdl = new List<SampleDescription>();

                for (int i = 0; i < SlimDX.Direct3D11.Device.MultisampleCountMaximum; i++)
                {
                    int level = this.Device.CheckMultisampleQualityLevels(fmt, i);
                    if (level > 0)
                    {
                        sdl.Add(new SampleDescription(i, level));
                    }
                }

                multisampleformats.Add(fmt, sdl);
            }
        }
        #endregion

        #region Is Supported
        /// <summary>
        /// Checks if a format is supported for a specific usage
        /// </summary>
        /// <param name="dev">Device to check</param>
        /// <param name="usage">Desired format usage</param>
        /// <param name="format">Desired format</param>
        /// <returns>true if format supported, false otherwise</returns>
        public bool IsSupported(FormatSupport usage, Format format)
        {
            FormatSupport support = this.Device.CheckFormatSupport(format);
            return (support | usage) == support;
        }
        #endregion

        #region Supported Formats
        /// <summary>
        /// Lists supported DXGI formats for a given usage
        /// </summary>
        /// <param name="dev">Device to check format support for</param>
        /// <param name="usage">Requested Usage</param>
        /// <returns>List of Supported formats</returns>
        private List<Format> SupportedFormats(FormatSupport usage)
        {
            List<Format> result = new List<Format>();
            foreach (object o in Enum.GetValues(typeof(Format)))
            {
                Format f = (Format)o;
                if (IsSupported(usage,f))
                {
                    result.Add(f);
                }
            }
            return result;
        }
        #endregion

        #region Build Format Supports
        private void BuildFormatSupport()
        {
            foreach (object o in Enum.GetValues(typeof(FormatSupport)))
            {
                FormatSupport usage = (FormatSupport)o;

                this.usageformats[usage] = this.SupportedFormats(usage);
            }
        }
        #endregion
    }
}
