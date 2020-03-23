using System.ComponentModel.DataAnnotations;

namespace blazorblog.Data.Dto
{
    public partial class Settings
    {
        [Key]
        public int SettingId { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
    }
}