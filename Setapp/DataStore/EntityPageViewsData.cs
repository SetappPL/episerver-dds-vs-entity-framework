using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Setapp.DataStore
{
    public class EntityPageViewsData
    {
        [Key]
        public int Id { get; set; }

        [Index("IDX_PageIdKey")]
        public int PageId { get; set; }

        public int ViewsAmount { get; set; }
    }

}