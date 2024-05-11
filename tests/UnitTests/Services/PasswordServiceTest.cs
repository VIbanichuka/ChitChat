using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Application.Implementations;
using ChitChat.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Routing;

namespace UnitTests.Services
{
    public class PasswordServiceTest
    {
        private readonly IPasswordService _passwordService;
        public PasswordServiceTest()
        {
            _passwordService = new PasswordService();
        }

        [Fact]
        public void CreatePasswordHash_ValidPassword_ReturnsHashAndSalt()
        {
            string password = "$$testPassword123";
            byte[] expectedPasswordHash;
            byte[] expectedPasswordSalt;

            _passwordService.CreatePasswordHash(password, out expectedPasswordHash, out expectedPasswordSalt);
            Assert.NotNull(expectedPasswordSalt);
            Assert.NotNull(expectedPasswordHash);
        }

        [Fact]
        public void CreatePasswordHash_EmptyPassword_ThrowsException()
        {
            string password = string.Empty;
            byte[] expectedPasswordHash;
            byte[] expectedPasswordSalt;

            Assert.Throws<ArgumentNullException>(() => _passwordService.CreatePasswordHash(password, out expectedPasswordHash, out expectedPasswordSalt));
        }

        [Fact]
        public void CreatePasswordHash_NullPassword_ThrowsException()
        {
            string password = null!;
            byte[] expectedPasswordHash;
            byte[] expectedPasswordSalt;

            Assert.Throws<ArgumentNullException>(() => _passwordService.CreatePasswordHash(password, out expectedPasswordHash, out expectedPasswordSalt));
        }

        [Fact]
        public void VerifyPassword_InvalidPassword_ShouldReturnFalse()
        {
            var password = "Password123";
            var passwordToBeVerified = "IncorrectPassword456";

            _passwordService.CreatePasswordHash(password, out var storedPasswordHash, out var storedPasswordSalt);
            var isPasswordValid = _passwordService.VerifyPasswordHash(passwordToBeVerified, storedPasswordHash, storedPasswordSalt);

            Assert.False(isPasswordValid);
        }

        [Fact]
        public void VerifyPassword_ValidPassword_ShouldReturnTrue()
        {
            var password = "Password123";
            var passwordToBeVerified = "Password123";

            _passwordService.CreatePasswordHash(password, out var storedPasswordHash, out var storedPasswordSalt);
            var isPasswordValid = _passwordService.VerifyPasswordHash(passwordToBeVerified, storedPasswordHash, storedPasswordSalt);

            Assert.True(isPasswordValid);
        }

        [Fact]
        public void VerifyPasswordHash_NullPassword_ThrowsException()
        {
            string password = null!;
            byte[] storedPasswordHash = new byte[32];
            byte[] storedPasswordSalt = new byte[32];

            Assert.Throws<ArgumentNullException>(() => _passwordService.VerifyPasswordHash(password, storedPasswordHash, storedPasswordSalt));
        }

        [Fact]
        public void VerifyPasswordHash_EmptyPassword_ThrowsException()
        {
            var password = string.Empty;
            byte[] storedPasswordHash = new byte[32];
            byte[] storedPasswordSalt = new byte[32];

            Assert.Throws<ArgumentNullException>(() => _passwordService.VerifyPasswordHash(password, storedPasswordHash, storedPasswordSalt));
        }

        [Fact]
        public void VerifyPasswordHash_MaximumLengthPassword_ShouldReturnFalse()
        {
            string password = new string('a', 30);
            byte[] storedPasswordHash = new byte[32];
            byte[] storedPasswordSalt = new byte[32];
            var result = _passwordService.VerifyPasswordHash(password, storedPasswordHash, storedPasswordSalt);
            Assert.False(result);
        }
    }
}
