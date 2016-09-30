using System.Drawing;

namespace Library.Draw.Water
{
    /// <summary>
    ///
    /// </summary>
    public class WaterImageFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="waterImageType"></param>
        /// <returns></returns>
        public static WaterImageBuilder CreateBuilder(WaterImageType waterImageType)
        {
            WaterImageBuilder builder = null;
            switch (waterImageType)
            {
                case WaterImageType.Full: builder = new WaterImageBuilderByFill(); break;
                case WaterImageType.Pixel: builder = new WaterImageBuilderByPixel(); break;
                case WaterImageType.Tile: builder = new WaterImageBuilderByTile(); break;
                case WaterImageType.Text: builder = new WaterImageBuilderByText(); break;
            }
            return builder;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="waterImageType"></param>
        /// <param name="sourceImgPath"></param>
        /// <param name="waterImgPath"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static Image CreateWaterImage(WaterImageType waterImageType, string sourceImgPath, string waterImgPath, ImageOption option)
        {
            WaterImageBuilder builder = new WaterImageBuilderByFill();

            builder.SetSourceImage(sourceImgPath);
            builder.SetWaterImage(waterImgPath);
            builder.SetOpetion(option);
            return builder.ProcessBitmap();
        }
    }
}