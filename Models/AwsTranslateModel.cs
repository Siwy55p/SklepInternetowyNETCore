namespace partner_aluro.Models
{
    public class AwsTranslateModel
    {
        public string InputText { get; set; }

        public string LanguageCode { get; set; }

        public string ResultText { get; set; }

    }

    public enum SupportedLanguages
    {
        fr,
        de,
        en,
        pl
    }
}
