using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.IO;


namespace InfaBGInfo.Security
{
    /// <summary>
    /// Summary description for Security
    /// </summary>
    public class asymmetricHash
    {
        private static MD5CryptoServiceProvider objHasher = new MD5CryptoServiceProvider();


        /// <summary>
        /// Hashes the string with 128Bit MD5 Encryption and returns a assymetric string. Used for passwords.
        /// </summary>
        /// <param name="strArg"></param>
        /// <returns></returns>
        public static string hashWithMD5128Bit(string strArg)
        {
            if (!string.IsNullOrEmpty(strArg))
            {
                byte[] tmpBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(strArg);

                //gen has from byte array
                byte[] tmpHash = objHasher.ComputeHash(tmpBytes);

                //convert has to string
                return Convert.ToBase64String(tmpHash, 0, tmpHash.Length);
            }
            else
            {
                return null;
            }
        }
    } //end main class hashObsfucate



    #region Security Hasing Exception Classes
    /// <summary>
    /// Exception 
    /// </summary>
    public class stringEncryptorException : Exception
    {
        public stringEncryptorException(string message)
            : base(message)
        {
        }
    } //end exception class


    public class ConfigXMLParseException : Exception
    {
        public ConfigXMLParseException(string message)
            : base(message)
        {
        }
    }


    #endregion




    public static class symmetricHash
    {
        public static string encryptWithDES(string strArg)
        {
            byte[] key = new byte[] { 13, 2, 3, 4, 122, 6, 74, 8 };
            byte[] iv = new byte[] { 1, 21, 3, 46, 5, 6, 76, 8 };

            try
            {
                byte[] sourceDataBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(strArg);

                MemoryStream tmpStream = new MemoryStream();

                //get encyptor and encryption stream
                DESCryptoServiceProvider encryptor = new DESCryptoServiceProvider();
                CryptoStream encryptionStream = new CryptoStream(tmpStream
                    , encryptor.CreateEncryptor(key, iv)
                    , CryptoStreamMode.Write);

                //encrypt data
                encryptionStream.Write(sourceDataBytes, 0, sourceDataBytes.Length);
                encryptionStream.FlushFinalBlock();

                //put data into byte array
                byte[] encryptedDataBytes = tmpStream.GetBuffer();
                               

                //convert encrypted data into string
                return Convert.ToBase64String(encryptedDataBytes, 0, (int)tmpStream.Length);

            }
            catch
            {
                throw new stringEncryptorException("Unable to encrypt data");
            }

        }


        public static string decryptWithDES(string strArg)
        {
            byte[] key = new byte[] { 13, 2, 3, 4, 122, 6, 74, 8 };
            byte[] iv = new byte[] { 1, 21, 3, 46, 5, 6, 76, 8 };

            try
            {
                byte[] encryptedDataBytes = Convert.FromBase64String(strArg);

                MemoryStream tmpStream = new MemoryStream(encryptedDataBytes, 0, encryptedDataBytes.Length);

                DESCryptoServiceProvider decryptor = new DESCryptoServiceProvider();
                CryptoStream decryptionStream = new CryptoStream(tmpStream
                            , decryptor.CreateDecryptor(key, iv)
                            , CryptoStreamMode.Read);

                //decrypt data
                StreamReader sdReader = new StreamReader(decryptionStream);
                return sdReader.ReadToEnd();
            }
            catch
            {
                throw new stringEncryptorException("Unable to decrypt data.");
            }

        }
    }//end stringEncryptor class




    /// <summary>
    /// Class the provides a secure implementation for credit card information in a wrapper
    /// </summary>
    public class secureCreditCardXML
    {
        /*//PReviously used credit card node for sending soap package to CYBERSOURCE via ASP
        	"<card>" & _
				"<accountNumber>" & get_cardNumber & "</accountNumber>" & _
				"<expirationMonth>" & get_cardExpMonth & "</expirationMonth>" & _
				"<expirationYear>" & get_cardExpYear & "</expirationYear>" & _
				"<cvNumber>" & get_cardId & "</cvNumber>" & _
			"</card>" & _
        */


        private bool isDecrypted = false;
        private bool isEncrypted = false;
        private string gatewayName = "cybersource";
        private string cardHolder;
        private string cardNumber;
        private string issueDate;
        private string expDate;
        private string expMonth;
        private string expYear;
        private string issueNumber;
        private string securityCode;
        private string cardType;
        private string encryptedData;
        private XmlDocument xmlCardData;


        private secureCreditCardXML()
        {

        }


        public secureCreditCardXML(string newEncryptedData)
        {
            //constructor for use with encrypted data
            encryptedData = newEncryptedData;
            decryptCardXMLData();
        }


