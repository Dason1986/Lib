﻿using System.Drawing;

namespace Library.Draw.Effects
{
    public sealed class WaterImageTileOption : ImageOption
    {
        /// <summary>
        /// 偏移
        /// </summary>
        public Point? Offset { get; set; }
        /// <summary>
        /// 隔多少空間
        /// </summary>
        public Size? Space { get; set; }
    }
}