using BTCPayServer.Models;
using NBitpayClient;
using System.Collections.Generic;

namespace BTCPayServer.Plugins.B2PCentral.Models
{
    public class B2PResult: BasePagingViewModel
    {
        public B2PResult()
        { 
            Rate = 1;
        }

        public decimal Rate { get; set; }
        public string ErrorMsg { get; set; }

        public List<B2POffer> Offers { get; set; }

        public override int CurrentPageCount => Offers.Count;

    }
}
