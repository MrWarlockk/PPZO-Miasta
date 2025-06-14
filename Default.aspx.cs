using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System;
using System.Data.SqlClient;
using System.Data.OleDb;

public partial class _Default : System.Web.UI.Page
{

    private void OdswiezMiasta()
    {
        try
        {
            // utworzenie polaczenia
            string ConnectStr = Application["ConnectStr"].ToString();
            OleDbConnection con = new OleDbConnection(ConnectStr);
            // stworzenie zrodla danych i pobranie danych
            OleDbDataAdapter myCommand = new OleDbDataAdapter("select * from Wojewodztwa", con);
            DataSet ds = new DataSet();
            myCommand.Fill(ds);
            DataTable dt = ds.Tables[0];
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "Wojewodztwo";
            DropDownList1.DataValueField = "ID";
            DropDownList1.DataBind();

        }

        catch (OleDbException ex)
        {
            labelkomunikat.Text = ex.Message;
        }

    }

    private void Miasta()
    {
        try
        {
            string selectedID = DropDownList1.SelectedValue;
            string ConnectStr = Application["ConnectStr"].ToString();
            OleDbConnection con = new OleDbConnection(ConnectStr);

            string query = "select * from Miasta where ID_woj = ?";
            OleDbCommand cmd = new OleDbCommand(query, con);

            cmd.Parameters.AddWithValue("?", selectedID);

            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            DropDownList2.DataSource = ds.Tables[0];
            DropDownList2.DataTextField = "Miasto";
            DropDownList2.DataValueField = "ID";
            DropDownList2.DataBind();
        }
        catch (Exception ex)
        {
            labelkomunikat.Text = ex.Message;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            OdswiezMiasta();
            Miasta();
        }

    }

    protected void Button1_Click(object sender, System.EventArgs e)
    {
        try
        {
            string selectedCityID = DropDownList2.SelectedValue;
            string ConnectStr = Application["ConnectStr"].ToString();
            OleDbConnection con = new OleDbConnection(ConnectStr);

            string query = "select Dl, Szer from Miasta where ID = ?";
            OleDbCommand cmd = new OleDbCommand(query, con);
            cmd.Parameters.AddWithValue("?", selectedCityID);

            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();
            reader.Read();
            string dlugosc = reader["Dl"].ToString();
            string szerokosc = reader["Szer"].ToString();
            labelkomunikat.Text = "Wspolrzedne miasta: Dlugosc geograficzna:" + dlugosc + ", Szerokosc geograficzna:" + szerokosc;
            reader.Close();
            con.Close();
        }
        catch (Exception ex)
        {
            labelkomunikat.Text = ex.Message;
        }
    }
    protected void DropDownList1_Change(object sender, EventArgs e)
    {
        Miasta();
    }
}
