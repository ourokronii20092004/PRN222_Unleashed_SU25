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
        public ICartRepository _cartRepo;
        public IUserRepository _userRepo;
        public IMapper _mapper;
        //public IVariationRepository _variationRepo;
        public CartService(ICartRepository cartRepo, IUserRepository userRepository, IMapper mapper) //,IUserRepository accountRepository, IVariationRepository variationRepo)
        {
            _userRepo = userRepository;
            _cartRepo = cartRepo;
            _mapper = mapper;
            //_variationRepo = variationRepo;
        }

        //public async Task AddToCartAsync(AddToCartDTO addToCartDTO)
        //{
        //    var userId = await GetUserIdByUserNameAsync(addToCartDTO.UserUsername);
        //    var carts = await _cartRepo.GetCartByUserIdAsync(userId);
        //    var result = carts.Select(c => new Cart
        //    {
        //        UserId = userId.Value,
        //        VariationId = c.VariationId,
        //        CartQuantity = c.CartQuantity
        //    }).ToList();

        //    await _cartRepo.AddToCartAsync(result);
        //}

        ////public async Task CreateCartAsync(Cart cart)
        ////{
        ////    // GetCurrentUserName
        ////    // GetVariationId
        ////    await _cartRepo.AddAsync(cart);
        ////}

        ////public async Task DeleteCartAsync((Guid, int) id)
        ////{
        ////    var cart = await _cartRepo.GetByIdAsync(id);
        ////    await _cartRepo.Delete(cart);
        ////}

        ////public async Task<IEnumerable<Cart>> GetAllAsync()
        ////{
        ////    return await _cartRepo.GetAllAsync();
        ////}

        ////public async Task<Cart> GetCartByIdAsync((Guid, int) id)
        ////{
        ////    return await _cartRepo.GetByIdAsync(id);
        ////}

        //public Task<List<CartDTO>> GetCartItemsAsync(Guid userId)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Guid?> GetUserIdByUserNameAsync(string userName)
        {
            var userId = await _cartRepo.GetUserIdByUserNameAsync(userName);
            if (userId == null) throw new Exception("Không tìm thấy user");
            return userId;
        }

        //public Task RemoveCartItemAsync(Guid userId, int variationId)
        //{
        //    throw new NotImplementedException();
        //}

        ////public async Task UpdateCartAsync(Cart cart)
        ////{
        ////    (Guid,int) id = new(cart.UserId,cart.VariationId);
        ////    var existingCart = await _cartRepo.GetByIdAsync(id);
        ////    existingCart.CartQuantity = cart.CartQuantity;
        ////    await _cartRepo.Update(existingCart);
        ////}
        ///
        public async Task<List<CartDTO>> GetCartItemsAsync(Guid userId)
        {
            var carts = await _cartRepo.GetCartByUserIdAsync(userId);

            var result = carts.Select(c => new CartDTO
            {
                UserId = c.UserId,
                VariationId = c.VariationId,
                Quantity = c.CartQuantity,
                ProductName = c.Variation.Product?.ProductName ?? "",
                ImageUrl = c.Variation.VariationImage,
                ColorName = c.Variation.Color?.ColorName ?? "",
                SizeName = c.Variation.Size?.SizeName ?? "",
                Price = c.Variation.VariationPrice
            }).ToList();

            return result;
        }

        public async Task AddToCartAsync(AddToCartDTO dto)
        {
            var existingCart = await _cartRepo.GetCartItemAsync(dto.UserId, dto.VariationId);

            if (existingCart != null)
            {
                existingCart.CartQuantity = (existingCart.CartQuantity ?? 0) + dto.Quantity;
                await _cartRepo.UpdateCartItemAsync(existingCart);
            }
            else
            {
                var newCart = new Cart
                {
                    UserId = dto.UserId,
                    VariationId = dto.VariationId,
                    CartQuantity = dto.Quantity
                };
                await _cartRepo.AddToCartAsync(newCart);
            }
        }

        public async Task RemoveCartItemAsync(Guid userId, int variationId)
        {
            await _cartRepo.RemoveCartItemAsync(userId, variationId);
        }

    }
}
