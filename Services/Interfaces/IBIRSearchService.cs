using partner_aluro.Models;

namespace partner_aluro
{
    public interface IBIRSearchService
    {
        /// <summary>
        /// Zwraca dane podmiotu w postaci modelu na postawie numeru identyfikacji VAT.
        /// </summary>
        /// <param name="vatId">Nr indentyfikacji podatkowej VAT</param>
        /// <returns></returns>
        Task<DanePodmiotu> GetCompanyDataByNipIdAsync(string vatId);
        /// <summary>
        /// Zwraca dane podmiotu w postaci modelu na podstawie numeru Regon.
        /// </summary>
        /// <param name="regonId">Nr identyfikacyjny Regon</param>
        /// <returns></returns>
        Task<DanePodmiotu> GetCompanyDataByRegonAsync(string regonId);

    }
}