using System.ComponentModel.DataAnnotations;

namespace Soa.GatewaySample.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}