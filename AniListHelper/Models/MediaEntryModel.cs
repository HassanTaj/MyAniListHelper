using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniListHelper.Models {
    [Table(nameof(MediaEntryModel))]
    public class MediaEntryModel {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //call: query.media
        // Do not show add button when not in local list 
        public string Name { get; set; }
        public string Image { get; set; }
        public string OtherNames { get; set; }
        public string Status { get; set; }
        //public string Description { get; set; }
    }
}
