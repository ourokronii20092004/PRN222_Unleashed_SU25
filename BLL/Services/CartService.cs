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
    public class CartService(ICartRepository cartRepository, IVariationRepository variationRepository, IUserRepository userRepository, IMapper mapper) : ICartService
    {
        private readonly ICartRepository _cartRepository = cartRepository;
        private readonly IVariationRepository _variationRepository = variationRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper =mapper;

        public async Task AddToCartAsync(string username, int variationId, int quantity)
        {
            var userId = await GetUserIdByUsername(username);
            var cart = new Cart
            {
                UserId = userId,
                VariationId = variationId,
                CartQuantity = quantity
            };

            await _cartRepository.AddOrUpdateAsync(cart);
        }
        public async Task UpdateQuantityAsync(string username, int variationId, int quantity)
        {
            var userId = await GetUserIdByUsername(username);
            await _cartRepository.UpdateQuantityAsync(userId, variationId, quantity);
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
            await _cartRepository.RemoveAsync(await GetUserIdByUsername(username), variationId);
        }
    }
}
