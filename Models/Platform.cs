using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PlatformService.Data;
using PlatformService.Model;

namespace PlatformService.Models
{
    public class Platform : IEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Publisher { get; set; }

        [Required]
        public string Cost { get; set; }
        
        /// <summary>
        /// Represents a purchased, valid license for the platform.
        /// </summary>
        public string LicenseKey { get; set; }

        /// <summary>
        /// This is the list of available commands for this platform.
        /// </summary>
        public ICollection<Command> Commands { get; set; } = new List<Command>();
    }
}