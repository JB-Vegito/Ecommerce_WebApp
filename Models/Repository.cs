using System;
namespace ecommerce_web.Models{
    public class Repository{
        private static List<Users> allUsers = new List<Users>{};
        public static IEnumerable<Users> AllUsers{
            get{
                return allUsers;
            }
        }

        public static void Register(Users user){
            allUsers.Add(user);
        }

        public static void Display(){

        }
    }
}