using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniListHelper.Models {
    [Table(nameof(MediaEntryModel))]
    public class MediaEntryModel {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string OtherNames { get; set; }
        public string Status { get; set; }
    }
}
