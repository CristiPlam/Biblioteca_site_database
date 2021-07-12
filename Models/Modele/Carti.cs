using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Atestat2._0.Models.Modele
{
    //Tabelul pentru Cărți. O carte are un id (număr de identificare; generat automat de baza de date), titlu, autor, numar de pagini,
    //descriere, un link pentru poza cu coperta cărții, o categorie (literară/nonliterară) și un preț
    public class Carti
    {
        [Key] //cheia primară a tabelului
        public int CarteId { get; set; } // get și set sunt constructori de inițializare pentru fiecare variabilă din clasă

        [MinLength(2, ErrorMessage = "Titlul nu poate să aibă mai puțin de 2 caractere"), // validări pentru titlul cărții (dacă nu se îndeplinesc condițiile, atunci baza de date nu este
           MaxLength(100, ErrorMessage = "Titlul nu poate să aibă mai mult de 100 de caractere")]//validă, iar aplicația dă crash (se oprește)
        public string Titlu { get; set; }

        [MinLength(2, ErrorMessage = "Numele autorului nu poate să aibă mai puțin de 2 caractere"), // validări pentru autorul cărții
           MaxLength(100, ErrorMessage = "Numele autorului nu poate să aibă mai mult de 100 de caractere")]
        public string Autor { get; set; }

        [MinLength(2, ErrorMessage = "Descriereaa cărții nu poate să fie mai scurtă de 2 caractere"), // validări pentru descrierea cărții
           MaxLength(3000, ErrorMessage = "Descrierea cărții nu poate să fie mai lungă de 3000 de caractere")]
        public string Descriere { get; set; }

        public int Nr_pagini { get; set; }

        public string image_link { get; set; } //string pentru link-ul către o poză cu coperta cărții

        public string Categorie { get; set; } //literară sau nonliterară

        [RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Prețul cărții nu poate fii 0 sau mai mic decaât 0!")]
        // expresie regulată (un fel de șablon pentru cuvinte; în cazul de față este un șablon care vede dacă sunt indroduse doar cifre în variabila Pret + numărul final este pozitiv) 
        // care verifică dacă prețul e un număr pozitiv
        public float Pret { get; set; }



        //relații many-to-one cu tabelul Tip_coperta și tabelul Editura
        [ForeignKey("Tip_carte")] // cheia străină cu care se face legătura la tabelul Tip_copertă este Tip_carte_id
        public int Tip_carte_id { get; set; }
        public virtual Tip_coperta Tip_carte { get; set; } // creăm un obiect virtual de tipul clasei Tip_coperta (conține toate variabilele din clasa Tip_coperta)

        [ForeignKey("Editura")] // cheia străină cu care se face legătură la tabelul Editura este Editura_id
        public int Editura_id { get; set; }
        public virtual Editura Editura { get; set; } // creez un obiect virtual de tipul clasei Editura (conține toate variabilele din clasa Editura)


        //Relația many-to-many cu tabelul Gen
        public virtual ICollection<Gen> Genuri { get; set; } // creez o colecție de date de tipul Gen și le stochez în obiectul Genuri

        //Liste dropdown
        // creez două liste enumerabile pentru a reține tipurile de coperți din baza de date și toate editurile din baza de date
        public IEnumerable<SelectListItem> Coperti_List { get; set; }
        public IEnumerable<SelectListItem> Edituri_List { get; set; }

        //lista de tip checkbox
        [NotMapped] // NotMapped înseamnă că nu se va crea o coloană pentru Genuri_List în tabelul Carti
        // creez o listă cu toate genurile de cărți din baza de date. Lista asta e folosită în partea de adăugare a unei cărți în baza de date direct din aplicație (site). 
        // Lista este de tipul CheckBoxViewModel, care este o clasă separată care pentru fiecare gen reține un id, numele genului respectiv și îi o variabilă
        // booleană (true sau false). Variabila booleană devine checkbox-ul din interfața grafică a aplicației. Dacă în interfață este activat checkbox-ul, atunci
        // cartea respectivă va primi genul selectat prin acel checkbox
        public List<CheckBoxViewModel> Genuri_List { get; set; }
    }
}