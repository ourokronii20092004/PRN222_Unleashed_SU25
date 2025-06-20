﻿using BLL.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        public AccountService(IAccountRepository accountRepository, IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
        }

        public async Task<bool> AddEmployeeAsync(User user)
        {
            try
            {
                user.IsUserEnabled = true;
                user.Role = await _roleRepository.GetByIdAsync(2);
                user.UserCreatedAt = DateTime.UtcNow;
                user.UserUpdatedAt = DateTime.UtcNow;
                await _accountRepository.AddAsync(user);
                return true;
            }
            catch (DBConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> AddCustomerAsync(User user)
        {
            try
            {
                user.IsUserEnabled = true;
                user.Role = await _roleRepository.GetByIdAsync(2);
                user.UserCreatedAt = DateTime.UtcNow;
                user.UserUpdatedAt = DateTime.UtcNow;
                await _accountRepository.AddAsync(user);
                return true;
            }
            catch (DBConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            try
            {
                user.IsUserEnabled = false;
                user.UserUpdatedAt = DateTime.UtcNow;
                await _accountRepository.Update(user);
                return true;
            }
            catch (DBConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> EditUserAsync(User user)
        {
            try
            {
                var _user = await _accountRepository.GetByUsernameAsync(user.UserUsername);
                if (_user != null)
                {
                    _user.UserFullname = user.UserFullname;
                    _user.IsUserEnabled = user.IsUserEnabled;
                    _user.UserEmail = user.UserEmail;
                    _user.UserBirthdate = user.UserBirthdate;
                    _user.UserPhone = user.UserPhone;
                    _user.UserAddress = user.UserAddress;
                    _user.UserImage = user.UserImage;
                    user.UserUpdatedAt = DateTime.UtcNow;
                    await _accountRepository.Update(_user);
                   return true; 
                }
                return false;
            }
            catch (DbUpdateConcurrencyException)
            {             
                    return false;             
            }
        }

        public async Task<IEnumerable<User>> GetAccountsAsync()
        {
           return await _accountRepository.GetAllAsync();
        }



        public async Task<bool> ValidationUserAsync(string username)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(username);
            return await _accountRepository.GetByUsernameAsync(username) != null;
        }

        public Task<User> GetUserByUsernameAsync(string username)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(username);
            return _accountRepository.GetByUsernameAsync(username);
        }
    }
}
