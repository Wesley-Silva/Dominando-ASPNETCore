using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ASPNETCoreMVC.Services
{
    public interface IImageUploadService
    {
        public Task<bool> UploadArquivo(ModelStateDictionary modelstate, IFormFile arquivo, string imgPrefixo);
    }
}
