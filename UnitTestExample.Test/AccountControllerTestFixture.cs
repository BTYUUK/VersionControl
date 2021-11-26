
using Moq;
using NUnit.Framework;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnitTestExample.Abstractions;
using UnitTestExample.Controllers;
using UnitTestExample.Entities;

namespace UnitTestExample.Test
{
    class AccountControllerTestFixture
    {
        [
               Test,
               TestCase("abcd1234", false),
               TestCase("irf@uni-corvinus", false),
               TestCase("irf.uni-corvinus.hu", false),
               TestCase("irf@uni-corvinus.hu", true)
           ]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            var accountController = new AccountController();
            var actualResult = accountController.ValidateEmail(email);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [
            Test,
            TestCase("Abcdefgh",false),
            TestCase("ABCD1234",false),
            TestCase("abcd1234",false),
            TestCase("Ab1",false),
            TestCase("Abcd1234",true)
        ]
        public bool TestValidatePassword(string password)
        {
            
            var kisBetu = new Regex(@"[a-z]+");
            var nagyBetu = new Regex(@"[A-Z]+");
            var szam = new Regex(@"[0-9]+");
            var nyolc = new Regex(@".{8,}");
            if (kisBetu.IsMatch(password) && nagyBetu.IsMatch(password) && szam.IsMatch(password) && nyolc.IsMatch(password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [
            Test,
            TestCase("irf@uni-corvinus.hu", "Abcd1234"),
            TestCase("irf@uni-corvinus.hu", "Abcd1234567"),
        ]
        public void TestRegisterHappyPath(string email, string password)
        {
            var accountServiceMock = new Mock<IAccountManager>(MockBehavior.Strict);
            accountServiceMock
                .Setup(m => m.CreateAccount(It.IsAny<Account>()))
                .Returns<Account>(a => a);
            var accountController = new AccountController();
            accountController.AccountManager = accountServiceMock.Object;
            var actualResult = accountController.Register(email, password);
            Assert.AreEqual(email, actualResult.Email);
            Assert.AreEqual(password, actualResult.Password);
            Assert.AreNotEqual(Guid.Empty, actualResult.ID);
        }
        [
            Test,
            TestCase("irf@uni-corvinus", "Abcd1234"),
            TestCase("irf.uni-corvinus.hu", "Abcd1234"),
            TestCase("irf@uni-corvinus.hu", "abcd1234"),
            TestCase("irf@uni-corvinus.hu", "ABCD1234"),
            TestCase("irf@uni-corvinus.hu", "abcdABCD"),
            TestCase("irf@uni-corvinus.hu", "Ab1234"),
            TestCase("irf@uni-corvinus.hu", "Abcd1234")
        ]
        public void TestRegisterValidateException(string email, string password)
        {
            var accountServiceMock = new Mock<IAccountManager>(MockBehavior.Strict);
            accountServiceMock
                .Setup(m => m.CreateAccount(It.IsAny<Account>()))
                .Throws<ApplicationException>();
            var accountController = new AccountController();
            accountController.AccountManager = accountServiceMock.Object;
            try
            {
                var actualResult = accountController.Register(email, password);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<ValidationException>(ex);
            }
        }
    }
}
