using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using partner_aluro.Models;


//"Prestashop": "Server=aluro.mysql.dhosting.pl,3306;ID=ieg3ga_aluro;Password=Siiwy1a2!;Database=ieza7a_aluropar",
namespace partner_aluro.Controllers
{
    public class ConnectPrestashopController : Controller
    {
        public IActionResult Index()
        {
            List<ContactPrestashop> persons = new List<ContactPrestashop>();

            using (MySqlConnection con = new MySqlConnection("server=aluro.mysql.dhosting.pl;user=ieg3ga_aluro;database=ieza7a_aluropar;port=3306;password=Siiwy1a2!"))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM ps_customer", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    //fetch your data
                    ContactPrestashop person = new ContactPrestashop();
                    person.Id = Convert.ToInt32(reader["id_customer"]);
                    person.FirstName = reader["firstname"].ToString();
                    person.LastName = reader["lastname"].ToString();

                    person.id_shop_group = Convert.ToInt32(reader["id_shop_group"]);
                    person.id_shop = Convert.ToInt32(reader["id_shop"]);
                    person.id_gender = Convert.ToInt32(reader["id_gender"]);
                    person.id_default_group = Convert.ToInt32(reader["id_default_group"]);
                    person.id_id_lang = Convert.ToInt32(reader["id_id_lang"]);
                    person.id_risk = Convert.ToInt32(reader["id_risk"]);

                    person.company = reader["company"].ToString();
                    person.email = reader["email"].ToString();
                    person.passwd = reader["passwd"].ToString();
                    person.website = reader["website"].ToString();
                    person.secure_key = reader["secure_key"].ToString();
                    person.note = reader["note"].ToString();

                    person.active = Convert.ToInt32(reader["active"]);
                    person.is_quest = Convert.ToInt32(reader["is_quest"]);
                    person.deleted = Convert.ToInt32(reader["deleted"]);

                    persons.Add(person);
                }
                reader.Close();
            }
            return View(persons);
        }
    }
}
