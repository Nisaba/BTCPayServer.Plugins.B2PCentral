using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BTCPayServer.Abstractions.Constants;
using BTCPayServer.Client;
using BTCPayServer.Plugins.B2PCentral.Data;
using BTCPayServer.Plugins.B2PCentral.Models;
using BTCPayServer.Plugins.B2PCentral.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BTCPayServer.Plugins.B2PCentral;

[Route("~/plugins/{storeId}/b2pcentral")]
[Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie)]
public class UIPluginController : Controller
{
    private readonly B2PCentralPluginService _PluginService;

    public UIPluginController(B2PCentralPluginService PluginService)
    {
        _PluginService = PluginService;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Policy= Policies.CanViewStoreSettings)]
    public async Task<IActionResult> Index(string storeId)
    {
        return View(await _PluginService.GetStoreSettings(storeId));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Policy = Policies.CanModifyStoreSettings)]
    public async Task<IActionResult> Index(B2PSettings model, string command)
    {
        if (ModelState.IsValid)
        {
            switch (command)
            {
                case "Save":
                    await _PluginService.UpdateSettings(model);
                    break;
                case "Test":
                    var sTest = await _PluginService.TestB2P(model);
                    if (sTest == "OK")
                    {
                        TempData[WellKnownTempData.SuccessMessage] = "Access to B2P Central API successful";
                    }
                    else
                    {
                        TempData[WellKnownTempData.ErrorMessage] = $"Access to B2P Central API failed: {sTest}";
                    }
                    break;
            }
        }
        return View("Index", model);
    }


    [HttpPost]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Policy = Policies.CanViewStoreSettings)]
    [Route("GetPartialB2PResult")]
    public async Task<IActionResult> GetPartialB2PResult([FromBody] B2PRequest req)
    {
        var model = new B2PResult { Rate = req.Rate };
        try
        {
            var ofrReq = new OffersRequest
            {
                Amount = (uint)req.Amount,
                CurrencyCode = req.CurrencyCode,
                IsBuy = false,
                Providers = req.Providers
            };
            model.Offers = await _PluginService.GetOffersListAsync(ofrReq, req.ApiKey);
        } catch (Exception ex)
        {
            model.ErrorMsg = ex.Message;
        }
        return PartialView("_B2PResults", model);
    }
}
