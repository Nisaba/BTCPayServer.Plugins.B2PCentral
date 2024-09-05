using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BTCPayServer.Client.Models;
using BTCPayServer.Plugins.B2PCentral.Data;
using Microsoft.EntityFrameworkCore;

namespace BTCPayServer.Plugins.B2PCentral.Services;

public class B2PCentralPluginService
{
    private readonly B2PCentralPluginDbContextFactory _pluginDbContextFactory;
    private readonly StoreData _storeData;

    public B2PCentralPluginService(B2PCentralPluginDbContextFactory pluginDbContextFactory, StoreData storeData)
    {
        _pluginDbContextFactory = pluginDbContextFactory;
        _storeData = storeData;
    }

    public async Task AddTestDataRecord()
    {
        await using var context = _pluginDbContextFactory.CreateContext();

        await context.B2PSettings.AddAsync(new B2PSettings { StoreId = "test-store-id", ApiKey = "my-api-key", ProvidersString= "1,2,3" });
        await context.SaveChangesAsync();
    }

    public async Task<B2PSettings> GetStoreSettings()
    {
        await using var context = _pluginDbContextFactory.CreateContext();
        
        return await context.B2PSettings.FirstOrDefaultAsync(a => a.StoreId == _storeData.Id);
    }
}

