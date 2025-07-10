using BLL.Services.Interfaces;
using DAL.DTOs.CartDTOs;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CartService : ICartService 
    {
        public ICartRepository _cartRepo;
        
        //public IUserRepository _userRepo;
        //public IVariationRepository _variationRepo;
        public CartService(ICartRepository cartRepo) //,IUserRepository accountRepository, IVariationRepository variationRepo)
        {
            //_userRepo = accountRepository;
            _cartRepo = cartRepo;
            //_variationRepo = variationRepo;
        }

        public async Task AddToCartAsync(AddToCartDTO addToCartDTO)
        {
            var carts = await _cartRepo.GetCartByUserIdAsync(addToCartDTO.UserId);

            var result = carts.Select(c => new Cart
            {
                UserId = c.UserId,
                VariationId = c.VariationId,
                CartQuantity = c.CartQuantity
            }).ToList();

            await _cartRepo.AddToCartAsync(result);
        }

        //public async Task CreateCartAsync(Cart cart)
        //{
        //    // GetCurrentUserName
        //    // GetVariationId
        //    await _cartRepo.AddAsync(cart);
        //}

        //public async Task DeleteCartAsync((Guid, int) id)
        //{
        //    var cart = await _cartRepo.GetByIdAsync(id);
        //    await _cartRepo.Delete(cart);
        //}

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _cartRepo.GetAllAsync();
        }

        public async Task<Cart> GetCartByIdAsync((Guid, int) id)
        {
            return await _cartRepo.GetByIdAsync(id);
        }

        public Task<List<CartDTO>> GetCartItemsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveCartItemAsync(Guid userId, int variationId)
        {
            throw new NotImplementedException();
        }

        //public async Task UpdateCartAsync(Cart cart)
        //{
        //    (Guid,int) id = new(cart.UserId,cart.VariationId);
        //    var existingCart = await _cartRepo.GetByIdAsync(id);
        //    existingCart.CartQuantity = cart.CartQuantity;
        //    await _cartRepo.Update(existingCart);
        //}
    }
}
