using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface ISetting
    {

        void AddSetting(Setting setting);

        void EditSettingAsync(Setting settings);
        Task<Setting> GetSetting(int SettingId);
        int GetSliderHome1(int SettingId);

        int GetSliderHome2(int SettingID);

        int GetSliderHome3(int SettingID);

        void SetSliderHome1(int SettingID, int SlidersHome1);
        void SetSliderHome2(int SettingID, int SlidersHome2);
        void SetSliderHome3(int SettingID, int SlidersHome3);


    }
}
