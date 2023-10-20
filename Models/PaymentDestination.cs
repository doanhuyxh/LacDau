﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LacDau.Models
{
    public class PaymentDestination
    {
        [Key]
        public string Id { get; set; }
        public string Logo { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public int SortIndex { get; set; }
        public string ParentId { get; set; }
        public bool IsActive { get; set; }

        
    }
}
