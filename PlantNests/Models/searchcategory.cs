namespace PlantNests.Models
{
    public class searchcategory
    {
        public IQueryable<Category> categories {get; set;}
        public string term { get; set; }

    }
}
