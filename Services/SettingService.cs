using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using partner_aluro.Data;
using partner_aluro.Migrations;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System;

namespace partner_aluro.Services
{
    public class SettingService : ISetting
    {
        ApplicationDbContext _context;

        public SettingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddSetting(Setting setting)
        {
            _context.Setting.Add(setting);
            _context.SaveChanges();

        }

        public async void EditSettingAsync(Setting settings) //Update
        {
            //Setting seting = await _context.Setting.Where(x => x.SettingID == settings.SettingID).FirstOrDefaultAsync();
            //seting = settings;
            _context.Update(settings);
            await _context.SaveChangesAsync();

        }

        public async Task<Setting> GetSetting(int SettingId)
        {
            Setting seting = await _context.Setting.Where(x => x.SettingID == SettingId).FirstOrDefaultAsync();

            return seting;
        }

        public int GetSliderHome1(int SettingID)
        {
            int SliderHome1 = (int)_context.Setting.Where(x => x.SettingID == SettingID).FirstOrDefault().SliderHome1;
            return SliderHome1;
        }

        public int GetSliderHome2(int SettingID)
        {
            int SliderHome2 = (int)_context.Setting.Where(x => x.SettingID == SettingID).FirstOrDefault().SliderHome2;
            return SliderHome2;
        }

        public int GetSliderHome3(int SettingID)
        {
            int SliderHome3 = (int)_context.Setting.Where(x => x.SettingID == SettingID).FirstOrDefault().SliderHome3;
            return SliderHome3;
        }

        async void ISetting.SetSliderHome1(int SettingID, int SlidersHome1)
        {
            Setting seting = await _context.Setting.Where(x => x.SettingID == SettingID).FirstOrDefaultAsync();
            seting.SliderHome1 = SlidersHome1;
            _context.Setting.Update(seting);
            await _context.SaveChangesAsync();
        }
        async void ISetting.SetSliderHome2(int SettingID, int SlidersHome2)
        {
            Setting seting = await _context.Setting.Where(x => x.SettingID == SettingID).FirstOrDefaultAsync();
            seting.SliderHome2 = SlidersHome2;
            _context.Setting.Update(seting);
            await _context.SaveChangesAsync();
        }
        async void ISetting.SetSliderHome3(int SettingID, int SlidersHome3)
        {
            Setting seting = await _context.Setting.Where(x => x.SettingID == SettingID).FirstOrDefaultAsync();
            seting.SliderHome3 = SlidersHome3;
            _context.Setting.Update(seting);
            await _context.SaveChangesAsync();
        }

    }
}
