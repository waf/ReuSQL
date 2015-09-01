using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ReuSQL
{
    
    class Program
    {
        static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        static void Main(string[] args)
        {
            var users = GetPublicProjectsForRole("Admin");
            Console.WriteLine(String.Join(",", users.Select(u => u.Name)));
            Console.ReadKey();
        }

        static IEnumerable<Project> GetPublicProjectsForRole(string role)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                return connection.QueryFromFile<Project>("SQL/ProjectsByUserRole", new
                {
                    Role = role,
                    IsSuperSecret = false
                });
            }
        }
    }

    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public bool IsSuperSecret { get; set; }
    }
}
