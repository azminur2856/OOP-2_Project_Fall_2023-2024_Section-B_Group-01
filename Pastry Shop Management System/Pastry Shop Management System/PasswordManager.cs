namespace Pastry_Shop_Management_System
{
    internal class PasswordManager
    {
        // Function to hash a password before saving it to the database
        public string HashPassword(string password)
        {
            // Hash the password using BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        // Function to check if the provided password matches the hashed password from the database
        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            // Verify if the input password matches the hashed password
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }
}
