using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw
{
    /// <summary>
    ///
    /// </summary>
    public class GifImage
    {
        private readonly Image _gifImage;
        private readonly FrameDimension _dimension;
        private readonly int _frameCount;
        private int _currentFrame = -1;
        private int _step = 1;

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        public GifImage(string path)
        {
            _gifImage = Image.FromFile(path); //initialize
            _dimension = new FrameDimension(_gifImage.FrameDimensionsList[0]); //gets the GUID
            _frameCount = _gifImage.GetFrameCount(_dimension); //total frames in the animation
        }

        /// <summary>
        ///
        /// </summary>
        public bool ReverseAtEnd { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Image GetNextFrame()
        {
            _currentFrame += _step;

            if (_currentFrame < _frameCount && _currentFrame >= 1) return GetFrame(_currentFrame);
            if (ReverseAtEnd)
            {
                _step *= -1;
                _currentFrame += _step;
            }
            else
            {
                _currentFrame = 0;
            }
            return GetFrame(_currentFrame);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Image GetFrame(int index)
        {
            _gifImage.SelectActiveFrame(_dimension, index);
            return (Image)_gifImage.Clone();
        }
    }
}