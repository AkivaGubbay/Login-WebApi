using LoginWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;


namespace LoginWebApi.Contracts
{
    public class Validation
    {
        public string errorMsg  { get; set; }
        public int methodOfResponse { get; set; }

        public Validation(string username, string email, string password)
        {
            errorMsg = string.Empty;
            methodOfResponse = GlobalParmeters.RESPONSE_OK;
            CheckValidation(username, email, password);
        }

        public void CheckValidation(string username, string email, string password)
        {

            ValidateUserName(username);
            if (methodOfResponse == GlobalParmeters.RESPONSE_SERVERERROR) return;

            ValidateEmail(email);
            if (methodOfResponse == GlobalParmeters.RESPONSE_SERVERERROR) return;

            ValidatePassword(password);
        }

        private void LegalAddress(string email)
        {
            //Validate email address
            try
            {
                MailAddress m = new MailAddress(email);
            }
            catch (FormatException ex)
            {
                this.errorMsg += "email addresss " + email + " not sufficient.";
                this.methodOfResponse = GlobalParmeters.RESPONSE_USERERROR;
            }
            catch
            {
                this.methodOfResponse = GlobalParmeters.RESPONSE_SERVERERROR;
            }
        }
 
        private void ValidateUserName(string username)
        {
            int res = DAL.UserNameExist(username);
            if (res == GlobalParmeters.ENCOUNTERED_EXCEPTION)
            {
                this.methodOfResponse = GlobalParmeters.RESPONSE_SERVERERROR;
            }
            else if (res == GlobalParmeters.NOT_VAID)
            {
                this.errorMsg += "username " + username + " already exists.\n";
                methodOfResponse = GlobalParmeters.RESPONSE_USERERROR;
            }
        }

        private void ValidatePassword(string password)
        {
            int res = DAL.PasswordExist(password);
            if (res == GlobalParmeters.ENCOUNTERED_EXCEPTION)
            {
                this.methodOfResponse = GlobalParmeters.RESPONSE_SERVERERROR;
            }
            else if (res == GlobalParmeters.NOT_VAID)
            {
                this.errorMsg += "password already exists.\n";
                methodOfResponse = GlobalParmeters.RESPONSE_USERERROR;
            }
        }

        private void ValidateEmail(string email)
        {
            LegalAddress(email);

            int res = DAL.EmailExist(email);
            if (res == GlobalParmeters.ENCOUNTERED_EXCEPTION)
            {
                this.methodOfResponse = GlobalParmeters.RESPONSE_SERVERERROR;
            }
            else if (res == GlobalParmeters.NOT_VAID)
            {
                this.errorMsg += "email address " + email + " already exists.\n";
                methodOfResponse = GlobalParmeters.RESPONSE_USERERROR;
            }
        }
    }
}
