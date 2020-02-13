using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoService.WEB.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        [Required]
        public string ServiceName { get; set; }
        [Required]
        public int Price { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}