using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Users;
public static class UserErrors
{
    public static readonly Error InvalidRegister = new(
        "User.Register",
        "Something went wrong while trying to register new user");

    public static readonly Error InvalidRole = new(
        "User.Role",
        "Sorry this role doesn't exist");

    public static readonly Error UserNotExist = new(
        "User.NotExist",
        "Sorry user does not exist with this givens");

    public static readonly Error EmailExists = new(
        "User.EmailDuplicate",
        "Sorry this email is already exists");
}
