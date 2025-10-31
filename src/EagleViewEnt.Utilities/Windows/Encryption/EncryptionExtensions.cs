//-----------------------------------------------------------------------
// <copyright 
//	   Author="Brian Dick"
//     Company="Eagle View Enterprises LLC"
//     Copyright="(c) Eagle View Enterprises LLC. All rights reserved."
//     Email="support@eagleviewent.com"
//     Website="www.eagleviewent.com"
// >
//	   <Disclaimer>
//			THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// 			TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// 			THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// 			CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// 			DEALINGS IN THE SOFTWARE.
// 		</Disclaimer>
// </copyright>
//-----------------------------------------------------------------------

using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace MgbUtilities.Windows.Encryption
{

    public static class EncryptionExtensions
    {

        public static string Decrypt( string encryptedText )
        {

            // Create a symmetric decryption algorithm
            using Aes aes = Aes.Create();
            aes.Key = GetHashBytes();
            aes.IV = new byte[16]; // Initialize IV with zeros

            // Decrypt the encrypted text
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decryptedBytes;
            using(MemoryStream memoryStream = new()) {
                using CryptoStream cryptoStream = new(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                cryptoStream.FlushFinalBlock();
                decryptedBytes = memoryStream.ToArray();
            }

            // Return the decrypted text as a string
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public static string Encrypt( string plainText
                                     , string? key = null )
        {

            // Create a symmetric encryption algorithm
            using Aes aes = Aes.Create();
            aes.Key = GetHashBytes();
            aes.IV = new byte[16]; // Initialize IV with zeros

            // Encrypt the plain text
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes;
            using(MemoryStream memoryStream = new MemoryStream()) {
                using CryptoStream cryptoStream = new CryptoStream(
                    memoryStream
                    , aes.CreateEncryptor()
                    , CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                encryptedBytes = memoryStream.ToArray();
            }

            // Return the encrypted text as a base64-encoded string
            return Convert.ToBase64String(encryptedBytes);
        }

        static byte[] GetHashBytes()
        {

            // Get the machine ID
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UUID FROM Win32_ComputerSystem");
            ManagementObjectCollection collection = searcher.Get();
            string machineId = collection.Cast<ManagementObject>().First()["UUID"].ToString()!;

            // Create a hash of the machine ID
            byte[] machineIdBytes = Encoding.UTF8.GetBytes(machineId);
            byte[] hashBytes;
            hashBytes = SHA256.HashData(machineIdBytes);
            return hashBytes;
        }

    }

}
