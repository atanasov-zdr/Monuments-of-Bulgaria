namespace MB.Common.Utilities
{
    using System;
    using System.IO;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;

    using Microsoft.AspNetCore.Http;

    using Common;

    public class ImagesUploader
    {
        private readonly Cloudinary cloudinary;

        public ImagesUploader(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public virtual string Upload(IFormFile photo, string imagesDirectory, string imagesFolderName)
        {
            Directory.CreateDirectory(imagesDirectory);
            string fileName = Guid.NewGuid().ToString() + GlobalConstants.PhotoFileExtension;
            if (photo.Length > 0)
            {
                using (var stream = new FileStream(Path.Combine(imagesDirectory, fileName), FileMode.Create))
                {
                    photo.CopyTo(stream);
                }
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imagesDirectory + fileName),
                Folder = imagesFolderName,
            };

            ImageUploadResult uploadResult = this.cloudinary.Upload(uploadParams);
            return uploadResult.SecureUri.ToString();
        }
    }
}
