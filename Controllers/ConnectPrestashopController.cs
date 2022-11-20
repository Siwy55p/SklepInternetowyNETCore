using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;


//"Prestashop": "Server=aluro.mysql.dhosting.pl,3306;ID=ieg3ga_aluro;Password=Siiwy1a2!;Database=ieza7a_aluropar",
namespace partner_aluro.Controllers
{
    public class ConnectPrestashopController : Controller
    {
        readonly IContactPrestashop _contactPrestashop;
        readonly IAddressPrestashop _addressPrestashop;

        public ConnectPrestashopController(IContactPrestashop contactPrestashop, IAddressPrestashop addressPrestashop)
        {
            _contactPrestashop = contactPrestashop;
            _addressPrestashop = addressPrestashop; 
        }
        public IActionResult AddressPrestashop()
        {
            List<AddresPrestashop> addresss = new List<AddresPrestashop>();

            using (MySqlConnection con = new MySqlConnection("server=aluro.mysql.dhosting.pl;user=ieg3ga_aluro;database=ieza7a_aluropar;port=3306;password=Siiwy1a2!;Allow Zero Datetime=True"))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM ps_address", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //fetch your data
                    AddresPrestashop address = new AddresPrestashop();
                    address.id_address = Convert.ToInt32(reader["id_address"]);
                    address.alias = reader["alias"].ToString();
                    address.company = reader["company"].ToString();

                    address.id_country = Convert.ToInt32(reader["id_country"]);
                    address.id_state = Convert.ToInt32(reader["id_state"]);
                    address.id_customer = Convert.ToInt32(reader["id_customer"]);
                    address.id_manufacturer = Convert.ToInt32(reader["id_manufacturer"]);
                    address.id_supplier = Convert.ToInt32(reader["id_supplier"]);
                    address.id_warehouse = Convert.ToInt32(reader["id_warehouse"]);

                    address.company = reader["company"].ToString();
                    address.lastname = reader["lastname"].ToString();
                    address.firstname = reader["firstname"].ToString();
                    address.address1 = reader["address1"].ToString();
                    address.address2 = reader["address2"].ToString();
                    address.postcode = reader["postcode"].ToString();


                    address.other = reader["other"].ToString();
                    address.phone = reader["phone"].ToString();
                    address.phone_mobile = reader["phone_mobile"].ToString();
                    address.vat_number = reader["vat_number"].ToString();
                    address.dni = reader["dni"].ToString();


                    address.date_add = Convert.ToDateTime(reader["date_add"]);
                    address.date_upd = Convert.ToDateTime(reader["date_upd"]);


                    address.active = Convert.ToInt32(reader["active"]);
                    address.deleted = Convert.ToInt32(reader["deleted"]);

                    //address.birthday = Convert.ToDateTime(reader["birthday"]);

                    //address.optin = Convert.ToInt32(reader["optin"]);

                    //address.newsletter_date_add = Convert.ToDateTime(reader["newsletter_date_add"]);


                    addresss.Add(address);
                    _addressPrestashop.Add(address);
                }
                reader.Close();
            }
            return View(addresss);
        }
        public IActionResult UsersPrestahop()
        {
            List<ContactPrestashop> persons = new List<ContactPrestashop>();

            using (MySqlConnection con = new MySqlConnection("server=aluro.mysql.dhosting.pl;user=ieg3ga_aluro;database=ieza7a_aluropar;port=3306;password=Siiwy1a2!;Allow Zero Datetime=True"))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM ps_customer", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    //fetch your data
                    ContactPrestashop person = new ContactPrestashop();
                    person.Idcustommer = Convert.ToInt32(reader["id_customer"]);
                    person.FirstName = reader["firstname"].ToString();
                    person.LastName = reader["lastname"].ToString();

                    person.id_shop_group = Convert.ToInt32(reader["id_shop_group"]);
                    person.id_shop = Convert.ToInt32(reader["id_shop"]);
                    person.id_gender = Convert.ToInt32(reader["id_gender"]);
                    person.id_default_group = Convert.ToInt32(reader["id_default_group"]);
                    person.id_id_lang = Convert.ToInt32(reader["id_lang"]);
                    person.id_risk = Convert.ToInt32(reader["id_risk"]);

                    person.company = reader["company"].ToString();
                    person.email = reader["email"].ToString();
                    person.passwd = reader["passwd"].ToString();
                    person.website = reader["website"].ToString();
                    person.secure_key = reader["secure_key"].ToString();
                    person.note = reader["note"].ToString();

                    person.active = Convert.ToInt32(reader["active"]);
                    person.is_quest = Convert.ToInt32(reader["is_guest"]);
                    person.deleted = Convert.ToInt32(reader["deleted"]);

                    person.date_add = Convert.ToDateTime(reader["date_add"]);
                    person.date_upd = Convert.ToDateTime(reader["date_upd"]);

                    //person.birthday = Convert.ToDateTime(reader["birthday"]);

                    person.optin = Convert.ToInt32(reader["optin"]);

                    //person.newsletter_date_add = Convert.ToDateTime(reader["newsletter_date_add"]);


                    persons.Add(person);
                    _contactPrestashop.Add(person);
                }
                reader.Close();
            }
            return View(persons);
        }
    }
}
