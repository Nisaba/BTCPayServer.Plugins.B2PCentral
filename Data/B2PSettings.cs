using B2P_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BTCPayServer.Plugins.B2PCentral.Data;

public class B2PSettings
{
    [Key]
    public string StoreId { get; set; }

    public string ApiKey { get; set; }

    public string ProvidersString { get; set; }

    [NotMapped]
    public List<ProvidersEnum> Providers
    {
        get
        {
            return ProvidersString.Split(",").ToList().Select(s => (ProvidersEnum)Enum.Parse(typeof(ProvidersEnum), s)).ToList();
        }
        set
        {
            ProvidersString = String.Join(",", value.ToArray());
        }
    }
}
