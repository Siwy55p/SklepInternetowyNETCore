using Microsoft.AspNetCore.Mvc;

namespace partner_aluro.Views.Shared.Components.ImageUploader
{
    public class ImageUploaderViewComponent : ViewComponent
    {
        public ImageUploaderViewComponent()
        {
        }

        public IViewComponentResult Invoke(string FieldName)
        {
            return View("Default", FieldName);
        }


    }
}
