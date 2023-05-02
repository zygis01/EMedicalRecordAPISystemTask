using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace EMedicalRecordAPISystemTask.Services
{
    public class ProfilePicService
    {
        public async Task<byte[]> ProcessProfilePicAsync(IFormFile profilePic)
        {
            if (profilePic == null)
            {
                throw new ArgumentNullException(nameof(profilePic), "Profile picture cannot be null.");
            }

            var allowedExtensions = new[] { ".jpg", ".png" };
            var extension = Path.GetExtension(profilePic.FileName);
            if (!allowedExtensions.Contains(extension))
            {
                throw new ArgumentException($"File extension {extension} is not allowed.", nameof(profilePic));
            }

            using var image = await Image.LoadAsync(profilePic.OpenReadStream());
            // Resize the image to 200x200 if it is too big or too small
            var width = image.Width > 200 ? 200 : image.Width;
            var height = image.Height > 200 ? 200 : image.Height;
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(width, height),
                Mode = ResizeMode.Max
            }));

            // Convert the image to a byte array
            using var output = new MemoryStream();
            await image.SaveAsync(output, new JpegEncoder());
            return output.ToArray();
        }
    }
}
