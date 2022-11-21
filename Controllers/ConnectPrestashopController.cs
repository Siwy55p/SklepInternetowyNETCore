using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using partner_aluro.Data;
using partner_aluro.Migrations;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;
using System.Globalization;


//"Prestashop": "Server=aluro.mysql.dhosting.pl,3306;ID=ieg3ga_aluro;Password=Siiwy1a2!;Database=ieza7a_aluropar",
namespace partner_aluro.Controllers
{
    [Authorize]
    public class ConnectPrestashopController : Controller
    {
        readonly IContactPrestashop _contactPrestashop;
        readonly IAddressPrestashop _addressPrestashop;
        readonly IProductPrestashop _productPrestashop;
        readonly IProductNazwyPrestashop _productNazwyPrestashop;
        readonly IProductQuantityPrestashop _productQuantityPrestashop;

        readonly IProductService _productService;

        readonly ApplicationDbContext _context;

        public string con1 = "server=aluro.mysql.dhosting.pl;user=ieg3ga_aluro;database=ieza7a_aluropar;port=3306;password=Siiwy1a2!;Allow Zero Datetime=True";
        public string con2 = "server=aluro.mysql.dhosting.pl;user=iefi4n_aluro2;database=euhi4i_aluroshop;port=3306;password=3jjN9vEn7T;Allow Zero Datetime=True"; //aktualna baza bierzaca
        public ConnectPrestashopController(IContactPrestashop contactPrestashop, IAddressPrestashop addressPrestashop, IProductPrestashop productPrestashop, ApplicationDbContext context, IProductNazwyPrestashop productNazwyPrestashop, IProductService productService, IProductQuantityPrestashop productQuantityPrestashop)
        {
            _contactPrestashop = contactPrestashop;
            _addressPrestashop = addressPrestashop;
            _productPrestashop = productPrestashop;



            _context = context;
            _productNazwyPrestashop = productNazwyPrestashop;
            _productService = productService;

            _productQuantityPrestashop = productQuantityPrestashop;
        }
        public IActionResult DodajProduktyZPrestashop()
        {

            List<ProductPrestashop> listaProduktowDostepnychzPresta = _context.ProductsPrestashop.ToList(); // Ta lista nie potrzebna
            List<ProductPrestashop> listaproduktowDostepnychZPresty = new List<ProductPrestashop>();

            List<ProductQuantityPrestashop> listaProduktowKToreSaNaStanie = _context.ProductsQuantityPrestashop.Where(x => x.quantity >= 1).ToList();

            for(int i = 0; i < listaProduktowKToreSaNaStanie.Count(); i++)
            {
                ProductPrestashop produktPresta = listaProduktowDostepnychzPresta.Where(x => x.id_product == listaProduktowKToreSaNaStanie[i].id_product).FirstOrDefault();
                //jesli sa ilosci dostepne to dodaj
                listaproduktowDostepnychZPresty.Add(produktPresta);
            }


            List<Product> nowe = new List<Product>();
            List<Product> aktualizacja = new List<Product>();

            for (int i = 0; i < listaproduktowDostepnychZPresty.Count(); i++)
            {
                //blad dla i = 120 id_product=1512 i=od 0
                //blad dla i = 158 id_product= 1570 i=od 121
                //blad dla i = 159 id_product= 1570 i=od 121
                if (listaproduktowDostepnychZPresty[i] != null)
                {
                        Product produkt = _context.Products.Where(x => x.Symbol == listaproduktowDostepnychZPresty[i].reference).FirstOrDefault();
                

                    if (produkt != null)
                    {

                        //produkt wystepuje i trzeba zaktualizowac dane
                        produkt.EAN13 = listaproduktowDostepnychZPresty[1].ean13;
                        produkt.Ilosc = _productQuantityPrestashop.iloscProduktu((int)listaproduktowDostepnychZPresty[i].id_product);
                        produkt.CenaProduktu = (decimal)listaproduktowDostepnychZPresty[i].price;
                        produkt.CenaProduktuDetal = (decimal)listaproduktowDostepnychZPresty[i].wholesale_price;
                        produkt.SzerokoscProduktu = (decimal)listaproduktowDostepnychZPresty[i].width;
                        produkt.WysokoscProduktu = (decimal)listaproduktowDostepnychZPresty[i].height;
                        produkt.GlebokoscProduktu = (decimal)listaproduktowDostepnychZPresty[i].depth;
                        produkt.WagaProduktu = (decimal)listaproduktowDostepnychZPresty[i].weight;
                        produkt.DataDodania = DateTime.Parse(listaproduktowDostepnychZPresty[i].date_add);
                        _productService.Update(produkt);

                        aktualizacja.Add(produkt);
                    }
                    else
                    {
                        //produkt niewystepuje dodaj do bazy
                        Product product = new Product();
                        product.CategoryId = 31;
                        product.Name = _productNazwyPrestashop.NazwaProduktu((int)listaproduktowDostepnychZPresty[i].id_product);
                        product.Symbol = listaproduktowDostepnychZPresty[i].reference;
                        product.Description = _productNazwyPrestashop.DlugiOpisProduktu((int)listaproduktowDostepnychZPresty[i].id_product);

                        product.DataDodania = DateTime.Parse(listaproduktowDostepnychZPresty[1].date_add);
                        product.CenaProduktu = (decimal)listaproduktowDostepnychZPresty[1].price;
                        product.Pakowanie = "";
                        product.Materiał = "";
                        product.Ilosc = _productQuantityPrestashop.iloscProduktu((int)listaproduktowDostepnychZPresty[1].id_product);
                        product.CenaProduktuDetal = (decimal)listaproduktowDostepnychZPresty[i].wholesale_price;
                        product.WagaProduktu = (decimal)listaproduktowDostepnychZPresty[i].weight;
                        product.SzerokoscProduktu = (decimal)listaproduktowDostepnychZPresty[i].width;
                        product.WysokoscProduktu = (decimal)listaproduktowDostepnychZPresty[i].height;
                        product.GlebokoscProduktu = (decimal)listaproduktowDostepnychZPresty[i].depth;
                        product.DataDodania = DateTime.Parse(listaproduktowDostepnychZPresty[i].date_add);
                        product.Ukryty = true;
                        product.EAN13 = listaproduktowDostepnychZPresty[i].ean13;
                        product.KrotkiOpis = _productNazwyPrestashop.KrotkiOpisProduktu((int)listaproduktowDostepnychZPresty[i].id_product);
                        product.Promocja = false;
                        _productService.AddProduct(product);
                        nowe.Add(product);
                    }
                }
            }
            //if(listaProduktowDostepnychzPresta[1].reference == )

            //listaProduktowDostepnychzPresta[1];

            //foreach (var prestaprodukt in listaProduktowDostepnychzPresta)
            //{

            //}

            ProductPrestashopNoweModel vm = new ProductPrestashopNoweModel();

            vm.nowe = nowe;
            vm.zaktualizowen = aktualizacja;

            return View(vm);
        }





