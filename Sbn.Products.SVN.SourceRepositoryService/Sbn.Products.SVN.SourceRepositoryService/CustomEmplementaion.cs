using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Web
{
    public class CustomCredentialValidator : System.IdentityModel.Selectors.UserNamePasswordValidator
    {
        public CustomCredentialValidator()
            : base()
        {
        }
        public override void Validate(string userName, string password)
        {
            if (userName == "najva" && password == "najva@atBa1395")
                return;
            if (userName == "sadak" && password == "sadak@keFalat1395")
                return;
            throw new System.IdentityModel.Tokens.SecurityTokenException(
                      "Unknown Username or Password");
        }
    }

}