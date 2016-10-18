using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestPj.Test
{

    [TestFixture]
    public class ThumbnailTest
    {
        string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        [Test]
        public void Test()
        {
            int count = 0;

            var files = System.IO.Directory.GetFiles(path, "*.jpg");
            Thread[] threads = new Thread[20];
            for (int kk = 0; kk < threads.Length; kk++)
            {
                threads[kk] = new Thread(n =>
                {
                    for (int i = 0; i < 9999; i++)
                    {
                        foreach (var item in files)
                        {
                            try
                            {


                                count++;
                                var fs = System.IO.File.Open(item, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                                var image = Image.FromStream(fs);
                                CreateThumbnail(image);
                                image.Dispose();
                                fs.Dispose();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Count:{0}", count);
                                Console.WriteLine(e);

                                break;
                            }
                        }

                    }
                });
            }
            foreach (var item in threads)
            {
                item.Start();
            }
            foreach (var item in threads)
            {
                item.Join();
            }
            Console.WriteLine("OK");

        }
        protected void CreateThumbnail(Image image)
        {


            var ThumbnailWidth = 0;
            var ThumbnailHeight = 0;
            if (image.Width < 120 && image.Height < 120)
            {
                ThumbnailWidth = image.Width;
                ThumbnailHeight = image.Height;
            }
            else if (image.Width > image.Height)
            {
                var per = (decimal)120 / image.Width;
                ThumbnailWidth = 120;

                ThumbnailHeight = (int)(image.Height * per);
            }
            else
            {
                var per = (decimal)120 / image.Height;
                ThumbnailHeight = 120;

                ThumbnailWidth = (int)(image.Width * per);
            }

            var thumbnailImage = image.GetThumbnailImage(ThumbnailWidth, ThumbnailHeight, () => { return false; }, IntPtr.Zero);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            thumbnailImage.Save(ms, ImageFormat.Jpeg);
            var buff = ms.ToArray();
            ms.Dispose();
            thumbnailImage.Dispose();
        }
    }
}
