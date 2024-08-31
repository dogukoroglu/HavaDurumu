namespace HavaDurumu.Models
{
    public class ApiUsers
    {
        // veritabanıymış gibi kullanacağımız liste
        public static List<ApiUser> Kullanicilar = new()
        {
            new ApiUser {Id = 1,UserName="dogu",Password="123456",Role="Yönetici"},
            new ApiUser {Id = 2,UserName="bulent",Password="123456",Role="Standart Kullanıcı"},
            new ApiUser {Id = 2,UserName="yekta",Password="123456",Role="Misafir"},
        };
    }
}
