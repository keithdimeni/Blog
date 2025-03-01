﻿using Blog.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Blog.Repositories;

public class UserRepository : Repository<UserRepository>
{
    private readonly SqlConnection _connection;


    public UserRepository(SqlConnection connection) : base(connection)
    {
        _connection = connection;
    }

    public List<User> GetWithRoles()
    {
        var querySQL = @"select [User].*, [Role].* from [User]
                        left join UserRole on UserRole.UserId = [User].Id
                        left join [Role] on UserRole.RoleId = [Role].Id";

        var users = new List<User>();
        var items = _connection.Query<User, Role, User>(querySQL, (user, role) =>
        {
            var usr = users.FirstOrDefault(x => x.Id == user.Id);
            if (usr == null)
            {
                usr = user;
                if(role != null)
                    usr.Roles.Add(role);
                users.Add(usr);
            } else
            {
                usr.Roles.Add(role);
            }
            return user;
        }, splitOn: "Id");

        return users;
    }
}
