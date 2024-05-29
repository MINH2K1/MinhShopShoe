using AutoMapper;
using Moq;
using ShopShoe.Application.AutoMapper;
using ShopShoe.Application.Implement;
using ShopShoe.Application.ViewModel.Query;
using ShopShoe.Domain.Entities;
using ShopShoe.Infastruction.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShopShoe.Infastruction.Repository.Interface.IRepository;

namespace ShopShoe.Application.UnitTest
{

    public class ProductServiceUnitTest
    {
        private readonly Mock<IRepository<Product, int>> _mockProductRepository;
        private readonly Mock<IRepository<Tag, string>> _mockTagRepository;
        private readonly Mock<IRepository<ProductQuantity, int>> _mockProductQuantityRepository;
        private readonly Mock<IRepository<ProductImage, int>> _mockProductImageRepository;
        private readonly Mock<IRepository<WholePrice, int>> _mockWhoPriceRepository;
        private readonly Mock<IRepository<ProductTag, int>> _mockProductTagRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public ProductServiceUnitTest() {
            _mockProductRepository = new Mock<IRepository<Product, int>>();
            _mockTagRepository = new Mock<IRepository<Tag, string>>();
            _mockProductQuantityRepository = new Mock<IRepository<ProductQuantity, int>>();
            _mockProductImageRepository = new Mock<IRepository<ProductImage, int>>();
            _mockWhoPriceRepository = new Mock<IRepository<WholePrice, int>>();
            _mockProductTagRepository=new Mock<IRepository<ProductTag, int>>();
            _mockUnitOfWork= new Mock<IUnitOfWork> ();
            }
      
        [Fact]
        public void Add_Invalid_OneParagram()
        {
            var _mockMapper = new MapperConfiguration(
                cfg => cfg.AddProfile(new AutoMapperProfile()));
            var mapper = _mockMapper.CreateMapper();


            var product = new ProductService(
                _mockProductRepository.Object,
                _mockTagRepository.Object,
                _mockProductQuantityRepository.Object,
                _mockProductImageRepository.Object,
                _mockWhoPriceRepository.Object,
                  mapper,
                _mockUnitOfWork.Object,
                _mockProductTagRepository.Object
                );

            var result = product.Add(new ProductViewModel()
            {
                Id = 0,
                Name = "Snaker 349",
                Tags = "daythethao daynam",
                OriginalPrice = 2000,
            });
            Assert.NotNull(mapper);
        }


        [Fact]
        public void GetAllProduct_NoParagram_Success()
        {
            var _mockMapper = new MapperConfiguration(
                  cfg => cfg.AddProfile(new AutoMapperProfile()));

            var mapper = _mockMapper.CreateMapper();
            var product = new ProductService(
                   _mockProductRepository.Object,
                   _mockTagRepository.Object,
                   _mockProductQuantityRepository.Object,
                   _mockProductImageRepository.Object,
                   _mockWhoPriceRepository.Object,
                     mapper,
                   _mockUnitOfWork.Object,
                   _mockProductTagRepository.Object
                   );
            var result = product.GetAll();
            Assert.NotNull(result);
        }
    }
}
