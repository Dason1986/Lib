using Library.HelperUtility;
using System.IO;

namespace Library.FileExtension.ExcelDataReader
{
    /// <summary>
    /// http://exceldatareader.codeplex.com/
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// Creates an instance of <see cref="ExcelBinaryReader"/>
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="isFirstRowAsColumnNames"></param>
        /// <returns></returns>
        public static IExcelDataReader CreateBinaryReader(Stream fileStream, bool isFirstRowAsColumnNames = true)
        {
            IExcelDataReader reader = new ExcelBinaryReader(isFirstRowAsColumnNames);
            reader.Initialize(fileStream);

            return reader;
        }

        /// <summary>
        /// Creates an instance of <see cref="ExcelOpenXmlReader"/>
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="isFirstRowAsColumnNames"></param>
        /// <returns></returns>
        public static IExcelDataReader CreateOpenXmlReader(Stream fileStream, bool isFirstRowAsColumnNames = true)
        {
            IExcelDataReader reader = new ExcelOpenXmlReader(isFirstRowAsColumnNames);
            reader.Initialize(fileStream);

            return reader;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="isFirstRowAsColumnNames"></param>
        /// <returns></returns>
        public static IExcelDataReader CreateReader(Stream fileStream, bool isFirstRowAsColumnNames = true)
        {
            IExcelDataReader reader = null;
            var code = FileUtility.GetFileCode(fileStream);
            switch (code)
            {
                case "080075":
                    reader = new ExcelOpenXmlReader(isFirstRowAsColumnNames);
                    reader.Initialize(fileStream); break;
                case "208207":
                    {
                        reader = new ExcelBinaryReader(isFirstRowAsColumnNames);
                        reader.Initialize(fileStream);

                        break;
                    }
            }
            return reader;
        }
    }
}