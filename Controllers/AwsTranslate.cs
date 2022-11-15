using Amazon.Runtime;
using Amazon.Translate;
using Amazon.Translate.Model;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Models;

namespace partner_aluro.Controllers
{
    public class AwsTranslate : Controller
    {
        AWSCredentials _awsCredentials;

        public AwsTranslate()
        {
            var chain = new Amazon.Runtime.CredentialManagement.CredentialProfileStoreChain();

            if(!chain.TryGetAWSCredentials("microsoft",out _awsCredentials))
            {
                throw new Exception("AWS Credential not available.");
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Translate(string text, string languageCode)
        {
            AwsTranslateModel model = new AwsTranslateModel()
            {
                InputText = "This is sample text",
                LanguageCode = "en",
            };

            if(!string.IsNullOrEmpty(text))
            {
                model.InputText = text;
            }
            if(!string.IsNullOrEmpty(languageCode))
            {
                model.LanguageCode = languageCode;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Translate(AwsTranslateModel formData)
        {
            using (IAmazonTranslate client = new AmazonTranslateClient(_awsCredentials))
            {
                TranslateTextRequest request = new TranslateTextRequest()
                {
                    SourceLanguageCode = "auto",
                    TargetLanguageCode = formData.LanguageCode,
                    Text = formData.InputText,
                };
                var result = await client.TranslateTextAsync(request);
                formData.ResultText = result.TranslatedText;
            }
            return RedirectToAction("Display", formData);
        }

        public IActionResult Display(AwsTranslateModel model)
        {
            return View(model);
        }

    }
}
