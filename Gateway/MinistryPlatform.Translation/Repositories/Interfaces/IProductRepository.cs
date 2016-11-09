using System.Collections.Generic;
using MinistryPlatform.Translation.Models.Product;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IProductRepository
    {
        MpProduct GetProduct(int productId);
        MpProduct GetProductForEvent(int eventId);
        MpProductOptionPrice GetProductOptionPrice(int productOptionPriceId);
        List<MpProductOptionPrice> GetProductOptionPricesForProduct(int productId);
    }
}
