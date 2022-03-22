using ProductAPI2.Context;
using ProductAPI2.Models;
using ProductAPI2.Repository.Repository;
using System.Collections.Generic;
using System.Linq;

namespace ProductAPI2.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext context;

        public ProductRepository(ApplicationDBContext context)
        {
            this.context = context;
        }
        public bool CreateProduct(Product product)
        {
            context.Product.Add(product);
            return Save();
        }

        public bool DeleteProduct(Product product)
        {
            context.Product.Remove(product);
            return Save();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return context.Product.ToList();
        }

        public Product GetProduct(int id)
        {
            return context.Product.FirstOrDefault(x=>x.Id.Equals(id));
        }

        public bool ProductExists(string name)
        {
            return context.Product.Any(x => x.Name.ToLower().Trim().Equals(name.ToLower().Trim()));
        }

        public bool ProductExists(int id)
        {
            return context.Product.Any(x => x.Id.Equals(id));
        }

        public bool Save()
        {
            return context.SaveChanges() > 0 ? true : false;
        }

        public bool UpdateProduct(Product product)
        {
            context.Product.Update(product);
            return Save();
        }
    }
}
