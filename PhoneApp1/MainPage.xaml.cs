using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.UI;
using Windows.UI.Popups;
using PhoneApp1.Resources;

namespace PhoneApp1
{   
  
    public partial class MainPage : PhoneApplicationPage
    {
        public Dictionary<int, TextBox> matrix = new Dictionary<int, TextBox>();
        public Dictionary<string, float> multis = new Dictionary<string, float>();

        public int chioX = 0;
        float a;
        float b;
        float c;
        float d;
        float e;
        float f;
        float multiplicador;
        private string textBox;

        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }
       /* public async void Aha(string a, string b)
        {
            var msg = new MessageDialog( "Rada");
            msg.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler(CommandHandlers)));
            await msg.ShowDialog();
        }*/
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox1.Text != "0")
            {
                for (int i = 1; i <= Convert.ToInt32(TextBox1.Text); i++)
                {
                    for (int j = 1; j <= Convert.ToInt32(TextBox2.Text); j++)
                    {
                        TextBox textBox = new TextBox();
                        textBox.Text = "0";
                        textBox.Height = 70;
                        textBox.Width = 87;
                        textBox.Margin = new Thickness(75*j - 100 ,100 * i - 500,0,0);
                        textBox.HorizontalAlignment = HorizontalAlignment.Left;
                        ContentPanel.Children.Add(textBox);
                        matrix.Add(Convert.ToInt32(i.ToString() + j.ToString()), textBox);
                    }
                }
            }
        }
        private void determinante()
        {
            if (chioX < 1 && matrix.Count == 4)
            {
                a = float.Parse(matrix[11].Text) * float.Parse(matrix[22].Text);
                b = float.Parse(matrix[12].Text) * float.Parse(matrix[21].Text);

            }
            else if (chioX < 1)
            {
                a = float.Parse(matrix[11].Text) * float.Parse(matrix[22].Text) * float.Parse(matrix[33].Text);
                b = float.Parse(matrix[12].Text) * float.Parse(matrix[23].Text) * float.Parse(matrix[31].Text);
                c = float.Parse(matrix[13].Text) * float.Parse(matrix[21].Text) * float.Parse(matrix[32].Text);
                d = float.Parse(matrix[13].Text) * float.Parse(matrix[22].Text) * float.Parse(matrix[31].Text);
                e = float.Parse(matrix[11].Text) * float.Parse(matrix[23].Text) * float.Parse(matrix[32].Text);
                f = float.Parse(matrix[12].Text) * float.Parse(matrix[21].Text) * float.Parse(matrix[33].Text);
            }
            else
            {
                a = multis["11"] * multis["22"] * multis["33"];
                b = multis["12"] * multis["23"] * multis["31"];
                c = multis["13"] * multis["21"] * multis["32"];
                d = multis["13"] * multis["22"] * multis["31"];
                e = multis["11"] * multis["23"] * multis["32"];
                f = multis["12"] * multis["21"] * multis["33"];
            }
            float result = (a + b + c) - (d + e + f);
            if (chioX > 1)
            {
                result *= multiplicador;
            } if (result == float.NaN || float.IsNaN(result))
            {
                result = 0;
            }
            for (int i = 0; i <= matrix.Count; i++)
            {
                var last = matrix.Last();
                TextBox rada = last.Value;
                rada.IsEnabled = !IsEnabled;
            }
            matrix.Clear();
            TextBox textBox = new TextBox();
            textBox.Text = "0";
            textBox.Height = 70;
            textBox.Width = 87;
            textBox.Margin = new Thickness(-25,-400, 0, 0);
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            ContentPanel.Children.Add(textBox);

            if (chioX < 1 && matrix.Count == 4)
            {
                textBox.Text = (a - b).ToString();
            }
            else textBox.Text = result.ToString();

        }
        private void Chio()
        {
            Dictionary<string, float> atual = new Dictionary<string, float>();

            if (chioX < 1)
            {
                for (int j = 1; j <= Convert.ToInt32(TextBox1.Text); j++)
                {
                    for (int i = 1; i <= Convert.ToInt32(TextBox2.Text); i++)
                    {
                        atual.Add(j.ToString() + i.ToString(), float.Parse(matrix[Convert.ToInt32(j.ToString() + i.ToString())].Text));
                    }
                }
            }
            else
            {
                for (int j = 1; j <= multis.Count / (Convert.ToInt32(TextBox1.Text) - chioX); j++)
                {
                    for (int i = 1; i <= multis.Count / (Convert.ToInt32(TextBox2.Text) - chioX); i++)
                    {
                        atual.Add(j.ToString() + i.ToString(), multis[j.ToString() + i.ToString()]);
                    }
                }

                multis.Clear();
            }

            multiplicador = 1 / atual["11"];

            for (int i = 1; i <= Convert.ToInt32(TextBox1.Text) - chioX; i++)
            {
                for (int j = 1; j <= Convert.ToInt32(TextBox2.Text) - chioX; j++)
                {

                    if (i == 1)
                    {
                        float b = atual[i.ToString() + j.ToString()] * multiplicador;
                    }
                    else if (j != 1)
                    {
                        multis.Add((i - 1).ToString() + (j - 1).ToString(), atual[i.ToString() + j.ToString()] - atual["1" + j.ToString()] * atual[i.ToString() + "1"]);
                    }
                }
            }

            chioX += 1;

            if (multis.Count > 9) Chio();
            else determinante();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (matrix.Count > 9) Chio();
            else determinante();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}