        public secureCreditCardXML(string newGatewayName, string newCardHolder, string newCardNumber, string newIssueDate,
            string newExpDate, string newExpMonth, string newExpYear, string newIssueNumber
            , string newSecurityCode, string newCardType)
        {
            //constructor for ue with decrypted data
            gatewayName = newGatewayName;
            cardHolder = newCardHolder;
            cardNumber = newCardNumber;
            issueDate = newIssueDate;
            expDate = newExpDate;
            expMonth = newExpMonth;
            expYear = newExpYear;
            issueNumber = newIssueNumber;
            securityCode = newSecurityCode;
            cardType = newCardType;

            encryptCardXMLData();
        }


        private void createCardXML()
        {
            //encode card details as XML Document 
            xmlCardData = new XmlDocument();
            XmlElement documentRoot = xmlCardData.CreateElement("card");
            
            XmlElement child;
            child = xmlCardData.CreateElement("gatewayName");
            child.InnerXml = gatewayName;
            documentRoot.AppendChild(child);

            child = xmlCardData.CreateElement("cardHolder");
            child.InnerXml = cardHolder;
            documentRoot.AppendChild(child);

            child = xmlCardData.CreateElement("cardNumber");
            child.InnerXml = cardNumber;
            documentRoot.AppendChild(child);

            child = xmlCardData.CreateElement("issueDate");
            child.InnerXml = issueDate;
            documentRoot.AppendChild(child);

            child = xmlCardData.CreateElement("expDate");
            child.InnerXml = expDate;
            documentRoot.AppendChild(child);

            child = xmlCardData.CreateElement("expMonth");
            child.InnerXml = expDate;
            documentRoot.AppendChild(child);

            child = xmlCardData.CreateElement("expYear");
            child.InnerXml = expDate;
            documentRoot.AppendChild(child);

            child = xmlCardData.CreateElement("issueNumber");
            child.InnerXml = String.IsNullOrEmpty(issueNumber) ? "" : issueNumber;
            documentRoot.AppendChild(child);

            child = xmlCardData.CreateElement("securityCode");
            child.InnerXml = securityCode;
            documentRoot.AppendChild(child);

            child = xmlCardData.CreateElement("cardType");
            child.InnerXml = cardType;
            documentRoot.AppendChild(child);

            xmlCardData.AppendChild(documentRoot);
        }


        private void extractCardXML()
        {
            gatewayName = xmlCardData.GetElementsByTagName("gatewayName").Item(0).InnerXml;
            cardHolder = xmlCardData.GetElementsByTagName("cardHolder").Item(0).InnerXml;
            cardNumber = xmlCardData.GetElementsByTagName("cardNumber").Item(0).InnerXml;
            issueDate = xmlCardData.GetElementsByTagName("issueDate").Item(0).InnerXml;
            expDate = xmlCardData.GetElementsByTagName("expDate").Item(0).InnerXml;
            expMonth = xmlCardData.GetElementsByTagName("expMonth").Item(0).InnerXml;
            expYear = xmlCardData.GetElementsByTagName("expYear").Item(0).InnerXml;
            issueNumber = xmlCardData.GetElementsByTagName("issueNumber").Item(0).InnerXml;
            securityCode = xmlCardData.GetElementsByTagName("securityCode").Item(0).InnerXml;
            cardType = xmlCardData.GetElementsByTagName("cardType").Item(0).InnerXml;
        }


        private void encryptCardXMLData()
        {
            try
            {
                createCardXML();

                encryptedData = symmetricHash.encryptWithDES(xmlCardData.OuterXml);

                isEncrypted = true;
            }
            catch
            {
                throw new ConfigXMLParseException("Unable to encrypt data.");
            }
        }


        private void decryptCardXMLData()
        {
            try
            {
                xmlCardData = new XmlDocument();
                xmlCardData.InnerXml = symmetricHash.decryptWithDES(encryptedData);

                //extrct data from XML
                extractCardXML();

                isDecrypted = true;
            }
            catch
            {
                throw new ConfigXMLParseException("Unable to encrypt data.");
            }
        }




        #region ACCESSORS FOR THE CLASS

        public string _gatewayName
        {
            get
            {
                if (isDecrypted)
                    return gatewayName;
                else
                    throw new ConfigXMLParseException("Data not decrypted");

            }
        }

        public string _cardHolderName
        {
            get {
                if (isDecrypted)
                    return cardHolder;
                else
                    throw new ConfigXMLParseException("Data not decrypted");
            
            }
        }


        public string _cardNumber
        {
            get
            {
                if (isDecrypted)
                    return cardNumber;
                else
                {
                    throw new ConfigXMLParseException("Data not decrypted");
                }
            }
        }


