using System.IO;

namespace Converter
{
    public interface IByteConverter
    {
        void Convert(Stream inputStream, Stream outputStream);
    }
}