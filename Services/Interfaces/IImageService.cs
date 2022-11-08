using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IImageService
    {
        Task<List<ImageModel>> ListImageAsync();

        void Add(ImageModel imgModel);

        Task<string> CreateImageAddAsync(ImageModel imageModel);
        Task<string> CreateImageAddAsync(Product product);

        Task Edit(int id, ImageModel imageModel);

    }
}
