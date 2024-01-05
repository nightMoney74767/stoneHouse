using J85452___CO5227_Restaurant_Project.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace J85452___CO5227_Restaurant_Project.Pages
{
    internal class SigninManager<T>
    {
        internal Task SignOutAsync()
        {
            throw new NotImplementedException();
        }

        public static implicit operator SigninManager<T>(SignInManager<AppUserClass> v)
        {
            throw new NotImplementedException();
        }
    }
}