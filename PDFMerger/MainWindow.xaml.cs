using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PDFMerger
{

    public partial class MainWindow : Window
    {
        private List<string> selectedFiles;
        
        public MainWindow()
        {
            InitializeComponent();
            selectedFiles = new List<string>();
        }

        private void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var fileName in openFileDialog.FileNames)
                {
                    FilesListBox.Items.Add(fileName);
                    selectedFiles.Add(fileName);
                }
            }
        }

        private void MergeButton_Click(Object sender, RoutedEventArgs e)
        {
            if(selectedFiles.Count < 2)
            {
                MessageBox.Show("Please select at least two files to merge");
                return;
            }

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF file (*.pdf)|*.pdf";
            saveFileDialog.FileName = "Merged.pdf";

            if(saveFileDialog.ShowDialog() == true)
            {
                MergePDFFiles(selectedFiles, saveFileDialog.FileName);
                MessageBox.Show("PDF files merged successfully");
                FilesListBox.Items.Clear();
                selectedFiles.Clear();
            }
        }

        private void MergePDFFiles(List<string> sourceFiles, string destinationFile)
        {
            using (var stream = new FileStream(destinationFile, FileMode.Create))
            {
                var document = new Document();
                var pdfCopy = new PdfCopy(document, stream);
                document.Open();

                foreach (var file in sourceFiles)
                {
                    var reader = new PdfReader(file);

                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        var page = pdfCopy.GetImportedPage(reader, i);
                        pdfCopy.AddPage(page);
                    }

                    reader.Close();
                }

                pdfCopy.Close();
                document.Close();
            }
        }
    }
}
