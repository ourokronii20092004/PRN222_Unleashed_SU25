using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.CartDTOs;
using DAL.DTOs.UserDTOs;
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
        private readonly ICartRepository _cartRepository;
        private readonly IVariationRepository _variationRepository;
        private readonly IUserRepository _userRepository;

        public CartService(ICartRepository cartRepository, IVariationRepository variationRepository, IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _variationRepository = variationRepository;
            _userRepository = userRepository;
        }

        public Task AddToCartAsync(string username, int variationId, int quantity)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, List<CartDTO>>> GetCartByUsernameAsync(string username)
        {
            var userId = await GetUserIdByUsername(username);
            var carts = await _cartRepository.GetAllByUserIdAsync(userId);
            var cartDtos = new List<CartDTO>();
            foreach (var cartItem in carts)
            {
                var variation = cartItem.Variation;
                cartDtos.Add(new CartDTO { Variation = variation,
                Quantity = cartItem.CartQuantity ?? 0});
            }
            return cartDtos.GroupBy(c => c.Variation.Product.ProductName).ToDictionary(g => g.Key, g => g.ToList());
        }

        public async Task<Guid> GetUserIdByUsername(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user.UserId;
        }

        public async Task RemoveAllFromCartAsync(string username)
        {
            var userId = await GetUserIdByUsername(username);
            await _cartRepository.RemoveAllAsync(userId);
        }

        public async Task RemoveFromCartAsync(string username, int variationId)
        {
            var userId = await GetUserIdByUsername(username);
            await _cartRepository.RemoveAsync(userId, variationId);
        }
    }
}
