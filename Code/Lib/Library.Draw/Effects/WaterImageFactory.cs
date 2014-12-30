using System.Drawing;

namespace Library.Draw.Effects
{
    public class WaterImageFactory
    {
        public static WaterImageBuilder CreateBuilder(WaterImageType waterImageType)
        {
            WaterImageBuilder builder = null;
            switch (waterImageType)
            {
                case WaterImageType.Full: builder = new WaterImageBuilderByFill(); break;
                case WaterImageType.Pixel: builder = new WaterImageBuilderByPixel(); break;
                case WaterImageType.Tile: builder = new WaterImageBuilderByTile(); break;

            }
            return builder;
        }

        public static Image CreateWaterImage(WaterImageType waterImageType, string sourceImgPath, string waterImgPath, ImageOption option)
        {
            WaterImageBuilder builder = new WaterImageBuilderByFill();

            builder.SetSourceImage(sourceImgPath);
            builder.SetWaterImage(waterImgPath);
            return builder.ProcessBitmap(option);
        }
    }
}