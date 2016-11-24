using Library.HelperUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 
    /// </summary>
    public class EffectsHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetEffects()
        {

            string[] sourceImageEffects =
          {
                "BlueImage", "GreenImage", "RedImage", "FogImage", "GaussianBlurImage"
                , "FlipImage", "MosaicImage", "NeonImage", "GrayImage", "RebelliousImage"
                , "ReliefImage", "SharpenImage", "TwoValueImage", "ColorGradationImage", "BlindsImage", "IlluminationImage"
                , "ZoomBlurImage", "ColorQuantizeImage", "ColorToneImage", "AutoLevelImage", "HistogramEqualImage"
                , "BrightContrastImage", "CleanGlassImage", "FeatherImage", "RaiseFrameImage", "ReflectionImage"
                , "ThreeDGridImage"
            };
            var effectsAssembly = Assembly.GetExecutingAssembly();
            var dt = new DataTable();
            dt.Columns.Add("name");
            dt.Columns.Add("DisplayName");
            dt.Columns.Add("ImageBuilder", typeof(IImageBuilder));
            dt.Columns.Add("option", typeof(ImageOption));
            for (int i = 0; i < sourceImageEffects.Length; i++)
            {
                var name = sourceImageEffects[i];
                var dr = dt.NewRow();
                string classname = string.Format("Library.Draw.Effects.{0}", name);
                var typeobj = effectsAssembly.GetType(classname);
                var builderobj = typeobj.CreateInstance<IImageBuilder>();
                var att = typeobj.GetCustomAttributes(typeof(DisplayNameAttribute), true).OfType<DisplayNameAttribute>().FirstOrDefault();
                dr[0] = name;
                dr[1] = att == null ? name : att.DisplayName;
                dr[2] = builderobj;
                dr[3] = builderobj.CreateOption();
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
