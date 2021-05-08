using System;
using Gtk;

using MySql.Data;
using MySql.Data.MySqlClient;

public partial class MainWindow : Gtk.Window
{
    static MySqlConnection conn;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnButton1Clicked(object sender, EventArgs e)
    {
        string filename = file.Buffer.Text;
        var alldata = HW5P2Lib.HW5P2.readfile(filename);

        string input = task.Buffer.Text;
        string title = HW5P2Lib.HW5P2.getTitle(Int32.Parse(input), alldata);

        answerReturned.Buffer.Text = "Title: " + title;

    }

    protected void OnButton2Clicked(object sender, EventArgs e)
    {
        string filename = file.Buffer.Text;
        var alldata = HW5P2Lib.HW5P2.readfile(filename);

        string input = task.Buffer.Text;
            int word_count = HW5P2Lib.HW5P2.wordCount(Int32.Parse(input), alldata);

        answerReturned.Buffer.Text = "Number of words in the article: " + word_count;
    }

    protected void OnButton3Clicked(object sender, EventArgs e)
    {
        string filename = file.Buffer.Text;
        var alldata = HW5P2Lib.HW5P2.readfile(filename);

        string input = task.Buffer.Text;
        string actualmonth = HW5P2Lib.HW5P2.getMonthName(Int32.Parse(input), alldata);

        answerReturned.Buffer.Text = "Month of chosen article: " + actualmonth;
    }

    protected void OnButton4Clicked(object sender, EventArgs e)
    {
        string filename = file.Buffer.Text;
        var alldata = HW5P2Lib.HW5P2.readfile(filename);

        var publisherNames = HW5P2Lib.HW5P2.publishers(alldata);

        answerReturned.Buffer.Text = "Unique Publishers: " + Environment.NewLine;
        foreach (var item in publisherNames)
        {
            answerReturned.Buffer.Text += item + Environment.NewLine;
        }
    }

    protected void OnButton5Clicked(object sender, EventArgs e)
    {
        string filename = file.Buffer.Text;
        var alldata = HW5P2Lib.HW5P2.readfile(filename);

        var countryNames = HW5P2Lib.HW5P2.countries(alldata);

        answerReturned.Buffer.Text = "Unique Countries: " + Environment.NewLine;
        foreach (var item in countryNames)
        {
            answerReturned.Buffer.Text += item + Environment.NewLine;
        }
    }

    protected void OnButton6Clicked(object sender, EventArgs e)
    {
        string filename = file.Buffer.Text;
        var alldata = HW5P2Lib.HW5P2.readfile(filename);

        double overallguard = HW5P2Lib.HW5P2.avgNewsguardscoreForArticles(alldata);

        answerReturned.Buffer.Text = "Average news guard score for all articles: " + overallguard;
    }

    protected void OnButton7Clicked(object sender, EventArgs e)
    {
        string filename = file.Buffer.Text;
        var alldata = HW5P2Lib.HW5P2.readfile(filename);

        var nArticles = HW5P2Lib.HW5P2.numberOfArticlesEachMonth(alldata);

        answerReturned.Buffer.Text = "Number of Articles for Each Month: " + Environment.NewLine;
        string output = HW5P2Lib.HW5P2.buildHistogram(nArticles, alldata.Length, "");
        output = output.Replace("/n", Environment.NewLine);
        answerReturned.Buffer.Text += output;
    }

    protected void OnButton8Clicked(object sender, EventArgs e)
    {
        string filename = file.Buffer.Text;
        var alldata = HW5P2Lib.HW5P2.readfile(filename);

        var reliablepct = HW5P2Lib.HW5P2.reliableArticlePercentEachPublisher(alldata);
        var output = HW5P2Lib.HW5P2.printNamesAndPercentages(reliablepct);

        answerReturned.Buffer.Text = "Percentage of Articles that are reliable for each publisher: " + Environment.NewLine;
        foreach (var item in output)
        {
            answerReturned.Buffer.Text += item;
        }
    }

    protected void OnButton9Clicked(object sender, EventArgs e)
    {
        string filename = file.Buffer.Text;
        var alldata = HW5P2Lib.HW5P2.readfile(filename);

        var countryNames = HW5P2Lib.HW5P2.countries(alldata);
        var averageguard = HW5P2Lib.HW5P2.avgNewsguardscoreEachCountry(alldata, countryNames);
        var result = HW5P2Lib.HW5P2.printNamesAndFloats(averageguard);

        answerReturned.Buffer.Text = "Average News Guard Score for Each Country: " + Environment.NewLine;
        foreach (var item in result)
        {
            answerReturned.Buffer.Text += item;
        }
    }

