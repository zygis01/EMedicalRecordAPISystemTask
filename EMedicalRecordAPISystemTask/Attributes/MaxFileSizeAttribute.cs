using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace EMedicalRecordAPISystemTask.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        private readonly int _maxWidth;
        private readonly int _maxHeight;

        public MaxFileSizeAttribute(int maxFileSize, int maxWidth = 200, int maxHeight = 200)
        {
            _maxFileSize = maxFileSize;
            _maxWidth = maxWidth;
            _maxHeight = maxHeight;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile file = value as IFormFile;

            if (file != null)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }

                using (var image = Image.Load(file.OpenReadStream()))
                {
                    if (image.Width > _maxWidth || image.Height > _maxHeight)
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(_maxWidth, _maxHeight),
                            Mode = ResizeMode.Max
                        }));
                    }

                    using (var stream = new MemoryStream())
                    {
                        image.Save(stream, new JpegEncoder());
                        file.OpenReadStream().Seek(0, SeekOrigin.Begin);
                        file.OpenReadStream().SetLength(stream.Length);
                        stream.Position = 0;
                        file.OpenReadStream().Write(stream.ToArray(), 0, (int)stream.Length);
                        
                    }
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is {_maxFileSize} bytes.";
        }
    }
}
