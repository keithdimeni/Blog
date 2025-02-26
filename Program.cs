using Blog.Models;
using Blog.Repositories;
using Microsoft.Data.SqlClient;

namespace Blog;

public static class Program
{
    const string CONNECTION_STRING = @"Server=localhost,1433;Database=Blog;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=True";
    public static void Main()
    {
        var connection = new SqlConnection(CONNECTION_STRING);
        connection.Open();

        //ReadUsers(connection);
        ReadUsersWithRoles(connection);
        //CreateUsers(connection);
        //ReadRoles(connection);
        connection.Close();
    }

    

    public static void ReadUsers(SqlConnection connection)
    {
        var repository = new Repository<User>(connection);
        var users = repository.Get();

        foreach (var user in users)
        {
            Console.WriteLine(user.Name);
            foreach (var role in user.Roles)
            {
                Console.WriteLine(role.Id);
                Console.WriteLine(role.Name);
                Console.WriteLine(role.Slug);
                Console.WriteLine();

            }
        }
    }
    public static void ReadUsersWithRoles(SqlConnection connection)
    {
        var repository = new UserRepository(connection);
        var users = repository.GetWithRoles();

        foreach (var user in users)
        {
            Console.WriteLine(user.Name);
            foreach (var role in user.Roles)
            {
                Console.WriteLine(role.Name);
                Console.WriteLine();
            }
        }
    }

    public static void CreateUsers(SqlConnection connection)
    {
        var user = new User()
        {
            Name = "Gustavo Dimeni",
            Email = "gustavo@email.com",
            Bio = "Biografia",
            PasswordHash = "Hash",
            Image = "Image",
            Slug = "gustavo-dimeni",
        };

        var repository = new Repository<User>(connection);
        repository.Create(user);
    }

    public static void ReadRoles(SqlConnection connection)
    {
        var repository = new Repository<Role>(connection);
        var roles = repository.Get();
        foreach (var role in roles)
        {
            repository.Delete(role);
            Console.WriteLine($"{role.Name} - {role.Slug}");
        }
    }

}