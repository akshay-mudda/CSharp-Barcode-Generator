using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ZXing;

namespace CSharp_Barcode_Generator
{
    public partial class Form1 : Form
    {
        private int maxCharacterLimit = 50; // Set your desired character limit here
        private string defaultWatermarkText = "Bar code Data (Click here)";

        public Form1()
        {
            InitializeComponent();
            // Set the default watermark text
            SetWatermark();
        }
        private void SetWatermark()
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Text = defaultWatermarkText;
                textBox1.ForeColor = SystemColors.WindowText;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateBarcode();
        }

        private void GenerateBarcode()
        {
            string inputValue = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(inputValue) && inputValue != defaultWatermarkText)
            {
                if (inputValue.Length <= maxCharacterLimit)
                {
                    BarcodeWriter barcodeWriter = new BarcodeWriter();
                    barcodeWriter.Format = BarcodeFormat.CODE_128;

                    Bitmap barcodeBitmap = new Bitmap(barcodeWriter.Write(inputValue));

                    if (barcodeBitmap.Width > BarcodeBox.Width)
                    {
                        // Calculate scaling factor to fit within PictureBox width
                        float scale = (float)BarcodeBox.Width / barcodeBitmap.Width;

                        // Create a new bitmap with scaled dimensions
                        Bitmap scaledBarcodeBitmap = new Bitmap(barcodeBitmap, new Size((int)(barcodeBitmap.Width * scale), (int)(barcodeBitmap.Height * scale)));

                        // Set the scaled image to PictureBox
                        BarcodeBox.Image = scaledBarcodeBitmap;
                    }
                    else
                    {
                        // Set the original image to PictureBox
                        BarcodeBox.Image = barcodeBitmap;
                    }
                }
                else
                {
                    MessageBox.Show($"Please enter a value with at most {maxCharacterLimit} characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a value in the Barcode Box before generating a barcode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (BarcodeBox.Image != null)
            {
                Bitmap bitmap = new Bitmap(BarcodeBox.Image);

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG File (*.png)|*.png|Bitmap (*.bmp)|*.bmp";
                saveFileDialog.Title = "Save Bar Code";
                saveFileDialog.FileName = "Bar_Code";

                if (saveFileDialog.ShowDialog()== DialogResult.OK)
                {
                    bitmap.Save(saveFileDialog.FileName);
                    MessageBox.Show("Image saved to your computer","Image Saved", MessageBoxButtons.OK);
                }
                bitmap.Dispose();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Trigger the same functionality as the "Generate" button click
                GenerateBarcode();
                e.Handled = true; // Handle the key event to prevent it from being processed further
            }
            else if (e.KeyChar != (char)Keys.Back)
            {
                // Check if the entered character is a valid character (alphabet, number, space, hyphen, or underscore)
                bool isValidCharacter = char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == ' ' || e.KeyChar == '-' || e.KeyChar == '_';

                // If not a valid character, handle the key event to prevent it from being processed further
                if (!isValidCharacter)
                {
                    e.Handled = true;
                }
            }
        }



        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string githubProfileLink = "https://github.com/akshay-mudda";

            // Open the link in the default web browser
            Process.Start(githubProfileLink);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            // When the TextBox gets focus, remove the watermark text if it's the default watermark
            if (textBox1.Text == defaultWatermarkText)
            {
                textBox1.Text = string.Empty;
                textBox1.ForeColor = SystemColors.WindowText;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            // When the TextBox loses focus and is empty, display the watermark text
            SetWatermark();
        }
    }
}
