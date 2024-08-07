using System;
using System.Security.Cryptography;
using System.Text;

namespace EncryptDeccrypt
{
    public partial class Form1 : Form
    {
        private readonly Aes _aes;
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public Form1()
        {
            InitializeComponent();
            _aes = Aes.Create();
            _aes.Key = Encoding.UTF8.GetBytes("1213141516171819");
            _aes.IV = Encoding.UTF8.GetBytes("1213141516171819");

            _key = _aes.Key;
            _iv = _aes.IV;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please enter a plaintext message.");
                return;
            }

            string plaintext = textBox1.Text;
            string ciphertext = Encrypt(plaintext);
            textBox2.Text = ciphertext;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please enter a ciphertext message.");
                return;
            }

            string ciphertext = textBox1.Text;
            string plaintext = Decrypt(ciphertext);
            textBox2.Text = plaintext;
        }

        private string Encrypt(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
            {
                throw new ArgumentNullException(nameof(plaintext), "Plaintext cannot be null or empty.");
            }

            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plaintext);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        private string Decrypt(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext))
            {
                throw new ArgumentNullException(nameof(ciphertext), "Ciphertext cannot be null or empty.");
            }

            byte[] buffer = Convert.FromBase64String(ciphertext);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}