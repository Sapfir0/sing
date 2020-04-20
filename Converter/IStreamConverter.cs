using System.IO;

namespace Converter
{
    public interface IStreamConverter
    {
        void Convert(Stream inputStream, Stream outputStream);
    }
}