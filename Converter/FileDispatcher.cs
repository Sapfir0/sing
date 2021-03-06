﻿using System;
using System.IO;
using File = Watcher.File;

namespace Converter
{
    public class FileDispatcher
    {

        private IStreamConverter _converter;
        private string _directory;
        public FileDispatcher(IStreamConverter converter, string saveDirectory)
        {
            _converter = converter;
            _directory = saveDirectory;
        }
        
        public void TransformFile(File image)
        {
            Console.WriteLine($"Изображение получено {image.data.Length}");
            var newFilePath = Path.Combine(_directory, image.name);
            
            using (var file = System.IO.File.OpenWrite(newFilePath))
            {
                _converter.Convert(new MemoryStream(image.data), file);
                file.Close();
            }
            
            Console.WriteLine("Изображение сохранено в " + newFilePath);
        }
    }
}