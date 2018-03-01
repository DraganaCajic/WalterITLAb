using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;
using Newtonsoft.Json;
using System.IO;
using SimpleJson;

namespace Zadatak1
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
            btnSearch.Enabled = false;
            tbName.Focus();

        }

       

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lbMovie.Items.Clear();
            pb.ImageLocation = "";
            rtbDetails.Text = "";
            rtbPlot.Text = "";
            RestClient restClient = new RestClient($"http://www.omdbapi.com/?apikey=98f50a84&");
            RestRequest restRequest = new RestRequest(Method.GET);
            restRequest.AddParameter("t", tbName.Text.ToString());
            //RestResponse<Person> response2 = client.Execute<Person>(request);
            List<Movie> movie;
            RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();
            var response = restClient.Execute(restRequest);
            movie = deserial.Deserialize<List<Movie>>(response);
            string data = response.Content;
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\tmpFile.txt", data);



            foreach (var item in movie)
            {
                
                if (item.Response == "True")
                {
                    lbMovie.Items.Add(item.Title.ToString());
                }
                else
                {
                    MessageBox.Show("Your search didn't return any result!");
                    tbName.Text = "";
                }
            }
                
        }

       

        private void lbMovie_SelectedValueChanged(object sender, EventArgs e)
        {
            RestClient restClient = new RestClient($"http://www.omdbapi.com/?apikey=98f50a84&");
            RestRequest restRequest = new RestRequest(Method.GET);
            restRequest.AddParameter("t", lbMovie.SelectedItem.ToString());
           
            var result = restClient.Execute(restRequest);
            RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();
            Movie movie = deserial.Deserialize<Movie>(result);
            pb.ImageLocation = movie.Poster;
            rtbDetails.Text = "Year: " + movie.Year + "\nActors: " + movie.Actors + "\nGenre: " + movie.Genre + "\nWriter: " + movie.Writer;
            rtbPlot.Text = "Plot: \n" + movie.Plot;


        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            btnSearch.Enabled = true;
        }
    }
}
