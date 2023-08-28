using System;
using System.Collections.Generic;
using System.Text;
using Transaction.Framework.Types;

namespace Transaction.Framework.DTO
{
    public class AccountSummaryDTO
    {
        public int AccountNumber { get; set; }

        public decimal OriginalAmount { get; private set; }
        public string OriginalNameCurrency { get; private set; }
        public decimal ConvertAmount { get; set; }
        public string ConversionCurrency { get; set; }
    }
}
