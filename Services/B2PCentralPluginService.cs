using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BTCPayServer.Client.Models;
using BTCPayServer.Plugins.B2PCentral.Data;
using BTCPayServer.Services.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BTCPayServer.Plugins.B2PCentral.Services;

public class B2PCentralPluginService
{
    private readonly B2PCentralPluginDbContextFactory _pluginDbContextFactory;
    private readonly StoreRepository _storeRepository;
    private readonly ILogger _logger;
    private readonly B2PCentralPluginDbContext context;

    public B2PCentralPluginService(B2PCentralPluginDbContextFactory pluginDbContextFactory, StoreRepository storeRepository, ILogger<B2PCentralPluginService> logger)
    {
        _pluginDbContextFactory = pluginDbContextFactory;
        _storeRepository = storeRepository;
        _logger = logger;
        context = _pluginDbContextFactory.CreateContext();
    }

    public async Task<string> TestB2P(B2PSettings settings)
    {
        try
        {
            return "OK";
        }
        catch (Exception e)
        {
            _logger.LogError(e, "B2PCentral:TestB2P()");
            return e.Message;
        }
    }

    public async Task<B2PSettings> GetStoreSettings(string storeId)
    {
        try
        {
            var settings = await context.B2PSettings.FirstOrDefaultAsync(a => a.StoreId == storeId);
            if (settings == null)
            {
                settings = new B2PSettings { StoreId = storeId, ProvidersString = "0" };
            }
            return settings;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "B2PCentral:GetStoreSettings()");
            throw;
        }
    }

    public async Task UpdateSettings(B2PSettings settings)
    {
        try
        {
            var dbSettings = await context.B2PSettings.FirstOrDefaultAsync(a => a.StoreId == settings.StoreId);
            if (dbSettings == null)
            {
                context.B2PSettings.Add(settings);
            } else
            {
                dbSettings.ProvidersString = settings.ProvidersString;
                dbSettings.ApiKey = settings.ApiKey;
                context.B2PSettings.Update(dbSettings);
            }

            await context.SaveChangesAsync();
            return;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "B2PCentral:UpdateSettings()");
            throw;
        }
    }
}

