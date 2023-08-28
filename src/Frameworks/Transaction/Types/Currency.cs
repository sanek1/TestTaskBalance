namespace Transaction.Framework.Types
{
    using System.ComponentModel;

    public enum Currency
    {
        Unknown = 0,

        [Description("Pound sterling")]
        GBP = 1,

        [Description("Euro")]
        EUR = 2,

        [Description("RUB")]
        RUB = 3,

    }
}
