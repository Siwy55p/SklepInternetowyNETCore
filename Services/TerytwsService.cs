using ServiceReference2;

namespace partner_aluro.Services
{
    public class TerytwsService : ITerytWs1
    {
        private readonly string _serviceKey;
        private readonly bool _isProduction;

        public TerytwsService(string serviceKey, bool isProduction = true)
        {
            if (string.IsNullOrEmpty(serviceKey)) throw new ArgumentNullException("Brak klucza BIR");
            _serviceKey = serviceKey;
            _isProduction = isProduction;
        }

        public Task<AdresoBudynki[]> AdresBudynkowAsync(string woj, string pow, string gmi, string rodz, string symbolMsc, string SymUl)
        {
            throw new NotImplementedException();
        }

        public Task<AdresoBudynkiMieszkania[]> AdresBudynkowMieszkaniaAsync(string woj, string pow, string gmi, string rodz, string symbolMsc, string SymUl)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZbioryNOBC> AdresyBudynkowAsync(string woj, string pow, string gmi, string rodz, string formatDanych, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZbioryNOBC> AdresyBudynkowImieszkaniaAsync(string woj, string pow, string gmi, string rodz, string formatDanych, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZbioryNOBC> AdresyBudynkowZIdentyfikatoremAdresuAsync(string woj, string pow, string gmi, string rodz, string formatDanych, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZbioryNOBC> AdresyBudynkowZIdentyfikatoremBudynkuAsync(string woj, string pow, string gmi, string rodz, string formatDanych, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZbioryNOBC> AdresyBudynkowZLiczbaMieszkanAsync(string woj, string pow, string gmi, string rodz, string formatDanych, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<OdpowiedzTeryt> AktualizujPunktAdresowyEMUiAAsync(PunktAdresowy punktAdresowy)
        {
            throw new NotImplementedException();
        }

        public Task<OdpowiedzTeryt> AktualizujUliceEMUiAAsync(PlacUlica placUlica)
        {
            throw new NotImplementedException();
        }

        public Task<string> CiekawostkiSIMCAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> CiekawostkiTERCAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> CiekawostkiULICAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CzyZalogowanyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Licznosc> LicznoscJednostekAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ObiektyZZ[]> ObiektyZZAsync(string woj, string pow, string gmi, string rodz, string symbolMsc, string SymUl)
        {
            throw new NotImplementedException();
        }

        public Task<DateTime> PobierzDateAktualnegoKatNTSAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DateTime> PobierzDateAktualnegoKatSimcAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DateTime> PobierzDateAktualnegoKatTercAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DateTime> PobierzDateAktualnegoKatUlicAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GeoTeryt> PobierzGeoTerytPlikPelnyAsync(string rok, string kwartal, string kodTerytorialny)
        {
            throw new NotImplementedException();
        }

        public Task<GeoTeryt> PobierzGeoTerytPlikRoznicowyAsync(string rok, string kwartal, string kodTerytorialny)
        {
            throw new NotImplementedException();
        }

        public Task<JednostkaTerytorialna[]> PobierzGminyiPowDlaWojAsync(string Woj, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikKatalog> PobierzKatalogNTSAsync(DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikKatalog> PobierzKatalogSIMCAdrAsync(DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikKatalog> PobierzKatalogSIMCAsync(DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikKatalog> PobierzKatalogSIMCStatAsync(DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikKatalog> PobierzKatalogTERCAdrAsync(DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikKatalog> PobierzKatalogTERCAsync(DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikKatalog> PobierzKatalogULICAdrAsync(DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikKatalog> PobierzKatalogULICAsync(DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikKatalog> PobierzKatalogULICBezDzielnicAsync(DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikKatalog> PobierzKatalogWMRODZAsync(DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<JednostkaTerytorialna[]> PobierzListeGminAsync(string Woj, string Pow, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<JednostkaNomenklaturyNTS[]> PobierzListeGminPowiecieAsync(string Pow, string Podreg, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<Miejscowosc[]> PobierzListeMiejscowosciWGminieAsync(string Wojewodztwo, string Powiat, string Gmina, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<MiejscowoscPelna[]> PobierzListeMiejscowosciWGminieZSymbolemAsync(string Woj, string Pow, string Gmi, string Rodz, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<Miejscowosc[]> PobierzListeMiejscowosciWRodzajuGminyAsync(string symbolWoj, string symbolPow, string symbolGmi, string symbolRodz, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<JednostkaNomenklaturyNTS[]> PobierzListePodregionowAsync(string Woj, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<JednostkaTerytorialna[]> PobierzListePowiatowAsync(string Woj, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<JednostkaNomenklaturyNTS[]> PobierzListePowiatowWPodregionieAsync(string Podreg, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<JednostkaNomenklaturyNTS[]> PobierzListeRegionowAsync(DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> PobierzListeStanowSimcAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string[]> PobierzListeStanowTercAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string[]> PobierzListeStanowUlicAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UlicaDrzewo[]> PobierzListeUlicDlaMiejscowosciAsync(string woj, string pow, string gmi, string rodzaj, string msc, bool czyWersjaUrzedowa, bool czyWersjaAdresowa, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<JednostkaTerytorialna[]> PobierzListeWojewodztwAsync(DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<JednostkaNomenklaturyNTS[]> PobierzListeWojewodztwWRegionieAsync(string Reg, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> PobierzSlownikCechULICAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string[]> PobierzSlownikRodzajowJednostekAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RodzajMiejscowosci[]> PobierzSlownikRodzajowSIMCAsync(DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikKatalog> PobierzStaryKatalogULICAsync(DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZmiany> PobierzZmianyNTSAsync(DateTime stanod, DateTime stando)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZmiany> PobierzZmianySimcAdresowyAsync(DateTime stanod, DateTime stando)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZmiany> PobierzZmianySimcStatystycznyAsync(DateTime stanod, DateTime stando)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZmiany> PobierzZmianySimcUrzedowyAsync(DateTime stanod, DateTime stando)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZmiany> PobierzZmianyTercAdresowyAsync(DateTime stanod, DateTime stando)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZmiany> PobierzZmianyTercUrzedowyAsync(DateTime stanod, DateTime stando)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZmiany> PobierzZmianyUlicAdresowyAsync(DateTime stanod, DateTime stando)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZmiany> PobierzZmianyUlicBezDzielnicAsync(DateTime stanod, DateTime stando)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZmiany> PobierzZmianyUlicUrzedowyAsync(DateTime stanod, DateTime stando)
        {
            throw new NotImplementedException();
        }

        public Task<RLiczbaJednostkiTerc[]> RaportLiczbaJednostekTercAsync(string dataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<RMiejscowosciWiejskie[]> RaportLiczbaMiejscowosciWiejskichAsync(string dataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<RJednostkiTerc[]> RaportPorownanieTercNoweJednostkiAsync(string dataOd, string dataDo)
        {
            throw new NotImplementedException();
        }

        public Task<RJednostkiTerc[]> RaportPorownanieTercUsunieteJednostkiAsync(string dataOd, string dataDo)
        {
            throw new NotImplementedException();
        }

        public Task<RZmianyTerc[]> RaportPorownanieTercZmienioneNazwyAsync(string dataOd, string dataDo)
        {
            throw new NotImplementedException();
        }

        public Task<RZmianyTerc[]> RaportPorownanieTercZmienioneSymboleAsync(string dataOd, string dataDo)
        {
            throw new NotImplementedException();
        }

        public Task<RZmianyTerc[]> RaportPorownanieTercZmienioneSymboleINazwyAsync(string dataOd, string dataDo)
        {
            throw new NotImplementedException();
        }

        public Task<Statystki> TerytWLiczbachAsync(int wybierz)
        {
            throw new NotImplementedException();
        }

        public Task<ZweryfikowanyAdresBezUlic[]> WeryfikujAdresDlaMiejscowosciAdresowyAsync(string symbolMsc)
        {
            throw new NotImplementedException();
        }

        public Task<ZweryfikowanyAdresBezUlic[]> WeryfikujAdresDlaMiejscowosciAsync(string symbolMsc)
        {
            throw new NotImplementedException();
        }

        public Task<ZweryfikowanyAdres[]> WeryfikujAdresDlaUlicAdresowyAsync(string symbolMsc, string SymUl)
        {
            throw new NotImplementedException();
        }

        public Task<ZweryfikowanyAdres[]> WeryfikujAdresDlaUlicAsync(string symbolMsc, string SymUl)
        {
            throw new NotImplementedException();
        }

        public Task<ZweryfikowanyAdresBezUlic[]> WeryfikujAdresWmiejscowosciAdresowyAsync(string Wojewodztwo, string Powiat, string Gmina, string Miejscowosc, string Rodzaj)
        {
            throw new NotImplementedException();
        }

        public Task<ZweryfikowanyAdresBezUlic[]> WeryfikujAdresWmiejscowosciAsync(string Wojewodztwo, string Powiat, string Gmina, string Miejscowosc, string Rodzaj)
        {
            throw new NotImplementedException();
        }

        public Task<ZweryfikowanyAdres[]> WeryfikujNazwaAdresUlicAdresowyAsync(string nazwaWoj, string nazwaPow, string nazwaGmi, string nazwaMiejscowosc, string rodzajMiejsc, string nazwaUlicy)
        {
            throw new NotImplementedException();
        }

        public Task<ZweryfikowanyAdres[]> WeryfikujNazwaAdresUlicAsync(string Wojewodztwo, string Powiat, string Gmina, string Miejscowosc, string Rodzaj, string NazwaUlicy)
        {
            throw new NotImplementedException();
        }

        public Task<JednostkaPodzialuTerytorialnego[]> WyszukajJednostkeWRejestrzeAsync(string nazwa, identyfikatory[] identyfiks, string kategoria, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<JednostkaPodzialuTerytorialnego[]> WyszukajJednostkeWRejestrzeWebLSAsync(string nazwa, identyfikatory[] identyfiks, string kategoria, bool zawezenieRekordow, int odKtoregoRekordu, int iloscRekordow, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<int> WyszukajJednostkeWRejestrzeWebLSCountAsync(string nazwa, identyfikatory[] identyfiks, string kategoria, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<JednostkaPodzialuTerytorialnegoDoSortowania[]> WyszukajJednostkeWRejestrzeWebLSZSortowaniemAsync(string nazwa, identyfikatory[] identyfiks, string kategoria, bool zawezenieRekordow, int odKtoregoRekordu, int iloscRekordow, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<JednostkaPodzialuTerytorialnego[]> WyszukajJPTAsync(string nazwa)
        {
            throw new NotImplementedException();
        }

        public Task<Miejscowosc[]> WyszukajMiejscowoscAsync(string nazwaMiejscowosci, string identyfikatorMiejscowosci)
        {
            throw new NotImplementedException();
        }

        public Task<WyszukanaMiejscowosc[]> WyszukajMiejscowoscWebAsync(string nazwa, string rodzajMiejscowosci, string symbol, identyfikatory[] identyfiks, bool czyPelnaNazwa, int iloscRekordow, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<int> WyszukajMiejscowoscWebCountAsync(string nazwa, string rodzajMiejscowosci, string symbol, identyfikatory[] identyfiks, bool czyPelnaNazwa, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<int> WyszukajMiejscowoscWebCountLSAsync(string nazwa, string rodzajMiejscowosci, string symbol, identyfikatory[] identyfiks, bool czyPelnaNazwa, bool czyFragmentNazwy, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<WyszukanaMiejscowoscZPodstawowa[]> WyszukajMiejscowoscWebLSAsync(string nazwa, string rodzajMiejscowosci, string symbol, identyfikatory[] idents, bool czyPelnaNazwa, bool czyFragmentNazwy, bool zawezenieRekordow, int odKtoregoRekordu, int iloscRekordow, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<Miejscowosc[]> WyszukajMiejscowoscWJPTAsync(string nazwaWoj, string nazwaPow, string nazwaGmi, string nazwaMiejscowosci, string identyfikatorMiejscowosci)
        {
            throw new NotImplementedException();
        }

        public Task<WyszukanaMiejscowosc[]> WyszukajMiejscowoscWRejestrzeAsync(string nazwa, string rodzajMiejscowosci, string symbol, identyfikatory[] identyfiks, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<Ulica[]> WyszukajUliceAsync(string nazwaulicy, string cecha, string nazwamiejscowosci)
        {
           
            
            return WyszukajUliceAsync(nazwaulicy, cecha, nazwamiejscowosci);
            //logika 

            throw new NotImplementedException();
        }

        public Task<WyszukanaUlica[]> WyszukajUliceWebAsync(string nazwa, string cecha, string identyfikator, identyfikatory[] identyfiks, bool czyPelnaNazwa, int iloscRekordow, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<int> WyszukajUliceWebCountAsync(string nazwa, string cecha, string identyfikator, identyfikatory[] identyfiks, bool czyPelnaNazwa, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<WyszukanaUlicaZPodstawowa[]> WyszukajUliceWebLSAsync(string nazwa, string cecha, string identyfikator, identyfikatory[] identyfiks, bool czyPelnaNazwa, bool czyFragmentNazwy, bool zawezenieRekordow, int odKtoregoRekordu, int iloscRekordow, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<int> WyszukajUliceWebLSCountAsync(string nazwa, string cecha, string identyfikator, identyfikatory[] identyfiks, bool czyPelnaNazwa, bool czyFragmentNazwy, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<WyszukanaUlica[]> WyszukajUliceWRejestrzeAsync(string nazwa, string cecha, string identyfikator, identyfikatory[] identyfiks, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<PlikZbioryNOBC> ZbiorObiektowZZAsync(string woj, string pow, string gmi, string rodz, string formatDanych, DateTime DataStanu)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ZweryfikowanyAsync()
        {
            throw new NotImplementedException();
        }
    }
}
