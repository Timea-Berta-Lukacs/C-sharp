using TAS_PROIECT.Interfete;

namespace TAS_PROIECT.Automat
{
    public class CountryPriceStub : ICountryPrice
    {
        private readonly decimal rataConversie;
        public CountryPriceStub(decimal rata)
        {
            this.rataConversie = rata;
        }
        public decimal Convert(decimal price)
        {
            decimal convertedPrice = price / rataConversie;
            return Math.Round(convertedPrice, 2, MidpointRounding.AwayFromZero);
        }
    }
}
