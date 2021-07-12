using Atestat2._0.Models;
using Atestat2._0.Models.Modele;
using Sitecore.FakeDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Atestat2._0.Controllers
{
    // CartiController conține toate acțiunile (funcțiile) pe care le folosesc pentru a manipula baza de date
    public class CartiController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext(); // accesez baza de date prin intermediul obiectului db

        [HttpGet] // HttpGet afișează pe pagina web
        public ActionResult Index() // Index este funcția ce afișează (prin intermediul paginii cshtml cu același nume) toate cărțile literare
        {
            List<Carti> carti = db.Carti.ToList(); // pun toate cărțile din baza de date 
            List<Carti> carti_literatura = new List<Carti>(); // inițializez o listă pentru cărțile care sunt doar din categoria de cărți literare
            foreach (var carte in carti) // iterez pe fiecare carte din lista de cărți
            {
                if(carte.Categorie == "literatura") // dacă categoria cărții respective este "literatura"
                {
                    carti_literatura.Add(carte); // atunci adaug cartea în lista de cărți litereare
                }
            }
            ViewBag.Carti = carti_literatura; // pun în ViewBag lista de cărți 
            // ViewBag este un fel de colecție de date pentru afișare specifică limbajului Razor folosit în view-uri (cod cshtml)
            return View(); // returnez view-ul specific funcției. View este funcția de afișare specifică limbajului Razor
        }


        // la fel pentru cărțile non-literare
        [HttpGet]
        public ActionResult Index2()
        {
            List<Carti> carti = db.Carti.ToList();
            List<Carti> carti_literatura = new List<Carti>();
            foreach (var carte in carti)
            {
                if (carte.Categorie == "non-literatura")
                {

                    carti_literatura.Add(carte);
                }
            }
            ViewBag.Carti = carti_literatura;
            return View();
        }



        [Authorize(Roles = "User, Admin")] // doar userii și administratorul pot avea acces la această funcție 
        public ActionResult Details(int? id) // funcție cu care pot afișa detaliile despre o carte (descrierea, genulurile ei, coperta, imaginea cu coperta)
        {
            if (id.HasValue) // dacă cartea curentă are un id valid, atunci găsesc cartea respectivă în baza de date și o băgăm în view-ul pentru funcția Details
            {
                Carti carte = db.Carti.Find(id);
                if (carte != null)
                {
                    return View(carte);
                }
                return HttpNotFound("Nu am putut gasi carte cu id-ul: " + id.ToString() + "!"); // dacă nu s-a putut găsi id-ul în baza de date, atunci returnez eroare
            }
            return HttpNotFound("Lipseste id-ul cartii!"); // dacă nu există un id înainte de căutare, atunci returnez eroare
        }



        [Authorize(Roles = "Admin")] // numai administratorul are acces la această acțiune
        [HttpGet]
        public ActionResult New() // prin funcția asta adun datele necesare pentru adăugarea unei cărți noi în baza de date direct de pe site
        {
            Carti carte = new Carti(); // creez o carte nouă (goală momentan)
            carte.Coperti_List = Get_tip_coperta(); // funcție cu care adaug în lista de coperți a cărții noi toate tipurile de coperți din baza de date
            carte.Edituri_List = Get_edituri(); // funcție cu care adaug în lista de edituri a cărții noi toate editurile din baza de date
            carte.Genuri_List = Get_genuri(); // funcție cu care adaug în lista de genuri a cărții noi toate genurile din baza de date
            carte.Genuri = new List<Gen>(); // creez pentru cartea nouă o listă de genuri în care o să adaug genurile cărții din lista de genuri
            return View(carte); // afișez datele cărții
        }



        [HttpPost] // HttpPost postează pe pagina web cartea pe care o adaug în baza de date
        public ActionResult New(Carti carte_request) // cu această acțiune adaug date în cartea pe care am creat-o în funcția anterioară
        {
            // carte_request este un obiect de tip Carti (basically o altă carte) care va primi toate input-urile de la interfața grafică (site-ul)

            carte_request.Coperti_List = Get_tip_coperta(); // funcție cu care adaug în lista de coperți a cărții noi toate tipurile de coperți din baza de date 
            carte_request.Edituri_List = Get_edituri(); // funcție cu care adaug în lista de edituri a cărții noi toate editurile din baza de date

            // memorez într-o listă doar genurile care au fost selectate
            var genuri_selectate = carte_request.Genuri_List.Where(b => b.Checked).ToList();
            try
            {
                if (ModelState.IsValid) // daca baza de date este stabilă, în special modelul Carti (datele din model sunt ok)
                {
                    carte_request.Genuri = new List<Gen>(); // creez o nouă listă de genuri
                    for (int i = 0; i < genuri_selectate.Count(); i++) // pentru fiecare gen selectat din interfața grafică
                    {
                        // cărții pe care vrem să o adăugam în baza de date îi asignăm genurile selectate 
                        Gen gen = db.Genuri.Find(genuri_selectate[i].Id);
                        carte_request.Genuri.Add(gen);
                    }
                    db.Carti.Add(carte_request); // adaug cartea la baza de date
                    db.SaveChanges(); // salvez modificările făcute la baza de date
                    return RedirectToAction("Index"); // după ce se termină procesul de adăugare la baza de date, ne întoarcem la Index (adică pe pagina cu lista cu cărți)
                }
                return HttpNotFound("Modelul nu este valid!"); // dacă modelul (clasa Carti) nu avea datele în regulă, atunci afișez eroare
            }
            catch (Exception e) // dacă prindem excepții afișăm eroare
            {
                var msg = e.Message;
                return View(carte_request);
            }
        }



        [HttpDelete]        // HttpDelete șterge din aplicație (site) datele din funcția ce îi corespunde
        public ActionResult Delete(int id) // acțiune prin care administratorul poate să șteargă o carte din aplicație (inclusiv din baza de date)
        {
            Carti carte = db.Carti.Find(id); // găsim cartea din baza de date pe baza id-ul pe care îl primește funcția
            if (carte != null)      // dacă cartea nu e goală
            {
                if(carte.Categorie == "non-literara")
                {
                    db.Carti.Remove(carte);     // șterg cartea din baza de date
                    db.SaveChanges();           // salvez modifcările 
                    return RedirectToAction("Index2"); // mă întorc pe pagina cu cărțile nonliterare
                }  
                else if(carte.Categorie == "literara")
                {
                    db.Carti.Remove(carte);     // șterg cartea din baza de date
                    db.SaveChanges();           // salvez modifcările 
                    return RedirectToAction("Index"); // mă întorc pe pagina cu cărțile literare
                }
            }
            return HttpNotFound("Nu s-a putut găsi cartea cu id-ul: " + id.ToString() + "!");
        }

        /*[NonAction] // NonAction reprezintă faptul că funcția următoare nu face modificări la baza de date
        public List<CheckBoxViewModel> Get_genuri() // funcție prin care returnez o listă de tip CheckBox cu toate genurile distincte din baza de date
        {
            var checkboxlist = new List<CheckBoxViewModel>(); // creez variabila de tip CheckBox în care voi pune genurile distincte
            List<string> genuri = new List<string>(); // creez o listă de string în care pun toate numele de genuri din baza de date
            foreach (var gen in db.Genuri.ToList()) // iterez în genurile din baza de date
            {
                genuri.Add(gen.Nume); // adaug toate numele de genuri din baza de date în lista de stringuri
            }
            
            // deoarece atunci când adaug o carte în baza de date direct din Seed() (din IdentityModels), numele genurilor se pot repeta în lista de genuri
            // în continuare voi tria acea listă astfel încât să conțină doar genuri distincte
            List<string> genuri_unice = new List<string>(); // creez o listă nouă de stringuri în care voi pune numele de genuri distincte
            genuri_unice = genuri.Distinct().ToList(); //folosind funcția Distinct().ToList() pot să elimin toate repetările din lista de genuri
            int id_gen = 350;
            for(int i = 0; i < genuri_unice.Count(); i++)
            {
                // adaug în lista de checkbox-uri toate genurile distincte, unul câte unul
                checkboxlist.Add(new CheckBoxViewModel
                {
                    Id = id_gen + i,
                    Nume = genuri_unice[i],
                    Checked = false // starea inițială a fiecărui gen este de nebifat (în interfața grafică)
                });
            }
            id_gen += 10;

            return checkboxlist; // returnez lista de checkbox-uri
        }*/

        [NonAction]
        public List<CheckBoxViewModel> Get_genuri()
        {
            var checkboxList = new List<CheckBoxViewModel>();
            foreach (var genre in db.Genuri.ToList())
            {
                checkboxList.Add(new CheckBoxViewModel
                {
                    Id = genre.Gen_id,
                    Nume = genre.Nume,
                    Checked = false
                });
            }
            return checkboxList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> Get_tip_coperta() // funcție care returnează o listă cu toate tipurile de coperte din baza de date
        {
            var selectList = new List<SelectListItem>(); // creez o variabilă în care voi pune lista
            foreach (var coperta in db.Coperti.ToList()) // pentru fiecare copertă din baza de date
            {
                // o adaug în lista de coperți
                selectList.Add(new SelectListItem 
                {
                    Value = coperta.Tip_carte_id.ToString(),
                    Text = coperta.Nume
                });
            }
            return selectList; // returnez lista de coperți
        }

        [NonAction]
        public IEnumerable<SelectListItem> Get_edituri() // funcție care returnează o listă cu toate editurile din baza de date
        {
            var selectList = new List<SelectListItem>(); // creez o variabilă în care voi pune lista
            foreach (var coperta in db.Edituri.ToList()) // pentru fiecare editură din baza de date
            {
                // o adaug în lista de edituri
                selectList.Add(new SelectListItem
                {
                    Value = coperta.Editura_id.ToString(),
                    Text = coperta.Nume
                });
            }
            return selectList; // returnez lista de edituri
        }
    }
}