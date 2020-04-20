using System.IO;

namespace Converter
{
    public interface IByteConverter
    {
        void Convert(byte[] bytes, Stream outputStream);
    }
}