        public string _cardNumberX
        {
            get
            {
                if (isDecrypted)
                    return "XXXX-XXXX-XXXX-" + cardNumber.Substring(cardNumber.Length - 4, 4);
                else
                {
                    throw new ConfigXMLParseException("Data not decrypted");
                }
            }
        }

        public string _issueDate
        {
            get
            {
                if (isDecrypted)
                    return issueDate;
                else
                    throw new ConfigXMLParseException("Data not decrypted");
            }
        }


        public string _expDate
        {
            get
            {
                if (isDecrypted)
                    return expDate;
                else
                {
                    throw new ConfigXMLParseException("Data not decrypted");
                }
            }
        }


        public string _expMonth
        {
            get
            {
                if (isDecrypted)
                    return expMonth;
                else
                {
                    throw new ConfigXMLParseException("Data not decrypted");
                }
            }
        }


        public string _expYear
        {
            get
            {
                if (isDecrypted)
                    return expYear;
                else
                {
                    throw new ConfigXMLParseException("Data not decrypted");
                }
            }
        }

        public string _issueNumber
        {
            get
            {
                if (isDecrypted)
                    return issueNumber;
                else
                    throw new ConfigXMLParseException("Data not decrypted");
            }
        }

        public string _securityCode
        {
            get
            {
                if (isDecrypted)
                    return securityCode;
                else
                    throw new ConfigXMLParseException("Data not decrypted");
            }
        }


        public string _cardType
        {
            get
            {
                if (isDecrypted)
                    return cardType;
                else
                    throw new ConfigXMLParseException("Data not decrypted");
            }
        }

        /// <summary>
        /// Returns the encryped XML data in the form of a string
        /// </summary>
        public string _encryptedData
        {
            get
            {
                if (isEncrypted)
                    return encryptedData;
                else
                    throw new ConfigXMLParseException("Data not Encrypted");
            }
        }



        #endregion




    } //end secureCreditCardXML class





    /// <summary>
    /// This class can generate random passwords, which do not include ambiguous 
    /// characters, such as I, l, and 1. The generated password will be made of
    /// 7-bit ASCII symbols. Every four characters will include one lower case
    /// character, one upper case character, one number, and one special symbol
    /// (such as '%') in a random order. The password will always start with an
    /// alpha-numeric character; it will not start with a special symbol (we do
    /// this because some back-end systems do not like certain special
    /// characters in the first position).
    /// </summary>
    public class randomPassword
    {
        public enum randomPasswordStrength{
            weak = 0
            ,secure
            ,strong
        }




        // Define default min and max password lengths.
        private static int DEFAULT_MIN_PASSWORD_LENGTH = 8;
        private static int DEFAULT_MAX_PASSWORD_LENGTH = 10;


        // Define strength depth variables
        private static string PASSWORD_CHARS_WEAK = "";
        private static string PASSWORD_CHARS_SECURE = "*";
        private static string PASSWORD_CHARS_STRONG = "*$@#%?&!";


        // Define supported password characters divided into groups.
        // You can add (or remove) characters to (from) these groups.
        private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
        private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
        private static string PASSWORD_CHARS_NUMERIC = "23456789";
        private static string PASSWORD_CHARS_SPECIAL = "*";

        /// <summary>
        /// Generates a random password.
        /// </summary>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        /// <remarks>
        /// The length of the generated password will be determined at
        /// random. It will be no shorter than the minimum default and
        /// no longer than maximum default.
        /// </remarks>
        public static string Generate()
        {
            return Generate(DEFAULT_MIN_PASSWORD_LENGTH,
                            DEFAULT_MAX_PASSWORD_LENGTH, randomPasswordStrength.strong);
        }

        /// <summary>
        /// Generates a random password of the exact length.
        /// </summary>
        /// <param name="length">
        /// Exact password length.
        /// </param>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        public static string Generate(int length)
        {
            return Generate(length, length, randomPasswordStrength.strong);
        }

