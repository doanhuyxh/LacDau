namespace LacDau.Models.CategoryVM
{
    public class ListCategory
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Icon { get; set; }
        public List<SubCategory>? ListSubCategory { get; set; }
    }
}
