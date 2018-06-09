using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using HtmlAgilityPack;


using System.Net;namespace WindowsFormsApplication3
{
    public partial class Form1 : Form




    {



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {


            
            // Single Http Client
          //   HttpClient client = new HttpClient();

            string SourcePages = "C:\\CarWebsites.txt";
            string TargetPath = "c:\\AutoCars\\";

            Directory.Delete(TargetPath, true);
            Directory.CreateDirectory(TargetPath);

            string[] lines = System.IO.File.ReadAllLines(SourcePages);

            int LocalDirCounter = 53;
            int FileNumberPrefixCounter = 23;
            int FilenumberSuffixCounter = 1;

            // Display the file contents by using a foreach loop.
            System.Console.WriteLine("Contents of CarWebsites.txt = ");
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Console.WriteLine("\t" + line);

                // Get the source of the web page
                var request = (System.Net.HttpWebRequest)WebRequest.Create(line);
                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                // Search for the carousel items //

                int Pointer = 0;
                int StartPos = 0;
                int EndPos = 0;
                string ImageTag = "";
                string LocalDir = @"IMRG2018CDEVAL" + LocalDirCounter.ToString().PadLeft(4,(char)48) + "\\";
                string FirstImageTrail = "_t";

                // create a Directory for the files
                Directory.CreateDirectory(TargetPath + LocalDir);

                // Write SQL 
                string TheSQL = "INSERT INTO car_image (car_id, file_name, width, height, format_type, image_type) VALUES ";


                // Loop through all the files in the Carousel
                while (responseString.IndexOf(@"class="+(char)34+"gallery-picture__image", Pointer) > 0)
                    {
                        StartPos = responseString.IndexOf(@"class=" + (char)34 + "gallery-picture__image", Pointer);
                        StartPos = StartPos -6;

                        EndPos = responseString.IndexOf(">", StartPos+1);

                        Pointer = EndPos + 1;

                        Console.WriteLine("\t" + responseString.Substring(StartPos, EndPos- StartPos+1));

                        ImageTag = responseString.Substring(StartPos, EndPos - StartPos + 1);

                    // get the URL from the Image tag and download it to the DIR

                    int sp = ImageTag.IndexOf("data-fullscreen-src=");
                    sp = sp + 21;
                    int ep = ImageTag.IndexOf((char)34,sp);

                    string DAUL = ImageTag.Substring(sp, ep - sp );

                    Console.WriteLine("\t" + DAUL);


                    // Now Download the Image
                    string  DaTargetFilename = (TargetPath.ToString() + LocalDir + FileNumberPrefixCounter.ToString() + "5515325" + (FilenumberSuffixCounter.ToString()).PadLeft(4, (Char)48) + FirstImageTrail.ToString() + ".jpg");
                    string  DaCleanFilename = (FileNumberPrefixCounter.ToString() + "5515325" + (FilenumberSuffixCounter.ToString()).PadLeft(4, (Char)48) + FirstImageTrail.ToString() + ".jpg");

                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(new Uri(DAUL), DaTargetFilename);

                    }

                    if (FirstImageTrail.Length < 0)
                    {

                        TheSQL = TheSQL + "((SELECT id FROM car WHERE vin = 'IMRG2018CDEVAL00051'), '"+ DaCleanFilename + "', 394, 295, 'JPG', 'TEASER'    ),";
                    }
                    else
                    {

                        TheSQL = TheSQL + "((SELECT id FROM car WHERE vin = 'IMRG2018CDEVAL00051'), '" + DaCleanFilename + "', 394, 295, 'JPG', 'TEASER'    ),";
                    }
                    FirstImageTrail = "";
                    FilenumberSuffixCounter++;

                    

                }                  // Loop through all the files in the Carousel

                // Remove the last comma and add a semi-colon
                TheSQL = TheSQL.Substring(0, TheSQL.Length - 1) + ";";


                FilenumberSuffixCounter = 1;
                LocalDirCounter++;
                FileNumberPrefixCounter++;

                // Write theSQL to a file
                System.IO.File.WriteAllText(@"C:\WriteText.txt", TheSQL);





                //  var pagesource = gethtml(line);

                //HtmlAgilityPack.HtmlDocument Thedocument = new HtmlAgilityPack.HtmlDocument();

                //Thedocument.OptionFixNestedTags = true;

                //Thedocument.LoadHtml(responseString);

                // Thedocument.
                //HtmlAgilityPack.HtmlNode a  = Thedocument.GetElementbyId("div");



                // Create a Directory with bthe correct filename


                // For each file, download then into the directory and rename fthe files correctly



                MessageBox.Show("Done");


            }

            // Keep the console window open in debug mode.
            //Console.WriteLine("Directory with files created");
            //// System.Console.ReadKey();

            //System.IO.Directory.Delete(TargetPath,true);
            //System.IO.Directory.CreateDirectory(TargetPath);


            // Traverse the directory to Generate SQL for each file

            // Generate 1 sql satement with multiple value statements per DIRECTORY for all the filenames in each directory












        }
    }
}
