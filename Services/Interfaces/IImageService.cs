using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IImageService
    {
        Task<List<ImageModel>> ListImageAsync();

        Task AddAsync(ImageModel imgModel);

        //Task<string> CreateImageAddAsync(ImageModel imageModel);
        //Task<string> CreateImageAddAsync(Product product);

        Task Edit(int id, ImageModel imageModel);

        //Task<string> DeleteFrontImage(Product product);

        ImageModel Get(string name);

        void Update(ImageModel imageModel);

        Task UploadFilesAsync(IFormFileCollection files, Product? product = null, Slider? slider = null);

    }
}
