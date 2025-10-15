namespace ModularMonolithDDD.Modules.UserAccess.Application.Authentication.Authenticate
{
    internal class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
    {
        public AuthenticateCommandValidator()
        {
            this.RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Username or email is required.")
            .Length(3, 50).WithMessage("Enter a valid username (3-50 characters) or email (max 254 characters).")
            .Must(BeValidUsernameOrEmail).WithMessage("Enter a valid username or email.")
            .Must(x => x == x.Trim()).WithMessage("Username may not contain leading or trailing spaces.");
            
            this.RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .MaximumLength(128).WithMessage("Password must not exceed 128 characters.")
            .Must(x => x == x?.Trim()).WithMessage("Password may not contain leading or trailing spaces.")
            //.Must(BeStrongPassword).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")
            ;
            
        }

        private bool BeValidUsernameOrEmail(string login)
        {
            if (string.IsNullOrEmpty(login)) return false;
            
            // Username pattern: 3-50 chars, letters, numbers, dot, dash, underscore
            var usernamePattern = @"^[A-Za-z0-9._-]{3,50}$";
            
            // Email pattern (basic)
            var emailPattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
            
            return Regex.IsMatch(login, usernamePattern) || 
                (Regex.IsMatch(login, emailPattern) && login.Length <= 254);
        }

        private bool BeStrongPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            
            return password.Any(char.IsUpper) &&           // At least 1 uppercase letter
                password.Any(char.IsLower) &&           // At least 1 lowercase letter 
                password.Any(char.IsDigit) &&           // At least 1 digit
                password.Any(c => "!@#$%^&*()_+-=[]{}|;:,.<>?".Contains(c)); // At least 1 special character
        }
    }
}
