﻿using System.ComponentModel.DataAnnotations;

namespace CategoryService.DTO
{
    public class CategoryReadDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
