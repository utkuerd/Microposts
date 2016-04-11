using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Micropost.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Micropost.Tests.Models
{
    [TestClass]
    public class ValidationTests
    {
        private User testObj;

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        private bool ValidateSuccesfull(object model)
        {
            return ValidateModel(model).Count == 0;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            testObj = new User
            {
                Name = "Deneme User",
                Email = "deneme@example.com",
                Password = "foobar",
                PasswordConfirmation = "foobar"
            };
        }        

        [TestMethod]
        public void TestUserClass()
        {                        
            // Validate
            Assert.IsTrue(ValidateSuccesfull(testObj), "Unable to create proper user object");            
        }

        [TestMethod]
        public void TestBlankUsername()
        {
            testObj.Name = "";
            Assert.IsFalse(ValidateSuccesfull(testObj), "User should not have blank name");
        }

        [TestMethod]
        public void TestBlankEMail()
        {
            testObj.Email = "";
            Assert.IsFalse(ValidateSuccesfull(testObj), "User should not have blank email");
        }

        [TestMethod]
        public void TestUsernameLength()
        {
            testObj.Name = new string('a', 51);
            Assert.IsFalse(ValidateSuccesfull(testObj), "Username length should not exceed 50 characters");
        }

        [TestMethod]
        public void TestEmailLength()
        {
            testObj.Name = new string('a', 244)+ "@example.com";
            Assert.IsFalse(ValidateSuccesfull(testObj), "Username email  should not exceed 254 characters");
        }

        [TestMethod]
        public void TestValidEmails()
        {
            string[] validAddresses = {"user@example.com", "USER@foo.COM", "A_US-ER@foo.bar.org",
                                        "first.last@foo.jp", "alice+bob@baz.cn"};

            foreach (string address in validAddresses)
            {
                testObj.Email = address;
                Assert.IsTrue(ValidateSuccesfull(testObj), "Email address {0} should be valid", address);
            }
        }

        [TestMethod]
        public void TestInvalidEmails()
        {
            string[] invalidAddresses = {"user@example,com", "user_at_foo.org", "user.name@example.",
                                        "foo@bar_baz.com", "foo@bar+baz.com"};

            foreach (string address in invalidAddresses)
            {
                testObj.Email = address;
                Assert.IsFalse(ValidateSuccesfull(testObj), "Email address {0} should be invalid", address);
            }
        }

        [TestMethod]
        public void TestPasswordShouldBeNonBlank()
        {
            testObj.Password = testObj.PasswordConfirmation = new String(' ',6);
            Assert.IsFalse(ValidateSuccesfull(testObj), "Password should be non blank");
        }

        [TestMethod]
        public void TestPasswordLength()
        {
            testObj.Password = testObj.PasswordConfirmation = new String('a', 5);
            Assert.IsFalse(ValidateSuccesfull(testObj), "Password should be 6 characters at least");
        }
    }
}
