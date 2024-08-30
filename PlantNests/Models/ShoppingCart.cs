
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PlantNests.Models
{
    public class ShoppingCart
    {
        [Key]
        public int cartId { get; set; }
        public int productId { get; set; }
        public int Id { get; set; }
        public virtual Product Product { get; set;}
        public int price { get; set; }  
        public int qty { get; set; }    
        public int bill { get; set; }
        public string image { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime Lastmodifield { get; set; }

    }
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }

}
