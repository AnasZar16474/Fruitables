﻿using Fruitables.DAL.Models;

namespace Fruitables.PL.Views.ViewModel
{
    public class ProductsVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImagName { get; set; } = null!;
        public decimal Price { get; set; }
        public ProductType Type { get; set; }
    }
}