    protected void OnButton10Clicked(object sender, EventArgs e)
    {
        string filename = file.Buffer.Text;
        var alldata = HW5P2Lib.HW5P2.readfile(filename);

        var bias = HW5P2Lib.HW5P2.avgNewsguardscoreEachBias(alldata);
        string output = HW5P2Lib.HW5P2.buildHistogramFloat(bias, "");

        output = output.Replace("/n", Environment.NewLine);
        answerReturned.Buffer.Text = "The Average News Guard Score for Each Political Bias Category: " + Environment.NewLine;
        answerReturned.Buffer.Text += output;
    }

    static void establishConnection(string server, string port, string username, string password, string database)
    {
        //myConnectionString = "server=127.0.0.1;user=root;" + "password=newpassword;database=project4";
        string connStr = "server=" + server + ";"
        + "user=" + username + ";"
        + "database=" + database + ";"
        + "port=" + port + ";"
        + "password=" + password;


        //string connStr = "server=localhost;user=root;database=database1;port=3306;password=karlov@1723";  // change the database and password to test on your machine
        conn = new MySqlConnection(connStr);                                                     // must be these values when submitting to gradescope
        conn.Open();
    }

    protected void OnButton11Clicked(object sender, EventArgs e)
    {
        try
        {
            establishConnection(server.Buffer.Text, port.Buffer.Text, username.Buffer.Text, password.Buffer.Text, database.Buffer.Text);
        }   
        catch (Exception ex)
        {
            answerReturned.Buffer.Text = "Could not connect to server";
            return;
        }

        string input = task.Buffer.Text;

        try
        {
            string query = String.Format(@"
                SELECT title
                FROM news
                WHERE news_id = {0};
                ", Int32.Parse(input));

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand(query, conn);
            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            answerReturned.Buffer.Text = String.Format("{0} {1}", reader.GetName(0), Environment.NewLine);

            // Loop through the rows of the DataReader to output the values from the DataReader
            while (reader.Read())
            {
                answerReturned.Buffer.Text += reader[0] + Environment.NewLine;
            }
            reader.Close();
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text = "Could not perform query";
        }

    }

    protected void OnButton12Clicked(object sender, EventArgs e)
    {
        try
        {
            establishConnection(server.Buffer.Text, port.Buffer.Text, username.Buffer.Text, password.Buffer.Text, database.Buffer.Text);
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text = "Could not connect to server";
            return;
        }

        try
        {
            string query = String.Format(@" 
                                                SELECT news_id, LENGTH(body_text) AS length
                                                FROM news
                                                WHERE LENGTH(body_text)>100
                                                ORDER BY news_id;
                                            ");

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand(query, conn);
            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            answerReturned.Buffer.Text = String.Format("{0}\t{1} {2}", reader.GetName(0), reader.GetName(1), Environment.NewLine);

            // Loop through the rows of the DataReader to output the values from the DataReader

            while (reader.Read())
            {
                answerReturned.Buffer.Text += String.Format("{0}\t{1} {2}", reader.GetString(0), reader.GetInt32(1), Environment.NewLine);
            }

            // Close the DataReader
            reader.Close();
        }
        catch (Exception ex)
        {
           answerReturned.Buffer.Text = "Could not perform query";
        }
    }

    protected void OnButton13Clicked(object sender, EventArgs e)
    {
        try
        {
            establishConnection(server.Buffer.Text, port.Buffer.Text, username.Buffer.Text, password.Buffer.Text, database.Buffer.Text);
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text = "Could not connect to server";
            return;
        }

        try
        {
            string query = String.Format(@"
                                                SELECT title, DATE_FORMAT(STR_TO_DATE(publish_date, '%c/%d/%y'), '%M') AS Month
                                                FROM news
                                                ORDER BY STR_TO_DATE(publish_date, '%m/%d/%y')
                                            ");

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand(query, conn);
            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            answerReturned.Buffer.Text = String.Format("{0}\t{1} {2}", reader.GetName(0), reader.GetName(1), Environment.NewLine);

            // Loop through the rows of the DataReader to output the values from the DataReader

            while (reader.Read())
            {
                answerReturned.Buffer.Text += String.Format("{0}\t{1} {2}", reader.GetString(0), reader.GetString(1), Environment.NewLine);
            }

            // Close the DataReader
            reader.Close();
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text = "Could not perform query";
        }
    }

    protected void OnButton14Clicked(object sender, EventArgs e)
    {
        try
        {
            establishConnection(server.Buffer.Text, port.Buffer.Text, username.Buffer.Text, password.Buffer.Text, database.Buffer.Text);
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text = "Could not connect to server";
            return;
        }

        try
        {
            string query = String.Format(@"
                                                SELECT publisher
                                                FROM publisher_table
                                                JOIN news
                                                USING (publisher_id)
                                                GROUP BY publisher
                                                ORDER BY publisher;
                                            ");

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand(query, conn);
            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            answerReturned.Buffer.Text = String.Format("{0} {1}", reader.GetName(0), Environment.NewLine);

            // Loop through the rows of the DataReader to output the values from the DataReader

            while (reader.Read())
            {
                answerReturned.Buffer.Text = reader[0] + Environment.NewLine;
            }

            // Close the DataReader
            reader.Close();
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text = "Could not perform query";
        }
    }

    protected void OnButton15Clicked(object sender, EventArgs e)
    {
        try
        {
            establishConnection(server.Buffer.Text, port.Buffer.Text, username.Buffer.Text, password.Buffer.Text, database.Buffer.Text);
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text = "Could not connect to server";
            return;
        }

        try
        {
            string query = String.Format(@"
                                                SELECT country, COUNT(news_id) AS articleCount
                                                FROM country_table
                                                LEFT JOIN news
                                                USING (country_id)
                                                GROUP BY country
                                                ORDER BY articleCount DESC;
                                            ");

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand(query, conn);
            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            answerReturned.Buffer.Text = String.Format("{0}\t{1} {2}", reader.GetName(0), reader.GetName(1), Environment.NewLine);

            // Loop through the rows of the DataReader to output the values from the DataReader

            while (reader.Read())
            {
                answerReturned.Buffer.Text += String.Format("{0}\t{1} {2}", reader.GetString(0), reader.GetInt32(1), Environment.NewLine);
            }

            // Close the DataReader
            reader.Close();
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text = "Could not perform query";
        }
    }

    protected void OnButton16Clicked(object sender, EventArgs e)
    {
        try
        {
            establishConnection(server.Buffer.Text, port.Buffer.Text, username.Buffer.Text, password.Buffer.Text, database.Buffer.Text);
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text = "Could not connect to server";
            return;
        }

        try
        {
            string query = String.Format(@"
                                                SELECT ROUND(AVG(news_guard_score),3) AS `Average Score`
                                                FROM news;
                                            ");

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand(query, conn);
            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            answerReturned.Buffer.Text = String.Format("{0} {1}", reader.GetName(0), Environment.NewLine);

            // Loop through the rows of the DataReader to output the values from the DataReader

            while (reader.Read())
            {
                answerReturned.Buffer.Text += String.Format("{0,0:N3} {1}", reader.GetDouble(0), Environment.NewLine);
            }

            // Close the DataReader
            reader.Close();
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text += "Could not perform query";
        }
    }

    protected void OnButton17Clicked(object sender, EventArgs e)
    {
        try
        {
            establishConnection(server.Buffer.Text, port.Buffer.Text, username.Buffer.Text, password.Buffer.Text, database.Buffer.Text);
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text = "Could not connect to server";
            return;
        }

        try
        {
            string query = String.Format(@"
                                                SELECT month, numArticles, overall, ROUND(100*numArticles/overall,3) AS percentage
                                                FROM
                                                (
                                                SELECT month, monthnum, COUNT(publish_date) AS numArticles, overallCount AS overall
                                                FROM
                                                (
                                                SELECT DATE_FORMAT(STR_TO_DATE(publish_date, '%m/%d/%y'), '%M') AS month, 
                                                DATE_FORMAT(STR_TO_DATE(publish_date, '%m/%d/%y'), '%m') AS monthnum,
                                                publish_date
                                                FROM news
                                                ) AS T1
                                                JOIN
                                                (
                                                SELECT COUNT(*) overallCount FROM news
                                                ) AS T2
                                                GROUP BY month, monthnum, overallCount
                                                ) AS T3
                                                ORDER BY monthnum;
                                            ");

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand(query, conn);
            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            answerReturned.Buffer.Text = String.Format("{0}\t{1}\t{2}\t{3} {4}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3), Environment.NewLine);

            // Loop through the rows of the DataReader to output the values from the DataReader

            while (reader.Read())
            {
                answerReturned.Buffer.Text += String.Format("{0}\t{1}\t{2}\t{3} {4}", reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetDouble(3), Environment.NewLine);
            }

            // Close the DataReader
            reader.Close();
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text += "Could not perform query";
        }
    }

    protected void OnButton18Clicked(object sender, EventArgs e)
    {
        try
        {
            establishConnection(server.Buffer.Text, port.Buffer.Text, username.Buffer.Text, password.Buffer.Text, database.Buffer.Text);
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text = "Could not connect to server";
            return;
        }

        try
        {
            string query = String.Format(@"
                                                SELECT publisher, ROUND(AVG(reliability)*100, 3) AS percentage
                                                FROM news
                                                JOIN publisher_table
                                                USING (publisher_id)
                                                GROUP BY publisher
                                                ORDER BY percentage DESC, publisher;
                                            ");

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand(query, conn);
            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            answerReturned.Buffer.Text = String.Format("{0}\t{1} {2}", reader.GetName(0), reader.GetName(1), Environment.NewLine);

            // Loop through the rows of the DataReader to output the values from the DataReader

            while (reader.Read())
            {
                answerReturned.Buffer.Text += String.Format("{0}\t{1,0:N3} {2}", reader.GetString(0), reader.GetDouble(1), Environment.NewLine);
            }

            // Close the DataReader
            reader.Close();
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text += "Could not perform query";
        }
    }

    protected void OnButton19Clicked(object sender, EventArgs e)
    {
        try
        {
            establishConnection(server.Buffer.Text, port.Buffer.Text, username.Buffer.Text, password.Buffer.Text, database.Buffer.Text);
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text = "Could not connect to server";
            return;
        }

        try
        {
            string query = String.Format(@"
                                            SELECT country, ROUND(AVG(news_guard_score),3) AS avg_news_score
                                                FROM news
                                                JOIN country_table
                                                USING (country_id)
                                                GROUP BY country
                                                ORDER BY AVG(news_guard_score) DESC, country ASC;    
                                            ");

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand(query, conn);
            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            answerReturned.Buffer.Text = String.Format("{0}\t{1} {2}", reader.GetName(0), reader.GetName(1), Environment.NewLine);

            // Loop through the rows of the DataReader to output the values from the DataReader

            while (reader.Read())
            {
                answerReturned.Buffer.Text += String.Format("{0}\t{1,0:N3} {2}", reader.GetString(0), reader.GetDouble(1), Environment.NewLine);
            }

            // Close the DataReader
            reader.Close();
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text += "Could not perform query";
        }
    }

    protected void OnButton20Clicked(object sender, EventArgs e)
    {
        try
        {
            establishConnection(server.Buffer.Text, port.Buffer.Text, username.Buffer.Text, password.Buffer.Text, database.Buffer.Text);
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text = "Could not connect to server";
            return;
        }

        try
        {
            string query = String.Format(@"
                                                SELECT author, political_bias, COUNT(*) AS numArticles
                                                FROM news
                                                JOIN news_authors
                                                USING (news_id)
                                                JOIN author_table
                                                USING (author_id)
                                                JOIN political_bias_table
                                                USING (political_bias_id)
                                                GROUP BY author, political_bias
                                                ORDER BY author, COUNT(*) DESC, political_bias;
                                            ");

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand(query, conn);
            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            answerReturned.Buffer.Text = String.Format("{0}\t{1}\t{2} {3}", reader.GetName(0), reader.GetName(1), reader.GetName(2), Environment.NewLine);

            // Loop through the rows of the DataReader to output the values from the DataReader

            while (reader.Read())
            {
                answerReturned.Buffer.Text += String.Format("{0}\t{1}\t{2} {3}", reader.GetString(0), reader.GetString(1), reader.GetInt32(2), Environment.NewLine);
            }

            // Close the DataReader
            reader.Close();
        }
        catch (Exception ex)
        {
            answerReturned.Buffer.Text   += "Could not perform query";
        }
    }
}