        /// <summary>
        /// Generates a random password.
        /// </summary>
        /// <param name="minLength">
        /// Minimum password length.
        /// </param>
        /// <param name="maxLength">
        /// Maximum password length.
        /// </param>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        /// <remarks>
        /// The length of the generated password will be determined at
        /// random and it will fall with the range determined by the
        /// function parameters.
        /// </remarks>
        public static string Generate(int minLength,
                                      int maxLength, randomPasswordStrength passwordStrength)
        {
            // Make sure that input parameters are valid.
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;

            switch (passwordStrength)
            {
                case randomPasswordStrength.secure:
                    PASSWORD_CHARS_SPECIAL = PASSWORD_CHARS_SECURE;
                    break;
                case randomPasswordStrength.strong:
                    PASSWORD_CHARS_SPECIAL = PASSWORD_CHARS_STRONG;
                    break;
                default:
                    PASSWORD_CHARS_SPECIAL = PASSWORD_CHARS_WEAK;
                    PASSWORD_CHARS_NUMERIC = "";
                    break;
            }


            // Create a local array containing supported password characters
            // grouped by types. You can remove character groups from this
            // array, but doing so will weaken the password strength.
            char[][] charGroups = new char[][] 
            {
                PASSWORD_CHARS_LCASE.ToCharArray(),
                PASSWORD_CHARS_UCASE.ToCharArray(),
                PASSWORD_CHARS_NUMERIC.ToCharArray(),
                PASSWORD_CHARS_SPECIAL.ToCharArray()
            };

            // Use this array to track the number of unused characters in each
            // character group.
            int[] charsLeftInGroup = new int[charGroups.Length];

            // Initially, all characters in each group are not used.
            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;

            // Use this array to track (iterate through) unused character groups.
            int[] leftGroupsOrder = new int[charGroups.Length];

            // Initially, all character groups are not used.
            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;

            // Because we cannot use the default randomizer, which is based on the
            // current time (it will produce the same "random" number within a
            // second), we will use a random number generator to seed the
            // randomizer.

            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            // Now, this is real randomization.
            Random random = new Random(seed);

            // This array will hold password characters.
            char[] password = null;

            // Allocate appropriate memory for the password.
            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];

            // Index of the next character to be added to password.
            int nextCharIdx;

            // Index of the next character group to be processed.
            int nextGroupIdx;

            // Index which will be used to track not processed character groups.
            int nextLeftGroupsOrderIdx;

            // Index of the last non-processed character in a group.
            int lastCharIdx;

            // Index of the last non-processed group.
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            // Generate password characters one at a time.
            for (int i = 0; i < password.Length; i++)
            {
                // If only one character group remained unprocessed, process it;
                // otherwise, pick a random character group from the unprocessed
                // group list. To allow a special character to appear in the
                // first position, increment the second parameter of the Next
                // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);

                // Get the actual index of the character group, from which we will
                // pick the next character.
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                // Get the index of the last unprocessed characters in this group.
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                // If only one unprocessed character is left, pick it; otherwise,
                // get a random character from the unused character list.
                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);

                // Add this character to the password.
                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                // If we processed the last character in this group, start over.
                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;
                // There are more unprocessed characters left.
                else
                {
                    // Swap processed character with the last unprocessed character
                    // so that we don't pick it until we process all characters in
                    // this group.
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    // Decrement the number of unprocessed characters in
                    // this group.
                    charsLeftInGroup[nextGroupIdx]--;
                }

                // If we processed the last group, start all over.
                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                // There are more unprocessed groups left.
                else
                {
                    // Swap processed group with the last unprocessed group
                    // so that we don't pick it until we process all groups.
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    // Decrement the number of unprocessed groups.
                    lastLeftGroupsOrderIdx--;
                }
            }

            // Convert password characters into a string and return the result.
            return new string(password);
        }
    }






    public class stringHash
    {
        public static string hashWithWeakDES(string strArg)
        {
            byte[] key = new byte[] { 9, 65, 3, 6, 74, 1, 11, 43 };
            byte[] iv = new byte[] { 3, 41, 13, 24, 66, 9, 19, 38 };

            try
            {
                byte[] sourceDataBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(strArg);

                MemoryStream tmpStream = new MemoryStream();

                //get encyptor and encryption stream
                DESCryptoServiceProvider encryptor = new DESCryptoServiceProvider();
                CryptoStream encryptionStream = new CryptoStream(tmpStream
                    , encryptor.CreateEncryptor(key, iv)
                    , CryptoStreamMode.Write);

                //encrypt data
                encryptionStream.Write(sourceDataBytes, 0, sourceDataBytes.Length);
                encryptionStream.FlushFinalBlock();

                //put data into byte array
                byte[] encryptedDataBytes = tmpStream.GetBuffer();


                //convert encrypted data into string
                return Convert.ToBase64String(encryptedDataBytes, 0, (int)tmpStream.Length);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static string deHashWithWeakDES(string strArg)
        {
            byte[] key = new byte[] { 9, 65, 3, 6, 74, 1, 11, 43 };
            byte[] iv = new byte[] { 3, 41, 13, 24, 66, 9, 19, 38 };

            try
            {
                byte[] encryptedDataBytes = Convert.FromBase64String(strArg);

                MemoryStream tmpStream = new MemoryStream(encryptedDataBytes, 0, encryptedDataBytes.Length);

                DESCryptoServiceProvider decryptor = new DESCryptoServiceProvider();
                CryptoStream decryptionStream = new CryptoStream(tmpStream
                            , decryptor.CreateDecryptor(key, iv)
                            , CryptoStreamMode.Read);

                //decrypt data
                StreamReader sdReader = new StreamReader(decryptionStream);
                return sdReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }




}// end namespace