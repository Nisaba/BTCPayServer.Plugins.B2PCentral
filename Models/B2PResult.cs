using System.Collections.Generic;

namespace BTCPayServer.Plugins.B2PCentral.Models
{
    public struct B2PResult
    {
        public decimal Rate { get; set; }
        public string ErrorMsg { get; set; }

        public List<B2POffer> Offers { get; set; }
    }
}
