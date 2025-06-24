using BLL.Services.Interfaces;
using DAL.DTOs;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProductVariationService : IProductVariationService
    {
        private readonly IVariationRepository _variationRepository;
        private readonly IColorRepository _colorRepository; 
        private readonly ISizeRepository _sizeRepository;    

        // Constructor to inject the repository
        public ProductVariationService(IVariationRepository variationRepository)
        {
            _variationRepository = variationRepository;
        }

        // Delete variation by ID (using repository)
        public async Task<bool> DeleteByIdAsync(int id)
        {
            var variation = await _variationRepository.GetByIdAsync(id); // Use the repo to get the variation by ID
            if (variation == null)
            {
                return false; // Variation not found
            }

            await _variationRepository.DeleteAsync(id); // Use the repo to delete
            return true;
        }

        // Find variation by ID (using repository)
        public async Task<Variation?> FindByIdAsync(int id)
        {
            return await _variationRepository.GetByIdAsync(id); // Use the repo to get the variation by ID
        }

        // Update product variation (using repository)
        public async Task<Variation> UpdateProductVariationAsync(int variationId, ProductDTO.ProductVariationDTO variationDTO)
        {
            // First, get the existing variation by ID
            var existingVariation = await _variationRepository.GetByIdAsync(variationId);
            if (existingVariation == null)
            {
                throw new Exception("Variation not found.");
            }

            // Map the Color and Size based on the provided DTO (retrieve Color and Size entities from the repositories)

            if (variationDTO.ColorId != null)
            {
                // Assuming you have a ColorRepository and it has a method to get the Color by name or ID
                var color = await _colorRepository.GetByIdAsync(variationDTO.ColorId.Value); // If Color is an object with an Id
                if (color != null)
                {
                    existingVariation.Color = color; // Set the Color to the existing variation
                }
                else
                {
                    throw new Exception("Color not found.");
                }
            }

            if (variationDTO.SizeId != null)
            {
                // Assuming you have a SizeRepository and it has a method to get the Size by name or ID
                var size = await _sizeRepository.GetByIdAsync(variationDTO.SizeId.Value); // If Size is an object with an Id
                if (size != null)
                {
                    existingVariation.Size = size; // Set the Size to the existing variation
                }
                else
                {
                    throw new Exception("Size not found.");
                }
            }

            // Map other properties from the DTO as needed...
            // Example: existingVariation.Price = variationDTO.Price;

            // Update variation via the repository
            await _variationRepository.UpdateAsync(existingVariation); // Use the repo to update
            await _variationRepository.SaveChangesAsync(); // Ensure changes are saved

            return existingVariation; // Return the updated variation
        }

    }
}
