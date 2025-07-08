using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(string? brand, string? type, string? sort) : base(x => 
        (string.IsNullOrEmpty(brand) || x.Brand == brand) && 
        (string.IsNullOrEmpty(type) || x.Type == type))
    {
        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort.ToLower())
            {
                case "priceasc":
                    AddOrderBy(p => p.Price);
                    break;
                case "pricedesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                case "nameasc":
                    AddOrderBy(p => p.Name);
                    break;
                case "namedesc":
                    AddOrderByDescending(p => p.Name);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
        else
        {
            AddOrderBy(p => p.Name);
        }
    }
}