        public IActionResult ProductQuantityPrestashop()
        {
            List<ProductQuantityPrestashop> ProduktQuantity = new List<ProductQuantityPrestashop>();



            using (MySqlConnection con = new MySqlConnection(con2))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM ps_stock_available", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //fetch your data
                    ProductQuantityPrestashop productQuantityPresta = new ProductQuantityPrestashop();
                    productQuantityPresta.id_stock_available = Convert.ToInt32(reader["id_stock_available"]);
                    productQuantityPresta.id_product = Convert.ToInt32(reader["id_product"]);
                    productQuantityPresta.id_product_attribute = Convert.ToInt32(reader["id_product_attribute"]);
                    productQuantityPresta.id_shop = Convert.ToInt32(reader["id_shop"]);
                    productQuantityPresta.quantity = Convert.ToInt32(reader["quantity"]);
                    productQuantityPresta.depends_on_stock = Convert.ToInt32(reader["depends_on_stock"]);
                    productQuantityPresta.out_of_stock = Convert.ToInt32(reader["out_of_stock"]);

                    ProductQuantityPrestashop wystepuje = _context.ProductsQuantityPrestashop.Where(x => x.id_product == productQuantityPresta.id_product).FirstOrDefault();
                    if (wystepuje != null)
                    {

                    }
                    else
                    {
                        _productQuantityPrestashop.Add(productQuantityPresta);
                    }
                }
                reader.Close();
            }
            return View();
        }
        public IActionResult ProductNazwyPrestashop()
        {
            List<ProductNazwyPrestashop> NazwyProduktow = new List<ProductNazwyPrestashop>();



            using (MySqlConnection con = new MySqlConnection(con2))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM ps_product_lang", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //fetch your data
                    ProductNazwyPrestashop productNazwa = new ProductNazwyPrestashop();
                    productNazwa.id_product = Convert.ToInt32(reader["id_product"]);
                    productNazwa.id_shop = Convert.ToInt32(reader["id_shop"]);
                    productNazwa.id_lang = Convert.ToInt32(reader["id_lang"]);
                    productNazwa.description = Convert.ToString(reader["description"]);
                    productNazwa.description_short = Convert.ToString(reader["description_short"]);
                    productNazwa.link_rewrite = Convert.ToString(reader["link_rewrite"]);
                    productNazwa.meta_description = Convert.ToString(reader["meta_description"]);
                    productNazwa.meta_keywords = Convert.ToString(reader["meta_keywords"]);
                    productNazwa.meta_title = Convert.ToString(reader["meta_title"]);
                    productNazwa.name = Convert.ToString(reader["name"]);
                    productNazwa.available_now = Convert.ToString(reader["available_now"]);
                    productNazwa.available_later = Convert.ToString(reader["available_later"]);


                    ProductNazwyPrestashop wystepuje = _context.ProductsNamePrestashop.Where(x => x.name == productNazwa.name).FirstOrDefault();
                    if (wystepuje != null)
                    {

                    }
                    else
                    {

                        NazwyProduktow.Add(productNazwa);

                        _productNazwyPrestashop.Add(productNazwa);
                    }
                }
                reader.Close();
            }
            return View(NazwyProduktow);
        }



        public IActionResult ProductPrestashop()
        {
            List<ProductPrestashop> products = new List<ProductPrestashop>();



            using (MySqlConnection con = new MySqlConnection(con2))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM ps_product", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //fetch your data
                    ProductPrestashop product = new ProductPrestashop();
                    product.id_product = Convert.ToInt32(reader["id_product"]);
                    product.id_supplier = Convert.ToInt32(reader["id_supplier"]);
                    product.id_manufacturer = Convert.ToInt32(reader["id_manufacturer"]);
                    product.id_category_default = Convert.ToInt32(reader["id_category_default"]);
                    product.id_shop_default = Convert.ToInt32(reader["id_shop_default"]);
                    product.id_tax_rules_group = Convert.ToInt32(reader["id_tax_rules_group"]);
                    product.on_sale = Convert.ToByte(reader["on_sale"]);
                    product.online_only = Convert.ToByte(reader["online_only"]);
                    product.ean13 = Convert.ToString(reader["ean13"]);
                    product.upc = Convert.ToString(reader["upc"]);
                    product.ecotax = Convert.ToDecimal(reader["ecotax"]);
                    product.quantity = Convert.ToInt32(reader["quantity"]);
                    product.minimal_quantity = Convert.ToInt32(reader["minimal_quantity"]);
                    product.price = Convert.ToDecimal(reader["price"]);
                    product.wholesale_price = Convert.ToDecimal(reader["wholesale_price"]);
                    product.unity = Convert.ToString(reader["unity"]);
                    product.unit_price_ratio = Convert.ToDecimal(reader["unit_price_ratio"]);
                    product.additional_shipping_cost = Convert.ToDecimal(reader["additional_shipping_cost"]);
                    product.reference = Convert.ToString(reader["reference"]);
                    product.supplier_reference = Convert.ToString(reader["supplier_reference"]);
                    product.location = Convert.ToString(reader["location"]);
                    product.width = Convert.ToDecimal(reader["width"]);
                    product.height = Convert.ToDecimal(reader["height"]);
                    product.depth = Convert.ToDecimal(reader["depth"]);
                    product.weight = Convert.ToDecimal(reader["weight"]);
                    product.out_of_stock = Convert.ToInt32(reader["out_of_stock"]);
                    product.quantity_discount = Convert.ToByte(reader["quantity_discount"]);
                    product.customizable = Convert.ToByte(reader["customizable"]);
                    product.uploadable_files = Convert.ToByte(reader["uploadable_files"]);
                    product.text_fields = Convert.ToByte(reader["text_fields"]);
                    product.active = Convert.ToByte(reader["active"]);
                    product.id_product_redirected = Convert.ToInt32(reader["id_product_redirected"]);
                    product.available_for_order = Convert.ToByte(reader["available_for_order"]);


                    product.available_date = Convert.ToString(reader["available_date"]);
                    //product.condition = (ProductPrestashop.condit)Enum.Parse(typeof(ProductPrestashop.condit),reader["condition"].ToString());
                    product.show_price = Convert.ToByte(reader["show_price"]);
                    product.indexed = Convert.ToByte(reader["indexed"]);
                    //product.visibility = Convert.ToByte(reader["visibility"]);
                    product.cache_is_pack = Convert.ToByte(reader["cache_is_pack"]);
                    product.cache_has_attachments = Convert.ToByte(reader["cache_has_attachments"]);
                    product.is_virtual = Convert.ToByte(reader["is_virtual"]);
                    product.cache_default_attribute = Convert.ToInt32(reader["cache_default_attribute"]);
                    product.date_add = Convert.ToString(reader["date_add"]);
                    product.date_upd = Convert.ToString(reader["date_upd"]);
                    product.advanced_stock_management = Convert.ToByte(reader["advanced_stock_management"]);
                    product.pack_stock_type = Convert.ToInt32(reader["pack_stock_type"]);


                    ProductPrestashop wystepuje = _context.ProductsPrestashop.Where(x => x.reference == product.reference).FirstOrDefault();
                    if (wystepuje != null)
                    {

                    }
                    else
                    {

                        products.Add(product);

                        _productPrestashop.Add(product);
                    }
                }
                reader.Close();
            }
            return View(products);
        }

        public IActionResult AddressPrestashop()
        {
            List<AddresPrestashop> addresss = new List<AddresPrestashop>();

            using (MySqlConnection con = new MySqlConnection(con2))
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


                    AddresPrestashop wystepuje = _context.AddressPrestashop.Where(x => x.id_address == address.id_address).FirstOrDefault();
                    if (wystepuje != null)
                    {

                    }
                    else
                    {

                        addresss.Add(address);
                        _addressPrestashop.Add(address);
                    }

                }
                reader.Close();
            }
            return View(addresss);
        }
        public IActionResult UsersPrestahop()
        {
            List<ContactPrestashop> persons = new List<ContactPrestashop>();

            using (MySqlConnection con = new MySqlConnection(con2))
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


                    ContactPrestashop wystepuje = _context.ContactsPrestashop.Where(x => x.Idcustommer == person.Idcustommer).FirstOrDefault();
                    if (wystepuje != null)
                    {

                    }
                    else
                    {
                        persons.Add(person);
                        _contactPrestashop.Add(person);
                    }
                }
                reader.Close();
            }
            return View(persons);
        }
    }
}
