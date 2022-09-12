using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces.Repos
{
    public class PasswordEncryptRepo : IPasswordEncryptRepo
    {
        private readonly ILogger<PasswordEncryptRepo> _logger;
        public PasswordEncryptRepo(ILogger<PasswordEncryptRepo> logger)
        {
            _logger = logger;
        }
        public string EncryptPassword(string plainPassword)
        {
            try
            {
                byte[] encData_byte = new byte[plainPassword.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(plainPassword);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return string.Empty;
            }
        }
        public string DecryptPassword(string encryptedPassword)
        {
            try
            {
                UTF8Encoding encoder = new UTF8Encoding();
                Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(encryptedPassword);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new string(decoded_char);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return string.Empty;
            }
            
        }        
    }
}
