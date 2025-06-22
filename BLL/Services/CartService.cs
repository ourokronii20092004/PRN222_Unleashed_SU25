using BLL.Interfaces;
using DAL.Models;
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
        public IAccountRepository _accountRepo;
        //public IVariontionRepository _variationRepo;
        public CartService(ICartRepository cartRepo, IAccountRepository accountRepository) 
        {
            _accountRepo = accountRepository;
            // _variationRepo = variationRepository;
            _cartRepo = cartRepo;
        }

        public async Task CreateCartAsync(Cart cart)
        {
            // GetCurrentUserId
            // GetVariationId
            await _cartRepo.AddAsync(cart);
        }

        public async Task DeleteCartAsync((Guid, int) id)
        {
            var cart = await _cartRepo.GetByIdAsync(id);
            await _cartRepo.Delete(cart);
        }

        public Task<IEnumerable<Cart>> GetAllAsync()
        {
            return _cartRepo.GetAllAsync();
        }

        public Task<Cart> GetCartByIdAsync((Guid, int) id)
        {
            return _cartRepo.GetByIdAsync(id);
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            (Guid,int) id = new(cart.UserId,cart.VariationId);
            var existingCart = await _cartRepo.GetByIdAsync(id);
            existingCart.CartQuantity = cart.CartQuantity;
            await _cartRepo.Update(existingCart);
        }
    }
}
